
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoUpdateClient
{
	/// <summary>
	/// 업데이트 여부 확인 클래스
	/// </summary>
	public class clsUpdateChecker
	{
		string strXmlPath = @"\UpdateClient.xml";
		
		/// <summary>
		/// 업데이트 타입..
		/// </summary>
		string strUpdateType;
		/// <summary>
		/// 대상 폴더(업데이트한 파일을 이동할 폴더)
		/// </summary>
		string strTargetPath;


		/// <summary>
		/// 서버 타입 : ORACLE, WEB
		/// </summary>
		string strSvrType = string.Empty;

		/// <summary>
		/// 오라클 연결 스트링
		/// </summary>
		OracleDB.strConnect strConn;

		AutoUpdateSvr.AutoUpdateServer web;


		XML xml;


		public clsUpdateChecker()
		{
			Init();
		}

		public void Init()
		{
			string path = Application.StartupPath +  strXmlPath;

			xml = new XML(XML.enXmlType.File, path);

			//업데이트 타입을 저장
			strUpdateType = xml.GetSingleNodeValue("UPDATETYPE");
			bool use_en = xml.GetSingleNodeValue("USE_ENCRYPTO").Equals("Y");

			vari.Init(xml, ref strConn,ref web);

			xml.chNode2Root();
			strSvrType = xml.GetSingleNodeValue("SERVER/SERVERTYPE");

			//cryptography CR = new cryptography();
			//CR.Key = key;
			//CR.IV = IV;


			//switch (strSvrType)
			//{
			//	case "ORACLE":
			//		strConn.strTNS = xml.GetSingleNodeValue("TNS");
					
			//		if (use_en)
			//		{
			//			strConn.strID = CR.Decrypting(xml.GetSingleNodeValue("ID"));
			//			strConn.strPass = CR.Decrypting(xml.GetSingleNodeValue("PASS"));
			//		}
			//		else
			//		{
			//			strConn.strID = xml.GetSingleNodeValue("ID"); //CR.Decrypting(xml.GetSingleNodeValue("ID"));
			//			strConn.strPass = xml.GetSingleNodeValue("PASS"); //CR.Decrypting(xml.GetSingleNodeValue("PASS"));
			//		}


			//		break;

			//	case "WEB":
			//		web = new AutoUpdateSvr.AutoUpdateServer();
			//		web.Url = xml.GetSingleNodeValue("URL");
			//		break;

			//	default:
			//		throw new Exception("Server Type을 알 수 없습니다.");
			//}
		}


		public bool UpdateCheck()
		{
			try
			{
				bool isUpdate = false;

				using (DataSet ds = clsSvr.File_GetList(strSvrType, strUpdateType, strConn, web))
				{
					int intUpdateTotalCount = ds.Tables[0].Rows.Count;
					int intUpdateCount = 0;
					

					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						intUpdateCount++;
						
						string strFileName = Application.StartupPath + @"\" + Fnc.obj2String(dr["FileName"]);
						string strTempFileName;
						string strVersion = Fnc.obj2String(dr["Version"]);
						DateTime dtFileDate = (DateTime)dr["FileDate"];
						string strCrc = Fnc.obj2String(dr["CRC"]);
						int intFileSize = Convert.ToInt32(dr["FileSize"]);
						

						string strFileVersion = string.Empty;
						DateTime dtFileFileDate = DateTime.Now;
						TimeSpan tspan;

						isUpdate = false;

						

						if (system.clsFile.FileExists(strFileName))
						{
							System.IO.FileInfo fi = new System.IO.FileInfo(strFileName);

							strFileVersion = Fnc.obj2String(system.clsFile.FileGetVersion(fi.FullName));
							dtFileFileDate = fi.LastWriteTime;

							tspan = dtFileFileDate - dtFileDate;

							if (strFileVersion != strVersion || tspan.TotalSeconds > 30 || tspan.TotalSeconds < -30)
							{
								isUpdate = true;
								break;							
							}

						}
						else
						{
							//파일이 없음
							isUpdate = true;
							break;				
						}					

					}
				}

				return isUpdate;

			}
			catch
			{
				//ProcExecption(ex);
				throw;
			}
			finally
			{
				//this.Close();
			}
		}
	}
}
