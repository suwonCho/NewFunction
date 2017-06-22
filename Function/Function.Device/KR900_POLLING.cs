//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Collections;// ArrayList
//using System.Runtime.InteropServices;
//using System.Threading;
//using kicUtil;

//using Function;
//using Function.Util;

//namespace Function.Device
//{
//	/// <summary>
//	/// kr900 폴링방식 클래스
//	/// 참조의 kicUtil를 설치시 등록 하여 주여야하고
//	/// MKUM300LIB_WIN32VB.dll 파일은 실행 폴더에 복사 하여 주어야 한다.
//	/// </summary>
//	public class KR900_Polling : _DeviceBaseClass
//	{
//		int hMkHandle = -1;
//		public int nMaxReadCnt = 7;
//		public int[] pMkHandle = new int[7];// 2008/5/7 jck  KR900 멀티
//		bool[] bReadingStart = new bool[7];// 2008/5/7 jck  해당 리더가 리딩 중인지 

//		Timer tmrChkTag;

//		bool isCheckTag = false;


//		/// <summary>
//		/// 연결 상태 변경 delegate
//		/// </summary>
//		/// <param name="isConnected"></param>
//		public delegate void delConnectStatusChange(bool isConnected);

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

//		/// <summary>
//		/// 트리거 상태
//		/// </summary>
//		private bool isTriggerStatus = false;


//		public  KR900_Polling()
//		{
//			Log = new Log(@".\KR900_POLL", "KR900P", 30, true);

//			hMkHandle = KR900_DLL.MkMultiInstanceOpen();

//		}

//		public void Dispose()
//		{
//			Close();
//			KR900_DLL.MkInstanceClose(hMkHandle);
//		}


//		/// <summary>
//		/// 포트를 연다..
//		/// </summary>
//		/// <param name="strPortNo">COM1, COM11 형태로..</param>
//		/// <returns></returns>
//		public void Open(string strPortNo)
//		{
//			if (!KR900_DLL.MkConnect(hMkHandle, strPortNo))
//			{
//				throw new Exception(strPortNo + "포트 오픈 에러.."); 
//			}


//			tmrChkTag = new Timer(new TimerCallback(tmrChkTag_Tick), null, 3000, 100);

//		}

//		void tmrChkTag_Tick(object obj)
//		{

//			if (isCheckTag) return;

//			try
//			{
//				isCheckTag = true;

//				int intStatus = KR900_DLL.MkGetCommStatus(hMkHandle, 0);


//				int ii = 0;

//				//gnCurrentReadNo = 0;



//				if (hMkHandle > -1)
//				{

//					if (TriggerStatus(1) > 1)//KR-900 트리거 상태 체크
//					{
//						if (!isTriggerStatus)
//						{
//							isTriggerStatus = true;

//							if (ReadStart())//KR-900에 판독 시작
//							{
//								//SetSerialDTR(1);//KR-900에 판독 LED 및 Beep음 출력 시작
//								Thread.Sleep(30);
//							}
//						}

						
//					}
//					else
//					{
//						if (isTriggerStatus)
//						{
//							isTriggerStatus = false;
//							//ReadStop();//KR-900에 판독 중지
//							//SetSerialDTR(0);//KR-900에 판독 LED 및 Beep음 출력 중지
//						}
//					}

//					DataRead();
//				}

//			}
//			catch (Exception ex)
//			{
//				ProcException(ex, "tmrChkTag_Tick");
//			}
//			finally
//			{
//				isCheckTag = false;
//			}
//		}


//		private void DataRead()
//		{
			
//			int nTagReadCnt = 0;
//			int nReadTotal = 0;
//			int ii = 0;
//			int nListCnt = 0;
//			string strReadTagData = "";
//			string[] strReadInfo;
//			ArrayList arReadTagData = new ArrayList();

//			nTagReadCnt = DataReceive(arReadTagData);

//			if (nTagReadCnt > 0)
//			{				

