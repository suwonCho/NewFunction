using System;
using System.Data;
using System.Threading;



namespace PLCModule
{

	/// <summary>
	/// clsTagPublishPLC�� ���� ��� �����Դϴ�.
	/// </summary>
	abstract class clsPLCModuleInterface
	{
		public clsPLCModuleInterface()
		{
			//
			// TODO: ���⿡ ������ ���� �߰��մϴ�.
			//
		}

		/// <summary>
		/// PLC �� ��ĵ ������..
		/// </summary>
		protected Thread thPLCScan;

		/// <summary>
		/// PLC ���� ������
		/// </summary>
		protected Thread thOpen;
		/// <summary>
		/// plc �� write order ���̺�
		/// </summary>
		internal DataTable dtWriteOrder = new DataTable("WirteOrder");


		
		/// <summary>
		/// plc�� �� ��ȸ�� ���� �߻� �ϴ� �̺�Ʈ
		/// </summary>
		public PLCModule.clsPLCModule.delPLCScan evtPLCScan = null;


		/// <summary>
		/// plc socket ��� ���� ����� ���� �߻� �ϴ� �̺�Ʈ..(�����)
		/// </summary>
		internal PLCModule.clsPLCModule.delChConnectionStatus evtChConnectionStatus = null;

		/// <summary>
		/// plc socket ��� ���� ����� ���� �߻� �ϴ� �̺�Ʈ
		/// </summary>
		internal event PLCModule.clsPLCModule.delChConnectionStatus _onChConnectionStatus;


		/// <summary>
		/// ���� ��õ� ������..
		/// </summary>
		protected bool _isTryOpen = false;

		/// <summary>
		/// ��������� �ܺο��� ������ ���� ���
		/// </summary>
		protected bool _isClose = true;

		protected void RetryOpen(bool isStart)
		{
			if (thOpen != null && thOpen.IsAlive)
			{
				thOpen.Abort();
				thOpen.Join();
			}
			thOpen = null;

			if (isStart)
			{
				thOpen = new Thread(new ThreadStart(thRetryOpen));
				thOpen.IsBackground = true;
				thOpen.Name = "Retry Open";
				thOpen.Start();
			}

		}

		protected void thRetryOpen()
		{
			_isTryOpen = true;
			
			try
			{
				close();
			}
			catch { }

			while (true)
			{
				try
				{
					if (_isClose) return;

					Thread.Sleep(2000);

					if (isConnected) return;

					if (open())
					{
						_isTryOpen = false;
						return;
					}
				}
				catch
				{
					Thread.Sleep(5000);
				}
			}
		}


		/// <summary>
		/// ������� ����� �Ͼ� ó��..
		/// </summary>
		/// <param name="bolConnectionStatus"></param>
		protected void ChConnection_Status(bool bolConnectionStatus)
		{
			isconnected = bolConnectionStatus;

			try		//������ ������ ���,.. ���� ����ش�.
			{
				if (!bolConnectionStatus)
				{
					foreach (DataRow dr in dtAddress.Rows)
					{
						dr["Value"] = System.DBNull.Value;
						dr["Value(Hex)"] = System.DBNull.Value;
					}
				}
				else
				{
					//������ �ݴ��� ���� ����
					_isClose = false;
				}
			}
			catch { }

			if (evtChConnectionStatus != null) evtChConnectionStatus(bolConnectionStatus);
			if (_onChConnectionStatus != null) _onChConnectionStatus(bolConnectionStatus);
			
		}

		public DataTable dtAddress = new DataTable("PLCAddress");



		/// <summary>
		/// ������ �õ��Ѵ�.
		/// </summary>
		/// <returns></returns>
		public abstract bool Open();

		/// <summary>
		/// ������ �õ��Ѵ�. �����峪 �ٸ��� ���� ���� �ʰ� ���� ���Ḹ �õ�
		/// </summary>
		/// <returns></returns>
		protected abstract bool open();

		/// <summary>
		/// ������ ���´�.
		/// </summary>
		/// <returns>��������</returns>
		public abstract bool Close();


		/// <summary>
		/// ������ ���´�. �����峪 �ٸ��� ���� ���� �ʰ� ���� ���Ḹ ����
		/// </summary>
		/// <returns>��������</returns>
		protected abstract bool close();


