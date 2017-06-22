using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Function;
using Function.form;
using Function.Util;

namespace AutoUpdater
{
	public partial class frmSetting : Function.form.frmBaseForm
	{
		/// <summary>
		/// 새로운 로우 여부
		/// </summary>
		bool isNewRow = false;

		/// <summary>
		/// 새로운 로추 추가됨
		/// </summary>
		bool isNewRowAdded = false;

		DataSet dsSetting;

		DataRow _currDr = null;

		/// <summary>
		/// 그룹 컬러
		/// </summary>
		Color[] groupColors = new Color[] { Color.White, Color.LightGreen, Color.LightGoldenrodYellow, Color.WhiteSmoke, Color.Beige };


		public frmSetting()
		{
			InitializeComponent();
		}

		private void DataSet_Save()
		{			
			dsSetting.WriteXml(frmUploader.strDSFileName, XmlWriteMode.WriteSchema);
		}
		
		private void InputBox_Collapes()
		{

			//if (txtUpdateType.Enabled) return;

			if (cmbType.Text.Equals(string.Empty)) return;

			enSeverType type;

			if (isNewRow)
			{
				pnlOracle.Visible = false;
				pnlConnSql.Visible = false;
				pnlConnWEB.Visible = false;
				return;
			}


			type	= (enSeverType)Fnc.String2Enum(new enSeverType(), cmbType.Text);


			string strGroupName = string.Empty;

			switch (type)
			{
				case enSeverType.ORACLE:
					pnlOracle.Visible = true;
					pnlConnSql.Visible = false;
					pnlConnWEB.Visible = false;
					break;

				//case enType.SQL:
				//    strGroupName = grpConnSql.Name;
				//    break;

				case enSeverType.WEB:
					pnlOracle.Visible = false;
					pnlConnSql.Visible = false;
					pnlConnWEB.Visible = true;
					break;			
	
				default:
					pnlOracle.Visible = false;
					pnlConnSql.Visible = false;
					pnlConnWEB.Visible = false;
					break;			
			}


			string strUpdateType = txtUpdateType.Text;

			DataTable dt = dsSetting.Tables[type.ToString()];

			if (dt.Select(string.Format("UPDATETYPE = '{0}'", strUpdateType)).Length < 1)
			{
				DataRow dr = dt.NewRow();

				dr["UPDATETYPE"] = strUpdateType;
				dt.Rows.Add(dr);				
				dt.AcceptChanges();
			}

			dt.DefaultView.RowFilter = string.Format("UPDATETYPE = '{0}'", strUpdateType);

		}


		/// <summary>
		/// 설정 로우를확인 - 그룹이 없으면 넣어 준다.
		/// </summary>
		/// <param name="dr"></param>
		private bool row_chk(DataRow dr)
		{
			if (Fnc.obj2String(dr["GroupCode"]).Equals(string.Empty))
			{
				DataRow r = dsSetting.Tables[fnc.GroupTable_Name].Rows[0];
				dr["GroupCode"] = r["GroupCode"];
				dr["GroupName"] = r["GroupName"];

				return true;
			}

			return false;
		}
		
		private void btnSave_Click(object sender, EventArgs e)
		{
			


			//신규의 경우 UPDATETYPE 중복 체크
			if (isNewRow && dsSetting.Tables["Setting"].Select(string.Format("UPDATETYPE = '{0}'", txtUpdateType.Text)).Length > 0)
			{
				Function.clsFunction.ShowMsg(this, "중복 UpdateType", "UpdateType이 중복 되었습니다.", frmMessage.enMessageType.OK);
				return;
			}

			DataRow dr = gvSettingList.GetFocusedDataRow();


			string groupCode = Fnc.obj2String(cmbGroup.ComboBoxSelectedValue);

			DataRow[] rows = dsSetting.Tables[fnc.GroupTable_Name].Select(string.Format("GroupCode = {0}", groupCode));

			if(rows.Length < 1)
			{
				Function.clsFunction.ShowMsg(this, "Group선택", "Group을 선택하여 주십시요.", frmMessage.enMessageType.OK);
				return;
			}

			string groupName = Fnc.obj2String(rows[0]["GroupName"]);

			if (txtPriority.Text.Equals(string.Empty)) txtPriority.Text = "9999";

			dr["GroupName"] = groupName;
			dr["Type"] = cmbType.Text;
			dr["생성일"] = DateTime.Now;
			dsSetting.Tables["Setting"].AcceptChanges();


			if (isNewRow)
			{
				isNewRow = false;
			}
			else
			{

				DataTable dt;
				string oldUType = Fnc.obj2String(_currDr["UpdateType"]);
				string newUtype = Fnc.obj2String(dr["UpdateType"]);

				bool isUpdateTypeCh = !oldUType.Equals(string.Empty) & !oldUType.Equals(newUtype);


				//세부 설정 테이블 update
				foreach (string i in Fnc.EnumItems2Strings(new enSeverType()))
				{
					dt = dsSetting.Tables[i];

					//updatetype이 변경 되면 세부 테이브로 변경 하여 준다.
					if (isUpdateTypeCh)
					{
						foreach (DataRow r in dt.Select(string.Format("UPDATETYPE = '{0}'", oldUType)))
						{
							r["UpdateType"] = newUtype;
						}
					}

					dt.AcceptChanges();
				}
			}

			DataSet_Save();

			//gvSettingList_FocusedRowChanged(null, null);

			Application.DoEvents();

			string sType = Fnc.obj2String(dr["UpdateType"]);
			Function.Component.DevExp.fnc.GridView_SelectRowBySearch(gvSettingList, "UpdateType", sType, 0);
			
			
					

		}

