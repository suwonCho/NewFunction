//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;

//using System.Windows.Forms;
//using System.Runtime.InteropServices;
//using System.Diagnostics;
//using nSCIF;

//using Function;
//using Function.Util;

//namespace Function.Device
//{
//	/// <summary>
//	/// KIC 휴대용 리더기 장비 클래스(바코드, RFID 수신처리)
//	/// </summary>
//	public class KR900 : _DeviceBaseClass
//	{

//		CnSCIF scif;

//		#region DllImport
//		/// <summary>
//		/// PC에서 2대 이상의 리더기와 동시에 통신 가능하도록 리더기별 핸들 값을 반환합니다.
//		/// PC에서 리더기 7대까지 동시 연결 가능합니다.
//		/// </summary>		
//		/// <example> 예제)
//		/// <code>
//		/// int nReaderCnt = 3; //동시에 3대의 리더기와 연결하여 운영 시
//		///	UINT mMkHandle[7];		
//		///	for (nCnt = 0 ; nCnt  nReaderCnt ; nCnt++)
//		///	{
//		///		mMkHandle(nCnt) = MkMultiInstanceOpen()
//		/// }
//		/// </code>
//		/// </example>
//		/// <returns></returns>
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern UInt32 MkMultiInstanceOpen();
		
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern void MkInstanceClose(UInt32 hMkHandle);

//		/// <summary>
//		/// 리더기에 접속을 시도하고 Reader와 정상 통신이 되는지 확인 합니다.
//		/// </summary>
//		/// <param name="hMkHandle">리더기 핸들 값</param>
//		/// <param name="szPort">접속할 Com 포트를 지정합니다. ex)'COM11'</param>
//		/// <returns>성공 시(TRUE), 실패 시(FALSE)</returns>
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern bool MkConnect(UInt32 hMkHandle, string szPort);

//		/// <summary>
//		/// 리더와의 통신 연결을 끊습니다.
//		/// </summary>
//		/// <param name="hMkHandle">리더기 핸들 값</param>
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern void MkDisconnect(UInt32 hMkHandle);

//		// start or stop reading tags

//		/// <summary>
//		/// Anti-Collision 읽기를 시작 합니다. 이 함수가 정상적으로 반환되면 리더는 RF 를
//		/// 송출하기 시작하고 필드 내에 태그 EPC 정보를 수집하기 시작 합니다.
//		/// </summary>
//		/// <param name="hMkHandle">리더기 핸들 값</param>
//		/// <returns>성공 시(TRUE), 실패 시(FALSE)</returns>
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern bool MkBeginReadTag(UInt32 hMkHandle);

//		/// <summary>
//		/// Anti-Collision 읽기를 중지 합니다.
//		/// </summary>
//		/// <param name="hMkHandle">리더기 핸들 값</param>
//		/// <returns>성공 시(TRUE), 실패 시(FALSE)</returns>
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern bool MkFinishReadTag(UInt32 hMkHandle);


//		/// <summary>
//		/// RF 출력을 조절할 수 있는 RF Attenuation값을 설정합니다.
//		///	RF Attenuation 값은 0부터 30(최소출력)까지 1씩 조정 가능합니다. 0이 최대 출력을
//		/// 나타냅니다. 0은 30dBm을 나타내며 1단위가 약 1dB의 감소를 나타냅니다.
//		///	인자의 값이 0보다 작거나, 30보다 크다면, FALSE를 반환합니다.
//		/// </summary>
//		/// <param name="hMkHandle">리더기 핸들 값</param>
//		/// <param name="nAttenuation">RF Attenuation 값</param>
//		/// <returns>성공 시(TRUE), 실패 시(FALSE)</returns>
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern bool MkSetRfPowerAttenuation(UInt32 hMkHandle, int nAttenuation);

//		/// <summary>
//		/// 현재 리더에 설정되어 있는 RF Attenuation 값을 가져옵니다.
//		/// </summary>
//		/// <param name="hMkHandle">리더기 핸들 값</param>
//		/// <returns>RF Attenuation 값</returns>
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern int MkGetRfPowerAttenuation(UInt32 hMkHandle);



//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern int MkGetTagUIDCount(UInt32 hMkHandle);			// getting the number of tags, after beginning read tag or deleting all tag lists
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern int MkGetTagReadCount(UInt32 hMkHandle, int nIndex);
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern int MkGetTagReadTime(UInt32 hMkHandle, int nIndex);
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern int MkGetTagUID(UInt32 hMkHandle, int nIndex, byte[] lpTagUID);
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern void MkDeleteAllDB(UInt32 hMkHandle);		 // purge all stored tag list

