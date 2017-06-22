using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Function.form
{
	public partial class frmWating : Form
	{
		string msg = "잠시만 기다려 주십시요.";


		/// <summary>
		/// 대기 창
		/// </summary>
		public frmWating()
		{
			InitializeComponent();
		}


		/// <summary>
		/// 대기 창을 초기화 화면서 메시지를 설정한다.
		/// </summary>
		/// <param name="_msg"></param>
		public frmWating(string _msg)
		{
			InitializeComponent();

			Msg = _msg;
		}



		/// <summary>
		/// 창에 표시되는 메지지를 가지고 오거나 설정합니다.
		/// </summary>
		public string Msg
		{
			get
			{
				return msg;
			}

			set
			{
				msg = value;
			}
		}

		private void frmWating_Load(object sender, EventArgs e)
		{
			//bit = Function.resIcon16.loading_002;
			lblMsg.Text = msg;
		}


		Bitmap bit;

		protected override void OnLoad(EventArgs e)
		{
			bit = new Bitmap("loading_002.gif");
			ImageAnimator.Animate(bit, new EventHandler(this.OnFrameChanged));
			base.OnLoad(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			ImageAnimator.UpdateFrames();
			Graphics g = pic.CreateGraphics();
			g.DrawImage(this.bit, new Point(0, 0));
			base.OnPaint(e);
		}

		private void OnFrameChanged(object sender, EventArgs e)
		{
			this.Invalidate();
		}
		




	}
}
