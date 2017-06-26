using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Function.form;
using Microsoft.Win32;

namespace Function
{

	/// <summary>
	/// Notice PopUP 창에서 버튼(ex: 닫힘) 눌렸을 경우 처리 delegate
	/// </summary>
	/// <param name="f"></param>
	/// <param name="command"></param>
	public delegate void delNoticePopUp_CMD(object sender, string command);

	/// <summary>
	/// 빈 델리게이트
	/// </summary>
	public delegate void delvoid_empt();

	/// <summary>
	/// Function 클래스에 추가로 기능 추가(프로젝트 위치는 Function.form)
	/// </summary>
	public partial class clsFunction
	{

		/// <summary>
		/// 메시지 창을 띄운다..
		/// </summary>
		/// <param name="win"></param>
		/// <param name="strMsg1"></param>
		/// <param name="strMsg2"></param>
		/// <param name="enType"></param>
		/// <returns></returns>
		public static DialogResult ShowMsg(IWin32Window win, string title, string message, form.frmMessage.enMessageType enType, int intSecClose = 0, DialogResult defalutResult = DialogResult.No)
		{

			if (win == null)
			{
				return ShowMsg(title, message, enType);
			}
			else
			{
				form.frmMessage frmMsg = new form.frmMessage(title, message, enType, intSecClose, defalutResult);

				if (((Form)win).InvokeRequired)
				{
					frmMsg.TopMost = true;

					return frmMsg.ShowDialog();
				}
				else
					return frmMsg.ShowDialog(win);
			}


		}


		public static DialogResult ShowMsg(IWin32Window win, string title, string message, form.frmMessage.enMessageType enType, out bool ApplyAll, int intSecClose = 0, DialogResult defalutResult = DialogResult.No)
		{


			if (win == null)
			{
				return ShowMsg(title, message, enType, out ApplyAll);
			}
			else
			{
				form.frmMessage frmMsg = new form.frmMessage(title, message, enType, intSecClose, defalutResult, true);

				if (((Form)win).InvokeRequired)
				{
					frmMsg.TopMost = true;

					frmMsg.ShowDialog();

					ApplyAll = frmMsg.ApplyAll;

					return frmMsg.DialogResult;
				}
				else
				{
					frmMsg.ShowDialog(win);

					ApplyAll = frmMsg.ApplyAll;

					return frmMsg.DialogResult;
				}
			}


		}




		public static DialogResult ShowMsg(string title, string message, form.frmMessage.enMessageType enType, int intSecClose =0, DialogResult defalutResult = DialogResult.No)
		{

			form.frmMessage frmMsg = new form.frmMessage(title, message, enType, intSecClose, defalutResult);
			return frmMsg.ShowDialog();
		}




		public static DialogResult ShowMsg(string title, string message, form.frmMessage.enMessageType enType, out bool ApplyAll, int intSecClose = 0, DialogResult defalutResult = DialogResult.No)
		{

			form.frmMessage frmMsg = new form.frmMessage(title, message, enType, intSecClose, defalutResult, true);

			frmMsg.ShowDialog();

			ApplyAll = frmMsg.ApplyAll;

			return frmMsg.DialogResult;
		}




		/// <summary>
		/// 패스워드 입력 창을 띄우고 암호를 입력 받는다
		/// </summary>
		/// <param name="title"></param>
		/// <param name="passwords"></param>
		/// <returns></returns>
		public static bool CheckPasswords(string title, string passwords)
		{
			Function.form.frmPasswords frm = new Function.form.frmPasswords(title, passwords);
			return (frm.ShowDialog() == DialogResult.OK);

		}

		delegate void delNoticePopUp(string msg, int monitor = 0, System.Drawing.ContentAlignment align = System.Drawing.ContentAlignment.BottomRight
			, int autoclose = 0, Form frm = null, delNoticePopUp_CMD cmd = null);

		

		/// <summary>
		/// 화면에 메시지 창을 띄운다.
		/// </summary>
		/// <param name="msg">메시지</param>
		/// <param name="monitor">띄울 모니터 번호</param>
		/// <param name="align">창 위치</param>
		/// <param name="autoclose">자동을 닫힐 시간 ms, 0:자동으로 안 닫힘</param>
		public static void NoticePopUp(string msg, int monitor = 0, System.Drawing.ContentAlignment align = System.Drawing.ContentAlignment.BottomRight
			, int autoclose = 0, Form frm = null, delNoticePopUp_CMD cmd = null)
		{

			if(frm != null)
			{
				if(frm.InvokeRequired)
				{
					frm.Invoke(new delNoticePopUp(NoticePopUp), msg, monitor, align, autoclose, frm, cmd);
					return;
				}
			}

			Function.form.popNotice p = new form.popNotice(msg, monitor, align, autoclose);

			p.OnNoticePopUp_CMD = cmd;

			if (frm != null)
				p.Show(frm);
			else
				p.Show();
		}