//		// Reader Version
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern IntPtr MkGetReaderVersion(UInt32 hMkHandle);

//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern IntPtr MkLibVersion(UInt32 hMkHandle);


//		// Reader Power
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern void MkReaderPowerOn(UInt32 hMkHandle, int powerMode);
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern void MkReaderPowerOff(UInt32 hMkHandle);

//		/// <summary>
//		/// 시리얼 포트 상태(Trigger 동작상태) 정보를 반환합니다.
//		/// 평상 시 ‘0’을 반환하며, KR-900에서 Trigger 동작 시 ‘32’를 반환합니다.
//		/// </summary>
//		/// <param name="hMkHandle">리더기 핸들 값</param>
//		/// <param name="nModemStat">확인하고자 하는 시리얼 포트 속성
//		/// CTS = 0
//		/// DSR = 1 : Trigger 동작상태
//		/// RING = 2
//		/// RLSD = 3
//		/// </param>
//		/// <returns></returns>
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern int MkGetCommStatus(UInt32 hMkHandle, int nModemStat);

//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern bool MkSetSerialDTR(UInt32 hMkHandle, int nFuncCode);

//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern int MkGetCodeType(UInt32 hMkHandle, int nIndex);

//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern string MkGetTagUIDString(UInt32 hMkHandle, int nIndex);

//		/// <summary>
//		/// KR-900의 현재 동작 모드를 반환합니다.
//		/// </summary>
//		/// <param name="hMkHandle">리더기 핸들 값</param>
//		/// <returns>
//		/// Return Value
//		///	1 = RFID 900MHz
//		///	2 = RFID 13.56MHz
//		///	3 = Barcode
//		/// 0 이하 disconnect
//		/// </returns>
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern int MkGetCurrentMode(UInt32 hMkHandle);


//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern bool MkSetMode(UInt32 hMkHandle, int nFuncCode);

//		/// <summary>
//		/// KR-900의 BEEP음 출력을 시작/중지 합니다.
//		/// Beep음은 출력 시작 후 정지 명령이 수신될 때 까지 계속해서 출력됩니다.
//		/// </summary>
//		/// <param name="hMkHandle">리더기 핸들 값</param>
//		/// <param name="nFuncCode">
//		/// 1 : 출력 시작
//		/// 0 : 출력 중지
//		/// </param>
//		/// <returns>인자에 의해서 반환되는 함수 수행 결과</returns>
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern bool MkSetBeep(UInt32 hMkHandle, int nFuncCode);

//		/// <summary>
//		/// KR-900의 Green Led를 On/Off 합니다.
//		/// Green Led는 On 후 Off 명령이 수신될 때 까지 계속해서 On상태를 유지합니다.
//		/// </summary>
//		/// <param name="hMkHandle">리더기 핸들 값</param>
//		/// <param name="nFuncCode">
//		/// 1 : 출력 시작
//		/// 0 : 출력 중지
//		/// </param>
//		/// <returns>인자에 의해서 반환되는 함수 수행 결과</returns>
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern bool MkSetGreenLED(UInt32 hMkHandle, int nFuncCode);

//		/// <summary>
//		/// KR-900의 Red Led를 On/Off 합니다.
//		/// Green Led는 On 후 Off 명령이 수신될 때 까지 계속해서 On상태를 유지합니다.
//		/// </summary>
//		/// <param name="hMkHandle">리더기 핸들 값</param>
//		/// <param name="nFuncCode">
//		/// 1 : 출력 시작
//		/// 0 : 출력 중지
//		/// </param>
//		/// <returns>인자에 의해서 반환되는 함수 수행 결과</returns>
//		[DllImport("MKUM300LIB_WIN32.dll")]
//		private static extern bool MkSetRedLED(UInt32 hMkHandle, int nFuncCode);

//		#endregion

//		/// <summary>
//		/// 핸들 값...
//		/// </summary>
//		UInt32 hMkHandle;

				
//		int intComport;
//		/// <summary>
//		/// 통신 컴포트 : 블루 투스 연결시 사용 포트
//		/// </summary>
//		public int intComPort
//		{
//			get
//			{
//				return intComport;
//			}
//			set
//			{
//				intComport = value;
//			}
//		}

//		bool isConnected = false;
//		/// <summary>
//		/// 장비 연결 상태..
//		/// </summary>
//		public bool IsConnected
//		{
//			get { return isConnected; }
//			set
//			{
//				if (IsConnected != value)
//				{
//					if (OnConnectStatusChange != null) 
//					{	
//						if(value)
//							OnConnectStatusChange(usrKR900_Monitoring.enStatusType.Connecting);
//						else
//							OnConnectStatusChange(usrKR900_Monitoring.enStatusType.Disconnect);
//					}
//				}

