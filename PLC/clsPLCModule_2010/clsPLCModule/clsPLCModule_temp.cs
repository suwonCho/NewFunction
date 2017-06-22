using System;


namespace PLCModule
{
	public enum enPlcType { NotSet, Melsec_A, Melsec_Q, AB };

	/// <summary>
	/// PLC ��� ���� �ܺ� ��� Ŭ����
	/// </summary>
	public class clsPLCModule : clsPLCModuleInterface, IDisposable
	{
		clsPLCModuleInterface clsPLC;

		/// <summary>
		/// Log��� Ŭ����
		/// </summary>
		clsLog	clsLog;
		enPlcType enPLCType = enPlcType.NotSet;	
		

		/// <summary>
		/// Melsec A/Q plc ��ü ����
		/// </summary>
		/// <param name="ePlcType"></param>
		/// <param name="?"></param>
		/// <param name="intPort"></param>
		/// <param name="strLogFileName"></param>
		public clsPLCModule(enPlcType ePlcType, string strIPAddress, int intPort, string strDeviceType, string strLogFileName)
		{
			try
			{
				enPLCType = ePlcType;

				if (ePlcType == enPlcType.Melsec_A)
					clsPLC = new clsMelsecA(strIPAddress, intPort, strDeviceType);
				else if(ePlcType == enPlcType.Melsec_Q)
					clsPLC = new clsMelsecQ(strIPAddress, intPort, strDeviceType);
				else
				{
					throw new Exception(string.Format("Melsec A/Q �������� Type�� �� �� �Ǿ��ϴ�.(���� ��û Ÿ�� : {0})",enPLCType.ToString()));
				}
			
				
				InitClass(strLogFileName);
			}
			catch(Exception ex)
			{
				ProcException(ex);
			}

		}


		public clsPLCModule(enPlcType ePlcType, string strIPAddress, string strProgramID, string strGroupName, string strTopicName, int intUpdateRate, string strLogFileName )
		{
			enPLCType = ePlcType;
			
			try
			{
				if (ePlcType == enPlcType.AB)
					clsPLC = new clsABPLC(strIPAddress, strProgramID, strGroupName, strTopicName, intUpdateRate);
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
			}
		}

        
		/// <summary>
		/// Log Ŭ������ �ʱ�ȭ ��Ų��.���α׷� ������ Log_PlcModule������ Log�� ���.
		/// </summary>
		/// <param name="strLogFileName">Log���� ���� �̸�</param>
		private void InitClass(string strLogFileName)
		{
			string strLogDirectory = @"\Log_PlcModule\";
			strLogFileName = strLogFileName.Trim();

			if (strLogFileName == string.Empty) strLogFileName = "plcModuleLog";

			clsLog = new clsLog(strLogDirectory, strLogFileName, 30, true);
			
			//���� �� �̺�Ʈ ����..
			this.dtAddress = clsPLC.dtAddress;
			
			//this.evtChConnectionStatus += new delChConnectionStatus(clsPLCModule_evtChConnectionStatus);

			
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
		

		
		#region override ó����	
		/// <summary>
		/// Opc�� ������ �մϴ�.
		/// </summary>
		/// <returns></returns>
		public override bool Open()
		{
			try
			{
				clsPLC.evtPLCScan = this.evtPLCScan;
				clsPLC.evtChConnectionStatus = this.evtChConnectionStatus;

				return this.clsPLC.Open();
			}
			catch (Exception ex)
			{	
				ProcException(ex);		
				throw ex;
			}
		}

		/// <summary>
		/// Opc ������ ������ ���´�.
		/// </summary>
		/// <returns></returns>
		public override bool Close()
		{
			try
			{
				return clsPLC.Close();
			}
			catch (Exception ex)
			{	
				ProcException(ex);	
				throw ex;
			}
		}

		public override bool AddAddress(int intAddress)
		{
			try
			{
				return clsPLC.AddAddress(intAddress);
			}
			catch (Exception ex)
			{	
				ProcException(ex);	
				throw ex;
			}
		}

		public override bool AddAddress(int intAddress, int intLength)
		{
			try
			{
				return clsPLC.AddAddress(intAddress, intLength);
			}
			catch (Exception ex)
			{	
				ProcException(ex);	
				throw ex;
			}
		}
	
		public override bool AddAddress(string[] strAddress)
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



		public override bool DelAddress(int intAddress)
		{
			try
			{
				return clsPLC.DelAddress(intAddress);
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}
		public override bool DelAddress(string[] strAddress)
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



		public override int GetValue(int intAddress)
		{
			try
			{
				return clsPLC.GetValue(intAddress);
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}

		public override int GetValue(int intAddress, int intLength)
		{
			try
			{
				return clsPLC.GetValue(intAddress, intLength);
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}

		public override string GetValue(int intAddress, int intLength, int intRetrunLength)
		{
			try
			{
				return clsPLC.GetValue(intAddress, intLength, intRetrunLength);
			}
			catch(Exception ex)
			{
				ProcException(ex);
				throw ex;
			}
		}
	
		public override int GetValue(string[] strAddress)
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
		public override string GetValue(string[] strAddress, int intRetrunLength)
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

		public override int GetValueInt(string strAddress)
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

		public override bool WriteOrder(int Address, int Value)
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
	
		public override bool WriteOrder(string[] Address, int[] Value)
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

		}

		#endregion
	}
}
