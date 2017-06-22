namespace AutoUpdater
{
	partial class frmGroup
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.grdGroup = new DevExpress.XtraGrid.GridControl();
			this.grvGroup = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.grdGroup)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.grvGroup)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// grdGroup
			// 
			this.grdGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdGroup.Location = new System.Drawing.Point(0, 0);
			this.grdGroup.MainView = this.grvGroup;
			this.grdGroup.Name = "grdGroup";
			this.grdGroup.Size = new System.Drawing.Size(523, 272);
			this.grdGroup.TabIndex = 1;
			this.grdGroup.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grvGroup});
			// 
			// grvGroup
			// 
			this.grvGroup.GridControl = this.grdGroup;
			this.grvGroup.Name = "grvGroup";
			this.grvGroup.OptionsEditForm.EditFormColumnCount = 2;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnSave);
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 272);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(523, 33);
			this.panel1.TabIndex = 2;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(368, 4);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(72, 24);
			this.btnSave.TabIndex = 4;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(446, 4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(72, 24);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "취 소";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// frmGroup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(523, 327);
			this.Controls.Add(this.grdGroup);
			this.Controls.Add(this.panel1);
			this.Name = "frmGroup";
			this.Text = "Update Group 관리";
			this.Load += new System.EventHandler(this.frmGroup_Load);
			this.Controls.SetChildIndex(this.panel1, 0);
			this.Controls.SetChildIndex(this.grdGroup, 0);
			((System.ComponentModel.ISupportInitialize)(this.grdGroup)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.grvGroup)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraGrid.GridControl grdGroup;
		private DevExpress.XtraGrid.Views.Grid.GridView grvGroup;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
	}
}