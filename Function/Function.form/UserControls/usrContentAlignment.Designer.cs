namespace Function.form.UserControls
{
	partial class usrContentAlignment
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
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
		/// </summary>
		private void InitializeComponent()
		{
			this.TopLeft = new System.Windows.Forms.Button();
			this.TopCenter = new System.Windows.Forms.Button();
			this.TopRight = new System.Windows.Forms.Button();
			this.MiddleLeft = new System.Windows.Forms.Button();
			this.MiddleCenter = new System.Windows.Forms.Button();
			this.MiddleRight = new System.Windows.Forms.Button();
			this.BottomLeft = new System.Windows.Forms.Button();
			this.BottomCenter = new System.Windows.Forms.Button();
			this.BottomRight = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// TopLeft
			// 
			this.TopLeft.Location = new System.Drawing.Point(0, 0);
			this.TopLeft.Name = "TopLeft";
			this.TopLeft.Size = new System.Drawing.Size(40, 40);
			this.TopLeft.TabIndex = 0;
			this.TopLeft.Text = "↖";
			this.TopLeft.UseVisualStyleBackColor = true;
			this.TopLeft.Click += new System.EventHandler(this.TopLeft_Click);
			// 
			// TopCenter
			// 
			this.TopCenter.Location = new System.Drawing.Point(42, 0);
			this.TopCenter.Name = "TopCenter";
			this.TopCenter.Size = new System.Drawing.Size(40, 40);
			this.TopCenter.TabIndex = 1;
			this.TopCenter.Text = "↑";
			this.TopCenter.UseVisualStyleBackColor = true;
			this.TopCenter.Click += new System.EventHandler(this.TopLeft_Click);
			// 
			// TopRight
			// 
			this.TopRight.Location = new System.Drawing.Point(88, 0);
			this.TopRight.Name = "TopRight";
			this.TopRight.Size = new System.Drawing.Size(40, 40);
			this.TopRight.TabIndex = 2;
			this.TopRight.Text = "↗";
			this.TopRight.UseVisualStyleBackColor = true;
			this.TopRight.Click += new System.EventHandler(this.TopLeft_Click);
			// 
			// MiddleLeft
			// 
			this.MiddleLeft.Location = new System.Drawing.Point(0, 46);
			this.MiddleLeft.Name = "MiddleLeft";
			this.MiddleLeft.Size = new System.Drawing.Size(40, 40);
			this.MiddleLeft.TabIndex = 3;
			this.MiddleLeft.Text = "←";
			this.MiddleLeft.UseVisualStyleBackColor = true;
			this.MiddleLeft.Click += new System.EventHandler(this.TopLeft_Click);
			// 
			// MiddleCenter
			// 
			this.MiddleCenter.Location = new System.Drawing.Point(42, 46);
			this.MiddleCenter.Name = "MiddleCenter";
			this.MiddleCenter.Size = new System.Drawing.Size(40, 40);
			this.MiddleCenter.TabIndex = 4;
			this.MiddleCenter.Text = "●";
			this.MiddleCenter.UseVisualStyleBackColor = true;
			this.MiddleCenter.Click += new System.EventHandler(this.TopLeft_Click);
			// 
			// MiddleRight
			// 
			this.MiddleRight.Location = new System.Drawing.Point(88, 46);
			this.MiddleRight.Name = "MiddleRight";
			this.MiddleRight.Size = new System.Drawing.Size(40, 40);
			this.MiddleRight.TabIndex = 5;
			this.MiddleRight.Text = "→";
			this.MiddleRight.UseVisualStyleBackColor = true;
			this.MiddleRight.Click += new System.EventHandler(this.TopLeft_Click);
			// 
			// BottomLeft
			// 
			this.BottomLeft.Location = new System.Drawing.Point(0, 92);
			this.BottomLeft.Name = "BottomLeft";
			this.BottomLeft.Size = new System.Drawing.Size(40, 40);
			this.BottomLeft.TabIndex = 6;
			this.BottomLeft.Text = "↙";
			this.BottomLeft.UseVisualStyleBackColor = true;
			this.BottomLeft.Click += new System.EventHandler(this.TopLeft_Click);
			// 
			// BottomCenter
			// 
			this.BottomCenter.Location = new System.Drawing.Point(42, 92);
			this.BottomCenter.Name = "BottomCenter";
			this.BottomCenter.Size = new System.Drawing.Size(40, 40);
			this.BottomCenter.TabIndex = 7;
			this.BottomCenter.Text = "↓";
			this.BottomCenter.UseVisualStyleBackColor = true;
			this.BottomCenter.Click += new System.EventHandler(this.TopLeft_Click);
			// 
			// BottomRight
			// 
			this.BottomRight.Location = new System.Drawing.Point(88, 92);
			this.BottomRight.Name = "BottomRight";
			this.BottomRight.Size = new System.Drawing.Size(40, 40);
			this.BottomRight.TabIndex = 8;
			this.BottomRight.Text = "↘";
			this.BottomRight.UseVisualStyleBackColor = true;
			this.BottomRight.Click += new System.EventHandler(this.TopLeft_Click);
			// 
			// usrContentAlignment
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.BottomRight);
			this.Controls.Add(this.BottomCenter);
			this.Controls.Add(this.BottomLeft);
			this.Controls.Add(this.MiddleRight);
			this.Controls.Add(this.MiddleCenter);
			this.Controls.Add(this.MiddleLeft);
			this.Controls.Add(this.TopRight);
			this.Controls.Add(this.TopCenter);
			this.Controls.Add(this.TopLeft);
			this.Name = "usrContentAlignment";
			this.Load += new System.EventHandler(this.usrContentAlignment_Load);
			this.SizeChanged += new System.EventHandler(this.usrContentAlignment_SizeChanged);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button TopLeft;
		private System.Windows.Forms.Button TopCenter;
		private System.Windows.Forms.Button TopRight;
		private System.Windows.Forms.Button MiddleLeft;
		private System.Windows.Forms.Button MiddleCenter;
		private System.Windows.Forms.Button MiddleRight;
		private System.Windows.Forms.Button BottomLeft;
		private System.Windows.Forms.Button BottomCenter;
		private System.Windows.Forms.Button BottomRight;
	}
}
