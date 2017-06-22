using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Data;



namespace PLCModule.PLCModules
{

	class clsMelsecQ : clsPLCModuleInterface, IDisposable
	{
		//plc�� ���� Read��� R1900-R1901���� �б�
		//����  :                      |                         ���                                  |  Read���  |����Ŀ�Ǵ� |���ε���̽�:R1900|�ڵ�|   ����   |   
		private byte[] yRead = { 0x50, 0x00, 0x00, 0xff, 0xff, 0x03, 0x00, 0x0c, 0x00, 0x28, 0x00, 0x01, 0x04, 0x00, 0x00, 0x6c, 0x07, 0x00, 0xaf, 0x02, 0x00 };

		private byte[] yRandomRead = { 0x50, 0x00, 0x00, 0xff, 0xff, 0x03, 0x00, 0x10, 0x00, 0x28, 0x00, 0x03, 0x04, 0x00, 0x00, 0x02, 0x00 };

		//plc�� ���� Write��� R1900-R1901���� ����
		//����  :                      |                         ���                                  |  Write���  |����Ŀ�Ǵ�|���ε���̽�:R1900|�ڵ�|   ����   |     ������1(3bytes)...
		private byte[] yWrite = { 0x50, 0x00, 0x00, 0xff, 0xff, 0x03, 0x00, 0x0c, 0x00, 0x28, 0x00, 0x01, 0x14, 0x00, 0x00, 0x6c, 0x07, 0x00, 0xaf, 0x02, 0x00 };

		private byte[] yRandomWrite = { 0x50, 0x00, 0x00, 0xff, 0xff, 0x03, 0x00, 0x10, 0x00, 0x28, 0x00, 0x02, 0x14, 0x00, 0x00, 0x00, 0x00 };

		private byte[] yReceived;

		private bool isReceived = false;


		#region Tcp/IP ��� ����
		private PLCSocket Comm = new PLCSocket();


		public clsMelsecQ(string strIPAddress, int intPort, string _strDeviceType)
		{
			
			Comm.Received += new PLCSocket.Receive(this.Received);

			Comm.strServerIP = strIPAddress;
			Comm.iPort = intPort;

			delStart = new delScanThread(Start_PLCScan);

			this.strDeviceType = _strDeviceType;
		}

	

		public override bool Open()
		{

			string strMsg = string.Format("(IP:{0} / Port : {1})", Comm.strServerIP, Comm.iPort);

			try
			{
				//Console.WriteLine("Retry Connect");

				if (open())
				{
					
					delStart(true);

					StartScan();

					strMsg = "Open ����" + strMsg;
					return true;
				}

				strMsg = "Open ����" + strMsg;
				return false;

			}
			catch (Exception ex)
			{
				strMsg = "Open ���� :" + ex.Message + strMsg + "\r\n" + ex.ToString();
				throw ex;
			}
			finally
			{
				PLCModule.clsPLCModule.LogWrite("Open", strMsg);
			}


		}

		protected override bool open()
		{
			if (Comm.client_Open())
			{
				this.ChConnection_Status(true);
				return true;
			}

			return false;
		}

		public override bool Close()
		{
			string strMsg = string.Empty;
			try
			{

				//��������� ������ ��õ��� ���� �ȴ´�.
				_isClose = true;

				Start_PLCScan(false);
				StopScan();

				//���� ������...
				if (!this.isConnected) return true;

				//RetryOpen(false);
				if (close())
				{
					this.ChConnection_Status(false);
					strMsg = "Sucess closing connection  ";
					return true;
				}
				else
				{
					strMsg = "Fail closing connection ";
					return false;
				}
			}
			catch (Exception ex)
			{
				strMsg = "Connection close fail : " + ex.Message + "\r\n" + ex.ToString();
				throw ex;
			}
			finally
			{
				PLCModule.clsPLCModule.LogWrite("Close", strMsg);
			}
		}

		protected override bool close()
		{
			try
			{
				return Comm.client_Close();
			}
			catch
			{
				throw;
			}
			finally { this.ChConnection_Status(false); }

		}

		//	public override bool isConnected
		//	{
		//		get
		//		{ return Comm.objClient.Connected; }
		//	}

		#endregion


		private int intTimeOut = 3000;

		private string strMsg = string.Empty;


		private string strdeviceType = "R";

