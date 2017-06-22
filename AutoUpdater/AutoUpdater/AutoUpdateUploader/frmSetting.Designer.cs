namespace AutoUpdater
{
	partial class frmSetting
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
			this.components = new System.ComponentModel.Container();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmdAdd = new System.Windows.Forms.ToolStripMenuItem();
			this.cmdDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlPoint = new System.Windows.Forms.Panel();
			this.txtPriority = new Function.form.usrInputBox();
			this.cmbGroup = new Function.form.usrInputBox();
			this.chkUseZip = new System.Windows.Forms.CheckBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.txtBigo = new Function.form.usrInputBox();
			this.txtUpdateType = new Function.form.usrInputBox();
			this.cmbType = new Function.form.usrInputBox();
			this.usrGroupHeader1 = new Function.form.usrGroupHeader();
			this.pnlOracle = new System.Windows.Forms.Panel();
			this.txtOraPass = new Function.form.usrInputBox();
			this.txtOraID = new Function.form.usrInputBox();
			this.txtOraTNS = new Function.form.usrInputBox();
			this.usrGroupHeader2 = new Function.form.usrGroupHeader();
			this.pnlConnSql = new System.Windows.Forms.Panel();
			this.txtSqlPass = new Function.form.usrInputBox();
			this.txtSqlID = new Function.form.usrInputBox();
			this.txtSqlDB = new Function.form.usrInputBox();
			this.txtSqlIP = new Function.form.usrInputBox();
			this.usrGroupHeader3 = new Function.form.usrGroupHeader();
			this.pnlConnWEB = new System.Windows.Forms.Panel();
			this.txtWEBPass = new Function.form.usrInputBox();
			this.txtWEBUrl = new Function.form.usrInputBox();
			this.usrGroupHeader4 = new Function.form.usrGroupHeader();
			this.gcSettingList = new DevExpress.XtraGrid.GridControl();
			this.gvSettingList = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.contextMenuStrip1.SuspendLayout();
			this.pnlPoint.SuspendLayout();
			this.pnlOracle.SuspendLayout();
			this.pnlConnSql.SuspendLayout();
			this.pnlConnWEB.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gcSettingList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gvSettingList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdAdd,
            this.cmdDelete});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(127, 48);
			// 
			// cmdAdd
			// 
			this.cmdAdd.Name = "cmdAdd";
			this.cmdAdd.Size = new System.Drawing.Size(126, 22);
			this.cmdAdd.Text = "항목 추가";
			this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
			// 
			// cmdDelete
			// 
			this.cmdDelete.Name = "cmdDelete";
			this.cmdDelete.Size = new System.Drawing.Size(126, 22);
			this.cmdDelete.Text = "항목 삭제";
			this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
			// 
			// pnlPoint
			// 
			this.pnlPoint.Controls.Add(this.txtPriority);
			this.pnlPoint.Controls.Add(this.cmbGroup);
			this.pnlPoint.Controls.Add(this.chkUseZip);
			this.pnlPoint.Controls.Add(this.btnSave);
			this.pnlPoint.Controls.Add(this.txtBigo);
			this.pnlPoint.Controls.Add(this.txtUpdateType);
			this.pnlPoint.Controls.Add(this.cmbType);
			this.pnlPoint.Controls.Add(this.usrGroupHeader1);
			this.pnlPoint.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlPoint.Location = new System.Drawing.Point(0, 0);
			this.pnlPoint.Name = "pnlPoint";
			this.pnlPoint.Size = new System.Drawing.Size(559, 105);
			this.pnlPoint.TabIndex = 2;
			// 
			// txtPriority
			// 
			this.txtPriority.BackColor = System.Drawing.SystemColors.Control;
			this.txtPriority.ChangeMark_Visable = false;
			this.txtPriority.ComboBoxDataSource = null;
			this.txtPriority.ComboBoxDisplayMember = "";
			this.txtPriority.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.txtPriority.ComboBoxSelectedValue = null;
			this.txtPriority.ComboBoxSelectIndex = -1;
			this.txtPriority.ComboBoxSelectItem = null;
			this.txtPriority.ComboBoxValueMember = "";
			this.txtPriority.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.txtPriority.DLabel_Blink = false;
			this.txtPriority.DLabel_BlinkColor = System.Drawing.Color.White;
			this.txtPriority.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtPriority.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtPriority.DLabel_FontAutoSize = false;
			this.txtPriority.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtPriority.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtPriority.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.txtPriority.Label_BackColor = System.Drawing.Color.Transparent;
			this.txtPriority.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtPriority.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtPriority.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtPriority.Label_Text = "Priority";
			this.txtPriority.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.txtPriority.Label_Visable = true;
			this.txtPriority.LabelWidth = -1;
			this.txtPriority.Location = new System.Drawing.Point(327, 29);
			this.txtPriority.Multiline = false;
			this.txtPriority.Name = "txtPriority";
			this.txtPriority.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.txtPriority.Size = new System.Drawing.Size(229, 23);
			this.txtPriority.TabIndex = 7;
			this.txtPriority.TEXT = "";
			this.txtPriority.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtPriority.TEXTBOX_PasswordChar = '\0';
			this.txtPriority.TextType = Function.form.usrInputBox.enTextType.NumberOlny;
			this.txtPriority.Value = "";
			// 
			// cmbGroup
			// 
			this.cmbGroup.BackColor = System.Drawing.SystemColors.Control;
			this.cmbGroup.ChangeMark_Visable = false;
			this.cmbGroup.ComboBoxDataSource = null;
			this.cmbGroup.ComboBoxDisplayMember = "";
			this.cmbGroup.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbGroup.ComboBoxSelectedValue = null;
			this.cmbGroup.ComboBoxSelectIndex = -1;
			this.cmbGroup.ComboBoxSelectItem = null;
			this.cmbGroup.ComboBoxValueMember = "";
			this.cmbGroup.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.cmbGroup.DLabel_Blink = false;
			this.cmbGroup.DLabel_BlinkColor = System.Drawing.Color.White;
			this.cmbGroup.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.cmbGroup.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.cmbGroup.DLabel_FontAutoSize = false;
			this.cmbGroup.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbGroup.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.cmbGroup.InputType = Function.form.usrInputBox.enInputType.COMBO;
			this.cmbGroup.Label_BackColor = System.Drawing.Color.Transparent;
			this.cmbGroup.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.cmbGroup.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.cmbGroup.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbGroup.Label_Text = "Group";
			this.cmbGroup.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cmbGroup.Label_Visable = true;
			this.cmbGroup.LabelWidth = 35;
			this.cmbGroup.Location = new System.Drawing.Point(3, 29);
			this.cmbGroup.Multiline = false;
			this.cmbGroup.Name = "cmbGroup";
			this.cmbGroup.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.cmbGroup.Size = new System.Drawing.Size(318, 22);
			this.cmbGroup.TabIndex = 6;
			this.cmbGroup.TEXT = "";
			this.cmbGroup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.cmbGroup.TEXTBOX_PasswordChar = '\0';
			this.cmbGroup.TextType = Function.form.usrInputBox.enTextType.All;
			this.cmbGroup.Value = "";
			// 
			// chkUseZip
			// 
			this.chkUseZip.AutoSize = true;
			this.chkUseZip.Location = new System.Drawing.Point(365, 83);
			this.chkUseZip.Name = "chkUseZip";
			this.chkUseZip.Size = new System.Drawing.Size(126, 16);
			this.chkUseZip.TabIndex = 5;
			this.chkUseZip.Text = "Zip압축 파일 이용";
			this.chkUseZip.UseVisualStyleBackColor = true;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(497, 79);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(59, 22);
			this.btnSave.TabIndex = 4;
			this.btnSave.Text = "저 장";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// txtBigo
			// 
			this.txtBigo.BackColor = System.Drawing.SystemColors.Control;
			this.txtBigo.ChangeMark_Visable = false;
			this.txtBigo.ComboBoxDataSource = null;
			this.txtBigo.ComboBoxDisplayMember = "";
			this.txtBigo.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.txtBigo.ComboBoxSelectedValue = null;
			this.txtBigo.ComboBoxSelectIndex = -1;
			this.txtBigo.ComboBoxSelectItem = null;
			this.txtBigo.ComboBoxValueMember = "";
			this.txtBigo.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.txtBigo.DLabel_Blink = false;
			this.txtBigo.DLabel_BlinkColor = System.Drawing.Color.White;
			this.txtBigo.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtBigo.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtBigo.DLabel_FontAutoSize = false;
			this.txtBigo.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtBigo.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtBigo.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.txtBigo.Label_BackColor = System.Drawing.Color.Transparent;
			this.txtBigo.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtBigo.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtBigo.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtBigo.Label_Text = "비고";
			this.txtBigo.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.txtBigo.Label_Visable = true;
			this.txtBigo.LabelWidth = -1;
			this.txtBigo.Location = new System.Drawing.Point(3, 79);
			this.txtBigo.Multiline = false;
			this.txtBigo.Name = "txtBigo";
			this.txtBigo.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.txtBigo.Size = new System.Drawing.Size(356, 23);
			this.txtBigo.TabIndex = 3;
			this.txtBigo.TEXT = "";
			this.txtBigo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtBigo.TEXTBOX_PasswordChar = '\0';
			this.txtBigo.TextType = Function.form.usrInputBox.enTextType.All;
			this.txtBigo.Value = "";
			// 
			// txtUpdateType
			// 
			this.txtUpdateType.BackColor = System.Drawing.SystemColors.Control;
			this.txtUpdateType.ChangeMark_Visable = false;
			this.txtUpdateType.ComboBoxDataSource = null;
			this.txtUpdateType.ComboBoxDisplayMember = "";
			this.txtUpdateType.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.txtUpdateType.ComboBoxSelectedValue = null;
			this.txtUpdateType.ComboBoxSelectIndex = -1;
			this.txtUpdateType.ComboBoxSelectItem = null;
			this.txtUpdateType.ComboBoxValueMember = "";
			this.txtUpdateType.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.txtUpdateType.DLabel_Blink = false;
			this.txtUpdateType.DLabel_BlinkColor = System.Drawing.Color.White;
			this.txtUpdateType.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtUpdateType.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtUpdateType.DLabel_FontAutoSize = false;
			this.txtUpdateType.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtUpdateType.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtUpdateType.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.txtUpdateType.Label_BackColor = System.Drawing.Color.Transparent;
			this.txtUpdateType.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtUpdateType.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtUpdateType.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtUpdateType.Label_Text = "UpdateType";
			this.txtUpdateType.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.txtUpdateType.Label_Visable = true;
			this.txtUpdateType.LabelWidth = -1;
			this.txtUpdateType.Location = new System.Drawing.Point(200, 55);
			this.txtUpdateType.Multiline = false;
			this.txtUpdateType.Name = "txtUpdateType";
			this.txtUpdateType.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.txtUpdateType.Size = new System.Drawing.Size(356, 23);
			this.txtUpdateType.TabIndex = 2;
			this.txtUpdateType.TEXT = "";
			this.txtUpdateType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtUpdateType.TEXTBOX_PasswordChar = '\0';
			this.txtUpdateType.TextType = Function.form.usrInputBox.enTextType.All;
			this.txtUpdateType.Value = "";
			// 
			// cmbType
			// 
			this.cmbType.BackColor = System.Drawing.SystemColors.Control;
			this.cmbType.ChangeMark_Visable = false;
			this.cmbType.ComboBoxDataSource = null;
			this.cmbType.ComboBoxDisplayMember = "";
			this.cmbType.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbType.ComboBoxSelectedValue = null;
			this.cmbType.ComboBoxSelectIndex = -1;
			this.cmbType.ComboBoxSelectItem = null;
			this.cmbType.ComboBoxValueMember = "";
			this.cmbType.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.cmbType.DLabel_Blink = false;
			this.cmbType.DLabel_BlinkColor = System.Drawing.Color.White;
			this.cmbType.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.cmbType.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.cmbType.DLabel_FontAutoSize = false;
			this.cmbType.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbType.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.cmbType.InputType = Function.form.usrInputBox.enInputType.COMBO;
			this.cmbType.Label_BackColor = System.Drawing.Color.Transparent;
			this.cmbType.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.cmbType.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.cmbType.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmbType.Label_Text = "TYPE";
			this.cmbType.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cmbType.Label_Visable = true;
			this.cmbType.LabelWidth = 35;
			this.cmbType.Location = new System.Drawing.Point(3, 55);
			this.cmbType.Multiline = false;
			this.cmbType.Name = "cmbType";
			this.cmbType.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.cmbType.Size = new System.Drawing.Size(191, 22);
			this.cmbType.TabIndex = 1;
			this.cmbType.TEXT = "";
			this.cmbType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.cmbType.TEXTBOX_PasswordChar = '\0';
			this.cmbType.TextType = Function.form.usrInputBox.enTextType.All;
			this.cmbType.Value = "";
			this.cmbType.Text_Changed += new Function.form.usrEventHander(this.cmbType_Text_Changed);
			// 
			// usrGroupHeader1
			// 
			this.usrGroupHeader1.BackColor = System.Drawing.Color.RoyalBlue;
			this.usrGroupHeader1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.usrGroupHeader1.Dock = System.Windows.Forms.DockStyle.Top;
			this.usrGroupHeader1.Location = new System.Drawing.Point(0, 0);
			this.usrGroupHeader1.Name = "usrGroupHeader1";
			this.usrGroupHeader1.Size = new System.Drawing.Size(559, 26);
			this.usrGroupHeader1.TabIndex = 0;
			this.usrGroupHeader1.TEXT = "포인트 정보";
			this.usrGroupHeader1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlOracle
			// 
			this.pnlOracle.Controls.Add(this.txtOraPass);
			this.pnlOracle.Controls.Add(this.txtOraID);
			this.pnlOracle.Controls.Add(this.txtOraTNS);
			this.pnlOracle.Controls.Add(this.usrGroupHeader2);
			this.pnlOracle.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlOracle.Location = new System.Drawing.Point(0, 232);
			this.pnlOracle.Name = "pnlOracle";
			this.pnlOracle.Size = new System.Drawing.Size(559, 232);
			this.pnlOracle.TabIndex = 3;
			// 
			// txtOraPass
			// 
			this.txtOraPass.BackColor = System.Drawing.SystemColors.Control;
			this.txtOraPass.ChangeMark_Visable = false;
			this.txtOraPass.ComboBoxDataSource = null;
			this.txtOraPass.ComboBoxDisplayMember = "";
			this.txtOraPass.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.txtOraPass.ComboBoxSelectedValue = null;
			this.txtOraPass.ComboBoxSelectIndex = -1;
			this.txtOraPass.ComboBoxSelectItem = null;
			this.txtOraPass.ComboBoxValueMember = "";
			this.txtOraPass.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.txtOraPass.DLabel_Blink = false;
			this.txtOraPass.DLabel_BlinkColor = System.Drawing.Color.White;
			this.txtOraPass.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtOraPass.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtOraPass.DLabel_FontAutoSize = false;
			this.txtOraPass.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtOraPass.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtOraPass.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.txtOraPass.Label_BackColor = System.Drawing.Color.Transparent;
			this.txtOraPass.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtOraPass.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtOraPass.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtOraPass.Label_Text = "Passwords";
			this.txtOraPass.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.txtOraPass.Label_Visable = true;
			this.txtOraPass.LabelWidth = -1;
			this.txtOraPass.Location = new System.Drawing.Point(202, 207);
			this.txtOraPass.Multiline = false;
			this.txtOraPass.Name = "txtOraPass";
			this.txtOraPass.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.txtOraPass.Size = new System.Drawing.Size(223, 23);
			this.txtOraPass.TabIndex = 5;
			this.txtOraPass.TEXT = "";
			this.txtOraPass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtOraPass.TEXTBOX_PasswordChar = '\0';
			this.txtOraPass.TextType = Function.form.usrInputBox.enTextType.All;
			this.txtOraPass.Value = "";
			// 
			// txtOraID
			// 
			this.txtOraID.BackColor = System.Drawing.SystemColors.Control;
			this.txtOraID.ChangeMark_Visable = false;
			this.txtOraID.ComboBoxDataSource = null;
			this.txtOraID.ComboBoxDisplayMember = "";
			this.txtOraID.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.txtOraID.ComboBoxSelectedValue = null;
			this.txtOraID.ComboBoxSelectIndex = -1;
			this.txtOraID.ComboBoxSelectItem = null;
			this.txtOraID.ComboBoxValueMember = "";
			this.txtOraID.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.txtOraID.DLabel_Blink = false;
			this.txtOraID.DLabel_BlinkColor = System.Drawing.Color.White;
			this.txtOraID.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtOraID.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtOraID.DLabel_FontAutoSize = false;
			this.txtOraID.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtOraID.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtOraID.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.txtOraID.Label_BackColor = System.Drawing.Color.Transparent;
			this.txtOraID.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtOraID.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtOraID.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtOraID.Label_Text = "ID";
			this.txtOraID.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.txtOraID.Label_Visable = true;
			this.txtOraID.LabelWidth = 35;
			this.txtOraID.Location = new System.Drawing.Point(3, 207);
			this.txtOraID.Multiline = false;
			this.txtOraID.Name = "txtOraID";
			this.txtOraID.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.txtOraID.Size = new System.Drawing.Size(193, 23);
			this.txtOraID.TabIndex = 4;
			this.txtOraID.TEXT = "";
			this.txtOraID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtOraID.TEXTBOX_PasswordChar = '\0';
			this.txtOraID.TextType = Function.form.usrInputBox.enTextType.All;
			this.txtOraID.Value = "";
			// 
			// txtOraTNS
			// 
			this.txtOraTNS.BackColor = System.Drawing.SystemColors.Control;
			this.txtOraTNS.ChangeMark_Visable = false;
			this.txtOraTNS.ComboBoxDataSource = null;
			this.txtOraTNS.ComboBoxDisplayMember = "";
			this.txtOraTNS.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.txtOraTNS.ComboBoxSelectedValue = null;
			this.txtOraTNS.ComboBoxSelectIndex = -1;
			this.txtOraTNS.ComboBoxSelectItem = null;
			this.txtOraTNS.ComboBoxValueMember = "";
			this.txtOraTNS.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.txtOraTNS.DLabel_Blink = false;
			this.txtOraTNS.DLabel_BlinkColor = System.Drawing.Color.White;
			this.txtOraTNS.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtOraTNS.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtOraTNS.DLabel_FontAutoSize = false;
			this.txtOraTNS.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtOraTNS.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtOraTNS.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.txtOraTNS.Label_BackColor = System.Drawing.Color.Transparent;
			this.txtOraTNS.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtOraTNS.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtOraTNS.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtOraTNS.Label_Text = "TNS";
			this.txtOraTNS.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.txtOraTNS.Label_Visable = true;
			this.txtOraTNS.LabelWidth = 35;
			this.txtOraTNS.Location = new System.Drawing.Point(3, 32);
			this.txtOraTNS.Multiline = true;
			this.txtOraTNS.Name = "txtOraTNS";
			this.txtOraTNS.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.txtOraTNS.Size = new System.Drawing.Size(553, 177);
			this.txtOraTNS.TabIndex = 3;
			this.txtOraTNS.TEXT = "";
			this.txtOraTNS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtOraTNS.TEXTBOX_PasswordChar = '\0';
			this.txtOraTNS.TextType = Function.form.usrInputBox.enTextType.All;
			this.txtOraTNS.Value = "";
			// 
			// usrGroupHeader2
			// 
			this.usrGroupHeader2.BackColor = System.Drawing.Color.RoyalBlue;
			this.usrGroupHeader2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.usrGroupHeader2.Dock = System.Windows.Forms.DockStyle.Top;
			this.usrGroupHeader2.Location = new System.Drawing.Point(0, 0);
			this.usrGroupHeader2.Name = "usrGroupHeader2";
			this.usrGroupHeader2.Size = new System.Drawing.Size(559, 26);
			this.usrGroupHeader2.TabIndex = 1;
			this.usrGroupHeader2.TEXT = "접속 정보[ORCLE]";
			this.usrGroupHeader2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlConnSql
			// 
			this.pnlConnSql.Controls.Add(this.txtSqlPass);
			this.pnlConnSql.Controls.Add(this.txtSqlID);
			this.pnlConnSql.Controls.Add(this.txtSqlDB);
			this.pnlConnSql.Controls.Add(this.txtSqlIP);
			this.pnlConnSql.Controls.Add(this.usrGroupHeader3);
			this.pnlConnSql.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlConnSql.Location = new System.Drawing.Point(0, 464);
			this.pnlConnSql.Name = "pnlConnSql";
			this.pnlConnSql.Size = new System.Drawing.Size(559, 82);
			this.pnlConnSql.TabIndex = 4;
			// 
			// txtSqlPass
			// 
			this.txtSqlPass.BackColor = System.Drawing.SystemColors.Control;
			this.txtSqlPass.ChangeMark_Visable = false;
			this.txtSqlPass.ComboBoxDataSource = null;
			this.txtSqlPass.ComboBoxDisplayMember = "";
			this.txtSqlPass.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.txtSqlPass.ComboBoxSelectedValue = null;
			this.txtSqlPass.ComboBoxSelectIndex = -1;
			this.txtSqlPass.ComboBoxSelectItem = null;
			this.txtSqlPass.ComboBoxValueMember = "";
			this.txtSqlPass.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.txtSqlPass.DLabel_Blink = false;
			this.txtSqlPass.DLabel_BlinkColor = System.Drawing.Color.White;
			this.txtSqlPass.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtSqlPass.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtSqlPass.DLabel_FontAutoSize = false;
			this.txtSqlPass.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtSqlPass.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtSqlPass.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.txtSqlPass.Label_BackColor = System.Drawing.Color.Transparent;
			this.txtSqlPass.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtSqlPass.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtSqlPass.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtSqlPass.Label_Text = "Passwords";
			this.txtSqlPass.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.txtSqlPass.Label_Visable = true;
			this.txtSqlPass.LabelWidth = 70;
			this.txtSqlPass.Location = new System.Drawing.Point(202, 54);
			this.txtSqlPass.Multiline = false;
			this.txtSqlPass.Name = "txtSqlPass";
			this.txtSqlPass.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.txtSqlPass.Size = new System.Drawing.Size(223, 23);
			this.txtSqlPass.TabIndex = 9;
			this.txtSqlPass.TEXT = "";
			this.txtSqlPass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtSqlPass.TEXTBOX_PasswordChar = '\0';
			this.txtSqlPass.TextType = Function.form.usrInputBox.enTextType.All;
			this.txtSqlPass.Value = "";
			// 
			// txtSqlID
			// 
			this.txtSqlID.BackColor = System.Drawing.SystemColors.Control;
			this.txtSqlID.ChangeMark_Visable = false;
			this.txtSqlID.ComboBoxDataSource = null;
			this.txtSqlID.ComboBoxDisplayMember = "";
			this.txtSqlID.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.txtSqlID.ComboBoxSelectedValue = null;
			this.txtSqlID.ComboBoxSelectIndex = -1;
			this.txtSqlID.ComboBoxSelectItem = null;
			this.txtSqlID.ComboBoxValueMember = "";
			this.txtSqlID.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.txtSqlID.DLabel_Blink = false;
			this.txtSqlID.DLabel_BlinkColor = System.Drawing.Color.White;
			this.txtSqlID.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtSqlID.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtSqlID.DLabel_FontAutoSize = false;
			this.txtSqlID.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtSqlID.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtSqlID.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.txtSqlID.Label_BackColor = System.Drawing.Color.Transparent;
			this.txtSqlID.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtSqlID.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtSqlID.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtSqlID.Label_Text = "ID";
			this.txtSqlID.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.txtSqlID.Label_Visable = true;
			this.txtSqlID.LabelWidth = 35;
			this.txtSqlID.Location = new System.Drawing.Point(3, 54);
			this.txtSqlID.Multiline = false;
			this.txtSqlID.Name = "txtSqlID";
			this.txtSqlID.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.txtSqlID.Size = new System.Drawing.Size(193, 23);
			this.txtSqlID.TabIndex = 8;
			this.txtSqlID.TEXT = "";
			this.txtSqlID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtSqlID.TEXTBOX_PasswordChar = '\0';
			this.txtSqlID.TextType = Function.form.usrInputBox.enTextType.All;
			this.txtSqlID.Value = "";
			// 
			// txtSqlDB
			// 
			this.txtSqlDB.BackColor = System.Drawing.SystemColors.Control;
			this.txtSqlDB.ChangeMark_Visable = false;
			this.txtSqlDB.ComboBoxDataSource = null;
			this.txtSqlDB.ComboBoxDisplayMember = "";
			this.txtSqlDB.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.txtSqlDB.ComboBoxSelectedValue = null;
			this.txtSqlDB.ComboBoxSelectIndex = -1;
			this.txtSqlDB.ComboBoxSelectItem = null;
			this.txtSqlDB.ComboBoxValueMember = "";
			this.txtSqlDB.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.txtSqlDB.DLabel_Blink = false;
			this.txtSqlDB.DLabel_BlinkColor = System.Drawing.Color.White;
			this.txtSqlDB.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtSqlDB.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtSqlDB.DLabel_FontAutoSize = false;
			this.txtSqlDB.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtSqlDB.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtSqlDB.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.txtSqlDB.Label_BackColor = System.Drawing.Color.Transparent;
			this.txtSqlDB.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtSqlDB.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtSqlDB.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtSqlDB.Label_Text = "DataBase";
			this.txtSqlDB.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.txtSqlDB.Label_Visable = true;
			this.txtSqlDB.LabelWidth = 70;
			this.txtSqlDB.Location = new System.Drawing.Point(202, 30);
			this.txtSqlDB.Multiline = false;
			this.txtSqlDB.Name = "txtSqlDB";
			this.txtSqlDB.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.txtSqlDB.Size = new System.Drawing.Size(223, 23);
			this.txtSqlDB.TabIndex = 7;
			this.txtSqlDB.TEXT = "";
			this.txtSqlDB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtSqlDB.TEXTBOX_PasswordChar = '\0';
			this.txtSqlDB.TextType = Function.form.usrInputBox.enTextType.All;
			this.txtSqlDB.Value = "";
			// 
			// txtSqlIP
			// 
			this.txtSqlIP.BackColor = System.Drawing.SystemColors.Control;
			this.txtSqlIP.ChangeMark_Visable = false;
			this.txtSqlIP.ComboBoxDataSource = null;
			this.txtSqlIP.ComboBoxDisplayMember = "";
			this.txtSqlIP.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.txtSqlIP.ComboBoxSelectedValue = null;
			this.txtSqlIP.ComboBoxSelectIndex = -1;
			this.txtSqlIP.ComboBoxSelectItem = null;
			this.txtSqlIP.ComboBoxValueMember = "";
			this.txtSqlIP.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.txtSqlIP.DLabel_Blink = false;
			this.txtSqlIP.DLabel_BlinkColor = System.Drawing.Color.White;
			this.txtSqlIP.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtSqlIP.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtSqlIP.DLabel_FontAutoSize = false;
			this.txtSqlIP.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtSqlIP.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtSqlIP.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.txtSqlIP.Label_BackColor = System.Drawing.Color.Transparent;
			this.txtSqlIP.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtSqlIP.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtSqlIP.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtSqlIP.Label_Text = "IP";
			this.txtSqlIP.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.txtSqlIP.Label_Visable = true;
			this.txtSqlIP.LabelWidth = 35;
			this.txtSqlIP.Location = new System.Drawing.Point(3, 30);
			this.txtSqlIP.Multiline = false;
			this.txtSqlIP.Name = "txtSqlIP";
			this.txtSqlIP.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.txtSqlIP.Size = new System.Drawing.Size(193, 23);
			this.txtSqlIP.TabIndex = 6;
			this.txtSqlIP.TEXT = "";
			this.txtSqlIP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtSqlIP.TEXTBOX_PasswordChar = '\0';
			this.txtSqlIP.TextType = Function.form.usrInputBox.enTextType.All;
			this.txtSqlIP.Value = "";
			// 
			// usrGroupHeader3
			// 
			this.usrGroupHeader3.BackColor = System.Drawing.Color.RoyalBlue;
			this.usrGroupHeader3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.usrGroupHeader3.Dock = System.Windows.Forms.DockStyle.Top;
			this.usrGroupHeader3.Location = new System.Drawing.Point(0, 0);
			this.usrGroupHeader3.Name = "usrGroupHeader3";
			this.usrGroupHeader3.Size = new System.Drawing.Size(559, 26);
			this.usrGroupHeader3.TabIndex = 1;
			this.usrGroupHeader3.TEXT = "접속 정보[SQL]";
			this.usrGroupHeader3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlConnWEB
			// 
			this.pnlConnWEB.Controls.Add(this.txtWEBPass);
			this.pnlConnWEB.Controls.Add(this.txtWEBUrl);
			this.pnlConnWEB.Controls.Add(this.usrGroupHeader4);
			this.pnlConnWEB.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlConnWEB.Location = new System.Drawing.Point(0, 105);
			this.pnlConnWEB.Name = "pnlConnWEB";
			this.pnlConnWEB.Size = new System.Drawing.Size(559, 127);
			this.pnlConnWEB.TabIndex = 5;
			// 
			// txtWEBPass
			// 
			this.txtWEBPass.BackColor = System.Drawing.SystemColors.Control;
			this.txtWEBPass.ChangeMark_Visable = false;
			this.txtWEBPass.ComboBoxDataSource = null;
			this.txtWEBPass.ComboBoxDisplayMember = "";
			this.txtWEBPass.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.txtWEBPass.ComboBoxSelectedValue = null;
			this.txtWEBPass.ComboBoxSelectIndex = -1;
			this.txtWEBPass.ComboBoxSelectItem = null;
			this.txtWEBPass.ComboBoxValueMember = "";
			this.txtWEBPass.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.txtWEBPass.DLabel_Blink = false;
			this.txtWEBPass.DLabel_BlinkColor = System.Drawing.Color.White;
			this.txtWEBPass.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtWEBPass.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtWEBPass.DLabel_FontAutoSize = false;
			this.txtWEBPass.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtWEBPass.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtWEBPass.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.txtWEBPass.Label_BackColor = System.Drawing.Color.Transparent;
			this.txtWEBPass.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtWEBPass.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtWEBPass.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtWEBPass.Label_Text = "Passwords";
			this.txtWEBPass.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.txtWEBPass.Label_Visable = true;
			this.txtWEBPass.LabelWidth = 70;
			this.txtWEBPass.Location = new System.Drawing.Point(3, 100);
			this.txtWEBPass.Multiline = false;
			this.txtWEBPass.Name = "txtWEBPass";
			this.txtWEBPass.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.txtWEBPass.Size = new System.Drawing.Size(267, 23);
			this.txtWEBPass.TabIndex = 10;
			this.txtWEBPass.TEXT = "";
			this.txtWEBPass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtWEBPass.TEXTBOX_PasswordChar = '\0';
			this.txtWEBPass.TextType = Function.form.usrInputBox.enTextType.All;
			this.txtWEBPass.Value = "";
			// 
			// txtWEBUrl
			// 
			this.txtWEBUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtWEBUrl.BackColor = System.Drawing.SystemColors.Control;
			this.txtWEBUrl.ChangeMark_Visable = false;
			this.txtWEBUrl.ComboBoxDataSource = null;
			this.txtWEBUrl.ComboBoxDisplayMember = "";
			this.txtWEBUrl.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.txtWEBUrl.ComboBoxSelectedValue = null;
			this.txtWEBUrl.ComboBoxSelectIndex = -1;
			this.txtWEBUrl.ComboBoxSelectItem = null;
			this.txtWEBUrl.ComboBoxValueMember = "";
			this.txtWEBUrl.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.txtWEBUrl.DLabel_Blink = false;
			this.txtWEBUrl.DLabel_BlinkColor = System.Drawing.Color.White;
			this.txtWEBUrl.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtWEBUrl.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtWEBUrl.DLabel_FontAutoSize = false;
			this.txtWEBUrl.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtWEBUrl.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtWEBUrl.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.txtWEBUrl.Label_BackColor = System.Drawing.Color.Transparent;
			this.txtWEBUrl.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtWEBUrl.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.txtWEBUrl.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtWEBUrl.Label_Text = "URL";
			this.txtWEBUrl.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.txtWEBUrl.Label_Visable = true;
			this.txtWEBUrl.LabelWidth = 35;
			this.txtWEBUrl.Location = new System.Drawing.Point(3, 29);
			this.txtWEBUrl.Multiline = true;
			this.txtWEBUrl.Name = "txtWEBUrl";
			this.txtWEBUrl.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.txtWEBUrl.Size = new System.Drawing.Size(553, 73);
			this.txtWEBUrl.TabIndex = 8;
			this.txtWEBUrl.TEXT = "";
			this.txtWEBUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtWEBUrl.TEXTBOX_PasswordChar = '\0';
			this.txtWEBUrl.TextType = Function.form.usrInputBox.enTextType.All;
			this.txtWEBUrl.Value = "";
			// 
			// usrGroupHeader4
			// 
			this.usrGroupHeader4.BackColor = System.Drawing.Color.RoyalBlue;
			this.usrGroupHeader4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.usrGroupHeader4.Dock = System.Windows.Forms.DockStyle.Top;
			this.usrGroupHeader4.Location = new System.Drawing.Point(0, 0);
			this.usrGroupHeader4.Name = "usrGroupHeader4";
			this.usrGroupHeader4.Size = new System.Drawing.Size(559, 26);
			this.usrGroupHeader4.TabIndex = 1;
			this.usrGroupHeader4.TEXT = "접속 정보[WEB]";
			this.usrGroupHeader4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// gcSettingList
			// 
			this.gcSettingList.ContextMenuStrip = this.contextMenuStrip1;
			this.gcSettingList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gcSettingList.Location = new System.Drawing.Point(0, 0);
			this.gcSettingList.MainView = this.gvSettingList;
			this.gcSettingList.Name = "gcSettingList";
			this.gcSettingList.Size = new System.Drawing.Size(630, 438);
			this.gcSettingList.TabIndex = 6;
			this.gcSettingList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvSettingList});
			// 
			// gvSettingList
			// 
			this.gvSettingList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5});
			this.gvSettingList.GridControl = this.gcSettingList;
			this.gvSettingList.Name = "gvSettingList";
			this.gvSettingList.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvSettingList_RowCellStyle);
			this.gvSettingList.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvSettingList_FocusedRowChanged);
			// 
			// gridColumn1
			// 
			this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
			this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColumn1.Caption = "Group";
			this.gridColumn1.FieldName = "GroupName";
			this.gridColumn1.Name = "gridColumn1";
			this.gridColumn1.Visible = true;
			this.gridColumn1.VisibleIndex = 0;
			this.gridColumn1.Width = 100;
			// 
			// gridColumn2
			// 
			this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
			this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColumn2.Caption = "Type";
			this.gridColumn2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.gridColumn2.FieldName = "TYPE";
			this.gridColumn2.Name = "gridColumn2";
			this.gridColumn2.Visible = true;
			this.gridColumn2.VisibleIndex = 1;
			this.gridColumn2.Width = 74;
			// 
			// gridColumn3
			// 
			this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
			this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColumn3.Caption = "UpdateType";
			this.gridColumn3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.gridColumn3.FieldName = "UPDATETYPE";
			this.gridColumn3.Name = "gridColumn3";
			this.gridColumn3.Visible = true;
			this.gridColumn3.VisibleIndex = 2;
			this.gridColumn3.Width = 160;
			// 
			// gridColumn4
			// 
			this.gridColumn4.Caption = "비  고";
			this.gridColumn4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.gridColumn4.FieldName = "BIGO";
			this.gridColumn4.Name = "gridColumn4";
			this.gridColumn4.Visible = true;
			this.gridColumn4.VisibleIndex = 3;
			this.gridColumn4.Width = 204;
			// 
			// gridColumn5
			// 
			this.gridColumn5.Caption = "생성일";
			this.gridColumn5.DisplayFormat.FormatString = "d";
			this.gridColumn5.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
			this.gridColumn5.FieldName = "생성일";
			this.gridColumn5.Name = "gridColumn5";
			this.gridColumn5.Visible = true;
			this.gridColumn5.VisibleIndex = 4;
			this.gridColumn5.Width = 74;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.gcSettingList);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.pnlConnSql);
			this.splitContainer1.Panel2.Controls.Add(this.pnlOracle);
			this.splitContainer1.Panel2.Controls.Add(this.pnlConnWEB);
			this.splitContainer1.Panel2.Controls.Add(this.pnlPoint);
			this.splitContainer1.Size = new System.Drawing.Size(1193, 438);
			this.splitContainer1.SplitterDistance = 630;
			this.splitContainer1.TabIndex = 7;
			// 
			// frmSetting
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1193, 460);
			this.Controls.Add(this.splitContainer1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmSetting";
			this.ShowIcon = false;
			this.Text = "Update Point 관리";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSetting_FormClosed);
			this.Load += new System.EventHandler(this.frmSetting_Load);
			this.Controls.SetChildIndex(this.splitContainer1, 0);
			this.contextMenuStrip1.ResumeLayout(false);
			this.pnlPoint.ResumeLayout(false);
			this.pnlPoint.PerformLayout();
			this.pnlOracle.ResumeLayout(false);
			this.pnlConnSql.ResumeLayout(false);
			this.pnlConnWEB.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gcSettingList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gvSettingList)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem cmdAdd;
		private System.Windows.Forms.ToolStripMenuItem cmdDelete;
		private DevExpress.XtraGrid.GridControl gcSettingList;
		private DevExpress.XtraGrid.Views.Grid.GridView gvSettingList;
		private System.Windows.Forms.Panel pnlConnWEB;
		private Function.form.usrGroupHeader usrGroupHeader4;
		private System.Windows.Forms.Panel pnlConnSql;
		private Function.form.usrGroupHeader usrGroupHeader3;
		private System.Windows.Forms.Panel pnlOracle;
		private Function.form.usrGroupHeader usrGroupHeader2;
		private System.Windows.Forms.Panel pnlPoint;
		private Function.form.usrGroupHeader usrGroupHeader1;
		private Function.form.usrInputBox txtBigo;
		private Function.form.usrInputBox txtUpdateType;
		private Function.form.usrInputBox cmbType;
		private Function.form.usrInputBox txtWEBPass;
		private Function.form.usrInputBox txtWEBUrl;
		private Function.form.usrInputBox txtSqlPass;
		private Function.form.usrInputBox txtSqlID;
		private Function.form.usrInputBox txtSqlDB;
		private Function.form.usrInputBox txtSqlIP;
		private Function.form.usrInputBox txtOraPass;
		private Function.form.usrInputBox txtOraID;
		private Function.form.usrInputBox txtOraTNS;
		private System.Windows.Forms.Button btnSave;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.CheckBox chkUseZip;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
		private Function.form.usrInputBox txtPriority;
		private Function.form.usrInputBox cmbGroup;
	}
}