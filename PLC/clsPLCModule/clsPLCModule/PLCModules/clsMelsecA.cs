using System;
using System.Text;
using System.Threading;
using System.Collections;
using System.Data;
using System.Runtime.InteropServices;



namespace PLCModule.PLCModules
{

	class clsMelsecA : clsPLCModuleInterface, IDisposable
	{
		/// <summary>
		/// Read 명령 Layout
		/// </summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		private struct readCommand
		{
			[MarshalAs(UnmanagedType.I1, SizeConst = 1)]
			public byte Command;
			[MarshalAs(UnmanagedType.I1, SizeConst = 1)]
			public byte No;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public byte[] Timer;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public byte[] Address;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public byte[] Type;
			[MarshalAs(UnmanagedType.I1, SizeConst = 1)]
			public byte Len;
			[MarshalAs(UnmanagedType.I1, SizeConst = 1)]
			public byte x00;
		}

		/// <summary>
		/// Write 명령 Layout
		/// </summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		private struct writeCommand
		{
			[MarshalAs(UnmanagedType.I1, SizeConst = 1)]
			public byte Command;
			[MarshalAs(UnmanagedType.I1, SizeConst = 1)]
			public byte No;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public byte[] Timer;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public byte[] Address;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public byte[] Type;
			[MarshalAs(UnmanagedType.I1, SizeConst = 1)]
			public byte Len;
			[MarshalAs(UnmanagedType.I1, SizeConst = 1)]
			public byte x00;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public byte[] Value;
		}



		private byte[] yReceived;
		readCommand ReadCommand;
		writeCommand WriteCommand;



		private bool isReceived = false;



		#region Tcp/IP 통신 관련
		private PLCSocket Comm = new PLCSocket();


		public clsMelsecA(string strIPAddress, int intPort, string _strDeviceType)
		{
			
			Comm.Received += new PLCSocket.Receive(this.Received);

			Comm.strServerIP = strIPAddress;
			Comm.iPort = intPort;

			this.strDeviceType = _strDeviceType;

			delStart = new delScanThread(Start_PLCScan);

			//Command 초기화
			this.ReadCommand.Command = 0x01;
			this.ReadCommand.No = 0xff;
			this.ReadCommand.Timer = new byte[] { 0x28, 0x00 };
			this.ReadCommand.Len = 0x01;
			this.ReadCommand.x00 = 0x00;

			this.WriteCommand.Command = 0x03;
			this.WriteCommand.No = 0xff;
			this.WriteCommand.Timer = new byte[] { 0x28, 0x00 };
			this.WriteCommand.Len = 0x01;
			this.WriteCommand.x00 = 0xff;
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

					strMsg = "Open 성공" + strMsg;
					return true;
				}

