using System;
using System.Data;
using System.Threading;

using Function;
using System.Collections.Generic;

namespace PLCComm
{

	/// <summary>
	/// iPLCCommInterface 클래스(2017년 개발)
	/// </summary>
	abstract class iPLCCommInterface
	{
		/// <summary>
		/// WriteOrderTable 사용 여부
		/// </summary>
		bool isUseWriteOrderTable;

		/// <summary>
		/// iPLCCommInterface 클래스
		/// </summary>
		/// <param name="UseWriteOrderTable">WriteOrderTable 사용 여부 
		///		<para/>사용:WriteOrder를 내리면 테이블에 쓰기 명령을 쌓아주며 처리는 별도로 해야 한다.(소켓통신시)
		///		<para/>미사용:WriteOrder Override해서 별도 처리 안하면 실행 시 오류 발생
		///		</param>
		public iPLCCommInterface(bool UseWriteOrderTable)
		{
			dtAddress = new DataTable("PLC Address");		
						
			dtAddress.Columns.Add(new DataColumn("Address", System.Type.GetType("System.String")));
			dtAddress.Columns.Add(new DataColumn("PLCValueType", System.Type.GetType("PLCComm.enPLCValueType")));
			dtAddress.Columns.Add(new DataColumn("Value", System.Type.GetType("System.Object")));
			dtAddress.Columns.Add(new DataColumn("Value(INT)", System.Type.GetType("System.Int16")));
			dtAddress.Columns.Add(new DataColumn("Value(HEX)", System.Type.GetType("System.String")));
			dtAddress.Columns.Add(new DataColumn("Value(STRING)", System.Type.GetType("System.String")));
			dtAddress.PrimaryKey = new DataColumn[] { dtAddress.Columns["Address"] };
			
			dtWriteOrder.Columns.Add(new DataColumn("Address", System.Type.GetType("System.String")));
			dtWriteOrder.Columns.Add(new DataColumn("PLCValueType", System.Type.GetType("PLCComm.enPLCValueType")));
			dtWriteOrder.Columns.Add(new DataColumn("Value", System.Type.GetType("System.Object")));
		}



		/// <summary>
		/// Log기록 클래스
		/// </summary>
		internal static Function.Util.Log _log = null;
		
		/// <summary>
		/// 등록되 PLC 주소를 관리 하는 Datatable
		/// </summary>
		public DataTable dtAddress = new DataTable("PLCAddress");

		/// <summary>
		/// plc 값 write order 테이블
		/// </summary>
		internal DataTable dtWriteOrder = new DataTable("WirteOrder");

		/// <summary>
		/// PLC 값 스캔 쓰래드..
		/// </summary>
		protected Thread thPLCScan;

		/// <summary>
		/// PLC 연결 쓰래드
		/// </summary>
		protected Thread thOpen;
		
		
		/// <summary>
		/// plc의 값 조회시 마다 발생 하는 이벤트
		/// </summary>
		event delPLCScan _onPLCScan = null;

		/// <summary>
		/// plc의 값 조회시 마다 발생 하는 이벤트
		/// </summary>
		public event delPLCScan OnPLCScan
		{
			add { _onPLCScan += value; }
			remove { _onPLCScan -= value; }
		}

		private enStatus _conectionStatus = enStatus.None;

		
		/// <summary>
		/// PLC 연결상태를 가지고 오거나 설정 합니다.
		/// </summary>
		internal enStatus _ConnctionStatus
		{
			get { return _conectionStatus; }
			set
			{
				if (value == _conectionStatus) return;
				
				_conectionStatus = value;


				try     //연결이 닫혔을 경우,.. 값을 비워준다.
				{
					if (_conectionStatus != enStatus.OK)
					{
						foreach (DataRow dr in dtAddress.Rows)
						{
							dr["Value"] = System.DBNull.Value;
							dr["Value(INT)"] = System.DBNull.Value;
							dr["Value(Hex)"] = System.DBNull.Value;
							dr["Value(STRING)"] = System.DBNull.Value;
						}
					}
					else
					{
						//열릴때 닫는중 변수 변경
						_isClose = false;
					}
				}
				catch { }


				if (_onChConnectionStatus != null) _onChConnectionStatus(_conectionStatus);

			}
		}


