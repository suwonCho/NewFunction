using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Function.form
{
	public partial class frmPasswords : Form
	{
		string _passwords = string.Empty;

		/// <summary>
		/// 입력한 패스 워드
		/// </summary>
		public string PassWords;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="title">창에 표시될 타이틀</param>
		/// <param name="passwords">확인할 암호, empty로 입력 시 비교는 하지 않고 입력만 받는다</param>
		public frmPasswords(string title, string passwords)
		{
			InitializeComponent();
			
			lblTitle.Text = title == string.Empty ? "암호를 입력 하여 주십시요." : title;
			_passwords = passwords;
		}
		
		private void picClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			

			if (txtPasswords.Text == _passwords || _passwords.Equals(string.Empty))
			{

				PassWords = txtPasswords.Text;
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
				this.Close();
				return;
			}

			Function.clsFunction.ShowMsg(this, "암호 오류", "암호가 틀렸습니다. 다시 입력 하여 주십시요", frmMessage.enMessageType.OK);
			txtPasswords.SelectAll();
			txtPasswords.Focus();
		}


		private void frmMessage_Load(object sender, EventArgs e)
		{
			txtPasswords.Text = string.Empty;
			txtPasswords.Focus();
		}

		private void txtPasswords_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) btnOk_Click(null, null);
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			txtPasswords.Text = string.Empty;
			this.Close();
		}
	}
}
