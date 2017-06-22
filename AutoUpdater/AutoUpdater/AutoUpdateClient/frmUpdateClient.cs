using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.ServiceProcess;

namespace AutoUpdateClient
{
	public partial class frmUpdateClient : Form
	{
		/// <summary>
		/// 오라클 연결 스트링
		/// </summary>
		OracleDB.strConnect strConn;

		AutoUpdateSvr.AutoUpdateServer web;

		Log clsLog;

		string strXmlPath = @"\UpdateClient.xml";
		string strBackgroundImgae_Path = @"\update.jpg";
		string strLogoImgae_Path = @"\logo.bmp";
		string strICON_Path = @"\icon.ico";
		XML xml;

		/// <summary>
		/// 서버 타입 : ORACLE, WEB
		/// </summary>
		string strSvrType = string.Empty;

		/// <summary>
		/// 업데이트 후 타겟폴더에서 실행할 파일 
		/// </summary>
		string strSTARTUPFILE;
		/// <summary>
		/// 업데이트 타입..
		/// </summary>
		string strUpdateType;
		/// <summary>
		/// 대상 폴더(업데이트한 파일을 이동할 폴더)
		/// </summary>
		string strTargetPath;
		/// <summary>
		/// 업데이트 파일 저장 폴더..
		/// </summary>
		string strPathUpdateTemp;

		char chrEmpty = Convert.ToChar(15);
		clsDBFunc.delFileInfo_GetFile evtReceiveFile;

		Thread thStartUp;
		/// <summary>
		/// 프로그램 시작 폴더
		/// </summary>
		string aPath;

		/// <summary>
		/// 201607추가 서비스 이름, client에서 서비스를 정지시키고, update 후에 mover에서 시작 한다.
		/// </summary>
		string svcName;

		

