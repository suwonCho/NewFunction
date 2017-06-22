using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoUpdateClient
{
	class vari
	{
		public const string key = "D0cS3iiX9GFj+9OKvKQtV+FulVkBE/o8";
		public const string IV = "zKSFCHfbnpY=";


		public static void Init(XML xml, ref OracleDB.strConnect strConn, ref AutoUpdateSvr.AutoUpdateServer web)
		{
			string strSvrType = xml.GetSingleNodeValue("SERVER/SERVERTYPE");
			bool use_en = xml.GetSingleNodeValue("USE_ENCRYPTO").Equals("Y");
			xml.chSingleNode("SERVER/" + strSvrType);


			//암호화 이용시 처리
			cryptography CR = new cryptography();
			CR.Key = key;
			CR.IV = IV;
			

			switch (strSvrType)
			{
				case "ORACLE":
					strConn.strTNS = xml.GetSingleNodeValue("TNS");
					if (use_en)
					{
						strConn.strID = CR.Decrypting(xml.GetSingleNodeValue("ID"));
						strConn.strPass = CR.Decrypting(xml.GetSingleNodeValue("PASS"));
					}
					else
					{
						strConn.strID = xml.GetSingleNodeValue("ID"); //CR.Decrypting(xml.GetSingleNodeValue("ID"));
						strConn.strPass = xml.GetSingleNodeValue("PASS"); //CR.Decrypting(xml.GetSingleNodeValue("PASS"));
					}

					break;

				case "WEB":
					web = new AutoUpdateSvr.AutoUpdateServer();
					if (use_en)
						web.Url = CR.Decrypting(xml.GetSingleNodeValue("URL"));
					else
						web.Url = xml.GetSingleNodeValue("URL");
					break;

				default:
					throw new Exception("Server Type을 알 수 없습니다.");
			}
		}




	}	// end class
}
