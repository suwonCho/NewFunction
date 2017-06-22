namespace Function.form
{
	partial class frmWating
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWating));
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblMsg = new System.Windows.Forms.Label();
			this.pic = new System.Windows.Forms.PictureBox();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.lblMsg);
			this.panel1.Location = new System.Drawing.Point(45, 10);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(495, 36);
			this.panel1.TabIndex = 2;
			// 
			// lblMsg
			// 
			this.lblMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblMsg.AutoEllipsis = true;
			this.lblMsg.Location = new System.Drawing.Point(10, 10);
			this.lblMsg.Margin = new System.Windows.Forms.Padding(5);
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.Size = new System.Drawing.Size(477, 18);
			this.lblMsg.TabIndex = 0;
			this.lblMsg.Text = "label2";
			this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pic
			// 
			this.pic.Image = ((System.Drawing.Image)(resources.GetObject("pic.Image")));
			this.pic.Location = new System.Drawing.Point(7, 10);
			this.pic.Name = "pic";
			this.pic.Size = new System.Drawing.Size(36, 36);
			this.pic.TabIndex = 3;
			this.pic.TabStop = false;
			// 
			// frmWating
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(548, 58);
			this.Controls.Add(this.pic);
			this.Controls.Add(this.panel1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmWating";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "frmMessage";
			this.Load += new System.EventHandler(this.frmWating_Load);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblMsg;
		private System.Windows.Forms.PictureBox pic;
	}
}