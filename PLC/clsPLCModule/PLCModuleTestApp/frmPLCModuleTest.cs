using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using PLCModule;

using Function;
using System.Collections.Generic;
using System.Threading;

namespace PLCModuleTestApp
{
	/// <summary>
	/// Form1에 대한 요약 설명입니다.
	/// </summary>
	public class frmTest : System.Windows.Forms.Form
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnOpen;
		private System.Windows.Forms.Button btnClose;
		
		string strMsg = string.Empty;
		private System.Windows.Forms.DataGrid Grid;
		private System.Windows.Forms.Label lblPLCType;
		private System.Windows.Forms.ComboBox cmbPLCType;
		private System.Windows.Forms.GroupBox grpMelsec;
		private System.Windows.Forms.GroupBox grpAB;
		private System.Windows.Forms.Label lblIP;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox txtDeviceType;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtTopic;
		private System.Windows.Forms.Label lblTopic;
		private System.Windows.Forms.TextBox txtNODE;
		private System.Windows.Forms.Label lblNode;
		private System.Windows.Forms.TextBox txtProgID;
		private System.Windows.Forms.Label lblProgID;
		private System.Windows.Forms.TextBox txtIPAdd;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox txtItem;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Button button2;
		private Button btnMonitor;
		private GroupBox groupBox1;
		private Label label3;
		private TextBox txtTestAddress;
		private Label label4;
		private Label label6;
		private NumericUpDown txtTestInterval;
		private Label label5;
		private Label lblTestMsg;
		private Button btnTestStart;
		private NumericUpDown txtTestTo;
		private NumericUpDown txtTestFrom;
		private Label label7;


		clsPLCModule Comm;
		private GroupBox groupBox2;
		private Button btnMtAdd;
		private Label txtMtMsg;
		private Button btnMtEnd;
		private Label label10;
		private NumericUpDown txtMtInterval;
		private Label label11;
		System.Threading.Timer tmrTest = null;

		public frmTest()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			clsPLCModule.Auth.Add("MtWwV7Gl5+2yv1P4ZSxd0gFCTLuJMZpK0+40l18VHWk=");