//				isConnected = value;
//			}
//		}

//		/// <summary>
//		/// 연결 상태 체크 쓰레드
//		/// </summary>
//		Thread thChkConnected;

//		/// <summary>
//		/// 연결 상태 변경 delegate
//		/// </summary>
//		/// <param name="isConnected"></param>
//		public delegate void delConnectStatusChange(usrKR900_Monitoring.enStatusType Status);

//		/// <summary>
//		/// 연결 상태 변경 event
//		/// </summary>
//		public delConnectStatusChange OnConnectStatusChange = null;
		
//		/// <summary>
//		/// 바코드 수신 delegate
//		/// </summary>
//		/// <param name="strBarcode"></param>
//		public delegate void delReceiveBarcode(string strBarcode);

//		/// <summary>
//		/// 바코드 수신 event
//		/// </summary>
//		public delReceiveBarcode OnReceiveBarcode = null;


//		/// <summary>
//		/// 바코드 수신 delegate
//		/// </summary>
//		/// <param name="strBarcode"></param>
//		public delegate void delReceiveTAGID(srTagInfo TagInfo);

//		/// <summary>
//		/// 바코드 수신 event
//		/// </summary>
//		public delReceiveTAGID OnReceiveTAGID = null;

//		/// <summary>
//		/// 테그 정보 구조체..
//		/// </summary>
//		public struct srTagInfo
//		{
//			/// <summary>
//			/// HEX Code
//			/// </summary>
//			public byte[] bytTagID;
//			/// <summary>
//			/// TagID
//			/// </summary>
//			public string strTagID;
//			/// <summary>
//			/// tagid epc code 변환 에러 여부
//			/// </summary>
//			public bool isEpcError;
//			/// <summary>
//			/// Epc TagID
//			/// </summary>
//			public string strEpcTagID;

//		}

//		int intBeepDuration = 500;

//		/// <summary>
//		/// 신호 수신시 Beep음 유지 시간(ms) - default : 500
//		/// </summary>
//		public int intBEEPDuration
//		{
//			get { return intBeepDuration; }
//			set
//			{
//				intBeepDuration = intBEEPDuration;
//			}
//		}


//		public KR900(int intCOMPort)
//		{
//			Init(intCOMPort, "KR900");
//		}

//		public KR900(int intCOMPort, string strLogFileName)
//		{
//			Init(intCOMPort, strLogFileName);
//		}

//		unsafe private void Init(int intCOMPort, string strLogFileName)
//		{
//			try
//			{
//				Log = new Log(@".\KR900", strLogFileName, 30, true);

				

//				scif = new CnSCIF();
//				scif.OnRead += new OnReadHandler(scif_OnRead);

//				hMkHandle = MkMultiInstanceOpen();

//				scif.Open(hMkHandle, 0, 0, 0, 0, 0);

//				intComPort = intCOMPort;				
//			}
//			catch (Exception ex)
//			{
//				ProcException(ex, "Init");
//			}
			
//		}


//		int intii = 0;
//		/// <summary>
//		/// 장비와의 통신 포트를 연다.
//		/// </summary>
//		public void Open()
//		{
//			string strComPort = string.Format("COM{0}", intComPort);
//			MkConnect(hMkHandle, strComPort);

//			Log.WLog(string.Format("Port Open : {0}", strComPort));

//			if (thChkConnected == null || !thChkConnected.IsAlive)
//			{
//				thChkConnected = new Thread(new ThreadStart(ChkConnected));
//				thChkConnected.IsBackground = true;
//				intii++;
//				thChkConnected.Name = string.Format("KR900[{0}] Connection Check thread", intii); //strComPort);
//				thChkConnected.Start();
//			}
//		}

//		public void Dispose()
//		{
//			Close();
//		}


//		bool isClosed = false;
//		/// <summary>
//		/// 장비와의 통신 포트를 닫는다.
//		/// </summary>
//		public void Close()
//		{
//			isClosed = true;
//			try
//			{
//				if (thChkConnected.IsAlive)
//				{
//					thChkConnected.Abort();
//					thChkConnected.Join();
//					thChkConnected = null;
//				}

//				MkDisconnect(hMkHandle);

//				IsConnected = false;

//				Log.WLog("Port Close");
//			}
//			catch (Exception ex)
//			{
//				ProcException(ex, "Close");
//			}
//			finally
//			{
//				isClosed = false;
//			}
//		}


