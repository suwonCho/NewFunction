using Function;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoUpdater
{
	class fncDB
	{
			
		/// <summary>
		/// 서버에 파일 등록 리스트를 가져온다.
		/// </summary>
		/// <param name="svrType"></param>
		/// <param name="strUpdateType"></param>
		/// <param name="strConn"></param>
		/// <param name="web"></param>
		/// <returns></returns>
		public static DataSet Server_FilList_Get(enSeverType svrType, string strUpdateType, 
			Function.Db.OracleDB.strConnect strConn, AutoUpdateServer_Web.AutoUpdateServer web)
		{
			DataSet ds = null;
			
			switch(svrType)
			{
				case enSeverType.ORACLE:
					ds = clsDBFunc.FileInfo_GetList(strConn, strUpdateType);
					break;

				case enSeverType.WEB:
					
					ds = web.UpdateDataSet_Req(strUpdateType);

					if(ds== null)
					{
						ds = new DataSet();
						ds.ReadXml(fnc.UpdateTypeSchema_FileName);

						web.UpdateDataSet_Schema_Upload(strUpdateType, ds);
						
					}

					break;
            }			

			return ds;
		}


		public static void Server_FileUpload(enSeverType svrType, string strUpdateType, FileInfo fi, DataRow row, Function.Db.OracleDB.delExcuteProcedure_Progress evt,
			Function.Db.OracleDB.strConnect strConn, AutoUpdateServer_Web.AutoUpdateServer web)
		{

			switch (svrType)
			{
				case enSeverType.ORACLE:
					clsDBFunc.FileInfo_Save(strConn, clsDBFunc.enUType.FILE,
						strUpdateType, fi, Fnc.obj2String(row["crc"]), string.Empty, evt);
					break;

				case enSeverType.WEB:


					FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
					BinaryReader br = new BinaryReader(fs);
					int intBlockSize = 1024 * 256;   //256kb

					try
					{
						byte[] bytes = new byte[intBlockSize];

						int intBytes;
						int intTotalBytes = 0;
						int key = -1;

						DataTable dt = row.Table.Clone();
						DataRow dr = dt.NewRow();
						dr.ItemArray = row.ItemArray;
						dt.Rows.Add(dr);						

						while ((intBytes = br.Read(bytes, 0, bytes.Length)) > 0)
						{

							key = web.FileUpload(strUpdateType, dt, bytes, intBytes, fs.Length, intTotalBytes, key);

							intTotalBytes += intBytes;

							if (evt != null)
								evt(int.Parse(fi.Length.ToString()), intTotalBytes);
						}


						dt.Dispose();
					}
					 catch
					{
						throw;
					}
					finally
					{
						br.Close();
						fs.Close();

						fs.Dispose();
					}
									
                    break;
			}
		}







		public static void Server_FileDelete(enSeverType svrType, string strUpdateType, DataRow row, 
			Function.Db.OracleDB.strConnect strConn, AutoUpdateServer_Web.AutoUpdateServer web)
		{
			string strFileName = Fnc.obj2String(row["FILENAME"]);


			switch (svrType)
			{
				case enSeverType.ORACLE:					
					clsDBFunc.FileInfo_DeleteFile(strConn, strUpdateType, clsDBFunc.enUType.FILE.ToString(), strFileName.ToUpper());
					break;

				case enSeverType.WEB:
					web.FileDelete(strUpdateType, strFileName);
					break;
			}
		}



	}
}
