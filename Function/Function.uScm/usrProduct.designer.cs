namespace Function.uScm
{
    partial class usrProduct
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
			this.lblType = new System.Windows.Forms.Label();
			this.lblInfo = new System.Windows.Forms.Label();
			this.lblNo = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblType
			// 
			this.lblType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lblType.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.lblType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblType.Font = new System.Drawing.Font("굴림체", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblType.Location = new System.Drawing.Point(45, 5);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(110, 30);
			this.lblType.TabIndex = 0;
			this.lblType.Text = "TYPE";
			this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblInfo
			// 
			this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblInfo.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblInfo.Font = new System.Drawing.Font("굴림체", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblInfo.Location = new System.Drawing.Point(159, 5);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(236, 30);
			this.lblInfo.TabIndex = 1;
			this.lblInfo.Text = "INFO";
			this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblNo
			// 
			this.lblNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lblNo.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.lblNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblNo.Font = new System.Drawing.Font("굴림체", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblNo.Location = new System.Drawing.Point(5, 5);
			this.lblNo.Name = "lblNo";
			this.lblNo.Size = new System.Drawing.Size(37, 30);
			this.lblNo.TabIndex = 2;
			this.lblNo.Text = "No";
			this.lblNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// usrProduct
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblNo);
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.lblType);
			this.Font = new System.Drawing.Font("굴림체", 9F);
			this.Name = "usrProduct";
			this.Size = new System.Drawing.Size(398, 40);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblNo;
    }
}
