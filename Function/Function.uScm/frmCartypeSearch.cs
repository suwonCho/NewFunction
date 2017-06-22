using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Function;
using Function.Db;

namespace Function.uScm
{
	public partial class frmCarttypeSearch : Form
    {
        OracleDB.strConnect strConn;

		public string strCartypeCode = string.Empty;
		public string strCartypeCodeName = string.Empty;
		public string strCarCode = string.Empty;


        public frmCarttypeSearch(OracleDB.strConnect _strConn)
        {
			InitializeComponent();
			strConn = _strConn;
        }

		/// <summary>
		/// 모델을 조회 한다.
		/// </summary>
		private void SearchCartype()
		{
			try
			{
				string strWord = txtWord.Text;

				using (DataSet ds = clsDBFunc.Get_Cartype(strConn, strWord))
				{
					form.control.Invoke_ListView_AddItem(lstList, true, ds.Tables[0], new string[] { "CARCODE", "CARTYPECODE", "CARTYPENAME", "CARNAME" });

					if (ds.Tables[0].Rows.Count < 1)
					{
						txtWord.Focus();
					}
					else
					{
						lstList.Focus();
					}

				}

			}
			catch
			{
			}

		}

		private void txtWord_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
				SearchCartype();
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			SearchCartype();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			strCarCode = string.Empty;
			strCartypeCodeName = string.Empty;
			strCartypeCode = string.Empty;
			this.Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (lstList.SelectedItems.Count < 1) return;

			strCarCode = lstList.SelectedItems[0].SubItems[0].Text;
			strCartypeCodeName = lstList.SelectedItems[0].SubItems[2].Text;
			strCartypeCode = lstList.SelectedItems[0].SubItems[1].Text;

			this.Close();
		}

		private void lstList_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			btnOk_Click(null, null);
		}

		private void lstList_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
				btnOk_Click(null, null);
		}





       
    }
}
