using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Function;
using Function.Util;

namespace Function.uScm
{
	/// <summary>
	/// frmNumber에 대한 요약 설명입니다.
	/// </summary>
	public class frmNumber : System.Windows.Forms.Form
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		/// 
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtPasswordBack;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.GroupBox gbBox1;

		
		private Log clsLog;
        private string strPass = string.Empty;

		public frmNumber(Log _clsLog, string _strPass)
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
			//

			clsLog = _clsLog;
            strPass = _strPass.ToUpper().Trim(); ;
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmdOK = new System.Windows.Forms.Button();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtPasswordBack = new System.Windows.Forms.TextBox();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.gbBox1 = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point(192, 112);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(104, 40);
			this.cmdOK.TabIndex = 5;
			this.cmdOK.TabStop = false;
			this.cmdOK.Text = "비밀번호 확인";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// txtPassword
			// 
			this.txtPassword.AutoSize = false;
			this.txtPassword.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtPassword.Font = new System.Drawing.Font("HY견고딕", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(129)));
			this.txtPassword.Location = new System.Drawing.Point(32, 40);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(352, 40);
			this.txtPassword.TabIndex = 6;
			this.txtPassword.Text = "";
			this.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
			this.txtPassword.DoubleClick += new System.EventHandler(this.txtPassword_DoubleClick);
			// 
			// txtPasswordBack
			// 
			this.txtPasswordBack.AutoSize = false;
			this.txtPasswordBack.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.txtPasswordBack.Enabled = false;
			this.txtPasswordBack.Location = new System.Drawing.Point(16, 24);
			this.txtPasswordBack.Name = "txtPasswordBack";
			this.txtPasswordBack.Size = new System.Drawing.Size(384, 68);
			this.txtPasswordBack.TabIndex = 4;
			this.txtPasswordBack.TabStop = false;
			this.txtPasswordBack.Text = "";
			// 
			// cmdCancel
			// 
			this.cmdCancel.Location = new System.Drawing.Point(304, 112);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(104, 40);
			this.cmdCancel.TabIndex = 2;
			this.cmdCancel.TabStop = false;
			this.cmdCancel.Text = "취 소";
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// gbBox1
			// 
			this.gbBox1.Location = new System.Drawing.Point(8, 8);
			this.gbBox1.Name = "gbBox1";
			this.gbBox1.Size = new System.Drawing.Size(400, 92);
			this.gbBox1.TabIndex = 3;
			this.gbBox1.TabStop = false;
			this.gbBox1.Text = "비밀 번호 입력";
			// 
			// frmNumber
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(416, 157);
			this.ControlBox = false;
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.txtPasswordBack);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.gbBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "frmNumber";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmNumber_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmNumber_Load(object sender, System.EventArgs e)
		{
		
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			string	strPassword = "";

			try
			{
				// 입력한 비밀번호를 가져오자
				strPassword = txtPassword.Text.ToUpper().Trim();

				// 입력한 비밀번호와 저장되어 있는 비밀번호가 동일한지 확인				
                if (strPassword != strPass)
				{					
					MessageBox.Show("입력하신 비밀번호는 관리자 비밀번호와 일치하지 않습니다.", "비밀번호 확인");
					txtPassword.Focus();
					return;
				}

				this.DialogResult = DialogResult.OK;
				
				this.Close();
			}
			catch (Exception ex)
			{
				
				clsLog.WLog(string.Format("비밀번호를 확인하는 중에 오류가 발생했습니다 : {0}\r\n{1}",ex.Message, ex.ToString()));
 
			}
		
		}

		private void cmdCancel_Click(object sender, System.EventArgs e)
		{ 
			try
			{
				this.DialogResult = DialogResult.Cancel;
				
				this.Close();
			}
			catch (Exception ex)
			{ 	
				clsLog.WLog(string.Format("비밀번호 확인 화면을 취소하는 중에 오류가 발생했습니다. : {0}\r\n{1}",ex.Message, ex.ToString()));				

			}
			
		}

		private void txtPassword_DoubleClick(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;			
			this.Close();
		}

		private void txtPassword_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				cmdOK_Click(null, null);
			}
		}
	}
}
