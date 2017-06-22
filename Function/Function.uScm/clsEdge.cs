using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Xml;
using System.Threading;
using System.Web;
using System.Web.Services;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Function;
using Function.Util;
using Function.Db;
using Function.Comm;

using net.mbiz.edt.pro.ricmgr.service.client;
using net.mbiz.edt.pro.ale.service.stub.ec;

namespace Function.uScm
{
	/// <summary>
	/// epcis M/W Edge 서버 관련 클래스
	/// </summary>
	public class clsEdge : IDisposable
	{

		public enum AntControlMode : int { Always = 0, Sensor = 1, Manual = 2 }

		string strRICName = string.Empty;
		string strEcSpecName = string.Empty;
		string SYSTEMID = " ";

		int intReadingDuration = 1000;
		int intWritingDuration = 1000;


		Function.Util.Log clsLog;
        OracleDB.strConnect strConn;

		string strLastError = string.Empty;

        public clsEdge(XML XML, OracleDB.strConnect _strConn)
		{

			XML.chNode2Root();

			string strSystemID = XML.GetSingleNodeValue("SystemInfo/PROGRAMID");
			strConn = _strConn;

			initXmlSetting(strSystemID, XML);

		}

        public clsEdge(string strSystemID, XML XML, OracleDB.strConnect _strConn)
		{
			strConn = _strConn;
			initXmlSetting(strSystemID, XML);
		}


		//public clsEpcMW(string strUrl, string strUrl_ALE, string strRicName, AntControlMode antMode, int intPort, int intAlarmId, int intGreenId)
		//{
		//    initClass(strUrl, strUrl_ALE, strRicName, antMode, intPort, intAlarmId, intGreenId);
		//}


		private void initXmlSetting(string strSystemID, XML XML)
		{
			try
			{
				XML.chNode2Root();
				//XML.chSingleNode(@"SETTING/" + strSystemID);
				//SYSTEMID = XML.GetSingleNodeValue("SYSTEMID");

				XML.chNode2Root();
				XML.chSingleNode("EpcMW");

				string strUrl = XML.GetSingleNodeValue("URL");
				string strUrl_ALE = XML.GetSingleNodeValue("URL_ALE");

				int intAlarmId = Convert.ToInt32(XML.GetSingleNodeValue("AlarmID"));
				int intGreenId = Convert.ToInt32(XML.GetSingleNodeValue("GreenID"));

				XML.chSingleNode(strSystemID);

				string strRicName = XML.GetSingleNodeValue("RICNAME");
				AntControlMode antMode = (AntControlMode)Convert.ToInt32(XML.GetSingleNodeValue("DefaultAntMode"));


				int intPort = Convert.ToInt32(XML.GetSingleNodeValue("ServerPort"));
				strEcSpecName = XML.GetSingleNodeValue("ECSPECNAME");
				intReadingDuration = Convert.ToInt32(XML.GetSingleNodeValue("ReadingDuration"));
				intWritingDuration = Convert.ToInt32(XML.GetSingleNodeValue("WritingDuration"));

				initClass(strUrl, strUrl_ALE, strRicName, antMode, intPort, intAlarmId, intGreenId);
			}
			catch (Exception ex)
			{
				SP_SMS_SET_ERROR(ex);
				throw;
			}
		}

		private void initClass(string strUrl, string strUrl_ALE, string strRicName, AntControlMode antMode, int intPort, int intAlarmId, int intGreenId)
		{
			clsLog = new Log(@".\RFID_MW", "MW", 30, true);

			client = new RICManagerEXClient(strUrl);
			Ale = new ALEService();
			Ale.Url = strUrl_ALE;

			//client.setAutoReadingControl(strRicName, (int)antMode);

			client.getWebservice();

			strRICName = strRicName;

			StratEventServer(intPort);

			SetSubScrib();

			intAlarmID = intAlarmId;
			intGreenID = intGreenId;

			//if (antMode == AntControlMode.Always)
			//    StartReading();
			//else
			//    StopReading();

			TurnOnAlarm(100);

		}


		#region <RFID 장비 Control>
		RICManagerEXClient client;
		int intAlarmID = 1;
		int intGreenID = 2;

		/// <summary>
		/// 테그 수신을 일정 시간 동안 읽는다.
		/// </summary>
		/// <param name="intDuration">읽을 시간(ms)</param>
		public void StartReading(int intDuration)
		{
			try
			{
				client.startReading(strRICName);

				Thread.Sleep(intDuration);

				client.stopReading(strRICName);
			}
			catch (Exception ex)
			{
				clsLog.WLog_Exception("StartReading", ex);
				SP_SMS_SET_ERROR(ex);
				throw ex;
			}
		}

