using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Function.form.UserControls
{
	public partial class usrPleaseWait : UserControl
	{

		private string _text;

		/// <summary>
		/// 안내 문구를 가지고 오거나 설정합니다.
		/// </summary>
		[Description("안내 문구를 가지고 오거나 설정합니다.")]
		[DefaultValue(typeof(System.String), "초기화 중 입니다. 잠시만 기다려 주십시요.")]
		[Browsable(true)]
		public string Text
		{
			get
			{
				return _text;
			}

			set
			{
				_text = value;

				label1.Text = _text;
			}
		}



		public usrPleaseWait()
		{
			InitializeComponent();
			
		}

		private void usrPleaseWait_Load(object sender, EventArgs e)
		{
			usrGifPicbox1.Image = Function.resIcon16.loading_004;
		}
	}
}
