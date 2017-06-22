using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web;
using System.IO;
using System.Net;
using System.Data;


using Function;
using Function.Util;

namespace Function.EpcMW
{
	public class clsEPCIS
	{

#if(TEST)
		public XML xml;
#else
		XML xml;
#endif

		public enum enAction { OBSERVE, ADD, DELETE };

		public string url = string.Empty;						//"http://122.237.168.61:8082/uplusepcis/capture";
		public string strTimeZone = "+0900";
		public string strTimeZoneOffSet = "+09:00";


		public string strbizStep = string.Empty;
		public string strdisposition = string.Empty;
		public string[] strreadPoint;
		public string[] strbizLocation;


		//XML 기본 BODY
		string strXmlBody;

		public void initClass()
		{

			strXmlBody = Fnc.GetResource2string(System.Reflection.Assembly.GetExecutingAssembly(), "Function.EpcMW.xmlEpcis.xml");

			this.ResetXml();

			//SetObjectEvnet(
			//    DateTime.Now,
			//    new string[] { "urn:epc:id:sscc:0614142.0000000003" },
			//    enAction.ADD,
			//    "urn:epcglobal:fmcg:bizstep:shipping",
			//    "urn:epcglobal:hls:active",
			//    new string[] { "urn:epcglobal:fmcg:loc:0614141073467.RP-2" },
			//    new string[] { "urn:epcglobal:fmcg:loc:0614141073467" }
			//    );

			//SEND();

		}

		public clsEPCIS()
		{
			initClass();


			//SetObjectEvnet( xmlEpcis,
			//    DateTime.Now,
			//    new string[] { "urn:epc:id:sscc:0614142.0000000003", "urn:epc:id:sscc:0614142.0000000004", "urn:epc:id:sscc:0614142.0000000005" },
			//    enAction.OBSERVE,
			//    "urn:epcglobal:fmcg:bizstep:shipping",
			//    "urn:epcglobal:hls:active",
			//    new string [] {"urn:epcglobal:fmcg:loc:0614141073467.RP-2"},
			//    new string [] {"urn:epcglobal:fmcg:loc:0614141073467"}				
			//    );

		}


		public clsEPCIS(XML xmlConfig)
		{
			xmlConfig.chNode2Root();

			xmlConfig.chSingleNode("SystemInfo");

			string strPROGID = xmlConfig.GetSingleNodeValue("PROGRAMID");

			initXml(strPROGID, xmlConfig);
		}

		public clsEPCIS(string strNodeName, XML xmlConfig)
		{
			initXml(strNodeName, xmlConfig);
		}

		private void initXml(string strNodeName, XML xmlConfig)
		{
			try
			{
				xmlConfig.chNode2Root();

				xmlConfig.chSingleNode("EPCIS");

				url = xmlConfig.GetSingleNodeValue("URL");

				xmlConfig.chSingleNode(strNodeName);

				strbizStep = xmlConfig.GetSingleNodeValue("bizStep");
				strdisposition = xmlConfig.GetSingleNodeValue("disposition");
				strreadPoint = xmlConfig.GetSelectNodeValues("readPoint");
				strbizLocation = xmlConfig.GetSelectNodeValues("bizLocation");

				initClass();
			}
			catch
			{ }
		}

		private string Time2EpcTime(DateTime dt)
		{
			return dt.ToString("yyyy-MM-ddTHH:mm:ssZ");
		}