//				foreach (Object objTag in arReadTagData)
//				{
//					//gnListViewCnt++;
//					strReadTagData = objTag.ToString();
//					strReadInfo = strReadTagData.Split(',');

//					srTagInfo TagInfo = new srTagInfo();

//					string str = strReadInfo[0];

//					//Console.WriteLine("RFID : " + str);

					
//					TagInfo.strTagID = str;
//					try
//					{
//						TagInfo.strEpcTagID = Function.EpcMW.clsEpcCode.Hex2EpcCode(str);
//						TagInfo.isEpcError = false;
//					}
//					catch
//					{
//						TagInfo.strEpcTagID = string.Empty;
//						TagInfo.isEpcError = true;
//					}

//					if (OnReceiveTAGID != null)
//					{
//						OnReceiveTAGID(TagInfo);
//					}
//					break;


					
//				}

//				TagBufferClear();

				
//			}




//		}

//		/// <summary>
//		/// 포트를 닫는다.
//		/// </summary>
//		public void Close()
//		{
//			KR900_DLL.MkDisconnect(hMkHandle);

//			if (tmrChkTag != null)
//			{
//				tmrChkTag.Dispose();
//				tmrChkTag = null;
//			}
//		}



//		public bool ReadStart()
//		{
//			return KR900_DLL.MkBeginReadTag(hMkHandle);
			
//		}

//		public bool ReadStop()
//		{
//			return KR900_DLL.MkFinishReadTag(hMkHandle);

//		}


//		public void TagBufferClear()
//		{
//			KR900_DLL.MkDeleteAllDB(hMkHandle);
//		}

//		public int TriggerStatus(int nModemStat)
//		{
//			return KR900_DLL.MkGetCommStatus(hMkHandle, nModemStat);
//		}

//		public int DataReceive(ArrayList arTagData)
//		{
//			int nReadTotalTagCnt = 0;
//			int lpTagUID = 0;
//			int lpTagUIDLen = 0;
//			int nTagCnt = 0;
//			string readData = "";
//			string strTagInfo = "";

//			//struReadTagInfo readInfo = new struReadTagInfo();

//			System.Windows.Forms.Application.DoEvents();

//			nReadTotalTagCnt = KR900_DLL.MkGetTagUIDCount(hMkHandle);


//			Console.WriteLine("읽은 숫자:{0}", nReadTotalTagCnt);

//			for (int ii = 0; ii < nReadTotalTagCnt; ii++)
//			{
//				lpTagUID = KR900_DLL.MkGetTagUIDStream(hMkHandle, ii, ref lpTagUIDLen);

//				kicUtil.clsGetStringClass kutil = new clsGetStringClass();
//				string strRet = kutil.RFIDGetString(lpTagUID, lpTagUIDLen);


//				readData = strRet.Replace(" ", "");

//				//readInfo.strTagID = readData;
//				//readInfo.nTagReadCnt = KR900_DLL.MkGetTagReadCount(hMkHandle, ii);

//				nTagCnt = KR900_DLL.MkGetTagReadCount(hMkHandle, ii);

//				strTagInfo = readData + "," + nTagCnt.ToString();

//				//arTagData.Add(readInfo);
//				arTagData.Add(strTagInfo);
//			}

//			//KR900_DLL.MkDeleteAllDB(hMkHandle);
//			return nReadTotalTagCnt;
//		}


//		/// <summary>
//		/// 안테나 파워 설정..
//		/// </summary>
//		/// <param name="nLevel">-3 ~ 30 설정..</param>
//		/// <returns></returns>
//		public bool SetRfPowerAttenuation(int nLevel)
//		{
//			return KR900_DLL.MkSetRfPowerAttenuation(hMkHandle, nLevel);
//		}

//		/// <summary>
//		/// 시리얼 DTR설정..
//		/// </summary>
//		/// <param name="nFuncCode"></param>
//		/// <returns></returns>
//		public int SetSerialDTR(int nFuncCode)
//		{
//			return KR900_DLL.MkSetSerialDTR(hMkHandle, nFuncCode);
//		}

