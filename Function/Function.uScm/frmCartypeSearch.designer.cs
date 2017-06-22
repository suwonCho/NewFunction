namespace Function.uScm
{
	partial class frmCarttypeSearch
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.lstList = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.btnSearch = new System.Windows.Forms.Button();
			this.txtWord = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Controls.Add(this.btnOk);
			this.panel1.Controls.Add(this.lstList);
			this.panel1.Controls.Add(this.btnSearch);
			this.panel1.Controls.Add(this.txtWord);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(702, 370);
			this.panel1.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold);
			this.btnCancel.Location = new System.Drawing.Point(615, 335);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(78, 28);
			this.btnCancel.TabIndex = 26;
			this.btnCancel.Text = "취 소";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOk
			// 
			this.btnOk.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold);
			this.btnOk.Location = new System.Drawing.Point(503, 335);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(78, 28);
			this.btnOk.TabIndex = 25;
			this.btnOk.Text = "확 인";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// lstList
			// 
			this.lstList.AutoArrange = false;
			this.lstList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader10,
            this.columnHeader3});
			this.lstList.Font = new System.Drawing.Font("돋움체", 11.25F);
			this.lstList.FullRowSelect = true;
			this.lstList.GridLines = true;
			this.lstList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstList.Location = new System.Drawing.Point(6, 37);
			this.lstList.MultiSelect = false;
			this.lstList.Name = "lstList";
			this.lstList.Size = new System.Drawing.Size(687, 292);
			this.lstList.TabIndex = 24;
			this.lstList.UseCompatibleStateImageBehavior = false;
			this.lstList.View = System.Windows.Forms.View.Details;
			this.lstList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstList_MouseDoubleClick);
			this.lstList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstList_KeyDown);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "flag";
			this.columnHeader1.Width = 0;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Code";
			this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader2.Width = 150;
			// 
			// columnHeader10
			// 
			this.columnHeader10.Text = "모델명";
			this.columnHeader10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader10.Width = 383;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "차 종";
			this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader3.Width = 124;
			// 
			// btnSearch
			// 
			this.btnSearch.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold);
			this.btnSearch.Location = new System.Drawing.Point(642, 6);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(51, 28);
			this.btnSearch.TabIndex = 23;
			this.btnSearch.Text = "조회";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// txtWord
			// 
			this.txtWord.Font = new System.Drawing.Font("굴림", 12F);
			this.txtWord.Location = new System.Drawing.Point(111, 6);
			this.txtWord.Name = "txtWord";
			this.txtWord.Size = new System.Drawing.Size(525, 26);
			this.txtWord.TabIndex = 22;
			this.txtWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtWord_KeyDown);
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label5.Font = new System.Drawing.Font("굴림체", 12F, System.Drawing.FontStyle.Bold);
			this.label5.Location = new System.Drawing.Point(8, 4);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(97, 28);
			this.label5.TabIndex = 21;
			this.label5.Text = "모델 검색";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// frmCarttypeSearch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(702, 370);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmCarttypeSearch";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.TextBox txtWord;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ListView lstList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader10;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;

	}
}
