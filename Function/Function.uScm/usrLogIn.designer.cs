namespace Function.uScm
{
	partial class usrLogIn
	{
		/// <summary> 
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 구성 요소 디자이너에서 생성한 코드

		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.cmbGubun = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbWorker = new System.Windows.Forms.ComboBox();
			this.btnLogin = new System.Windows.Forms.Button();
			this.lblGubun = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label1.Location = new System.Drawing.Point(-4, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(467, 36);
			this.label1.TabIndex = 0;
			this.label1.Text = "작업 사용자를 선택 하여 주십시요.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmbGubun
			// 
			this.cmbGubun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbGubun.Font = new System.Drawing.Font("굴림", 25F);
			this.cmbGubun.FormattingEnabled = true;
			this.cmbGubun.Location = new System.Drawing.Point(178, 73);
			this.cmbGubun.Name = "cmbGubun";
			this.cmbGubun.Size = new System.Drawing.Size(261, 41);
			this.cmbGubun.TabIndex = 10;
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label6.Font = new System.Drawing.Font("굴림체", 15F, System.Drawing.FontStyle.Bold);
			this.label6.Location = new System.Drawing.Point(18, 73);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(145, 41);
			this.label6.TabIndex = 9;
			this.label6.Text = "사용자 구분";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label2.Font = new System.Drawing.Font("굴림체", 15F, System.Drawing.FontStyle.Bold);
			this.label2.Location = new System.Drawing.Point(18, 127);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(145, 41);
			this.label2.TabIndex = 11;
			this.label2.Text = "사 용 자";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmbWorker
			// 
			this.cmbWorker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbWorker.Font = new System.Drawing.Font("굴림", 25F);
			this.cmbWorker.FormattingEnabled = true;
			this.cmbWorker.Location = new System.Drawing.Point(178, 127);
			this.cmbWorker.Name = "cmbWorker";
			this.cmbWorker.Size = new System.Drawing.Size(181, 41);
			this.cmbWorker.TabIndex = 12;
			this.cmbWorker.SelectedIndexChanged += new System.EventHandler(this.cmbWorker_SelectedIndexChanged);
			// 
			// btnLogin
			// 
			this.btnLogin.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold);
			this.btnLogin.Location = new System.Drawing.Point(365, 127);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(74, 41);
			this.btnLogin.TabIndex = 13;
			this.btnLogin.Text = "로그인";
			this.btnLogin.UseVisualStyleBackColor = true;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// lblGubun
			// 
			this.lblGubun.BackColor = System.Drawing.Color.RoyalBlue;
			this.lblGubun.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblGubun.Font = new System.Drawing.Font("굴림체", 25F, System.Drawing.FontStyle.Bold);
			this.lblGubun.ForeColor = System.Drawing.Color.Yellow;
			this.lblGubun.Location = new System.Drawing.Point(178, 73);
			this.lblGubun.Name = "lblGubun";
			this.lblGubun.Size = new System.Drawing.Size(261, 41);
			this.lblGubun.TabIndex = 14;
			this.lblGubun.Text = "사출 사용자";
			this.lblGubun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// usrLogIn
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.btnLogin);
			this.Controls.Add(this.cmbWorker);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbGubun);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblGubun);
			this.Name = "usrLogIn";
			this.Size = new System.Drawing.Size(454, 208);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbGubun;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbWorker;
		private System.Windows.Forms.Label lblGubun;
		public System.Windows.Forms.Button btnLogin;
	}
}
