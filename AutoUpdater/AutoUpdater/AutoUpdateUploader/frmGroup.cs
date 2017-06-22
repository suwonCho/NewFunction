using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoUpdater
{
	public partial class frmGroup : Function.form.frmBaseForm
	{
		/// <summary>
		/// group table
		/// </summary>
		DataTable dtGroup;

		public frmGroup()
		{
			InitializeComponent();

			group_datatable_init();
		}


		/// <summary>
		/// 그룹 데이터 테이블 생성하고 테이블 구조를 확인한다.
		/// </summary>
		private void group_datatable_init()
		{

			//그룹이 없으면 만들어 준다.
			if (!frmUploader.dsSetting.Tables.Contains(fnc.GroupTable_Name))
			{
				dtGroup = new DataTable();

				dtGroup.Columns.Add("GroupCode", typeof(System.Int32));
				dtGroup.Columns.Add("GroupName", typeof(System.String));

				dtGroup.TableName = fnc.GroupTable_Name;

				frmUploader.dsSetting.Tables.Add(dtGroup);
			}
			else
				dtGroup = frmUploader.dsSetting.Tables[fnc.GroupTable_Name];
		}



		//-----------이하 이벤트 
		private void frmGroup_Load(object sender, EventArgs e)
		{
			grdGroup.DataSource = dtGroup.DefaultView;
			dtGroup.DefaultView.Sort = "GroupCode";

			Function.Component.DevExp.fnc.GridView_SetColumnAutoWidth(grvGroup);

			Function.Component.DevExp.fnc.GridView_EditInit(grvGroup);
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			frmUploader.dsSetting.WriteXml(frmUploader.strDSFileName, XmlWriteMode.WriteSchema);
			dtGroup.AcceptChanges();
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			dtGroup.RejectChanges();
			this.Close();
		}
	}
}
