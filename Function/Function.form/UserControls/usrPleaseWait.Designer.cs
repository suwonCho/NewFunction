namespace Function.form.UserControls
{
	partial class usrPleaseWait
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
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
		/// </summary>
		private void InitializeComponent()
		{
			this.pnlBody = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.usrGifPicbox1 = new Function.form.UserControls.usrGifPicbox();
			this.pnlBody.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.usrGifPicbox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlBody
			// 
			this.pnlBody.BackColor = System.Drawing.Color.Transparent;
			this.pnlBody.Controls.Add(this.label1);
			this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlBody.Location = new System.Drawing.Point(65, 4);
			this.pnlBody.Name = "pnlBody";
			this.pnlBody.Padding = new System.Windows.Forms.Padding(6);
			this.pnlBody.Size = new System.Drawing.Size(370, 57);
			this.pnlBody.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("굴림체", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label1.Location = new System.Drawing.Point(6, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(358, 45);
			this.label1.TabIndex = 0;
			this.label1.Text = "초기화 중 입니다. 잠시만 기다려 주십시요.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// usrGifPicbox1
			// 
			this.usrGifPicbox1.BackColor = System.Drawing.Color.Transparent;
			this.usrGifPicbox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.usrGifPicbox1.Image = null;
			this.usrGifPicbox1.Location = new System.Drawing.Point(4, 4);
			this.usrGifPicbox1.Name = "usrGifPicbox1";
			this.usrGifPicbox1.Size = new System.Drawing.Size(61, 57);
			this.usrGifPicbox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.usrGifPicbox1.TabIndex = 0;
			this.usrGifPicbox1.TabStop = false;
			// 
			// usrPleaseWait
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlBody);
			this.Controls.Add(this.usrGifPicbox1);
			this.Name = "usrPleaseWait";
			this.Padding = new System.Windows.Forms.Padding(4);
			this.Size = new System.Drawing.Size(439, 65);
			this.Load += new System.EventHandler(this.usrPleaseWait_Load);
			this.pnlBody.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.usrGifPicbox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private usrGifPicbox usrGifPicbox1;
		private System.Windows.Forms.Panel pnlBody;
		private System.Windows.Forms.Label label1;
	}
}
