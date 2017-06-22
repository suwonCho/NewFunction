using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Function;
using Function.form;
using Function.Component;
using System.IO;
using DevExpress.XtraGrid;

namespace AutoUpdater
{
	public partial class frmUploadWindow : frmBaseForm
	{
		/// <summary>
		/// 오라클 연결스트링
		/// </summary>
		Function.Db.OracleDB.strConnect strConn;
		/// <summary>
		/// 리스트 컨텍스트메뉴
		/// </summary>
		ContextMenuStrip cmsFpList;
		/// <summary>
		/// oldversion contextmenu
		/// </summary>
		ContextMenuStrip cmsOldVersion;
		/// <summary>
		/// oldversion menu
		/// </summary>
		ToolStripMenuItem tsiOldVersion;
		/// <summary>
		/// file update type : file (추가예정)xml등등..
		/// </summary>
		clsDBFunc.enUType UType = clsDBFunc.enUType.FILE;


		/// <summary>
		/// WEB Sever 연결 크래스
		/// </summary>
		AutoUpdateServer_Web.AutoUpdateServer web;

		/// <summary>
		/// Zip압축 사용 여부
		/// </summary>
		bool UseZip;


		/// <summary>
		/// 서버 DB Type
		/// </summary>
		enSeverType SvrType;

		/// <summary>
		/// 임시 폴더 경로
		/// </summary>
		string _strTempFld = "Temp\\";


		string strUpdateType = string.Empty;
		string strSaveFileName = string.Empty;

		DataSet dsUpdateType = null;
		DataSet dsSvrUpdateType = null;

		Color colError = Color.Tomato;
		Color colNormal = Color.White;


		public frmUploadWindow(string _strUpdateType, string strBigo)
		{
			InitializeComponent();

			this.Text = string.Format("[{0}] Upload Window", strBigo);

			strUpdateType = _strUpdateType;

			strSaveFileName = string.Format("UpdateType\\{0}.xml", strUpdateType);

			//임시 폴더 생성여부 확인
			Function.system.clsFile.FolderCreate(_strTempFld);

			Function.Component.DevExp.fnc.GridView_ViewInit(gvFileList, true, true, false);

		}

