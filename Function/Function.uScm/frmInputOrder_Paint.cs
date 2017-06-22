using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Function;


namespace prjU_SCM
{
    public partial class frmInputOrder_Paint : Form
    {
        Function.clsOracle.strConnect clsDB;
        clsLog clsLog;
		string SYSTEMID = string.Empty;
		string strCode;		
		string strStationID;

		string strCarTypeName_Frt = string.Empty;
		string strCarType_Frt = string.Empty;
		/// <summary>
		/// Frt Model Code
		/// </summary>
		string strCartype_Frt
		{
			get
			{
				return strCarType_Frt;
			}
			set
			{
				strCarType_Frt = value;

				if (strCarType_Frt == string.Empty)
				{
					clsControl.Invoke_Label_TextColor(lblModel_FRT, "모델선택", Color.Black, System.Drawing.SystemColors.ControlDark);
				}
				else
				{
					clsControl.Invoke_Label_TextColor(lblModel_FRT, strCarTypeName_Frt, Color.Yellow, Color.RoyalBlue);
				}

			}

		}

		string strCarTypeName_RR = string.Empty;
		string strCarType_RR = string.Empty;
		/// <summary>
		/// RR Model Code
		/// </summary>
		string strCartype_RR
		{
			get
			{
				return strCarType_RR;
			}
			set
			{
				strCarType_RR = value;

				if (strCarType_RR == string.Empty)
				{
					clsControl.Invoke_Label_TextColor(lblModel_RR, "모델선택", Color.Black, System.Drawing.SystemColors.ControlDark);
				}
				else
				{
					clsControl.Invoke_Label_TextColor(lblModel_RR, strCarTypeName_RR, Color.Yellow, Color.RoyalBlue);
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

				if (strcar == string.Empty)
				{
					cmbPartCode.DataSource = null;
					
					if (strCartype_Frt == string.Empty)		return;
				}

				using (DataSet ds = clsDBFunc.Get_CartypeColor(clsDB, strcar))
				{
					cmbPartCode.DataSource = ds.Tables[0];
					cmbPartCode.DisplayMember = "CODEVALUENAME";
				}	

			}
		}


		public frmInputOrder_Paint(Function.clsOracle.strConnect clsdb, clsLog clslog, string _SystemID, string _strOrderType, string _strStationID)
        {
            InitializeComponent();

            clsDB = clsdb;
            clsLog = clslog;
			SYSTEMID = _SystemID;			
			strStationID = _strStationID;
        }

        private void frmInputOrder_Load(object sender, EventArgs e)
        {
			
		
			this.Text = "도장 오더 입력";
			strCode = "COLOR";
			lblCode.Text = "색상";		

            ResetInput();

        }

        /// <summary>
        /// 등록버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInput_Click(object sender, EventArgs e)
        {
			if (strCartype_Frt == string.Empty)
			{
				clsFunction.ShowMsg(this, "모델(FRT)를 선택 하여 주십시요", string.Empty, frmMessage.enMessageType.OK);
				return;
			}

			if (strCartype_RR == string.Empty)
			{
				clsFunction.ShowMsg(this, "모델(RR)를 선택 하여 주십시요", string.Empty, frmMessage.enMessageType.OK);
				return;
			}

            if (cmbPartCode.SelectedIndex == -1)
            {
                clsFunction.ShowMsg(this, strCode + "를 선택 하여 주십시요", string.Empty, frmMessage.enMessageType.OK);
                return;
            }

            if (txtTargetCnt.Value == 0)
            {
                clsFunction.ShowMsg(this, "수량을 입력 하여 주십시요.", string.Empty, frmMessage.enMessageType.OK);
                return;
            }

            string strDt = clsFunction.Date2String(dtPlan.Value, clsFunction.enDateType.DBType);
            string strPartCode = ((DataRowView)cmbPartCode.SelectedItem)["CODEVALUE"].ToString();
			string strPartCodeName = ((DataRowView)cmbPartCode.SelectedItem)["CODEVALUEName"].ToString();
            string strTarget_Cnt = txtTargetCnt.Value.ToString();
            string strNo = string.Format("{0}", lstPlan.Items.Count + 1);

            lstPlan.Items.Add(new ListViewItem(new string [] { strNo, strDt, strCarTypeName_Frt, strCartype_Frt, strCarTypeName_RR, strCartype_RR
							, strPartCodeName, strPartCode, strTarget_Cnt }));

            ResetInput();
        }


        private void ResetInput()
        {
			strCartype_Frt = string.Empty;
			strCartype_RR = string.Empty;

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


                clsOleDB clsOle = new clsOleDB(enProvider.Excel, strFileName, string.Empty, string.Empty, string.Empty);

                using (DataSet ds = clsOle.dsExcute_Query("select orderdate, partcode, target_cnt from [order$]"))
                {
                    if (ds.Tables[0].Rows.Count < 1) return;

                    lstPlan.Items.Clear();
                    ResetInput();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstPlan.Items.Add(new ListViewItem(new string[] {   (lstPlan.Items.Count + 1).ToString(),
                                                                            clsFunction.Null2String(dr["ORDERDATE"]),
                                                                            clsFunction.Null2String(dr["PARTCODE"]),
                                                                            clsFunction.Null2String(dr["TARGET_CNT"])
                                                                            }));
                    }
                }                

            }
            catch(Exception ex)
            {
                clsLog.WLog_Exception("btnXlsLoad", ex);
                clsFunction.ShowMsg(this, ex.Message, string.Empty, frmMessage.enMessageType.OK);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
				clsDBFunc.Insert_Order_Paint(clsDB, lstPlan, clsLog, strStationID, SYSTEMID);

                clsFunction.ShowMsg(this, "생산지시 저장이 완료 되었습니다.", string.Empty, frmMessage.enMessageType.OK);

                lstPlan.Items.Clear();
                ResetInput();

				this.Close();


            }
            catch(Exception ex)
            {
                clsLog.WLog_Exception("btnXlsLoad", ex);
                clsFunction.ShowMsg(this, ex.Message, string.Empty, frmMessage.enMessageType.OK);
            }
        }

		private void lblModel_FRT_Click(object sender, EventArgs e)
		{
			prjU_SCM.frmCarttypeSearch frm = new prjU_SCM.frmCarttypeSearch(clsDB);
			frm.ShowDialog();
						
			strCarTypeName_Frt = frm.strCartypeCodeName;
			strCartype_Frt = frm.strCartypeCode;

			strCar = frm.strCarCode;

		}

		private void lblModel_RR_Click(object sender, EventArgs e)
		{
			prjU_SCM.frmCarttypeSearch frm = new prjU_SCM.frmCarttypeSearch(clsDB);
			frm.ShowDialog();
			
			strCarTypeName_RR = frm.strCartypeCodeName;
			strCartype_RR = frm.strCartypeCode;		

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