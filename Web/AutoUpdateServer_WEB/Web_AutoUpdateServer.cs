using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Data;
using System.IO;

namespace AutoUpdateServer_WEB
{
	/// <summary>
	/// Autoupdate Web 서버 <para/>
	/// Updater에서 사용하려면 상속 클래스 명을 'AutoUpdateServer'로 할것
	/// </summary>
	public class Web_AutoUpdateServer : System.Web.Services.WebService
	{
		string _updateDirectoryPath = null;


		static Dictionary<UpdateTypeKey, FileStream> dicUploadStream = new Dictionary<UpdateTypeKey, FileStream>();
		
		/// <summary>
		/// update 폴더 경로를 가져오가나 설정한다.
		/// </summary>
		public string UpdateDirectoryPath
		{
			get { return _updateDirectoryPath; }
			set
			{
				if(Directory.Exists(value))
				{
					_updateDirectoryPath = value;
				}
				else
				{
					throw new Exception("{0} 폴더가 존재 하지 않습니다.");
				}
			}
		}



		/// <summary>
		/// 클래스를 생성한다.
		/// </summary>
		/// <param name="updateDirectoryPath">파일디랙토리를 지정하면 그 아래에 'AutoUpdate' 폴더를 생성한다.</param>
		public Web_AutoUpdateServer(string updateDirectoryPath)
		{
			//if (!Directory.Exists(updateDirectoryPath)) throw new Exception(string.Format("{0} 폴더가 존재 하지 않습니다.", updateDirectoryPath));

			UpdateDirectoryPath = updateDirectoryPath;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="updateType"></param>
		/// <returns></returns>
		public DataSet UpdateType_GetDataSet(string updateType)
		{
			DataSet ds = new DataSet();
			ds.ReadXml(UpdateTypeXmlfilePath(updateType));

			return ds;
        }

		/// <summary>
		/// UpdateType 별 폴더경로를 가져온다.
		/// </summary>
		/// <param name="updateType"></param>
		/// <returns></returns>
		public string UpdateTypeDirectoryPath(string updateType)
		{
			string rtn = UpdateDirectoryPath + "\\AutoUpdate\\" + updateType + "\\"; // + fnc.UpdateTypeXmlFileName;
			fnc.Directory_Create(rtn);
			return rtn;
		}

		/// <summary>
		/// UpdateType 별설정 파일 경로를 가져온다.
		/// </summary>
		/// <param name="updateType"></param>
		/// <returns></returns>
		public string UpdateTypeXmlfilePath(string updateType)
		{
			return UpdateTypeDirectoryPath(updateType) + "\\" + fnc.UpdateTypeXmlFileName;
		}

		/// <summary>
		/// UpdateType 별 임시 폴더경로를 가져온다.
		/// </summary>
		/// <param name="updateType"></param>
		/// <returns></returns>
		public string UpdateTypeTempDirectoryPath(string updateType)
		{
			string rtn = UpdateTypeDirectoryPath(updateType) + "\\" + fnc.TempDiretoryName;
			fnc.Directory_Create(rtn);
			return rtn;
        }

		/// <summary>
		/// UpdateType 별 히스톨 폴더경로를 가져온다.
		/// </summary>
		/// <param name="updateType"></param>
		/// <returns></returns>
		public string UpdateTypeHistoryDirectoryPath(string updateType)
		{
			string rtn = UpdateTypeDirectoryPath(updateType) + "\\" + fnc.HistoyDiretoryName;

			fnc.Directory_Create(rtn);
			return rtn;
		}




		/// <summary>
		/// UpdateDirectoryPath가 등록되어 있는지 확인한다.
		/// </summary>
		void UpdateDirectory_Chk()
		{
			if (UpdateDirectoryPath == null) throw new Exception("UpdateDirectoryPath가 설정되지 안았습니다.");

			fnc.Directory_Create(UpdateDirectoryPath);
		}

		


		/// <summary>
		/// UpdateType의 데이터셑을 요청한다. 
		/// </summary>
		/// <param name="UpdateType"></param>
		/// <returns></returns>
		[WebMethod]
		public DataSet UpdateDataSet_Req(string updateType)
		{
			UpdateDirectory_Chk();

			string file = UpdateTypeXmlfilePath(updateType);
			DataSet ds = null;

			if (File.Exists(file))
			{
				ds = new DataSet();
				ds.ReadXml(file);
			}

			//필드추가는 클라이언트에서 한다.
			//if(ds != null && ds.Tables.Count > 0)
			//{
			//	//201512 추가 컬럼 확인
			//	string[] Add_Cols = new string[] { "ZipFilePath" };
			//	Type[] Add_Types = new Type[] { typeof(System.String) };
			//	object[] dValue = new object[] { string.Empty };


			//	if (fnc.DataTable_ColumnsAdd(ds.Tables[0], Add_Cols, Add_Types, dValue))
			//	{
			//		ds.Tables[0].AcceptChanges();
			//		ds.WriteXml(file, XmlWriteMode.WriteSchema);
			//	}
			//}


			return ds;
		}


		/// <summary>
		/// UpdateType의 데이터셑의 스키마 업로드를 처리 한다.. 
		/// </summary>
		/// <param name="updateType"></param>
		/// <param name="ds"></param>
		[WebMethod]
		public void UpdateDataSet_Schema_Upload(string updateType, DataSet ds)
		{
			UpdateDirectory_Chk();

			string file = UpdateTypeXmlfilePath(updateType);
					


			if (File.Exists(file)) File.Delete(file);

			ds.Tables[0].Rows.Clear();

			ds.WriteXml(file, XmlWriteMode.WriteSchema);

		}


		[WebMethod]
		public void FileDelete(string updateType, string fileName)
		{
			string fldU = UpdateTypeDirectoryPath(updateType);

			DataSet ds = UpdateType_GetDataSet(updateType);

			DataRow[] rows = ds.Tables[0].Select(string.Format("fileName = '{0}'", fileName));

			FileInfo fi;

			foreach(DataRow row in rows)
			{
				fi = new FileInfo(fldU + "\\" + row["filename"].ToString());

				if (fi.Exists) fi.Delete();

				row.Table.Rows.Remove(row);
			}

			ds.WriteXml(UpdateTypeXmlfilePath(updateType), XmlWriteMode.WriteSchema);

		}

		[WebMethod]
		public void ColumnAddedChk(string updateType, string[] Add_Cols, string[] Add_Types, object[] dValue)
		{
			string file = UpdateTypeXmlfilePath(updateType);

			using (DataSet ds = UpdateDataSet_Req(updateType))
			{
				if (fnc.DataTable_ColumnsAdd(ds.Tables[0], Add_Cols, Add_Types, dValue))
				{
					ds.Tables[0].AcceptChanges();
					ds.WriteXml(file, XmlWriteMode.WriteSchema);
				}
			}
		}


		[WebMethod]
		public int FileUpload(string updateType, DataTable dt, byte[] filedata, int filedatalength, long fileLenth, long fileIdx, int key)
		{
			UpdateDirectory_Chk();

			//UpdateType폴더 경로
			string fldU = UpdateTypeDirectoryPath(updateType);
			//임시폴더 경로
			string fldT = UpdateTypeTempDirectoryPath(updateType);
			//히스토리경로
			string fldH	= UpdateTypeHistoryDirectoryPath(updateType);
						
			string crc;

			FileInfo fi;
			FileInfo fileT;

			DataRow[] rows;

			UpdateTypeKey uKey = new UpdateTypeKey(updateType);
			DateTime dtFile;
			FileStream fs;
			

			
			if (fileIdx == 0)
			{

				if (dicUploadStream.ContainsKey(uKey))
				{
					//기존 스트림 정리
					foreach(UpdateTypeKey k in dicUploadStream.Keys)
					{
						if(k.Equals(uKey))
						{
							fs = dicUploadStream[k];
                            if (k.StreamStatus == enStreamStatus.Opened) fnc.FileStream_Close(ref fs);
							k.StreamStatus = enStreamStatus.Closed;
							uKey = k;
						}
					}
				}
				else
				{
					dicUploadStream.Add(uKey, null);
				}

				uKey.ds = UpdateType_GetDataSet(updateType);
				

				//신규/기존 데이터 처리
				uKey.row = uKey.ds.Tables[0].NewRow();
								
				uKey.row.ItemArray = dt.Rows[0].ItemArray;

				rows = uKey.ds.Tables[0].Select(string.Format("UpdateType = '{0}' and FileName = '{1}'", updateType, uKey.row["FileName"]));

				if (rows.Length > 0)
				{
					uKey.row = rows[0];
					uKey.row.ItemArray = dt.Rows[0].ItemArray;
				}
				else
					uKey.ds.Tables[0].Rows.Add(uKey.row);


				if (uKey.row["ZipFilePath"].ToString() == string.Empty)
				{
					uKey.file = fldU + "\\" + uKey.row["filename"].ToString();
					uKey.fileT = fldT + "\\" + uKey.row["filename"].ToString();
				}
				else
				{   //압축파일로 올라옴
					uKey.file = fldU + "\\" + uKey.row["ZipFilePath"].ToString();
					uKey.fileT = fldT + "\\" + uKey.row["ZipFilePath"].ToString();
				}

				uKey.fileT = fnc.TempFile_init(uKey.fileT);

				uKey.Key_GetNew();
				key = uKey.Key;

				fileT = new FileInfo(uKey.fileT);

				//임시 폴더 파일 삭제
				if (fileT.Exists) fileT.Delete();

				if (!Directory.Exists(fileT.DirectoryName)) Directory.CreateDirectory(fileT.DirectoryName);

				dicUploadStream[uKey] = new FileStream(uKey.fileT, FileMode.Create, FileAccess.Write, FileShare.Delete);
			}
			else
			{
				foreach (UpdateTypeKey k in dicUploadStream.Keys)
				{
					if (k.Equals(uKey))
					{							
						uKey = k;
					}
				}

				if (uKey.Key != key)
				{						
					throw new Exception("WEB FileUpload 오류 - Key 값 불일치");
				}
			}

			fs = dicUploadStream[uKey];

			//file stream write
			fs.Write(filedata, 0, filedatalength);
				
			uKey.StreamStatus = enStreamStatus.Opened;

				

			//파일 업로드 종료
			if ((fileIdx + filedata.Length) >= fileLenth)
			{

				//스트립종료
				if (uKey.StreamStatus == enStreamStatus.Opened) fnc.FileStream_Close(ref fs);
				uKey.StreamStatus = enStreamStatus.Closed;


				fi = new FileInfo(uKey.fileT);

				//crc 확인
				crc = fnc.Get_Crc32(uKey.fileT);

				if (!crc.Equals(uKey.row["crc"].ToString()))
				{
					try
					{
						fi.Delete();						
					}
					catch {	}
					finally
					{
						throw new Exception("CRC 불일치");
					}

				}

				dtFile = (DateTime)uKey.row["File_fileDate"];
							

				//update 폴더로 파일 이동
				if (File.Exists(uKey.file)) File.Delete(uKey.file);

				fileT = new FileInfo(uKey.file);

				if (!Directory.Exists(fileT.DirectoryName)) Directory.CreateDirectory(fileT.DirectoryName);

				fi.CopyTo(uKey.file);

				fi = new FileInfo(uKey.file);

				string[] hflds = uKey.row["filename"].ToString().Split('\\');
				fldH += "\\";
                
				for(int hi = 0;hi < hflds.Length -1; hi++)
				{
					//if (hfile != string.Empty) hfile += "\\";
					fldH += hflds[hi] + "\\";
				}

				
				//history 폴더의 파일폴더 확인
				if (!Directory.Exists(fldH)) Directory.CreateDirectory(fldH);

				//history 생성
				uKey.fileH = string.Format("{0}{1}_{2}_{3}.{4}", fldH, fi.Name, dtFile.ToString("yyyyMMddHHmmss"), uKey.row["file_version"], fi.Extension);
				if (File.Exists(uKey.fileH)) File.Delete(uKey.fileH);
				fi.CopyTo(uKey.fileH);


				uKey.row["FileDate"] = uKey.row["File_fileDate"];
				uKey.row["Version"] = uKey.row["File_Version"];



				//dataset 저장
				uKey.ds.WriteXml(UpdateTypeXmlfilePath(updateType), XmlWriteMode.WriteSchema);
			}
            

			return key;
			


		}

		

		[WebMethod]
		public byte[] Update_FileStreamGet(string updateType, string filename)
		{
			try
			{
				string path = UpdateTypeDirectoryPath(updateType);

				byte[] rst;

				using (FileStream fs = new FileStream(path + "\\" + filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{

					rst = new byte[fs.Length];

					fs.Read(rst, 0, int.Parse(fs.Length.ToString()));

					fs.Close();
				}



				return rst;
			}
			catch
			{
				return null;
			}
			finally
			{
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
		}



		[WebMethod]
		public byte[] Update_FileStreamGet2(string updateType, string filename, int index)
		{
			try
			{
				string path = UpdateTypeDirectoryPath(updateType);

				byte[] rst;
				int length = 1024 * 256;

				using (FileStream fs = new FileStream(path + "\\" + filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					if (fs.Length <= (index + length)) length = int.Parse((fs.Length - index).ToString());

					rst = new byte[length];

					fs.Seek(index, SeekOrigin.Begin);
					
					fs.Read(rst, 0, length);

					fs.Close();
				}
				
				return rst;
			}
			catch
			{
				return null;
			}
			finally
			{
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
		}

		[WebMethod]
		public int Update_FileLengthGet(string updateType, string filename)
		{
			string path = UpdateTypeDirectoryPath(updateType);

			FileInfo fi = new FileInfo(path + "\\" + filename);

			return int.Parse(fi.Length.ToString());
			
		}




	}
}
