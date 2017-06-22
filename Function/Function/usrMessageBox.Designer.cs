namespace Function
{
	partial class usrMessageBox
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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnNO = new System.Windows.Forms.Button();
			this.btnYes = new System.Windows.Forms.Button();
			this.lblMsg2 = new System.Windows.Forms.Label();
			this.lblMsg1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnOK.Location = new System.Drawing.Point(317, 102);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(91, 35);
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "확  인";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Visible = false;
			this.btnOK.Click += new System.EventHandler(this.btn_Click);
			// 
			// btnNO
			// 
			this.btnNO.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnNO.Location = new System.Drawing.Point(488, 102);
			this.btnNO.Name = "btnNO";
			this.btnNO.Size = new System.Drawing.Size(91, 35);
			this.btnNO.TabIndex = 6;
			this.btnNO.Text = "아니요";
			this.btnNO.UseVisualStyleBackColor = true;
			this.btnNO.Visible = false;
			this.btnNO.Click += new System.EventHandler(this.btn_Click);
			// 
			// btnYes
			// 
			this.btnYes.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnYes.Location = new System.Drawing.Point(144, 102);
			this.btnYes.Name = "btnYes";
			this.btnYes.Size = new System.Drawing.Size(91, 35);
			this.btnYes.TabIndex = 5;
			this.btnYes.Text = "예";
			this.btnYes.UseVisualStyleBackColor = true;
			this.btnYes.Visible = false;
			this.btnYes.Click += new System.EventHandler(this.btn_Click);
			// 
			// lblMsg2
			// 
			this.lblMsg2.Font = new System.Drawing.Font("굴림체", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblMsg2.Location = new System.Drawing.Point(17, 39);
			this.lblMsg2.Name = "lblMsg2";
			this.lblMsg2.Size = new System.Drawing.Size(718, 60);
			this.lblMsg2.TabIndex = 9;
			// 
			// lblMsg1
			// 
			this.lblMsg1.AutoSize = true;
			this.lblMsg1.Font = new System.Drawing.Font("굴림체", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblMsg1.Location = new System.Drawing.Point(17, 12);
			this.lblMsg1.Name = "lblMsg1";
			this.lblMsg1.Size = new System.Drawing.Size(0, 16);
			this.lblMsg1.TabIndex = 8;
			// 
			// usrMessageBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.lblMsg2);
			this.Controls.Add(this.lblMsg1);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnNO);
			this.Controls.Add(this.btnYes);
			this.Name = "usrMessageBox";
			this.Size = new System.Drawing.Size(748, 148);
			this.Load += new System.EventHandler(this.usrMessageBox_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnNO;
		private System.Windows.Forms.Button btnYes;
		private System.Windows.Forms.Label lblMsg2;
		private System.Windows.Forms.Label lblMsg1;
	}
}
