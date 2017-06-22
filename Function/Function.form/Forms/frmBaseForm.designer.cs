namespace Function.form
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBaseForm));
			this.stBar = new System.Windows.Forms.StatusStrip();
			this.pgrBar = new System.Windows.Forms.ToolStripProgressBar();
			this.tsLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsStatus = new System.Windows.Forms.ToolStripSplitButton();
			this.Notifyicon = new System.Windows.Forms.NotifyIcon(this.components);
			this.stBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// stBar
			// 
			this.stBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pgrBar,
            this.tsLabel,
            this.tsStatus});
			this.stBar.Location = new System.Drawing.Point(0, 544);
			this.stBar.Name = "stBar";
			this.stBar.Size = new System.Drawing.Size(792, 22);
			this.stBar.TabIndex = 0;
			this.stBar.Text = "statusStrip1";
			// 
			// pgrBar
			// 
			this.pgrBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.pgrBar.Name = "pgrBar";
			this.pgrBar.Size = new System.Drawing.Size(150, 16);
			// 
			// tsLabel
			// 
			this.tsLabel.AutoSize = false;
			this.tsLabel.Name = "tsLabel";
			this.tsLabel.Size = new System.Drawing.Size(600, 17);
			this.tsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tsStatus
			// 
			this.tsStatus.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tsStatus.Name = "tsStatus";
			this.tsStatus.Size = new System.Drawing.Size(16, 20);
			this.tsStatus.Visible = false;
			// 
			// Notifyicon
			// 
			this.Notifyicon.Icon = ((System.Drawing.Icon)(resources.GetObject("Notifyicon.Icon")));
			this.Notifyicon.Text = "notifyIcon1";
			this.Notifyicon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Notifyicon_MouseDoubleClick);
			// 
			// frmBaseForm
			// 
			this.ClientSize = new System.Drawing.Size(792, 566);
			this.Controls.Add(this.stBar);
			this.Font = new System.Drawing.Font("굴림체", 9F);
			this.Name = "frmBaseForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBaseForm_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmBaseForm_FormClosed);
			this.Load += new System.EventHandler(this.frmBasic_Load);
			this.LocationChanged += new System.EventHandler(this.frmBaseForm_LocationChanged);
			this.SizeChanged += new System.EventHandler(this.frmBaseForm_SizeChanged);
			this.Resize += new System.EventHandler(this.frmBaseForm_Resize);
			this.stBar.ResumeLayout(false);
			this.stBar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStripStatusLabel tsLabel;
		private System.Windows.Forms.ToolStripSplitButton tsStatus;
		protected System.Windows.Forms.ToolStripProgressBar pgrBar;
		protected System.Windows.Forms.StatusStrip stBar;
		public System.Windows.Forms.NotifyIcon Notifyicon;
	}
}