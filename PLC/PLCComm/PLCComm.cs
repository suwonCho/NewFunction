
using Function;
using Function.Util;
using System;
using System.Data;


namespace PLCComm
{
	public enum enPlcType : int
	{	NotSet = 0, 
		Melsec_A = 1,
		Melsec_Q = 2,
		AB = 3,
		LS_XGT = 4,
		LS_XGI = 5,
		TEST = 99
	};


	public enum enLS_XGT_CpuType : byte
	{
		XGK = 0xA0,
		KGI = 0xA4,
		XGR = 0xA8
	}



	/// <summary>
	/// (인증필요)PLC 통신 관련 외부 사용 클래스
	/// </summary>	
	public class PLCComm : IDisposable
	{	

		private iPLCCommInterface plc;

		/// <summary>
		/// Log기록 클래스
		/// </summary>
		private static Function.Util.Log log;
		private static string	strLogHeader;

		

		public static void LogWrite(string strModule, string strMsg)
		{	
			try
			{
				if (log == null) return;
			
				string strMessage = string.Format("[{0}] [{1}] {2}", strLogHeader, strModule, strMsg);
				log.WLog(strMessage);
			}
			catch
			{}

		}

		/// <summary>
		/// plc Type 설정.
		/// </summary>
		public enPlcType enPLCType = enPlcType.NotSet;	
		
		/// <summary>
		/// Address / Value Monitoring Table
		/// </summary>
		public DataTable dtAddress;

		public string IPAddress;

		public int Port;

		
		/// <summary>
		/// 연결상태 변경시 일어나는 이벤트 처리
		/// </summary>
		/// <param name="bolConnectionStatus">현재 상태</param>
		public event delChConnectionStatus OnChConnectionStatus
		{
			add
			{
				plc.OnChConnectionStatus += value;
			}
			remove
			{
				plc.OnChConnectionStatus -= value;
			}
		}
		

		/// <summary>
		/// PLC 접속 여부 상태를 가지고 온다.
		/// </summary>
		public enStatus ConnctionStatus { get { return plc.ConnctionStatus; } }



		internal void Set_Module(enPlcType ePlcType, string strIPAddress, int intPort, string strDeviceType, 
			string LogFolderPath, string strLogFileName)
		{
			if (plc != null) return;

			try
			{
				enPLCType = ePlcType;

				IPAddress = strIPAddress;
				Port = intPort;

				switch (ePlcType)
				{
					/*
					case enPlcType.Melsec_A:
						plc = new PLCModule.PLCModules.clsMelsecA(strIPAddress, intPort, strDeviceType);
						break;

					case enPlcType.Melsec_Q:
						plc = new PLCModule.PLCModules.clsMelsecQ(strIPAddress, intPort, strDeviceType);
						break;

					case enPlcType.LS_XGT:
						plc = new PLCModule.PLCModules.clsLS_XGT(strIPAddress, intPort, strDeviceType);
						break;

					case enPlcType.LS_XGI:
						plc = new PLCModule.PLCModules.clsLS_XGI(strIPAddress, intPort, strDeviceType);
						break;
					*/

					case enPlcType.TEST:
						plc = new TEST_PLC(strIPAddress, intPort, strDeviceType);
						break;

					default:
						throw new Exception(string.Format("Melsec A/Q 생성자의 Type이 잘 못 되었니다.(생성 요청 타입 : {0})", enPLCType.ToString()));

				}
				

				InitClass(LogFolderPath, strLogFileName);
			}
			catch (Exception ex)
			{
				ProcException(ex);
			}

		}


		/// <summary>
		/// (인증필요)plc 객체 생성
		/// </summary>
		public PLCComm()
		{
			
		}

		/// <summary>
		/// (인증필요)Melsec A/Q plc 객체 생성
		/// </summary>
		/// <param name="ePlcType">PLC종류 : Melsec_A or Melsec_Q</param>
		/// <param name="strIPAddress">IP Address</param>
		/// <param name="intPort">Port No</param>
		/// <param name="strLogFileName">기록을 쌓을 로그 파일(프로그램 폴더 \Log_PlcModule 폴더에 기록.</param>
		public PLCComm(enPlcType ePlcType, string strIPAddress, int intPort, string strDeviceType, string strLogFileName)
		{
			
			try
			{				
				string strLogDirectory = System.Windows.Forms.Application.StartupPath + @"\Log_PlcModule\";
				Set_Module(ePlcType, strIPAddress, intPort, strDeviceType, strLogDirectory, strLogFileName);				
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}

		}