//		/// <summary>
//		/// 장비 연결 상태 체크...
//		/// </summary>
//		private void ChkConnected()
//		{
			
					
//			int nCurrentMode;
//			int nConnectChkCnt = 20;
//			bool isTrigger = false;
//			bool bConnectState = false;
//			do
//			{
//				try
//				{

//					//Console.WriteLine(Thread.CurrentThread.Name);

//					nConnectChkCnt++;

//					//20초 단위로 연결 상태 체크를 한다.
//					if (nConnectChkCnt > 5)
//					{
//						nCurrentMode = MkGetCurrentMode(hMkHandle);
//						if (nCurrentMode < 0)//KR-900연결 종료
//						{
//							IsConnected = false;
//							try
//							{
//								MkDisconnect(hMkHandle);
//								Thread.Sleep(2000);
//								Open();
//							}
//							catch (Exception ex)
//							{
//								ProcException(ex, "ChkConnected");
//							}
//						}
//						else
//						{
//							nConnectChkCnt = 0;
//							IsConnected = true;
//						}
//					}

//					if (IsConnected == true)
//					{
//						//현재 트리거 상태..
//						if (MkGetCommStatus(hMkHandle, 1) == 32)
//						{
//							if (!isTrigger)
//							{
//								//태그 수신 처리를 한다.
//								MkBeginReadTag(hMkHandle);
//								Console.WriteLine("TagReadingStart");
//								isTrigger = true;
//							}
//						}
//						else
//						{
//							if (isTrigger)
//							{
//								//태그 수신 처리를 끝낸다.
//								MkFinishReadTag(hMkHandle);
//								Console.WriteLine("TagReadingEnd");
//								isTrigger = false;
//							}
//						}
//					}

					
//				}
//				catch (Exception ex)
//				{
//					if (isClosed) return;

//					ProcException( ex, "ChkConnected");
//				}
								
//				Thread.Sleep(2000);


//			} while (true);
//		}


//		/// <summary>
//		/// RF 출력을 조절할 수 있는 RF Attenuation값을 설정합니다.
//		/// RF Attenuation 값은 0부터 30(최소출력)까지 1씩 조정 가능합니다. 0이 최대 출력을
//		/// 나타냅니다. 0은 30dBm을 나타내며 1단위가 약 1dB의 감소를 나타냅니다.
//		/// 인자의 값이 0보다 작거나, 30보다 크다면, FALSE를 반환합니다.
//		/// </summary>
//		public int RFPowerSet
//		{
//			get
//			{
//				return MkGetRfPowerAttenuation(hMkHandle);
//			}
//			set
//			{
//				if (value < 0 || value > 30)
//				{
//					throw new Exception("RF Power 설정 값은 0(최대)~30(최소) 내에 값을 사용 하여야 합니다.");
//				}

//				MkSetRfPowerAttenuation(hMkHandle, value);
//			}

//		}


//		bool isRecv = false;

//		/// <summary>
//		/// 수신 데이터 처리...
//		/// </summary>
//		/// <param name="buf"></param>
//		/// <param name="len"></param>
//		/// <returns></returns>
//		private unsafe int scif_OnRead(byte* buf, int len)
//		{

//			if (isRecv) return 0;

//			try
//			{

//				isRecv = true;

//				string str = "";
//				string strCodeKind = "";

//				byte btCodeKidn = buf[len + 1];

//				switch (btCodeKidn)
//				{
//					case 0:
//						strCodeKind = "Gen2";
//						srTagInfo TagInfo = new srTagInfo();

//						byte[] bytes = new byte[len - 4];

//						//Tag 앞뒤로 4자리 더 나오는 코드..
//						for (int ii = 2; ii < len - 2; ii++)
//						{
//							//str += Convert.ToChar(buf[ii]).ToString();
//							bytes[ii - 2] = buf[ii];
//						}



//						str = Fnc.ByteArray2HexString(bytes, string.Empty);

//						//Console.WriteLine("RFID : " + str);

//						TagInfo.bytTagID = bytes;
//						TagInfo.strTagID = str;
//						try
//						{
//							TagInfo.strEpcTagID = Function.EpcMW.clsEpcCode.Hex2EpcCode(str);
//							TagInfo.isEpcError = false;
//						}
//						catch
//						{
//							TagInfo.strEpcTagID = string.Empty;
//							TagInfo.isEpcError = true;
//						}

//						if (OnReceiveTAGID != null)
//						{
//							OnReceiveTAGID(TagInfo);
//						}
//						break;

//					case 1:
//						strCodeKind = "18000-6B";
//						break;

