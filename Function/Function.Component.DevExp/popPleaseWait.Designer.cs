namespace Function.Component.DevExp
{
	partial class popPleaseWait
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
			this.progressPanel1 = new DevExpress.XtraWaitForm.ProgressPanel();
			this.SuspendLayout();
			// 
			// progressPanel1
			// 
			this.progressPanel1.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.progressPanel1.Appearance.Font = new System.Drawing.Font("나눔고딕코딩", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.progressPanel1.Appearance.Options.UseBackColor = true;
			this.progressPanel1.Appearance.Options.UseFont = true;
			this.progressPanel1.AppearanceCaption.Font = new System.Drawing.Font("나눔고딕코딩", 12F);
			this.progressPanel1.AppearanceCaption.Options.UseFont = true;
			this.progressPanel1.AppearanceDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.progressPanel1.AppearanceDescription.Options.UseFont = true;
			this.progressPanel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
			this.progressPanel1.Caption = "잠시만 기다려 주십시요.";
			this.progressPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.progressPanel1.Location = new System.Drawing.Point(0, 0);
			this.progressPanel1.Name = "progressPanel1";
			this.progressPanel1.Size = new System.Drawing.Size(261, 63);
			this.progressPanel1.TabIndex = 0;
			this.progressPanel1.Text = "progressPanel1";
			// 
			// popPleaseWait
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(261, 63);
			this.Controls.Add(this.progressPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "popPleaseWait";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "popPleaseWait";
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraWaitForm.ProgressPanel progressPanel1;
	}
}