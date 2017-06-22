namespace PLCModule
{
	partial class frmPLCValueChange
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
			this.lblAdd = new System.Windows.Forms.TextBox();
			this.lblPLCType = new System.Windows.Forms.Label();
			this.lblValue = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lblHex = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lbit0 = new System.Windows.Forms.Label();
			this.lbit1 = new System.Windows.Forms.Label();
			this.lbit2 = new System.Windows.Forms.Label();
			this.lbit3 = new System.Windows.Forms.Label();
			this.lbit7 = new System.Windows.Forms.Label();
			this.lbit6 = new System.Windows.Forms.Label();
			this.lbit5 = new System.Windows.Forms.Label();
			this.lbit4 = new System.Windows.Forms.Label();
			this.lbit11 = new System.Windows.Forms.Label();
			this.lbit10 = new System.Windows.Forms.Label();
			this.lbit9 = new System.Windows.Forms.Label();
			this.lbit8 = new System.Windows.Forms.Label();
			this.lbit15 = new System.Windows.Forms.Label();
			this.lbit14 = new System.Windows.Forms.Label();
			this.lbit13 = new System.Windows.Forms.Label();
			this.lbit12 = new System.Windows.Forms.Label();
			this.txtHex = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtValue = new System.Windows.Forms.NumericUpDown();
			this.btnOk = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.txtValue)).BeginInit();
			this.SuspendLayout();
			// 
			// lblAdd
			// 
			this.lblAdd.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 10F);
			this.lblAdd.Location = new System.Drawing.Point(59, 4);
			this.lblAdd.Name = "lblAdd";
			this.lblAdd.ReadOnly = true;
			this.lblAdd.Size = new System.Drawing.Size(101, 23);
			this.lblAdd.TabIndex = 8;
			this.lblAdd.Text = "%DW2000";
			// 
			// lblPLCType
			// 
			this.lblPLCType.Location = new System.Drawing.Point(-1, 3);
			this.lblPLCType.Name = "lblPLCType";
			this.lblPLCType.Size = new System.Drawing.Size(64, 24);
			this.lblPLCType.TabIndex = 7;
			this.lblPLCType.Text = "Address";
			this.lblPLCType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblValue
			// 
			this.lblValue.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 10F);
			this.lblValue.Location = new System.Drawing.Point(208, 4);
			this.lblValue.Name = "lblValue";
			this.lblValue.ReadOnly = true;
			this.lblValue.Size = new System.Drawing.Size(83, 23);
			this.lblValue.TabIndex = 10;
			this.lblValue.Text = "0012";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(163, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(42, 24);
			this.label1.TabIndex = 9;
			this.label1.Text = "Value";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblHex
			// 
			this.lblHex.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 10F);
			this.lblHex.Location = new System.Drawing.Point(369, 4);
			this.lblHex.Name = "lblHex";
			this.lblHex.ReadOnly = true;
			this.lblHex.Size = new System.Drawing.Size(42, 23);
			this.lblHex.TabIndex = 12;
			this.lblHex.Text = "000C";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(288, 3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(75, 24);
			this.label2.TabIndex = 11;
			this.label2.Text = "Value(Hex)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbit0
			// 
			this.lbit0.BackColor = System.Drawing.Color.LightGray;
			this.lbit0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit0.Location = new System.Drawing.Point(387, 26);
			this.lbit0.Name = "lbit0";
			this.lbit0.Size = new System.Drawing.Size(24, 24);
			this.lbit0.TabIndex = 13;
			this.lbit0.Text = "0";
			this.lbit0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit0.Click += new System.EventHandler(this.lbit_Click);
			// 
			// lbit1
			// 
			this.lbit1.BackColor = System.Drawing.Color.LightGray;
			this.lbit1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit1.Location = new System.Drawing.Point(363, 26);
			this.lbit1.Name = "lbit1";
			this.lbit1.Size = new System.Drawing.Size(24, 24);
			this.lbit1.TabIndex = 14;
			this.lbit1.Text = "1";
			this.lbit1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit1.Click += new System.EventHandler(this.lbit_Click);
			// 
			// lbit2
			// 
			this.lbit2.BackColor = System.Drawing.Color.LightGray;
			this.lbit2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit2.Location = new System.Drawing.Point(339, 26);
			this.lbit2.Name = "lbit2";
			this.lbit2.Size = new System.Drawing.Size(24, 24);
			this.lbit2.TabIndex = 15;
			this.lbit2.Text = "2";
			this.lbit2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit2.Click += new System.EventHandler(this.lbit_Click);
			// 
			// lbit3
			// 
			this.lbit3.BackColor = System.Drawing.Color.LightGray;
			this.lbit3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit3.Location = new System.Drawing.Point(315, 26);
			this.lbit3.Name = "lbit3";
			this.lbit3.Size = new System.Drawing.Size(24, 24);
			this.lbit3.TabIndex = 16;
			this.lbit3.Text = "3";
			this.lbit3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit3.Click += new System.EventHandler(this.lbit_Click);
			// 
			// lbit7
			// 
			this.lbit7.BackColor = System.Drawing.Color.LightGray;
			this.lbit7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit7.Location = new System.Drawing.Point(211, 26);
			this.lbit7.Name = "lbit7";
			this.lbit7.Size = new System.Drawing.Size(24, 24);
			this.lbit7.TabIndex = 20;
			this.lbit7.Text = "7";
			this.lbit7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit7.Click += new System.EventHandler(this.lbit_Click);
			// 
			// lbit6
			// 
			this.lbit6.BackColor = System.Drawing.Color.LightGray;
			this.lbit6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit6.Location = new System.Drawing.Point(235, 26);
			this.lbit6.Name = "lbit6";
			this.lbit6.Size = new System.Drawing.Size(24, 24);
			this.lbit6.TabIndex = 19;
			this.lbit6.Text = "6";
			this.lbit6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit6.Click += new System.EventHandler(this.lbit_Click);
			// 
			// lbit5
			// 
			this.lbit5.BackColor = System.Drawing.Color.LightGray;
			this.lbit5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit5.Location = new System.Drawing.Point(259, 26);
			this.lbit5.Name = "lbit5";
			this.lbit5.Size = new System.Drawing.Size(24, 24);
			this.lbit5.TabIndex = 18;
			this.lbit5.Text = "5";
			this.lbit5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit5.Click += new System.EventHandler(this.lbit_Click);
			// 
			// lbit4
			// 
			this.lbit4.BackColor = System.Drawing.Color.LightGray;
			this.lbit4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit4.Location = new System.Drawing.Point(283, 26);
			this.lbit4.Name = "lbit4";
			this.lbit4.Size = new System.Drawing.Size(24, 24);
			this.lbit4.TabIndex = 17;
			this.lbit4.Text = "4";
			this.lbit4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit4.Click += new System.EventHandler(this.lbit_Click);
			// 
			// lbit11
			// 
			this.lbit11.BackColor = System.Drawing.Color.LightGray;
			this.lbit11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit11.Location = new System.Drawing.Point(109, 26);
			this.lbit11.Name = "lbit11";
			this.lbit11.Size = new System.Drawing.Size(24, 24);
			this.lbit11.TabIndex = 24;
			this.lbit11.Text = "11";
			this.lbit11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit11.Click += new System.EventHandler(this.lbit_Click);
			// 
			// lbit10
			// 
			this.lbit10.BackColor = System.Drawing.Color.LightGray;
			this.lbit10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit10.Location = new System.Drawing.Point(133, 26);
			this.lbit10.Name = "lbit10";
			this.lbit10.Size = new System.Drawing.Size(24, 24);
			this.lbit10.TabIndex = 23;
			this.lbit10.Text = "10";
			this.lbit10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit10.Click += new System.EventHandler(this.lbit_Click);
			// 
			// lbit9
			// 
			this.lbit9.BackColor = System.Drawing.Color.LightGray;
			this.lbit9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit9.Location = new System.Drawing.Point(157, 26);
			this.lbit9.Name = "lbit9";
			this.lbit9.Size = new System.Drawing.Size(24, 24);
			this.lbit9.TabIndex = 22;
			this.lbit9.Text = "9";
			this.lbit9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit9.Click += new System.EventHandler(this.lbit_Click);
			// 
			// lbit8
			// 
			this.lbit8.BackColor = System.Drawing.Color.LightGray;
			this.lbit8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit8.Location = new System.Drawing.Point(181, 26);
			this.lbit8.Name = "lbit8";
			this.lbit8.Size = new System.Drawing.Size(24, 24);
			this.lbit8.TabIndex = 21;
			this.lbit8.Text = "8";
			this.lbit8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit8.Click += new System.EventHandler(this.lbit_Click);
			// 
			// lbit15
			// 
			this.lbit15.BackColor = System.Drawing.Color.LightGray;
			this.lbit15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit15.Location = new System.Drawing.Point(6, 26);
			this.lbit15.Name = "lbit15";
			this.lbit15.Size = new System.Drawing.Size(24, 24);
			this.lbit15.TabIndex = 28;
			this.lbit15.Text = "15";
			this.lbit15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit15.Click += new System.EventHandler(this.lbit_Click);
			// 
			// lbit14
			// 
			this.lbit14.BackColor = System.Drawing.Color.LightGray;
			this.lbit14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit14.Location = new System.Drawing.Point(30, 26);
			this.lbit14.Name = "lbit14";
			this.lbit14.Size = new System.Drawing.Size(24, 24);
			this.lbit14.TabIndex = 27;
			this.lbit14.Text = "14";
			this.lbit14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit14.Click += new System.EventHandler(this.lbit_Click);
			// 
			// lbit13
			// 
			this.lbit13.BackColor = System.Drawing.Color.LightGray;
			this.lbit13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit13.Location = new System.Drawing.Point(54, 26);
			this.lbit13.Name = "lbit13";
			this.lbit13.Size = new System.Drawing.Size(24, 24);
			this.lbit13.TabIndex = 26;
			this.lbit13.Text = "13";
			this.lbit13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit13.Click += new System.EventHandler(this.lbit_Click);
			// 
			// lbit12
			// 
			this.lbit12.BackColor = System.Drawing.Color.LightGray;
			this.lbit12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbit12.Location = new System.Drawing.Point(78, 26);
			this.lbit12.Name = "lbit12";
			this.lbit12.Size = new System.Drawing.Size(24, 24);
			this.lbit12.TabIndex = 25;
			this.lbit12.Text = "12";
			this.lbit12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbit12.Click += new System.EventHandler(this.lbit_Click);
			// 
			// txtHex
			// 
			this.txtHex.Font = new System.Drawing.Font("Bitstream Vera Sans Mono", 10F);
			this.txtHex.Location = new System.Drawing.Point(236, 55);
			this.txtHex.MaxLength = 4;
			this.txtHex.Name = "txtHex";
			this.txtHex.Size = new System.Drawing.Size(51, 23);
			this.txtHex.TabIndex = 0;
			this.txtHex.Text = "0000";
			this.txtHex.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHex_KeyDown);
			this.txtHex.Leave += new System.EventHandler(this.txtHex_Leave);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(155, 54);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(75, 24);
			this.label3.TabIndex = 31;
			this.label3.Text = "Value(Hex)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(0, 54);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(82, 24);
			this.label4.TabIndex = 29;
			this.label4.Text = "변경할 Value";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtValue
			// 
			this.txtValue.Location = new System.Drawing.Point(79, 56);
			this.txtValue.Maximum = new decimal(new int[] {
            66000,
            0,
            0,
            0});
			this.txtValue.Minimum = new decimal(new int[] {
            66000,
            0,
            0,
            -2147483648});
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(79, 21);
			this.txtValue.TabIndex = 33;
			this.txtValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtValue.ValueChanged += new System.EventHandler(this.txtValue_ValueChanged);
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(297, 55);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(58, 23);
			this.btnOk.TabIndex = 34;
			this.btnOk.Text = "변경";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(361, 55);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(58, 23);
			this.button1.TabIndex = 35;
			this.button1.Text = "취소";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// frmPLCValueChange
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(420, 80);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.txtValue);
			this.Controls.Add(this.txtHex);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.lbit15);
			this.Controls.Add(this.lbit14);
			this.Controls.Add(this.lbit13);
			this.Controls.Add(this.lbit12);
			this.Controls.Add(this.lbit11);
			this.Controls.Add(this.lbit10);
			this.Controls.Add(this.lbit9);
			this.Controls.Add(this.lbit8);
			this.Controls.Add(this.lbit7);
			this.Controls.Add(this.lbit6);
			this.Controls.Add(this.lbit5);
			this.Controls.Add(this.lbit4);
			this.Controls.Add(this.lbit3);
			this.Controls.Add(this.lbit2);
			this.Controls.Add(this.lbit1);
			this.Controls.Add(this.lbit0);
			this.Controls.Add(this.lblHex);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblValue);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblAdd);
			this.Controls.Add(this.lblPLCType);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.KeyPreview = true;
			this.Name = "frmPLCValueChange";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Address 값 변경";
			this.Load += new System.EventHandler(this.frmPLCValueChange_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPLCValueChange_KeyDown);
			((System.ComponentModel.ISupportInitialize)(this.txtValue)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox lblAdd;
		private System.Windows.Forms.Label lblPLCType;
		private System.Windows.Forms.TextBox lblValue;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox lblHex;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lbit0;
		private System.Windows.Forms.Label lbit1;
		private System.Windows.Forms.Label lbit2;
		private System.Windows.Forms.Label lbit3;
		private System.Windows.Forms.Label lbit7;
		private System.Windows.Forms.Label lbit6;
		private System.Windows.Forms.Label lbit5;
		private System.Windows.Forms.Label lbit4;
		private System.Windows.Forms.Label lbit11;
		private System.Windows.Forms.Label lbit10;
		private System.Windows.Forms.Label lbit9;
		private System.Windows.Forms.Label lbit8;
		private System.Windows.Forms.Label lbit15;
		private System.Windows.Forms.Label lbit14;
		private System.Windows.Forms.Label lbit13;
		private System.Windows.Forms.Label lbit12;
		private System.Windows.Forms.TextBox txtHex;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown txtValue;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button button1;
	}
}