//					case 5:
//						strCodeKind = "Barcode";
//						//Tag 앞뒤로 4자리 더 나오는 코드..
//						for (int ii = 0; ii < len; ii++)
//						{
//							str += Convert.ToChar(buf[ii]).ToString();
//							//bytes[ii] = buf[ii];
//						}

//						//str = Encoding.Default.GetString(bytes);

//						if (OnReceiveBarcode != null) OnReceiveBarcode(str);
//						break;

//					default:
//						strCodeKind = "Error";
//						break;
//				}


//				//SetBeep_Async(intBeepDuration);


//				return 0;

//			}
//			catch (Exception ex)
//			{
//				ProcException(ex, "scif_OnRead");
//				return 1;
//			}
//			finally
//			{
//				isRecv = false;
//				Thread.Sleep(100);
//			}
		
//		}


//		/// <summary>
//		/// Beep음을 일정 시간 동안 출력 한다.
//		/// </summary>
//		/// <param name="intDuration"></param>
//		public void SetBeep_Async(int intDuration)
//		{
//			try
//			{
//				ThreadPool.QueueUserWorkItem(new WaitCallback(thSetBeep_Async), intDuration);
//			}
//			catch (Exception ex)
//			{
//				ProcException(ex, "SetBeep_Async");
//			}
//		}


//		private void thSetBeep_Async(object intDuration)
//		{
//			try
//			{
//				SetBeep(1);
//				Thread.Sleep((int)intDuration);
//				SetBeep(0);
//			}
//			catch (Exception ex)
//			{
//				ProcException(ex, "thSetBeep_Async");
//			}
			
//		}


//		/// <summary>
//		/// KR-900의 BEEP음 출력을 시작/중지 합니다.
//		/// Beep음은 출력 시작 후 정지 명령이 수신될 때 까지 계속해서 출력됩니다.
//		/// </summary>
//		/// <param name="nFunction">
//		/// 1 : 출력 시작
//		/// 0 : 출력 중지
//		/// </param>
//		public void SetBeep(int nFunction)
//		{
//			MkSetBeep(hMkHandle, nFunction);
//		}


//		/// <summary>
//		/// Green Led을 일정 시간 동안 출력 한다.
//		/// </summary>
//		/// <param name="intDuration"></param>
//		public void SetGreenLED_Async(int intDuration)
//		{
//			try
//			{
//				ThreadPool.QueueUserWorkItem(new WaitCallback(thSetGreenLED_Async), intDuration);
//			}
//			catch (Exception ex)
//			{
//				ProcException(ex, "SetGreenLED_Async");
//			}
//		}


//		private void thSetGreenLED_Async(object intDuration)
//		{
//			try
//			{
//				SetGreenLED(1);
//				Thread.Sleep((int)intDuration);
//				SetGreenLED(0);
//			}
//			catch (Exception ex)
//			{
//				ProcException(ex, "thSetGreenLED_Async");
//			}

//		}

//		/// <summary>
//		/// KR-900의 Green Led를 On/Off 합니다.
//		/// Green Led는 On 후 Off 명령이 수신될 때 까지 계속해서 On상태를 유지합니다.
//		/// </summary>
//		/// <param name="nFunction">
//		/// 1 : 출력 시작
//		/// 0 : 출력 중지
//		/// </param>
//		public void SetGreenLED(int nFunction)
//		{
//			MkSetGreenLED(hMkHandle, nFunction);
//		}



//		/// <summary>
//		/// Red Led을 일정 시간 동안 출력 한다.
//		/// </summary>
//		/// <param name="intDuration"></param>
//		public void SetRedLED_Async(int intDuration)
//		{
//			try
//			{
//				ThreadPool.QueueUserWorkItem(new WaitCallback(thSetGreenLED_Async), intDuration);
//			}
//			catch (Exception ex)
//			{
//				ProcException(ex, "SetRedLED_Async");
//			}
//		}


//		private void thSetRedLED_Async(object intDuration)
//		{
//			try
//			{
//				SetRedLED(1);
//				Thread.Sleep((int)intDuration);
//				SetRedLED(0);
//			}
//			catch (Exception ex)
//			{
//				ProcException(ex, "thSetRedLED_Async");
//			}

//		}

//		/// <summary>
//		/// KR-900의 Red Led를 On/Off 합니다.
//		/// Green Led는 On 후 Off 명령이 수신될 때 까지 계속해서 On상태를 유지합니다.
//		/// </summary>
//		/// <param name="nFunction">
//		/// 1 : 출력 시작
//		/// 0 : 출력 중지
//		/// </param>
//		public void SetRedLED(int nFunction)
//		{
//			MkSetRedLED(hMkHandle, nFunction);
//		}






//	}
//}
