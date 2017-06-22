namespace SocketPLC
{
	partial class frmLSXGT
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
			this.btnOnOff = new System.Windows.Forms.Button();
			this.lblServerStats = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtPort = new System.Windows.Forms.MaskedTextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtMutiple = new System.Windows.Forms.MaskedTextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.gcAdd = new DevExpress.XtraGrid.GridControl();
			this.gvAdd = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.colGroup = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colAddress = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colValue = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colHexValue = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colDesc = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colPriority = new DevExpress.XtraGrid.Columns.GridColumn();
			this.txtAddType = new Function.form.usrInputBox();
			this.gcLog = new DevExpress.XtraGrid.GridControl();
			this.gvLog = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.colTime = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colType = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colLog = new DevExpress.XtraGrid.Columns.GridColumn();
			this.pnlBottom = new System.Windows.Forms.Panel();
			this.pnlLeft = new System.Windows.Forms.Panel();
			this.spAddress = new System.Windows.Forms.Splitter();
			this.pnlAddress = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.chkAddGroup = new System.Windows.Forms.CheckBox();
			this.pnlLeftDown = new System.Windows.Forms.Panel();
			this.gcMng = new DevExpress.XtraGrid.GridControl();
			this.gvMng = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.mngPriority = new DevExpress.XtraGrid.Columns.GridColumn();
			this.mngMng_Type = new DevExpress.XtraGrid.Columns.GridColumn();
			this.mngisUse = new DevExpress.XtraGrid.Columns.GridColumn();
			this.mngAddress = new DevExpress.XtraGrid.Columns.GridColumn();
			this.mngAddress_Length = new DevExpress.XtraGrid.Columns.GridColumn();
			this.mngValue = new DevExpress.XtraGrid.Columns.GridColumn();
			this.mngValueType = new DevExpress.XtraGrid.Columns.GridColumn();
			this.mngCondtion = new DevExpress.XtraGrid.Columns.GridColumn();
			this.mngDesc = new DevExpress.XtraGrid.Columns.GridColumn();
			this.mngButton = new DevExpress.XtraGrid.Columns.GridColumn();
			this.pnlRight = new System.Windows.Forms.Panel();
			this.spLeft = new System.Windows.Forms.Splitter();
			((System.ComponentModel.ISupportInitialize)(this.gcAdd)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gvAdd)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gcLog)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gvLog)).BeginInit();
			this.pnlBottom.SuspendLayout();
			this.pnlLeft.SuspendLayout();
			this.pnlAddress.SuspendLayout();
			this.panel1.SuspendLayout();
			this.pnlLeftDown.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gcMng)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gvMng)).BeginInit();
			this.pnlRight.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOnOff
			// 
			this.btnOnOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOnOff.Location = new System.Drawing.Point(82, 1);
			this.btnOnOff.Name = "btnOnOff";
			this.btnOnOff.Size = new System.Drawing.Size(87, 28);
			this.btnOnOff.TabIndex = 1;
			this.btnOnOff.Text = "Sever On/off";
			this.btnOnOff.UseVisualStyleBackColor = true;
			this.btnOnOff.Click += new System.EventHandler(this.btnOnOff_Click);
			// 
			// lblServerStats
			// 
			this.lblServerStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblServerStats.BackColor = System.Drawing.Color.Red;
			this.lblServerStats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblServerStats.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.lblServerStats.Location = new System.Drawing.Point(3, 2);
			this.lblServerStats.Name = "lblServerStats";
			this.lblServerStats.Size = new System.Drawing.Size(74, 27);
			this.lblServerStats.TabIndex = 2;
			this.lblServerStats.Text = "SeverStop";
			this.lblServerStats.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Location = new System.Drawing.Point(658, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(74, 22);
			this.label1.TabIndex = 3;
			this.label1.Text = "Port No";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtPort
			// 
			this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPort.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.txtPort.Location = new System.Drawing.Point(737, 5);
			this.txtPort.Mask = "9999";
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(86, 22);
			this.txtPort.TabIndex = 4;
			this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(831, 3);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(110, 29);
			this.button1.TabIndex = 7;
			this.button1.Text = "이력 Clear";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(0, 28);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(456, 12);
			this.label2.TabIndex = 9;
			this.label2.Text = "PLC Address";
			// 
			// label3
			// 
			this.label3.Dock = System.Windows.Forms.DockStyle.Top;
			this.label3.Location = new System.Drawing.Point(0, 5);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(485, 12);
			this.label3.TabIndex = 10;
			this.label3.Text = "Comm. Log";
			// 
			// txtMutiple
			// 
			this.txtMutiple.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMutiple.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.txtMutiple.Location = new System.Drawing.Point(550, 6);
			this.txtMutiple.Mask = "99";
			this.txtMutiple.Name = "txtMutiple";
			this.txtMutiple.Size = new System.Drawing.Size(42, 22);
			this.txtMutiple.TabIndex = 13;
			this.txtMutiple.Text = " 1";
			this.txtMutiple.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtMutiple.TextChanged += new System.EventHandler(this.txtAdd_Type_TextChanged);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label4.Location = new System.Drawing.Point(472, 6);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(74, 22);
			this.label4.TabIndex = 12;
			this.label4.Text = "Mutiple  x";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// gcAdd
			// 
			this.gcAdd.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gcAdd.Location = new System.Drawing.Point(0, 40);
			this.gcAdd.MainView = this.gvAdd;
			this.gcAdd.Name = "gcAdd";
			this.gcAdd.Size = new System.Drawing.Size(456, 212);
			this.gcAdd.TabIndex = 15;
			this.gcAdd.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvAdd});
			// 
			// gvAdd
			// 
			this.gvAdd.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colGroup,
            this.colAddress,
            this.colValue,
            this.colHexValue,
            this.colDesc,
            this.colPriority});
			this.gvAdd.GridControl = this.gcAdd;
			this.gvAdd.Name = "gvAdd";
			this.gvAdd.OptionsBehavior.Editable = false;
			// 
			// colGroup
			// 
			this.colGroup.Caption = "Group";
			this.colGroup.FieldName = "AddGroup";
			this.colGroup.Name = "colGroup";
			this.colGroup.Visible = true;
			this.colGroup.VisibleIndex = 0;
			this.colGroup.Width = 60;
			// 
			// colAddress
			// 
			this.colAddress.AppearanceHeader.Options.UseTextOptions = true;
			this.colAddress.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.colAddress.Caption = "어드레스";
			this.colAddress.FieldName = "Address";
			this.colAddress.Name = "colAddress";
			this.colAddress.Visible = true;
			this.colAddress.VisibleIndex = 1;
			// 
			// colValue
			// 
			this.colValue.AppearanceCell.Options.UseTextOptions = true;
			this.colValue.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.colValue.AppearanceHeader.Options.UseTextOptions = true;
			this.colValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.colValue.Caption = "값";
			this.colValue.FieldName = "Value";
			this.colValue.Name = "colValue";
			this.colValue.Visible = true;
			this.colValue.VisibleIndex = 2;
			// 
			// colHexValue
			// 
			this.colHexValue.AppearanceCell.Options.UseTextOptions = true;
			this.colHexValue.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.colHexValue.AppearanceHeader.Options.UseTextOptions = true;
			this.colHexValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.colHexValue.Caption = "Hex";
			this.colHexValue.FieldName = "HexValue";
			this.colHexValue.Name = "colHexValue";
			this.colHexValue.Visible = true;
			this.colHexValue.VisibleIndex = 3;
			// 
			// colDesc
			// 
			this.colDesc.AppearanceHeader.Options.UseTextOptions = true;
			this.colDesc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.colDesc.Caption = "Desc";
			this.colDesc.FieldName = "Desc";
			this.colDesc.Name = "colDesc";
			this.colDesc.Visible = true;
			this.colDesc.VisibleIndex = 4;
			// 
			// colPriority
			// 
			this.colPriority.AppearanceHeader.Options.UseTextOptions = true;
			this.colPriority.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.colPriority.Caption = "순위";
			this.colPriority.FieldName = "Priority";
			this.colPriority.Name = "colPriority";
			this.colPriority.Visible = true;
			this.colPriority.VisibleIndex = 5;
			this.colPriority.Width = 78;
			// 
			// txtAddType
			// 
			this.txtAddType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtAddType.BackColor = System.Drawing.SystemColors.Control;
			this.txtAddType.ChangeMark_Visable = false;
			this.txtAddType.ComboBoxDataSource = null;
			this.txtAddType.ComboBoxDisplayMember = "";
			this.txtAddType.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.txtAddType.ComboBoxSelectIndex = -1;
			this.txtAddType.ComboBoxSelectItem = null;
			this.txtAddType.ComboBoxValueMember = null;
			this.txtAddType.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.txtAddType.DLabel_Blink = false;
			this.txtAddType.DLabel_BlinkColor = System.Drawing.Color.White;
			this.txtAddType.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtAddType.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtAddType.DLabel_FontAutoSize = false;
			this.txtAddType.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtAddType.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtAddType.InputType = Function.form.usrInputBox.enInputType.COMBO;
			this.txtAddType.Label_BackColor = System.Drawing.Color.Transparent;
			this.txtAddType.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtAddType.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtAddType.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtAddType.Label_Text = "AddType";
			this.txtAddType.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.txtAddType.Label_Visable = true;
			this.txtAddType.LabelWidth = -1;
			this.txtAddType.Location = new System.Drawing.Point(4, 3);
			this.txtAddType.Multiline = false;
			this.txtAddType.Name = "txtAddType";
			this.txtAddType.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.txtAddType.Size = new System.Drawing.Size(389, 22);
			this.txtAddType.TabIndex = 16;
			this.txtAddType.TEXT = "";
			this.txtAddType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtAddType.TextType = Function.form.usrInputBox.enTextType.All;
			this.txtAddType.Value = "";
			this.txtAddType.Text_Changed += new Function.form.usrEventHander(this.txtAddType_Text_Changed);
			// 
			// gcLog
			// 
			this.gcLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gcLog.Location = new System.Drawing.Point(0, 17);
			this.gcLog.MainView = this.gvLog;
			this.gcLog.Name = "gcLog";
			this.gcLog.Size = new System.Drawing.Size(485, 451);
			this.gcLog.TabIndex = 17;
			this.gcLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvLog});
			// 
			// gvLog
			// 
			this.gvLog.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTime,
            this.colType,
            this.colLog});
			this.gvLog.GridControl = this.gcLog;
			this.gvLog.Name = "gvLog";
			// 
			// colTime
			// 
			this.colTime.Caption = "시 간";
			this.colTime.FieldName = "Time";
			this.colTime.Name = "colTime";
			this.colTime.Visible = true;
			this.colTime.VisibleIndex = 0;
			// 
			// colType
			// 
			this.colType.Caption = "타 입";
			this.colType.FieldName = "Type";
			this.colType.Name = "colType";
			this.colType.Visible = true;
			this.colType.VisibleIndex = 1;
			// 
			// colLog
			// 
			this.colLog.Caption = "Log";
			this.colLog.FieldName = "Log";
			this.colLog.Name = "colLog";
			this.colLog.Visible = true;
			this.colLog.VisibleIndex = 2;
			// 
			// pnlBottom
			// 
			this.pnlBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlBottom.Controls.Add(this.lblServerStats);
			this.pnlBottom.Controls.Add(this.btnOnOff);
			this.pnlBottom.Controls.Add(this.label1);
			this.pnlBottom.Controls.Add(this.txtPort);
			this.pnlBottom.Controls.Add(this.button1);
			this.pnlBottom.Controls.Add(this.txtMutiple);
			this.pnlBottom.Controls.Add(this.label4);
			this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBottom.Location = new System.Drawing.Point(0, 468);
			this.pnlBottom.Name = "pnlBottom";
			this.pnlBottom.Size = new System.Drawing.Size(946, 34);
			this.pnlBottom.TabIndex = 18;
			// 
			// pnlLeft
			// 
			this.pnlLeft.Controls.Add(this.spAddress);
			this.pnlLeft.Controls.Add(this.pnlAddress);
			this.pnlLeft.Controls.Add(this.pnlLeftDown);
			this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlLeft.Location = new System.Drawing.Point(0, 0);
			this.pnlLeft.Name = "pnlLeft";
			this.pnlLeft.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.pnlLeft.Size = new System.Drawing.Size(456, 468);
			this.pnlLeft.TabIndex = 19;
			// 
			// spAddress
			// 
			this.spAddress.BackColor = System.Drawing.SystemColors.ControlDark;
			this.spAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.spAddress.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.spAddress.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.spAddress.Location = new System.Drawing.Point(0, 252);
			this.spAddress.Name = "spAddress";
			this.spAddress.Size = new System.Drawing.Size(456, 5);
			this.spAddress.TabIndex = 22;
			this.spAddress.TabStop = false;
			// 
			// pnlAddress
			// 
			this.pnlAddress.Controls.Add(this.gcAdd);
			this.pnlAddress.Controls.Add(this.label2);
			this.pnlAddress.Controls.Add(this.panel1);
			this.pnlAddress.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlAddress.Location = new System.Drawing.Point(0, 5);
			this.pnlAddress.Name = "pnlAddress";
			this.pnlAddress.Size = new System.Drawing.Size(456, 252);
			this.pnlAddress.TabIndex = 18;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.chkAddGroup);
			this.panel1.Controls.Add(this.txtAddType);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(456, 28);
			this.panel1.TabIndex = 16;
			// 
			// chkAddGroup
			// 
			this.chkAddGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkAddGroup.AutoSize = true;
			this.chkAddGroup.Location = new System.Drawing.Point(399, 7);
			this.chkAddGroup.Name = "chkAddGroup";
			this.chkAddGroup.Size = new System.Drawing.Size(54, 16);
			this.chkAddGroup.TabIndex = 17;
			this.chkAddGroup.Text = "Group";
			this.chkAddGroup.UseVisualStyleBackColor = true;
			this.chkAddGroup.CheckedChanged += new System.EventHandler(this.chkAddGroup_CheckedChanged);
			// 
			// pnlLeftDown
			// 
			this.pnlLeftDown.Controls.Add(this.gcMng);
			this.pnlLeftDown.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlLeftDown.Location = new System.Drawing.Point(0, 257);
			this.pnlLeftDown.Name = "pnlLeftDown";
			this.pnlLeftDown.Size = new System.Drawing.Size(456, 211);
			this.pnlLeftDown.TabIndex = 17;
			// 
			// gcMng
			// 
			this.gcMng.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gcMng.Location = new System.Drawing.Point(0, 0);
			this.gcMng.MainView = this.gvMng;
			this.gcMng.Name = "gcMng";
			this.gcMng.Size = new System.Drawing.Size(456, 211);
			this.gcMng.TabIndex = 16;
			this.gcMng.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMng});
			this.gcMng.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gcMng_MouseClick);
			// 
			// gvMng
			// 
			this.gvMng.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.mngPriority,
            this.mngMng_Type,
            this.mngisUse,
            this.mngAddress,
            this.mngAddress_Length,
            this.mngValue,
            this.mngValueType,
            this.mngCondtion,
            this.mngDesc,
            this.mngButton});
			this.gvMng.GridControl = this.gcMng;
			this.gvMng.Name = "gvMng";
			// 
			// mngPriority
			// 
			this.mngPriority.AppearanceHeader.Options.UseTextOptions = true;
			this.mngPriority.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.mngPriority.Caption = "순서";
			this.mngPriority.FieldName = "Priority";
			this.mngPriority.Name = "mngPriority";
			this.mngPriority.Visible = true;
			this.mngPriority.VisibleIndex = 0;
			// 
			// mngMng_Type
			// 
			this.mngMng_Type.AppearanceHeader.Options.UseTextOptions = true;
			this.mngMng_Type.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.mngMng_Type.Caption = "관리타입";
			this.mngMng_Type.FieldName = "MNG_Type";
			this.mngMng_Type.Name = "mngMng_Type";
			this.mngMng_Type.Visible = true;
			this.mngMng_Type.VisibleIndex = 1;
			// 
			// mngisUse
			// 
			this.mngisUse.AppearanceHeader.Options.UseTextOptions = true;
			this.mngisUse.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.mngisUse.Caption = "U";
			this.mngisUse.FieldName = "isUse";
			this.mngisUse.Name = "mngisUse";
			this.mngisUse.Visible = true;
			this.mngisUse.VisibleIndex = 2;
			// 
			// mngAddress
			// 
			this.mngAddress.AppearanceHeader.Options.UseTextOptions = true;
			this.mngAddress.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.mngAddress.Caption = "Address";
			this.mngAddress.FieldName = "Address";
			this.mngAddress.Name = "mngAddress";
			this.mngAddress.Visible = true;
			this.mngAddress.VisibleIndex = 3;
			// 
			// mngAddress_Length
			// 
			this.mngAddress_Length.AppearanceHeader.Options.UseTextOptions = true;
			this.mngAddress_Length.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.mngAddress_Length.Caption = "길이";
			this.mngAddress_Length.FieldName = "Address_Length";
			this.mngAddress_Length.Name = "mngAddress_Length";
			this.mngAddress_Length.Visible = true;
			this.mngAddress_Length.VisibleIndex = 4;
			// 
			// mngValue
			// 
			this.mngValue.AppearanceHeader.Options.UseTextOptions = true;
			this.mngValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.mngValue.Caption = "Value";
			this.mngValue.FieldName = "Value";
			this.mngValue.Name = "mngValue";
			this.mngValue.Visible = true;
			this.mngValue.VisibleIndex = 5;
			// 
			// mngValueType
			// 
			this.mngValueType.AppearanceHeader.Options.UseTextOptions = true;
			this.mngValueType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.mngValueType.Caption = "형식";
			this.mngValueType.FieldName = "ValueType";
			this.mngValueType.Name = "mngValueType";
			this.mngValueType.Visible = true;
			this.mngValueType.VisibleIndex = 6;
			// 
			// mngCondtion
			// 
			this.mngCondtion.AppearanceHeader.Options.UseTextOptions = true;
			this.mngCondtion.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.mngCondtion.Caption = "Condition";
			this.mngCondtion.FieldName = "Condition";
			this.mngCondtion.Name = "mngCondtion";
			this.mngCondtion.Visible = true;
			this.mngCondtion.VisibleIndex = 7;
			// 
			// mngDesc
			// 
			this.mngDesc.AppearanceHeader.Options.UseTextOptions = true;
			this.mngDesc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.mngDesc.Caption = "Desc";
			this.mngDesc.FieldName = "Desc";
			this.mngDesc.Name = "mngDesc";
			this.mngDesc.Visible = true;
			this.mngDesc.VisibleIndex = 8;
			// 
			// mngButton
			// 
			this.mngButton.AppearanceHeader.Options.UseTextOptions = true;
			this.mngButton.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.mngButton.Caption = "실행";
			this.mngButton.FieldName = "Button";
			this.mngButton.Name = "mngButton";
			this.mngButton.Visible = true;
			this.mngButton.VisibleIndex = 9;
			// 
			// pnlRight
			// 
			this.pnlRight.Controls.Add(this.gcLog);
			this.pnlRight.Controls.Add(this.label3);
			this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlRight.Location = new System.Drawing.Point(461, 0);
			this.pnlRight.Name = "pnlRight";
			this.pnlRight.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.pnlRight.Size = new System.Drawing.Size(485, 468);
			this.pnlRight.TabIndex = 20;
			// 
			// spLeft
			// 
			this.spLeft.BackColor = System.Drawing.SystemColors.ControlDark;
			this.spLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.spLeft.Location = new System.Drawing.Point(456, 0);
			this.spLeft.Name = "spLeft";
			this.spLeft.Size = new System.Drawing.Size(5, 468);
			this.spLeft.TabIndex = 21;
			this.spLeft.TabStop = false;
			// 
			// frmLSXGT
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(946, 524);
			this.Controls.Add(this.pnlRight);
			this.Controls.Add(this.spLeft);
			this.Controls.Add(this.pnlLeft);
			this.Controls.Add(this.pnlBottom);
			this.Name = "frmLSXGT";
			this.Text = "LS XGT Server";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Q_TagPublish_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLSXGT_FormClosed);
			this.Load += new System.EventHandler(this.frmServer_Load);
			this.Controls.SetChildIndex(this.pnlBottom, 0);
			this.Controls.SetChildIndex(this.pnlLeft, 0);
			this.Controls.SetChildIndex(this.spLeft, 0);
			this.Controls.SetChildIndex(this.pnlRight, 0);
			((System.ComponentModel.ISupportInitialize)(this.gcAdd)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gvAdd)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gcLog)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gvLog)).EndInit();
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.pnlLeft.ResumeLayout(false);
			this.pnlAddress.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.pnlLeftDown.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gcMng)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gvMng)).EndInit();
			this.pnlRight.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Button btnOnOff;
        private System.Windows.Forms.Label lblServerStats;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.MaskedTextBox txtPort;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.MaskedTextBox txtMutiple;
		private System.Windows.Forms.Label label4;
		private DevExpress.XtraGrid.GridControl gcAdd;
		private DevExpress.XtraGrid.Views.Grid.GridView gvAdd;
		private Function.form.usrInputBox txtAddType;
		private DevExpress.XtraGrid.GridControl gcLog;
		private DevExpress.XtraGrid.Views.Grid.GridView gvLog;
		private System.Windows.Forms.Panel pnlBottom;
		private System.Windows.Forms.Panel pnlLeft;
		private System.Windows.Forms.Panel pnlLeftDown;
		private System.Windows.Forms.Panel pnlRight;
		private System.Windows.Forms.Splitter spLeft;
		private System.Windows.Forms.Splitter spAddress;
		private System.Windows.Forms.Panel pnlAddress;
		private DevExpress.XtraGrid.Columns.GridColumn colAddress;
		private DevExpress.XtraGrid.Columns.GridColumn colValue;
		private DevExpress.XtraGrid.Columns.GridColumn colHexValue;
		private DevExpress.XtraGrid.Columns.GridColumn colDesc;
		private DevExpress.XtraGrid.Columns.GridColumn colPriority;
		private DevExpress.XtraGrid.Columns.GridColumn colTime;
		private DevExpress.XtraGrid.Columns.GridColumn colType;
		private DevExpress.XtraGrid.Columns.GridColumn colLog;
		private DevExpress.XtraGrid.GridControl gcMng;
		private DevExpress.XtraGrid.Views.Grid.GridView gvMng;
		private DevExpress.XtraGrid.Columns.GridColumn mngPriority;
		private DevExpress.XtraGrid.Columns.GridColumn mngMng_Type;
		private DevExpress.XtraGrid.Columns.GridColumn mngAddress;
		private DevExpress.XtraGrid.Columns.GridColumn mngAddress_Length;
		private DevExpress.XtraGrid.Columns.GridColumn mngValue;
		private DevExpress.XtraGrid.Columns.GridColumn mngValueType;
		private DevExpress.XtraGrid.Columns.GridColumn mngCondtion;
		private DevExpress.XtraGrid.Columns.GridColumn mngDesc;
		private DevExpress.XtraGrid.Columns.GridColumn mngButton;
		private DevExpress.XtraGrid.Columns.GridColumn mngisUse;
		private DevExpress.XtraGrid.Columns.GridColumn colGroup;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox chkAddGroup;
        
    }
}

