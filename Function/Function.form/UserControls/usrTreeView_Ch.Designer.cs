namespace Function.form
{
	partial class usrTreeView_Ch
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.trMain = new System.Windows.Forms.TreeView();
			this.pnlButton = new System.Windows.Forms.SplitContainer();
			this.btnMC_Up = new System.Windows.Forms.Button();
			this.btnMC_Down = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pnlButton)).BeginInit();
			this.pnlButton.Panel1.SuspendLayout();
			this.pnlButton.Panel2.SuspendLayout();
			this.pnlButton.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.trMain);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.pnlButton);
			this.splitContainer1.Size = new System.Drawing.Size(370, 150);
			this.splitContainer1.SplitterDistance = 344;
			this.splitContainer1.SplitterWidth = 1;
			this.splitContainer1.TabIndex = 0;
			// 
			// trMain
			// 
			this.trMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trMain.HideSelection = false;
			this.trMain.Location = new System.Drawing.Point(0, 0);
			this.trMain.Name = "trMain";
			this.trMain.Size = new System.Drawing.Size(344, 150);
			this.trMain.TabIndex = 10;
			this.trMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trMain_AfterSelect);
			// 
			// pnlButton
			// 
			this.pnlButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlButton.Location = new System.Drawing.Point(0, 0);
			this.pnlButton.Name = "pnlButton";
			this.pnlButton.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// pnlButton.Panel1
			// 
			this.pnlButton.Panel1.Controls.Add(this.btnMC_Up);
			// 
			// pnlButton.Panel2
			// 
			this.pnlButton.Panel2.Controls.Add(this.btnMC_Down);
			this.pnlButton.Size = new System.Drawing.Size(25, 150);
			this.pnlButton.SplitterDistance = 75;
			this.pnlButton.SplitterWidth = 1;
			this.pnlButton.TabIndex = 0;
			// 
			// btnMC_Up
			// 
			this.btnMC_Up.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnMC_Up.Location = new System.Drawing.Point(0, 0);
			this.btnMC_Up.Name = "btnMC_Up";
			this.btnMC_Up.Size = new System.Drawing.Size(25, 75);
			this.btnMC_Up.TabIndex = 25;
			this.btnMC_Up.UseVisualStyleBackColor = true;
			this.btnMC_Up.Click += new System.EventHandler(this.btnMC_Up_Click);
			// 
			// btnMC_Down
			// 
			this.btnMC_Down.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnMC_Down.Location = new System.Drawing.Point(0, 0);
			this.btnMC_Down.Name = "btnMC_Down";
			this.btnMC_Down.Size = new System.Drawing.Size(25, 74);
			this.btnMC_Down.TabIndex = 26;
			this.btnMC_Down.UseVisualStyleBackColor = true;
			this.btnMC_Down.Click += new System.EventHandler(this.btnMC_Down_Click);
			// 
			// usrTreeView_Ch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Name = "usrTreeView_Ch";
			this.Size = new System.Drawing.Size(370, 150);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.pnlButton.Panel1.ResumeLayout(false);
			this.pnlButton.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pnlButton)).EndInit();
			this.pnlButton.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer pnlButton;
		private System.Windows.Forms.TreeView trMain;
		private System.Windows.Forms.Button btnMC_Up;
		private System.Windows.Forms.Button btnMC_Down;
	}
}