//		/// <summary>
//		/// 디버그 윈도우를 뛰운다..
//		/// </summary>
//		/// <param name="fEnable"></param>
//		/// <returns></returns>
//		public int MkShowDebugWindow(Boolean fEnable)
//		{
//			return KR900_DLL.MkShowDebugWindow(hMkHandle, fEnable);
//		}

//		/// <summary>
//		/// 디버그 창을 뛰운다.
//		/// </summary>
//		/// <param name="hMkHandle"></param>
//		/// <param name="nDebugLevel"></param>
//		/// <returns></returns>
//		public int MkSetDebugLevel(int nDebugLevel)
//		{
//			return KR900_DLL.MkSetDebugLevel(hMkHandle, nDebugLevel);
//		}
//		//public bool setPower(bool power)
//		//{
//		//    return true;
//		//}

//		//public int ReadDataCount()
//		//{
//		//    return rowList.Count;
//		//}

//		//public bool TagWrite(int startBlock, byte[] writeBytes)
//		//{
//		//    return false;
//		//}


//		//// 읽어온 TagData clear
//		//public void ListClear()
//		//{
//		//    rowList.Clear();
//		//}
//		//// 리더기에 읽힌 Tag 카운트
//		//public int ListCount()
//		//{
//		//    return rowList.Count;
//		//}

//	}

//	internal class KR900_DLL
//	{

//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkMultiInstanceOpen", SetLastError = true)]
//		public static extern int MkMultiInstanceOpen();
//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkInstanceClose", SetLastError = true)]
//		public static extern void MkInstanceClose(int hMkHandle);

//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkConnect", SetLastError = true)]
//		public static extern bool MkConnect(int hMkHandle, string strPort);
//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkDisconnect", SetLastError = true)]
//		public static extern void MkDisconnect(int hMkHandle);

//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkBeginReadTag", SetLastError = true)]
//		public static extern bool MkBeginReadTag(int hMkHandle);
//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkFinishReadTag", SetLastError = true)]
//		public static extern bool MkFinishReadTag(int hMkHandle);

//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkGetTagUIDCount", SetLastError = true)]
//		public static extern int MkGetTagUIDCount(int hMkHandle);

//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkGetTagUIDStream", SetLastError = true)]
//		public static extern int MkGetTagUIDStream(int hMkHandle, int nIndex, ref int lpTagUIDLen);

//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkGetTagReadCount", SetLastError = true)]
//		public static extern int MkGetTagReadCount(int hMkHandle, int nIndex);

//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkGetTagReadTime", SetLastError = true)]
//		public static extern int MkGetTagReadTime(int hMkHandle, int nIndex);

//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkDeleteAllDB", SetLastError = true)]
//		public static extern void MkDeleteAllDB(int hMkHandle);

//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkSetRfPowerAttenuation", SetLastError = true)]
//		public static extern bool MkSetRfPowerAttenuation(int hMkHandle, int nAttenuation);

//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkGetRfPowerAttenuation", SetLastError = true)]
//		public static extern int MkGetRfPowerAttenuation(int hMkHandle);

//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkGetMemoryBank", SetLastError = true)]
//		public static extern int MkGetMemoryBank(int hMkHandle, int nBank);


//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkGetCommStatus", SetLastError = true)]
//		public static extern int MkGetCommStatus(int hMkHandle, int nModemStat);

//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkSetSerialDTR", SetLastError = true)]
//		public static extern int MkSetSerialDTR(int hMkHandle, int nFuncCode);

//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkShowDebugWindow", SetLastError = true)]
//		public static extern int MkShowDebugWindow(int hMkHandle, Boolean fEnable);
//		[DllImport("MKUM300LIB_WIN32VB.dll", EntryPoint = "MkSetDebugLevel", SetLastError = true)]
//		public static extern int MkSetDebugLevel(int hMkHandle, int nDebugLevel);



//	}
//}
