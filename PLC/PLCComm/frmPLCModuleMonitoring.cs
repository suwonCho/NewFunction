using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;

using Function;


namespace PLCComm
{
	/// <summary>
	/// Form1에 대한 요약 설명입니다.
	/// </summary>
	public class frmPLCMonitor : System.Windows.Forms.Form
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		string strMsg = string.Empty;
		private System.Windows.Forms.Label lblPLCType;
		private System.Windows.Forms.GroupBox grpMelsec;
		private System.Windows.Forms.Label lblIP;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox txtIPAdd;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Label lblStatus;
		private TextBox txtPlcType;
		PLCComm Comm;
		private DataGridView Grid;
		private Label lblR;

		public bool isOpenForm = false;

		public frmPLCMonitor(PLCComm comm)
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent를 호출한 다음 생성자 코드를 추가합니다.
			//

			Comm = comm;
			Comm.OnChConnectionStatus += Ch_ConnectionStatus;			
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
			this.lblStatus = new System.Windows.Forms.Label();
			this.lblPLCType = new System.Windows.Forms.Label();
			this.grpMelsec = new System.Windows.Forms.GroupBox();
			this.txtPlcType = new System.Windows.Forms.TextBox();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.txtIPAdd = new System.Windows.Forms.TextBox();
			this.lblIP = new System.Windows.Forms.Label();
			this.Grid = new System.Windows.Forms.DataGridView();
			this.lblR = new System.Windows.Forms.Label();
			this.grpMelsec.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
			this.SuspendLayout();
			// 
			// lblStatus
			// 
			this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblStatus.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblStatus.Location = new System.Drawing.Point(344, 17);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(67, 24);
			this.lblStatus.TabIndex = 2;
			this.lblStatus.Text = "Close";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblPLCType
			// 
			this.lblPLCType.Location = new System.Drawing.Point(19, 17);
			this.lblPLCType.Name = "lblPLCType";
			this.lblPLCType.Size = new System.Drawing.Size(64, 24);
			this.lblPLCType.TabIndex = 5;
			this.lblPLCType.Text = "PLC Type";
			this.lblPLCType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// grpMelsec
			// 
			this.grpMelsec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpMelsec.Controls.Add(this.txtPlcType);
			this.grpMelsec.Controls.Add(this.txtPort);
			this.grpMelsec.Controls.Add(this.lblStatus);
			this.grpMelsec.Controls.Add(this.lblPort);
			this.grpMelsec.Controls.Add(this.lblPLCType);
			this.grpMelsec.Controls.Add(this.txtIPAdd);
			this.grpMelsec.Controls.Add(this.lblIP);
			this.grpMelsec.Location = new System.Drawing.Point(8, 12);
			this.grpMelsec.Name = "grpMelsec";
			this.grpMelsec.Size = new System.Drawing.Size(424, 107);
			this.grpMelsec.TabIndex = 8;
			this.grpMelsec.TabStop = false;
			this.grpMelsec.Text = "PLC Setting";
			// 
			// txtPlcType
			// 
			this.txtPlcType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPlcType.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 10F);
			this.txtPlcType.Location = new System.Drawing.Point(96, 17);
			this.txtPlcType.Name = "txtPlcType";
			this.txtPlcType.ReadOnly = true;
			this.txtPlcType.Size = new System.Drawing.Size(242, 23);
			this.txtPlcType.TabIndex = 6;
			this.txtPlcType.Text = "Melsec Q";
			// 
			// txtPort
			// 
			this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPort.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 10F);
			this.txtPort.Location = new System.Drawing.Point(96, 76);
			this.txtPort.Name = "txtPort";
			this.txtPort.ReadOnly = true;
			this.txtPort.Size = new System.Drawing.Size(291, 23);
			this.txtPort.TabIndex = 3;
			this.txtPort.Text = "2004";
			// 
			// lblPort
			// 
			this.lblPort.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblPort.Location = new System.Drawing.Point(8, 76);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(80, 24);
			this.lblPort.TabIndex = 2;
			this.lblPort.Text = "Port";
			this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtIPAdd
			// 
			this.txtIPAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtIPAdd.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 10F);
			this.txtIPAdd.Location = new System.Drawing.Point(96, 44);
			this.txtIPAdd.Name = "txtIPAdd";
			this.txtIPAdd.ReadOnly = true;
			this.txtIPAdd.Size = new System.Drawing.Size(315, 23);
			this.txtIPAdd.TabIndex = 1;
			this.txtIPAdd.Text = "192.168.0.110";
			// 
			// lblIP
			// 
			this.lblIP.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblIP.Location = new System.Drawing.Point(8, 44);
			this.lblIP.Name = "lblIP";
			this.lblIP.Size = new System.Drawing.Size(80, 24);
			this.lblIP.TabIndex = 0;
			this.lblIP.Text = "IPAddress";
			this.lblIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Grid
			// 
			this.Grid.AllowUserToResizeRows = false;
			this.Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
			this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.Grid.Location = new System.Drawing.Point(7, 125);
			this.Grid.MultiSelect = false;
			this.Grid.Name = "Grid";
			this.Grid.ReadOnly = true;
			this.Grid.RowTemplate.Height = 23;
			this.Grid.Size = new System.Drawing.Size(425, 234);
			this.Grid.TabIndex = 9;
			this.Grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CellDoubleClick);
			this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
			// 
			// lblR
			// 
			this.lblR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblR.BackColor = System.Drawing.SystemColors.Highlight;
			this.lblR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblR.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblR.ForeColor = System.Drawing.Color.Snow;
			this.lblR.Location = new System.Drawing.Point(401, 88);
			this.lblR.Name = "lblR";
			this.lblR.Size = new System.Drawing.Size(25, 24);
			this.lblR.TabIndex = 7;
			this.lblR.Text = "R";
			this.lblR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblR.Visible = false;
			// 
			// frmPLCMonitor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(440, 371);
			this.Controls.Add(this.lblR);
			this.Controls.Add(this.Grid);
			this.Controls.Add(this.grpMelsec);
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Name = "frmPLCMonitor";
			this.Text = "PLC Comm. Monitoring";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPLCMonitor_FormClosed);
			this.Load += new System.EventHandler(this.frmTest_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPLCMonitor_KeyDown);
			this.grpMelsec.ResumeLayout(false);
			this.grpMelsec.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		 
		
		private void frmTest_Load(object sender, System.EventArgs e)
		{
			Grid.DataSource = Comm.dtAddress;
			txtPlcType.Text = Comm.enPLCType.ToString();
			txtIPAdd.Text = Comm.IPAddress;
			txtPort.Text = Comm.Port.ToString();
			Ch_ConnectionStatus(Comm.ConnctionStatus );
			isOpenForm = true;


			foreach(DataGridViewColumn c in Grid.Columns)
			{
				c.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
			}


			
			this.Text += " v" + Assembly.GetAssembly(Type.GetType("PLCComm.PLCComm")).GetName().Version.ToString();

#if(LOG)
			this.Text += "[LOG]";
#endif

			//string[] s = a.GetManifestResourceInfo("Version"); //.GetManifestResourceInfo("AssemblyVersion");
		}

		private delegate void delCh_ConnectionStatus(enStatus bolStatus);

		private void Ch_ConnectionStatus(enStatus bolStatus)
		{
			if (lblStatus.InvokeRequired)
			{
				lblStatus.BeginInvoke(new delCh_ConnectionStatus(Ch_ConnectionStatus), new object[] { bolStatus });
				return;
			}

			if (bolStatus == enStatus.OK)
			{
				lblStatus.Text = "Open";
				lblStatus.BackColor = Color.Green;
			}
			else
			{
				lblStatus.Text = "Close";
				lblStatus.BackColor = Color.Red;
			}
		}

		private void frmPLCMonitor_FormClosed(object sender, FormClosedEventArgs e)
		{
			isOpenForm = false;
		}

		private void frmPLCMonitor_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F9)
			{
				lblR.Visible = Grid.ReadOnly;
				Grid.ReadOnly = !Grid.ReadOnly;
			}

			if (e.KeyCode == Keys.Escape) this.Close();
		}

	

		private void Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (Grid.ReadOnly) return;

			if (Grid.SelectedRows.Count < 1) return;

			showValueChangedForm(Grid.SelectedRows[0].Cells["Address"].Value.ToString());
			
		}

		private void Grid_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (Grid.ReadOnly) return;

				if (Grid.SelectedRows.Count < 1) return;

				showValueChangedForm(Grid.SelectedRows[0].Cells["Address"].Value.ToString());
				
				e.Handled = true;
			}
		}

		System.Collections.Generic.Dictionary<string, Form> dicAdds = new System.Collections.Generic.Dictionary<string, Form>();
		
		private void showValueChangedForm(string address)
		{
			frmPLCValueChange f;

			if (dicAdds.ContainsKey(address))
			{
				f = dicAdds[address] as frmPLCValueChange;
				f.BringToFront();
				f.Focus();
			}
			else
			{
				f = new frmPLCValueChange(this, Comm, Grid.SelectedRows[0].Cells["Address"].Value.ToString());
				f.Tag = address;
				f.FormClosed += f_FormClosed;
				dicAdds.Add(address, f);
				f.Show();
			}
		}

		void f_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				string add = (string)((Form)sender).Tag;
				dicAdds.Remove(add);
			}
			catch
			{ }


		}
		




	}
}