		private void frmSetting_Load(object sender, EventArgs e)
		{

			try
			{
				Function.Component.DevExp.fnc.GridView_ViewInit(gvSettingList, false, true);
				//Function.Component.DevExp.fnc.GridView_EditInit(gvSettingList);


				//type 콤보 박스
				foreach (string i in Fnc.EnumItems2Strings(new enSeverType()))
				{
					cmbType.ComboBoxItems.Add(i);
				}
				
				dsSetting = frmUploader.dsSetting;
				

				//Group 콤보 박스
				cmbGroup.ComboBoxValueMember = "GroupCode";
				cmbGroup.ComboBoxDisplayMember = "GroupName";				
				cmbGroup.ComboBoxDataSource = dsSetting.Tables[fnc.GroupTable_Name].DefaultView;
				dsSetting.Tables[fnc.GroupTable_Name].DefaultView.Sort = "GroupCode";

				DataTable dt = dsSetting.Tables["Setting"];

				//201512 추가 컬럼 확인
				string[] Add_Cols = new string[] { "UseZip", "GroupCode", "GroupName", "Priority" };
				Type[] Add_Types = new Type[] { typeof(System.Boolean), typeof(System.Int32), typeof(System.String), typeof(System.Int32) };
				object[] dValue = new object[] { false, string.Empty, string.Empty, 0 };


				if (Fnc.DataTable_ColumnsAdd(dt, Add_Cols, Add_Types, dValue))
				{
					dt.AcceptChanges();
					DataSet_Save();
				}

				int cnt = 0;

				//그룹을 강제 입력
				foreach(DataRow r in dt.Rows)
				{
					cnt += row_chk(r) ? 1: 0;
				}

				if(cnt > 0)
				{
					dt.AcceptChanges();
					DataSet_Save();
				}

				dt.Columns["GroupCode"].AllowDBNull = false;

				gcSettingList.DataSource = dt.DefaultView;
				dt.DefaultView.Sort = "GroupCode, Priority";
				
				DataView dv = dt.DefaultView;				

				//input box binding			
				cmbType.DataBindings.Add(new Binding("SelectedItem", dv, "Type"));
				txtUpdateType.DataBindings.Add(new Binding("Text", dv, "UPDATETYPE"));
				txtBigo.DataBindings.Add(new Binding("Text", dv, "BIGO"));
				chkUseZip.DataBindings.Add(new Binding("Checked", dv, "UseZip"));

				


				//Group, Priority 추가
				cmbGroup.DataBindings.Add(new Binding("SelectedValue", dv, "GroupCode"));
				txtPriority.DataBindings.Add(new Binding("Text", dv, "Priority"));

				dt = dsSetting.Tables["ORACLE"];
				dv = dt.DefaultView;

				txtOraTNS.DataBindings.Add(new Binding("Text", dv, "TNS"));
				txtOraID.DataBindings.Add(new Binding("Text", dv, "ID"));
				txtOraPass.DataBindings.Add(new Binding("Text", dv, "PASS"));

				dv = dsSetting.Tables["SQL"].DefaultView;
				txtSqlIP.DataBindings.Add(new Binding("Text", dv, "IP"));
				txtSqlDB.DataBindings.Add(new Binding("Text", dv, "DataBase"));
				txtSqlID.DataBindings.Add(new Binding("Text", dv, "ID"));
				txtSqlPass.DataBindings.Add(new Binding("Text", dv, "PASS"));

				dv = dsSetting.Tables["WEB"].DefaultView;
				txtWEBUrl.DataBindings.Add(new Binding("Text", dv, "URL"));
				txtWEBPass.DataBindings.Add(new Binding("Text", dv, "PASS"));


				cmdAdd.Image = (Image)Function.resIcon16.add;
				cmdDelete.Image = (Image)Function.resIcon16.delete;

				InputBox_Collapes();
			}
			catch(Exception ex)
			{
				ProcException(ex, "frmSetting_Load");
			}
			
			
		}


