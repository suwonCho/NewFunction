namespace Function.form
{
	partial class usrInputBox
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
			this.txtBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lblText = new System.Windows.Forms.Label();
			this.cmbBox = new System.Windows.Forms.ComboBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.label = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtBox
			// 
			this.txtBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtBox.Location = new System.Drawing.Point(0, 0);
			this.txtBox.Name = "txtBox";
			this.txtBox.Size = new System.Drawing.Size(183, 21);
			this.txtBox.TabIndex = 0;
			this.txtBox.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Right;
			this.label1.Location = new System.Drawing.Point(183, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(11, 24);
			this.label1.TabIndex = 1;
			this.label1.Text = "*";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblText
			// 
			this.lblText.BackColor = System.Drawing.Color.Transparent;
			this.lblText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblText.Location = new System.Drawing.Point(0, 0);
			this.lblText.Name = "lblText";
			this.lblText.Size = new System.Drawing.Size(183, 24);
			this.lblText.TabIndex = 2;
			this.lblText.Text = "label2";
			this.lblText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblText.Visible = false;
			this.lblText.Click += new System.EventHandler(this.lblText_Click);
			// 
			// cmbBox
			// 
			this.cmbBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cmbBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbBox.FormattingEnabled = true;
			this.cmbBox.Location = new System.Drawing.Point(0, 0);
			this.cmbBox.Name = "cmbBox";
			this.cmbBox.Size = new System.Drawing.Size(183, 20);
			this.cmbBox.TabIndex = 3;
			this.cmbBox.Visible = false;
			this.cmbBox.SelectedIndexChanged += new System.EventHandler(this.cmbBox_SelectedIndexChanged);
			// 
			// splitContainer1
			// 
			this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.label);
			this.splitContainer1.Panel1MinSize = 1;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.cmbBox);
			this.splitContainer1.Panel2.Controls.Add(this.lblText);
			this.splitContainer1.Panel2.Controls.Add(this.txtBox);
			this.splitContainer1.Panel2.Controls.Add(this.label1);
			this.splitContainer1.Panel2MinSize = 1;
			this.splitContainer1.Size = new System.Drawing.Size(248, 24);
			this.splitContainer1.TabIndex = 4;
			this.splitContainer1.TabStop = false;
			// 
			// label
			// 
			this.label.BackColor = System.Drawing.Color.Transparent;
			this.label.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label.Location = new System.Drawing.Point(0, 0);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(50, 24);
			this.label.TabIndex = 1;
			this.label.Text = "Label";
			this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label.Click += new System.EventHandler(this.label_Click);
			// 
			// usrInputBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.splitContainer1);
			this.Name = "usrInputBox";
			this.Size = new System.Drawing.Size(248, 24);
			this.Load += new System.EventHandler(this.usrInputBox_Load);
			this.SizeChanged += new System.EventHandler(this.usrInputBox_SizeChanged);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label;
		protected System.Windows.Forms.TextBox txtBox;
		protected System.Windows.Forms.Label lblText;
		protected System.Windows.Forms.SplitContainer splitContainer1;
		protected System.Windows.Forms.Label label1;
		protected System.Windows.Forms.ComboBox cmbBox;
	}
}
