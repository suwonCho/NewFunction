namespace Function.form
{
	partial class usrEditLabel
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
			this.panelbody = new System.Windows.Forms.Panel();
			this.label = new System.Windows.Forms.Label();
			this.textBox = new System.Windows.Forms.TextBox();
			this.pnlleft = new System.Windows.Forms.Panel();
			this.pnlRight = new System.Windows.Forms.Panel();
			this.panelbody.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelbody
			// 
			this.panelbody.BackColor = System.Drawing.Color.Transparent;
			this.panelbody.Controls.Add(this.label);
			this.panelbody.Controls.Add(this.textBox);
			this.panelbody.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelbody.Location = new System.Drawing.Point(0, 0);
			this.panelbody.Name = "panelbody";
			this.panelbody.Size = new System.Drawing.Size(154, 30);
			this.panelbody.TabIndex = 0;
			// 
			// label
			// 
			this.label.BackColor = System.Drawing.Color.Transparent;
			this.label.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label.Location = new System.Drawing.Point(0, 0);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(154, 30);
			this.label.TabIndex = 1;
			this.label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label.DoubleClick += new System.EventHandler(this.label_DoubleClick);
			// 
			// textBox
			// 
			this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox.Location = new System.Drawing.Point(0, 0);
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(154, 30);
			this.textBox.TabIndex = 0;
			this.textBox.Visible = false;
			this.textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
			// 
			// pnlleft
			// 
			this.pnlleft.BackColor = System.Drawing.Color.Transparent;
			this.pnlleft.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlleft.Location = new System.Drawing.Point(0, 0);
			this.pnlleft.Name = "pnlleft";
			this.pnlleft.Size = new System.Drawing.Size(0, 30);
			this.pnlleft.TabIndex = 1;
			// 
			// pnlRight
			// 
			this.pnlRight.BackColor = System.Drawing.Color.Transparent;
			this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlRight.Location = new System.Drawing.Point(154, 0);
			this.pnlRight.Name = "pnlRight";
			this.pnlRight.Size = new System.Drawing.Size(0, 30);
			this.pnlRight.TabIndex = 2;
			// 
			// usrEditLabel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panelbody);
			this.Controls.Add(this.pnlRight);
			this.Controls.Add(this.pnlleft);
			this.Name = "usrEditLabel";
			this.Size = new System.Drawing.Size(154, 30);
			this.FontChanged += new System.EventHandler(this.itmEditLabel_FontChanged);
			this.panelbody.ResumeLayout(false);
			this.panelbody.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelbody;
		private System.Windows.Forms.Label label;
		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.Panel pnlleft;
		private System.Windows.Forms.Panel pnlRight;
	}
}