			//
			// TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
			//
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnOpen = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.lblStatus = new System.Windows.Forms.Label();
			this.Grid = new System.Windows.Forms.DataGrid();
			this.lblPLCType = new System.Windows.Forms.Label();
			this.cmbPLCType = new System.Windows.Forms.ComboBox();
			this.grpMelsec = new System.Windows.Forms.GroupBox();
			this.txtDeviceType = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.txtIPAdd = new System.Windows.Forms.TextBox();
			this.lblIP = new System.Windows.Forms.Label();
			this.grpAB = new System.Windows.Forms.GroupBox();
			this.txtTopic = new System.Windows.Forms.TextBox();
			this.lblTopic = new System.Windows.Forms.Label();
			this.txtNODE = new System.Windows.Forms.TextBox();
			this.lblNode = new System.Windows.Forms.Label();
			this.txtProgID = new System.Windows.Forms.TextBox();
			this.lblProgID = new System.Windows.Forms.Label();
			this.txtItem = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.btnMonitor = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblTestMsg = new System.Windows.Forms.Label();
			this.btnTestStart = new System.Windows.Forms.Button();
			this.txtTestTo = new System.Windows.Forms.NumericUpDown();
			this.txtTestFrom = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtTestInterval = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtTestAddress = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnMtAdd = new System.Windows.Forms.Button();
			this.txtMtMsg = new System.Windows.Forms.Label();
			this.btnMtEnd = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.txtMtInterval = new System.Windows.Forms.NumericUpDown();
			this.label11 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
			this.grpMelsec.SuspendLayout();
			this.grpAB.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtTestTo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtTestFrom)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtTestInterval)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtMtInterval)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOpen
			// 
			this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOpen.Location = new System.Drawing.Point(8, 436);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(72, 32);
			this.btnOpen.TabIndex = 1;
			this.btnOpen.Text = "Open";
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnClose.Location = new System.Drawing.Point(88, 436);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(72, 32);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// lblStatus
			// 
			this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblStatus.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblStatus.Location = new System.Drawing.Point(288, 444);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(80, 24);
			this.lblStatus.TabIndex = 2;
			this.lblStatus.Text = "Close";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Grid
			// 
			this.Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Grid.DataMember = "";
			this.Grid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.Grid.Location = new System.Drawing.Point(8, 40);
			this.Grid.Name = "Grid";
			this.Grid.Size = new System.Drawing.Size(360, 359);
			this.Grid.TabIndex = 4;
			// 
			// lblPLCType
			// 
			this.lblPLCType.Location = new System.Drawing.Point(8, 8);
			this.lblPLCType.Name = "lblPLCType";
			this.lblPLCType.Size = new System.Drawing.Size(64, 24);
			this.lblPLCType.TabIndex = 5;
			this.lblPLCType.Text = "PLC Type";
			this.lblPLCType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmbPLCType
			// 
			this.cmbPLCType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPLCType.Location = new System.Drawing.Point(80, 8);
			this.cmbPLCType.Name = "cmbPLCType";
			this.cmbPLCType.Size = new System.Drawing.Size(112, 20);
			this.cmbPLCType.TabIndex = 6;
			this.cmbPLCType.SelectedIndexChanged += new System.EventHandler(this.cmbPLCType_SelectedIndexChanged);
			// 
			// grpMelsec
			// 
			this.grpMelsec.Controls.Add(this.txtDeviceType);
			this.grpMelsec.Controls.Add(this.label1);
			this.grpMelsec.Controls.Add(this.txtPort);
			this.grpMelsec.Controls.Add(this.lblPort);
			this.grpMelsec.Controls.Add(this.txtIPAdd);
			this.grpMelsec.Controls.Add(this.lblIP);
			this.grpMelsec.Location = new System.Drawing.Point(384, 16);
			this.grpMelsec.Name = "grpMelsec";
			this.grpMelsec.Size = new System.Drawing.Size(432, 128);
			this.grpMelsec.TabIndex = 8;
			this.grpMelsec.TabStop = false;
			this.grpMelsec.Text = "Mesle A/Q Setting";
			// 
			// txtDeviceType
			// 
			this.txtDeviceType.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 10F);
			this.txtDeviceType.Location = new System.Drawing.Point(104, 88);
			this.txtDeviceType.MaxLength = 1;
			this.txtDeviceType.Name = "txtDeviceType";
			this.txtDeviceType.Size = new System.Drawing.Size(56, 23);
			this.txtDeviceType.TabIndex = 5;
			this.txtDeviceType.Text = "D";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label1.Location = new System.Drawing.Point(16, 88);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 24);
			this.label1.TabIndex = 4;
			this.label1.Text = "DeviceType";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtPort
			// 
			this.txtPort.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 10F);
			this.txtPort.Location = new System.Drawing.Point(104, 56);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(208, 23);
			this.txtPort.TabIndex = 3;
			this.txtPort.Text = "7700";
			// 
			// lblPort
			// 
			this.lblPort.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblPort.Location = new System.Drawing.Point(16, 56);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(80, 24);
			this.lblPort.TabIndex = 2;
			this.lblPort.Text = "Port";
			this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtIPAdd
			// 
			this.txtIPAdd.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 10F);
			this.txtIPAdd.Location = new System.Drawing.Point(104, 24);
			this.txtIPAdd.Name = "txtIPAdd";
			this.txtIPAdd.Size = new System.Drawing.Size(208, 23);
			this.txtIPAdd.TabIndex = 1;
			this.txtIPAdd.Text = "192.168.0.169";
			this.txtIPAdd.TextChanged += new System.EventHandler(this.txtIPAdd_TextChanged);
			// 
			// lblIP
			// 
			this.lblIP.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblIP.Location = new System.Drawing.Point(16, 24);
			this.lblIP.Name = "lblIP";
			this.lblIP.Size = new System.Drawing.Size(80, 24);
			this.lblIP.TabIndex = 0;
			this.lblIP.Text = "IPAddress";
			this.lblIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// grpAB
			// 
			this.grpAB.Controls.Add(this.txtTopic);
			this.grpAB.Controls.Add(this.lblTopic);
			this.grpAB.Controls.Add(this.txtNODE);
			this.grpAB.Controls.Add(this.lblNode);
			this.grpAB.Controls.Add(this.txtProgID);
			this.grpAB.Controls.Add(this.lblProgID);
			this.grpAB.Location = new System.Drawing.Point(384, 152);
			this.grpAB.Name = "grpAB";
			this.grpAB.Size = new System.Drawing.Size(432, 128);
			this.grpAB.TabIndex = 9;
			this.grpAB.TabStop = false;
			this.grpAB.Text = "AB Setting";
			// 
			// txtTopic
			// 
			this.txtTopic.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 10F);
			this.txtTopic.Location = new System.Drawing.Point(104, 88);
			this.txtTopic.MaxLength = 100;
			this.txtTopic.Name = "txtTopic";
			this.txtTopic.Size = new System.Drawing.Size(136, 23);
			this.txtTopic.TabIndex = 11;
			this.txtTopic.Text = "AA010Cell";
			// 
			// lblTopic
			// 
			this.lblTopic.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblTopic.Location = new System.Drawing.Point(16, 88);
			this.lblTopic.Name = "lblTopic";
			this.lblTopic.Size = new System.Drawing.Size(80, 24);
			this.lblTopic.TabIndex = 10;
			this.lblTopic.Text = "Topic";
			this.lblTopic.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtNODE
			// 
			this.txtNODE.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 10F);
			this.txtNODE.Location = new System.Drawing.Point(104, 56);
			this.txtNODE.Name = "txtNODE";
			this.txtNODE.Size = new System.Drawing.Size(208, 23);
			this.txtNODE.TabIndex = 9;
			// 
			// lblNode
			// 
			this.lblNode.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblNode.Location = new System.Drawing.Point(16, 56);
			this.lblNode.Name = "lblNode";
			this.lblNode.Size = new System.Drawing.Size(80, 24);
			this.lblNode.TabIndex = 8;
			this.lblNode.Text = "NODE";
			this.lblNode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtProgID
			// 
			this.txtProgID.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 10F);
			this.txtProgID.Location = new System.Drawing.Point(104, 24);
			this.txtProgID.Name = "txtProgID";
			this.txtProgID.Size = new System.Drawing.Size(208, 23);
			this.txtProgID.TabIndex = 7;
			this.txtProgID.Text = "RSLinx OPC Server";
			// 
			// lblProgID
			// 
			this.lblProgID.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblProgID.Location = new System.Drawing.Point(16, 24);
			this.lblProgID.Name = "lblProgID";
			this.lblProgID.Size = new System.Drawing.Size(80, 24);
			this.lblProgID.TabIndex = 6;
			this.lblProgID.Text = "ProgID";
			this.lblProgID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtItem
			// 
			this.txtItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.txtItem.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 10F);
			this.txtItem.Location = new System.Drawing.Point(96, 407);
			this.txtItem.MaxLength = 100;
			this.txtItem.Name = "txtItem";
			this.txtItem.Size = new System.Drawing.Size(186, 23);
			this.txtItem.TabIndex = 13;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label2.Location = new System.Drawing.Point(8, 407);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 24);
			this.label2.TabIndex = 12;
			this.label2.Text = "AddItem";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Location = new System.Drawing.Point(288, 406);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(80, 24);
			this.button1.TabIndex = 14;
			this.button1.Text = "Add Item";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button2.Location = new System.Drawing.Point(168, 436);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(72, 32);
			this.button2.TabIndex = 15;
			this.button2.Text = "Write";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// btnMonitor
			// 
			this.btnMonitor.Location = new System.Drawing.Point(288, 5);
			this.btnMonitor.Name = "btnMonitor";
			this.btnMonitor.Size = new System.Drawing.Size(80, 24);
			this.btnMonitor.TabIndex = 16;
			this.btnMonitor.Text = "Monitor";
			this.btnMonitor.Click += new System.EventHandler(this.btnMonitor_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblTestMsg);
			this.groupBox1.Controls.Add(this.btnTestStart);
			this.groupBox1.Controls.Add(this.txtTestTo);
			this.groupBox1.Controls.Add(this.txtTestFrom);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.txtTestInterval);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtTestAddress);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Location = new System.Drawing.Point(384, 286);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(432, 107);
			this.groupBox1.TabIndex = 17;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Test";
			// 
			// lblTestMsg
			// 
			this.lblTestMsg.BackColor = System.Drawing.Color.White;
			this.lblTestMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblTestMsg.Location = new System.Drawing.Point(17, 77);
			this.lblTestMsg.Name = "lblTestMsg";
			this.lblTestMsg.Size = new System.Drawing.Size(409, 23);
			this.lblTestMsg.TabIndex = 16;
			this.lblTestMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnTestStart
			// 
			this.btnTestStart.Location = new System.Drawing.Point(341, 44);
			this.btnTestStart.Name = "btnTestStart";
			this.btnTestStart.Size = new System.Drawing.Size(80, 24);
			this.btnTestStart.TabIndex = 15;
			this.btnTestStart.Text = "시 작";
			this.btnTestStart.Click += new System.EventHandler(this.btnTestStart_Click);
			// 
			// txtTestTo
			// 
			this.txtTestTo.Location = new System.Drawing.Point(192, 46);
			this.txtTestTo.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.txtTestTo.Name = "txtTestTo";
			this.txtTestTo.Size = new System.Drawing.Size(76, 21);
			this.txtTestTo.TabIndex = 10;
			this.txtTestTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtTestTo.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			// 
			// txtTestFrom
			// 
			this.txtTestFrom.Location = new System.Drawing.Point(104, 46);
			this.txtTestFrom.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.txtTestFrom.Name = "txtTestFrom";
			this.txtTestFrom.Size = new System.Drawing.Size(76, 21);
			this.txtTestFrom.TabIndex = 9;
			this.txtTestFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label7.Location = new System.Drawing.Point(178, 45);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(18, 24);
			this.label7.TabIndex = 11;
			this.label7.Text = "~";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label6.Location = new System.Drawing.Point(395, 19);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(26, 24);
			this.label6.TabIndex = 8;
			this.label6.Text = "ms";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtTestInterval
			// 
			this.txtTestInterval.Location = new System.Drawing.Point(317, 19);
			this.txtTestInterval.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.txtTestInterval.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.txtTestInterval.Name = "txtTestInterval";
			this.txtTestInterval.Size = new System.Drawing.Size(76, 21);
			this.txtTestInterval.TabIndex = 7;
			this.txtTestInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtTestInterval.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label5.Location = new System.Drawing.Point(246, 17);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 24);
			this.label5.TabIndex = 6;
			this.label5.Text = "Interval";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label3.Location = new System.Drawing.Point(16, 46);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 24);
			this.label3.TabIndex = 4;
			this.label3.Text = "Num Range";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtTestAddress
			// 
			this.txtTestAddress.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 10F);
			this.txtTestAddress.Location = new System.Drawing.Point(104, 17);
			this.txtTestAddress.Name = "txtTestAddress";
			this.txtTestAddress.Size = new System.Drawing.Size(136, 23);
			this.txtTestAddress.TabIndex = 3;
			this.txtTestAddress.Text = "900";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label4.Location = new System.Drawing.Point(16, 17);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 24);
			this.label4.TabIndex = 2;
			this.label4.Text = "Address";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.btnMtAdd);
			this.groupBox2.Controls.Add(this.txtMtMsg);
			this.groupBox2.Controls.Add(this.btnMtEnd);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.txtMtInterval);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Location = new System.Drawing.Point(384, 399);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(432, 75);
			this.groupBox2.TabIndex = 18;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Multi Thread Read/Write Test";
			// 
			// btnMtAdd
			// 
			this.btnMtAdd.Location = new System.Drawing.Point(266, 16);
			this.btnMtAdd.Name = "btnMtAdd";
			this.btnMtAdd.Size = new System.Drawing.Size(80, 24);
			this.btnMtAdd.TabIndex = 17;
			this.btnMtAdd.Text = "10개추가";
			this.btnMtAdd.Click += new System.EventHandler(this.btnMtAdd_Click);
			// 
			// txtMtMsg
			// 
			this.txtMtMsg.BackColor = System.Drawing.Color.White;
			this.txtMtMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtMtMsg.Location = new System.Drawing.Point(17, 43);
			this.txtMtMsg.Name = "txtMtMsg";
			this.txtMtMsg.Size = new System.Drawing.Size(409, 23);
			this.txtMtMsg.TabIndex = 16;
			this.txtMtMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnMtEnd
			// 
			this.btnMtEnd.Location = new System.Drawing.Point(348, 16);
			this.btnMtEnd.Name = "btnMtEnd";
			this.btnMtEnd.Size = new System.Drawing.Size(80, 24);
			this.btnMtEnd.TabIndex = 15;
			this.btnMtEnd.Text = "전체종료";
			this.btnMtEnd.Click += new System.EventHandler(this.btnMtEnd_Click);
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label10.Location = new System.Drawing.Point(156, 19);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(26, 24);
			this.label10.TabIndex = 8;
			this.label10.Text = "ms";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtMtInterval
			// 
			this.txtMtInterval.Location = new System.Drawing.Point(78, 19);
			this.txtMtInterval.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.txtMtInterval.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.txtMtInterval.Name = "txtMtInterval";
			this.txtMtInterval.Size = new System.Drawing.Size(76, 21);
			this.txtMtInterval.TabIndex = 7;
			this.txtMtInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtMtInterval.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label11.Location = new System.Drawing.Point(7, 17);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(80, 24);
			this.label11.TabIndex = 6;
			this.label11.Text = "Interval";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// frmTest
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(824, 478);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnMonitor);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.txtItem);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.grpAB);
			this.Controls.Add(this.grpMelsec);
			this.Controls.Add(this.cmbPLCType);
			this.Controls.Add(this.lblPLCType);
			this.Controls.Add(this.Grid);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.btnOpen);
			this.Controls.Add(this.btnClose);
			this.Name = "frmTest";
			this.Text = "PLC Communication Test";
			this.Load += new System.EventHandler(this.frmTest_Load);
			((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
			this.grpMelsec.ResumeLayout(false);
			this.grpMelsec.PerformLayout();
			this.grpAB.ResumeLayout(false);
			this.grpAB.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtTestTo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtTestFrom)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtTestInterval)).EndInit();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.txtMtInterval)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
		
		/// <summary>
		/// 해당 응용 프로그램의 주 진입점입니다.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmTest());
		}


		bool isTestWork = false;
		long lngTestTimes = 0;
		decimal decTestValue = 0;
		decimal decTestMaxValue = 0;
		Function.Util.Log log = null;


		private void btnOpen_Click(object sender, System.EventArgs e)
		{
			if (Comm == null) return;
			
			if (Comm.evtChConnectionStatus == null)
				Comm.evtChConnectionStatus = new clsPLCModule.delChConnectionStatus(Ch_ConnectionStatus);

			
			Comm.Open();
			
			this.Grid.DataSource = Comm.dtAddress;
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			Comm.Close();
		}

		private void frmTest_Load(object sender, System.EventArgs e)
		{
			this.Text += " v." + Application.ProductVersion;

			foreach(enPlcType en in Enum.GetValues(typeof(enPlcType)))
			{
				if (en != 0)	//this.cmbPLCType.Items.Add(en.ToString());	
					this.cmbPLCType.Items.Add(en);	
			}
		}


		string strLogFileName = "testPLC";
		private void cmbPLCType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//enPlcType en= (enPlcType)Enum.Parse(typeof(enPlcType), cmbPLCType.SelectedItem.ToString(), true);
			enPlcType en = (enPlcType)cmbPLCType.SelectedItem;

			


			try
			{
				switch(en)
				{				
					case enPlcType.Melsec_A:
					case enPlcType.Melsec_Q:
					case enPlcType.LS_XGT:
					case enPlcType.LS_XGI:
					case enPlcType.TEST:
						string strIP = this.txtIPAdd.Text;
						int intPort = int.Parse(this.txtPort.Text);
						string strDeviceType = this.txtDeviceType.Text.PadLeft(1, char.Parse(" "));
                        Comm = new clsPLCModule(en, strIP, intPort, strDeviceType,strLogFileName);

						Comm.AddAddress("900");
						Comm.AddAddress("901");
						Comm.AddAddress("902");
						Comm.AddAddress("903");
						Comm.AddAddress("904");
						
						Comm.AddAddress("1000");
						Comm.AddAddress("1001");


						//Comm.AddAddress("%MW500");
						//Comm.AddAddress("%DW1000");
						//Comm.AddAddress("%DW1001");
						//Comm.AddAddress("%DW1100");
						//Comm.AddAddress("%DW1101");
						


						break;
#if(ABPLC)
					case enPlcType.AB:
						string strNode = this.txtNODE.Text;
						string strProgID = this.txtProgID.Text;
						string strGroupName = "grpTest";
						string strTopicName = this.txtTopic.Text;
						int intUpdateRate = 1000;
						Comm = new clsPLCModule(en, strNode, strProgID, strGroupName, strTopicName, intUpdateRate, strLogFileName);
						break;
#endif
					default:
						Comm = null;
						break;
				}
			}
			catch(Exception ex)
			{
				ExceptionMsg(ex);
			}
		}

		private void thTest(object obj)
		{
			if (isTestWork) return;

			try
			{
				isTestWork = true;
				if(log == null)
				{
					string fld = ".\\TestLog";

					if(!Function.system.clsFile.FolderExists(fld))	Function.system.clsFile.FolderCreate(fld);
					
					log = new Function.Util.Log(fld, "TestLog", 30, true);
				}

				//address, interval, from, to
				string[] param = (string[])obj;


				lngTestTimes++;
				if(lngTestTimes == 1)
				{
					decTestValue = decimal.Parse(param[2]);
					decTestMaxValue = decimal.Parse(param[3]);
					log.WLog("PlcTest를 시작합니다.[IP]{0} [Port]{1} [PlcType]{2} [ValueFrom]{3} [ValueTo]{4}", Comm.IPAddress, Comm.Port, Comm.enPLCType, decTestValue, decTestMaxValue);
				}
				else if( (lngTestTimes%100) == 0)
				{
					log.WLog("PlcTest {0}번째 진행중 [현재값]{1}", lngTestTimes, decTestValue);
				}

				Comm.WriteOrder(param[0], int.Parse(decTestValue.ToString()));

				Function.form.control.Invoke_Control_Text(lblTestMsg, string.Format("[{0}] Value:{1}",DateTime.Now.ToString("HH:mm:ss"),decTestValue));

				decTestValue++;

			}
			catch(Exception ex)
			{
				log.WLog_Exception("thTest", ex);

				Function.form.control.Invoke_Control_Text(lblTestMsg, ex.Message);
			}
			finally
			{
				isTestWork = false;
			}


		}


		private void ExceptionMsg(Exception ex)
		{
			MessageBox.Show(ex.Message, "에러", MessageBoxButtons.OK);
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			
			string txtItemName = this.txtItem.Text.Trim();
			if (txtItemName != string.Empty)
			{
				Comm.AddAddress(new string [] {txtItemName});
				this.Grid.DataSource = Comm.dtAddress;
			}

		}

		private void Ch_ConnectionStatus(bool bolStatus)
		{
			if (bolStatus)
			{				
				Function.form.control.Invoke_Control_Text(lblStatus, "Open");
			}
			else
			{
				Function.form.control.Invoke_Control_Text(lblStatus, "Close");				
			}
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			foreach(DataRow dr in Comm.dtAddress.Rows)
			{
				string strAdd = (string)dr["Address"];
				Comm.WriteOrder(strAdd, 998);
			}

		}

		private void txtIPAdd_TextChanged(object sender, EventArgs e)
		{

		}

		private void btnMonitor_Click(object sender, EventArgs e)
		{
			if (Comm == null) return;

			//int v = Comm.GetValueInt("%DW1200");

			Comm.MonitorFormOpen();

			return;
		}

		private void btnTestStart_Click(object sender, EventArgs e)
		{
			bool enabled = false;
			if(tmrTest == null)
			{
				lngTestTimes = 0;
				//address, interval, from, to
				string[] param = new string[] { txtTestAddress.Text, txtTestInterval.Value.ToString(), txtTestFrom.Value.ToString(), txtTestTo.Value.ToString() };
				int period = int.Parse(txtTestInterval.Value.ToString());
				tmrTest = new System.Threading.Timer(new System.Threading.TimerCallback(thTest), param , 0, period );				
			}
			else
			{
				tmrTest.Dispose();
				tmrTest = null;
				enabled = true;
			}


			txtTestAddress.Enabled = enabled;
			txtTestInterval.Enabled = enabled;
			txtTestFrom.Enabled = enabled;
			txtTestTo.Enabled = enabled;

			btnTestStart.Text = enabled ? "시 작" : "종 료";
		}




		
		bool bMtStart = false;
		Random rnd = new Random();
		int iMtInterval;
		int iMtThCnt = 0;
		string[] sMtAdds;

		private void btnMtAdd_Click(object sender, EventArgs e)
		{
			bMtStart = true;
			iMtInterval = Fnc.obj2int(txtMtInterval.Value);

			sMtAdds = new string[Comm.dtAddress.Rows.Count];

			for(int i=0;i < sMtAdds.Length;i++)
			{
				sMtAdds[i] = Fnc.obj2String(Comm.dtAddress.Rows[i]["Address"]);
			}



			for (int i=0;i<10;i++)
			{
				Thread th = new Thread(new ParameterizedThreadStart(thMt_Test));
				th.IsBackground = true;
				th.Start(null);

				iMtThCnt++;				
			}

			txtMtMsg.Text = string.Format("{0}개의 쓰래드가 작업 중입니다.", iMtThCnt);
		}

		private void thMt_Test(object obj)
		{
			
			while(true)
			{
				if (!bMtStart) break;

				int r = rnd.Next(0, sMtAdds.Length);
				string add = sMtAdds[r];
				int v = Comm.GetValueInt(add);

				if (v >= int.MaxValue) v = 0;

				Comm.WriteOrder(add, v + 1);

				Thread.Sleep(iMtInterval);

			}


		}

		private void btnMtEnd_Click(object sender, EventArgs e)
		{
			bMtStart = false;

			txtMtMsg.Text = string.Format("작업을 중지 하였습니다.");

		}
	}
}
