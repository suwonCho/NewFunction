namespace Function.form.Db
{
	partial class DBSetting
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
			this.inpDbType = new Function.form.usrInputBox();
			this.inpIp = new Function.form.usrInputBox();
			this.inpId = new Function.form.usrInputBox();
			this.inpPass = new Function.form.usrInputBox();
			this.inpDataBase = new Function.form.usrInputBox();
			this.inpAuthType = new Function.form.usrInputBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// inpDbType
			// 
			this.inpDbType.BackColor = System.Drawing.SystemColors.Control;
			this.inpDbType.ChangeMark_Visable = true;
			this.inpDbType.ComboBoxDataSource = null;
			this.inpDbType.ComboBoxDisplayMember = "";
			this.inpDbType.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.inpDbType.ComboBoxItems.AddRange(new object[] {
            "MS-SQL",
            "ORACLE"});
			this.inpDbType.ComboBoxSelectedValue = null;
			this.inpDbType.ComboBoxSelectIndex = -1;
			this.inpDbType.ComboBoxSelectItem = null;
			this.inpDbType.ComboBoxValueMember = "";
			this.inpDbType.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.inpDbType.DLabel_Blink = Function.form.usrInputBox.enBlinkType.None;
			this.inpDbType.DLabel_BlinkColor = System.Drawing.Color.White;
			this.inpDbType.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpDbType.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.inpDbType.DLabel_FontAutoSize = false;
			this.inpDbType.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpDbType.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpDbType.InputType = Function.form.usrInputBox.enInputType.COMBO;
			this.inpDbType.Label_BackColor = System.Drawing.Color.Transparent;
			this.inpDbType.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpDbType.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.inpDbType.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpDbType.Label_Text = "DB타입";
			this.inpDbType.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.inpDbType.Label_Visable = true;
			this.inpDbType.LabelWidth = 50;
			this.inpDbType.Location = new System.Drawing.Point(5, 9);
			this.inpDbType.Multiline = false;
			this.inpDbType.Name = "inpDbType";
			this.inpDbType.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.inpDbType.Size = new System.Drawing.Size(168, 22);
			this.inpDbType.TabIndex = 1;
			this.inpDbType.Text = "";
			this.inpDbType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpDbType.TextBox_AcceptsTab = false;
			this.inpDbType.TextBox_PasswordChar = '\0';
			this.inpDbType.TextBox_TabStopsLength = 8;
			this.inpDbType.TextType = Function.form.usrInputBox.enTextType.All;
			this.inpDbType.Value = "";
			this.inpDbType.Text_Changed += new Function.form.usrEventHander(this.inpDbType_Text_Changed);
			// 
			// inpIp
			// 
			this.inpIp.BackColor = System.Drawing.SystemColors.Control;
			this.inpIp.ChangeMark_Visable = true;
			this.inpIp.ComboBoxDataSource = null;
			this.inpIp.ComboBoxDisplayMember = "";
			this.inpIp.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.inpIp.ComboBoxItems.AddRange(new object[] {
            "MS-SQL",
            "ORACLE"});
			this.inpIp.ComboBoxSelectedValue = null;
			this.inpIp.ComboBoxSelectIndex = -1;
			this.inpIp.ComboBoxSelectItem = null;
			this.inpIp.ComboBoxValueMember = "";
			this.inpIp.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.inpIp.DLabel_Blink = Function.form.usrInputBox.enBlinkType.None;
			this.inpIp.DLabel_BlinkColor = System.Drawing.Color.White;
			this.inpIp.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpIp.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.inpIp.DLabel_FontAutoSize = false;
			this.inpIp.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpIp.DLabel_TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.inpIp.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.inpIp.Label_BackColor = System.Drawing.Color.Transparent;
			this.inpIp.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpIp.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.inpIp.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpIp.Label_Text = "IP";
			this.inpIp.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.inpIp.Label_Visable = true;
			this.inpIp.LabelWidth = 50;
			this.inpIp.Location = new System.Drawing.Point(5, 37);
			this.inpIp.Multiline = true;
			this.inpIp.Name = "inpIp";
			this.inpIp.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.inpIp.Size = new System.Drawing.Size(341, 128);
			this.inpIp.TabIndex = 2;
			this.inpIp.Text = "";
			this.inpIp.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.inpIp.TextBox_AcceptsTab = false;
			this.inpIp.TextBox_PasswordChar = '\0';
			this.inpIp.TextBox_TabStopsLength = 8;
			this.inpIp.TextType = Function.form.usrInputBox.enTextType.All;
			this.inpIp.Value = "";
			// 
			// inpId
			// 
			this.inpId.BackColor = System.Drawing.SystemColors.Control;
			this.inpId.ChangeMark_Visable = true;
			this.inpId.ComboBoxDataSource = null;
			this.inpId.ComboBoxDisplayMember = "";
			this.inpId.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.inpId.ComboBoxItems.AddRange(new object[] {
            "MS-SQL",
            "ORACLE"});
			this.inpId.ComboBoxSelectedValue = null;
			this.inpId.ComboBoxSelectIndex = -1;
			this.inpId.ComboBoxSelectItem = null;
			this.inpId.ComboBoxValueMember = "";
			this.inpId.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.inpId.DLabel_Blink = Function.form.usrInputBox.enBlinkType.None;
			this.inpId.DLabel_BlinkColor = System.Drawing.Color.White;
			this.inpId.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpId.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.inpId.DLabel_FontAutoSize = false;
			this.inpId.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpId.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpId.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.inpId.Label_BackColor = System.Drawing.Color.Transparent;
			this.inpId.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpId.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.inpId.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpId.Label_Text = "ID";
			this.inpId.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.inpId.Label_Visable = true;
			this.inpId.LabelWidth = 50;
			this.inpId.Location = new System.Drawing.Point(3, 195);
			this.inpId.Multiline = false;
			this.inpId.Name = "inpId";
			this.inpId.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.inpId.Size = new System.Drawing.Size(178, 23);
			this.inpId.TabIndex = 3;
			this.inpId.Text = "";
			this.inpId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpId.TextBox_AcceptsTab = false;
			this.inpId.TextBox_PasswordChar = '*';
			this.inpId.TextBox_TabStopsLength = 8;
			this.inpId.TextType = Function.form.usrInputBox.enTextType.All;
			this.inpId.Value = "";
			// 
			// inpPass
			// 
			this.inpPass.BackColor = System.Drawing.SystemColors.Control;
			this.inpPass.ChangeMark_Visable = true;
			this.inpPass.ComboBoxDataSource = null;
			this.inpPass.ComboBoxDisplayMember = "";
			this.inpPass.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.inpPass.ComboBoxItems.AddRange(new object[] {
            "MS-SQL",
            "ORACLE"});
			this.inpPass.ComboBoxSelectedValue = null;
			this.inpPass.ComboBoxSelectIndex = -1;
			this.inpPass.ComboBoxSelectItem = null;
			this.inpPass.ComboBoxValueMember = "";
			this.inpPass.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.inpPass.DLabel_Blink = Function.form.usrInputBox.enBlinkType.None;
			this.inpPass.DLabel_BlinkColor = System.Drawing.Color.White;
			this.inpPass.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpPass.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.inpPass.DLabel_FontAutoSize = false;
			this.inpPass.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpPass.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpPass.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.inpPass.Label_BackColor = System.Drawing.Color.Transparent;
			this.inpPass.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpPass.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.inpPass.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpPass.Label_Text = "암호";
			this.inpPass.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.inpPass.Label_Visable = true;
			this.inpPass.LabelWidth = 50;
			this.inpPass.Location = new System.Drawing.Point(176, 195);
			this.inpPass.Multiline = false;
			this.inpPass.Name = "inpPass";
			this.inpPass.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.inpPass.Size = new System.Drawing.Size(168, 23);
			this.inpPass.TabIndex = 4;
			this.inpPass.Text = "";
			this.inpPass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpPass.TextBox_AcceptsTab = false;
			this.inpPass.TextBox_PasswordChar = '*';
			this.inpPass.TextBox_TabStopsLength = 8;
			this.inpPass.TextType = Function.form.usrInputBox.enTextType.All;
			this.inpPass.Value = "";
			// 
			// inpDataBase
			// 
			this.inpDataBase.BackColor = System.Drawing.SystemColors.Control;
			this.inpDataBase.ChangeMark_Visable = true;
			this.inpDataBase.ComboBoxDataSource = null;
			this.inpDataBase.ComboBoxDisplayMember = "";
			this.inpDataBase.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.inpDataBase.ComboBoxItems.AddRange(new object[] {
            "MS-SQL",
            "ORACLE"});
			this.inpDataBase.ComboBoxSelectedValue = null;
			this.inpDataBase.ComboBoxSelectIndex = -1;
			this.inpDataBase.ComboBoxSelectItem = null;
			this.inpDataBase.ComboBoxValueMember = "";
			this.inpDataBase.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.inpDataBase.DLabel_Blink = Function.form.usrInputBox.enBlinkType.None;
			this.inpDataBase.DLabel_BlinkColor = System.Drawing.Color.White;
			this.inpDataBase.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpDataBase.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.inpDataBase.DLabel_FontAutoSize = false;
			this.inpDataBase.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpDataBase.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpDataBase.InputType = Function.form.usrInputBox.enInputType.COMBO;
			this.inpDataBase.Label_BackColor = System.Drawing.Color.Transparent;
			this.inpDataBase.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpDataBase.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.inpDataBase.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpDataBase.Label_Text = "DB선택";
			this.inpDataBase.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.inpDataBase.Label_Visable = true;
			this.inpDataBase.LabelWidth = 50;
			this.inpDataBase.Location = new System.Drawing.Point(3, 223);
			this.inpDataBase.Multiline = false;
			this.inpDataBase.Name = "inpDataBase";
			this.inpDataBase.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.inpDataBase.Size = new System.Drawing.Size(341, 22);
			this.inpDataBase.TabIndex = 5;
			this.inpDataBase.Text = "";
			this.inpDataBase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpDataBase.TextBox_AcceptsTab = false;
			this.inpDataBase.TextBox_PasswordChar = '\0';
			this.inpDataBase.TextBox_TabStopsLength = 8;
			this.inpDataBase.TextType = Function.form.usrInputBox.enTextType.All;
			this.inpDataBase.Value = "";
			this.inpDataBase.ComboBoxDropDown += new System.EventHandler(this.inpDataBase_ComboBoxDropDown);
			// 
			// inpAuthType
			// 
			this.inpAuthType.BackColor = System.Drawing.SystemColors.Control;
			this.inpAuthType.ChangeMark_Visable = true;
			this.inpAuthType.ComboBoxDataSource = null;
			this.inpAuthType.ComboBoxDisplayMember = "";
			this.inpAuthType.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.inpAuthType.ComboBoxItems.AddRange(new object[] {
            "Sql Server 인증",
            "Windows 인증"});
			this.inpAuthType.ComboBoxSelectedValue = null;
			this.inpAuthType.ComboBoxSelectIndex = -1;
			this.inpAuthType.ComboBoxSelectItem = null;
			this.inpAuthType.ComboBoxValueMember = "";
			this.inpAuthType.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.inpAuthType.DLabel_Blink = Function.form.usrInputBox.enBlinkType.None;
			this.inpAuthType.DLabel_BlinkColor = System.Drawing.Color.White;
			this.inpAuthType.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpAuthType.DLabel_Font = new System.Drawing.Font("굴림체", 9F);
			this.inpAuthType.DLabel_FontAutoSize = false;
			this.inpAuthType.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpAuthType.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpAuthType.InputType = Function.form.usrInputBox.enInputType.COMBO;
			this.inpAuthType.Label_BackColor = System.Drawing.Color.Transparent;
			this.inpAuthType.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpAuthType.Label_Font = new System.Drawing.Font("굴림체", 9F);
			this.inpAuthType.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpAuthType.Label_Text = "Sql인증";
			this.inpAuthType.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.inpAuthType.Label_Visable = true;
			this.inpAuthType.LabelWidth = 50;
			this.inpAuthType.Location = new System.Drawing.Point(5, 169);
			this.inpAuthType.Multiline = false;
			this.inpAuthType.Name = "inpAuthType";
			this.inpAuthType.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.inpAuthType.Size = new System.Drawing.Size(176, 22);
			this.inpAuthType.TabIndex = 6;
			this.inpAuthType.Text = "";
			this.inpAuthType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpAuthType.TextBox_AcceptsTab = false;
			this.inpAuthType.TextBox_PasswordChar = '\0';
			this.inpAuthType.TextBox_TabStopsLength = 8;
			this.inpAuthType.TextType = Function.form.usrInputBox.enTextType.All;
			this.inpAuthType.Value = "";
			this.inpAuthType.Text_Changed += new Function.form.usrEventHander(this.inpAuthType_Text_Changed);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(180, 246);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "  저 장";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(261, 246);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "  취 소";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// DBSetting
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BaseFormStyle = Function.form.frmBaseForm.enBaseFormStyle.toolbox;
			this.ClientSize = new System.Drawing.Size(344, 296);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.inpAuthType);
			this.Controls.Add(this.inpDataBase);
			this.Controls.Add(this.inpPass);
			this.Controls.Add(this.inpId);
			this.Controls.Add(this.inpIp);
			this.Controls.Add(this.inpDbType);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "DBSetting";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.ShowStatusBar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "데이터 베이스 설정";
			this.Load += new System.EventHandler(this.DBSetting_Load);
			this.Controls.SetChildIndex(this.inpDbType, 0);
			this.Controls.SetChildIndex(this.inpIp, 0);
			this.Controls.SetChildIndex(this.inpId, 0);
			this.Controls.SetChildIndex(this.inpPass, 0);
			this.Controls.SetChildIndex(this.inpDataBase, 0);
			this.Controls.SetChildIndex(this.inpAuthType, 0);
			this.Controls.SetChildIndex(this.btnSave, 0);
			this.Controls.SetChildIndex(this.btnCancel, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private usrInputBox inpDbType;
		private usrInputBox inpIp;
		private usrInputBox inpId;
		private usrInputBox inpPass;
		private usrInputBox inpDataBase;
		private usrInputBox inpAuthType;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
	}
}