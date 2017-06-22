using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Function.form
{
	public partial class frmWorkBase : frmBaseForm
	{

		System.Threading.Timer tmr1Sec;


		bool _promptQuitPgm = false;

		[Description(@"프로그램 종료 시 확인 여부
			DialogResult = Ignore 설정 시 값에 상관 없이 물어 보지 않음")]
		public bool PromptQuitPgm
		{
			get { return _promptQuitPgm; }
			set
			{
				_promptQuitPgm = value;
			}
		}


		string _title_Label = "작업 이름";

		/// <summary>
		/// 폼 Title을 가져오거나 설정한다.
		/// </summary>
		public string Title_Label
		{
			get { return _title_Label; }
			set
			{
				_title_Label = value;
				Function.form.control.Invoke_Control_Text(lblTitle, _title_Label);
			}
		}



		public frmWorkBase()
		{
			InitializeComponent();

			tmr1Sec = new System.Threading.Timer(new TimerCallback(th1Sec), null, 0, 1000);
		}


		private void th1Sec(object obj)
		{
			string clo = DateTime.Now.ToString("yyyy년MM월dd일 HH:mm:ss");
			Function.form.control.Invoke_Control_Text(lblClock,clo);

			
		}

		private void frmWorkBase_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (DialogResult == System.Windows.Forms.DialogResult.Ignore) return;

			if(PromptQuitPgm)
			{
				if (Function.clsFunction.ShowMsg(this, "종료 확인", "프로그램을 종료 하시겠습니까?", frmMessage.enMessageType.YesNo)
				!= System.Windows.Forms.DialogResult.Yes) e.Cancel = true;
			}


		}
	}
}
