using System;


namespace PLCModule
{
	public enum enPlcType { NotSet, Melsec_A, Melsec_Q, AB };

	/// <summary>
	/// PLC 통신 관련 외부 사용 클래스
	/// </summary>
	public class clsPLCModule : clsPLCModuleInterface, IDisposable
	{
		clsPLCModuleInterface clsPLC;

		/// <summary>
		/// Log기록 클래스
		/// </summary>
		clsLog	clsLog;
		enPlcType enPLCType = enPlcType.NotSet;	
		

		/// <summary>
		/// Melsec A/Q plc 객체 생성
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
					throw new Exception(string.Format("Melsec A/Q 생성자의 Type이 잘 못 되었니다.(생성 요청 타입 : {0})",enPLCType.ToString()));
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
					throw new Exception(string.Format("AB PLC 생성자의 Type이 잘 못 되었니다.(생성 요청 타입 : {0})",enPLCType.ToString()));
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
		/// Log 클래스를 초기화 시킨다.프로그램 폴더에 Log_PlcModule폴더에 Log가 기록.
		/// </summary>
		/// <param name="strLogFileName">Log파일 생성 이름</param>
		private void InitClass(string strLogFileName)
		{
			string strLogDirectory = @"\Log_PlcModule\";
			strLogFileName = strLogFileName.Trim();

			if (strLogFileName == string.Empty) strLogFileName = "plcModuleLog";

			clsLog = new clsLog(strLogDirectory, strLogFileName, 30, true);
			
			//변수 및 이벤트 연결..
			this.dtAddress = clsPLC.dtAddress;
			
			//this.evtChConnectionStatus += new delChConnectionStatus(clsPLCModule_evtChConnectionStatus);

			
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
		

		
		#region override 처리부	
		/// <summary>
		/// Opc와 연결을 합니다.
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
		/// Opc 서버와 연결은 끊는다.
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
		
		#region IDisposable 멤버

		public void Dispose()
		{
			clsPLC.Close();

		}

		#endregion
	}
}