		/// <summary>
		/// PLC 연결상태를 가지고 옴니다.
		/// </summary>
		public enStatus ConnctionStatus
		{
			get { return _conectionStatus; }		
		}


		/// <summary>
		/// plc socket 통신 상태 변경시 마다 발생 하는 이벤트
		/// </summary>
		event delChConnectionStatus _onChConnectionStatus;

		/// <summary>
		/// plc socket 통신 상태 변경시 마다 발생 하는 이벤트
		/// </summary>
		public event delChConnectionStatus OnChConnectionStatus
		{
			add { _onChConnectionStatus += value; }
			remove { _onChConnectionStatus -= value; }
		}
		
		Dictionary<string, List<delChAddressValue>> dicEvtAdd_ch = new Dictionary<string, List<delChAddressValue>>();

		/// <summary>
		/// 해당 주소의 값이 변경 될 경우 발생 할 이벤틀르 등록 한다.(실행 시 비동기 처리를 하므로 쓰레드 처리를 할것)
		/// </summary>
		/// <param name="address">감시할 주소</param>
		/// <param name="onChAddressValue">일어날 이벤트</param>
		public void ChangeEvtAddress_Add(string address, delChAddressValue onChAddressValue)
		{
			List<delChAddressValue> lst;

			if (dicEvtAdd_ch.ContainsKey(address))
			{
				lst = dicEvtAdd_ch[address];				
			}
			else
			{
				lst = new List<delChAddressValue>();
				dicEvtAdd_ch.Add(address, lst);
			}

			lst.Add(onChAddressValue);
		}
		

		
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

					if (_ConnctionStatus == enStatus.OK) return;

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

				if (this.GetType() == Type.GetType("PLCModule.PLCModules.clsTEST") && ConnctionStatus == enStatus.OK)
					row["Value"] = 0;
				else
					row["Value"] = DBNull.Value;

				dtAddress.Rows.Add(row);

