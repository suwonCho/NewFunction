namespace Function.uScm
{
    partial class frmInputOrder
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
			this.lstPlan = new System.Windows.Forms.ListView();
			this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.label3 = new System.Windows.Forms.Label();
			this.dtPlan = new System.Windows.Forms.DateTimePicker();
			this.lblCode = new System.Windows.Forms.Label();
			this.cmbPartCode = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTargetCnt = new System.Windows.Forms.NumericUpDown();
			this.btnInput = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.btnDel = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.lblModel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.txtTargetCnt)).BeginInit();
			this.SuspendLayout();
			// 
			// lstPlan
			// 
			this.lstPlan.AutoArrange = false;
			this.lstPlan.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9,
            this.columnHeader8,
            this.columnHeader1,
            this.columnHeader4,
            this.columnHeader2,
            this.columnHeader5,
            this.columnHeader3});
			this.lstPlan.Font = new System.Drawing.Font("돋움체", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lstPlan.FullRowSelect = true;
			this.lstPlan.GridLines = true;
			this.lstPlan.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstPlan.Location = new System.Drawing.Point(7, 65);
			this.lstPlan.MultiSelect = false;
			this.lstPlan.Name = "lstPlan";
			this.lstPlan.Size = new System.Drawing.Size(708, 237);
			this.lstPlan.TabIndex = 11;
			this.lstPlan.UseCompatibleStateImageBehavior = false;
			this.lstPlan.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader9
			// 
			this.columnHeader9.Text = "번호";
			this.columnHeader9.Width = 55;
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "생산 일자";
			this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader8.Width = 200;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "차종";
			this.columnHeader1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader1.Width = 213;
			// 
			// columnHeader4
			// 
			this.columnHeader4.DisplayIndex = 5;
			this.columnHeader4.Text = "CARTYPE";
			this.columnHeader4.Width = 0;
			// 
			// columnHeader2
			// 
			this.columnHeader2.DisplayIndex = 3;
			this.columnHeader2.Text = "사 양";
			this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader2.Width = 95;
			// 
			// columnHeader5
			// 
			this.columnHeader5.DisplayIndex = 6;
			this.columnHeader5.Text = "PARTCODE";
			this.columnHeader5.Width = 0;
			// 
			// columnHeader3
			// 
			this.columnHeader3.DisplayIndex = 4;
			this.columnHeader3.Text = "목표 수량";
			this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader3.Width = 113;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label3.Location = new System.Drawing.Point(3, 13);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 13;
			this.label3.Text = "생산일자";
			// 
			// dtPlan
			// 
			this.dtPlan.Font = new System.Drawing.Font("굴림", 11F);
			this.dtPlan.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtPlan.Location = new System.Drawing.Point(74, 9);
			this.dtPlan.Name = "dtPlan";
			this.dtPlan.Size = new System.Drawing.Size(117, 24);
			this.dtPlan.TabIndex = 12;
			// 
			// lblCode
			// 
			this.lblCode.AutoSize = true;
			this.lblCode.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblCode.Location = new System.Drawing.Point(195, 39);
			this.lblCode.Name = "lblCode";
			this.lblCode.Size = new System.Drawing.Size(40, 16);
			this.lblCode.TabIndex = 14;
			this.lblCode.Text = "사양";
			// 
			// cmbPartCode
			// 
			this.cmbPartCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPartCode.Font = new System.Drawing.Font("굴림", 12F);
			this.cmbPartCode.FormattingEnabled = true;
			this.cmbPartCode.Location = new System.Drawing.Point(241, 35);
			this.cmbPartCode.Name = "cmbPartCode";
			this.cmbPartCode.Size = new System.Drawing.Size(207, 24);
			this.cmbPartCode.TabIndex = 15;
			this.cmbPartCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbPartCode_KeyDown);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label2.Location = new System.Drawing.Point(454, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 16;
			this.label2.Text = "목표수량";
			// 
			// txtTargetCnt
			// 
			this.txtTargetCnt.Font = new System.Drawing.Font("굴림", 11F);
			this.txtTargetCnt.Location = new System.Drawing.Point(523, 35);
			this.txtTargetCnt.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
			this.txtTargetCnt.Name = "txtTargetCnt";
			this.txtTargetCnt.Size = new System.Drawing.Size(89, 24);
			this.txtTargetCnt.TabIndex = 17;
			this.txtTargetCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtTargetCnt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTargetCnt_KeyDown);
			// 
			// btnInput
			// 
			this.btnInput.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnInput.Location = new System.Drawing.Point(634, 33);
			this.btnInput.Name = "btnInput";
			this.btnInput.Size = new System.Drawing.Size(77, 30);
			this.btnInput.TabIndex = 18;
			this.btnInput.Text = "등 록";
			this.btnInput.UseVisualStyleBackColor = true;
			this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
			// 
			// btnSave
			// 
			this.btnSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnSave.Location = new System.Drawing.Point(589, 308);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(126, 30);
			this.btnSave.TabIndex = 19;
			this.btnSave.Text = "저     장";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnClear
			// 
			this.btnClear.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnClear.Location = new System.Drawing.Point(7, 308);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(126, 30);
			this.btnClear.TabIndex = 21;
			this.btnClear.Text = "초 기 화";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnDel
			// 
			this.btnDel.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnDel.Location = new System.Drawing.Point(457, 308);
			this.btnDel.Name = "btnDel";
			this.btnDel.Size = new System.Drawing.Size(126, 30);
			this.btnDel.TabIndex = 22;
			this.btnDel.Text = "선택삭제";
			this.btnDel.UseVisualStyleBackColor = true;
			this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label4.Location = new System.Drawing.Point(195, 13);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 16);
			this.label4.TabIndex = 23;
			this.label4.Text = "모델";
			// 
			// lblModel
			// 
			this.lblModel.BackColor = System.Drawing.SystemColors.ControlDark;
			this.lblModel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblModel.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblModel.Location = new System.Drawing.Point(241, 9);
			this.lblModel.Name = "lblModel";
			this.lblModel.Size = new System.Drawing.Size(469, 24);
			this.lblModel.TabIndex = 24;
			this.lblModel.Text = "모델선택";
			this.lblModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblModel.Click += new System.EventHandler(this.lblModel_Click);
			// 
			// frmInputOrder
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(722, 342);
			this.Controls.Add(this.lblModel);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btnDel);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnInput);
			this.Controls.Add(this.txtTargetCnt);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbPartCode);
			this.Controls.Add(this.dtPlan);
			this.Controls.Add(this.lblCode);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lstPlan);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmInputOrder";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "생산 계획 입력";
			this.Load += new System.EventHandler(this.frmInputOrder_Load);
			((System.ComponentModel.ISupportInitialize)(this.txtTargetCnt)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstPlan;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtPlan;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.ComboBox cmbPartCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown txtTargetCnt;
        private System.Windows.Forms.Button btnInput;
		private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnDel;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.Label lblModel;
    }
}