		/// <summary>
		/// 수동 TagID 읽기 시작
		/// </summary>
		public void StartReading()
		{
			try
			{
				client.startReading(strRICName);
			}
			catch (Exception ex)
			{
				SP_SMS_SET_ERROR(ex);
				throw;
			}
		}
		/// <summary>
		/// 수공 TagID 읽기 정지
		/// </summary>
		public void StopReading()
		{
			try
			{
				client.stopReading(strRICName);
			}
			catch (Exception ex)
			{
				SP_SMS_SET_ERROR(ex);
				throw;
			}
		}


		/// <summary>
		/// 동기 방식으로 TagId를 읽는다.
		/// </summary>
		/// <param name="intReadingDuration"></param>
		/// <param name="intWatingDuration"></param>
		/// <returns></returns>
		public string[] SyncReading()
		{
			try
			{
				strTagid = null;
				StartReading();

				for (int i = 0; i < intReadingDuration; i += 200)
				{
					if (strTagid != null) break;
					Thread.Sleep(200);
				}

				StopReading();

				for (int i = 0; i < intReadingDuration; i += 200)
				{
					if (strTagid != null) break;
					Thread.Sleep(200);
				}

				return strTagid;
			}
			catch (Exception ex)
			{
				SP_SMS_SET_ERROR(ex);
				throw;
			}

		}

		/// <summary>
		/// 동기 방식으로 TagId를 읽는다.
		/// </summary>
		/// <param name="intReadingDuration"></param>
		/// <param name="intWatingDuration"></param>
		/// <returns></returns>
		public string[] SyncReading_NoTrigger()
		{
			try
			{
				strTagid = null;

				for (int i = 0; i < intReadingDuration; i += 200)
				{
					if (strTagid != null) break;
					Thread.Sleep(200);
				}

				for (int i = 0; i < intReadingDuration; i += 200)
				{
					if (strTagid != null) break;
					Thread.Sleep(200);
				}

				return strTagid;
			}
			catch (Exception ex)
			{
				SP_SMS_SET_ERROR(ex);
				throw;
			}

		}



		/// <summary>
		/// Subscribe동기 방식으로 TagId를 읽는다.
		/// </summary>
		/// <param name="intReadingDuration"></param>
		/// <param name="intWatingDuration"></param>
		/// <returns></returns>
		public string[] SyncReading_BySubscrib()
		{
			try
			{
				strTagid = null;

				SetSubScrib();

				for (int i = 0; i < intReadingDuration; i += 200)
				{
					if (strTagid != null) break;
					Thread.Sleep(200);
				}

				UnSubScribe();

				for (int i = 0; i < intReadingDuration; i += 200)
				{
					if (strTagid != null) break;
					Thread.Sleep(200);
				}

				return strTagid;
			}
			catch (Exception ex)
			{
				SP_SMS_SET_ERROR(ex);
				throw;
			}

		}


		/// <summary>
		/// 알람을 일정 시간동안 작동한다.
		/// </summary>
		/// <param name="intDuration">작동시간(ms)</param>
		public void TurnOnAlarm(int intDuration)
		{

			TurnOnAlarm();

			Thread.Sleep(intDuration);

			TurnOffAlarm();

		}
		/// <summary>
		/// 알람을 작동한다.
		/// </summary>
		public void TurnOnAlarm()
		{
			try
			{
				client.controlLED(strRICName, intAlarmID, true);
			}
			catch (Exception ex)
			{
				SP_SMS_SET_ERROR(ex);
				throw;
			}
		}
		/// <summary>
		/// 알람을 끊다.
		/// </summary>
		public void TurnOffAlarm()
		{
			try
			{
				client.controlLED(strRICName, intAlarmID, false);
			}
			catch (Exception ex)
			{
				SP_SMS_SET_ERROR(ex);
				throw;
			}
		}


		/// <summary>
		/// 테그를 Write 한다.
		/// </summary>
		/// <param name="strTagID"></param>
		/// <returns></returns>
		public bool WriteTagID(string strTagID)
		{
			try
			{
				int i = client.writeTag(strRICName, strTagID);

				if (i == 0)
					return true;
				else
					return false;
			}
			catch (Exception ex)
			{
				clsLog.WLog_Exception("WriteTagID", ex);
				SP_SMS_SET_ERROR(ex);
				throw ex;
			}
		}

		/// <summary>
		/// 정상 램프를 일정 시간동안 작동한다.
		/// </summary>
		/// <param name="intDuration">작동시간(ms)</param>
		public void TurnOnGreen(int intDuration)
		{
			try
			{
				TurnOnGreen();

				Thread.Sleep(intDuration);

				TurnOffGreen();
			}
			catch (Exception ex)
			{
				SP_SMS_SET_ERROR(ex);
				clsLog.WLog_Exception("TurnOnGreen", ex);
			}
		}

		public void TurnOnGreen()
		{
			try
			{
				client.controlLED(strRICName, intGreenID, true);
			}
			catch (Exception ex)
			{
				SP_SMS_SET_ERROR(ex);
				throw;
			}
		}

