namespace Function.Device
{
	partial class usrRS232_Setting
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

		#region 구성 요소 디자이너에서 생성한 코드

		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel2 = new System.Windows.Forms.Panel();
			this.cmbDatabit = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.cmbParity = new System.Windows.Forms.ComboBox();
			this.Parity = new System.Windows.Forms.Label();
			this.cmbHandshake = new System.Windows.Forms.ComboBox();
			this.cmbStopbit = new System.Windows.Forms.ComboBox();
			this.cmbBaudRate = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtComport = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtComport)).BeginInit();
			this.SuspendLayout();
			// 
			// panel2
			// 
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.cmbDatabit);
			this.panel2.Controls.Add(this.label12);
			this.panel2.Controls.Add(this.cmbParity);
			this.panel2.Controls.Add(this.Parity);
			this.panel2.Controls.Add(this.cmbHandshake);
			this.panel2.Controls.Add(this.cmbStopbit);
			this.panel2.Controls.Add(this.cmbBaudRate);
			this.panel2.Controls.Add(this.label6);
			this.panel2.Controls.Add(this.label5);
			this.panel2.Controls.Add(this.label4);
			this.panel2.Controls.Add(this.txtComport);
			this.panel2.Controls.Add(this.label3);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(215, 160);
			this.panel2.TabIndex = 2;
			// 
			// cmbDatabit
			// 
			this.cmbDatabit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDatabit.FormattingEnabled = true;
			this.cmbDatabit.Location = new System.Drawing.Point(84, 54);
			this.cmbDatabit.Name = "cmbDatabit";
			this.cmbDatabit.Size = new System.Drawing.Size(121, 20);
			this.cmbDatabit.TabIndex = 15;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(3, 59);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(44, 12);
			this.label12.TabIndex = 14;
			this.label12.Text = "DataBit";
			// 
			// cmbParity
			// 
			this.cmbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbParity.FormattingEnabled = true;
			this.cmbParity.Location = new System.Drawing.Point(84, 104);
			this.cmbParity.Name = "cmbParity";
			this.cmbParity.Size = new System.Drawing.Size(121, 20);
			this.cmbParity.TabIndex = 13;
			// 
			// Parity
			// 
			this.Parity.AutoSize = true;
			this.Parity.Location = new System.Drawing.Point(3, 107);
			this.Parity.Name = "Parity";
			this.Parity.Size = new System.Drawing.Size(37, 12);
			this.Parity.TabIndex = 12;
			this.Parity.Text = "Parity";
			// 
			// cmbHandshake
			// 
			this.cmbHandshake.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbHandshake.FormattingEnabled = true;
			this.cmbHandshake.Location = new System.Drawing.Point(84, 128);
			this.cmbHandshake.Name = "cmbHandshake";
			this.cmbHandshake.Size = new System.Drawing.Size(121, 20);
			this.cmbHandshake.TabIndex = 7;
			// 
			// cmbStopbit
			// 
			this.cmbStopbit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbStopbit.FormattingEnabled = true;
			this.cmbStopbit.Location = new System.Drawing.Point(84, 79);
			this.cmbStopbit.Name = "cmbStopbit";
			this.cmbStopbit.Size = new System.Drawing.Size(121, 20);
			this.cmbStopbit.TabIndex = 6;
			// 
			// cmbBaudRate
			// 
			this.cmbBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbBaudRate.FormattingEnabled = true;
			this.cmbBaudRate.Location = new System.Drawing.Point(84, 30);
			this.cmbBaudRate.Name = "cmbBaudRate";
			this.cmbBaudRate.Size = new System.Drawing.Size(121, 20);
			this.cmbBaudRate.TabIndex = 5;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(3, 133);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(53, 12);
			this.label6.TabIndex = 4;
			this.label6.Text = "흐름제어";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 82);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(44, 12);
			this.label5.TabIndex = 3;
			this.label5.Text = "StopBit";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 35);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(59, 12);
			this.label4.TabIndex = 2;
			this.label4.Text = "BaudRate";
			// 
			// txtComport
			// 
			this.txtComport.Location = new System.Drawing.Point(84, 6);
			this.txtComport.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.txtComport.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.txtComport.Name = "txtComport";
			this.txtComport.Size = new System.Drawing.Size(75, 21);
			this.txtComport.TabIndex = 1;
			this.txtComport.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(54, 12);
			this.label3.TabIndex = 0;
			this.label3.Text = "ComPort";
			// 
			// usrMK80_Setting
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel2);
			this.Name = "usrMK80_Setting";
			this.Size = new System.Drawing.Size(215, 160);
			this.Load += new System.EventHandler(this.usrMK80_Setting_Load);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtComport)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ComboBox cmbDatabit;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox cmbParity;
		private System.Windows.Forms.Label Parity;
		private System.Windows.Forms.ComboBox cmbHandshake;
		private System.Windows.Forms.ComboBox cmbStopbit;
		private System.Windows.Forms.ComboBox cmbBaudRate;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown txtComport;
		private System.Windows.Forms.Label label3;
	}
}