		/// <summary>
		/// 항목 추가
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdAdd_Click(object sender, EventArgs e)
		{	

			DataRow dr = dsSetting.Tables["Setting"].NewRow();

			dr["Type"] = "WEB";
			dr["UPDATETYPE"] = "New_Item";
			dr["bigo"] = " ";
			dr["생성일"] = DateTime.Now;
			dr["UseZip"] = false;
			dr["GroupCode"] = 9000;
			dr["GroupName"] = "기타프로그램";
			dr["Priority"] = 9999;

			dsSetting.Tables["Setting"].Rows.Add(dr);

			dsSetting.AcceptChanges();

			isNewRowAdded = true;

			Application.DoEvents();

			gvSettingList.FocusedRowHandle = gvSettingList.RowCount - 1;

			//Function.form.control.InputForm_Init(splitContainer1.Panel2);
			////cmbType.ComboBoxSelectIndex= -1;
			//txtUpdateType.Enabled = true;					

		}

		/// <summary>
		/// 항목삭제..
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdDelete_Click(object sender, EventArgs e)
		{
			DataRow dr = gvSettingList.GetFocusedDataRow();


			if (dr == null)
			{				
				return;
			}


			if (Function.clsFunction.ShowMsg(this, "항목 삭제", "선택된 항목을 삭제 하시겠습니까?", frmMessage.enMessageType.YesNo) != DialogResult.Yes)
			{
				return;
			}

			string strUpdateType = Fnc.obj2String(dr["UpdateType"]);

			DataTable dt;

			//세부 설정 테이블 update
			foreach (string i in Fnc.EnumItems2Strings(new enSeverType()))
			{
				dt = dsSetting.Tables[i];


				foreach (DataRow r in dt.Select(string.Format("UPDATETYPE = '{0}'", strUpdateType)))
				{
					dt.Rows.Remove(r);
				}
				

				dt.AcceptChanges();
			}


			foreach (DataRow r in dsSetting.Tables["Setting"].Select(string.Format("UPDATETYPE = '{0}'", strUpdateType)))
			{
				dsSetting.Tables["Setting"].Rows.Remove(r);
			}

			


			dsSetting.AcceptChanges();
			DataSet_Save();

		}

		private void btnOraSave_Click(object sender, EventArgs e)
		{
			dsSetting.Tables["ORACLE"].AcceptChanges();
			DataSet_Save();
		}


		private void frmSetting_FormClosed(object sender, FormClosedEventArgs e)
		{
			frmUploader.mdiMain.UpdateItemMenu_ReMake();
		}

		/// <summary>
		/// 로우 선택이 변경
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void gvSettingList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
		{
			//저장하시 않는 내요은 날린다
			if (isNewRowAdded)
				isNewRowAdded = false;
			else
				dsSetting.RejectChanges();

			//새로운 로우 여부 확인
			DataRow dr = gvSettingList.GetFocusedDataRow();

			_currDr = dsSetting.Tables["Setting"].NewRow();

			_currDr.ItemArray = dr.ItemArray;

			isNewRow = Fnc.obj2String(dr["UpdateType"]).Equals(string.Empty);

			Application.DoEvents();


			InputBox_Collapes();
		}

		private void cmbType_Text_Changed(object sender, usrEventArgs e)
		{
			InputBox_Collapes();
		}
		

		/// <summary>
		/// 그리드 그룹별 색상 변경
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void gvSettingList_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
		{

			if (gvSettingList.FocusedRowHandle == e.RowHandle) return;

			DevExpress.XtraGrid.Views.Grid.GridView gv = sender as DevExpress.XtraGrid.Views.Grid.GridView;
			DataRow r = gv.GetDataRow(e.RowHandle);
			int idx = 0;
			int i = 0;

			string groupName = Fnc.obj2String(r["GroupName"]);

			foreach(DataRow dr in dsSetting.Tables[fnc.GroupTable_Name].Rows)
			{
				if(Fnc.obj2String(dr["GroupName"]).Equals(groupName))
				{
					idx = i % groupColors.Length;					
					break;
				}

				i++;
			}


			e.Appearance.BackColor = groupColors[idx];
		}
	}
}