		/// <summary>
		/// 현재 xml에 설정 되어 있는 데이터를 보낸다.
		/// </summary>
		/// <returns></returns>
		public string SEND()
		{

			byte[] buffer;

			HttpWebRequest hwr;
			HttpWebResponse hwrsp;
			Stream strm;
			StreamReader sr;
			try
			{

				hwr = (HttpWebRequest)WebRequest.Create(url);
				hwr.Timeout = 30000;
				hwr.Method = "POST";

				//hwr.ContentType = @"application/x-www-form-urlencoded";				
				//hwr.CookieContainer = cookieContainer;

				xml.chNode2Root();

				string strXml = xml.GetSingleNodeInnerXml("");
				buffer = Encoding.UTF8.GetBytes(strXml);

				hwr.ContentLength = buffer.Length;
				Stream sendStream = hwr.GetRequestStream();
				sendStream.Write(buffer, 0, buffer.Length);
				sendStream.Close();
				hwrsp = (HttpWebResponse)hwr.GetResponse();
				strm = hwrsp.GetResponseStream();
				strm.ReadTimeout = 5000;
				if (hwrsp.CharacterSet.IndexOf("949") > 0)
					sr = new StreamReader(strm, Encoding.Default);
				else
					sr = new StreamReader(strm, Encoding.GetEncoding(hwrsp.CharacterSet));

				string responseHtml = sr.ReadToEnd();
				sr.Close();
				strm.Close();

				return responseHtml;
			}
			catch (WebException ex)
			{
				//throw ex;
				return string.Empty;
			}
			catch
			{
				//throw;
				return string.Empty;
			}




		}


		/// <summary>
		/// xml내용을 초기화 한다.
		/// </summary>
		public void ResetXml()
		{
			xml = new XML(XML.enXmlType.String, strXmlBody);
		}



		/// <summary>
		/// object 이벤트를 만든다.
		/// </summary>
		/// <param name="xml"></param>
		/// <param name="dtEventTime"></param>
		/// <param name="strEpcList"></param>
		/// <param name="eAction"></param>
		/// <param name="strBizStep"></param>
		/// <param name="strDispostion"></param>
		/// <param name="strReadPoint"></param>
		/// <param name="strbizLocation"></param>
		public void SetObjectEvnet(DateTime dtEventTime, string[] strEpcList, enAction eAction, string strBizStep, string strDispostion, string[] strReadPoint, string[] strbizLocation)
		{

			xml.chNode2Root();
			xml.chSingleNode("EPCISBody/EventList");


			//ObjectEvent 생성
			xml.AddChild("ObjectEvent", "");

			XmlNodeList xnl = xml.GetNodeList("ObjectEvent");

			int i = xnl.Count;

			xml.xmlNode = xnl[i - 1];

			string strEventTime = Time2EpcTime(dtEventTime);
			string strRecordTime = Time2EpcTime(DateTime.Now);

			xml.AddChild("eventTime", strEventTime);
			xml.AddChild("recordTime", strRecordTime);
			xml.AddChild("eventTimeZoneOffset", strTimeZoneOffSet);

			xml.AddChild("epcList", string.Empty);

			foreach (string str in strEpcList)
			{
				xml.AddChild("epcList", "epc", str);
			}

			xml.AddChild("action", eAction.ToString());
			xml.AddChild("bizStep", strBizStep);
			xml.AddChild("disposition", strDispostion);


			xml.AddChild("readPoint", string.Empty);

			foreach (string str in strReadPoint)
			{
				xml.AddChild("readPoint", "id", str);
			}

			xml.AddChild("bizLocation", string.Empty);

			foreach (string str in strbizLocation)
			{
				xml.AddChild("bizLocation", "id", str);
			}


			xml.chNode2Root();

			//string strxml = xml.GetSingleNodeInnerXml("");

			//xml.SaveToFile(@"test_ObjectEvet.xml");

			//strTest = xml.GetSingleNodeInnerXml("");

		}


		public void SetAggregationEvent(DateTime dtEventTime, string[] strParentIDs, int intChildCnt, DataTable dtProduct,
											enAction eAction, string BizStep, string Dispostion, string[] ReadPoint, string[] bizLocation)
		{
			int intRowCnt = 0;
			foreach (string strParentID in strParentIDs)
			{
				string[] strChild = new string[intChildCnt];

				for (int i = 0; i < intChildCnt; i++)
				{
					if (intRowCnt >= dtProduct.Rows.Count) break;
					strChild[i] = Fnc.obj2String(dtProduct.Rows[intRowCnt]["TAGID"]);
					intRowCnt++;
				}

				SetAggregationEvent(dtEventTime, strParentID, strChild, eAction, BizStep, Dispostion, ReadPoint, bizLocation);

			}
		}


