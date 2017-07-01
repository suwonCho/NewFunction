using System;
using System.Data;
using System.Threading;



namespace PLCModule
{

	/// <summary>
	/// clsTagPublishPLC에 대한 요약 설명입니다.
	/// </summary>
	abstract class clsPLCModuleInterface
	{
		public clsPLCModuleInterface()
		{
			//
			// TODO: 여기에 생성자 논리를 추가합니다.
			//
		}

		/// <summary>
		/// PLC 값 스캔 쓰래드..
		/// </summary>
		protected Thread thPLCScan;

		/// <summary>
		/// PLC 연결 쓰래드
		/// </summary>
		protected Thread thOpen;
		/// <summary>
		/// plc 값 write order 테이블
		/// </summary>
		internal DataTable dtWriteOrder = new DataTable("WirteOrder");


		
		/// <summary>
		/// plc의 값 조회시 마다 발생 하는 이벤트
		/// </summary>
		public PLCModule.clsPLCModule.delPLCScan evtPLCScan = null;


		/// <summary>
		/// plc socket 통신 상태 변경시 마다 발생 하는 이벤트..(구방식)
		/// </summary>
		internal PLCModule.clsPLCModule.delChConnectionStatus evtChConnectionStatus = null;

		/// <summary>
		/// plc socket 통신 상태 변경시 마다 발생 하는 이벤트
		/// </summary>
		internal event PLCModule.clsPLCModule.delChConnectionStatus _onChConnectionStatus;


		/// <summary>
		/// 연결 재시도 중인지..
		/// </summary>
		protected bool _isTryOpen = false;

		/// <summary>
		/// 명시적으로 외부에서 연결을 끊을 경우
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
		/// 연결상태 변경시 일어 처리..
		/// </summary>
		/// <param name="bolConnectionStatus"></param>
		protected void ChConnection_Status(bool bolConnectionStatus)
		{
			isconnected = bolConnectionStatus;

			try		//연결이 닫혔을 경우,.. 값을 비워준다.
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
					//열릴때 닫는중 변수 변경
					_isClose = false;
				}
			}
			catch { }

			if (evtChConnectionStatus != null) evtChConnectionStatus(bolConnectionStatus);
			if (_onChConnectionStatus != null) _onChConnectionStatus(bolConnectionStatus);
			
		}

		public DataTable dtAddress = new DataTable("PLCAddress");



		/// <summary>
		/// 연결을 시도한다.
		/// </summary>
		/// <returns></returns>
		public abstract bool Open();

		/// <summary>
		/// 연결일 시도한다. 쓰레드나 다른것 연결 하지 않고 순수 연결만 시도
		/// </summary>
		/// <returns></returns>
		protected abstract bool open();

		/// <summary>
		/// 연결을 끊는다.
		/// </summary>
		/// <returns>성공여부</returns>
		public abstract bool Close();


		/// <summary>
		/// 연결일 끊는다. 쓰레드나 다른것 연결 하지 않고 순수 연결만 종료
		/// </summary>
		/// <returns>성공여부</returns>
		protected abstract bool close();


		private bool isconnected = false;
		/// <summary>
		/// PLC 접속 여부 확인.
		/// </summary>
		public bool isConnected { get { return isconnected; } }

		/// <summary>
		/// PLC와 통신할 PLC ADDRESS를 추가한다.
		/// </summary>
		/// <param name="Address">추가할 주소</param>	
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
		/// PLC와 통신할 PLC ADDRESS를 추가한다.
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
		/// 등록된 Address를 삭제한다.
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
		/// 등록된 Address를 삭제한다.
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
				if (!this.isConnected) throw new Exception("PLC와 연결이 끊어 졌습니다.");

				DataRow[] row = this.dtAddress.Select("Address = '" + Address + "'");

				if (row.Length > 0)
					return Convert.ToInt16(row[0]["Value"].ToString(), 10);
				else
					throw new Exception("확인(등록) 되어 있지 PLC Address입니다.[" + Address + "]");
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
				if (!this.isConnected) throw new Exception("PLC와 연결이 끊어 졌습니다.");
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
				if (!this.isConnected) throw new Exception("PLC와 연결이 끊어 졌습니다.");

				DataRow[] row = this.dtAddress.Select("Address = '" + strAddress + "'");

				if (row.Length > 0)
					return Function.Fnc.obj2String(row[0]["Value(HEX)"]);
				else
					throw new Exception("확인(등록) 되어 있지 PLC Address입니다.[" + strAddress + "]");
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		/// <summary>
		/// 주소 값에 값을 써준다.
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
					if (!this.isConnected) throw new Exception("PLC와 연결이 끊어 졌습니다.");

					if (this.GetType() == Type.GetType("PLCModule.PLCModules.clsTEST"))
					{   //test늘 write오더는 바로 값변경..
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
		/// 주소 값에 값을 써준다.(TestPLC 전용)
		/// </summary>
		/// <param name="Address"></param>
		/// <param name="intValue"></param>
		/// <returns></returns>
		public bool WriteOrder(string Address, string sValue)
		{
			lock (this)
			{
				if (!this.isConnected) throw new Exception("PLC와 연결이 끊어 졌습니다.");

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



		#region 공용 function

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
		/// Address 값 변경 시 일어나는 이벤트 처리.
		/// </summary>
		protected void ThevtPLCScan()
		{
			Thread thevtPLCScan = new Thread(new ThreadStart(this.evtPLCScan));
			thevtPLCScan.IsBackground = true;
			thevtPLCScan.Name = "evtPLCScan";
			thevtPLCScan.Start();

		}



		/// <summary>
		/// 바이트 배열을 string숫자 연결 형태로
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
		///  각 어드레스에 값이 변경 되었을 경우.. Log를 기록한다.
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="ReceiveValue"></param>
		/// <param name="isWirteByte"></param>
		/// <param name="SendByte"></param>
		/// <param name="ReciveByte"></param>
		/// <returns>변경 여부</returns>
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
		/// 어드레스 값 변경 로그를 남긴다.
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

		#region PLC Scan Theread 검사 처리 부분..
		/// <summary>
		/// Opc 연결상태 체크 쓰레드타이머
		/// </summary>
		Timer tmrCheckSacnThread;
		/// <summary>
		/// 스캔 시작 Delegate
		/// </summary>
		protected delegate void delScanThread(bool isStart);
		/// <summary>
		/// 스캔 시작 연결 Method
		/// </summary>
		protected delScanThread delStart = null;

		/// <summary>
		/// 스캔 검사 Timer 시작..
		/// </summary>
		public void StartScan()
		{
			tmrCheckSacnThread = new Timer(new TimerCallback(tmrCheckScan), null, 0, 10000);
		}

		/// <summary>
		/// 스캔 검사 Timer 정지..
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
		/// 스캔 쓰래드 검사..
		/// </summary>
		/// <param name="obj"></param>
		private void tmrCheckScan(object obj)
		{
			//연결이 끊겨 있으면 검사 할 필요 없음.
			if (!this.isConnected) return;

			//정상 작동중..
			if (thPLCScan != null && thPLCScan.IsAlive) return;

			//기능 사용 않함.
			if (delStart == null) return;

			PLCModule.clsPLCModule.LogWrite("tmrCheckScan", "Address 스캔 쓰래드가 중지 되었습니다. 다시 시작 합니다.");

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
