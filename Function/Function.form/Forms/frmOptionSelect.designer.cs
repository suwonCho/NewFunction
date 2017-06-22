namespace Function.form
{
	partial class frmOptionSelect
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptionSelect));
			this.btnYes = new System.Windows.Forms.Button();
			this.picClose = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblMsg = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.btnNo = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnYes
			// 
			this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnYes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnYes.Location = new System.Drawing.Point(187, 83);
			this.btnYes.Name = "btnYes";
			this.btnYes.Size = new System.Drawing.Size(67, 31);
			this.btnYes.TabIndex = 0;
			this.btnYes.Text = "  확 인";
			this.btnYes.UseVisualStyleBackColor = true;
			this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
			// 
			// picClose
			// 
			this.picClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.picClose.BackColor = System.Drawing.Color.Transparent;
			this.picClose.Image = ((System.Drawing.Image)(resources.GetObject("picClose.Image")));
			this.picClose.Location = new System.Drawing.Point(522, 6);
			this.picClose.Name = "picClose";
			this.picClose.Size = new System.Drawing.Size(19, 19);
			this.picClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picClose.TabIndex = 1;
			this.picClose.TabStop = false;
			this.picClose.Click += new System.EventHandler(this.picClose_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.lblMsg);
			this.panel1.Location = new System.Drawing.Point(8, 29);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(531, 49);
			this.panel1.TabIndex = 2;
			// 
			// lblMsg
			// 
			this.lblMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblMsg.AutoEllipsis = true;
			this.lblMsg.Location = new System.Drawing.Point(9, 7);
			this.lblMsg.Margin = new System.Windows.Forms.Padding(5);
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.Size = new System.Drawing.Size(513, 35);
			this.lblMsg.TabIndex = 0;
			this.lblMsg.Text = "label2";
			this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblMsg.DoubleClick += new System.EventHandler(this.lblMsg_DoubleClick);
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.BackColor = System.Drawing.Color.Transparent;
			this.lblTitle.Font = new System.Drawing.Font("굴림체", 12.25F, System.Drawing.FontStyle.Bold);
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(12, 6);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(58, 17);
			this.lblTitle.TabIndex = 3;
			this.lblTitle.Text = "Title";
			// 
			// btnNo
			// 
			this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnNo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnNo.Location = new System.Drawing.Point(289, 83);
			this.btnNo.Name = "btnNo";
			this.btnNo.Size = new System.Drawing.Size(67, 31);
			this.btnNo.TabIndex = 4;
			this.btnNo.Text = "    취  소";
			this.btnNo.UseVisualStyleBackColor = true;
			this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
			// 
			// frmOptionSelect
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(548, 120);
			this.Controls.Add(this.btnNo);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.picClose);
			this.Controls.Add(this.btnYes);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmOptionSelect";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "frmMessage";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMessage_FormClosed);
			this.Load += new System.EventHandler(this.frmMessage_Load);
			((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnYes;
		private System.Windows.Forms.PictureBox picClose;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Button btnNo;
		private System.Windows.Forms.Label lblMsg;
	}
}