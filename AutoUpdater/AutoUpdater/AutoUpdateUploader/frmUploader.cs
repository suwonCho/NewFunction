using Function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AutoUpdater
{
	public partial class frmUploader : Function.form.frmBaseForm
	{

		public static DataSet dsSetting;
		public static readonly string strDSFileName = @"Setting.xml";
		public static frmUploader mdiMain;

		int dGrpCode = 9999;
		string dGrpName = "PGM";

		/// <summary>
		/// 
		/// </summary>
		public frmUploader()
		{
			InitializeComponent();
			SavePosition_Setting = Properties.Settings.Default;

			this.Text += string.Format(" v.{0}", Application.ProductVersion);
		}

		private void frmUploader_Load(object sender, EventArgs e)
		{
			//config.xml 파일을 읽는다..
			dsSetting = new DataSet();

			mdiMain = this;

			string[] Add_Cols = new string[] { "GroupCode", "GroupName" };
			string[] Add_Types = new string[] { "System.Int32", "System.String" };
			object[] dValue = new object[] { dGrpCode, dGrpName };

			bool isCh = false;

			if (Function.system.clsFile.FileExists(strDSFileName))
			{
				dsSetting.ReadXml(strDSFileName);


				if(DataTable_Add(dsSetting, "Group", Add_Cols, Add_Types, dValue))
				{
					DataRow dr = dsSetting.Tables["Group"].NewRow();

					//기본 그룹을 생성한다.
					dr["GroupCode"] = dGrpCode;
					dr["GroupName"] = dGrpName;
					dsSetting.Tables["Group"].Rows.Add(dr);

					dsSetting.Tables["Group"].AcceptChanges();

					isCh = true;


				}

				if(Fnc.DataTable_ColumnsAdd(dsSetting.Tables["Setting"], Add_Cols, Add_Types, dValue))				
				{
					foreach(DataRow r in dsSetting.Tables["Setting"].Rows)
					{
						//기본 그룹을 생성한다.
						r["GroupCode"] = dGrpCode;
						r["GroupName"] = dGrpName;
					}

					dsSetting.Tables["Setting"].AcceptChanges();

					isCh = true;
				}
				

				foreach (DataTable dt in dsSetting.Tables)
				{
					dt.PrimaryKey = new DataColumn[] { dt.Columns["UPLOADTYPE"] };
				}

				dsSetting.AcceptChanges();

				if(isCh)
				{
					dsSetting.WriteXml(strDSFileName, XmlWriteMode.WriteSchema);
				}
			}
			else
			{
				dsSetting = new DataSet();

				DataTable_Add(dsSetting, "Group", Add_Cols, Add_Types, dValue);



				DataTable dt = new DataTable("Setting");

				dt.Columns.Add(new DataColumn("TYPE", Type.GetType("System.String")));
				dt.Columns.Add(new DataColumn("UPDATETYPE", Type.GetType("System.String")));
				dt.Columns.Add(new DataColumn("BIGO", Type.GetType("System.String")));
				dt.Columns.Add(new DataColumn("생성일", Type.GetType("System.DateTime")));
				
				dt.Columns.Add(new DataColumn("UseZip", Type.GetType("System.Boolean")));
				dt.Columns.Add(new DataColumn("GroupCode", Type.GetType("System.Int32")));
				dt.Columns.Add(new DataColumn("GroupName", Type.GetType("System.String")));
				dt.Columns.Add(new DataColumn("Priority", Type.GetType("System.Int32")));


				dt.PrimaryKey = new DataColumn[] { dt.Columns["UPLOADTYPE"] };
				dsSetting.Tables.Add(dt);

				dt = new DataTable("ORACLE");
				dt.Columns.Add(new DataColumn("UPDATETYPE", Type.GetType("System.String")));
				dt.Columns.Add(new DataColumn("TNS", Type.GetType("System.String")));
				dt.Columns.Add(new DataColumn("ID", Type.GetType("System.String")));
				dt.Columns.Add(new DataColumn("PASS", Type.GetType("System.String")));
				dt.PrimaryKey = new DataColumn[] { dt.Columns["UPLOADTYPE"] };
				dsSetting.Tables.Add(dt);

				dt = new DataTable("SQL");
				dt.Columns.Add(new DataColumn("UPDATETYPE", Type.GetType("System.String")));
				dt.Columns.Add(new DataColumn("IP", Type.GetType("System.String")));
				dt.Columns.Add(new DataColumn("DATABASE", Type.GetType("System.String")));
				dt.Columns.Add(new DataColumn("ID", Type.GetType("System.String")));
				dt.Columns.Add(new DataColumn("PASS", Type.GetType("System.String")));
				dt.PrimaryKey = new DataColumn[] { dt.Columns["UPLOADTYPE"] };
				dsSetting.Tables.Add(dt);

				dt = new DataTable("TCP");
				dt.Columns.Add(new DataColumn("UPDATETYPE", Type.GetType("System.String")));
				dt.Columns.Add(new DataColumn("IP", Type.GetType("System.String")));
				dt.Columns.Add(new DataColumn("PORT", Type.GetType("System.Int32")));
				dt.PrimaryKey = new DataColumn[] { dt.Columns["UPLOADTYPE"] };
				dsSetting.Tables.Add(dt);
			}

			UpdateItemMenu_ReMake();
		}

		/// <summary>
		/// 데이터셑에 데이터테이블을 추가한다.
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="tableName"></param>
		/// <param name="Add_Cols"></param>
		/// <param name="Add_Types"></param>
		/// <param name="dValue"></param>
		/// <returns></returns>
		private bool DataTable_Add(DataSet ds, string tableName, string[] Add_Cols, string[] Add_Types, object[] dValue)
		{
			if (dsSetting.Tables.IndexOf(tableName) >= 0) return false;

			DataTable grp = new DataTable(tableName);

			Fnc.DataTable_ColumnsAdd(grp, Add_Cols, Add_Types, dValue);

			ds.Tables.Add(grp);
			
			return true;
		}


		public void UpdateItemMenu_ReMake()
		{
			updatesToolStripMenuItem.DropDownItems.Clear();

			DataView dv = dsSetting.Tables["Group"].DefaultView;
			dv.Sort = "GroupCode";

			DataRow[] rows;
			DataRowView rv;
			for (int idx = 0; idx < dv.Count; idx++)
			{
				ToolStripMenuItem tsm = new ToolStripMenuItem();

				rv = dv[idx];

				tsm.Text = Function.Fnc.obj2String(rv["GroupName"]);
				tsm.Name = Function.Fnc.obj2String(rv["GroupName"]);				

				updatesToolStripMenuItem.DropDownItems.Add(tsm);

				rows = dsSetting.Tables["Setting"].Select(string.Format("GroupCode = {0}", rv["GroupCode"]));

				foreach (DataRow dr in rows)
				{
					ToolStripMenuItem tss = new ToolStripMenuItem();
					tss.Text = Function.Fnc.obj2String(dr["BIGO"]);
					tss.Name = Function.Fnc.obj2String(dr["UPDATETYPE"]);
					tss.Click += new EventHandler(tsm_Click);

					tsm.DropDownItems.Add(tss);					
				}

			}
			
		}

		private void tsm_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem tsm = sender as ToolStripMenuItem;

			frmUploadWindow frm = new frmUploadWindow(tsm.Name, tsm.Text);

			Function.form.control.Invoke_Form_Add_MdiChild(this, frm, true);

		}

		private void fileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem mi = sender as ToolStripMenuItem;

			if (mi == null) return;

			string idx = (string)mi.Tag;

			switch(idx)
			{
				case "0":
					frmGroup gr = new frmGroup();
					Function.form.control.Invoke_Form_Add_MdiChild(this, gr, true);
					break;

				case "1":
					frmSetting frm = new frmSetting();
					Function.form.control.Invoke_Form_Add_MdiChild(this, frm, true);
					break;
			}


			
		}



	
	}
}