		public void TurnOffGreen()
		{
			try
			{
				client.controlLED(strRICName, intGreenID, false);
			}
			catch (Exception ex)
			{
				SP_SMS_SET_ERROR(ex);
				throw;
			}
		}





		public void SP_SMS_SET_ERROR(Exception ex)
		{
			if (strConn.strTNS == string.Empty) return;
			try
			{
				//같은 에러는 보내지 않는다.
				if (strLastError == ex.Message) return;

                OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

				OracleParameter[] param = new OracleParameter[] { 
                                                new OracleParameter("ps_SYSTEMID", OracleDbType.Varchar2,   15),
                                                new OracleParameter("ps_Desc", OracleDbType.Varchar2, 500)      };

				param[0].Value = SYSTEMID;
				param[1].Value = ex.Message;

				clsDB.intExcute_StoredProcedure("SP_SMS_SET_ERROR", param);

				strLastError = ex.Message;

			}
			catch //(Exception e)
			{
			}

		}



		#endregion

		#region <EcReport수신: TagID 수신 처리부>

		ALEService Ale;
		clsSocketServer clsSvr;
		private delegate void delClientConected(string strMsg);
		private delClientConected evtClientConected = null;


		public delegate void delReceiveTagID(string[] strTagid);
		/// <summary>
		/// tagID 수신 event
		/// </summary>
		public delReceiveTagID evtReceiveTagID;

		string[] strTagid = null;

		int intPort = 1000;


		private bool CheckSubscribes(Subscribe sub)
		{
			GetSubscribers subs = new GetSubscribers();


			foreach (string str in Ale.getSubscribers(subs))
			{
				Console.WriteLine(str);
			}

			return true;
		}

		public void SetSubScrib()
		{
			try
			{
				Subscribe sub = new Subscribe();
				string str = Function.Comm.clsSocketServer.MyIp(); 
				sub.notificationURI = string.Format("tcp://{0}:{1}", str, intPort);
				sub.specName = strEcSpecName;

				//CheckSubscribes(sub);

				Ale.subscribe(sub);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		public void UnSubScribe()
		{
			try
			{
				Unsubscribe uSci = new Unsubscribe();

				string str = Function.Comm.clsSocketServer.MyIp();

				uSci.notificationURI = string.Format("tcp://{0}:{1}", str, intPort);
				uSci.specName = strEcSpecName;

				Ale.unsubscribe(uSci);
			}
			catch
			{
			}

		}

		/// <summary>
		/// tagID 수신
		/// </summary>
		/// <param name="strTagid"></param>
		private void ReceiveTagId(object obj)
		{
			strTagid = (string[])obj;

			if (evtReceiveTagID != null) evtReceiveTagID(strTagid);
		}

		public void Dispose()
		{
			clsSvr.Stop();
		}


		private void StratEventServer(int _intPort)
		{
			try
			{
				intPort = _intPort;
				clsSvr = new clsSocketServer(intPort, 1);
				clsSvr.evtClentConnect = new clsSocketServer.delClientConnect(evtClientConnected);
				clsSvr.evtReceiveRequest = new clsSocketServer.delReceiveRequest(evtReceiveRequest);
				clsSvr.Start();

			}
			catch (Exception)
			{
				throw;
			}
		}



		private void evtClientConnected(Socket soc)
		{
			IPEndPoint ip = (IPEndPoint)soc.RemoteEndPoint;

			string str = string.Format("{0}:{1} 접속 확인", ip.Address.ToString(), ip.Port);

			clsLog.WLog(str);

			//if (evtClientConected != null) evtClientConected(str);

		}



		private void evtReceiveRequest(Socket soc, byte[] byt)
		{
			string str = Encoding.UTF8.GetString(byt);

			try
			{
				//받은 파일을 xml로 전환
				XML XML = new XML(XML.enXmlType.String, str);

				XML.xml.Save(@"c:\xml.xml");

				XML.chSingleNode("report/group");
				int intCnt = Convert.ToInt32(XML.GetSingleNodeValue("groupCount/count"));
				int i = 0;
				string[] strTagID = new string[intCnt];


				//XML.chSingleNode("groupList");
				string st = string.Empty;
				foreach (XmlNode xn in XML.GetNodeList("groupList/member"))
				{
					strTagID[i] = xn.SelectSingleNode("epc").InnerText;

					if (st == string.Empty)
						st = "수신 TagID : ";
					else
						st += "/";

					st += strTagID[i];

					i++;
				}

				clsLog.WLog(st);

				ThreadPool.QueueUserWorkItem(new WaitCallback(ReceiveTagId), strTagID);
			}
			catch (Exception ex)
			{
				clsLog.WLog_Exception("evtReceiveRequest", ex);
				clsLog.WLog("Received String : " + str);
			}



		}


		#endregion

	}
}
