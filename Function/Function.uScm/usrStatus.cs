using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;


namespace Function.uScm
{
	/// <summary>
	/// usrStatus에 대한 요약 설명입니다.
	/// </summary>
	public class usrStatus : System.Windows.Forms.UserControl
	{
		public enum wError { None, OK, NG };

		private System.Windows.Forms.Label lblHeader;
		public System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.PictureBox picArrow;
		/// <summary> 
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public usrStatus()
		{
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
			InitializeComponent();

			// TODO: InitializeComponent를 호출한 다음 초기화 작업을 추가합니다.

		}

		/// <summary> 
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region 구성 요소 디자이너에서 생성한 코드
		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(usrStatus));
			this.lblHeader = new System.Windows.Forms.Label();
			this.lblInfo = new System.Windows.Forms.Label();
			this.picArrow = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.picArrow)).BeginInit();
			this.SuspendLayout();
			// 
			// lblHeader
			// 
			this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblHeader.BackColor = System.Drawing.Color.White;
			this.lblHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblHeader.Font = new System.Drawing.Font("굴림체", 11F, System.Drawing.FontStyle.Bold);
			this.lblHeader.Location = new System.Drawing.Point(0, 0);
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.Size = new System.Drawing.Size(116, 32);
			this.lblHeader.TabIndex = 0;
			this.lblHeader.Text = "헤더표시";
			this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblInfo
			// 
			this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblInfo.BackColor = System.Drawing.Color.Beige;
			this.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblInfo.Font = new System.Drawing.Font("굴림체", 14F, System.Drawing.FontStyle.Bold);
			this.lblInfo.Location = new System.Drawing.Point(0, 32);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(116, 56);
			this.lblInfo.TabIndex = 1;
			this.lblInfo.Text = "정보표시";
			this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// picArrow
			// 
			this.picArrow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.picArrow.Image = ((System.Drawing.Image)(resources.GetObject("picArrow.Image")));
			this.picArrow.Location = new System.Drawing.Point(116, 0);
			this.picArrow.Name = "picArrow";
			this.picArrow.Size = new System.Drawing.Size(28, 88);
			this.picArrow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picArrow.TabIndex = 2;
			this.picArrow.TabStop = false;
			// 
			// usrStatus
			// 
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.lblHeader);
			this.Controls.Add(this.picArrow);
			this.Font = new System.Drawing.Font("굴림체", 9F);
			this.Name = "usrStatus";
			this.Size = new System.Drawing.Size(144, 88);
			((System.ComponentModel.ISupportInitialize)(this.picArrow)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

	

		/// <summary>
		/// 헤더를 지정한다. 
		/// </summary>
		/// <param name="strHeader"></param>
		public void SetHeader(string strHeader)
		{
			Color color = Color.White;
			SetLabel(lblHeader, strHeader, color);
			SetStatus(string.Empty, wError.None);
		}

		public void SetStatus(string strInfo, wError uStatus)
		{
			Color color;
			Color color1;
		

			switch(uStatus)
			{
				case wError.NG:
					color = Color.Tomato;
					color1 = Color.Salmon;
					break;

				case wError.OK:
					color = Color.GreenYellow;
					color1 = Color.LightSteelBlue;
					break;

				default:
					color = Color.White;
					color1 = Color.Beige;
					break;

			}
			SetLabel(this.lblInfo, strInfo, color1);
			SetLabel(this.lblHeader, lblHeader.Text, color);

		}

		delegate void delSetLabel(Label lbl, string strText, Color color);

		private void SetLabel(Label lbl, string strText, Color color)
		{
			if (lbl.InvokeRequired)
			{
				lbl.Invoke(new delSetLabel(SetLabel), new object [] { lbl, strText, color } );
				return;
			}

			lbl.Text = strText;
			lbl.BackColor = color;
		}

		public bool isShowArrow
		{
			set 
			{
				this.picArrow.Visible = value;				
			}
		}

	}
}