		private void frmUploadWindow_Load(object sender, EventArgs e)
		{
			try
			{
				//setting 정보 조회..
				DataRow[] dr = frmUploader.dsSetting.Tables["Setting"].Select(string.Format("UpdateType = '{0}'", strUpdateType));
				if (dr.Length < 1)
				{
					SetMessage(true, "설정에 저장 되어 있지 않은 UPDATETYPE입니다.", false);
					return;
				}

				//암축파일 사용 여부확인
				UseZip = Fnc.obj2Bool(dr[0]["UseZip"]);

				SvrType = (enSeverType)Fnc.String2Enum(new enSeverType(), Fnc.obj2String(dr[0]["TYPE"]));

				string strBigo = Fnc.obj2String(dr[0]["bigo"]);

				//db 접속 정보 보회
				switch (SvrType)
				{
					case enSeverType.ORACLE:
						dr = frmUploader.dsSetting.Tables["ORACLE"].Select(string.Format("UPDATETYPE = '{0}'", strUpdateType));
						if (dr.Length < 1)
						{
							SetMessage(true, "오라클 설정에 해당 UPDATETYPE이 등록되어 있지 않습니다.", false);
							return;
						}

						btnDB_Init.Visible = true;

						strConn.strTNS = Fnc.obj2String(dr[0]["tns"]);

#if (TEST_DB)
					strConn.strTNS = @"(DESCRIPTION=
	(ADDRESS =
	  (PROTOCOL = TCP)
	  (HOST = 210.100.74.22)
	  (PORT = 17521)
	)
	(CONNECT_DATA =
	  (SERVICE_NAME = KGCSCM)
	)
  )";
#endif

						strConn.strID = Fnc.obj2String(dr[0]["id"]);
						strConn.strPass = Fnc.obj2String(dr[0]["pass"]);

						txtInfo.Text = string.Format("UPDATETYPE:{2}({3})     Zip압축사용:{4}\r\nDB Type : {0}\r\nTNS:{1}", SvrType, strConn.strTNS, strUpdateType, strBigo, UseZip ? "예" : "아니요");
						break;

					case enSeverType.WEB:
						dr = frmUploader.dsSetting.Tables["WEB"].Select(string.Format("UPDATETYPE = '{0}'", strUpdateType));
						if (dr.Length < 1)
						{
							SetMessage(true, "오라클 설정에 해당 UPDATETYPE이 등록되어 있지 않습니다.", false);
							return;
						}


						web = new AutoUpdateServer_Web.AutoUpdateServer();


						web.Url = Fnc.obj2String(dr[0]["URL"]);


						txtInfo.Text = string.Format("UPDATETYPE:{2}({3})     Zip압축사용:{4}\r\nDB Type : {0}\r\nURL:{1}", SvrType, web.Url, strUpdateType, strBigo, UseZip ? "예" : "아니요");
						break;

					default:
						SetMessage(true, "오라클/WEB 이외에는 지원 하지 않습니다. 추후 지원예정.", false);
						return;
				}

				//로컬 설정 파일을 가져온다.
				dsUpdateType = new DataSet();

				if (system.clsFile.FileExists(strSaveFileName))
				{
					dsUpdateType.ReadXml(strSaveFileName, XmlReadMode.Auto);
				}
				else
				{
					//dsUpdateType = clsDBFunc.FileInfo_GetList(strConn, strUpdateType);
					dsUpdateType = new DataSet();
					dsUpdateType.ReadXml(fnc.UpdateTypeSchema_FileName, XmlReadMode.ReadSchema);
				}

				//데이터 초기화
				dsUpdateType_init();


				//201512 추가 컬럼 확인 - UpdateTypeSchema.xml 에도 추가 하여야 한다.
				string[] Add_Cols = new string[] { "ZipFilePath", "CompType" };
				string[] Add_Types = new string[] { "System.String", "System.String" };
				object[] dValue = new object[] { string.Empty, "A" };


				if (Fnc.DataTable_ColumnsAdd(dsUpdateType.Tables[0], Add_Cols, Add_Types, dValue))
				{
					dsUpdateType.Tables[0].AcceptChanges();
					dsUpdateType.WriteXml(strSaveFileName, XmlWriteMode.WriteSchema);
				}

				//웹에 추가 컬럼처리를 하여 준다.
				if (SvrType == enSeverType.WEB)
				{
					web.ColumnAddedChk(strUpdateType, Add_Cols, Add_Types, dValue);
				}



				gcFileList.DataSource = dsUpdateType.Tables[0];

				cmsFpList = new ContextMenuStrip();

				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "mnuAddItem", "항목 추가", Function.resIcon16.add, new EventHandler[] { cmsFpList_Click });
				//control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "", "", null, new EventHandler[] {cmsFpList_Click});
				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "mnuAddFolder", "폴더 추가", Function.resIcon16.folder_white_up, new EventHandler[] { cmsFpList_Click });
				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "", "", null, new EventHandler[] { cmsFpList_Click });
				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "mnuCompAll", "전체 비교", Function.resIcon16.misc_green, new EventHandler[] { cmsFpList_Click });
				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "mnuCompVer", "버젼만 비교", Function.resIcon16.server, new EventHandler[] { cmsFpList_Click });
				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "mnuCompDate", "파일날짜 만 비교", Function.resIcon16.cal, new EventHandler[] { cmsFpList_Click });
				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "", "", null, new EventHandler[] { cmsFpList_Click });
				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "mnuDelItem", "항목삭제(SVR, Local)", Function.resIcon16.delete, new EventHandler[] { cmsFpList_Click });
				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "mnuDeLink", "항목삭제(Local)", Function.resIcon16.delete_alt, new EventHandler[] { cmsFpList_Click });
				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "", "", null, new EventHandler[] { cmsFpList_Click });
				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "mnuAllChecked", "전체 체크", Function.resIcon16.ok, new EventHandler[] { cmsFpList_Click });
				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "mnuAllUnchecked", "전체 체크해제", Function.resIcon16.button_withe, new EventHandler[] { cmsFpList_Click });
				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "", "", null, new EventHandler[] { cmsFpList_Click });
				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "mnuSelChecked", "선택 체크", Function.resIcon16.misc_green, new EventHandler[] { cmsFpList_Click });
				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "mnuSelUnchecked", "선택 체크해제", Function.resIcon16.misc, new EventHandler[] { cmsFpList_Click });
				control.Invoke_ContextMenuStrip_ItemAdd(cmsFpList, "", "", null, new EventHandler[] { cmsFpList_Click });


				cmsFpList.Opening += new CancelEventHandler(cmsFpList_Opening);
				cmsOldVersion = new ContextMenuStrip();
				tsiOldVersion = new ToolStripMenuItem("OLD Version", Function.resIcon16.redo);
				tsiOldVersion.DropDown = cmsOldVersion;
				tsiOldVersion.DropDownOpening += new EventHandler(tsiOldVersion_DropDownOpening);
				cmsFpList.Items.Add(tsiOldVersion);

				gcFileList.ContextMenuStrip = cmsFpList;



				//그리트 포맷 적용 -> 미사용.. RowCellStyle 에서 처리
				//StyleFormatCondition con = new StyleFormatCondition();
				//con.Condition = FormatConditionEnum.Expression;
				//con.Expression = "!(FILEDATE = FILE_FILEDATE AND VERSION = FILE_VERSION)";

				////con.Expression = "FILEDATE = FILE_FILEDATE";

				//con.Appearance.BackColor = Color.LightCoral;
				//con.Appearance.Options.UseBackColor = true;
				//con.ApplyToRow = false;


				//gvFileList.FormatConditions.Add(con);





				List_DataClear();


				//SetMessage(false, "Refresh 버튼을 클릭하여 파일과 서버정보를 확인 하십시요.", false);

				ListCheckAllFile();
			}
			catch(Exception ex)
			{
				ProcException(ex, "Form Load", true);
			}

		}

		/// <summary>
		/// 설정 파일을 초기화 한다.
		/// </summary>
		void dsUpdateType_init()
		{
			foreach (DataRow row in dsUpdateType.Tables[0].Rows)
			{
				row["ISUPLOAD"] = "F";
				row["UPLOAD"] = 0;
			}
		}


		void cmsFpList_Opening(object sender, CancelEventArgs e)
		{

			//if (fpFileList.ActiveSheet.Rows.Count < 1)
			{
				tsiOldVersion.Text = "OLD Version";
				tsiOldVersion.Enabled = false;
				return;
			}

			int intRow = -1; // fpFileList.ActiveSheet.ActiveRowIndex;

			if (intRow < 0)
			{
				tsiOldVersion.Text = "OLD Version";
				tsiOldVersion.Enabled = false;
			}
			else
			{
				//tsiOldVersion.Text = string.Format("[{0}] old ver.", fpFileList.ActiveSheet.Cells[intRow, 1].Text);
				tsiOldVersion.Enabled = true;
			}

		}

		/// <summary>
		/// 콘텍스트 메뉴 링크용 딕셔너리.
		/// </summary>
		Dictionary<string, DataRow> dicOldVersion = new Dictionary<string, DataRow>();

		string strOldVersionFileName = string.Empty;
		DataRow OldVersionRow;

		/// <summary>
		/// 콘텍스트 메뉴에서 oldversion 선택..
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tsiOldVersion_DropDownOpening(object sender, EventArgs e)
		{
			OldVersionRow = gvFileList.GetFocusedDataRow();


			cmsOldVersion.Items.Clear();
			dicOldVersion.Clear();

			//선택된 로우 없음
			if (OldVersionRow == null)
			{
				strOldVersionFileName = string.Empty;
				return;
			}


			string strTemp = Fnc.obj2String(OldVersionRow["FileName"]);

			//파일명이 비어 있음
			if (strTemp == string.Empty)
			{
				strOldVersionFileName = strTemp;
				return;
			}

			strOldVersionFileName = strTemp;

			try
			{
				cmsFpList.Cursor = Cursors.WaitCursor;

				//old version 정보 조회
				using (DataSet ds = clsDBFunc.FileInfo_Get_History(strConn, strUpdateType, UType, strOldVersionFileName))
				{
					if (ds.Tables[0].Rows.Count < 1)
					{
						control.Invoke_ContextMenuStrip_ItemAdd(cmsOldVersion, "NotExist", "대상 없음", Function.resIcon16.trash_empty, null);
					}
					else
					{
						foreach (DataRow dr in ds.Tables[0].Rows)
						{
							string strText = string.Format("[v{0}]-{1}", dr["version"], dr["uploaddate"]);
							string strName = string.Format("v{0}", dr["version"]).Replace(" ", "");
							dicOldVersion.Add(strName, dr);

							control.Invoke_ContextMenuStrip_ItemAdd(cmsOldVersion, strName, strText, Function.resIcon16.unlock, new EventHandler[] { cmsOldVersion_Click });
						}
					}

				}
			}
			catch (Exception ex)
			{
				ProcException(ex, "tsiOldVersion_DropDownOpening");
			}
			finally
			{
				cmsFpList.Cursor = Cursors.Default;
			}

		}

		private void cmsOldVersion_Click(object obj, EventArgs e)
		{
			ToolStripMenuItem mi = (ToolStripMenuItem)obj;
			DataRow dr = dicOldVersion[mi.Name];



			if (Function.clsFunction.ShowMsg("아래 버전으로 DB에 내용을 변경 하시겠습니까?",
				string.Format("[FileName]{0} [Version]{1} [UPLOADDATE]{2}", dr["FILENAME"], dr["Version"], Fnc.Date2String((DateTime)dr["UPLOADDATE"], Fnc.enDateType.DateTime))
				, frmMessage.enMessageType.YesNo) != DialogResult.Yes) return;


			try
			{
				string strMsg = clsDBFunc.FileInfo_Restore_OldVersion(strConn, strUpdateType, UType, Fnc.obj2String(dr["FILENAME"]), Fnc.obj2String(dr["Version"]));
				if (strMsg != string.Empty)
				{
					SetMessage(true, strMsg, false);
				}
				else
				{
					SetMessage(false, string.Format("버젼 원복 : [FileName]{0} [Version]{1} [UPLOADDATE]{2}", dr["FILENAME"], dr["Version"], Fnc.Date2String((DateTime)dr["UPLOADDATE"], Fnc.enDateType.DateTime))
					, false);

					ListSetFile(OldVersionRow);
				}
			}
			catch (Exception ex)
			{
				ProcException(ex, "tsiOldVersion_DropDownOpening");
			}


		}

		/// <summary>
		/// 체크박스와 파일정보(로컬/db) 정보 삭제.
		/// </summary>
		private void List_DataClear()
		{
			//int intRows = fpFileList.ActiveSheet.Rows.Count;
			//if (intRows < 1) return;

			//fpFileList.ActiveSheet.Cells[0, 0, intRows - 1, 0].Value = false;
			//fpFileList.ActiveSheet.Cells[0, 5, intRows - 1, 10].Value = null;
			//fpFileList.ActiveSheet.Cells[0, 5, intRows - 1, 10].BackColor = colNormal;
			//fpFileList.ActiveSheet.Cells[0, 4, intRows - 1, 4].Value = string.Empty;
		}


		/// <summary>
		/// 리스트에 파일정보와 db정보를 확인하여 비교 한다.
		/// </summary>
		private void ListCheckAllFile()
		{
			int intRows = gvFileList.DataRowCount;

			int intRowNum = 1;

			try
			{
				DataRow row;
				List_DataClear();
				ProgressBar_MaxValue(intRows);

				//데이터베이스에 파일이 로컬에 없을경우 추가한다.
				dsSvrUpdateType = fncDB.Server_FilList_Get(SvrType, strUpdateType, strConn, web);

				for (int i = 0; i < intRows; i++)
				{
					SetMessage(false, string.Format("파일 확인중 [{0} / {1}]", i + 1, intRows), false);
					ProgressBar_Value(i + 1);
					gvFileList.FocusedRowHandle = i;

					Application.DoEvents();
					row = gvFileList.GetFocusedDataRow();

					ListSetFile(row);

					row["rownum"] = intRowNum;
					intRowNum++;
				}

				//ds.WriteXml("UpdateTypeSchema.xml", XmlWriteMode.WriteSchema);

				foreach (DataRow dr in dsSvrUpdateType.Tables[0].Rows)
				{

					DataRow[] rows = dsUpdateType.Tables[0].Select(string.Format("fileName='{0}'", dr["FILENAME"]));


					//로컬에 추가
					if (rows.Length < 1)
					{
						SetMessage(false, string.Format("추가 파일 확인[{0}]", dr["FILENAME"]), false);
						//int intR = ListAddRow();

						dr["source_filepath"] = DBNull.Value;
						dr["file_version"] = DBNull.Value;
						dr["file_filedate"] = DBNull.Value;

						ListSetFile(dr, true);


						row = dsUpdateType.Tables[0].NewRow();
						row.ItemArray = dr.ItemArray;

						row["rownum"] = intRowNum;
						intRowNum++;
						dsUpdateType.Tables[0].Rows.Add(row);

					}
				}


				SetMessage(false, "리스트에 파일정보와 db정보를 확인을 완료 하였습니다.", false);

			}
			catch (Exception ex)
			{
				ProcException(ex, "ListCheckAllFile");
			}
			finally
			{
				GC.Collect();
				Application.DoEvents();
				GC.WaitForPendingFinalizers();
			}




		}

		/// <summary>
		/// 해당 Row에 crc를 구한다.
		/// </summary>
		/// <param name="intRow"></param>
		private void GetCrc(int intRow)
		{
			try
			{
				DataRow row = gvFileList.GetDataRow(intRow);
				string strFile = Fnc.obj2String(row["Source_FilePath"]);

				if (!system.clsFile.FileExists(strFile)) return;

				FileInfo fi = new FileInfo(strFile);

				string strCrc = system.clsFile.Get_Crc32(fi);

				row["crc"] = strCrc;

			}
			catch (Exception ex)
			{
				ProcException(ex, "GetCrc");
			}
		}

		/// <summary>
		/// 툴립메뉴 눌렀을 때 이벤트
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="e"></param>
		private void cmsFpList_Click(object obj, EventArgs e)
		{
			ToolStripMenuItem tsi = (ToolStripMenuItem)obj;

			switch (tsi.Text)
			{
				case "항목 추가":
					ListAddItem();
					break;

				case "폴더 추가":
					ListFolderAddItem();
					break;

				case "항목삭제(SVR, Local)":
					ListSelectedRowDelete();
					break;

				case "항목삭제(Local)":
					ListSelectedRowUnlink();
					break;

				case "전체 체크":
					ListAllCheckedCh(true);
					break;

				case "전체 체크해제":
					ListAllCheckedCh(false);
					break;

				case "선택 체크":
					ListSelectedCheckedCh(true);
					break;

				case "선택 체크해제":
					ListSelectedCheckedCh(false);
					break;

				case "전체 비교":
					ListSelectedCompTypeCh("A");
					break;

				case "버젼만 비교":
					ListSelectedCompTypeCh("V");
					break;

				case "파일날짜 만 비교":
					ListSelectedCompTypeCh("D");
					break;

			}

		}

		/// <summary>
		/// 선택된 row들을 비교 방법 변경
		/// </summary>
		private void ListSelectedCompTypeCh(string val)
		{

			int[] irows = gvFileList.GetSelectedRows();


			DataRow[] rows = new DataRow[irows.Length];
			int icnt = 0;

			foreach (int intRow in irows)
			{
				rows[icnt] = gvFileList.GetDataRow(intRow);
				icnt++;
			}


			foreach (DataRow row in rows)
			{
				row["CompType"] = val;
				ListSetFile(row, false);
			}


		}

		/// <summary>
		/// 선택된 row들을 Upload 여부 변경
		/// </summary>
		private void ListSelectedCheckedCh(bool value)
		{

			int[] irows = gvFileList.GetSelectedRows();


			DataRow[] rows = new DataRow[irows.Length];
			int icnt = 0;

			foreach (int intRow in irows)
			{
				rows[icnt] = gvFileList.GetDataRow(intRow);
				icnt++;
			}


			foreach (DataRow row in rows)
			{
				row["isUpload"] = value ? "T" : "F";
			}


		}


		/// <summary>
		/// 전체 row들을 Upload 여부 변경
		/// </summary>
		/// <param name="value"></param>
		private void ListAllCheckedCh(bool value)
		{

			foreach (DataRow row in dsUpdateType.Tables[0].Rows)
			{
				row["isUpload"] = value ? "T" : "F";
			}


		}


		/// <summary>
		/// 선택된 row들을 삭제..
		/// </summary>
		private void ListSelectedRowDelete()
		{

			int[] irows = gvFileList.GetSelectedRows();

			if (Function.clsFunction.ShowMsg("삭제확인", string.Format("선택한 {0}개 파일을 삭제 하시겠습니까?", irows.Length), frmMessage.enMessageType.YesNo) != DialogResult.Yes)
				return;

			DataRow[] rows = new DataRow[irows.Length];
			int icnt = 0;

			foreach (int intRow in irows)
			{
				rows[icnt] = gvFileList.GetDataRow(intRow);
				icnt++;
			}


			foreach (DataRow row in rows)
			{
				ListRowDelete(row);
			}

			SetMessage(false, string.Format("선택된 파일 삭제가 완료 되었습니다.[{0}건]", icnt), false);

			ListCheckAllFile();


		}


		/// <summary>
		/// 선택된 row들을 로컬 등록 해제
		/// </summary>
		private void ListSelectedRowUnlink()
		{

			int[] irows = gvFileList.GetSelectedRows();

			if (Function.clsFunction.ShowMsg("삭제확인", string.Format("선택한 {0}개 파일의 Local 연결을 삭제 하시겠습니까?", irows.Length), frmMessage.enMessageType.YesNo) != DialogResult.Yes)
				return;

			DataRow[] rows = new DataRow[irows.Length];
			int icnt = 0;

			foreach (int intRow in irows)
			{
				rows[icnt] = gvFileList.GetDataRow(intRow);
				icnt++;
			}


			foreach (DataRow row in rows)
			{
				row["SOURCE_FILEPATH"] = string.Empty;
				row["isUpload"] = "F";
			}

			SetMessage(false, string.Format("선택된 파일 Local 연결 삭제가 완료 되었습니다.[{0}건]", icnt), false);

			//ListCheckAllFile();


		}


		/// <summary>
		/// 해당 row 삭제
		/// </summary>
		/// <param name="intRow"></param>
		private void ListRowDelete(DataRow row)
		{

			//먼저 db에 데이터가 있으면 삭제 해준다.
			if (Fnc.obj2String(row["FILEDATE"]) != string.Empty)
			{
				fncDB.Server_FileDelete(SvrType, strUpdateType, row, strConn, web);
			}

			row.Table.Rows.Remove(row);
		}


		/// <summary>
		/// 파일을 Load한다.
		/// </summary>
		private void ListAddItem()
		{
			//다이얼로그 박스를 열어 파일으 선택 하도록 한다.
			string[] strFiles = system.clsFile.FileSelect(string.Empty, string.Empty, true, this);

			if (strFiles == null) return;

			DataRow row;
			bool isAdd = true;

			foreach (string strFile in strFiles)
			{
				FileInfo fi = new FileInfo(strFile);

				isAdd = true;

				//업로드 불가 파일명 체크
				foreach (string fname in fnc.NoUpload_FileName)
				{
					if (fi.Name.ToUpper().Equals(fname.ToUpper()))
					{
						Function.clsFunction.ShowMsg(this, "파일추가오류", string.Format("[파일명]{0}은 관리파일명으로 업로드할수 없습니다.", fname), frmMessage.enMessageType.OK);
						isAdd = false;
						break;
					}
				}

				if (!isAdd) continue;

				DataRow[] rows = dsUpdateType.Tables[0].Select(string.Format("UpdateType = '{0}'", fi.Name));

				if (rows.Length < 1)
				{
					row = dsUpdateType.Tables[0].NewRow();
					row["UpdateType"] = strUpdateType;
					row["UType"] = "File";
					row["FileName"] = fi.Name.ToUpper();
					row["FileName2"] = fi.Name;
					row["Source_FilePath"] = fi.FullName;

					row["CompType"] = "A";

					row["rowNum"] = dsUpdateType.Tables[0].Rows.Count + 1;
					row["DEL"] = "...";
					row["CH_SOURCE_FILEPATH"] = "...";
					row["Upload"] = 0;

					dsUpdateType.Tables[0].Rows.Add(row);
				}
				else
					row = rows[0];



				Application.DoEvents();

				ListSetFile(row);
			}

		}

		/// <summary>
		/// 폴더로 Upload한다.
		/// </summary>
		private void ListFolderAddItem()
		{
			//다이얼로그 박스를 열어 파일으 선택 하도록 한다.
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.ShowNewFolderButton = true;

			DataRow dr = gvFileList.GetFocusedDataRow();

			if (dr != null)
			{
				FileInfo fi = new FileInfo(Fnc.obj2String(dr["Source_FilePath"]));
				fbd.SelectedPath = fi.DirectoryName;
			}


			//폴더선택을 하는 다이얼로그를 연다
			if (fbd.ShowDialog(this) != DialogResult.OK) return;

			DirectoryInfo di = new DirectoryInfo(fbd.SelectedPath);

			foreach (string fname in fnc.NoUpload_FolderName)
			{
				if (di.Name.ToUpper().Equals(fname.ToUpper()))
				{
					Function.clsFunction.ShowMsg(this, "폴더추가오류", string.Format("[폴더명]{0}은 관리폴더명으로 업로드할수 없습니다.", fname), frmMessage.enMessageType.OK);
					return;
				}
			}


			ListFolderAdd(di, di.Name);

		}

		private void ListFolderAdd(DirectoryInfo di, string defaultFolderName)
		{
			DataRow[] rows;
			DataRow row;
			string fname;

			foreach (FileInfo fi in di.GetFiles())
			{
				rows = dsUpdateType.Tables[0].Select(string.Format("UpdateType = '{0}'", fi.Name));

				if (rows.Length < 1)
				{
					fname = string.Format("{0}\\{1}", defaultFolderName, fi.Name);

					row = dsUpdateType.Tables[0].NewRow();
					row["UpdateType"] = strUpdateType;
					row["UType"] = "File";
					row["FileName"] = fname.ToUpper();
					row["FileName2"] = fname;
					row["Source_FilePath"] = fi.FullName;

					row["rowNum"] = dsUpdateType.Tables[0].Rows.Count + 1;
					row["DEL"] = "...";
					row["CH_SOURCE_FILEPATH"] = "...";
					row["Upload"] = 0;

					dsUpdateType.Tables[0].Rows.Add(row);
				}
				else
					row = rows[0];



				Application.DoEvents();

				ListSetFile(row);
			}

			foreach (DirectoryInfo d in di.GetDirectories())
			{
				ListFolderAdd(d, string.Format("{0}\\{1}", defaultFolderName, d.Name));
			}
		}


		/// <summary>
		/// 해당 로우 리스트에 변경여부를 체크여 upload여부 체크
		/// </summary>
		/// <param name="row"></param>
		/// <param name="isAddNew"></param>
		private void ListSetFile(DataRow row, bool isAddNew = false)
		{
			try
			{

				string strFile = Fnc.obj2String(row["FileName"]);
				string strFileName = string.Empty;
				string strFilePath = Fnc.obj2String(row["Source_FilePath"]);
				bool isFileExists = true;
				string comp = Fnc.obj2String(row["comptype"]);


				if (strFilePath != string.Empty && system.clsFile.FileExists(strFilePath))
				{
					FileInfo fi = new FileInfo(strFilePath);

					strFileName = fi.Name;
					if (Fnc.obj2String(row["fileName"]).Equals(string.Empty))
					{
						row["fileName"] = strFileName;
						row["fileName2"] = strFileName.ToUpper();
					}
					row["Source_FilePath"] = strFilePath;

					//원본 버젼
					row["File_Version"] = Fnc.obj2String(system.clsFile.FileGetVersion(strFilePath));
					//원본 날짜
					row["File_FileDate"] = fi.LastWriteTime;
					//원본 crc
					if (UseZip)
						row["crc"] = string.Empty;
					else
						row["crc"] = system.clsFile.Get_Crc32(fi);
					//원본 size
					row["filesize"] = fi.Length;
				}
				else
				{
					strFileName = strFile;
					if (Fnc.obj2String(row["fileName"]).Equals(string.Empty))
					{
						row["fileName"] = strFileName;
						row["fileName2"] = strFileName.ToUpper();
					}

					row["File_Version"] = "파일없음";
					isFileExists = false;
				}

				//서버에 파일과 비교를 한다.
				DataRow[] rows = dsSvrUpdateType.Tables[0].Select(string.Format("UpdateType = '{0}' AND FileName = '{1}'", strUpdateType, strFile.ToUpper()));

				if (rows.Length > 0)
				{
					//db 버젼
					row["Version"] = Fnc.obj2String(rows[0]["VERSION"]);
					//db 날짜
					row["FileDate"] = (DateTime)rows[0]["FILEDATE"];
				}
				else
				{
					row["Version"] = "파일없음";
					row["FileDate"] = DBNull.Value;
				}


				bool isUpload = false;

				if (comp.Equals("A") || comp.Equals("V"))
				{

					//db와 원본 버젼 확인
					if (Fnc.obj2String(row["File_Version"]) != Fnc.obj2String(row["Version"]))
					{
						//fpFileList.ActiveSheet.Cells[intRow, 5, intRow, 6].BackColor = colError;
						isUpload = true;
					}
					else
					{
						//fpFileList.ActiveSheet.Cells[intRow, 5, intRow, 6].BackColor = colNormal;
					}
				}


				if (comp.Equals("A") || comp.Equals("D"))
				{
					//db와 원본 날짜 확인
					if (Fnc.obj2String(row["File_FileDate"]) != Fnc.obj2String(row["FileDate"]))
					{
						//fpFileList.ActiveSheet.Cells[intRow, 7, intRow, 8].BackColor = colError;
						isUpload = true;
					}
					else
					{
						//fpFileList.ActiveSheet.Cells[intRow, 7, intRow, 8].BackColor = colNormal;
					}
				}

				row["IsUpload"] = isFileExists & isUpload ? "T" : "F";






			}
			catch (Exception ex)
			{
				ProcExecption(ex);
			}



		}


		Function.Db.OracleDB.delExcuteProcedure_Progress evtP;
		DataRow ProgressRow;
		private void Upload_Progress(int intMax, int intValue)
		{
			ProgressRow["Upload"] = (intValue * 100) / intMax;

			Application.DoEvents();
		}

		private void FileUpload()
		{
			try
			{
				if (evtP == null)
					evtP = new Function.Db.OracleDB.delExcuteProcedure_Progress(Upload_Progress);

				int intRows = gvFileList.DataRowCount;
				int intCnt = 0;
				int intTotalCnt = 0;
				string msg;

				//체크된 row 확인..
				intTotalCnt = 0;
				DataRow row;

				//정리
				GC.Collect();
				Application.DoEvents();
				GC.WaitForPendingFinalizers();

				//임시폴더 파일은 전부 삭제한다.
				Function.system.clsFile.FolderFileDelete(_strTempFld);

				//압출 파일 클래스
				Function.Archive.ZipStorer zip;

				string zipfileName = string.Empty;

				FileInfo fi;

				FileInfo fiNew;

				for (int idx = 0; idx < gvFileList.RowCount; idx++)
				{
					row = gvFileList.GetDataRow(idx);

					if (Fnc.obj2String(row["IsUpload"]).Equals("T")) intTotalCnt++;
				}



				for (int idx = 0; idx < gvFileList.RowCount; idx++)
				{

					gvFileList.FocusedRowHandle = idx;
					row = gvFileList.GetDataRow(idx);

					if (!Fnc.obj2String(row["IsUpload"]).Equals("T")) continue;

					intCnt++;
					Application.DoEvents();

					if (UseZip)
					{   //압축 사용 Upload

						ProgressRow = row;

						msg = string.Format("[{1}/{0}] {2}을 압축 중 입니다.", intTotalCnt, intCnt, row["FileName"]);
						SetMessage(false, msg, false);
						Application.DoEvents();

						//압축파일 생성
						fi = new FileInfo(Fnc.obj2String(row["Source_FilePath"]));

						zipfileName = string.Format("{0}.zip", row["FileName"]);
						fiNew = new FileInfo(_strTempFld + zipfileName);

						Function.system.clsFile.FolderCreate(fiNew.DirectoryName);

						zip = Function.Archive.ZipStorer.Create(_strTempFld + zipfileName, string.Empty);

						zip.AddFile(Function.Archive.ZipStorer.Compression.Deflate, fi.FullName, fi.Name, string.Empty);
						zip.Dispose();



						row["ZipFilePath"] = zipfileName;
						row["crc"] = system.clsFile.Get_Crc32(fiNew);

						msg = string.Format("[{1}/{0}] {2}을 업로드 합니다.", intTotalCnt, intCnt, row["FileName"]);
						SetMessage(false, msg, false);
						Application.DoEvents();

						fncDB.Server_FileUpload(SvrType, strUpdateType, fiNew, row, evtP, strConn, web);

					}
					else
					{   //압축 미사용 Upload
						msg = string.Format("[{1}/{0}] {2}을 업로드 합니다.", intTotalCnt, intCnt, row["FileName"]);

						SetMessage(false, msg, false);

						Application.DoEvents();
						//Thread.Sleep(50);

						ProgressRow = row;
						row["ZipFilePath"] = string.Empty;


						fi = new FileInfo(Fnc.obj2String(row["Source_FilePath"]));


						fncDB.Server_FileUpload(SvrType, strUpdateType, fi, row, evtP, strConn, web);
					}

				}

				dsUpdateType_init();

				ListCheckAllFile();

				msg = string.Format("{0}건 업로드를 완료 했습니다.", intTotalCnt);
				SetMessage(false, msg, false);

			}
			catch (Exception ex)
			{
				ListCheckAllFile();
				ProcExecption(ex);
			}
			finally
			{
				GC.Collect();
				Application.DoEvents();
				GC.WaitForPendingFinalizers();
			}

		}




		private void ProcExecption(Exception ex)
		{
			SetMessage(true, ex.Message, false);
		}



		#region spread 컨트롤부분

		/// <summary>
		/// list에 목록을 추가한다. -> 로우를 직접 추가 하도록
		/// </summary>
		/// <returns></returns>
		//private int ListAddRow()
		//{
		//	//Spread.Invoke_AddRow(fpFileList, fpFileList.ActiveSheet, -1, 1);

		//	//return fpFileList.ActiveSheet.Rows.Count - 1;
		//}


		//private void fpFileList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
		//{
		//	int intRow = e.Row;

		//	switch(e.Column)
		//	{	
		//              //파일 경로 클릭...
		//		case 3:
		//                  FilePath_Change(false, intRow, string.Empty);
		//			break;

		//		case 11:
		//			if (Function.clsFunction.ShowMsg("항목을 삭제 하시겠습니까?", "", frmMessage.enMessageType.YesNo) != DialogResult.Yes) return;
		//			ListRowDelete(intRow);
		//			break;
		//	}
		//}


		private void FilePath_Change(bool isFolderChange, DataRow row, string strChangeFolder)
		{
			string strPath = Fnc.obj2String(row["Source_FilePath"]);

			string strFileName = Fnc.obj2String(row["FileName"]);

			if (!isFolderChange && strFileName != string.Empty)
			{
				strFileName = strFileName + "|" + strFileName;
			}


			string[] strFiles;

			if (isFolderChange)
				strFiles = new string[] { strChangeFolder + "\\" + strFileName };
			else
				strFiles = system.clsFile.FileSelect(strPath, strFileName, false, this);

			if (strFiles != null)
			{
				strPath = strFiles[0];
				row["Source_FilePath"] = strPath;
				ListSetFile(row);

			}
		}


		private void btnSave_Click(object sender, EventArgs e)
		{
			ListCheckAllFile();
		}

		private void btnUpload_Click(object sender, EventArgs e)
		{
			FileUpload();
		}

		private void frmUploadWindow_FormClosed(object sender, FormClosedEventArgs e)
		{
			dsUpdateType.WriteXml(strSaveFileName, XmlWriteMode.WriteSchema);
		}

		#endregion

		private void btnDB_Init_Click(object sender, EventArgs e)
		{
			try
			{
				if (clsDBFunc.DB_Init_Check(strConn))
				{
					if (Function.clsFunction.ShowMsg(this, "이미 DB에 Autoupdate관련 설정이 되어 있습니다. 다시 설정 하시겠습니까?",
						"다시 설정 시 기존 데이터는 전부 삭제 됩니다.", frmMessage.enMessageType.YesNo) != DialogResult.Yes) return;
				}


				clsDBFunc.DB_Init(strConn);

				SetMessage(false, "DB 설정이 완료 되었습니다.", false);

			}
			catch (Exception ex)
			{
				ProcException(ex, "btnDB_Init_Click");
			}
		}


		/// <summary>
		/// 전체 폴더 변경....
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				FolderBrowserDialog fd = new FolderBrowserDialog();
				fd.RootFolder = Environment.SpecialFolder.MyComputer;

				if (fd.ShowDialog()
					!= DialogResult.OK) return;

				foreach (DataRow row in dsUpdateType.Tables[0].Rows)
				{
					FilePath_Change(true, row, fd.SelectedPath);
				}



			}
			catch (System.Exception ex)
			{
				ProcException(ex, "button1_Click");
			}

		}


		/// <summary>
		/// 그리드 셀클릭 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void gvFileList_Click(object sender, EventArgs e)
		{
			DataRow row = gvFileList.GetFocusedDataRow();

			int col = gvFileList.FocusedColumn.VisibleIndex;
			int irow = gvFileList.FocusedRowHandle;

			switch (col)
			{
				//isUpload
				case 1:
					row["isUpload"] = Fnc.obj2String(row["isUpload"]) != "T" ? "T" : "F";
					break;


				//CH_SOURCE_FILEPATH
				case 4:
					FilePath_Change(false, row, string.Empty);
					break;


				//file delete
				case 12:
					if (Function.clsFunction.ShowMsg("삭제확인", string.Format("{0} 파일을 삭제 하시겠습니까?", row["FILENAME"]), frmMessage.enMessageType.YesNo) == DialogResult.Yes)
					{
						ListRowDelete(row);

						ListCheckAllFile();

					}

					break;

			}


		}


		public void DrawCellBorder(DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
		{
			Brush brush = Brushes.LightPink;
			e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.X - 1, e.Bounds.Y - 1, e.Bounds.Width + 2, 2));
			e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.Right - 1, e.Bounds.Y - 1, 2, e.Bounds.Height + 2));
			e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.X - 1, e.Bounds.Bottom - 1, e.Bounds.Width + 2, 2));
			e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.X - 1, e.Bounds.Y - 1, 2, e.Bounds.Height + 2));
		}

		private void gvFileList_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
		{
			try
			{
				DevExpress.XtraGrid.Views.Grid.GridView gv = sender as DevExpress.XtraGrid.Views.Grid.GridView;
				DataRow r = gv.GetDataRow(e.RowHandle);

				//con.Expression = "!(FILEDATE = FILE_FILEDATE AND VERSION = FILE_VERSION)";
				string comp = Fnc.obj2String(r["comptype"]);
				bool isUp = false;

				if (comp.Equals("A") || comp.Equals("F"))
				{
					isUp = !Fnc.obj2String(r["FILEDATE"]).Equals(Fnc.obj2String(r["FILE_FILEDATE"]));
				}

				if (!isUp && (comp.Equals("A") || comp.Equals("V")))
				{
					isUp = !Fnc.obj2String(r["VERSION"]).Equals(Fnc.obj2String(r["FILE_VERSION"]));
				}

				if (isUp) e.Appearance.BackColor = Color.LightCoral;

			}
			catch
			{

			}


		}
	}
}