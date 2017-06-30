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
	/// (�����ʿ�)PLC ��� ���� �ܺ� ��� Ŭ����
	/// </summary>	
	public class clsPLCModule : AppAuth.aAuth, IDisposable
	{

		public delegate void delChConnectionStatus(bool bolSocketStats);

		public delegate void delPLCScan();


		private clsPLCModuleInterface clsPLC;

		/// <summary>
		/// Log��� Ŭ����
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
		/// plc Type ����.
		/// </summary>
		public enPlcType enPLCType = enPlcType.NotSet;	
		
		/// <summary>
		/// Address / Value Monitoring Table
		/// </summary>
		public DataTable dtAddress;

		public string IPAddress;

		public int Port;

		/// <summary>
		/// plc socket ��� ���� ����� ���� �߻� �ϴ� �̺�Ʈ..
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
		/// ������� ����� �Ͼ�� �̺�Ʈ ó��..(delegate) �����
		/// </summary>
		/// <param name="bolConnectionStatus">���� ����</param>
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
		/// ������� ����� �Ͼ�� �̺�Ʈ ó��
		/// </summary>
		public event delChConnectionStatus OnChConnectionStatus
		{
			add { clsPLC._onChConnectionStatus += value; }
			remove { clsPLC._onChConnectionStatus -= value; }
		}
		

		/// <summary>
		/// PLC ���� ���� Ȯ��.
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
						throw new Exception(string.Format("Melsec A/Q �������� Type�� �� �� �Ǿ��ϴ�.(���� ��û Ÿ�� : {0})", enPLCType.ToString()));

				}

				InitClass(LogFolderPath, strLogFileName);
			}
			catch (Exception ex)
			{
				ProcException(ex);
			}

		}


		/// <summary>
		/// (�����ʿ�)plc ��ü ����
		/// </summary>
		public clsPLCModule()
		{
			
		}

		/// <summary>
		/// (�����ʿ�)Melsec A/Q plc ��ü ����
		/// </summary>
		/// <param name="ePlcType">PLC���� : Melsec_A or Melsec_Q</param>
		/// <param name="strIPAddress">IP Address</param>
		/// <param name="intPort">Port No</param>
		/// <param name="strLogFileName">����� ���� �α� ����(���α׷� ���� \Log_PlcModule ������ ���.</param>
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
		/// (�����ʿ�)AB Plc ��ü ����.
		/// </summary>
		/// <param name="ePlcType">PLC���� : AB</param>
		/// <param name="strIPAddress">Remote Gateway�� ��� �� ��� ip address �Է�, local gateway ��� �� ���� �Է�</param>
		/// <param name="strProgramID">local : 'RSLinx OPC Server' / ���� : 'RSLinx Remote OPC Server'</param>
		/// <param name="strGroupName">Item Group�� : ���Է½�'Group'���� ���� </param>
		/// <param name="strTopicName">������ Topic �̸�</param>
		/// <param name="intUpdateRate">Value �����̽� �ð�(ms)</param>
		/// <param name="strLogFileName">����� ���� �α� ����(���α׷� ���� \Log_PlcModule ������ ���.</param>
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
					throw new Exception(string.Format("AB PLC �������� Type�� �� �� �Ǿ��ϴ�.(���� ��û Ÿ�� : {0})",enPLCType.ToString()));
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
		/// Log Ŭ������ �ʱ�ȭ ��Ų��.���α׷� ������ Log_PlcModule������ Log�� ���.
		/// </summary>
		/// <param name="strLogFileName">Log���� ���� �̸�</param>
		private void InitClass(string strLogFileName)
		{
			string strLogDirectory = @".\Log_PlcModule\";

			InitClass(strLogDirectory, strLogFileName);
				
		}


		/// <summary>
		/// Log Ŭ������ �ʱ�ȭ ��Ų��.���α׷� ������ Log_PlcModule������ Log�� ���.
		/// </summary>
		/// <param name="strLogFileName">Log���� ���� �̸�</param>
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



			//���� �� �̺�Ʈ ����..
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
		/// Exception ó���� �Ѵ�. �߻��� Log��� �� �߻�.
		/// </summary>
		/// <param name="ex"></param>
		private void ProcException(Exception ex)
		{
			string strLog = string.Empty;

			strLog = string.Format("[{0}]",enPLCType.ToString());
			
			strLog = string.Format("Error : {0} [Msg] {1} - {2}", strLog, ex.Message, ex.ToString());

			clsLog.WLog(strLog);

		}
		

		
		#region Method ó����	
		/// <summary>
		/// Opc�� ������ �մϴ�.
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
		/// Opc ������ ������ ���´�.
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
		/// Melset A/Q �˻��� Address �߰�
		/// AB������ ���� �߻�.
		/// </summary>
		/// <param name="intAddress">Address ��</param>
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
		/// Address �߰� ���� �Է�.
		/// </summary>
		/// <param name="strAddress">�߰��� Address string �迭</param>
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
		/// ��ϵ� Address ����
		/// AB������ ���� �߻�.
		/// </summary>
		/// <param name="intAddress">Address �ּ�</param>
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
		/// ��ϵ� Address ���� ����
		/// AB������ ���� �߻�.
		/// </summary>
		/// <param name="strAddress">������ Address string �迭</param>
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
		/// 1�� Address�� ���� ���� 10������ ��ȯ
		/// AB������ ���� �߻�.
		/// </summary>
		/// <param name="intAddress">��</param>
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
		/// ���� Address�� ���� ���� ���ϴ� 10���� ���̷� ��ȯ
		/// </summary>
		/// <param name="strAddress">Address ����</param>
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
		/// ���� Address�� ���� ���� ���ϴ� 10���� ���̷� ��ȯ
		/// </summary>
		/// <param name="strAddress">Address ����</param>
		/// <param name="intRetrunLength">��Ⱥ �� ����</param>
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
		/// 1�� Address�� ���� ���� 10������ ��ȯ
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
		/// Address�� ���� ��ִ´�.
		/// AB������ ���� �߻�.
		/// </summary>
		/// <param name="Address">Address</param>
		/// <param name="Value">��</param>
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
		/// Address ������ ���� ��ִ´�.		
		/// </summary>
		/// <param name="Address">Address ����</param>
		/// <param name="Value">��</param>
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
		
		#region IDisposable ���

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