		public frmUpdateClient()
		{
			InitializeComponent();
			try
			{
				aPath = Application.StartupPath;

				clsLog = new Log(aPath + @"\UpdateClient\", "UpdateClient", 30, true);
				xml = new XML(XML.enXmlType.File, aPath + strXmlPath);

				try
				{
					strBackgroundImgae_Path = aPath + @"\" + xml.GetSingleNodeValue("BackgroundImgae_Path");
					strLogoImgae_Path = aPath + @"\" + xml.GetSingleNodeValue("LogoImgae_Path");
					strICON_Path = aPath + @"\" + xml.GetSingleNodeValue("ICON_Path");
				}
				catch
				{
				}

				if (system.clsFile.FileExists(strICON_Path))
				{
					this.Icon = new Icon(strICON_Path);
				}

				if (system.clsFile.FileExists(strBackgroundImgae_Path))
				{
					Image img = new Bitmap(strBackgroundImgae_Path, false);
					this.BackgroundImage = img;
				}


				if (system.clsFile.FileExists(strLogoImgae_Path))
				{
					picLogo.Load(strLogoImgae_Path);
					picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
				}
				else
				{
					picLogo.Visible = false;
				}

				//서비스 이름이 등록 되어 있으면 정시 시킨다.
				try
				{
					svcName = xml.GetSingleNodeValue("ServiceName").Trim();
				}
				catch
				{
					svcName = string.Empty;
				}

				strPathUpdateTemp = aPath + @"\Temp\";
				system.clsFile.FolderCreate(strPathUpdateTemp);

				evtReceiveFile = new clsDBFunc.delFileInfo_GetFile(FileDownloading);

				this.Opacity = 0;
				control.Invoke_Control_Text(lblVersion, "V." + Application.ProductVersion);
				clsLog.WLog("업데이트 프로그램을 실행합니다.\tv" + Application.ProductVersion);
			}
			catch (Exception ex)
			{
				clsLog.WLog_Exception("frmUpdateClient", ex);
			}

			

		}


		private void StartUp()
		{
			try
			{

				for (int i = 1; i < 11; i++)
				{
					double dbl = i * 0.1;

					control.Invoke_Form_Opacity(this, dbl);

					Thread.Sleep(150);
					Application.DoEvents();
				}
								
				DoEvent(true, "Config파일 읽는 중..");



				
				//업데이트 타입을 저장
				strUpdateType = xml.GetSingleNodeValue("UPDATETYPE");
				//대상폴더 지정
				strTargetPath = aPath + @"\" + xml.GetSingleNodeValue("TARGETFOLDER");

				if (strTargetPath != string.Empty && !strTargetPath.EndsWith("\\")) strTargetPath += @"\";

#if (DEBUG)
				//strTargetPath = @"c:\Program Files (x86)\KPGMS\";
				//strPathUpdateTemp = @"c:\Program Files (x86)\KPGMS\Temp\";
#endif



				if (svcName != string.Empty)
				{					
					ServiceController service = new ServiceController(svcName);

					try
					{

						if (service.Status == ServiceControllerStatus.Running)
						{
							DoEvent(true, string.Format("서비스 [{0}] 중지 중입니다.", svcName));
							service.Stop();
							service.WaitForStatus(ServiceControllerStatus.Stopped);
						}
					}
					catch
					{

					}
				}



				strSTARTUPFILE = xml.GetSingleNodeValue("STARTUPFILE");

				strSvrType = xml.GetSingleNodeValue("SERVER/SERVERTYPE");

				bool use_en = xml.GetSingleNodeValue("USE_ENCRYPTO").Equals("Y");

				vari.Init(xml, ref strConn, ref web);


				xml.chNode2Root();

				//프로새스 종료 여부
				string strKillProcess = xml.GetSingleNodeValue("KILLPROCESS");

				if(strKillProcess != "N")
				{
					string[] fi = strSTARTUPFILE.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

					System.Diagnostics.Process[] s = System.Diagnostics.Process.GetProcessesByName(fi[0]);

					foreach (System.Diagnostics.Process p in s)
					{				
						p.Kill();
						p.WaitForExit();
					}

					Application.DoEvents();


					/*
					s = System.Diagnostics.Process.GetProcesses();

					//if (s.Length > 0) s[0].Kill();

					foreach(System.Diagnostics.Process p in s)
					{
						Console.WriteLine(p.ProcessName);

						if(p.ProcessName.ToUpper().Equals(strSTARTUPFILE.ToUpper()))
						{
							p.Kill();
						}
					}

					*/

				}




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
				//		if(use_en)
				//			web.Url = CR.Decrypting(xml.GetSingleNodeValue("URL"));
				//		else
				//			web.Url = xml.GetSingleNodeValue("URL");
				//		break;

				//	default:
				//		throw new Exception("Server Type을 알 수 없습니다.");
				//}
				
				
				xml.chSingleNode("SERVER/" + strSvrType);


				DoEvent(true, "임시폴더 파일을 삭제합니다.");
				//temp폴더에 있는 파일을 삭제한다.
				system.clsFile.FolderFileDelete(strPathUpdateTemp);

				//파일을 다운로드 받는다.
				FileDownload();

				DoEvent(true, "업데이트 파일을 이동합니다.");
				//temp폴데에 있는 파일을 이동시킨다. -> 파일무버에서 처리 한다.
				//Function.system.clsFile.FolderFileMove(strPathUpdateTemp, strTargetPath);

			}
			catch(Exception ex)
			{
				clsLog.WLog_Exception("StartUp", ex);
			}
			finally
			{
				DoEvent(true, "업데이트가 완료되었습니다.");
				clsLog.WLog("업데이트 프로그램을 종료합니다.");

				try
				{

					strPathUpdateTemp = strPathUpdateTemp.Replace(" ", chrEmpty.ToString());
					strTargetPath = strTargetPath.Replace(" ", chrEmpty.ToString());
					strSTARTUPFILE = strSTARTUPFILE.Replace(" ", chrEmpty.ToString());
					svcName = svcName.Replace(" ", chrEmpty.ToString());
					strSTARTUPFILE = Fnc.StringAdd(strTargetPath, strSTARTUPFILE, "\\");
					if (strTargetPath.Trim().Equals(string.Empty)) strTargetPath = chrEmpty.ToString();


					System.Diagnostics.Process pro = new System.Diagnostics.Process();
					pro.StartInfo.FileName = aPath + @"\FileMover.exe";
					//pro.StartInfo.Arguments = string.Format(@"""{0}"" """"{1}"" """"{2}""", strPathUpdate, strPath , strSTARTUPFILE);
					pro.StartInfo.Arguments = string.Format(@"{0} {1} {2} {3}", strPathUpdateTemp, strTargetPath, strSTARTUPFILE, svcName);
					pro.Start();
				}
				catch(Exception ex)
				{
					clsLog.WLog_Exception("FileMove", ex);
				}

				control.Invoke_Form_Close(this);
			}
		}


		private void DoEvent(bool isTotal, string strMsg)
		{
			if(isTotal)
				control.Invoke_Control_Text(lblTotalInfo, strMsg);
			else
				control.Invoke_Control_Text(lblSubInfo, strMsg);


			Application.DoEvents();
			Thread.Sleep(50);
		}


		private void FileDownload()
		{
			try
			{
				DoEvent(true, "서버에서 업데이트 내역을 확인합니다");

				using (DataSet ds = clsSvr.File_GetList(strSvrType, strUpdateType, strConn, web)) 
				{
					int intUpdateTotalCount = ds.Tables[0].Rows.Count;
					int intUpdateCount = 0;

					DoEvent(true, string.Format("업데이트 항목확인 {0}건 - 파일 다운로드 시작", intUpdateTotalCount));
					
					
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						intUpdateCount++;
						control.Invoke_Control_Text(lblSubCount, string.Format("{0:D2}/{1:D2}", intUpdateCount, intUpdateTotalCount));
						control.Invoke_Control_Text(lblTotal, string.Format("{0:D0}%", 100 * intUpdateCount / intUpdateTotalCount  ));
						control.Invoke_ProgressBar_Value(pBarTotal, intUpdateTotalCount, intUpdateCount);

						string strFileName = strTargetPath + Fnc.obj2String(dr["FileName"]);
						string strTempFileName;
						string strVersion = Fnc.obj2String(dr["Version"]);
						DateTime dtFileDate = (DateTime)dr["FileDate"];
						string strCrc = Fnc.obj2String(dr["CRC"]);
						int intFileSize = Convert.ToInt32(dr["FileSize"]);
						bool isUpdate = false;
						
						string strFileVersion = string.Empty;
						DateTime dtFileFileDate = DateTime.Now;
						TimeSpan tspan;

						isUpdate = false;

						DoEvent(false, string.Format("파일[{0}]을 확인 합니다.",dr["FileName"]));

						if (system.clsFile.FileExists(strFileName))
						{
							System.IO.FileInfo fi = new System.IO.FileInfo(strFileName);

							strFileVersion = Fnc.obj2String(system.clsFile.FileGetVersion(fi.FullName));
							dtFileFileDate = fi.LastWriteTime;

							tspan = dtFileFileDate - dtFileDate;

							if (strFileVersion != strVersion || tspan.TotalSeconds > 30 || tspan.TotalSeconds < -30)
							{
								isUpdate = true;
								clsLog.WLog(string.Format("파일[{0}]을 업데이트 합니다. - VERSION[{1}] FILEDATE[{2}] -> VERSION[{3}] FILEDATE[{4}]",
									dr["FileName"], strVersion, dtFileDate, strFileVersion, dtFileFileDate));
							}

						}
						else
						{
							isUpdate = true;

							clsLog.WLog(string.Format("파일[{0}]을 다운로드 받습니다. - VERSION[{1}] FILEDATE[{2}]",
									dr["FileName"], strVersion, dtFileDate));
						}


						if(!isUpdate)
						{
							clsLog.WLog(string.Format("파일[{0}]은 업데이트 필요 없음- VERSION[{1}] FILEDATE[{2}]", 
									dr["FileName"], strVersion, dtFileDate));
						}

						int intErrCnt = 0;
						string strFileCrc = string.Empty;
						string field = "FileName";
						bool isZipFile = false;

						while (isUpdate)
						{

							if (ds.Tables[0].Columns.Contains("ZipFilePath") && !Fnc.obj2String(dr["ZipFilePath"]).Equals(string.Empty))
							{
								field = "ZipFilePath";
								isZipFile = true;
							}
							else
							{
								field = "FileName";
								isZipFile = false;
							}

							strTempFileName  = strPathUpdateTemp + Fnc.obj2String(dr[field]);

							strFileCrc = clsSvr.FileInfo_GetFile(strSvrType, strUpdateType, Fnc.obj2String(dr[field]).ToUpper(),
									strPathUpdateTemp, intFileSize, evtReceiveFile, strConn, web);

							GC.Collect();
							Application.DoEvents();
							GC.WaitForPendingFinalizers();

							//crc검사
							if (strCrc != strFileCrc)
							{
								intErrCnt++;

								system.clsFile.FileDelete(strPathUpdateTemp + Fnc.obj2String(dr[field]));

								//3회까지 시도한다.
								if (intErrCnt > 2)
									isUpdate = false;
								else
								{
									isUpdate = true;
								}
							}
							else
							{
								System.IO.FileInfo fi = new System.IO.FileInfo(strTempFileName);
								
								if (isZipFile)
								{   //zip 파일은 압축을 풀어주고

									System.IO.FileInfo newfi = new System.IO.FileInfo(strTempFileName);

									DoEvent(false, string.Format("파일[{0}]을 압축을 해제 합니다.", dr["FileName"]));

									ZipStorer zip;
									List<ZipStorer.ZipFileEntry> lstZip;

									zip = ZipStorer.Open(fi.FullName, FileAccess.Read);
									lstZip = zip.ReadCentralDir();

									foreach (ZipStorer.ZipFileEntry z in lstZip)
									{
										string strNewFileName = fi.DirectoryName + "\\" + z.FilenameInZip;
										zip.ExtractFile(z, strNewFileName);

										newfi = new FileInfo(strNewFileName);
                                    }

									zip.Dispose();

									GC.Collect();
									Application.DoEvents();
									GC.WaitForPendingFinalizers();

									fi.Delete();

									newfi.LastWriteTime = dtFileDate;
									newfi.CreationTime = dtFileDate;


								}
								else
								{	//파일은 마지막 파일날짜를 변경 하여 준다
									
									fi.LastWriteTime = dtFileDate;
									fi.CreationTime = dtFileDate;
								}

								isUpdate = false;

							}

						}

					}
				}

				DoEvent(true, "서버에서 파일 다운로드가 완료 되었습니다.");

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

		private void FileDownloading(string strFileName, int intTotalSize, int intRecevedFileSize)
		{
			control.Invoke_Control_Text(lblSubInfo, string.Format("{0} {1}/{2}", strFileName, intRecevedFileSize, intTotalSize));
			control.Invoke_ProgressBar_Value(pBarSub, 100 , 100 * intRecevedFileSize / intTotalSize);
			control.Invoke_Control_Text(lblSub, string.Format("{0:D0}%", 100 * intRecevedFileSize/intTotalSize));

			Application.DoEvents();

		}

		private void ProcExecption(Exception ex)
		{
			Fnc.ShowMsg(ex.Message, string.Empty, frmMessage.enMessageType.OK);
		}


		private void frmUpdateClient_Load(object sender, EventArgs e)
		{

			thStartUp = new Thread(new ThreadStart(StartUp));
			thStartUp.IsBackground = true;
			thStartUp.Start();
		}
	}
}