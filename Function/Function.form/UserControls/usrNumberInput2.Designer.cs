namespace Function.form
{
	partial class usrNumberInput2
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
			this.pnlTop = new System.Windows.Forms.Panel();
			this.lblNumber = new System.Windows.Forms.Label();
			this.pnlBody = new System.Windows.Forms.Panel();
			this.pnlTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlTop
			// 
			this.pnlTop.BackColor = System.Drawing.Color.Gold;
			this.pnlTop.Controls.Add(this.lblNumber);
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTop.Location = new System.Drawing.Point(0, 0);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(455, 44);
			this.pnlTop.TabIndex = 0;
			// 
			// lblNumber
			// 
			this.lblNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblNumber.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblNumber.Font = new System.Drawing.Font("굴림", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblNumber.Location = new System.Drawing.Point(0, 0);
			this.lblNumber.Name = "lblNumber";
			this.lblNumber.Size = new System.Drawing.Size(455, 44);
			this.lblNumber.TabIndex = 0;
			this.lblNumber.Text = "○○○○○";
			this.lblNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlBody
			// 
			this.pnlBody.BackColor = System.Drawing.Color.PowderBlue;
			this.pnlBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlBody.Location = new System.Drawing.Point(0, 44);
			this.pnlBody.Name = "pnlBody";
			this.pnlBody.Size = new System.Drawing.Size(455, 99);
			this.pnlBody.TabIndex = 1;
			// 
			// usrNumberInput2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlBody);
			this.Controls.Add(this.pnlTop);
			this.Name = "usrNumberInput2";
			this.Size = new System.Drawing.Size(455, 143);
			this.Load += new System.EventHandler(this.usrNumberInput2_Load);
			this.SizeChanged += new System.EventHandler(this.usrNumberInput2_SizeChanged);
			this.pnlTop.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.Label lblNumber;
		private System.Windows.Forms.Panel pnlBody;
	}
}
