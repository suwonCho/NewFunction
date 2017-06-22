using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Function.form
{
	/// <summary>
	/// ���� �Է�â Ŭ����
	/// </summary>
	partial class frmNumberInput : Form
	{
		public frmNumberInput()
		{
			InitializeComponent();
		}

		public string strNumber = string.Empty;


		/// <summary>
		/// �Է� ���� ���� ���� ���� (1~10)
		/// </summary>
		public int intMaxLength
		{
			get { return txtNumber.MaxLength; }
			set
			{
				if (value < 1)
					txtNumber.MaxLength = 1;
				else if (value > 10)
					txtNumber.MaxLength = 10;
				else
					txtNumber.MaxLength = value;

			}
		}
		
		private void frmNemberInput_Load(object sender, EventArgs e)
		{
			txtNumber.Text = string.Empty;

			controlEvent.TextBox_Press_NumberOnly(txtNumber);

		}

		/// <summary>
		/// ��ȣ�� Ŭ��..
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NumberClick(object sender, EventArgs e)
		{

			string strNumber = ((Button)sender).Name;

			//�ִ� ���̰� ���� �ʰ�..
			if (txtNumber.Text.Length >= txtNumber.MaxLength) return;
			
			strNumber = strNumber.Substring(3, 1);

			if (char.IsDigit(strNumber, 0))
			{
				txtNumber.Text += strNumber;
			}

		}

		private void btnBack_Click(object sender, EventArgs e)
		{
			string strNumber = txtNumber.Text;

			if (strNumber.Length < 1) return;

			txtNumber.Text = strNumber.Substring(0, strNumber.Length - 1);

		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			strNumber = txtNumber.Text;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}








	}
}