				return true;

			}
			catch (Exception ex)
			{
				throw ex;
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




		/// <summary>
		/// 등록된 어드레스의 값을 가지고 온다.(등록 값 타입으로 가지고 온다.)
		/// </summary>
		/// <param name="Address"></param>
		/// <returns></returns>
		public AddressValue GetValue(string Address)
		{
			try
			{
				if (ConnctionStatus != enStatus.OK ) throw new Exception("PLC와 연결이 끊어 졌습니다.");

				DataRow[] row = this.dtAddress.Select("Address = '" + Address + "'");

				if (row.Length > 0)
				{
					AddressValue rtn = new AddressValue();

					rtn.Value = row[0]["Value"];
					rtn.ValueType = (enPLCValueType)row[0]["ValueType"];

					return rtn;
				}
				else
					throw new Exception("확인(등록) 되어 있지 PLC Address입니다.[" + Address + "]");
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>
		/// 등록된 어드레스의 값을 요청한 타입으로 가지고 온다.
		/// </summary>
		/// <param name="Address">요청 주소</param>
		/// <param name="valuetype">요청 값 타임</param>
		/// <returns></returns>
		public AddressValue GetValue(string Address, enPLCValueType valuetype)
		{
			try
			{
				if (ConnctionStatus != enStatus.OK) throw new Exception("PLC와 연결이 끊어 졌습니다.");

				DataRow[] row = this.dtAddress.Select("Address = '" + Address + "'");

				if (row.Length > 0)
				{
					AddressValue rtn = new AddressValue();

					switch(valuetype)
					{
						case enPLCValueType.HEX:
							rtn.Value = row[0]["Value(HEX)"];
							break;

						case enPLCValueType.STRING:
							rtn.Value = row[0]["Value(STRING)"];
							break;

						default:
							rtn.Value = row[0]["Value(INT)"];
							break;
					}
					
					rtn.ValueType = valuetype;

					return rtn;
				}
				else
					throw new Exception("확인(등록) 되어 있지 PLC Address입니다.[" + Address + "]");
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}


		/// <summary>
		/// 등록된 어드레스의 값을 Hex 형태로 가지고 온다.(요청한 어드레스 순서로 붙여서)
		/// </summary>
		/// <param name="strAddress"></param>
		/// <returns></returns>
		public string GetValueHex(string[] strAddress)
		{
			try
			{
				string strValue = string.Empty;
				foreach (string strAdd in strAddress)
				{
					strValue += GetValue(strAdd);
				}
				return strValue;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>
		/// 등록된 어드레스의 값을 Hex 형태로 가지고 온다.
		/// </summary>
		/// <param name="strAddress"></param>
		/// <returns></returns>
		public string GetValueHex(string strAddress)
		{

			try
			{
				string rtn = GetValue(strAddress, enPLCValueType.HEX).Value.ToString();

				return rtn;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		/// <summary>
		/// 등록된 어드레스의 값을 int 형태로 가지고 온다.
		/// </summary>
		/// <param name="strAddress"></param>
		/// <returns></returns>
		public int GetValueInt(string strAddress)
		{
			try
			{
				int rtn = (int)GetValue(strAddress, enPLCValueType.INT).Value;

				return rtn;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		/// <summary>
		/// 등록된 어드레스의 값을 string 형태로 가지고 온다.
		/// </summary>
		/// <param name="strAddress"></param>
		/// <returns></returns>
		public string GetValueString(string strAddress)
		{

			try
			{
				string rtn = GetValue(strAddress, enPLCValueType.STRING).Value.ToString();

				return rtn;
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
		public virtual bool WriteOrder(string Address, int intValue)
		{
			if (isUseWriteOrderTable) throw new Exception("WriterOrder를 Override 하여 주십시요.");

			try
			{
				lock (this)
				{
					if (ConnctionStatus != enStatus.OK ) throw new Exception("PLC와 연결이 끊어 졌습니다.");

					if (this.GetType() == Type.GetType("PLCModule.PLCModules.clsTEST"))
					{   //test늘 write오더는 바로 값변경..
						DataRow[] d = dtAddress.Select(string.Format("Address = '{0}'", Address));

						if (d.Length > 0)
						{
							ChangeddAddressValue(Address, intValue, false, null, null);
							return true;
						}
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

		public virtual bool WriteOrder(string[] strAddress, int[] intValue)
		{
			if (isUseWriteOrderTable) throw new Exception("WriterOrder를 Override 하여 주십시요.");

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
		public virtual bool WriteOrder(string Address, string sValue)
		{
			throw new Exception("WriterOrder를 Override 하여 주십시요.");

			lock (this)
			{
				if (ConnctionStatus != enStatus.OK) throw new Exception("PLC와 연결이 끊어 졌습니다.");

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



		/// <summary>
		/// 어드래스 등록 여부를 확인한다.
		/// </summary>
		/// <param name="Address"></param>
		/// <returns></returns>
		protected bool AddressIsExist(string Address)
		{
			DataRow[] drs = dtAddress.Select("Address = '" + Address.ToString() + "'");

			if (drs.Length > 0) return true;
			return false;
		}

		/// <summary>
		/// 등록된 어드레스의 Datarow를 가지고 온다
		/// </summary>
		/// <param name="Address"></param>
		/// <returns>null이면 미등록</returns>
		protected DataRow AddressGetRow(string Address)
		{
			DataRow[] drs = dtAddress.Select("Address = '" + Address.ToString() + "'");

			if (drs.Length > 0) return drs[0];
			return null;
		}

		/// <summary>
		/// 로그 기록을 합니다.
		/// </summary>
		/// <param name="strModule"></param>
		/// <param name="strMsg"></param>

		internal static void LogWrite(string strModule, string strMsg)
		{
			try
			{
				if (_log == null) return;

				string strMessage = string.Format("[{1}] {2}", strModule, strMsg);
				_log.WLog(strMessage);
			}
			catch
			{ }

		}


		/// <summary>
		/// 각 어드레스에 값이 변경 되었을 경우.. Log를 기록하고, 등록되 주소 값 변경 이벤트가 있으면 비동기 실행 하여준다.
		/// </summary>
		/// <param name="address"></param>
		/// <param name="ReceiveValue">변경된 값 byte배열로 넘기거나 3자 이상 문자열만 HEX로 보내고 나머지는 숫자(int)로 넘길것</param>
		/// <param name="isWirteByte"></param>
		/// <param name="SendByte"></param>
		/// <param name="ReceiveByte"></param>
		/// <returns></returns>
		protected void ChangeddAddressValue(string address, object ReceiveValue, bool isWirteByte, byte[] SendByte, byte[] ReceiveByte)
		{
			

			try
			{

				lock (this)
				{
					DataRow dr = AddressGetRow(address);

					if (dr == null)
					{
						LogWrite("ChangeddAddressValue", $"미등록된 주소 값변경 수신(미처리)[Address]{address} [ReceiveValue]{ReceiveValue}");
						return;
					}

					

					object oldValue = dr["Value"] != DBNull.Value ? dr["Value"] : "(null)";
					object newValue = null;
					enPLCValueType vType = (enPLCValueType)dr["PLCValueType"];

					enPLCValueType nType = enPLCValueType.INT;

					Int16 intReceiveValue = -1;
					bool isChage = false;
					string tmp;				

					byte[] bts = ReceiveValue as byte[];


					//신규값 처리 및 값 변경 여부 확인한다.
					if(bts != null)
					{	//바이트 배열 처리
						//2개 이하면 short 이므로
						if(bts.Length < 3)
						{   //int16 처리
							intReceiveValue = Int16.Parse(fnc.ByteToLong(bts).ToString());

							isChage = (Int16)dr["Value(INT)"] != intReceiveValue;
							newValue = intReceiveValue;
						}
						else
						{   //Hex처리
							nType = enPLCValueType.HEX;
							newValue = fnc.ByteToHex(bts);

							isChage = Fnc.obj2String(dr["Value(HEX)"]).Equals(newValue.ToString());
						}

					}
					else if (Int16.TryParse(ReceiveValue.ToString(), out intReceiveValue))
					{	//int16 처리
						isChage = (Int16)dr["Value(INT)"] != intReceiveValue;
						newValue = intReceiveValue;
					}
					else
					{	//hex 처리
						isChage = !Fnc.obj2String(dr["Value(HEX)"]).Equals(ReceiveValue.ToString());
						nType = enPLCValueType.HEX;
					}
					
					
					//변경이 되었으면 로그를 남기로 이벤트를 실행 한다.
					if (isChage)
					{
						if (isWirteByte)
						{
							if (SendByte != null) LogWrite("ChangeddAddressValue", "[Sent] " + fnc.ByteToHex(SendByte));
							if (ReceiveByte != null) LogWrite("ChangeddAddressValue", "[Received] " + fnc.ByteToHex(ReceiveByte));
						}

						LogWrite("ChangeddAddressValue", string.Format("[{0}] {1} --> {2}", dr["Address"], oldValue, intReceiveValue.ToString("X4")));

						if (nType == enPLCValueType.INT)
						{
							dr["Value"] = intReceiveValue;
							dr["Value(INT)"] = intReceiveValue;
							tmp = string.Format("{0:X4}", intReceiveValue);
							dr["Value(HEX)"] = tmp;
							dr["Value(STRING)"] = fnc.HexToString(tmp);
						}
						else
						{	//hex
							dr["Value"] = newValue;
							dr["Value(INT)"] = DBNull.Value;
							dr["Value(HEX)"] = newValue;
							dr["Value(STRING)"] = fnc.HexToString(newValue.ToString());
						}

						//이벤트 등록 여부 확인
						if (dicEvtAdd_ch.ContainsKey(address))
						{
							List<delChAddressValue> lst = dicEvtAdd_ch[address];

							foreach(delChAddressValue del in lst)
							{
								del.BeginInvoke(address, vType, newValue, null, null);								
							}

						}
						
					}

					
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
		/// <param name="SendByte">기록에 남길 보낸 byte 배열(null 입력 시 로그기록X)</param>
		/// <param name="ReceiveByte">기록에 남길 받은(응답) byte 배열(null 입력 시 로그기록X)</param>
		/// <returns></returns>
		protected bool WroteAddressValue(string strChanged, byte[] SendByte, byte[] ReceiveByte)
		{
			if (SendByte != null) LogWrite("WroteAddressValue", "[Sent] " + fnc.ByteToHex(SendByte));
			if (ReceiveByte != null) LogWrite("WroteAddressValue", "[Received] " + fnc.ByteToHex(ReceiveByte));
			LogWrite("WroteAddressValue", strChanged);

			return true;
		}

		

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
			if (ConnctionStatus != enStatus.OK ) return;

			//정상 작동중..
			if (thPLCScan != null && thPLCScan.IsAlive) return;

			//기능 사용 않함.
			if (delStart == null) return;

			LogWrite("tmrCheckScan", "Address 스캔 쓰래드가 중지 되었습니다. 다시 시작 합니다.");

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