		/// <summary>
		/// �޸� Device ������ ���� �Ѵ�. : 'R'(�⺻), 'D', 'W'
		/// </summary>
		public string strDeviceType
		{
			get
			{
				return strdeviceType;
			}
			set
			{
				string strValue = value.ToUpper();
				if (strValue == "D" || strValue == "W")
					strdeviceType = strValue;
				else
					strdeviceType = "R";
			}
		}

		/// <summary>
		/// TimeOut Time Set (ms)
		/// </summary>
		public int IntTimeOut
		{
			get
			{
				return intTimeOut;
			}
			set
			{
				intTimeOut = value;
			}
		}

		private int intScanRate = 500;
		/// <summary>
		/// ScanRate Time Set (ms)
		/// </summary>
		public int IntScanRate
		{
			get
			{
				return IntScanRate;
			}
			set
			{
				IntScanRate = value;
			}
		}



		/// <summary>
		/// ��� �Ǿ� �ִ� ��巹������ �ֱ������� ���� Ȯ�� �ϴ� �����带 �����Ѵ�.
		/// </summary>
		/// <param name="isStart"></param>
		private void Start_PLCScan(bool isStart)
		{
			if (this.thPLCScan != null && thPLCScan.IsAlive)
			{
				thPLCScan.Abort();
				thPLCScan.Join();
			}

			thPLCScan = null;

			if (isStart)
			{
				thPLCScan = new Thread(new ThreadStart(this.threadScanPLC));
				thPLCScan.Name = "PLC Address Scan";
				thPLCScan.IsBackground = true;
				thPLCScan.Start();
			}

		}


		/// <summary>
		/// ���� �����Ǿ� �ִ� ����̽� ������ ���� �ڵ带 �Ѱ� �ش�.
		/// </summary>
		/// <returns></returns>
		private byte bytDeviceType()
		{
			byte byt = 0xaf;
			switch (this.strDeviceType)
			{
				case "D":
					byt = 0xa8;
					break;
				case "W":
					byt = 0xB4;
					break;
				default:
					byt = 0xaf;
					break;
			}

			return byt;
		}

		/// <summary>
		/// ��� �Ǿ� �ִ� ��巹���� ���� Ȯ��..
		/// </summary>
		private void threadScanPLC()
		{
			int intErrCount = 0;
			string strMsg = string.Empty;
			bool isWriteByte = true;
			while (true)
			{
				try
				{

					if (_isTryOpen)
					{
						Thread.Sleep(this.intScanRate);
						continue;
					}

					if (this.dtWriteOrder.Rows.Count > 0)
						SendWriteOrder();

					int intRows = this.dtAddress.Rows.Count;

					for (int i = 0; i < intRows; i += 15)
					{
						int intScanCount = (i + 15 > intRows ? intRows - i : 15);
						byte[] bytSend = new byte[17 + (4 * intScanCount)];
						string[] strAddress = new string[intScanCount];
						this.yRandomRead.CopyTo(bytSend, 0);

						//���� ���� Set
						ByteSetValue(bytSend, 7, 2, 8 + (4 * intScanCount));
						//Point Count Set
						ByteSetValue(bytSend, 15, 1, intScanCount);

						for (int j = i; j < i + intScanCount; j++)
						{
							ByteSetValue(bytSend, 17 + (j * 4), 3, int.Parse(dtAddress.Rows[j]["Address"].ToString()));
							bytSend[20 + (j * 4)] = bytDeviceType();
							strAddress[j - i] = dtAddress.Rows[j]["Address"].ToString();
						}

						this.isReceived = false;
						Comm.Send(bytSend, ref strMsg);
						WaitRecieved();

						if (yReceived.Length != (11 + (2 * intScanCount)))
							throw new Exception(string.Format("Ack ���̰� ������ �ƴմϴ�. [���� ���� : {0} / ���� ���� : {1} / ���� Point {2}]",
											yReceived.Length, (11 + (2 * intScanCount)), intScanCount));

						isWriteByte = true;

						for (int k = 0; k < intScanCount; k++)
						{
							DataRow[] row = this.dtAddress.Select("Address = " + strAddress[k]);

							//row[0]["Value"] = ByteToInt(new byte [] { yReceived[11 + (k*2)], yReceived[12 + (k*2)]});
							if (row.Length > 0)
								isWriteByte = !ChangeddAddressValue(row[0], Convert.ToInt32(ByteToInt(new byte[] { yReceived[11 + (k * 2)], yReceived[12 + (k * 2)] })),
									isWriteByte, bytSend, yReceived);

						}
					}

					intErrCount = 0;

					if (this.evtPLCScan != null) ThevtPLCScan();

					if (this.dtWriteOrder.Rows.Count > 0)
						SendWriteOrder();
					else
						Thread.Sleep(this.intScanRate);
				}
				catch (Exception ex)
				{

					intErrCount++;
					strMsg = "�ּ� �� �˻� ���� [ErrorCount:" + intErrCount.ToString() + "] : " + ex.Message + "\r\n" + ex.ToString();
					PLCModule.clsPLCModule.LogWrite("threadScanPLC", strMsg);

					if (intErrCount > 1)
					{

						strMsg = "�ּ� �� �˻� ���� [ErrorCount:" + intErrCount.ToString() + "]�� ���� ������ �缳�� �մϴ�.";
						PLCModule.clsPLCModule.LogWrite("threadScanPLC", strMsg);

						RetryOpen(true);
						return;
					}

				}
			}

		}


