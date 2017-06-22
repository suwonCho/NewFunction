namespace Function.uScm
{
    partial class frmBaseForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBaseForm));
			this.picLogo = new System.Windows.Forms.PictureBox();
			this.pnlTitle = new System.Windows.Forms.Panel();
			this.lblNowTime = new System.Windows.Forms.Label();
			this.lblStartTime = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lbl01 = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.pnlInit = new System.Windows.Forms.Panel();
			this.lblWorking = new System.Windows.Forms.Label();
			this.lblPnl = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.grpMessageBox = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
			this.pnlTitle.SuspendLayout();
			this.pnlInit.SuspendLayout();
			this.grpMessageBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// picLogo
			// 
			this.picLogo.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.picLogo.Location = new System.Drawing.Point(0, 0);
			this.picLogo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.picLogo.Name = "picLogo";
			this.picLogo.Size = new System.Drawing.Size(78, 80);
			this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picLogo.TabIndex = 0;
			this.picLogo.TabStop = false;
			// 
			// pnlTitle
			// 
			this.pnlTitle.BackColor = System.Drawing.SystemColors.Highlight;
			this.pnlTitle.Controls.Add(this.lblNowTime);
			this.pnlTitle.Controls.Add(this.lblStartTime);
			this.pnlTitle.Controls.Add(this.label1);
			this.pnlTitle.Controls.Add(this.lbl01);
			this.pnlTitle.Controls.Add(this.lblTitle);
			this.pnlTitle.Controls.Add(this.picLogo);
			this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTitle.Location = new System.Drawing.Point(0, 0);
			this.pnlTitle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.pnlTitle.Name = "pnlTitle";
			this.pnlTitle.Size = new System.Drawing.Size(1024, 80);
			this.pnlTitle.TabIndex = 1;
			// 
			// lblNowTime
			// 
			this.lblNowTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblNowTime.Font = new System.Drawing.Font("굴림체", 12F, System.Drawing.FontStyle.Bold);
			this.lblNowTime.Location = new System.Drawing.Point(801, 44);
			this.lblNowTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblNowTime.Name = "lblNowTime";
			this.lblNowTime.Size = new System.Drawing.Size(192, 28);
			this.lblNowTime.TabIndex = 4;
			this.lblNowTime.Text = "2009-01-01 00:00:00";
			this.lblNowTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblStartTime
			// 
			this.lblStartTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblStartTime.Font = new System.Drawing.Font("굴림체", 12F, System.Drawing.FontStyle.Bold);
			this.lblStartTime.Location = new System.Drawing.Point(801, 9);
			this.lblStartTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblStartTime.Name = "lblStartTime";
			this.lblStartTime.Size = new System.Drawing.Size(192, 28);
			this.lblStartTime.TabIndex = 3;
			this.lblStartTime.Text = "2009-01-01 00:00:00";
			this.lblStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("굴림체", 12F, System.Drawing.FontStyle.Bold);
			this.label1.Location = new System.Drawing.Point(711, 44);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 28);
			this.label1.TabIndex = 3;
			this.label1.Text = "현재시간 :";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbl01
			// 
			this.lbl01.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lbl01.Font = new System.Drawing.Font("굴림체", 12F, System.Drawing.FontStyle.Bold);
			this.lbl01.Location = new System.Drawing.Point(711, 9);
			this.lbl01.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lbl01.Name = "lbl01";
			this.lbl01.Size = new System.Drawing.Size(96, 28);
			this.lbl01.TabIndex = 2;
			this.lbl01.Text = "시작시간 :";
			this.lbl01.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(95, 10);
			this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(118, 48);
			this.lblTitle.TabIndex = 1;
			this.lblTitle.Text = "Title";
			// 
			// pnlInit
			// 
			this.pnlInit.BackColor = System.Drawing.SystemColors.ControlDark;
			this.pnlInit.Controls.Add(this.lblWorking);
			this.pnlInit.Controls.Add(this.lblPnl);
			this.pnlInit.Location = new System.Drawing.Point(272, 241);
			this.pnlInit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.pnlInit.Name = "pnlInit";
			this.pnlInit.Size = new System.Drawing.Size(510, 286);
			this.pnlInit.TabIndex = 2;
			// 
			// lblWorking
			// 
			this.lblWorking.AutoSize = true;
			this.lblWorking.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblWorking.ForeColor = System.Drawing.Color.Yellow;
			this.lblWorking.Location = new System.Drawing.Point(38, 182);
			this.lblWorking.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblWorking.Name = "lblWorking";
			this.lblWorking.Size = new System.Drawing.Size(0, 27);
			this.lblWorking.TabIndex = 1;
			// 
			// lblPnl
			// 
			this.lblPnl.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblPnl.Location = new System.Drawing.Point(18, 31);
			this.lblPnl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblPnl.Name = "lblPnl";
			this.lblPnl.Size = new System.Drawing.Size(472, 121);
			this.lblPnl.TabIndex = 0;
			this.lblPnl.Text = "프로그램을 초기화 중입니다.\r\n잠시만 기다려 주십시요.";
			this.lblPnl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblMessage
			// 
			this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblMessage.BackColor = System.Drawing.SystemColors.WindowText;
			this.lblMessage.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblMessage.ForeColor = System.Drawing.Color.Yellow;
			this.lblMessage.Location = new System.Drawing.Point(6, 19);
			this.lblMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(1002, 35);
			this.lblMessage.TabIndex = 3;
			this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// grpMessageBox
			// 
			this.grpMessageBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpMessageBox.Controls.Add(this.lblMessage);
			this.grpMessageBox.Font = new System.Drawing.Font("굴림", 12F);
			this.grpMessageBox.Location = new System.Drawing.Point(5, 648);
			this.grpMessageBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.grpMessageBox.Name = "grpMessageBox";
			this.grpMessageBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.grpMessageBox.Size = new System.Drawing.Size(1013, 61);
			this.grpMessageBox.TabIndex = 4;
			this.grpMessageBox.TabStop = false;
			this.grpMessageBox.Text = "메세지";
			// 
			// frmBaseForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1024, 712);
			this.Controls.Add(this.grpMessageBox);
			this.Controls.Add(this.pnlInit);
			this.Controls.Add(this.pnlTitle);
			this.Font = new System.Drawing.Font("굴림체", 9.5F);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.Name = "frmBaseForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "BaseForm";
			this.Load += new System.EventHandler(this.frmBaseForm_Load);
			this.SizeChanged += new System.EventHandler(this.frmBaseForm_SizeChanged);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBaseForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
			this.pnlTitle.ResumeLayout(false);
			this.pnlTitle.PerformLayout();
			this.pnlInit.ResumeLayout(false);
			this.pnlInit.PerformLayout();
			this.grpMessageBox.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblNowTime;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl01;
        private System.Windows.Forms.Panel pnlInit;
        private System.Windows.Forms.Label lblWorking;
        private System.Windows.Forms.Label lblPnl;
		private System.Windows.Forms.Label lblMessage;
		protected System.Windows.Forms.Label lblTitle;
		public System.Windows.Forms.GroupBox grpMessageBox;
		public System.Windows.Forms.Panel pnlTitle;
    }
}