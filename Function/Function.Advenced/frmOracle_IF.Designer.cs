namespace Function.Advenced
{
	partial class frmOracle_IF
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
			FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer enhancedColumnHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer();
			FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer enhancedColumnHeaderRenderer2 = new FarPoint.Win.Spread.CellType.EnhancedColumnHeaderRenderer();
			FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer enhancedRowHeaderRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedRowHeaderRenderer();
			FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Style1");
			FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("RowHeaderEnhanced");
			FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("CornerEnhanced");
			FarPoint.Win.Spread.CellType.EnhancedCornerRenderer enhancedCornerRenderer1 = new FarPoint.Win.Spread.CellType.EnhancedCornerRenderer();
			FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("DataAreaDefault");
			FarPoint.Win.Spread.CellType.GeneralCellType generalCellType1 = new FarPoint.Win.Spread.CellType.GeneralCellType();
			FarPoint.Win.Spread.SpreadSkin spreadSkin1 = new FarPoint.Win.Spread.SpreadSkin();
			FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer enhancedFocusIndicatorRenderer1 = new FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer();
			FarPoint.Win.Spread.EnhancedInterfaceRenderer enhancedInterfaceRenderer1 = new FarPoint.Win.Spread.EnhancedInterfaceRenderer();
			FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
			FarPoint.Win.Spread.CellType.DateTimeCellType dateTimeCellType1 = new FarPoint.Win.Spread.CellType.DateTimeCellType();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOLEDB_IF));
			FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
			this.lblSvrInfo = new System.Windows.Forms.Label();
			this.lblFormTime = new System.Windows.Forms.Label();
			this.lblSvrStatus = new System.Windows.Forms.Label();
			this.fpLog = new FarPoint.Win.Spread.FpSpread();
			this.sheetView1 = new FarPoint.Win.Spread.SheetView();
			((System.ComponentModel.ISupportInitialize)(this.fpLog)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sheetView1)).BeginInit();
			this.SuspendLayout();
			enhancedColumnHeaderRenderer1.BackColor = System.Drawing.SystemColors.Control;
			enhancedColumnHeaderRenderer1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			enhancedColumnHeaderRenderer1.ForeColor = System.Drawing.SystemColors.ControlText;
			enhancedColumnHeaderRenderer1.Name = "enhancedColumnHeaderRenderer1";
			enhancedColumnHeaderRenderer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			enhancedColumnHeaderRenderer1.TextRotationAngle = 0;
			enhancedColumnHeaderRenderer2.BackColor = System.Drawing.SystemColors.Control;
			enhancedColumnHeaderRenderer2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			enhancedColumnHeaderRenderer2.ForeColor = System.Drawing.SystemColors.ControlText;
			enhancedColumnHeaderRenderer2.Name = "enhancedColumnHeaderRenderer2";
			enhancedColumnHeaderRenderer2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			enhancedColumnHeaderRenderer2.TextRotationAngle = 0;
			enhancedRowHeaderRenderer1.Name = "enhancedRowHeaderRenderer1";
			enhancedRowHeaderRenderer1.TextRotationAngle = 0;
			// 
			// lblSvrInfo
			// 
			this.lblSvrInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblSvrInfo.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.lblSvrInfo.Font = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
			this.lblSvrInfo.Location = new System.Drawing.Point(0, 0);
			this.lblSvrInfo.Name = "lblSvrInfo";
			this.lblSvrInfo.Size = new System.Drawing.Size(549, 42);
			this.lblSvrInfo.TabIndex = 1;
			this.lblSvrInfo.Text = "서버 시작 대기중";
			this.lblSvrInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblFormTime
			// 
			this.lblFormTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblFormTime.BackColor = System.Drawing.SystemColors.ControlText;
			this.lblFormTime.Font = new System.Drawing.Font("굴림체", 11F);
			this.lblFormTime.ForeColor = System.Drawing.Color.White;
			this.lblFormTime.Location = new System.Drawing.Point(549, 0);
			this.lblFormTime.Name = "lblFormTime";
			this.lblFormTime.Size = new System.Drawing.Size(218, 42);
			this.lblFormTime.TabIndex = 2;
			this.lblFormTime.Text = "시작: 2010-03-22 08:33:03\r\n종료: 2010-03-22 09:33:03";
			this.lblFormTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSvrStatus
			// 
			this.lblSvrStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSvrStatus.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.lblSvrStatus.Font = new System.Drawing.Font("굴림체", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblSvrStatus.ForeColor = System.Drawing.Color.White;
			this.lblSvrStatus.Location = new System.Drawing.Point(767, 0);
			this.lblSvrStatus.Name = "lblSvrStatus";
			this.lblSvrStatus.Size = new System.Drawing.Size(66, 42);
			this.lblSvrStatus.TabIndex = 3;
			this.lblSvrStatus.Text = "시작\r\n대기";
			this.lblSvrStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// fpLog
			// 
			this.fpLog.AccessibleDescription = "fpLog, Sheet1, Row 0, Column 0, 10-03-22 11:41:29";
			this.fpLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.fpLog.BackColor = System.Drawing.SystemColors.Control;
			this.fpLog.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
			this.fpLog.Location = new System.Drawing.Point(0, 41);
			this.fpLog.Name = "fpLog";
			namedStyle1.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			namedStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
			namedStyle1.Locked = false;
			namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
			namedStyle1.Renderer = enhancedColumnHeaderRenderer2;
			namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
			namedStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(236)))), ((int)(((byte)(247)))));
			namedStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
			namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
			namedStyle2.Renderer = enhancedRowHeaderRenderer1;
			namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
			namedStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(196)))), ((int)(((byte)(233)))));
			namedStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
			namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
			namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
			namedStyle3.Renderer = enhancedCornerRenderer1;
			namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
			namedStyle4.BackColor = System.Drawing.SystemColors.Window;
			namedStyle4.CellType = generalCellType1;
			namedStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
			namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
			namedStyle4.Renderer = generalCellType1;
			this.fpLog.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4});
			this.fpLog.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.fpLog.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.sheetView1});
			this.fpLog.Size = new System.Drawing.Size(833, 501);
			spreadSkin1.ColumnHeaderDefaultStyle = namedStyle1;
			spreadSkin1.CornerDefaultStyle = namedStyle3;
			spreadSkin1.DefaultStyle = namedStyle4;
			spreadSkin1.FocusRenderer = enhancedFocusIndicatorRenderer1;
			enhancedInterfaceRenderer1.GrayAreaColor = System.Drawing.SystemColors.ControlLight;
			enhancedInterfaceRenderer1.ScrollBoxBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(186)))), ((int)(((byte)(221)))));
			enhancedInterfaceRenderer1.SheetTabLowerActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(210)))), ((int)(((byte)(244)))));
			enhancedInterfaceRenderer1.SheetTabLowerNormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(231)))), ((int)(((byte)(249)))));
			enhancedInterfaceRenderer1.SheetTabUpperActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(228)))), ((int)(((byte)(244)))));
			enhancedInterfaceRenderer1.SheetTabUpperNormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(229)))), ((int)(((byte)(249)))));
			enhancedInterfaceRenderer1.TabStripBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(186)))), ((int)(((byte)(221)))));
			spreadSkin1.InterfaceRenderer = enhancedInterfaceRenderer1;
			spreadSkin1.Name = "CustomSkin2";
			spreadSkin1.RowHeaderDefaultStyle = namedStyle2;
			spreadSkin1.ScrollBarRenderer = enhancedScrollBarRenderer1;
			spreadSkin1.SelectionRenderer = new FarPoint.Win.Spread.GradientSelectionRenderer(System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(97))))), System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(110)))), ((int)(((byte)(170))))), System.Drawing.Drawing2D.LinearGradientMode.Vertical, 30);
			this.fpLog.Skin = spreadSkin1;
			this.fpLog.TabIndex = 4;
			this.fpLog.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
			// 
			// sheetView1
			// 
			this.sheetView1.Reset();
			this.sheetView1.SheetName = "Sheet1";
			// Formulas and custom names must be loaded with R1C1 reference style
			this.sheetView1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
			this.sheetView1.ColumnCount = 4;
			this.sheetView1.RowCount = 1;
			this.sheetView1.Cells.Get(0, 0).Value = new System.DateTime(2010, 3, 22, 11, 41, 29, 0);
			this.sheetView1.Cells.Get(0, 1).Value = "TEST";
			this.sheetView1.Cells.Get(0, 2).Value = "\r\n";
			this.sheetView1.ColumnHeader.Cells.Get(0, 0).Value = "처리시간";
			this.sheetView1.ColumnHeader.Cells.Get(0, 1).Value = "클라이언트이름";
			this.sheetView1.ColumnHeader.Cells.Get(0, 2).Value = "Query";
			this.sheetView1.ColumnHeader.Cells.Get(0, 3).Value = "비고";
			this.sheetView1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
			this.sheetView1.ColumnHeader.DefaultStyle.Parent = "Style1";
			this.sheetView1.ColumnHeader.Rows.Get(0).Height = 21F;
			dateTimeCellType1.Calendar = ((System.Globalization.Calendar)(resources.GetObject("dateTimeCellType1.Calendar")));
			dateTimeCellType1.CalendarSurroundingDaysColor = System.Drawing.SystemColors.GrayText;
			dateTimeCellType1.DateDefault = new System.DateTime(2010, 3, 22, 11, 41, 29, 0);
			dateTimeCellType1.DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.UserDefined;
			dateTimeCellType1.MaximumTime = System.TimeSpan.Parse("23:59:59.9999999");
			dateTimeCellType1.TimeDefault = new System.DateTime(2010, 3, 22, 11, 41, 29, 0);
			dateTimeCellType1.UserDefinedFormat = "yy-MM-dd hh:mm:ss";
			this.sheetView1.Columns.Get(0).CellType = dateTimeCellType1;
			this.sheetView1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
			this.sheetView1.Columns.Get(0).Label = "처리시간";
			this.sheetView1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
			this.sheetView1.Columns.Get(0).Width = 112F;
			this.sheetView1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
			this.sheetView1.Columns.Get(1).Label = "클라이언트이름";
			this.sheetView1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
			this.sheetView1.Columns.Get(1).Width = 117F;
			textCellType1.MaxLength = 9999;
			textCellType1.Multiline = true;
			this.sheetView1.Columns.Get(2).CellType = textCellType1;
			this.sheetView1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
			this.sheetView1.Columns.Get(2).Label = "Query";
			this.sheetView1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
			this.sheetView1.Columns.Get(2).Width = 382F;
			this.sheetView1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
			this.sheetView1.Columns.Get(3).Label = "비고";
			this.sheetView1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
			this.sheetView1.Columns.Get(3).Width = 183F;
			this.sheetView1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
			this.sheetView1.RowHeader.Columns.Default.Resizable = false;
			this.sheetView1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
			// 
			// frmOLEDB_IF
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(833, 566);
			this.Controls.Add(this.fpLog);
			this.Controls.Add(this.lblSvrStatus);
			this.Controls.Add(this.lblFormTime);
			this.Controls.Add(this.lblSvrInfo);
			this.Name = "frmOLEDB_IF";
			this.Text = "OLE DB I/F";
			this.Controls.SetChildIndex(this.lblSvrInfo, 0);
			this.Controls.SetChildIndex(this.lblFormTime, 0);
			this.Controls.SetChildIndex(this.lblSvrStatus, 0);
			this.Controls.SetChildIndex(this.fpLog, 0);
			((System.ComponentModel.ISupportInitialize)(this.fpLog)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sheetView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSvrInfo;
		private System.Windows.Forms.Label lblFormTime;
		private System.Windows.Forms.Label lblSvrStatus;
		private FarPoint.Win.Spread.FpSpread fpLog;
		private FarPoint.Win.Spread.SheetView sheetView1;
	}
}