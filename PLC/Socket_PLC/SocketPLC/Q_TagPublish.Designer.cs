namespace SocketPLC
{
    partial class Q_TagPublish
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
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.DateTimeCellType dateTimeCellType1 = new FarPoint.Win.Spread.CellType.DateTimeCellType();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Q_TagPublish));
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.fpView = new FarPoint.Win.Spread.FpSpread();
            this.fpView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnOnOff = new System.Windows.Forms.Button();
            this.lblServerStats = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.MaskedTextBox();
            this.txtBodySerial = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fpView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpView_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // fpView
            // 
            this.fpView.About = "2.5.2013.2005";
            this.fpView.AccessibleDescription = "fpView, Sheet1, Row 0, Column 0, ";
            this.fpView.BackColor = System.Drawing.SystemColors.Control;
            this.fpView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.fpView.Location = new System.Drawing.Point(2, 2);
            this.fpView.Name = "fpView";
            this.fpView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fpView.SelectionBlockOptions = FarPoint.Win.Spread.SelectionBlockOptions.Rows;
            this.fpView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpView_Sheet1});
            this.fpView.Size = new System.Drawing.Size(825, 447);
            this.fpView.TabIndex = 0;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.fpView.TextTipAppearance = tipAppearance1;
            // 
            // fpView_Sheet1
            // 
            this.fpView_Sheet1.Reset();
            this.fpView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpView_Sheet1.ColumnCount = 22;
            this.fpView_Sheet1.RowCount = 1;
            this.fpView_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.SystemColors.AppWorkspace, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(60)))), ((int)(((byte)(140))))), System.Drawing.Color.Gainsboro, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.LightGray, System.Drawing.Color.Black, System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(138)))), ((int)(((byte)(156))))), System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247))))), System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(255))))), System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247))))), false, false, true, true, true);
            this.fpView_Sheet1.AutoCalculation = false;
            this.fpView_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.LightGray;
            this.fpView_Sheet1.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpView_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpView_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            dateTimeCellType1.Calendar = ((System.Globalization.Calendar)(resources.GetObject("dateTimeCellType1.Calendar")));
            dateTimeCellType1.DateDefault = new System.DateTime(2007, 12, 23, 9, 20, 49, 0);
            dateTimeCellType1.TimeDefault = new System.DateTime(2007, 12, 23, 9, 20, 49, 0);
            this.fpView_Sheet1.Columns.Get(0).CellType = dateTimeCellType1;
            this.fpView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(0).Width = 106F;
            this.fpView_Sheet1.Columns.Get(1).CellType = textCellType1;
            this.fpView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(2).Width = 30F;
            this.fpView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(3).Width = 30F;
            this.fpView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(4).Width = 30F;
            this.fpView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(5).Width = 30F;
            this.fpView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(6).Width = 30F;
            this.fpView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(7).Width = 30F;
            this.fpView_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(8).Width = 30F;
            this.fpView_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(9).Width = 30F;
            this.fpView_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(10).Width = 30F;
            this.fpView_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(11).Width = 30F;
            this.fpView_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(12).Width = 30F;
            this.fpView_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(13).Width = 30F;
            this.fpView_Sheet1.Columns.Get(14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(14).Width = 30F;
            this.fpView_Sheet1.Columns.Get(15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(15).Width = 30F;
            this.fpView_Sheet1.Columns.Get(16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(16).Width = 30F;
            this.fpView_Sheet1.Columns.Get(17).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(17).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(17).Width = 30F;
            this.fpView_Sheet1.Columns.Get(18).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(18).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(18).Width = 30F;
            this.fpView_Sheet1.Columns.Get(19).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(19).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(19).Width = 30F;
            this.fpView_Sheet1.Columns.Get(20).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(20).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(20).Width = 30F;
            this.fpView_Sheet1.Columns.Get(21).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(21).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpView_Sheet1.Columns.Get(21).Width = 30F;
            this.fpView_Sheet1.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpView_Sheet1.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(60)))), ((int)(((byte)(140)))));
            this.fpView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpView_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.LightGray;
            this.fpView_Sheet1.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpView_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpView_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpView_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpView_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.LightGray;
            this.fpView_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.Black;
            this.fpView_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnOnOff
            // 
            this.btnOnOff.Location = new System.Drawing.Point(104, 488);
            this.btnOnOff.Name = "btnOnOff";
            this.btnOnOff.Size = new System.Drawing.Size(101, 30);
            this.btnOnOff.TabIndex = 1;
            this.btnOnOff.Text = "Sever On/off";
            this.btnOnOff.UseVisualStyleBackColor = true;
            this.btnOnOff.Click += new System.EventHandler(this.btnOnOff_Click);
            // 
            // lblServerStats
            // 
            this.lblServerStats.BackColor = System.Drawing.Color.Red;
            this.lblServerStats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblServerStats.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblServerStats.Location = new System.Drawing.Point(12, 488);
            this.lblServerStats.Name = "lblServerStats";
            this.lblServerStats.Size = new System.Drawing.Size(86, 30);
            this.lblServerStats.TabIndex = 2;
            this.lblServerStats.Text = "SeverStop";
            this.lblServerStats.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(12, 452);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 22);
            this.label1.TabIndex = 3;
            this.label1.Text = "Port No";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPort.Location = new System.Drawing.Point(104, 452);
            this.txtPort.Mask = "9999";
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 22);
            this.txtPort.TabIndex = 4;
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            // 
            // txtBodySerial
            // 
            this.txtBodySerial.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtBodySerial.Location = new System.Drawing.Point(727, 452);
            this.txtBodySerial.Mask = "99999";
            this.txtBodySerial.Name = "txtBodySerial";
            this.txtBodySerial.Size = new System.Drawing.Size(100, 22);
            this.txtBodySerial.TabIndex = 6;
            this.txtBodySerial.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBodySerial.TextChanged += new System.EventHandler(this.txtBodySerial_TextChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(635, 452);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 22);
            this.label2.TabIndex = 5;
            this.label2.Text = "BodySerial";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(698, 488);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 29);
            this.button1.TabIndex = 7;
            this.button1.Text = "이력 Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Q_TagPublish
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 520);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtBodySerial);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblServerStats);
            this.Controls.Add(this.btnOnOff);
            this.Controls.Add(this.fpView);
            this.Name = "Q_TagPublish";
            this.Text = "Q_Serise Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Q_TagPublish_FormClosing);
            this.Load += new System.EventHandler(this.frmServer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fpView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpView_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread fpView;
        private FarPoint.Win.Spread.SheetView fpView_Sheet1;
        private System.Windows.Forms.Button btnOnOff;
        private System.Windows.Forms.Label lblServerStats;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox txtPort;
        private System.Windows.Forms.MaskedTextBox txtBodySerial;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        
    }
}

