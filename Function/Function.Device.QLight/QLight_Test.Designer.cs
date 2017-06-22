namespace Function.Device.QLight
{
	partial class QLight_Test
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
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.inpIpAddress = new Function.form.usrInputBox();
			this.inpPort = new Function.form.usrInputBox();
			this.picStatus = new System.Windows.Forms.PictureBox();
			this.lblStatus = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnClear = new System.Windows.Forms.Button();
			this.btnAlarm = new System.Windows.Forms.Button();
			this.inpAlarm = new Function.form.usrInputBox();
			this.button13 = new System.Windows.Forms.Button();
			this.button14 = new System.Windows.Forms.Button();
			this.button15 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.button10 = new System.Windows.Forms.Button();
			this.button11 = new System.Windows.Forms.Button();
			this.button12 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.button7 = new System.Windows.Forms.Button();
			this.button8 = new System.Windows.Forms.Button();
			this.button9 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.lblColor = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.btnSettingPgm = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.picStatus)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// inpIpAddress
			// 
			this.inpIpAddress.BackColor = System.Drawing.SystemColors.Control;
			this.inpIpAddress.ChangeMark_Visable = true;
			this.inpIpAddress.ComboBoxDataSource = null;
			this.inpIpAddress.ComboBoxDisplayMember = "";
			this.inpIpAddress.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.inpIpAddress.ComboBoxSelectIndex = -1;
			this.inpIpAddress.ComboBoxSelectItem = null;
			this.inpIpAddress.ComboBoxValueMember = null;
			this.inpIpAddress.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.inpIpAddress.DLabel_Blink = false;
			this.inpIpAddress.DLabel_BlinkColor = System.Drawing.Color.White;
			this.inpIpAddress.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpIpAddress.DLabel_Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.inpIpAddress.DLabel_FontAutoSize = false;
			this.inpIpAddress.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpIpAddress.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpIpAddress.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.inpIpAddress.Label_BackColor = System.Drawing.Color.Transparent;
			this.inpIpAddress.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpIpAddress.Label_Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.inpIpAddress.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpIpAddress.Label_Text = "IP";
			this.inpIpAddress.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.inpIpAddress.Label_Visable = true;
			this.inpIpAddress.LabelWidth = -1;
			this.inpIpAddress.Location = new System.Drawing.Point(6, 8);
			this.inpIpAddress.Multiline = false;
			this.inpIpAddress.Name = "inpIpAddress";
			this.inpIpAddress.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.inpIpAddress.Size = new System.Drawing.Size(166, 23);
			this.inpIpAddress.TabIndex = 0;
			this.inpIpAddress.TEXT = "";
			this.inpIpAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpIpAddress.TextType = Function.form.usrInputBox.enTextType.NumberOlny;
			this.inpIpAddress.Value = "";
			this.inpIpAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inpIpAddress_KeyDown);
			// 
			// inpPort
			// 
			this.inpPort.BackColor = System.Drawing.SystemColors.Control;
			this.inpPort.ChangeMark_Visable = true;
			this.inpPort.ComboBoxDataSource = null;
			this.inpPort.ComboBoxDisplayMember = "";
			this.inpPort.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.inpPort.ComboBoxSelectIndex = -1;
			this.inpPort.ComboBoxSelectItem = null;
			this.inpPort.ComboBoxValueMember = null;
			this.inpPort.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.inpPort.DLabel_Blink = false;
			this.inpPort.DLabel_BlinkColor = System.Drawing.Color.White;
			this.inpPort.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpPort.DLabel_Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.inpPort.DLabel_FontAutoSize = false;
			this.inpPort.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpPort.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpPort.InputType = Function.form.usrInputBox.enInputType.TEXTBOX;
			this.inpPort.Label_BackColor = System.Drawing.Color.Transparent;
			this.inpPort.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpPort.Label_Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.inpPort.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpPort.Label_Text = "Port";
			this.inpPort.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.inpPort.Label_Visable = true;
			this.inpPort.LabelWidth = -1;
			this.inpPort.Location = new System.Drawing.Point(6, 37);
			this.inpPort.Multiline = false;
			this.inpPort.Name = "inpPort";
			this.inpPort.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.inpPort.Size = new System.Drawing.Size(130, 23);
			this.inpPort.TabIndex = 1;
			this.inpPort.TEXT = "";
			this.inpPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpPort.TextType = Function.form.usrInputBox.enTextType.NumberOlny;
			this.inpPort.Value = "";
			this.inpPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inpPort_KeyDown);
			// 
			// picStatus
			// 
			this.picStatus.Location = new System.Drawing.Point(141, 37);
			this.picStatus.Name = "picStatus";
			this.picStatus.Size = new System.Drawing.Size(23, 23);
			this.picStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picStatus.TabIndex = 2;
			this.picStatus.TabStop = false;
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Location = new System.Drawing.Point(167, 42);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(35, 12);
			this.lblStatus.TabIndex = 3;
			this.lblStatus.Text = "None";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.btnClear);
			this.groupBox1.Controls.Add(this.btnAlarm);
			this.groupBox1.Controls.Add(this.inpAlarm);
			this.groupBox1.Controls.Add(this.button13);
			this.groupBox1.Controls.Add(this.button14);
			this.groupBox1.Controls.Add(this.button15);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.button10);
			this.groupBox1.Controls.Add(this.button11);
			this.groupBox1.Controls.Add(this.button12);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.button7);
			this.groupBox1.Controls.Add(this.button8);
			this.groupBox1.Controls.Add(this.button9);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.button4);
			this.groupBox1.Controls.Add(this.button5);
			this.groupBox1.Controls.Add(this.button6);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.button3);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Controls.Add(this.lblColor);
			this.groupBox1.Location = new System.Drawing.Point(6, 99);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(219, 311);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Control";
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(6, 282);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(207, 22);
			this.btnClear.TabIndex = 21;
			this.btnClear.Tag = "6";
			this.btnClear.Text = "명령 초기화";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnAlarm
			// 
			this.btnAlarm.Location = new System.Drawing.Point(172, 257);
			this.btnAlarm.Name = "btnAlarm";
			this.btnAlarm.Size = new System.Drawing.Size(40, 22);
			this.btnAlarm.TabIndex = 20;
			this.btnAlarm.Tag = "6";
			this.btnAlarm.Text = "전송";
			this.btnAlarm.UseVisualStyleBackColor = true;
			this.btnAlarm.Click += new System.EventHandler(this.btnAlarm_Click);
			// 
			// inpAlarm
			// 
			this.inpAlarm.BackColor = System.Drawing.SystemColors.Control;
			this.inpAlarm.ChangeMark_Visable = false;
			this.inpAlarm.ComboBoxDataSource = null;
			this.inpAlarm.ComboBoxDisplayMember = "";
			this.inpAlarm.ComboBoxDropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.inpAlarm.ComboBoxSelectIndex = -1;
			this.inpAlarm.ComboBoxSelectItem = null;
			this.inpAlarm.ComboBoxValueMember = null;
			this.inpAlarm.DLabel_BackColor = System.Drawing.Color.Transparent;
			this.inpAlarm.DLabel_Blink = false;
			this.inpAlarm.DLabel_BlinkColor = System.Drawing.Color.White;
			this.inpAlarm.DLabel_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpAlarm.DLabel_Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.inpAlarm.DLabel_FontAutoSize = false;
			this.inpAlarm.DLabel_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpAlarm.DLabel_TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpAlarm.InputType = Function.form.usrInputBox.enInputType.COMBO;
			this.inpAlarm.Label_BackColor = System.Drawing.Color.Transparent;
			this.inpAlarm.Label_BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.inpAlarm.Label_Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.inpAlarm.Label_ForeColor = System.Drawing.SystemColors.ControlText;
			this.inpAlarm.Label_Text = "Alarm";
			this.inpAlarm.Label_TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.inpAlarm.Label_Visable = true;
			this.inpAlarm.LabelWidth = 50;
			this.inpAlarm.Location = new System.Drawing.Point(6, 257);
			this.inpAlarm.Multiline = false;
			this.inpAlarm.Name = "inpAlarm";
			this.inpAlarm.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.inpAlarm.Size = new System.Drawing.Size(160, 22);
			this.inpAlarm.TabIndex = 5;
			this.inpAlarm.TEXT = "";
			this.inpAlarm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.inpAlarm.TextType = Function.form.usrInputBox.enTextType.All;
			this.inpAlarm.Value = "";
			// 
			// button13
			// 
			this.button13.Location = new System.Drawing.Point(157, 211);
			this.button13.Name = "button13";
			this.button13.Size = new System.Drawing.Size(40, 40);
			this.button13.TabIndex = 19;
			this.button13.Tag = "6";
			this.button13.Text = "OFF";
			this.button13.UseVisualStyleBackColor = true;
			this.button13.Click += new System.EventHandler(this.button13_Click);
			// 
			// button14
			// 
			this.button14.Location = new System.Drawing.Point(116, 211);
			this.button14.Name = "button14";
			this.button14.Size = new System.Drawing.Size(40, 40);
			this.button14.TabIndex = 18;
			this.button14.Tag = "6";
			this.button14.Text = "Blink";
			this.button14.UseVisualStyleBackColor = true;
			this.button14.Click += new System.EventHandler(this.button13_Click);
			// 
			// button15
			// 
			this.button15.Location = new System.Drawing.Point(75, 211);
			this.button15.Name = "button15";
			this.button15.Size = new System.Drawing.Size(40, 40);
			this.button15.TabIndex = 17;
			this.button15.Tag = "6";
			this.button15.Text = "ON";
			this.button15.UseVisualStyleBackColor = true;
			this.button15.Click += new System.EventHandler(this.button13_Click);
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.White;
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label4.Location = new System.Drawing.Point(22, 211);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 40);
			this.label4.TabIndex = 16;
			// 
			// button10
			// 
			this.button10.Location = new System.Drawing.Point(157, 165);
			this.button10.Name = "button10";
			this.button10.Size = new System.Drawing.Size(40, 40);
			this.button10.TabIndex = 15;
			this.button10.Tag = "5";
			this.button10.Text = "OFF";
			this.button10.UseVisualStyleBackColor = true;
			this.button10.Click += new System.EventHandler(this.button13_Click);
			// 
			// button11
			// 
			this.button11.Location = new System.Drawing.Point(116, 165);
			this.button11.Name = "button11";
			this.button11.Size = new System.Drawing.Size(40, 40);
			this.button11.TabIndex = 14;
			this.button11.Tag = "5";
			this.button11.Text = "Blink";
			this.button11.UseVisualStyleBackColor = true;
			this.button11.Click += new System.EventHandler(this.button13_Click);
			// 
			// button12
			// 
			this.button12.Location = new System.Drawing.Point(75, 165);
			this.button12.Name = "button12";
			this.button12.Size = new System.Drawing.Size(40, 40);
			this.button12.TabIndex = 13;
			this.button12.Tag = "5";
			this.button12.Text = "ON";
			this.button12.UseVisualStyleBackColor = true;
			this.button12.Click += new System.EventHandler(this.button13_Click);
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Blue;
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label3.Location = new System.Drawing.Point(22, 165);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 40);
			this.label3.TabIndex = 12;
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(157, 119);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(40, 40);
			this.button7.TabIndex = 11;
			this.button7.Tag = "4";
			this.button7.Text = "OFF";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new System.EventHandler(this.button13_Click);
			// 
			// button8
			// 
			this.button8.Location = new System.Drawing.Point(116, 119);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(40, 40);
			this.button8.TabIndex = 10;
			this.button8.Tag = "4";
			this.button8.Text = "Blink";
			this.button8.UseVisualStyleBackColor = true;
			this.button8.Click += new System.EventHandler(this.button13_Click);
			// 
			// button9
			// 
			this.button9.Location = new System.Drawing.Point(75, 119);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(40, 40);
			this.button9.TabIndex = 9;
			this.button9.Tag = "4";
			this.button9.Text = "ON";
			this.button9.UseVisualStyleBackColor = true;
			this.button9.Click += new System.EventHandler(this.button13_Click);
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Green;
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label2.Location = new System.Drawing.Point(22, 119);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 40);
			this.label2.TabIndex = 8;
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(156, 73);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(40, 40);
			this.button4.TabIndex = 7;
			this.button4.Tag = "3";
			this.button4.Text = "OFF";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button13_Click);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(115, 73);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(40, 40);
			this.button5.TabIndex = 6;
			this.button5.Tag = "3";
			this.button5.Text = "Blink";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.button13_Click);
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(74, 73);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(40, 40);
			this.button6.TabIndex = 5;
			this.button6.Tag = "3";
			this.button6.Text = "ON";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.button13_Click);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Yellow;
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Location = new System.Drawing.Point(21, 73);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 40);
			this.label1.TabIndex = 4;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(156, 27);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(40, 40);
			this.button3.TabIndex = 3;
			this.button3.Tag = "2";
			this.button3.Text = "OFF";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button13_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(115, 27);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(40, 40);
			this.button2.TabIndex = 2;
			this.button2.Tag = "2";
			this.button2.Text = "Blink";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button13_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(74, 27);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(40, 40);
			this.button1.TabIndex = 1;
			this.button1.Tag = "2";
			this.button1.Text = "ON";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button13_Click);
			// 
			// lblColor
			// 
			this.lblColor.BackColor = System.Drawing.Color.Red;
			this.lblColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblColor.Location = new System.Drawing.Point(21, 27);
			this.lblColor.Name = "lblColor";
			this.lblColor.Size = new System.Drawing.Size(40, 40);
			this.lblColor.TabIndex = 0;
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Red;
			this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label5.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label5.Location = new System.Drawing.Point(9, 63);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(30, 30);
			this.label5.TabIndex = 22;
			this.label5.Text = "R";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.Yellow;
			this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label6.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label6.Location = new System.Drawing.Point(45, 63);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(30, 30);
			this.label6.TabIndex = 23;
			this.label6.Text = "Y";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.Color.Green;
			this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label7.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label7.Location = new System.Drawing.Point(81, 63);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(30, 30);
			this.label7.TabIndex = 24;
			this.label7.Text = "G";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label8
			// 
			this.label8.BackColor = System.Drawing.Color.Blue;
			this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label8.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label8.Location = new System.Drawing.Point(117, 63);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(30, 30);
			this.label8.TabIndex = 25;
			this.label8.Text = "B";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.White;
			this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label9.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label9.Location = new System.Drawing.Point(153, 63);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(30, 30);
			this.label9.TabIndex = 26;
			this.label9.Text = "W";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label10
			// 
			this.label10.BackColor = System.Drawing.Color.CadetBlue;
			this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label10.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label10.Location = new System.Drawing.Point(188, 63);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(30, 30);
			this.label10.TabIndex = 27;
			this.label10.Text = "A";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnSettingPgm
			// 
			this.btnSettingPgm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSettingPgm.Location = new System.Drawing.Point(209, 8);
			this.btnSettingPgm.Name = "btnSettingPgm";
			this.btnSettingPgm.Size = new System.Drawing.Size(24, 23);
			this.btnSettingPgm.TabIndex = 28;
			this.toolTip1.SetToolTip(this.btnSettingPgm, "경광등 설정 프로그램 실행");
			this.btnSettingPgm.UseVisualStyleBackColor = true;
			this.btnSettingPgm.Click += new System.EventHandler(this.btnSettingPgm_Click);
			// 
			// QLight_Test
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(238, 413);
			this.Controls.Add(this.btnSettingPgm);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.picStatus);
			this.Controls.Add(this.inpPort);
			this.Controls.Add(this.inpIpAddress);
			this.Name = "QLight_Test";
			this.ShowInTaskbar = false;
			this.Text = "QLight Test Form";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.QLight_Test_FormClosed);
			this.Load += new System.EventHandler(this.QLight_Test_Load);
			((System.ComponentModel.ISupportInitialize)(this.picStatus)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Function.form.usrInputBox inpIpAddress;
		private Function.form.usrInputBox inpPort;
		private System.Windows.Forms.PictureBox picStatus;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button button13;
		private System.Windows.Forms.Button button14;
		private System.Windows.Forms.Button button15;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button button10;
		private System.Windows.Forms.Button button11;
		private System.Windows.Forms.Button button12;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label lblColor;
		private Function.form.usrInputBox inpAlarm;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Button btnAlarm;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Button btnSettingPgm;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}

