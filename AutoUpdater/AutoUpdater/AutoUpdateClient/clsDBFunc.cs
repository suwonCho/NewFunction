using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.IO;

/*
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types; // Blob타입 사용을 위함
 */




namespace AutoUpdateClient
{
	class clsDBFunc
	{
		
		/// <summary>
		/// db에 저장된 파일 정보를 조회 한다.
		/// </summary>
		/// <param name="strConn"></param>
		/// <param name="strUpdateType"></param>
		/// <param name="strFileName"></param>
		public static DataSet FileInfo_GetList(OracleDB.strConnect strConn, string strUpdateType)
		{
            OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

            
			OracleParameter[] param = new OracleParameter[] { 
							                    new OracleParameter("ps_UpdateType", OracleDbType.Varchar2, 20),												
                                                new OracleParameter("OUT_CURSOR", OracleDbType.RefCursor)      };


			param[1].Direction = ParameterDirection.Output;
			param[0].Value = strUpdateType;

			return clsDB.dsExcute_StoredProcedure("AutoUpdater_PKG.FileInfo_GetList", param);
		}


		public delegate void delFileInfo_GetFile(string strFileName, int intTotalSize, int intRecevedFileSize);
		/// <summary>
		/// db에 저장된 파일 image를 구한다.
		/// </summary>
		/// <param name="strConn"></param>
		/// <param name="strUpdateType"></param>
		/// <param name="strFileName"></param>
		/// <param name="strPath"></param>
		/// <returns>CRC깂</returns>
        public static string FileInfo_GetFile(OracleDB.strConnect strConn, string strUpdateType, string strFileName, string strPath, 
			int intTotalSize, delFileInfo_GetFile del)
		{
            OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

			OracleParameter[] param = new OracleParameter[] { 
							                    new OracleParameter("ps_UpdateType", OracleDbType.Varchar2, 20),
												new OracleParameter("ps_FileName", OracleDbType.Varchar2, 100),
                                                new OracleParameter("ps_FileImage", OracleDbType.Blob)      };


			param[2].Direction = ParameterDirection.Output;
			param[0].Value = strUpdateType;
			param[1].Value = strFileName;

			clsDB.BeginTransaction();

			clsDB.intExcute_StoredProcedure("AutoUpdater_PKG.FileInfo_GetFileImage", param);

            OracleBlob Lob = (OracleBlob)param[2].Value;

			int blockSize = 15000;
			//임시 디렉토리에 저장 한다.
						
			int intRecevedFileSize = 0;

			FileInfo fi = new FileInfo(strPath + strFileName);

			if (!Directory.Exists(fi.DirectoryName)) Directory.CreateDirectory(fi.DirectoryName);

			using (FileStream fs = new FileStream(strPath + strFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
			{
				using (BinaryWriter Bw = new BinaryWriter(fs))
				{
					try
					{
						byte[] byteArray = new byte[blockSize];
						int bytes;

						while ((bytes = Lob.Read(byteArray, 0, byteArray.Length)) > 0)
						{
							Bw.Write(byteArray, 0, bytes);

							// 여기서 bytes정보를 누적해나가면서, 해당 누적정보를 스레드를 통해
							// 사용자에게 보여주면, 다운로드 진행상황을 표현할 수 있겠다.(해당 구현 사항은 생략)

							intRecevedFileSize += bytes;

							if(del != null)
							{
								del(strFileName, intTotalSize, intRecevedFileSize);
							}
						}

						
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
						
						clsDB.RollBackTransaction();
					}
				}
			}
			
			

			return system.clsFile.Get_Crc32(fi);


		}
		 
		/*

		/// <summary>
		/// db에 저장된 파일 정보를 조회 한다.
		/// </summary>
		/// <param name="strConn"></param>
		/// <param name="strUpdateType"></param>
		/// <param name="strFileName"></param>
		public static DataSet FileInfo_GetList(Oracle.strConnect strConn, string strUpdateType)
		{
			OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

			OracleParameter[] param = new OracleParameter[] { 
							                    new OracleParameter("ps_UpdateType", OracleDbType.Varchar2, 20),												
                                                new OracleParameter("OUT_CURSOR", OracleDbType.RefCursor)      };


			param[1].Direction = ParameterDirection.Output;
			param[0].Value = strUpdateType;

			return clsDB.dsExcute_StoredProcedure("AutoUpdater_PKG.FileInfo_GetList", param);
		}

		/// <summary>
		/// db에 저장된 파일 image를 구한다.
		/// </summary>
		/// <param name="strConn"></param>
		/// <param name="strUpdateType"></param>
		/// <param name="strFileName"></param>
		/// <param name="strPath"></param>
		/// <returns>CRC깂</returns>
		public static string FileInfo_GetFile(Oracle.strConnect strConn, string strUpdateType, string strFileName, string strPath)
		{
			OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

			OracleParameter[] param = new OracleParameter[] { 
							                    new OracleParameter("ps_UpdateType", OracleDbType.Varchar2, 20),
												new OracleParameter("ps_FileName", OracleDbType.Varchar2, 100),
                                                new OracleParameter("ps_FileImage", OracleDbType.Blob)      };


			param[2].Direction = ParameterDirection.Output;
			param[0].Value = strUpdateType;
			param[1].Value = strFileName;

			clsDB.BeginTransaction();

			clsDB.intExcute_StoredProcedure("AutoUpdater_PKG.FileInfo_GetFileImage", param);

			OracleBlob Lob = (OracleBlob)param[2].Value;//new OracleBlob(clsDB.Dbconn);

			int blockSize = 15000;
			//임시 디렉토리에 저장 한다.

			using (FileStream fs = new FileStream(strPath + strFileName, FileMode.Create, FileAccess.Write))
			{
				using (BinaryWriter Bw = new BinaryWriter(fs))
				{
					try
					{
						byte[] byteArray = new byte[blockSize];
						int bytes;

						while ((bytes = Lob.Read(byteArray, 0, byteArray.Length)) > 0)
						{
							Bw.Write(byteArray, 0, bytes);

							// 여기서 bytes정보를 누적해나가면서, 해당 누적정보를 스레드를 통해
							// 사용자에게 보여주면, 다운로드 진행상황을 표현할 수 있겠다.(해당 구현 사항은 생략)
						}


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

						clsDB.RollBackTransaction();
					}
				}
			}

			FileInfo fi = new FileInfo(strPath + strFileName);

			return clsFile.Get_Crc32(fi);


		}
		 * */

	}
}