		/// <summary>
		/// (인증필요)AB Plc 객체 생성.
		/// </summary>
		/// <param name="ePlcType">PLC종류 : AB</param>
		/// <param name="strIPAddress">Remote Gateway를 사용 할 경우 ip address 입력, local gateway 사용 시 공백 입력</param>
		/// <param name="strProgramID">local : 'RSLinx OPC Server' / 원격 : 'RSLinx Remote OPC Server'</param>
		/// <param name="strGroupName">Item Group명 : 미입력시'Group'으로 설정 </param>
		/// <param name="strTopicName">설정되 Topic 이릅</param>
		/// <param name="intUpdateRate">Value 업데이스 시간(ms)</param>
		/// <param name="strLogFileName">기록을 쌓을 로그 파일(프로그램 폴더 \Log_PlcModule 폴더에 기록.</param>
		public PLCComm(enPlcType ePlcType, string strIPAddress, string strProgramID, string strGroupName, string strTopicName, int intUpdateRate, string strLogFileName )
		{

			enPLCType = ePlcType;
			
			try
			{

				if (ePlcType == enPlcType.AB)
					plc = new clsABPLC(strIPAddress, strProgramID, strGroupName, strTopicName, intUpdateRate);
				else
				{
					throw new Exception(string.Format("AB PLC 생성자의 Type이 잘 못 되었니다.(생성 요청 타입 : {0})",enPLCType.ToString()));
				}

			
				enPLCType = ePlcType;
				InitClass(strLogFileName);
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}

        
		/// <summary>
		/// Log 클래스를 초기화 시킨다.프로그램 폴더에 Log_PlcModule폴더에 Log가 기록.
		/// </summary>
		/// <param name="strLogFileName">Log파일 생성 이름</param>
		private void InitClass(string strLogFileName)
		{
			string strLogDirectory = @".\Log_PlcModule\";

			InitClass(strLogDirectory, strLogFileName);
				
		}


		/// <summary>
		/// Log 클래스를 초기화 시킨다.프로그램 폴더에 Log_PlcModule폴더에 Log가 기록.
		/// </summary>
		/// <param name="strLogFileName">Log파일 생성 이름</param>
		private void InitClass(string folderPath, string strLogFileName)
		{	
			strLogFileName = strLogFileName.Trim();

			if (strLogFileName == string.Empty) strLogFileName = "PlcCommLog";

			plc.log = new Log(folderPath, strLogFileName, 30, true);
			log = plc.log;


			//변수 및 이벤트 연결..
			this.dtAddress = plc.dtAddress;
			
			strLogHeader = enPLCType.ToString();
		}

		/// <summary>
		/// 해당 주소의 값이 변경 될 경우 발생 할 이벤틀르 등록 한다.(실행 시 비동기 처리를 하므로 쓰레드 처리를 할것)
		/// </summary>
		/// <param name="address">감시할 주소</param>
		/// <param name="onChAddressValue">일어날 이벤트</param>
		public void ChangeEvtAddress_Add(string address, delChAddressValue onChAddressValue)
		{
			plc.ChangeEvtAddress_Add(address, onChAddressValue);
		}



		void dtAddress_ColumnChanged(object sender, DataColumnChangeEventArgs e)
		{
			string col_name = e.Column.ColumnName;
			int value = 0;

			if (col_name == "Value")
				value = int.Parse(e.Row[col_name].ToString());
			else if (col_name == "Value(HEX)")
				value = Convert.ToInt32(e.Row[col_name].ToString(), 16);
			else
				return;

			plc.WriteOrder(e.Row["Address"].ToString(), value);
			

		}
		
		
		/// <summary>
		/// Exception 처리를 한다. 발생전 Log기록 후 발생.
		/// </summary>
		/// <param name="ex"></param>
		private void ProcException(Exception ex)
		{
			string strLog = string.Empty;

			strLog = string.Format("[{0}]",enPLCType.ToString());
			
			strLog = string.Format("Error : {0} [Msg] {1} - {2}", strLog, ex.Message, ex.ToString());

			log.WLog(strLog);

		}
		

		
		#region Method 처리부	
		/// <summary>
		/// Opc와 연결을 합니다.
		/// </summary>
		/// <returns></returns>
		public bool Open()
		{
			try
			{				
				return this.plc.Open();
			}
			catch (Exception ex)
			{	
				//ProcException(ex);		
				throw ex;
			}
		}

		/// <summary>
		/// Opc 서버와 연결은 끊는다.
		/// </summary>
		/// <returns></returns>
		public bool Close()
		{
			try
			{
				return plc.Close();
			}
			catch (Exception ex)
			{	
				//ProcException(ex);	
				throw ex;
			}
		}

		/// <summary>
		/// 검색할 Address 추가
		/// </summary>
		/// <param name="Address">Address 값</param>
		/// /// <param name="type">ValueType</param>
		/// <returns></returns>
		public bool AddAddress(string Address, enPLCValueType type = enPLCValueType.INT)
		{
			try
			{
				return plc.AddAddress(Address, type);
			}
			catch (Exception ex)
			{	
				ProcException(ex);	
				//return false;
				throw ex;
			}
		}
		
	
		
		
		
		/// <summary>
		/// 등록된 Address 삭제
		/// AB에서는 에러 발생.
		/// </summary>
		/// <param name="intAddress">Address 주소</param>
		/// <returns></returns>
		public bool DelAddress(string Address)
		{
			try
			{
				return plc.DelAddress(Address);
			}
			catch(Exception ex)
			{
				ProcException(ex);
				//return false;
				throw ex;
			}
		}

		/// <summary>
		/// 등록된 Address 범위 삭제
		/// AB에서는 에러 발생.
		/// </summary>
		/// <param name="strAddress">삭제할 Address string 배열</param>
		/// <returns></returns>
		public bool DelAddress(string[] strAddress)
		{
			try
			{
				return plc.DelAddress(strAddress);
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}


		/// <summary>
		/// 1개 Address에 대한 값을 10진수로 반환		
		/// </summary>
		/// <param name="Address">값</param>
		/// <returns></returns>
		public int GetValueInt(string Address)
		{
			try
			{
				return plc.GetValueInt(Address);
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}
		

		
		/// <summary>
		/// 범위 Address에 대한 값을 원하는 10진수로 반환
		/// </summary>
		/// <param name="strAddress">Address 범위</param>
		/// <returns></returns>
		public int GetValuesInt(string[] strAddress)
		{		
			try
			{
				string hex = string.Empty;

				foreach(string a in strAddress)
				{
					hex += plc.GetValueHex(a);
				}

				return Convert.ToInt32(hex, 16);
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}


		/// <summary>
		/// 1개 Address의 값을 가져온다.
		/// </summary>
		/// <param name="strAddress"></param>
		/// <returns></returns>
		public AddressValue GetValue(string strAddress)
		{
			try
			{
				return plc.GetValue(strAddress);
			}
			catch (Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}

		/// <summary>
		///  1개 Address에 대한 String 값을 가져 온다.
		/// </summary>
		/// <param name="strAddress"></param>
		/// <returns></returns>

		public string GetValueString(string strAddress)
		{
			try
			{
				return plc.GetValueString(strAddress);
			}
			catch (Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}
	
		
		

		/// <summary>
		///  1개 Address에 대한 HEX값을 가져 온다.
		/// </summary>
		/// <param name="strAddress"></param>
		/// <returns></returns>
		public string GetValueHex(string strAddress)
		{
			try
			{
				return plc.GetValueHex(strAddress);
			}
			catch (Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}


		/// <summary>
		/// Address에 값을 써넣는다.
		/// AB에서는 에러 발생.
		/// </summary>
		/// <param name="Address">Address</param>
		/// <param name="Value">값</param>
		/// <returns></returns>
		public bool WriteOrder(string Address, int Value)
		{
			try
			{
				return plc.WriteOrder(Address, Value);
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}
		
		/// <summary>
		/// Address 범위에 값을 써넣는다.		
		/// </summary>
		/// <param name="Address">Address 범위</param>
		/// <param name="Value">값</param>
		/// <returns></returns>
		public bool WriteOrder(string[] Address, int[] Value)
		{
			try
			{
				return plc.WriteOrder(Address, Value);
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}


		/// <summary>
		/// Address에 값을 써넣는다.
		/// </summary>
		/// <param name="Address">Address</param>
		/// <param name="Value">값</param>
		/// <returns></returns>
		public bool WriteOrder(string Address, string Value)
		{
			try
			{
				return plc.WriteOrder(Address, Value);
			}
			catch (Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}



		#endregion

		#region IDisposable 멤버

		public void Dispose()
		{
			plc.Close();

			if (fMonitor != null && fMonitor.isOpenForm)
			{
				fMonitor.Close();
			}

		}

		#endregion

		frmPLCMonitor fMonitor;

		public void MonitorFormOpen()
		{
			if (fMonitor == null || !fMonitor.isOpenForm)
			{
				fMonitor = new frmPLCMonitor(this);
				fMonitor.Show();
			}
		}

	}
}
