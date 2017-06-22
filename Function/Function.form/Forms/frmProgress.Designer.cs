namespace Function.form
{
	partial class frmProgress
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
			this.pgbEach = new System.Windows.Forms.ProgressBar();
			this.pgbAll = new System.Windows.Forms.ProgressBar();
			this.lblTitle = new System.Windows.Forms.Label();
			this.lblDetail = new System.Windows.Forms.Label();
			this.lblEachPer = new System.Windows.Forms.Label();
			this.lblEachCnt = new System.Windows.Forms.Label();
			this.lblAllCnt = new System.Windows.Forms.Label();
			this.lblAllPer = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// pgbEach
			// 
			this.pgbEach.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pgbEach.Location = new System.Drawing.Point(12, 56);
			this.pgbEach.Name = "pgbEach";
			this.pgbEach.Size = new System.Drawing.Size(364, 31);
			this.pgbEach.TabIndex = 0;
			// 
			// pgbAll
			// 
			this.pgbAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pgbAll.Location = new System.Drawing.Point(12, 93);
			this.pgbAll.Name = "pgbAll";
			this.pgbAll.Size = new System.Drawing.Size(364, 31);
			this.pgbAll.TabIndex = 1;
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Location = new System.Drawing.Point(199, 9);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(41, 12);
			this.lblTitle.TabIndex = 2;
			this.lblTitle.Text = "타이틀";
			// 
			// lblDetail
			// 
			this.lblDetail.Location = new System.Drawing.Point(12, 30);
			this.lblDetail.Name = "lblDetail";
			this.lblDetail.Size = new System.Drawing.Size(412, 23);
			this.lblDetail.TabIndex = 3;
			// 
			// lblEachPer
			// 
			this.lblEachPer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblEachPer.AutoSize = true;
			this.lblEachPer.Location = new System.Drawing.Point(375, 65);
			this.lblEachPer.Name = "lblEachPer";
			this.lblEachPer.Size = new System.Drawing.Size(33, 12);
			this.lblEachPer.TabIndex = 4;
			this.lblEachPer.Text = "100%";
			// 
			// lblEachCnt
			// 
			this.lblEachCnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblEachCnt.Location = new System.Drawing.Point(375, 72);
			this.lblEachCnt.Name = "lblEachCnt";
			this.lblEachCnt.Size = new System.Drawing.Size(49, 28);
			this.lblEachCnt.TabIndex = 5;
			this.lblEachCnt.Text = "0/0";
			this.lblEachCnt.Visible = false;
			// 
			// lblAllCnt
			// 
			this.lblAllCnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblAllCnt.Location = new System.Drawing.Point(375, 110);
			this.lblAllCnt.Name = "lblAllCnt";
			this.lblAllCnt.Size = new System.Drawing.Size(49, 28);
			this.lblAllCnt.TabIndex = 7;
			this.lblAllCnt.Text = "0/0";
			// 
			// lblAllPer
			// 
			this.lblAllPer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblAllPer.AutoSize = true;
			this.lblAllPer.Location = new System.Drawing.Point(375, 96);
			this.lblAllPer.Name = "lblAllPer";
			this.lblAllPer.Size = new System.Drawing.Size(33, 12);
			this.lblAllPer.TabIndex = 6;
			this.lblAllPer.Text = "100%";
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(331, 126);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(93, 28);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "취  소";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// frmProgress
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(425, 156);
			this.Controls.Add(this.pgbEach);
			this.Controls.Add(this.lblEachPer);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.pgbAll);
			this.Controls.Add(this.lblAllCnt);
			this.Controls.Add(this.lblAllPer);
			this.Controls.Add(this.lblEachCnt);
			this.Controls.Add(this.lblDetail);
			this.Controls.Add(this.lblTitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Name = "frmProgress";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "진행 상태";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar pgbEach;
		private System.Windows.Forms.ProgressBar pgbAll;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Label lblDetail;
		private System.Windows.Forms.Label lblEachPer;
		private System.Windows.Forms.Label lblEachCnt;
		private System.Windows.Forms.Label lblAllCnt;
		private System.Windows.Forms.Label lblAllPer;
		private System.Windows.Forms.Button btnCancel;
	}
}