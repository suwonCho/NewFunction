namespace SocketPLC
{
    partial class frmMain
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
			this.btnQ_Tag = new System.Windows.Forms.Button();
			this.txtQ_Tag = new System.Windows.Forms.TextBox();
			this.txtMelsecA = new System.Windows.Forms.TextBox();
			this.btnMelsec_A = new System.Windows.Forms.Button();
			this.txtLSXGT = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnQ_Tag
			// 
			this.btnQ_Tag.Location = new System.Drawing.Point(218, 12);
			this.btnQ_Tag.Name = "btnQ_Tag";
			this.btnQ_Tag.Size = new System.Drawing.Size(122, 23);
			this.btnQ_Tag.TabIndex = 0;
			this.btnQ_Tag.Text = "Q_TagPulish";
			this.btnQ_Tag.UseVisualStyleBackColor = true;
			this.btnQ_Tag.Click += new System.EventHandler(this.btnQ_Tag_Click);
			// 
			// txtQ_Tag
			// 
			this.txtQ_Tag.Location = new System.Drawing.Point(12, 12);
			this.txtQ_Tag.Name = "txtQ_Tag";
			this.txtQ_Tag.Size = new System.Drawing.Size(200, 21);
			this.txtQ_Tag.TabIndex = 1;
			this.txtQ_Tag.Text = "Q_TagPublish";
			// 
			// txtMelsecA
			// 
			this.txtMelsecA.Location = new System.Drawing.Point(12, 41);
			this.txtMelsecA.Name = "txtMelsecA";
			this.txtMelsecA.Size = new System.Drawing.Size(200, 21);
			this.txtMelsecA.TabIndex = 3;
			this.txtMelsecA.Text = "Melsec_A";
			// 
			// btnMelsec_A
			// 
			this.btnMelsec_A.Location = new System.Drawing.Point(218, 41);
			this.btnMelsec_A.Name = "btnMelsec_A";
			this.btnMelsec_A.Size = new System.Drawing.Size(122, 23);
			this.btnMelsec_A.TabIndex = 2;
			this.btnMelsec_A.Text = "Melsec_A";
			this.btnMelsec_A.UseVisualStyleBackColor = true;
			this.btnMelsec_A.Click += new System.EventHandler(this.btnMelsec_A_Click);
			// 
			// txtLSXGT
			// 
			this.txtLSXGT.Location = new System.Drawing.Point(12, 70);
			this.txtLSXGT.Name = "txtLSXGT";
			this.txtLSXGT.Size = new System.Drawing.Size(200, 21);
			this.txtLSXGT.TabIndex = 5;
			this.txtLSXGT.Text = "LS_XGT";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(218, 70);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(122, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "LS XGT";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(352, 266);
			this.Controls.Add(this.txtLSXGT);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.txtMelsecA);
			this.Controls.Add(this.btnMelsec_A);
			this.Controls.Add(this.txtQ_Tag);
			this.Controls.Add(this.btnQ_Tag);
			this.Name = "frmMain";
			this.Text = "Main Form";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnQ_Tag;
        private System.Windows.Forms.TextBox txtQ_Tag;
		private System.Windows.Forms.TextBox txtMelsecA;
		private System.Windows.Forms.Button btnMelsec_A;
		private System.Windows.Forms.TextBox txtLSXGT;
		private System.Windows.Forms.Button button1;
    }
}