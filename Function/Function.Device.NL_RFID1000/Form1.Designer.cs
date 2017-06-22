namespace Function.Device.NL_RFID1000
{
	partial class Form1
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
			this.btnConn = new System.Windows.Forms.Button();
			this.btnDisconn = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnEnd = new System.Windows.Forms.Button();
			this.txtConn = new System.Windows.Forms.TextBox();
			this.txtTag = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnConn
			// 
			this.btnConn.Location = new System.Drawing.Point(12, 12);
			this.btnConn.Name = "btnConn";
			this.btnConn.Size = new System.Drawing.Size(75, 23);
			this.btnConn.TabIndex = 0;
			this.btnConn.Text = "연결";
			this.btnConn.UseVisualStyleBackColor = true;
			this.btnConn.Click += new System.EventHandler(this.btnConn_Click);
			// 
			// btnDisconn
			// 
			this.btnDisconn.Location = new System.Drawing.Point(105, 12);
			this.btnDisconn.Name = "btnDisconn";
			this.btnDisconn.Size = new System.Drawing.Size(75, 23);
			this.btnDisconn.TabIndex = 1;
			this.btnDisconn.Text = "연결해제";
			this.btnDisconn.UseVisualStyleBackColor = true;
			this.btnDisconn.Click += new System.EventHandler(this.btnDisconn_Click);
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(12, 41);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 2;
			this.btnStart.Text = "리딩 시작";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnEnd
			// 
			this.btnEnd.Location = new System.Drawing.Point(105, 41);
			this.btnEnd.Name = "btnEnd";
			this.btnEnd.Size = new System.Drawing.Size(75, 23);
			this.btnEnd.TabIndex = 3;
			this.btnEnd.Text = "리딩 종료";
			this.btnEnd.UseVisualStyleBackColor = true;
			this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
			// 
			// txtConn
			// 
			this.txtConn.Location = new System.Drawing.Point(197, 14);
			this.txtConn.Name = "txtConn";
			this.txtConn.Size = new System.Drawing.Size(304, 21);
			this.txtConn.TabIndex = 4;
			// 
			// txtTag
			// 
			this.txtTag.Location = new System.Drawing.Point(12, 81);
			this.txtTag.Name = "txtTag";
			this.txtTag.Size = new System.Drawing.Size(579, 21);
			this.txtTag.TabIndex = 5;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(603, 262);
			this.Controls.Add(this.txtTag);
			this.Controls.Add(this.txtConn);
			this.Controls.Add(this.btnEnd);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.btnDisconn);
			this.Controls.Add(this.btnConn);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnConn;
		private System.Windows.Forms.Button btnDisconn;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnEnd;
		private System.Windows.Forms.TextBox txtConn;
		private System.Windows.Forms.TextBox txtTag;
	}
}

