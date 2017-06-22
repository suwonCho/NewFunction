using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Function;
using Function.Db;
using Function.Util;


namespace Function.uScm
{
    public partial class frmInputOrder : Form
    {
        OracleDB.strConnect clsDB;
        Log clsLog;
		string SYSTEMID = string.Empty;
		string strCode;
		string strOrderType;
		string strStationID;
		string strCarTypeName = string.Empty;
		string strCarType = string.Empty;
		/// <summary>
		/// 선택된 모델 코드
		/// </summary>
		string strCartype
		{
			get
			{
				return strCarType;
			}
			set
			{
				strCarType = value;

				if (strCarType == string.Empty)
				{
					form.control.Invoke_Control_Text(lblModel, "모델선택");
					form.control.Invoke_Control_Color(lblModel, Color.Black, System.Drawing.SystemColors.ControlDark);
				}
				else
				{
					form.control.Invoke_Control_Text(lblModel, strCarTypeName);
					form.control.Invoke_Control_Color(lblModel, Color.Yellow, Color.RoyalBlue);
				}

			}

		}


		string strcar;
		string strCar
		{
			get
			{
				return strcar;
			}
			set
			{
				strcar = value;

				if (strOrderType == "I") return;

				if (strcar == string.Empty)
				{
					cmbPartCode.DataSource = null;
					return;
				}

				using (DataSet ds = clsDBFunc.Get_CartypeColor(clsDB, strcar))
				{
					cmbPartCode.DataSource = ds.Tables[0];
					cmbPartCode.DisplayMember = "CODEVALUENAME";
				}	

			}
		}


        public frmInputOrder(OracleDB.strConnect clsdb, Log clslog, string _SystemID, string _strOrderType, string _strStationID)
        {
            InitializeComponent();

            clsDB = clsdb;
            clsLog = clslog;
			SYSTEMID = _SystemID;
			strOrderType = _strOrderType;
			strStationID = _strStationID;
        }

        private void frmInputOrder_Load(object sender, EventArgs e)
        {
			
			if (strOrderType == "I")
			{				
				this.Text = "사출 오더 입력";
				strCode = "PARTCODE";
				lblCode.Text = "반";

				using (DataSet ds = clsDBFunc.Get_PartCode(clsDB, strCode))
				{
					cmbPartCode.DataSource = ds.Tables[0];
					cmbPartCode.DisplayMember = "CODEVALUENAME";
				}	

			}
			else
			{
				this.Text = "도장 오더 입력";
				strCode = "COLOR";
				lblCode.Text = "색상";
			}
			

            ResetInput();

        }

        /// <summary>
        /// 등록버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInput_Click(object sender, EventArgs e)
        {
			if (strCartype == string.Empty)
			{
				Fnc.ShowMsg(this, "CarType를 선택 하여 주십시요", string.Empty, frmMessage.enMessageType.OK);
				return;
			}

            if (cmbPartCode.SelectedIndex == -1)
            {
                Fnc.ShowMsg(this, strCode + "를 선택 하여 주십시요", string.Empty, frmMessage.enMessageType.OK);
                return;
            }

            if (txtTargetCnt.Value == 0)
            {
                Fnc.ShowMsg(this, "수량을 입력 하여 주십시요.", string.Empty, frmMessage.enMessageType.OK);
                return;
            }

			string strDt = Fnc.Date2String(dtPlan.Value, Fnc.enDateType.DBType);
			//string strCarType = ((DataRowView)cmbCarType.SelectedItem)["CODEVALUE"].ToString();
			//string strCarTypeName = ((DataRowView)cmbCarType.SelectedItem)["CODEVALUENAME"].ToString();
            string strPartCode = ((DataRowView)cmbPartCode.SelectedItem)["CODEVALUE"].ToString();
			string strPartCodeName = ((DataRowView)cmbPartCode.SelectedItem)["CODEVALUEName"].ToString();
            string strTarget_Cnt = txtTargetCnt.Value.ToString();
            string strNo = string.Format("{0}", lstPlan.Items.Count + 1);

            lstPlan.Items.Add(new ListViewItem(new string [] { strNo, strDt, strCarTypeName, strCarType, strPartCodeName, strPartCode, strTarget_Cnt }));

            ResetInput();
        }


        private void ResetInput()
        {
			//cmbCarType.SelectedIndex = -1;
			strCartype = string.Empty;
            cmbPartCode.SelectedIndex = -1;
            txtTargetCnt.Value = 0;
        }

        /// <summary>
        /// 선택된 Row삭제..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (lstPlan.SelectedItems.Count < 1) return;

            lstPlan.Items.Remove(lstPlan.SelectedItems[0]);
        }

        /// <summary>
        /// 폼초기화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            lstPlan.Items.Clear();
            ResetInput();
            dtPlan.Value = DateTime.Now;
        }

        private void btnXlsLoad_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dial = new OpenFileDialog();

                dial.Multiselect = false;
                dial.ShowDialog();

                string strFileName = dial.FileName;

                dial.Dispose();

                if (strFileName == string.Empty) return;

                strFileName = strFileName.Replace(@"\\", @"\");

                //strFileName = @".\chasys.xls";

				

                OleDB clsOle = new OleDB(OleDB.enProvider.Excel, strFileName, string.Empty, string.Empty, string.Empty);

                using (DataSet ds = clsOle.dsExcute_Query("select orderdate, partcode, target_cnt from [order$]"))
                {
                    if (ds.Tables[0].Rows.Count < 1) return;

                    lstPlan.Items.Clear();
                    ResetInput();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstPlan.Items.Add(new ListViewItem(new string[] {   (lstPlan.Items.Count + 1).ToString(),
                                                                            Fnc.obj2String(dr["ORDERDATE"]),
                                                                            Fnc.obj2String(dr["PARTCODE"]),
                                                                            Fnc.obj2String(dr["TARGET_CNT"])
                                                                            }));
                    }
                }                

            }
            catch(Exception ex)
            {
                clsLog.WLog_Exception("btnXlsLoad", ex);
                Fnc.ShowMsg(this, ex.Message, string.Empty, frmMessage.enMessageType.OK);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
				clsDBFunc.Insert_Order(clsDB, lstPlan, strOrderType, clsLog, strStationID, SYSTEMID);

                Fnc.ShowMsg(this, "생산지시 저장이 완료 되었습니다.", string.Empty, frmMessage.enMessageType.OK);

                lstPlan.Items.Clear();
                ResetInput();

				this.Close();


            }
            catch(Exception ex)
            {
                clsLog.WLog_Exception("btnXlsLoad", ex);
                Fnc.ShowMsg(this, ex.Message, string.Empty, frmMessage.enMessageType.OK);
            }
        }

		private void lblModel_Click(object sender, EventArgs e)
		{
			frmCarttypeSearch frm = new frmCarttypeSearch(clsDB);
			frm.ShowDialog();

			strCar = frm.strCarCode;
			strCarTypeName = frm.strCartypeCodeName;
			strCartype = frm.strCartypeCode;			

		}

		private void cmbPartCode_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				txtTargetCnt.Focus();
		}

		private void txtTargetCnt_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				btnInput.Focus();
		}

  



    }
}