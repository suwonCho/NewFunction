using PLCModule.PLCModules;
using System;
using System.Data;


namespace PLCModule
{
	public enum enPlcType : int
	{	NotSet = 0, 
		Melsec_A = 1,
		Melsec_Q = 2,
#if (ABPLC_A || ABPLC_B)
		AB = 3,
#endif
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
	public class clsPLCModule : AppAuth.aAuth, IDisposable
	{

		public delegate void delChConnectionStatus(bool bolSocketStats);

		public delegate void delPLCScan();


		private clsPLCModuleInterface clsPLC;

		/// <summary>
		/// Log기록 클래스
		/// </summary>
		private static clsLog	clsLog;
		private static string	strLogHeader;

		

		public static void LogWrite(string strModule, string strMsg)
		{	
			try
			{
				if (clsLog == null) return;
			
				string strMessage = string.Format("[{0}] [{1}] {2}", strLogHeader, strModule, strMsg);
				clsLog.WLog(strMessage);
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
		/// plc socket 통신 상태 변경시 마다 발생 하는 이벤트..
		/// </summary>
		public  PLCModule.clsPLCModule.delPLCScan  evtPLCScan
		{
			get
			{
				return clsPLC.evtPLCScan;
			}			
			set
			{
				clsPLC.evtPLCScan = value;
			}
			
		}
		
		/// <summary>
		/// 연결상태 변경시 일어나는 이벤트 처리..(delegate) 구방식
		/// </summary>
		/// <param name="bolConnectionStatus">현재 상태</param>
		public delChConnectionStatus evtChConnectionStatus
		{
			get
			{
				return clsPLC.evtChConnectionStatus;
			}
			set
			{
				clsPLC.evtChConnectionStatus = value;
			}
		}


		

		/// <summary>
		/// 연결상태 변경시 일어나는 이벤트 처리
		/// </summary>
		public event delChConnectionStatus OnChConnectionStatus
		{
			add { clsPLC._onChConnectionStatus += value; }
			remove { clsPLC._onChConnectionStatus -= value; }
		}
		

		/// <summary>
		/// PLC 접속 여부 확인.
		/// </summary>
		public bool isConnected{ get { return clsPLC.isConnected; } }



		public void Set_Module(enPlcType ePlcType, string strIPAddress, int intPort, string strDeviceType, 
			string LogFolderPath, string strLogFileName)
		{
			if (clsPLC != null) return;

			try
			{
				enPLCType = ePlcType;

				IPAddress = strIPAddress;
				Port = intPort;

				switch (ePlcType)
				{
					case enPlcType.Melsec_A:
						clsPLC = new PLCModule.PLCModules.clsMelsecA(strIPAddress, intPort, strDeviceType);
						break;

					case enPlcType.Melsec_Q:
						clsPLC = new PLCModule.PLCModules.clsMelsecQ(strIPAddress, intPort, strDeviceType);
						break;

					case enPlcType.LS_XGT:
						clsPLC = new PLCModule.PLCModules.clsLS_XGT(strIPAddress, intPort, strDeviceType);
						break;

					case enPlcType.LS_XGI:
						clsPLC = new PLCModule.PLCModules.clsLS_XGI(strIPAddress, intPort, strDeviceType);
						break;

					case enPlcType.TEST:
						clsPLC = new PLCModule.PLCModules.clsTEST(strIPAddress, intPort, strDeviceType);
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
		public clsPLCModule()
		{
			
		}

		/// <summary>
		/// (인증필요)Melsec A/Q plc 객체 생성
		/// </summary>
		/// <param name="ePlcType">PLC종류 : Melsec_A or Melsec_Q</param>
		/// <param name="strIPAddress">IP Address</param>
		/// <param name="intPort">Port No</param>
		/// <param name="strLogFileName">기록을 쌓을 로그 파일(프로그램 폴더 \Log_PlcModule 폴더에 기록.</param>
		public clsPLCModule(enPlcType ePlcType, string strIPAddress, int intPort, string strDeviceType, string strLogFileName)
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
		public clsPLCModule(enPlcType ePlcType, string strIPAddress, string strProgramID, string strGroupName, string strTopicName, int intUpdateRate, string strLogFileName )
		{

			enPLCType = ePlcType;
			
			try
			{
#if(ABPLC_A || ABPLC_B)

				if (ePlcType == enPlcType.AB)
					clsPLC = new clsABPLC(strIPAddress, strProgramID, strGroupName, strTopicName, intUpdateRate);
				else
				{
					throw new Exception(string.Format("AB PLC 생성자의 Type이 잘 못 되었니다.(생성 요청 타입 : {0})",enPLCType.ToString()));
				}
#endif
			
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

			if (strLogFileName == string.Empty) strLogFileName = "plcModuleLog";

			clsLog = new clsLog(folderPath, strLogFileName, 30, true);

			dtAddress = new DataTable("PLC Address");

			dtAddress.Columns.Add(new DataColumn("Address", System.Type.GetType("System.String")));
			dtAddress.Columns.Add(new DataColumn("Value", System.Type.GetType("System.Int16")));
			dtAddress.Columns.Add(new DataColumn("Value(HEX)", System.Type.GetType("System.String")));

			dtAddress.PrimaryKey = new DataColumn[] { dtAddress.Columns["Address"] };

			clsPLC.dtWriteOrder.Columns.Add(new DataColumn("Address", System.Type.GetType("System.String")));
			clsPLC.dtWriteOrder.Columns.Add(new DataColumn("Value", System.Type.GetType("System.Int16")));



			//변수 및 이벤트 연결..
			clsPLC.dtAddress = this.dtAddress;

			//dtAddress.ColumnChanged += new DataColumnChangeEventHandler(dtAddress_ColumnChanged);

			strLogHeader = enPLCType.ToString();
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

			clsPLC.WriteOrder(e.Row["Address"].ToString(), value);
			

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

			clsLog.WLog(strLog);

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
				return this.clsPLC.Open();
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
				return clsPLC.Close();
			}
			catch (Exception ex)
			{	
				//ProcException(ex);	
				throw ex;
			}
		}
		
		/// <summary>
		/// Melset A/Q 검색할 Address 추가
		/// AB에서는 에러 발생.
		/// </summary>
		/// <param name="intAddress">Address 값</param>
		/// <returns></returns>
		public bool AddAddress(string Address)
		{
			try
			{
				return clsPLC.AddAddress(Address);
			}
			catch (Exception ex)
			{	
				ProcException(ex);	
				//return false;
				throw ex;
			}
		}
		
	
		
		
		/// <summary>
		/// Address 추가 범위 입력.
		/// </summary>
		/// <param name="strAddress">추가할 Address string 배열</param>
		/// <returns></returns>
		public bool AddAddress(string[] strAddress)
		{
			try
			{
				return clsPLC.AddAddress(strAddress);
			}
			catch(Exception ex)
			{
				ProcException(ex);
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
				return clsPLC.DelAddress(Address);
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
				return clsPLC.DelAddress(strAddress);
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}


		/// <summary>
		/// 1개 Address에 대한 값을 10진수로 반환
		/// AB에서는 에러 발생.
		/// </summary>
		/// <param name="intAddress">값</param>
		/// <returns></returns>
		public int GetValue(int intAddress)
		{
			try
			{
				return clsPLC.GetValue(intAddress.ToString());
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}
		

		
		/// <summary>
		/// 범위 Address에 대한 값을 원하는 10진수 길이로 반환
		/// </summary>
		/// <param name="strAddress">Address 범위</param>
		/// <returns></returns>
		public int GetValue(string[] strAddress)
		{		
			try
			{
				return clsPLC.GetValue(strAddress);
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}

		/// <summary>
		/// 범위 Address에 대한 값을 원하는 10진수 길이로 반환
		/// </summary>
		/// <param name="strAddress">Address 범위</param>
		/// <param name="intRetrunLength">반횐 값 길이</param>
		/// <returns></returns>
		public string GetValue(string[] strAddress, int intRetrunLength)
		{
			try
			{
				return clsPLC.GetValue(strAddress, intRetrunLength);
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
		/// <param name="strAddress"></param>
		/// <returns></returns>
		public int GetValueInt(string strAddress)
		{
			try
			{
				return clsPLC.GetValueInt(strAddress);
			}
			catch(Exception ex)
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
				return clsPLC.WriteOrder(Address, Value);
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
				return clsPLC.WriteOrder(Address, Value);
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}

		#endregion
		
		#region IDisposable 멤버

		public void Dispose()
		{
			clsPLC.Close();

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
