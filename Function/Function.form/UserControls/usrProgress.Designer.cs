namespace Function.form
{
	partial class usrProgress
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
			this.pnlBody = new System.Windows.Forms.Panel();
			this.pgbEach = new System.Windows.Forms.ProgressBar();
			this.lblEachPer = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.pgbAll = new System.Windows.Forms.ProgressBar();
			this.lblAllCnt = new System.Windows.Forms.Label();
			this.lblAllPer = new System.Windows.Forms.Label();
			this.lblEachCnt = new System.Windows.Forms.Label();
			this.lblDetail = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.pnlBody.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBody
			// 
			this.pnlBody.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlBody.Controls.Add(this.pgbEach);
			this.pnlBody.Controls.Add(this.lblEachPer);
			this.pnlBody.Controls.Add(this.btnCancel);
			this.pnlBody.Controls.Add(this.pgbAll);
			this.pnlBody.Controls.Add(this.lblAllCnt);
			this.pnlBody.Controls.Add(this.lblAllPer);
			this.pnlBody.Controls.Add(this.lblEachCnt);
			this.pnlBody.Controls.Add(this.lblDetail);
			this.pnlBody.Controls.Add(this.lblTitle);
			this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlBody.Location = new System.Drawing.Point(0, 0);
			this.pnlBody.Name = "pnlBody";
			this.pnlBody.Size = new System.Drawing.Size(430, 161);
			this.pnlBody.TabIndex = 0;
			// 
			// pgbEach
			// 
			this.pgbEach.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pgbEach.Location = new System.Drawing.Point(7, 47);
			this.pgbEach.Name = "pgbEach";
			this.pgbEach.Size = new System.Drawing.Size(359, 31);
			this.pgbEach.TabIndex = 9;
			// 
			// lblEachPer
			// 
			this.lblEachPer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblEachPer.AutoSize = true;
			this.lblEachPer.Location = new System.Drawing.Point(372, 56);
			this.lblEachPer.Name = "lblEachPer";
			this.lblEachPer.Size = new System.Drawing.Size(33, 12);
			this.lblEachPer.TabIndex = 13;
			this.lblEachPer.Text = "100%";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(328, 121);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(93, 28);
			this.btnCancel.TabIndex = 17;
			this.btnCancel.Text = "취  소";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// pgbAll
			// 
			this.pgbAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pgbAll.Location = new System.Drawing.Point(7, 84);
			this.pgbAll.Name = "pgbAll";
			this.pgbAll.Size = new System.Drawing.Size(359, 31);
			this.pgbAll.TabIndex = 10;
			// 
			// lblAllCnt
			// 
			this.lblAllCnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblAllCnt.Location = new System.Drawing.Point(372, 101);
			this.lblAllCnt.Name = "lblAllCnt";
			this.lblAllCnt.Size = new System.Drawing.Size(49, 28);
			this.lblAllCnt.TabIndex = 16;
			this.lblAllCnt.Text = "0/0";
			// 
			// lblAllPer
			// 
			this.lblAllPer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblAllPer.AutoSize = true;
			this.lblAllPer.Location = new System.Drawing.Point(372, 87);
			this.lblAllPer.Name = "lblAllPer";
			this.lblAllPer.Size = new System.Drawing.Size(33, 12);
			this.lblAllPer.TabIndex = 15;
			this.lblAllPer.Text = "100%";
			// 
			// lblEachCnt
			// 
			this.lblEachCnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblEachCnt.Location = new System.Drawing.Point(372, 63);
			this.lblEachCnt.Name = "lblEachCnt";
			this.lblEachCnt.Size = new System.Drawing.Size(49, 28);
			this.lblEachCnt.TabIndex = 14;
			this.lblEachCnt.Text = "0/0";
			this.lblEachCnt.Visible = false;
			// 
			// lblDetail
			// 
			this.lblDetail.Location = new System.Drawing.Point(7, 21);
			this.lblDetail.Name = "lblDetail";
			this.lblDetail.Size = new System.Drawing.Size(412, 23);
			this.lblDetail.TabIndex = 12;
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblTitle.Location = new System.Drawing.Point(194, 4);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(44, 12);
			this.lblTitle.TabIndex = 11;
			this.lblTitle.Text = "타이틀";
			// 
			// usrProgress
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.pnlBody);
			this.Name = "usrProgress";
			this.Size = new System.Drawing.Size(430, 161);
			this.pnlBody.ResumeLayout(false);
			this.pnlBody.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlBody;
		private System.Windows.Forms.ProgressBar pgbEach;
		private System.Windows.Forms.Label lblEachPer;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ProgressBar pgbAll;
		private System.Windows.Forms.Label lblAllCnt;
		private System.Windows.Forms.Label lblAllPer;
		private System.Windows.Forms.Label lblEachCnt;
		private System.Windows.Forms.Label lblDetail;
		private System.Windows.Forms.Label lblTitle;
	}
}
