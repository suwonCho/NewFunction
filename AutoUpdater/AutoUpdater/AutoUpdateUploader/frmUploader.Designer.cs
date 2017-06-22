namespace AutoUpdater
{
	partial class frmUploader
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUploader));
			this.mnuMain = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tstGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.updatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tsiUpdatePoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// mnuMain
			// 
			this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.updatesToolStripMenuItem});
			this.mnuMain.Location = new System.Drawing.Point(0, 0);
			this.mnuMain.Name = "mnuMain";
			this.mnuMain.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
			this.mnuMain.Size = new System.Drawing.Size(1046, 24);
			this.mnuMain.TabIndex = 1;
			this.mnuMain.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tstGroup,
            this.tsiUpdatePoint});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			this.fileToolStripMenuItem.Text = "&Setting";
			// 
			// tstGroup
			// 
			this.tstGroup.Name = "tstGroup";
			this.tstGroup.Size = new System.Drawing.Size(164, 22);
			this.tstGroup.Tag = "0";
			this.tstGroup.Text = "Group 관리";
			this.tstGroup.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
			// 
			// updatesToolStripMenuItem
			// 
			this.updatesToolStripMenuItem.Name = "updatesToolStripMenuItem";
			this.updatesToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
			this.updatesToolStripMenuItem.Text = "Updates";
			// 
			// tsiUpdatePoint
			// 
			this.tsiUpdatePoint.Name = "tsiUpdatePoint";
			this.tsiUpdatePoint.Size = new System.Drawing.Size(164, 22);
			this.tsiUpdatePoint.Tag = "1";
			this.tsiUpdatePoint.Text = "UpdatePoint관리";
			this.tsiUpdatePoint.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
			// 
			// frmUploader
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1046, 664);
			this.Controls.Add(this.mnuMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.MainMenuStrip = this.mnuMain;
			this.Name = "frmUploader";
			this.SavePosition = true;
			this.ShowStatusBar = false;
			this.Text = "Update Uploader";
			this.Load += new System.EventHandler(this.frmUploader_Load);
			this.Controls.SetChildIndex(this.mnuMain, 0);
			this.mnuMain.ResumeLayout(false);
			this.mnuMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mnuMain;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem updatesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tstGroup;
		private System.Windows.Forms.ToolStripMenuItem tsiUpdatePoint;
	}
}