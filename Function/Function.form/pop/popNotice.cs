using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Function;
using Function.form;
using System.Threading;

namespace Function.form
{
	public partial class popNotice : Form // Function.form.subBaseForm
	{
		public delegate void delpopNotice(DataRow item, int monitor, ContentAlignment align, int AutoClose);

		//lblLink bit : 0 닫기, 1 해제, 2 항목링크, 3 목록 링크
		int bitClose = 0;
		int bitArmOff = 1;
		int bitItemLink = 2;
		int bitListLink = 3;
		int _autoClose = 0;

		int t = 0;
		int l = 0;
		int ss = 10;
		int lf = -1, lt = -1, ls = 1;
		int tf = -1, tt = -1, ts = 1;


		Thread thMoveForm = null;

		string _msg;
		ContentAlignment _align;
		int _monitor;

		public Function.delNoticePopUp_CMD OnNoticePopUp_CMD = null;


		/// <summary>
		///  
		/// </summary>
		/// <param name="msg">보여줄 메시지</param>
		/// <param name="monitor">모니터 번호</param>
		/// <param name="align">메시지 창 위치</param>
		/// <param name="AutoClose">자동 닫기(초) 0이면 사용자가 닫는다.</param>
		public popNotice(string msg, int monitor, ContentAlignment align, int AutoClose)
		{
			InitializeComponent();

			SetNotice(msg, monitor, align, AutoClose);

			Rectangle[] nScreen;
		}

		public void SetNotice(string msg, int monitor, ContentAlignment align, int AutoClose)
		{
			_msg = msg;
			_align = align;
			_monitor = monitor;

			_autoClose = AutoClose;
		}


		private void popNotice_Load(object sender, EventArgs e)
		{

			Notice_RePop();
		}

		public void  Notice_RePop()
		{
			Screen scr;

			if(thMoveForm != null && thMoveForm.IsAlive)
			{
				thMoveForm.Abort();
				thMoveForm.Join(150);
			}


			if (_monitor > -1 && Screen.AllScreens.Length > _monitor)
				scr = Screen.AllScreens[_monitor];
			else
				scr = Screen.PrimaryScreen;

			if (_msg != null)
			{
				lblText.Text = _msg;
			}

			t = 0;
			l = 0;
			ss = 10;
			lf = -1; lt = -1; ls = 1;
			tf = -1; tt = -1; ts = 1;

			//top 계산
			switch (_align)
			{
				case ContentAlignment.TopCenter:
				case ContentAlignment.TopLeft:
				case ContentAlignment.TopRight:
					t = scr.WorkingArea.Y -  this.Height;
					tf = t;
					tt = scr.WorkingArea.Y;
					ts = ss;
					break;

				case ContentAlignment.MiddleCenter:
				case ContentAlignment.MiddleLeft:
				case ContentAlignment.MiddleRight:
					t = scr.WorkingArea.Y + (scr.WorkingArea.Height / 2) - (this.Height / 2);
					break;

				case ContentAlignment.BottomCenter:
				case ContentAlignment.BottomLeft:
				case ContentAlignment.BottomRight:
					t = scr.WorkingArea.Height;
					tf = t;
					tt = t - this.Height;
					ts = ss * -1;
					break;					

			}

			//left계산
			switch(_align)
			{
				case ContentAlignment.BottomLeft:
				case ContentAlignment.TopLeft:
					l = scr.WorkingArea.X;
					break;

				case ContentAlignment.MiddleLeft:
					l = scr.WorkingArea.X - this.Width;
					lf = l;
					lt = scr.WorkingArea.X;
					ls = ss;
					break;

				case ContentAlignment.BottomRight:
				case ContentAlignment.TopRight:
					l = scr.WorkingArea.X + scr.WorkingArea.Width - this.Width;					
					break;

				case ContentAlignment.MiddleRight:
					l = scr.WorkingArea.Width;
					lf = l;
					lt = l - this.Width;
					ls = ss * -1;
					break;


				case ContentAlignment.BottomCenter:
				case ContentAlignment.MiddleCenter:
				case ContentAlignment.TopCenter:
					l = scr.WorkingArea.X + (scr.WorkingArea.Width / 2) - (this.Width / 2);
					break;
			}

			this.Top = t;
			this.Left = l;


			thMoveForm = new Thread(new ThreadStart(doMoveForm));
			thMoveForm.IsBackground = true;
			thMoveForm.Start();
			

		}


		public void doMoveForm()
		{
			try
			{
				int term = 15;

				if(lf != lt)
				{
					ls = (lt - lf) / 30;
				}


				for (int fl = lf; (ls > 0 ? fl < lt : fl > lt); fl += ls)
				{
					Function.form.control.Invoke_Control_Left(this, fl);
					//this.Left = fl;
					Thread.Sleep(term);

					//Application.DoEvents();
				}

				if (lt != -1) Function.form.control.Invoke_Control_Left(this, lt);
				


				if (tf != tt)
				{
					ts = (tt - tf) / 30;
				}


				for (int ft = tf; (ts > 0 ? ft < tt : ft > tt); ft += ts)
				{
					Function.form.control.Invoke_Control_Top(this, ft);
					//this.Top = ft;
					Thread.Sleep(term);

					//Application.DoEvents();
				}

				if (tt != -1) Function.form.control.Invoke_Control_Top(this, tt); 


				if(_autoClose > 0)
				{
					Thread.Sleep(_autoClose * 1000);

					Function.form.control.Invoke_Form_Close(this);
				}

			}
			catch
			{

			}
		}



		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			
			this.Close();
			//lk.TextAlign

			if (OnNoticePopUp_CMD != null)
			{
				OnNoticePopUp_CMD(this, "Close");
			}

		}

	}
}
