namespace Function.form
{
	partial class usrNumberInput
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
			this.spMain = new System.Windows.Forms.SplitContainer();
			this.txtNumber = new System.Windows.Forms.TextBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnC = new System.Windows.Forms.Button();
			this.btn0 = new System.Windows.Forms.Button();
			this.btnB = new System.Windows.Forms.Button();
			this.btn3 = new System.Windows.Forms.Button();
			this.btn2 = new System.Windows.Forms.Button();
			this.btn1 = new System.Windows.Forms.Button();
			this.btn6 = new System.Windows.Forms.Button();
			this.btn5 = new System.Windows.Forms.Button();
			this.btn4 = new System.Windows.Forms.Button();
			this.btn9 = new System.Windows.Forms.Button();
			this.btn8 = new System.Windows.Forms.Button();
			this.btn7 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.spMain)).BeginInit();
			this.spMain.Panel1.SuspendLayout();
			this.spMain.Panel2.SuspendLayout();
			this.spMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// spMain
			// 
			this.spMain.BackColor = System.Drawing.SystemColors.Control;
			this.spMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.spMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.spMain.Location = new System.Drawing.Point(4, 4);
			this.spMain.Name = "spMain";
			this.spMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// spMain.Panel1
			// 
			this.spMain.Panel1.Controls.Add(this.txtNumber);
			// 
			// spMain.Panel2
			// 
			this.spMain.Panel2.Controls.Add(this.btnOk);
			this.spMain.Panel2.Controls.Add(this.btnC);
			this.spMain.Panel2.Controls.Add(this.btn0);
			this.spMain.Panel2.Controls.Add(this.btnB);
			this.spMain.Panel2.Controls.Add(this.btn3);
			this.spMain.Panel2.Controls.Add(this.btn2);
			this.spMain.Panel2.Controls.Add(this.btn1);
			this.spMain.Panel2.Controls.Add(this.btn6);
			this.spMain.Panel2.Controls.Add(this.btn5);
			this.spMain.Panel2.Controls.Add(this.btn4);
			this.spMain.Panel2.Controls.Add(this.btn9);
			this.spMain.Panel2.Controls.Add(this.btn8);
			this.spMain.Panel2.Controls.Add(this.btn7);
			this.spMain.Size = new System.Drawing.Size(379, 506);
			this.spMain.SplitterDistance = 68;
			this.spMain.TabIndex = 0;
			// 
			// txtNumber
			// 
			this.txtNumber.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtNumber.Font = new System.Drawing.Font("굴림체", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.txtNumber.Location = new System.Drawing.Point(0, 0);
			this.txtNumber.MaxLength = 10;
			this.txtNumber.Multiline = true;
			this.txtNumber.Name = "txtNumber";
			this.txtNumber.Size = new System.Drawing.Size(377, 66);
			this.txtNumber.TabIndex = 2;
			this.txtNumber.Text = "00";
			this.txtNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnOk
			// 
			this.btnOk.BackColor = System.Drawing.Color.Silver;
			this.btnOk.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnOk.Font = new System.Drawing.Font("돋움", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnOk.Location = new System.Drawing.Point(0, 390);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(377, 42);
			this.btnOk.TabIndex = 28;
			this.btnOk.Text = "확인";
			this.btnOk.UseVisualStyleBackColor = false;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnC
			// 
			this.btnC.BackColor = System.Drawing.Color.White;
			this.btnC.Font = new System.Drawing.Font("굴림체", 65F, System.Drawing.FontStyle.Bold);
			this.btnC.Location = new System.Drawing.Point(133, 298);
			this.btnC.Name = "btnC";
			this.btnC.Size = new System.Drawing.Size(124, 93);
			this.btnC.TabIndex = 29;
			this.btnC.Text = ".";
			this.btnC.UseVisualStyleBackColor = false;
			this.btnC.Click += new System.EventHandler(this.NumberClick);
			// 
			// btn0
			// 
			this.btn0.BackColor = System.Drawing.Color.White;
			this.btn0.Font = new System.Drawing.Font("굴림체", 65F, System.Drawing.FontStyle.Bold);
			this.btn0.Location = new System.Drawing.Point(3, 298);
			this.btn0.Name = "btn0";
			this.btn0.Size = new System.Drawing.Size(124, 93);
			this.btn0.TabIndex = 27;
			this.btn0.Text = "0";
			this.btn0.UseVisualStyleBackColor = false;
			this.btn0.Click += new System.EventHandler(this.NumberClick);
			// 
			// btnB
			// 
			this.btnB.BackColor = System.Drawing.Color.Silver;
			this.btnB.Font = new System.Drawing.Font("돋움", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnB.Location = new System.Drawing.Point(263, 298);
			this.btnB.Name = "btnB";
			this.btnB.Size = new System.Drawing.Size(124, 93);
			this.btnB.TabIndex = 26;
			this.btnB.Text = "←";
			this.btnB.UseVisualStyleBackColor = false;
			this.btnB.Click += new System.EventHandler(this.btnBack_Click);
			// 
			// btn3
			// 
			this.btn3.BackColor = System.Drawing.Color.White;
			this.btn3.Font = new System.Drawing.Font("굴림체", 65F, System.Drawing.FontStyle.Bold);
			this.btn3.Location = new System.Drawing.Point(263, 199);
			this.btn3.Name = "btn3";
			this.btn3.Size = new System.Drawing.Size(124, 93);
			this.btn3.TabIndex = 25;
			this.btn3.Text = "3";
			this.btn3.UseVisualStyleBackColor = false;
			this.btn3.Click += new System.EventHandler(this.NumberClick);
			// 
			// btn2
			// 
			this.btn2.BackColor = System.Drawing.Color.White;
			this.btn2.Font = new System.Drawing.Font("굴림체", 65F, System.Drawing.FontStyle.Bold);
			this.btn2.Location = new System.Drawing.Point(133, 199);
			this.btn2.Name = "btn2";
			this.btn2.Size = new System.Drawing.Size(124, 93);
			this.btn2.TabIndex = 24;
			this.btn2.Text = "2";
			this.btn2.UseVisualStyleBackColor = false;
			this.btn2.Click += new System.EventHandler(this.NumberClick);
			// 
			// btn1
			// 
			this.btn1.BackColor = System.Drawing.Color.White;
			this.btn1.Font = new System.Drawing.Font("굴림체", 65F, System.Drawing.FontStyle.Bold);
			this.btn1.Location = new System.Drawing.Point(3, 199);
			this.btn1.Name = "btn1";
			this.btn1.Size = new System.Drawing.Size(124, 93);
			this.btn1.TabIndex = 23;
			this.btn1.Text = "1";
			this.btn1.UseVisualStyleBackColor = false;
			this.btn1.Click += new System.EventHandler(this.NumberClick);
			// 
			// btn6
			// 
			this.btn6.BackColor = System.Drawing.Color.White;
			this.btn6.Font = new System.Drawing.Font("굴림체", 65F, System.Drawing.FontStyle.Bold);
			this.btn6.Location = new System.Drawing.Point(263, 100);
			this.btn6.Name = "btn6";
			this.btn6.Size = new System.Drawing.Size(124, 93);
			this.btn6.TabIndex = 22;
			this.btn6.Text = "6";
			this.btn6.UseVisualStyleBackColor = false;
			this.btn6.Click += new System.EventHandler(this.NumberClick);
			// 
			// btn5
			// 
			this.btn5.BackColor = System.Drawing.Color.White;
			this.btn5.Font = new System.Drawing.Font("굴림체", 65F, System.Drawing.FontStyle.Bold);
			this.btn5.Location = new System.Drawing.Point(133, 100);
			this.btn5.Name = "btn5";
			this.btn5.Size = new System.Drawing.Size(124, 93);
			this.btn5.TabIndex = 21;
			this.btn5.Text = "5";
			this.btn5.UseVisualStyleBackColor = false;
			this.btn5.Click += new System.EventHandler(this.NumberClick);
			// 
			// btn4
			// 
			this.btn4.BackColor = System.Drawing.Color.White;
			this.btn4.Font = new System.Drawing.Font("굴림체", 65F, System.Drawing.FontStyle.Bold);
			this.btn4.Location = new System.Drawing.Point(3, 100);
			this.btn4.Name = "btn4";
			this.btn4.Size = new System.Drawing.Size(124, 93);
			this.btn4.TabIndex = 20;
			this.btn4.Text = "4";
			this.btn4.UseVisualStyleBackColor = false;
			this.btn4.Click += new System.EventHandler(this.NumberClick);
			// 
			// btn9
			// 
			this.btn9.BackColor = System.Drawing.Color.White;
			this.btn9.Font = new System.Drawing.Font("굴림체", 65F, System.Drawing.FontStyle.Bold);
			this.btn9.Location = new System.Drawing.Point(263, 3);
			this.btn9.Name = "btn9";
			this.btn9.Size = new System.Drawing.Size(124, 93);
			this.btn9.TabIndex = 19;
			this.btn9.Text = "9";
			this.btn9.UseVisualStyleBackColor = false;
			this.btn9.Click += new System.EventHandler(this.NumberClick);
			// 
			// btn8
			// 
			this.btn8.BackColor = System.Drawing.Color.White;
			this.btn8.Font = new System.Drawing.Font("굴림체", 65F, System.Drawing.FontStyle.Bold);
			this.btn8.Location = new System.Drawing.Point(133, 3);
			this.btn8.Name = "btn8";
			this.btn8.Size = new System.Drawing.Size(124, 93);
			this.btn8.TabIndex = 18;
			this.btn8.Text = "8";
			this.btn8.UseVisualStyleBackColor = false;
			this.btn8.Click += new System.EventHandler(this.NumberClick);
			// 
			// btn7
			// 
			this.btn7.BackColor = System.Drawing.Color.White;
			this.btn7.Font = new System.Drawing.Font("굴림체", 65F, System.Drawing.FontStyle.Bold);
			this.btn7.Location = new System.Drawing.Point(3, 3);
			this.btn7.Name = "btn7";
			this.btn7.Size = new System.Drawing.Size(124, 93);
			this.btn7.TabIndex = 17;
			this.btn7.Text = "7";
			this.btn7.UseVisualStyleBackColor = false;
			this.btn7.Click += new System.EventHandler(this.NumberClick);
			// 
			// usrNumberInput
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Silver;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.spMain);
			this.Name = "usrNumberInput";
			this.Padding = new System.Windows.Forms.Padding(4);
			this.Size = new System.Drawing.Size(387, 514);
			this.Load += new System.EventHandler(this.usrNumberInput_Load);
			this.spMain.Panel1.ResumeLayout(false);
			this.spMain.Panel1.PerformLayout();
			this.spMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.spMain)).EndInit();
			this.spMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer spMain;
		private System.Windows.Forms.Button btnC;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btn0;
		private System.Windows.Forms.Button btnB;
		private System.Windows.Forms.Button btn3;
		private System.Windows.Forms.Button btn2;
		private System.Windows.Forms.Button btn1;
		private System.Windows.Forms.Button btn6;
		private System.Windows.Forms.Button btn5;
		private System.Windows.Forms.Button btn4;
		private System.Windows.Forms.Button btn9;
		private System.Windows.Forms.Button btn8;
		private System.Windows.Forms.Button btn7;
		private System.Windows.Forms.TextBox txtNumber;

	}
}
