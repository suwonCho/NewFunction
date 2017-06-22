﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Function.Db;
using Function;

namespace Function.form.Db
{
	public partial class DBSetting : frmBaseForm
	{

		public MsSQL.strConnect sql = new MsSQL.strConnect();
		public OracleDB.strConnect ora = new OracleDB.strConnect();
		public Function.Db.enDBType dbType = enDBType.None;


		bool isInit = false;



		public DBSetting()
		{
			
		}

		public DBSetting(MsSQL.strConnect _sql)
		{
			dbType = enDBType.MsSQL;
			sql = _sql;
			Form_Init();
		}


		public DBSetting(OracleDB.strConnect _orc)
		{
			dbType = enDBType.Oracle;
			ora = _orc;
			Form_Init();
		}


		public new void Form_Init()
		{
			InitializeComponent();

			try
			{
				isInit = true;

				btnSave.Image = Function.resIcon16.save_alt;
				btnSave.ImageAlign = ContentAlignment.MiddleLeft;


				btnCancel.Image = Function.resIcon16.delete_alt;
				btnCancel.ImageAlign = ContentAlignment.MiddleLeft;

				switch (dbType)
				{
					case enDBType.Oracle:
						inpDbType.Value = "ORACLE";
						inpIp.Value = ora.strTNS;
						inpId.Value = ora.strID;
						inpPass.Value = ora.strPass;
						
						break;

					case enDBType.MsSQL:
						inpDbType.Value = "MS-SQL";
						inpIp.Value = sql.strIP;
						inpId.Value = sql.strID;
						inpPass.Value = sql.strPass;
						string db = sql.strDataBase;
						inpDataBase_ComboBoxDropDown(null, null);

						inpDataBase.Value = db;
						break;
				}
			}
			catch
			{
			}
			finally
			{
				isInit = false;
			}

		}


		private void set_ConnString()
		{
			
			if (inpIp.TEXT.Trim().Equals(string.Empty))	
			{
				if(dbType == enDBType.Oracle)
					SetMessage(true, "IP를 입력 하여 주세요", false);
				else
					SetMessage(true, "TNS를 입력 하여 주세요", false);
			}

			bool auth = inpAuthType.TEXT.Equals("Sql Server 인증");

			if (auth)
			{
				if (inpId.TEXT.Trim().Equals(string.Empty)) SetMessage(true, "ID를 입력 하여 주세요", false);

				if (inpPass.TEXT.Trim().Equals(string.Empty)) SetMessage(true, "암호를 입력 하여 주세요", false);
			}

			switch (dbType)
			{
				case enDBType.Oracle:
					ora.strTNS = inpIp.TEXT.Trim();
					ora.strID = inpId.TEXT.Trim();
					ora.strPass = inpPass.TEXT.Trim();
					break;

				case enDBType.MsSQL:
					sql.strIP = inpIp.TEXT.Trim();
					if (auth)
					{
						sql.strID = inpId.TEXT.Trim();
						sql.strPass = inpPass.TEXT.Trim();
					}
					else
					{
						sql.strID = string.Empty;
						sql.strPass = string.Empty;
					}

					
					sql.strDataBase = inpDataBase.TEXT.Trim();
					break;

				default:
					SetMessage(true, "DB타입을 선택 하여 주세요.", false);
					break;

			}
		
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		/// <summary>
		/// db 타입 변경
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void inpDbType_Text_Changed(object sender, usrEventArgs e)
		{
			switch(inpDbType.ComboBoxSelectItem.ToString())
			{
				case "ORACLE":
					inpAuthType.ComboBoxSelectItem = "Sql Server 인증";
					inpAuthType.Enabled = false;

					inpIp.Label_Text = "TNS";

					inpDataBase.ComboBoxSelectItem = null;
					inpDataBase.Enabled = false;
					dbType = enDBType.Oracle;
					break;

				case "MS-SQL":
					inpAuthType.ComboBoxSelectItem = "Sql Server 인증";
					inpAuthType.Enabled = true;

					inpIp.Label_Text = "IP";

					inpDataBase.ComboBoxSelectItem = null;
					inpDataBase.Enabled = true;
					dbType = enDBType.MsSQL;
					break;

				default:
					inpAuthType.Enabled = false;
					inpDataBase.ComboBoxSelectItem = null;
					inpDataBase.Enabled = false;
					dbType = enDBType.None;
					break;
					
			}
		}

		private void inpAuthType_Text_Changed(object sender, usrEventArgs e)
		{
			bool value = inpAuthType.TEXT.Equals("Sql Server 인증");

			inpId.Enabled = value;
			inpPass.Enabled = value;
			
		}

		private void inpDataBase_ComboBoxDropDown(object sender, EventArgs e)
		{
			inpDataBase.ComboBoxDataSource = null;

			if (dbType != enDBType.MsSQL)
			{				
				return;
			}

			set_ConnString();

			try
			{
				SetMessage(false, "DB 목록 조회중...", false);
				Application.DoEvents();

				DataTable dt = MsSQL.DBListGet(sql);

				inpDataBase.ComboBoxDataSource = dt;
				inpDataBase.ComboBoxDisplayMember = "Name";
				inpDataBase.ComboBoxValueMember = "Name";

				SetMessage(false, "", false);
			}
			catch(Exception ex)
			{
				if(ex.Message.Trim().Equals(string.Empty))
					SetMessage(true, ex.InnerException.Message, false);
				else
					SetMessage(true, ex.Message, false);
			}




		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
			set_ConnString();
			Close();
		}

		private void DBSetting_Load(object sender, EventArgs e)
		{
			Form_Init();
		}
	}
}