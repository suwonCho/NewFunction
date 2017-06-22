namespace Function.form
{
	partial class btnAdvToggleButton
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(btnAdvToggleButton));
			this.lblText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblText
			// 
			this.lblText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblText.Font = new System.Drawing.Font("굴림", 9F);
			this.lblText.Location = new System.Drawing.Point(0, 0);
			this.lblText.Name = "lblText";
			this.lblText.Size = new System.Drawing.Size(87, 45);
			this.lblText.TabIndex = 0;
			this.lblText.Text = "O N";
			this.lblText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblText.Visible = false;			
			this.lblText.DoubleClick += new System.EventHandler(this.lblText_DoubleClick);
			// 
			// btnAdvToggleButton
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.Controls.Add(this.lblText);
			this.DoubleBuffered = true;
			this.Name = "btnAdvToggleButton";
			this.Size = new System.Drawing.Size(87, 45);
			this.FontChanged += new System.EventHandler(this.btnToggleButton_FontChanged);			
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblText;

	}
}