		/// <summary>
		/// 어그리게이션 이벤트를 올린다.
		/// </summary>
		/// <param name="dtEventTime"></param>
		/// <param name="strParentID"></param>
		/// <param name="strChildEpc"></param>
		/// <param name="eAction"></param>
		/// <param name="strBizStep"></param>
		/// <param name="strDispostion"></param>
		/// <param name="strReadPoint"></param>
		/// <param name="strbizLocation"></param>
		public void SetAggregationEvent(DateTime dtEventTime, string strParentID, string[] strChildEpc,
											enAction eAction, string strBizStep, string strDispostion, string[] strReadPoint, string[] strbizLocation)
		{
			xml.chNode2Root();
			xml.chSingleNode("EPCISBody/EventList");


			//ObjectEvent 생성
			xml.AddChild("AggregationEvent", "");

			XmlNodeList xnl = xml.GetNodeList("AggregationEvent");

			int i = xnl.Count;

			xml.xmlNode = xnl[i - 1];


			string strEventTime = Time2EpcTime(dtEventTime);

			xml.AddChild("eventTime", strEventTime);

			xml.AddChild("eventTimeZoneOffset", strTimeZoneOffSet);

			xml.AddChild("parentID", strParentID);

			xml.AddChild("childEPCs", string.Empty);

			foreach (string str in strChildEpc)
			{
				if (str != null && str != string.Empty) xml.AddChild("childEPCs", "epc", str);
			}

			xml.AddChild("action", eAction.ToString());
			xml.AddChild("bizStep", strBizStep);
			xml.AddChild("disposition", strDispostion);

			xml.AddChild("readPoint", string.Empty);

			foreach (string str in strReadPoint)
			{
				xml.AddChild("readPoint", "id", str);
			}

			xml.AddChild("bizLocation", string.Empty);
			foreach (string str in strbizLocation)
			{
				xml.AddChild("bizLocation", "id", str);
			}

			xml.SaveToFile(@"test_AggEvet.xml");

			xml.chNode2Root();

		}

		/// <summary>
		/// TransactionEvent 생성 xml 내용을 만든다.
		/// </summary>
		/// <param name="dtEventTime"></param>
		/// <param name="strParentID"></param>
		/// <param name="strEpcList"></param>
		/// <param name="eAction"></param>
		/// <param name="strBizStep"></param>
		/// <param name="strDispostion"></param>
		/// <param name="strReadPoint"></param>
		/// <param name="strbizLocation">bizlocatitonType/Value 형태로 데이터 넘길것..</param>
		/// <param name="strbizTransactionList"></param>
		public void SetTransactionEvent(DateTime dtEventTime, string strParentID, string[] strEpcList,
											enAction eAction, string strBizStep, string strDispostion, string[] strReadPoint,
												string[] strbizLocation, string[] strbizTransactionList)
		{
			xml.chNode2Root();
			xml.chSingleNode("EPCISBody/EventList");


			//ObjectEvent 생성
			xml.AddChild("TransactionEvent", "");

			XmlNodeList xnl = xml.GetNodeList("TransactionEvent");

			int i = xnl.Count;

			xml.xmlNode = xnl[i - 1];

			string strEventTime = Time2EpcTime(dtEventTime);

			xml.AddChild("eventTime", strEventTime);
			xml.AddChild("eventTimeZoneOffset", strTimeZoneOffSet);


			xml.AddChild("bizTransactionList", string.Empty);
			foreach (string str in strbizTransactionList)
			{
				string[] strV = str.Split(new string[] { "/" }, StringSplitOptions.None);
				xml.AddAttChild("bizTransactionList", "bizTransaction", strV[0], "type", strV[1]);
			}

			xml.AddChild("epcList", string.Empty);
			foreach (string str in strEpcList)
			{
				xml.AddChild("epcList", "epc", str);
			}


			xml.AddChild("action", eAction.ToString());
			xml.AddChild("bizStep", strBizStep);
			xml.AddChild("disposition", strDispostion);


			xml.AddChild("readPoint", strDispostion);
			foreach (string str in strReadPoint)
			{
				xml.AddChild("readPoint", "id", str);
			}


			xml.AddChild("bizLocation", strDispostion);
			foreach (string str in strbizLocation)
			{
				xml.AddChild("bizLocation", "id", str);
			}

			xml.chNode2Root();

			//xml.SaveToFile(@"test_AggregationEvent.xml");

		}


	}
}
