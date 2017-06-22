namespace Function.Advenced.IF
{
	partial class usrIF_Base
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
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.pnlBody = new System.Windows.Forms.Panel();
			this.lblTitle = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.lblShippingLasttime = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblInterval = new System.Windows.Forms.Label();
			this.pnlHeader.SuspendLayout();
			this.pnlBody.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlHeader
			// 
			this.pnlHeader.BackColor = System.Drawing.Color.RoyalBlue;
			this.pnlHeader.Controls.Add(this.lblShippingLasttime);
			this.pnlHeader.Controls.Add(this.lblInterval);
			this.pnlHeader.Controls.Add(this.label15);
			this.pnlHeader.Controls.Add(this.lblTitle);
			this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlHeader.Location = new System.Drawing.Point(0, 0);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(1016, 27);
			this.pnlHeader.TabIndex = 11;
			// 
			// pnlBody
			// 
			this.pnlBody.BackColor = System.Drawing.Color.Transparent;
			this.pnlBody.Controls.Add(this.label1);
			this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlBody.Location = new System.Drawing.Point(0, 27);
			this.pnlBody.Name = "pnlBody";
			this.pnlBody.Size = new System.Drawing.Size(1016, 513);
			this.pnlBody.TabIndex = 12;
			// 
			// lblTitle
			// 
			this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblTitle.BackColor = System.Drawing.Color.Transparent;
			this.lblTitle.Font = new System.Drawing.Font("굴림체", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblTitle.ForeColor = System.Drawing.Color.Yellow;
			this.lblTitle.Location = new System.Drawing.Point(3, 4);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(743, 19);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "Title";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label15
			// 
			this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label15.BackColor = System.Drawing.Color.Transparent;
			this.label15.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label15.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label15.Location = new System.Drawing.Point(607, 4);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(149, 19);
			this.label15.TabIndex = 2;
			this.label15.Text = "마지막 작업수행 :";
			this.label15.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// lblShippingLasttime
			// 
			this.lblShippingLasttime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblShippingLasttime.BackColor = System.Drawing.Color.Transparent;
			this.lblShippingLasttime.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblShippingLasttime.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.lblShippingLasttime.Location = new System.Drawing.Point(745, 4);
			this.lblShippingLasttime.Name = "lblShippingLasttime";
			this.lblShippingLasttime.Size = new System.Drawing.Size(149, 19);
			this.lblShippingLasttime.TabIndex = 1;
			this.lblShippingLasttime.Text = "작업 미시작";
			this.lblShippingLasttime.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(358, 216);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(308, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Log용 FpSpread를 디자이너 모드에서 생성하여\r\n유저 컨트롤에 fpLog에 바인딩 하신 후 디자인 하십시요.";
			// 
			// lblInterval
			// 
			this.lblInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblInterval.BackColor = System.Drawing.Color.Transparent;
			this.lblInterval.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblInterval.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.lblInterval.Location = new System.Drawing.Point(895, 4);
			this.lblInterval.Name = "lblInterval";
			this.lblInterval.Size = new System.Drawing.Size(118, 19);
			this.lblInterval.TabIndex = 3;
			this.lblInterval.Text = "(간격: 5000ms)";
			this.lblInterval.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// usrIF_Base
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Silver;
			this.Controls.Add(this.pnlBody);
			this.Controls.Add(this.pnlHeader);
			this.Name = "usrIF_Base";
			this.Size = new System.Drawing.Size(1016, 540);
			this.pnlHeader.ResumeLayout(false);
			this.pnlBody.ResumeLayout(false);
			this.pnlBody.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
				
		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label lblShippingLasttime;
		public System.Windows.Forms.Panel pnlBody;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblInterval;
	}
}
