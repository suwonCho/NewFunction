
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
	/// (�����ʿ�)PLC ��� ���� �ܺ� ��� Ŭ����
	/// </summary>	
	public class PLCComm : IDisposable
	{	

		private iPLCCommInterface plc;

		/// <summary>
		/// Log��� Ŭ����
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
		/// ������� ����� �Ͼ�� �̺�Ʈ ó��
		/// </summary>
		/// <param name="bolConnectionStatus">���� ����</param>
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
		/// PLC ���� ���� ���¸� ������ �´�.
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
		public PLCComm()
		{
			
		}

		/// <summary>
		/// (�����ʿ�)Melsec A/Q plc ��ü ����
		/// </summary>
		/// <param name="ePlcType">PLC���� : Melsec_A or Melsec_Q</param>
		/// <param name="strIPAddress">IP Address</param>
		/// <param name="intPort">Port No</param>
		/// <param name="strLogFileName">����� ���� �α� ����(���α׷� ���� \Log_PlcModule ������ ���.</param>
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
		/// (�����ʿ�)AB Plc ��ü ����.
		/// </summary>
		/// <param name="ePlcType">PLC���� : AB</param>
		/// <param name="strIPAddress">Remote Gateway�� ��� �� ��� ip address �Է�, local gateway ��� �� ���� �Է�</param>
		/// <param name="strProgramID">local : 'RSLinx OPC Server' / ���� : 'RSLinx Remote OPC Server'</param>
		/// <param name="strGroupName">Item Group�� : ���Է½�'Group'���� ���� </param>
		/// <param name="strTopicName">������ Topic �̸�</param>
		/// <param name="intUpdateRate">Value �����̽� �ð�(ms)</param>
		/// <param name="strLogFileName">����� ���� �α� ����(���α׷� ���� \Log_PlcModule ������ ���.</param>
		public PLCComm(enPlcType ePlcType, string strIPAddress, string strProgramID, string strGroupName, string strTopicName, int intUpdateRate, string strLogFileName )
		{

			enPLCType = ePlcType;
			
			try
			{

				if (ePlcType == enPlcType.AB)
					plc = new clsABPLC(strIPAddress, strProgramID, strGroupName, strTopicName, intUpdateRate);
				else
				{
					throw new Exception(string.Format("AB PLC �������� Type�� �� �� �Ǿ��ϴ�.(���� ��û Ÿ�� : {0})",enPLCType.ToString()));
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

			if (strLogFileName == string.Empty) strLogFileName = "PlcCommLog";

			plc.log = new Log(folderPath, strLogFileName, 30, true);
			log = plc.log;


			//���� �� �̺�Ʈ ����..
			this.dtAddress = plc.dtAddress;
			
			strLogHeader = enPLCType.ToString();
		}

		/// <summary>
		/// �ش� �ּ��� ���� ���� �� ��� �߻� �� �̺�Ʋ�� ��� �Ѵ�.(���� �� �񵿱� ó���� �ϹǷ� ������ ó���� �Ұ�)
		/// </summary>
		/// <param name="address">������ �ּ�</param>
		/// <param name="onChAddressValue">�Ͼ �̺�Ʈ</param>
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
		/// Exception ó���� �Ѵ�. �߻��� Log��� �� �߻�.
		/// </summary>
		/// <param name="ex"></param>
		private void ProcException(Exception ex)
		{
			string strLog = string.Empty;

			strLog = string.Format("[{0}]",enPLCType.ToString());
			
			strLog = string.Format("Error : {0} [Msg] {1} - {2}", strLog, ex.Message, ex.ToString());

			log.WLog(strLog);

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
				return this.plc.Open();
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
				return plc.Close();
			}
			catch (Exception ex)
			{	
				//ProcException(ex);	
				throw ex;
			}
		}

		/// <summary>
		/// �˻��� Address �߰�
		/// </summary>
		/// <param name="Address">Address ��</param>
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
		/// ��ϵ� Address ����
		/// AB������ ���� �߻�.
		/// </summary>
		/// <param name="intAddress">Address �ּ�</param>
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
		/// ��ϵ� Address ���� ����
		/// AB������ ���� �߻�.
		/// </summary>
		/// <param name="strAddress">������ Address string �迭</param>
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
		/// 1�� Address�� ���� ���� 10������ ��ȯ		
		/// </summary>
		/// <param name="Address">��</param>
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
		/// ���� Address�� ���� ���� ���ϴ� 10������ ��ȯ
		/// </summary>
		/// <param name="strAddress">Address ����</param>
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
		/// 1�� Address�� ���� �����´�.
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
		///  1�� Address�� ���� String ���� ���� �´�.
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
		///  1�� Address�� ���� HEX���� ���� �´�.
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
				return plc.WriteOrder(Address, Value);
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
				return plc.WriteOrder(Address, Value);
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}


		/// <summary>
		/// Address�� ���� ��ִ´�.
		/// </summary>
		/// <param name="Address">Address</param>
		/// <param name="Value">��</param>
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

		#region IDisposable ���

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