		private bool isconnected = false;
		/// <summary>
		/// PLC ���� ���� Ȯ��.
		/// </summary>
		public bool isConnected { get { return isconnected; } }

		/// <summary>
		/// PLC�� ����� PLC ADDRESS�� �߰��Ѵ�.
		/// </summary>
		/// <param name="Address">�߰��� �ּ�</param>	
		/// <returns></returns>
		public bool AddAddress(string Address)
		{
			try
			{
				DataRow row = dtAddress.NewRow();

				row["Address"] = Address;

				if (this.GetType() == Type.GetType("PLCModule.PLCModules.clsTEST") && isConnected)
					row["Value"] = 0;
				else
					row["Value"] = DBNull.Value;

				dtAddress.Rows.Add(row);

				return true;

			}
			catch (Exception ex)
			{
				return false;
				//throw ex;
			}
		}



		/// <summary>
		/// PLC�� ����� PLC ADDRESS�� �߰��Ѵ�.
		/// </summary>
		/// <param name="Address"></param>
		/// <returns></returns>
		public bool AddAddress(string[] Address)
		{
			try
			{
				foreach (string strAdd in Address)
				{
					AddAddress(strAdd);
				}

				return true;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		/// <summary>
		/// ��ϵ� Address�� �����Ѵ�.
		/// </summary>
		/// <param name="Address"></param>
		/// <returns></returns>
		public bool DelAddress(string Address)
		{
			try
			{
				DataRow[] rows = dtAddress.Select("Address = " + Address);

				foreach (DataRow row in rows)
				{
					dtAddress.Rows.Remove(row);
				}
				return true;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// ��ϵ� Address�� �����Ѵ�.
		/// </summary>
		/// <param name="Address"></param>
		/// <returns></returns>
		public bool DelAddress(string[] Address)
		{
			try
			{
				foreach (string strAdd in Address)
				{
					DelAddress(strAdd);
				}
				return true;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}





		public int GetValue(string Address)
		{
			try
			{
				if (!this.isConnected) throw new Exception("PLC�� ������ ���� �����ϴ�.");

				DataRow[] row = this.dtAddress.Select("Address = '" + Address + "'");

				if (row.Length > 0)
					return Convert.ToInt16(row[0]["Value"].ToString(), 10);
				else
					throw new Exception("Ȯ��(���) �Ǿ� ���� PLC Address�Դϴ�.[" + Address + "]");
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public int GetValue(string[] strAddress)
		{
			try
			{
				string strValue = string.Empty;
				foreach (string strAdd in strAddress)
				{
					strValue += IntToHex(GetValue(strAdd));
				}

				return int.Parse(strValue);
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public string GetValue(string[] strAddress, int intRetrunLength)
		{
			try
			{
				if (!this.isConnected) throw new Exception("PLC�� ������ ���� �����ϴ�.");
				return string.Format("{0:D" + intRetrunLength.ToString() + "}", GetValue(strAddress));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		public int GetValueInt(string strAddress)
		{
			try
			{
				return GetValue(strAddress);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public string GetValueHex(string strAddress)
		{

			try
			{
				if (!this.isConnected) throw new Exception("PLC�� ������ ���� �����ϴ�.");

				DataRow[] row = this.dtAddress.Select("Address = '" + strAddress + "'");

				if (row.Length > 0)
					return Function.Fnc.obj2String(row[0]["Value(HEX)"]);
				else
					throw new Exception("Ȯ��(���) �Ǿ� ���� PLC Address�Դϴ�.[" + strAddress + "]");
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		/// <summary>
		/// �ּ� ���� ���� ���ش�.
		/// </summary>
		/// <param name="Address"></param>
		/// <param name="Value"></param>
		/// <returns></returns>
		public bool WriteOrder(string Address, int intValue)
		{
			try
			{
				lock (this)
				{
					if (!this.isConnected) throw new Exception("PLC�� ������ ���� �����ϴ�.");

					if (this.GetType() == Type.GetType("PLCModule.PLCModules.clsTEST"))
					{   //test�� write������ �ٷ� ������..
						DataRow[] d = dtAddress.Select(string.Format("Address = '{0}'", Address));

						if (d.Length > 0)
							return ChangeddAddressValue(d[0], intValue, false, null, null);
						else
							return false;
					}

					
					DataRow row = dtWriteOrder.NewRow();

					row["Address"] = Address;
					row["Value"] = intValue;

					dtWriteOrder.Rows.Add(row);

					return true;
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public bool WriteOrder(string[] strAddress, int[] intValue)
		{
			try
			{
				int i = 0;

				foreach (string strAdd in strAddress)
				{
					WriteOrder(strAdd, intValue[i]);
					i++;
				}


				return true;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>
		/// �ּ� ���� ���� ���ش�.(TestPLC ����)
		/// </summary>
		/// <param name="Address"></param>
		/// <param name="intValue"></param>
		/// <returns></returns>
		public bool WriteOrder(string Address, string sValue)
		{
			lock (this)
			{
				if (!this.isConnected) throw new Exception("PLC�� ������ ���� �����ϴ�.");

				if (this.GetType() != Type.GetType("PLCModule.PLCModules.clsTEST")) return false;

				
				DataRow[] d = dtAddress.Select(string.Format("Address = '{0}'", Address));

				if (d.Length > 0)
				{
					d[0]["Value(HEX)"] = sValue;
					return true;
				}
				else
					return false;
				
				
			}
		}



		#region ���� function

		protected string IntToHex(int intValue)
		{
			try
			{
				string strTemp = string.Format("{0:X4}", intValue);
				strTemp = strTemp.Substring(strTemp.Length - 4, 4);
				return strTemp;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		protected long HexToInt(string strValue)
		{
			return Convert.ToInt16(strValue, 16);
		}

		protected void ByteSetValue(byte[] bytByt, int intStartLength, int intLength, int intValue)
		{
			byte[] bytSet = IntToByte(intValue, intLength);

			for (int i = intStartLength; i < intStartLength + intLength; i++)
			{
				bytByt[i] = bytSet[i - intStartLength];
			}

		}

		protected byte[] IntToByte(int intValue, int intLength)
		{
			int IntLength = intLength * 2;
			byte[] bytValue = new byte[intLength];
			string strValue = intValue.ToString("X" + IntLength.ToString());

			for (int i = 0; i < IntLength; i += 2)
			{
				bytValue[i / 2] = Convert.ToByte(strValue.Substring(strValue.Length - (i + 2), 2), 16);
			}

			return bytValue;

		}

		protected long ByteToInt(byte[] bytByt)
		{
			string strByte = this.byteToString(bytByt);
			string strValue = string.Empty;

			for (int i = 0; i < strByte.Length; i += 2)
			{
				strValue += strByte.Substring(strByte.Length - (i + 2), 2);
			}

			return Convert.ToInt16(strValue, 16);
		}


		protected string byteToString(byte[] yDATA)
		{
			int yLen = yDATA.Length;
			string strData = string.Empty;
			for (int i = 0; i < yLen; i++)
			{
				strData += yDATA[i].ToString("X2");
			}

			return strData;
		}

		protected bool AddressIsExist(string Address)
		{
			DataRow[] drs = dtAddress.Select("Address = '" + Address.ToString() + "'");

			if (drs.Length > 0) return true;

			return false;

		}

		/// <summary>
		/// Address �� ���� �� �Ͼ�� �̺�Ʈ ó��.
		/// </summary>
		protected void ThevtPLCScan()
		{
			Thread thevtPLCScan = new Thread(new ThreadStart(this.evtPLCScan));
			thevtPLCScan.IsBackground = true;
			thevtPLCScan.Name = "evtPLCScan";
			thevtPLCScan.Start();

		}



		/// <summary>
		/// ����Ʈ �迭�� string���� ���� ���·�
		/// </summary>
		/// <param name="b"></param>
		protected string ByteToString(byte[] bb)
		{
			string temp = "";
			foreach (byte b in bb)
			{
				temp = temp + string.Format("{0:X2}", Convert.ToInt32(b)) + " ";
			}

			return temp;
		}

		/// <summary>
		///  �� ��巹���� ���� ���� �Ǿ��� ���.. Log�� ����Ѵ�.
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="ReceiveValue"></param>
		/// <param name="isWirteByte"></param>
		/// <param name="SendByte"></param>
		/// <param name="ReciveByte"></param>
		/// <returns>���� ����</returns>
		protected bool ChangeddAddressValue(DataRow dr, int ReceiveValue, bool isWirteByte, byte[] SendByte, byte[] ReceiveByte)
		{
			

			try
			{

				lock (this)
				{
					string oldValue = dr["Value"] != DBNull.Value ? string.Format("{0:X4}", dr["Value"]) : "(null)";
					System.Int16 intReceiveValue = Convert.ToInt16(ReceiveValue);

					if (oldValue != intReceiveValue.ToString("X4"))
					{
						if (isWirteByte)
						{
							if (SendByte != null) PLCModule.clsPLCModule.LogWrite("ChangeddAddressValue", "[Sent] " + ByteToString(SendByte));
							if (ReceiveByte != null) PLCModule.clsPLCModule.LogWrite("ChangeddAddressValue", "[Received] " + ByteToString(ReceiveByte));
						}

						PLCModule.clsPLCModule.LogWrite("ChangeddAddressValue", string.Format("[{0}] {1} --> {2}", dr["Address"], oldValue, intReceiveValue.ToString("X4")));

						dr["Value"] = intReceiveValue;
						dr["Value(HEX)"] = string.Format("{0:X4}", intReceiveValue);

						return true;
					}

					return false;
				}
			}
			catch
			{
				throw;
			}
			
		}

		/// <summary>
		/// ��巹�� �� ���� �α׸� �����.
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="newValue"></param>
		/// <param name="SendByte"></param>
		/// <param name="ReceiveByte"></param>
		/// <returns></returns>
		protected bool WroteAddressValue(string strChanged, byte[] SendByte, byte[] ReceiveByte)
		{
			if (SendByte != null) PLCModule.clsPLCModule.LogWrite("WroteAddressValue", "[Sent] " + ByteToString(SendByte));
			if (ReceiveByte != null) PLCModule.clsPLCModule.LogWrite("WroteAddressValue", "[Received] " + ByteToString(ReceiveByte));
			PLCModule.clsPLCModule.LogWrite("WroteAddressValue", strChanged);

			return true;
		}

		#endregion

		#region PLC Scan Theread �˻� ó�� �κ�..
		/// <summary>
		/// Opc ������� üũ ������Ÿ�̸�
		/// </summary>
		Timer tmrCheckSacnThread;
		/// <summary>
		/// ��ĵ ���� Delegate
		/// </summary>
		protected delegate void delScanThread(bool isStart);
		/// <summary>
		/// ��ĵ ���� ���� Method
		/// </summary>
		protected delScanThread delStart = null;

		/// <summary>
		/// ��ĵ �˻� Timer ����..
		/// </summary>
		public void StartScan()
		{
			tmrCheckSacnThread = new Timer(new TimerCallback(tmrCheckScan), null, 0, 10000);
		}

		/// <summary>
		/// ��ĵ �˻� Timer ����..
		/// </summary>
		protected void StopScan()
		{
			if (tmrCheckSacnThread != null)
			{
				tmrCheckSacnThread.Dispose();
				tmrCheckSacnThread = null;
			}
		}

		/// <summary>
		/// ��ĵ ������ �˻�..
		/// </summary>
		/// <param name="obj"></param>
		private void tmrCheckScan(object obj)
		{
			//������ ���� ������ �˻� �� �ʿ� ����.
			if (!this.isConnected) return;

			//���� �۵���..
			if (thPLCScan != null && thPLCScan.IsAlive) return;

			//��� ��� ����.
			if (delStart == null) return;

			PLCModule.clsPLCModule.LogWrite("tmrCheckScan", "Address ��ĵ �����尡 ���� �Ǿ����ϴ�. �ٽ� ���� �մϴ�.");

			delStart(true);

		}


		protected int _ReOpen_cnt = 0;
		protected void _ReOpen()
		{
			try
			{
				Close();
			}
			catch
			{
			}
			Thread.Sleep(3000);
			try
			{
				Open();
			}
			catch
			{
			}
			_ReOpen_cnt = 0;
		}

		#endregion




	}
}
