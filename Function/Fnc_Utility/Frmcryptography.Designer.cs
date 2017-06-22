namespace Fnc_Utility
{
    partial class Frmcryptography
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
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtEnResult = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtEnSource = new System.Windows.Forms.TextBox();
			this.btnEnIVGen = new System.Windows.Forms.Button();
			this.txtEnIV = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnEcKeyGen = new System.Windows.Forms.Button();
			this.txtEnKey = new System.Windows.Forms.TextBox();
			this.btnEnc = new System.Windows.Forms.Button();
			this.btnDec = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(25, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "key";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnDec);
			this.groupBox1.Controls.Add(this.btnEnc);
			this.groupBox1.Controls.Add(this.txtEnResult);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtEnSource);
			this.groupBox1.Controls.Add(this.btnEnIVGen);
			this.groupBox1.Controls.Add(this.txtEnIV);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.btnEcKeyGen);
			this.groupBox1.Controls.Add(this.txtEnKey);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(954, 307);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "암호화";
			// 
			// txtEnResult
			// 
			this.txtEnResult.Location = new System.Drawing.Point(504, 77);
			this.txtEnResult.Multiline = true;
			this.txtEnResult.Name = "txtEnResult";
			this.txtEnResult.Size = new System.Drawing.Size(432, 213);
			this.txtEnResult.TabIndex = 8;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(479, 192);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(19, 12);
			this.label3.TabIndex = 7;
			this.label3.Text = "->";
			// 
			// txtEnSource
			// 
			this.txtEnSource.Location = new System.Drawing.Point(17, 77);
			this.txtEnSource.Multiline = true;
			this.txtEnSource.Name = "txtEnSource";
			this.txtEnSource.Size = new System.Drawing.Size(456, 213);
			this.txtEnSource.TabIndex = 6;
			// 
			// btnEnIVGen
			// 
			this.btnEnIVGen.Location = new System.Drawing.Point(430, 40);
			this.btnEnIVGen.Name = "btnEnIVGen";
			this.btnEnIVGen.Size = new System.Drawing.Size(75, 23);
			this.btnEnIVGen.TabIndex = 5;
			this.btnEnIVGen.Text = "IV생성";
			this.btnEnIVGen.UseVisualStyleBackColor = true;
			this.btnEnIVGen.Click += new System.EventHandler(this.btnEnIVGen_Click);
			// 
			// txtEnIV
			// 
			this.txtEnIV.Location = new System.Drawing.Point(46, 42);
			this.txtEnIV.Name = "txtEnIV";
			this.txtEnIV.Size = new System.Drawing.Size(378, 21);
			this.txtEnIV.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(15, 45);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(16, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "IV";
			// 
			// btnEcKeyGen
			// 
			this.btnEcKeyGen.Location = new System.Drawing.Point(431, 12);
			this.btnEcKeyGen.Name = "btnEcKeyGen";
			this.btnEcKeyGen.Size = new System.Drawing.Size(75, 23);
			this.btnEcKeyGen.TabIndex = 2;
			this.btnEcKeyGen.Text = "Key생성";
			this.btnEcKeyGen.UseVisualStyleBackColor = true;
			this.btnEcKeyGen.Click += new System.EventHandler(this.btnEcKeyGen_Click);
			// 
			// txtEnKey
			// 
			this.txtEnKey.Location = new System.Drawing.Point(47, 14);
			this.txtEnKey.Name = "txtEnKey";
			this.txtEnKey.Size = new System.Drawing.Size(378, 21);
			this.txtEnKey.TabIndex = 1;
			// 
			// btnEnc
			// 
			this.btnEnc.Location = new System.Drawing.Point(810, 48);
			this.btnEnc.Name = "btnEnc";
			this.btnEnc.Size = new System.Drawing.Size(57, 23);
			this.btnEnc.TabIndex = 9;
			this.btnEnc.Text = "암호화";
			this.btnEnc.UseVisualStyleBackColor = true;
			this.btnEnc.Click += new System.EventHandler(this.btnEnc_Click);
			// 
			// btnDec
			// 
			this.btnDec.Location = new System.Drawing.Point(879, 48);
			this.btnDec.Name = "btnDec";
			this.btnDec.Size = new System.Drawing.Size(57, 23);
			this.btnDec.TabIndex = 10;
			this.btnDec.Text = "복호화";
			this.btnDec.UseVisualStyleBackColor = true;
			this.btnDec.Click += new System.EventHandler(this.btnDec_Click);
			// 
			// Frmcryptography
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.ClientSize = new System.Drawing.Size(961, 322);
			this.Controls.Add(this.groupBox1);
			this.Name = "Frmcryptography";
			this.Text = "암호화/복호화 테스트";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtEnResult;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtEnSource;
		private System.Windows.Forms.Button btnEnIVGen;
		private System.Windows.Forms.TextBox txtEnIV;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnEcKeyGen;
		private System.Windows.Forms.TextBox txtEnKey;
		private System.Windows.Forms.Button btnEnc;
		private System.Windows.Forms.Button btnDec;
    }
}

