namespace AutoUpdater
{
	partial class frmUploadWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUploadWindow));
			DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
			DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
			this.gridColumn7 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.btnUpload = new System.Windows.Forms.Button();
			this.txtInfo = new System.Windows.Forms.Label();
			this.btnDB_Init = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.gcFileList = new DevExpress.XtraGrid.GridControl();
			this.gvFileList = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
			this.bandedGridColumn1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
			this.gridColumn1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
			this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
			this.gridColumn2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
			this.gridColumn3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
			this.bandedGridColumn2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
			this.gridColumn5 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
			this.repositoryItemProgressBar1 = new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
			this.gridColumn6 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
			this.gridColumn8 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
			this.gridColumn9 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
			this.gridColumn10 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
			this.gridColumn11 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
			this.gridColumn12 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
			this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
			this.repositoryItemButtonEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
			this.pnlTop = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.bandedGridColumn3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
			this.gridBand5 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
			this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
			this.gridBand4 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
			this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
			((System.ComponentModel.ISupportInitialize)(this.gcFileList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gvFileList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemProgressBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).BeginInit();
			this.pnlTop.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// gridColumn7
			// 
			this.gridColumn7.AppearanceCell.Options.UseTextOptions = true;
			this.gridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColumn7.Caption = "SVR";
			this.gridColumn7.FieldName = "VERSION";
			this.gridColumn7.Name = "gridColumn7";
			this.gridColumn7.OptionsColumn.FixedWidth = true;
			this.gridColumn7.OptionsColumn.ReadOnly = true;
			this.gridColumn7.Visible = true;
			this.gridColumn7.Width = 72;
			// 
			// btnRefresh
			// 
			this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefresh.BackColor = System.Drawing.SystemColors.Control;
			this.btnRefresh.Font = new System.Drawing.Font("굴림체", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnRefresh.ForeColor = System.Drawing.Color.Black;
			this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
			this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnRefresh.Location = new System.Drawing.Point(1048, 513);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(102, 35);
			this.btnRefresh.TabIndex = 1;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnRefresh.UseVisualStyleBackColor = false;
			this.btnRefresh.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnUpload
			// 
			this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUpload.BackColor = System.Drawing.SystemColors.Control;
			this.btnUpload.Font = new System.Drawing.Font("굴림체", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnUpload.Image = ((System.Drawing.Image)(resources.GetObject("btnUpload.Image")));
			this.btnUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnUpload.Location = new System.Drawing.Point(1156, 513);
			this.btnUpload.Name = "btnUpload";
			this.btnUpload.Size = new System.Drawing.Size(96, 35);
			this.btnUpload.TabIndex = 2;
			this.btnUpload.Text = "Upload";
			this.btnUpload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnUpload.UseVisualStyleBackColor = false;
			this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
			// 
			// txtInfo
			// 
			this.txtInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtInfo.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.txtInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtInfo.Font = new System.Drawing.Font("굴림체", 12F);
			this.txtInfo.Location = new System.Drawing.Point(6, 3);
			this.txtInfo.Name = "txtInfo";
			this.txtInfo.Size = new System.Drawing.Size(1186, 74);
			this.txtInfo.TabIndex = 4;
			// 
			// btnDB_Init
			// 
			this.btnDB_Init.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDB_Init.Location = new System.Drawing.Point(1198, 3);
			this.btnDB_Init.Name = "btnDB_Init";
			this.btnDB_Init.Size = new System.Drawing.Size(56, 74);
			this.btnDB_Init.TabIndex = 5;
			this.btnDB_Init.Text = "DB\r\n설정";
			this.btnDB_Init.UseVisualStyleBackColor = true;
			this.btnDB_Init.Visible = false;
			this.btnDB_Init.Click += new System.EventHandler(this.btnDB_Init_Click);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.BackColor = System.Drawing.SystemColors.Control;
			this.button1.Font = new System.Drawing.Font("굴림체", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.button1.ForeColor = System.Drawing.Color.Black;
			this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button1.Location = new System.Drawing.Point(3, 513);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(149, 35);
			this.button1.TabIndex = 6;
			this.button1.Text = "전체 폴더 변경";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// gcFileList
			// 
			this.gcFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gcFileList.Location = new System.Drawing.Point(0, 0);
			this.gcFileList.MainView = this.gvFileList;
			this.gcFileList.Name = "gcFileList";
			this.gcFileList.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1,
            this.repositoryItemCheckEdit1,
            this.repositoryItemProgressBar1,
            this.repositoryItemButtonEdit2});
			this.gcFileList.Size = new System.Drawing.Size(1256, 512);
			this.gcFileList.TabIndex = 7;
			this.gcFileList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvFileList});
			// 
			// gvFileList
			// 
			this.gvFileList.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand5,
            this.gridBand1,
            this.gridBand4,
            this.gridBand3});
			this.gvFileList.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.bandedGridColumn1,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.bandedGridColumn2,
            this.gridColumn5,
            this.bandedGridColumn3,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12});
			styleFormatCondition1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			styleFormatCondition1.Appearance.Options.UseBackColor = true;
			styleFormatCondition1.ApplyToRow = true;
			styleFormatCondition1.Column = this.gridColumn7;
			styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
			styleFormatCondition1.Expression = "[FILE_VERSION] != [VERSION]";
			this.gvFileList.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1});
			this.gvFileList.GridControl = this.gcFileList;
			this.gvFileList.Name = "gvFileList";
			this.gvFileList.OptionsView.EnableAppearanceEvenRow = true;
			this.gvFileList.OptionsView.EnableAppearanceOddRow = true;
			this.gvFileList.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
			this.gvFileList.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvFileList_RowCellStyle);
			this.gvFileList.Click += new System.EventHandler(this.gvFileList_Click);
			// 
			// bandedGridColumn1
			// 
			this.bandedGridColumn1.Caption = "No";
			this.bandedGridColumn1.FieldName = "ROWNUM";
			this.bandedGridColumn1.Name = "bandedGridColumn1";
			this.bandedGridColumn1.OptionsColumn.FixedWidth = true;
			this.bandedGridColumn1.OptionsColumn.ReadOnly = true;
			this.bandedGridColumn1.Visible = true;
			this.bandedGridColumn1.Width = 23;
			// 
			// gridColumn1
			// 
			this.gridColumn1.Caption = "업";
			this.gridColumn1.ColumnEdit = this.repositoryItemCheckEdit1;
			this.gridColumn1.FieldName = "ISUPLOAD";
			this.gridColumn1.Name = "gridColumn1";
			this.gridColumn1.OptionsColumn.FixedWidth = true;
			this.gridColumn1.Visible = true;
			this.gridColumn1.Width = 20;
			// 
			// repositoryItemCheckEdit1
			// 
			this.repositoryItemCheckEdit1.AutoHeight = false;
			this.repositoryItemCheckEdit1.Caption = "Check";
			this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
			this.repositoryItemCheckEdit1.ValueChecked = "T";
			this.repositoryItemCheckEdit1.ValueGrayed = "0";
			this.repositoryItemCheckEdit1.ValueUnchecked = "F";
			// 
			// gridColumn2
			// 
			this.gridColumn2.Caption = "파일명";
			this.gridColumn2.FieldName = "FILENAME";
			this.gridColumn2.Name = "gridColumn2";
			this.gridColumn2.OptionsColumn.ReadOnly = true;
			this.gridColumn2.Visible = true;
			this.gridColumn2.Width = 200;
			// 
			// gridColumn3
			// 
			this.gridColumn3.Caption = "원본경로";
			this.gridColumn3.FieldName = "SOURCE_FILEPATH";
			this.gridColumn3.Name = "gridColumn3";
			this.gridColumn3.OptionsColumn.ReadOnly = true;
			this.gridColumn3.Visible = true;
			this.gridColumn3.Width = 261;
			// 
			// bandedGridColumn2
			// 
			this.bandedGridColumn2.AppearanceCell.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.bandedGridColumn2.AppearanceCell.BackColor2 = System.Drawing.SystemColors.ButtonShadow;
			this.bandedGridColumn2.AppearanceCell.BorderColor = System.Drawing.Color.Black;
			this.bandedGridColumn2.AppearanceCell.Options.UseBackColor = true;
			this.bandedGridColumn2.AppearanceCell.Options.UseBorderColor = true;
			this.bandedGridColumn2.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.bandedGridColumn2.Caption = "변경";
			this.bandedGridColumn2.FieldName = "CH_SOURCE_FILEPATH";
			this.bandedGridColumn2.Name = "bandedGridColumn2";
			this.bandedGridColumn2.OptionsColumn.FixedWidth = true;
			this.bandedGridColumn2.Visible = true;
			this.bandedGridColumn2.Width = 35;
			// 
			// gridColumn5
			// 
			this.gridColumn5.Caption = "Upload";
			this.gridColumn5.ColumnEdit = this.repositoryItemProgressBar1;
			this.gridColumn5.FieldName = "UPLOAD";
			this.gridColumn5.Name = "gridColumn5";
			this.gridColumn5.OptionsColumn.FixedWidth = true;
			this.gridColumn5.OptionsColumn.ReadOnly = true;
			this.gridColumn5.Visible = true;
			this.gridColumn5.Width = 72;
			// 
			// repositoryItemProgressBar1
			// 
			this.repositoryItemProgressBar1.Name = "repositoryItemProgressBar1";
			this.repositoryItemProgressBar1.ShowTitle = true;
			this.repositoryItemProgressBar1.Step = 1;
			// 
			// gridColumn6
			// 
			this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
			this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColumn6.Caption = "로컬";
			this.gridColumn6.FieldName = "FILE_VERSION";
			this.gridColumn6.Name = "gridColumn6";
			this.gridColumn6.OptionsColumn.FixedWidth = true;
			this.gridColumn6.OptionsColumn.ReadOnly = true;
			this.gridColumn6.Visible = true;
			this.gridColumn6.Width = 72;
			// 
			// gridColumn8
			// 
			this.gridColumn8.AppearanceCell.Options.UseTextOptions = true;
			this.gridColumn8.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColumn8.Caption = "로컬";
			this.gridColumn8.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
			this.gridColumn8.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
			this.gridColumn8.FieldName = "FILE_FILEDATE";
			this.gridColumn8.Name = "gridColumn8";
			this.gridColumn8.OptionsColumn.FixedWidth = true;
			this.gridColumn8.OptionsColumn.ReadOnly = true;
			this.gridColumn8.Visible = true;
			this.gridColumn8.Width = 135;
			// 
			// gridColumn9
			// 
			this.gridColumn9.AppearanceCell.Options.UseTextOptions = true;
			this.gridColumn9.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColumn9.Caption = "SVR";
			this.gridColumn9.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
			this.gridColumn9.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
			this.gridColumn9.FieldName = "FILEDATE";
			this.gridColumn9.Name = "gridColumn9";
			this.gridColumn9.OptionsColumn.FixedWidth = true;
			this.gridColumn9.OptionsColumn.ReadOnly = true;
			this.gridColumn9.Visible = true;
			this.gridColumn9.Width = 135;
			// 
			// gridColumn10
			// 
			this.gridColumn10.Caption = "CRC";
			this.gridColumn10.FieldName = "CRC";
			this.gridColumn10.Name = "gridColumn10";
			this.gridColumn10.OptionsColumn.FixedWidth = true;
			this.gridColumn10.OptionsColumn.ReadOnly = true;
			this.gridColumn10.Visible = true;
			this.gridColumn10.Width = 70;
			// 
			// gridColumn11
			// 
			this.gridColumn11.Caption = "FileSize";
			this.gridColumn11.DisplayFormat.FormatString = "#,###";
			this.gridColumn11.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
			this.gridColumn11.FieldName = "FILESIZE";
			this.gridColumn11.Name = "gridColumn11";
			this.gridColumn11.OptionsColumn.FixedWidth = true;
			this.gridColumn11.OptionsColumn.ReadOnly = true;
			this.gridColumn11.Visible = true;
			this.gridColumn11.Width = 80;
			// 
			// gridColumn12
			// 
			this.gridColumn12.AppearanceCell.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.gridColumn12.AppearanceCell.BackColor2 = System.Drawing.SystemColors.ButtonShadow;
			this.gridColumn12.AppearanceCell.BorderColor = System.Drawing.Color.Black;
			this.gridColumn12.AppearanceCell.Options.UseBackColor = true;
			this.gridColumn12.AppearanceCell.Options.UseBorderColor = true;
			this.gridColumn12.AppearanceCell.Options.UseTextOptions = true;
			this.gridColumn12.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColumn12.Caption = "삭제";
			this.gridColumn12.FieldName = "DEL";
			this.gridColumn12.Name = "gridColumn12";
			this.gridColumn12.OptionsColumn.FixedWidth = true;
			this.gridColumn12.OptionsColumn.ReadOnly = true;
			this.gridColumn12.Visible = true;
			this.gridColumn12.Width = 35;
			// 
			// repositoryItemButtonEdit1
			// 
			this.repositoryItemButtonEdit1.AutoHeight = false;
			this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "삭제", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
			this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
			// 
			// repositoryItemButtonEdit2
			// 
			this.repositoryItemButtonEdit2.AutoHeight = false;
			this.repositoryItemButtonEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "...", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
			this.repositoryItemButtonEdit2.Name = "repositoryItemButtonEdit2";
			// 
			// pnlTop
			// 
			this.pnlTop.Controls.Add(this.btnDB_Init);
			this.pnlTop.Controls.Add(this.txtInfo);
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTop.Location = new System.Drawing.Point(0, 0);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(1257, 80);
			this.pnlTop.TabIndex = 8;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.gcFileList);
			this.panel2.Controls.Add(this.button1);
			this.panel2.Controls.Add(this.btnRefresh);
			this.panel2.Controls.Add(this.btnUpload);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 86);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(1257, 551);
			this.panel2.TabIndex = 9;
			// 
			// splitter1
			// 
			this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 80);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(1257, 6);
			this.splitter1.TabIndex = 10;
			this.splitter1.TabStop = false;
			// 
			// bandedGridColumn3
			// 
			this.bandedGridColumn3.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.bandedGridColumn3.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
			this.bandedGridColumn3.Caption = "비교";
			this.bandedGridColumn3.FieldName = "CompType";
			this.bandedGridColumn3.Name = "bandedGridColumn3";
			this.bandedGridColumn3.OptionsColumn.FixedWidth = true;
			this.bandedGridColumn3.Visible = true;
			this.bandedGridColumn3.Width = 35;
			// 
			// gridBand5
			// 
			this.gridBand5.AppearanceHeader.Options.UseTextOptions = true;
			this.gridBand5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridBand5.Caption = "파일정보";
			this.gridBand5.Columns.Add(this.bandedGridColumn1);
			this.gridBand5.Columns.Add(this.gridColumn1);
			this.gridBand5.Columns.Add(this.gridColumn2);
			this.gridBand5.Columns.Add(this.gridColumn3);
			this.gridBand5.Columns.Add(this.bandedGridColumn2);
			this.gridBand5.Columns.Add(this.gridColumn5);
			this.gridBand5.Columns.Add(this.bandedGridColumn3);
			this.gridBand5.Name = "gridBand5";
			this.gridBand5.VisibleIndex = 0;
			this.gridBand5.Width = 646;
			// 
			// gridBand1
			// 
			this.gridBand1.AppearanceHeader.Options.UseTextOptions = true;
			this.gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridBand1.Caption = "Version";
			this.gridBand1.Columns.Add(this.gridColumn6);
			this.gridBand1.Columns.Add(this.gridColumn7);
			this.gridBand1.Name = "gridBand1";
			this.gridBand1.VisibleIndex = 1;
			this.gridBand1.Width = 144;
			// 
			// gridBand4
			// 
			this.gridBand4.AppearanceHeader.Options.UseTextOptions = true;
			this.gridBand4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridBand4.Caption = "파일날짜";
			this.gridBand4.Columns.Add(this.gridColumn8);
			this.gridBand4.Columns.Add(this.gridColumn9);
			this.gridBand4.Name = "gridBand4";
			this.gridBand4.VisibleIndex = 2;
			this.gridBand4.Width = 270;
			// 
			// gridBand3
			// 
			this.gridBand3.AppearanceHeader.Options.UseTextOptions = true;
			this.gridBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridBand3.Caption = "파일 관리";
			this.gridBand3.Columns.Add(this.gridColumn10);
			this.gridBand3.Columns.Add(this.gridColumn11);
			this.gridBand3.Columns.Add(this.gridColumn12);
			this.gridBand3.Name = "gridBand3";
			this.gridBand3.VisibleIndex = 3;
			this.gridBand3.Width = 185;
			// 
			// frmUploadWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1257, 659);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.pnlTop);
			this.Name = "frmUploadWindow";
			this.ShowIcon = false;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmUploadWindow_FormClosed);
			this.Load += new System.EventHandler(this.frmUploadWindow_Load);
			this.Controls.SetChildIndex(this.pnlTop, 0);
			this.Controls.SetChildIndex(this.splitter1, 0);
			this.Controls.SetChildIndex(this.panel2, 0);
			((System.ComponentModel.ISupportInitialize)(this.gcFileList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gvFileList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemProgressBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).EndInit();
			this.pnlTop.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.Button btnUpload;
		private System.Windows.Forms.Label txtInfo;
		private System.Windows.Forms.Button btnDB_Init;
        private System.Windows.Forms.Button button1;
		private DevExpress.XtraGrid.GridControl gcFileList;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvFileList;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn1;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn2;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn3;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn5;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn6;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn7;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn8;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn9;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn10;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn11;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn12;
		private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn1;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
		private DevExpress.XtraEditors.Repository.RepositoryItemProgressBar repositoryItemProgressBar1;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn2;
		private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit2;
		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Splitter splitter1;
		private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand5;
		private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn3;
		private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
		private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand4;
		private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
	}
}