using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.ServiceProcess;

namespace prjFileMover
{	
	//파일을 이동시킨다.
	class Program
	{
		static void Main(string[] args)
		{
			string strStartFile = string.Empty;
			string svcName = string.Empty;
			char chrEmpty = Convert.ToChar(15);

			//args = new string[] { @"temp", chrEmpty.ToString(), @"AUTOUPDATER.EXE", "InspectAlarmService" };

			//args = new string[] { @"d:\_Task\2012 Ginseng\02.Net Source\AutoUpdater_KGC\AutoUpdater\AutoUpdateClient\bin\x86\Debug\Temp\",
			//	@"d:\_Task\2012 Ginseng\02.Net Source\AutoUpdater_KGC\AutoUpdater\AutoUpdateClient\bin\x86\Debug\KPGMS\", @"AUTOUPDATER.EXE" };

			//string r1 = Console.ReadLine();

			try
			{
				Thread.Sleep(500);

				string strSoruceFolder = args[0];
				string strTargetFolder = args[1];
				strStartFile = args[2];

				strSoruceFolder = strSoruceFolder.Replace(chrEmpty.ToString(), " ").Trim();
				strTargetFolder = strTargetFolder.Replace(chrEmpty.ToString(), " ").Trim();
				strStartFile = strStartFile.Replace(chrEmpty.ToString(), " ").Trim();

				if(args.Length > 3)
				{
					svcName = args[3].Replace(chrEmpty.ToString(), " ").Trim();
				}


				//원본폴더 확인
				if (!Directory.Exists(strSoruceFolder)) return;

				//대상폴더 확인
				if (Directory.Exists(strTargetFolder))
				{
					Directory.CreateDirectory(strTargetFolder);
				}

				FolderFileMove(strSoruceFolder, strTargetFolder);
				

			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
				string r = Console.ReadLine();
			}
			finally
			{
				if (File.Exists(strStartFile))
				{
					try
					{
						System.Diagnostics.Process pro = new System.Diagnostics.Process();
						pro.StartInfo.FileName = strStartFile;
						pro.Start();
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString());
					}
					
				}

				//서비스를 시작한다
				try
				{
					if (svcName != string.Empty)
					{
						ServiceController service = new ServiceController(svcName);


						if (service.Status != ServiceControllerStatus.Running)
						{
							service.Start();
						}

					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}


			}
		}


		public static void FolderFileMove(string strSourcePath, string strTargetPath)
		{
			if (!FolderExists(strSourcePath)) return;

			//FolderCreate(strTargetPath);

			FileInfo fi;
			FileInfo fiNew;
			string strNewFileName;

			foreach (string strFile in Directory.GetFiles(strSourcePath))
			{
				fi = new FileInfo(strFile);

			
				//그외 파일은 이동처리
				//strNewFileName = strTargetPath + (!strTargetPath.Trim().Equals(String.Empty) && strTargetPath.EndsWith("\\") ? string.Empty : "\\") + fi.Name;

				strNewFileName = strTargetPath + (strTargetPath.Trim().Equals(String.Empty) || strTargetPath.EndsWith("\\") ? string.Empty : "\\") + fi.Name;

				fiNew = new FileInfo(strNewFileName);

				if (!fiNew.Directory.Exists) fiNew.Directory.Create();

				if (fiNew.Exists) fiNew.Delete();

				fi.MoveTo(strNewFileName);
				
			}

			foreach (string strDic in Directory.GetDirectories(strSourcePath))
			{
				DirectoryInfo di = new DirectoryInfo(strDic);
				FolderFileMove(strDic, StringAdd(strTargetPath, di.Name, "\\"));
			}

		}




		/// <summary>
		/// 폴더 존재 유무를 검사한다.
		/// </summary>
		/// <param name="strPath"></param>
		public static bool FolderExists(string strPath)
		{
			return Directory.Exists(strPath);
		}

		/// <summary>
		/// 폴더를 생성 한다.
		/// </summary>
		/// <param name="strPath"></param>
		public static void FolderCreate(string strPath)
		{
			if (!FolderExists(strPath))
			{
				Directory.CreateDirectory(strPath);
			}
		}

		/// <summary>
		/// 폴더에 있는 파일들을 삭제한다.
		/// </summary>
		/// <param name="strPath"></param>
		public static void FolderFileDelete(string strPath)
		{
			if (!FolderExists(strPath)) return;

			foreach (string strFile in Directory.GetFiles(strPath))
			{
				File.Delete(strFile);
			}

			foreach (string dir in Directory.GetDirectories(strPath))
			{
				FolderFileDelete(dir);
				Directory.Delete(dir);
			}

		}

		public static string StringAdd(string strData, string strAddData, string strSpreator)
		{
			if (strData == string.Empty)
				strData = strAddData;
			else
				strData += strSpreator + strAddData;

			return strData;
		}



	}
}
