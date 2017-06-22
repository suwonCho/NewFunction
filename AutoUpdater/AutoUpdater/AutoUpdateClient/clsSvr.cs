using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.IO;

namespace AutoUpdateClient
{
	class clsSvr
	{

		/// <summary>
		/// 서버에 파일 목록을 가져온다.
		/// </summary>
		/// <param name="svrType"></param>
		/// <param name="updateType"></param>
		/// <param name="strConn"></param>
		/// <param name="web"></param>
		/// <returns></returns>
		public static DataSet File_GetList(string svrType, string updateType, OracleDB.strConnect strConn, AutoUpdateSvr.AutoUpdateServer web)
		{
			DataSet ds = null;

			switch(svrType)
            {
				case "ORACLE":
					ds = clsDBFunc.FileInfo_GetList(strConn, updateType);
                    break;

				case "WEB":
					ds = web.UpdateDataSet_Req(updateType);
					break;					
					
			}


			return ds;
		}


		public static string FileInfo_GetFile(string svrType, string UpdateType, string FileName, string Path,
			int intTotalSize, clsDBFunc.delFileInfo_GetFile del,OracleDB.strConnect strConn, AutoUpdateSvr.AutoUpdateServer web)
		{
			string strFileCrc = string.Empty;
            switch (svrType)
			{
				case "ORACLE":
					strFileCrc = clsDBFunc.FileInfo_GetFile(strConn, UpdateType, FileName,	Path, intTotalSize, del);
					break;

				case "WEB":
					byte[] data;
					int idx = 0;
					int length = web.Update_FileLengthGet(UpdateType, FileName);
					bool isFin = false;

					FileInfo fi = new FileInfo(Path + FileName);

					if (!Directory.Exists(fi.DirectoryName)) Directory.CreateDirectory(fi.DirectoryName);

					del(FileName, length, 0 );


					using (FileStream fs = new FileStream(Path + FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
					{
						using (BinaryWriter Bw = new BinaryWriter(fs))
						{
							try
							{
								while (!isFin)
								{
									data = web.Update_FileStreamGet2(UpdateType, FileName, idx);

									Bw.Write(data, 0, data.Length);

									idx += data.Length;

									if(data.Length < 1 || data.Length != (1024 * 256))
									{
										isFin = true;
										continue;
									}


									if (del != null)
									{
										del(FileName, length , idx);
									}
								}


								del(FileName, length, length);

							}
							catch
							{
								throw;
							}
							finally
							{
								Bw.Flush();
								fs.Flush();

								Bw.Close();
								fs.Close();

								fs.Dispose();
								
							}
						}
					}
					
					strFileCrc = system.clsFile.Get_Crc32(fi);


					break;

				default:
					throw new Exception("Server Type을 알 수 없습니다.");

			}


			return strFileCrc;
        }



		public static void Web_GetFile()
		{

		}


	}		//end class





}
