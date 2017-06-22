using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;
using Function;
using Function.Db;



namespace Function.uScm
{
	public partial class usrLogIn : UserControl
	{
		OracleDB.strConnect strConn;
		public DataRowView drvWorker = null;
		public delegate void delLogIn();
		public delLogIn evtLogIn = null;
		public string strCodeValue = string.Empty;

		string strUserType;

        public usrLogIn(OracleDB.strConnect _strConn, string strCode, bool isComboBox, string strUsertype, string strDepartment)
		{
			InitializeComponent();

			try
			{
				strConn = _strConn;
				strUserType = strUsertype;

				if (isComboBox)
				{
					cmbGubun.Visible = true;
					lblGubun.Visible = false;

					cmbGubun.SelectedIndexChanged += new EventHandler(cmbGubun_SelectedIndexChanged);

					using (DataSet ds = Get_CodeDetail(strConn, strCode))
					{
						form.control.Invoke_ComboBox_DataSource(cmbGubun, ds.Tables[0], "CODEVALUENAME");
					}

					

				}
				else
				{
					cmbGubun.Visible = false;
					lblGubun.Visible = true;
					lblGubun.Text = strCode;
					using (DataSet ds = Get_Workers(strConn, strUserType, strDepartment))
					{
						form.control.Invoke_ComboBox_DataSource(cmbWorker, ds.Tables[0], "USERNAME");
					}
				}

				this.Visible = false;
			}
			catch (Exception ex)
			{
				ErrorProc(ex);
			}


		}

		private void cmbGubun_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (cmbGubun.SelectedItem == null) return;

				DataRowView dv = (DataRowView)cmbGubun.SelectedItem;

				strCodeValue = Fnc.obj2String(dv["CODEVALUE"]);

				using (DataSet ds = Get_Workers(strConn, strUserType, strCodeValue))
				{
					form.control.Invoke_ComboBox_DataSource(cmbWorker, ds.Tables[0], "USERNAME");
				}
			}
			catch (Exception ex)
			{
				ErrorProc(ex);
			}


		}


		private void ErrorProc(Exception ex)
		{
			Fnc.ShowMsg("프로그램 에러 발생...", ex.Message, frmMessage.enMessageType.OK);
		}



		delegate void delSetLocation(int intTop, int intLeft);
		public void SetLocation(int intTop, int intLeft)
		{
			if(this.InvokeRequired)
			{
				this.Invoke(new delSetLocation(SetLocation), new object[] { intTop, intLeft });
				return;
			}

			this.Top = intTop;
			this.Left = intLeft;
		}


		delegate void delSetVisible(bool isVisible);
		public void SetVisible(bool isVisible)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new delSetVisible(SetVisible), new object[] { isVisible });
				return;
			}

			this.Visible = isVisible;
			this.BringToFront();
		}




		/// <summary>
		/// 작업자 목록을 조회한다.
		/// </summary>
		/// <param name="clsDB"></param>
		/// <returns></returns>
        public static DataSet Get_Workers(OracleDB.strConnect strConn, string strUserType, string strDepartment)
		{
            OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

			OracleParameter[] param = new OracleParameter[] { 
												new OracleParameter("ps_UserType", OracleDbType.Varchar2)   ,
												new OracleParameter("PS_DEPARTMENT", OracleDbType.Varchar2)   ,
                                                new OracleParameter("T_CURSOR", OracleDbType.RefCursor)   
												};

			param[0].Value = strUserType;
			param[1].Value = strDepartment;
			param[2].Direction = ParameterDirection.Output;

			return clsDB.dsExcute_StoredProcedure("SP_GET_Workers", param);

		}

		/// <summary>
		/// CodeDetail목록을 조회한다.
		/// </summary>
		/// <param name="clsDB"></param>
		/// <returns></returns>
        public static DataSet Get_CodeDetail(OracleDB.strConnect strConn, string strCode)
		{
            OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

			OracleParameter[] param = new OracleParameter[] { 
												new OracleParameter("ps_Code", OracleDbType.Varchar2)   ,
                                                new OracleParameter("T_CURSOR", OracleDbType.RefCursor)   
												};

			param[0].Value = strCode;
			param[1].Direction = ParameterDirection.Output;

			return clsDB.dsExcute_StoredProcedure("SP_GET_CODEDETAILLIST", param);

		}

		private void cmbWorker_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbWorker.SelectedItem == null)
			{
				drvWorker = null;
				return;
			}

			drvWorker = (DataRowView)cmbWorker.SelectedItem;
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			if (drvWorker == null)
			{
				Fnc.ShowMsg("작업자를 선택하여 주십시요.", string.Empty, frmMessage.enMessageType.OK);
				return;
			}

			if (evtLogIn != null) evtLogIn();
		}



	}
}
