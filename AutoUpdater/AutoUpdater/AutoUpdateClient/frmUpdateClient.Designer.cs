namespace AutoUpdateClient
{
	partial class frmUpdateClient
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

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpdateClient));
			this.pBarTotal = new System.Windows.Forms.ProgressBar();
			this.lblTotal = new System.Windows.Forms.Label();
			this.lblTotalInfo = new System.Windows.Forms.Label();
			this.lblSubInfo = new System.Windows.Forms.Label();
			this.lblSub = new System.Windows.Forms.Label();
			this.pBarSub = new System.Windows.Forms.ProgressBar();
			this.picLogo = new System.Windows.Forms.PictureBox();
			this.lblSubCount = new System.Windows.Forms.Label();
			this.lblVersion = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// pBarTotal
			// 
			this.pBarTotal.Location = new System.Drawing.Point(12, 66);
			this.pBarTotal.Name = "pBarTotal";
			this.pBarTotal.Size = new System.Drawing.Size(357, 17);
			this.pBarTotal.Step = 1;
			this.pBarTotal.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.pBarTotal.TabIndex = 1;
			// 
			// lblTotal
			// 
			this.lblTotal.BackColor = System.Drawing.Color.Transparent;
			this.lblTotal.ForeColor = System.Drawing.Color.White;
			this.lblTotal.Location = new System.Drawing.Point(375, 66);
			this.lblTotal.Name = "lblTotal";
			this.lblTotal.Size = new System.Drawing.Size(38, 17);
			this.lblTotal.TabIndex = 2;
			this.lblTotal.Text = "0%";
			this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTotalInfo
			// 
			this.lblTotalInfo.BackColor = System.Drawing.Color.Transparent;
			this.lblTotalInfo.ForeColor = System.Drawing.Color.White;
			this.lblTotalInfo.Location = new System.Drawing.Point(12, 86);
			this.lblTotalInfo.Name = "lblTotalInfo";
			this.lblTotalInfo.Size = new System.Drawing.Size(357, 19);
			this.lblTotalInfo.TabIndex = 5;
			this.lblTotalInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblSubInfo
			// 
			this.lblSubInfo.BackColor = System.Drawing.Color.Transparent;
			this.lblSubInfo.ForeColor = System.Drawing.Color.White;
			this.lblSubInfo.Location = new System.Drawing.Point(12, 128);
			this.lblSubInfo.Name = "lblSubInfo";
			this.lblSubInfo.Size = new System.Drawing.Size(314, 19);
			this.lblSubInfo.TabIndex = 8;
			this.lblSubInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblSub
			// 
			this.lblSub.BackColor = System.Drawing.Color.Transparent;
			this.lblSub.ForeColor = System.Drawing.Color.White;
			this.lblSub.Location = new System.Drawing.Point(375, 108);
			this.lblSub.Name = "lblSub";
			this.lblSub.Size = new System.Drawing.Size(38, 17);
			this.lblSub.TabIndex = 7;
			this.lblSub.Text = "0%";
			this.lblSub.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pBarSub
			// 
			this.pBarSub.Location = new System.Drawing.Point(12, 108);
			this.pBarSub.Name = "pBarSub";
			this.pBarSub.Size = new System.Drawing.Size(357, 17);
			this.pBarSub.Step = 1;
			this.pBarSub.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.pBarSub.TabIndex = 6;
			// 
			// picLogo
			// 
			this.picLogo.BackColor = System.Drawing.Color.Transparent;
			this.picLogo.Image = global::AutoUpdateClient.Properties.Resources.KGC_CI;
			this.picLogo.Location = new System.Drawing.Point(354, 7);
			this.picLogo.Name = "picLogo";
			this.picLogo.Size = new System.Drawing.Size(58, 53);
			this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picLogo.TabIndex = 9;
			this.picLogo.TabStop = false;
			// 
			// lblSubCount
			// 
			this.lblSubCount.BackColor = System.Drawing.Color.Transparent;
			this.lblSubCount.ForeColor = System.Drawing.Color.White;
			this.lblSubCount.Location = new System.Drawing.Point(351, 128);
			this.lblSubCount.Name = "lblSubCount";
			this.lblSubCount.Size = new System.Drawing.Size(44, 19);
			this.lblSubCount.TabIndex = 10;
			this.lblSubCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblVersion
			// 
			this.lblVersion.AutoSize = true;
			this.lblVersion.BackColor = System.Drawing.Color.Transparent;
			this.lblVersion.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblVersion.ForeColor = System.Drawing.Color.White;
			this.lblVersion.Location = new System.Drawing.Point(15, 167);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(19, 12);
			this.lblVersion.TabIndex = 11;
			this.lblVersion.Text = "v.";
			// 
			// frmUpdateClient
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.BackgroundImage = global::AutoUpdateClient.Properties.Resources.update;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(425, 198);
			this.Controls.Add(this.picLogo);
			this.Controls.Add(this.lblVersion);
			this.Controls.Add(this.lblSubCount);
			this.Controls.Add(this.lblSubInfo);
			this.Controls.Add(this.lblSub);
			this.Controls.Add(this.pBarSub);
			this.Controls.Add(this.lblTotalInfo);
			this.Controls.Add(this.lblTotal);
			this.Controls.Add(this.pBarTotal);
			this.Font = new System.Drawing.Font("굴림체", 10F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmUpdateClient";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "AutoUpdateClient";
			this.Load += new System.EventHandler(this.frmUpdateClient_Load);
			((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar pBarTotal;
		private System.Windows.Forms.Label lblTotal;
		private System.Windows.Forms.Label lblTotalInfo;
		private System.Windows.Forms.Label lblSubInfo;
		private System.Windows.Forms.Label lblSub;
		private System.Windows.Forms.ProgressBar pBarSub;
		private System.Windows.Forms.PictureBox picLogo;
		private System.Windows.Forms.Label lblSubCount;
		private System.Windows.Forms.Label lblVersion;
	}
}