		static Function.form.frmWating frm_wait = null;
		/// <summary>
		/// 대기 요청창을 띄운다
		/// </summary>
		/// <param name="win"></param>
		/// <param name="msg"></param>
		public static void WaitForm_Show(IWin32Window win, string msg = "잠시만 기다려 주십시요.")
		{
			WaitForm_Close();

			frm_wait = new form.frmWating(msg);
			frm_wait.ShowDialog(win);			
		}


		/// <summary>
		/// 대기 요청 창을 닫는다.
		/// </summary>
		public static void WaitForm_Close()
		{
			if (frm_wait == null) return;

			try
			{
				Function.form.control.Invoke_Form_Close(frm_wait);
			}
			catch { }
			
		}

		/// <summary>
		/// 폼의 위치를 조정 하여 준다.
		/// </summary>
		/// <param name="frm"></param>
		/// <param name="type">조정할 기준 타입</param>
		public static void From_PositionSet(Form frm, Function.form.enFormPositionType type)
		{
			int x = 0;
			int y=0;

			//기준 위치
			int cX = 0;
			int cY = 0;

			try
			{
				switch(type)
				{

					case enFormPositionType.MousePosition_Center:
						api.POINT pnt = new api.POINT();
						api.GetCursorPos(ref pnt);
						
						x = pnt.X - (frm.Width / 2);
						y = pnt.Y - (frm.Height / 2);

						cX = pnt.X;
						cY = pnt.Y;

						break;


				}

				int scrNo = ScreenNumGet(cX, cY);

				//스크린 밖으로 창이 나가 지 않도록
				if (scrNo >= 0)
				{
					Screen scr = Screen.AllScreens[scrNo];

					if (x < scr.Bounds.Left)
						x = scr.Bounds.Left;
					else if ((x + frm.Width) > scr.Bounds.Right)
						x = scr.Bounds.Right - frm.Width;


					if (y < scr.Bounds.Top)
						y = scr.Bounds.Top;
					else if ((y+frm.Height) > scr.Bounds.Bottom)
						y = scr.Bounds.Bottom - frm.Height;

				}

				frm.Left = x;
				frm.Top = y;

			}
			catch
			{

			}
		}


		/// <summary>
		/// 화면(모니터) 번호를 구한다.-현재 마우스 위치 기준
		/// </summary>
		/// <returns></returns>
		public static int ScreenNumGet()
		{
			api.POINT pnt = new api.POINT();
			api.GetCursorPos(ref pnt);

			return ScreenNumGet(pnt.X, pnt.Y);
		}

		/// <summary>
		/// 화면(모니터) 번호를 구한다. 
		/// </summary>
		/// <returns>-1 이면 찾지 못한 경우</returns>
		public static int ScreenNumGet(int x, int y)
		{
			int idx = 0;
			int sc = -1;
			foreach(Screen scr in Screen.AllScreens)
			{
				if(scr.Bounds.Top <= y && scr.Bounds.Bottom >= y &&
					scr.Bounds.Left <= x && scr.Bounds.Right >= x)
				{
					sc = idx;
					break;
				}


				idx++;
			}
			return sc;
		}


		/// <summary>
		/// 시작프로그램 등록 여부를 가져 옵니다.
		/// </summary>
		/// <param name="key">프로그램명</param>
		/// <param name="value">프로그램 실행 경로 ex)Application.ExecutablePath</param>
		/// <returns></returns>
		public static bool StartUpPgm_isReg(string key, string value)
		{
			RegistryKey k = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
						
			object v = k.GetValue(key);

			bool rtn = v != null && v.ToString().ToUpper().Equals(value.ToUpper());

			return rtn;

		}

		/// <summary>
		/// 시작프로그램 등록 하거나 삭제 합니다.
		/// </summary>
		/// <param name="isReg">[true]등록 [false]해제</param>
		/// <param name="key">프로그램명</param>
		/// <param name="value">프로그램 실행 경로 ex)Application.ExecutablePath</param>
		public static void StartUpPgm_Reg(bool isReg, string key, string value)
		{

			RegistryKey k = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
			
			if(isReg)
			{
				k.SetValue(key, value);
			}
			else
			{
				if(StartUpPgm_isReg(key,value))	k.DeleteValue(key);
			}
		}

		/// <summary>
		/// Container 및의 userInputBox의 값을 Commit 한다. Text -> Value
		/// </summary>
		/// <param name="container"></param>
		public static void InputBox_Commit(Control container)
		{
			inputBox_Commit(container);
		}

		private static void inputBox_Commit(Control ctrl)
		{
			usrInputBox i = ctrl as usrInputBox;

			if(i != null)
			{
				i.Commit();
				return;
			}


			foreach(Control c in ctrl.Controls)
			{
				inputBox_Commit(c);
			}

		}


	}	//end class
}