		private void SendWriteOrder()
		{
			int intErrCount = 0;
			while (true)
			{
				try
				{
					int intRows = dtWriteOrder.Rows.Count;
					int intWritePointLength = 6;
					string strChanged = string.Empty;

					for (int i = 0; i < intRows; i += 10)
					{
						int intWriteCount = (i + 10 > intRows ? intRows - i : 10);
						byte[] bytSend = new byte[17 + (intWritePointLength * intWriteCount)];
						string[] strAddress = new string[intWriteCount];
						this.yRandomWrite.CopyTo(bytSend, 0);

						//���� ���� Set
						ByteSetValue(bytSend, 7, 2, 8 + (intWritePointLength * intWriteCount));
						//Point Count Set
						ByteSetValue(bytSend, 15, 1, intWriteCount);

						for (int j = i; j < i + intWriteCount; j++)
						{
							ByteSetValue(bytSend, 17 + (j * intWritePointLength), 3, (int)dtWriteOrder.Rows[j]["Address"]);
							bytSend[20 + (j * intWritePointLength)] = bytDeviceType();
							ByteSetValue(bytSend, 17 + 4 + (j * intWritePointLength), 2, Convert.ToInt16(dtWriteOrder.Rows[j]["Value"].ToString(), 10));
							try
							{
								strChanged += strChanged != string.Empty ? "\r\n" : string.Empty;
								strChanged += string.Format("[{0}] WorteValue : {1:x4}", dtWriteOrder.Rows[j]["Address"], dtWriteOrder.Rows[j]["Value"]);
							}
							catch (Exception ex)
							{
								strChanged = ex.Message;
							}

						}

						this.isReceived = false;
						Comm.Send(bytSend, ref strMsg);
						WaitRecieved();

						if (yReceived.Length != 11)
							throw new Exception("Ack�� ���� ���� ���Ͽ����ϴ�.. [" + this.byteToString(yReceived) + "]");

						WroteAddressValue(strChanged, bytSend, yReceived);

					}

					dtWriteOrder.Rows.Clear();

					return;

				}
				catch (Exception ex)
				{
					intErrCount++;

					strMsg = "�ּ� �� ���� ���� [ErrorCount:" + intErrCount.ToString() + "] : " + ex.Message + "\r\n" + ex.ToString();
					PLCModule.clsPLCModule.LogWrite("SendWriteOrder", strMsg);

					if (intErrCount > 4)
					{
						strMsg = "�ּ� �� ���� ���� [ErrorCount:" + intErrCount.ToString() + "]�� ���� ������ �缳�� �մϴ�.";
						PLCModule.clsPLCModule.LogWrite("SendWriteOrder", strMsg);

						dtWriteOrder.Clear();
						RetryOpen(true);
						return;
					}
				}
			}
		}


		private void Received(byte[] bytByte)
		{
			this.yReceived = new Byte[bytByte.Length];
			bytByte.CopyTo(yReceived, 0);
			this.isReceived = true;
		}

		private void WaitRecieved()
		{
			try
			{
				DateTime dtSendTime = DateTime.Now;

				TimeSpan dtSpan;
				do
				{
					Thread.Sleep(300);
					dtSpan = DateTime.Now - dtSendTime;
					if (isReceived) break;
				} while (dtSpan.TotalMilliseconds < this.IntTimeOut);

				if (isReceived)
				{
					return;
				}
				else
				{
					throw new Exception("PLC Time Out...");
				}

			}
			catch (Exception ex)
			{
				throw new Exception("PLC Time Out...", ex);
			}
		}





		#region IDisposable ���

		public void Dispose()
		{
			this.Close();

		}

		#endregion

	}
}