				strMsg = "Open 실패" + strMsg;
				return false;

			}
			catch (Exception ex)
			{
				strMsg = "Open 실패 :" + ex.Message + strMsg + "\r\n" + ex.ToString();
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

				//명시적으로 닫을때 재시도를 하지 안는다.
				_isClose = true;

				Start_PLCScan(false);
				StopScan();
				//RetryOpen(false);

				//닫혀 있으면...
				if (!this.isConnected) return true;

				if (close())
				{					
					strMsg = "Sucess closing connection";
					return true;
				}
				else
				{
					strMsg = "Fail closing connection";
					return false;
				}
			}
			catch (Exception ex)
			{
				strMsg = "Fail closing connection : " + ex.Message + "\r\n" + ex.ToString();
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
		/// 메모리 Device 종류를 설정 한다. : 'R'(기본), 'D', 'W'
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

		private int intScanRate = 1000;
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
		/// 등록 되어 있는 어드레스들을 주기적으로 값을 확인 하는 쓰레드를 시작한다.
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
		/// 현재 설정되어 있는 디바이스 종류에 따라 코드를 넘겨 준다.
		/// </summary>
		/// <returns></returns>
		private byte[] bytDeviceType()
		{
			byte[] byt = new byte[] { 0x20, 0x52 };
			switch (this.strDeviceType)
			{
				case "D":
					byt = new byte[] { 0x20, 0x44 };
					break;
				case "W":
					byt = new byte[] { 0x20, 0x57 };
					break;
				default:
					byt = new byte[] { 0x20, 0x52 };
					break;
			}

			return byt;
		}


		/// <summary>
		/// 등록 되어 있는 어드레스에 값을 확인..
		/// </summary>
		private void threadScanPLC()
		{
			int intErrCount = 0;

			string strMsg = string.Empty;

			while (true)
			{
				//ByteSetValue(bytSend, 17 + ( j * 4), 3, (int)dtAddress.Rows[j]["Address"]);
				//bytDeviceType()
				try
				{

					if (_isTryOpen)
					{
						Thread.Sleep(this.intScanRate);
						continue;
					}



					if (this.dtWriteOrder.Rows.Count > 0)
						SendWriteOrder();

					byte[] bytAddress = new byte[4];
					byte[] bytDevice = bytDeviceType();
					byte[] buffer = new byte[Marshal.SizeOf(ReadCommand)];

										
					DataRow dr;
					

					for (int i = 0; i < dtAddress.Rows.Count;i++ )
					{
						dr = dtAddress.Rows[i];

						ByteSetValue(bytAddress, 0, 4, int.Parse(dr["Address"].ToString()));

						ReadCommand.Address = bytAddress;
						ReadCommand.Type = bytDevice;

						unsafe
						{
							fixed (byte* fixed_buffer = buffer)
							{
								Marshal.StructureToPtr(ReadCommand, (IntPtr)fixed_buffer, false);
							}
						}

						this.isReceived = false;

						Comm.Send(buffer, ref strMsg);
						WaitRecieved();

						if (yReceived.Length != 4)
							throw new Exception(string.Format("ReadOrder : Ack 길이가 정상이 아닙니다. [받은 길이 : {0} / 정상 길이 : {1} / 읽은 Point {2}]",
								yReceived.Length, 4, 1));

						ChangeddAddressValue(dr, Convert.ToInt32(ByteToInt(new byte[] { yReceived[2], yReceived[3] })), true, buffer, yReceived);

						//dr["Value"] = ByteToInt(new byte [] { yReceived[2], yReceived[3]});
						
						Thread.Sleep(300);
					}
					

					intErrCount = 0;

					if (this.evtPLCScan != null) ThevtPLCScan();

					if (this.dtWriteOrder.Rows.Count > 0)
						SendWriteOrder();
					else
					{
						if (dtAddress.Rows.Count <= 2)	Thread.Sleep(this.intScanRate);
					}

				}
				catch (Exception ex)
				{

					intErrCount++;


					strMsg = "주소 값 검색 오류 [ErrorCount:" + intErrCount.ToString() + "] : " + ex.Message + "\r\n" + ex.ToString();
					PLCModule.clsPLCModule.LogWrite("threadScanPLC", strMsg);

					if (intErrCount > 2)
					{

						strMsg = "주소 값 검색 오류 [ErrorCount:" + intErrCount.ToString() + "]로 인한 연결을 재설정 합니다.";
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
			string strMsg = string.Empty;

			while (true)
			{
				try
				{

					byte[] bytAddress = new byte[4];
					byte[] bytValue = new byte[2];
					byte[] bytDevice = bytDeviceType();
					byte[] buffer = new byte[Marshal.SizeOf(WriteCommand)];

					DataRow dr;
										
					for (int i = 0; i < dtWriteOrder.Rows.Count; i++ )
					{
						dr = dtWriteOrder.Rows[i];

						ByteSetValue(bytAddress, 0, 4, int.Parse(dr["Address"].ToString()));
						ByteSetValue(bytValue, 0, 2, int.Parse((dr["Value"].ToString())));

						WriteCommand.Address = bytAddress;
						WriteCommand.Type = bytDevice;
						WriteCommand.Value = bytValue;


						unsafe
						{
							fixed (byte* fixed_buffer = buffer)
							{
								Marshal.StructureToPtr(WriteCommand, (IntPtr)fixed_buffer, false);
							}
						}

						this.isReceived = false;
						Comm.Send(buffer, ref strMsg);
						WaitRecieved();

						if (yReceived.Length != 2)
							throw new Exception(string.Format("WriteOrder : Ack 길이가 정상이 아닙니다. [받은 길이 : {0} / 정상 길이 : {1} / 읽은 Point {2}]",
								yReceived.Length, 4, 1));


						string strChanged = string.Format("[{0}] WorteValue : {1}", dr["Address"], dr["Value"]);
						WroteAddressValue(strChanged, buffer, yReceived);

						Thread.Sleep(500);
					}

					dtWriteOrder.Rows.Clear();

					return;

				}
				catch (Exception ex)
				{
					intErrCount++;

					strMsg = "주소 값 쓰기 오류 [ErrorCount:" + intErrCount.ToString() + "] : " + ex.Message + "\r\n" + ex.ToString();
					PLCModule.clsPLCModule.LogWrite("SendWriteOrder", strMsg);

					if (intErrCount > 4)
					{
						strMsg = "주소 값 쓰기 오류 [ErrorCount:" + intErrCount.ToString() + "]로 인한 연결을 재설정 합니다.";
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





		#region IDisposable 멤버

		public void Dispose()
		{
			this.Close();

		}

		#endregion

	}

}