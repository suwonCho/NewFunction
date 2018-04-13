using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Function;


namespace Function.form
{
	/// <summary>
	/// 기본 폼 스타일 
	/// 폼상태 저장을 위해서는 SavePosition_Setting 변수에 사용할 프로퍼티 클래스를 할당하고<para/>
	/// (ex)SavePosition_Setting = Properties.Settings.Default / [변수]SavePosition = true 할당<para/>
	///		프로퍼티 클래스에 BF_FORMPOSITION항목 추가한다(형식은 string) 
	/// </summary>
	public partial class frmBaseForm : clsbaseForm
	{
		
		public enum enBaseFormStyle
		{
			Normal,
			toolbox
		}

		enBaseFormStyle _baseFormStyle = enBaseFormStyle.Normal;

		public enBaseFormStyle BaseFormStyle
		{
			get { return _baseFormStyle; }
			set
			{
				if (_baseFormStyle == value) return;

				_baseFormStyle = value;

				switch (_baseFormStyle)
				{
					case  enBaseFormStyle.Normal:
						pgrBar.Visible = true;
						this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
						break;

					case enBaseFormStyle.toolbox:
						pgrBar.Visible = false;
						this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
						break;
				}

			}
		}


		bool _showStatusBar = true;

		/// <summary>
		/// 상태창 표시 여부를 가져온다.
		/// </summary>
		public bool ShowStatusBar
		{
			get { return _showStatusBar; }
			set
			{
				_showStatusBar = value;
				stBar.Visible = _showStatusBar;
			}
		}


		/// <summary>
		/// 마지막 메세지 내용
		/// </summary>		
		public string LastMessage { get; set; }
		/// <summary>
		/// 마지막 메세지 에러 여부
		/// </summary>
		public bool LastMessageError { get; set; }

		
		/// <summary>
		/// Form에 현재시간 부분 업데이트
		/// </summary>
		protected System.Threading.Timer tmrDtNow;
		/// <summary>
		/// 폼 시작 시간.
		/// </summary>
		protected DateTime dtFormStart;

		/// <summary>
		/// 프로그램 이름
		/// </summary>
		string pgmName = string.Empty;


		/// <summary>
		/// 프로그램 이름을 설정 하거나 가저 옵니다.
		/// </summary>
		[Description("프로그램 이름을 설정 하거나 가저 옵니다.")]
		public string PgmName
		{
			get
			{
				return pgmName;
			}
			set
			{
				pgmName = value;
			}
		}


		/// <summary>
		/// 메세지 표시 델리게이트 : 타 폼이나 프로그램에서 폼에 메세지 표시시 이용.
		/// </summary>
		/// <param name="msgType"></param>
		/// <param name="strMessage"></param>
		/// <param name="isLog"></param>		
		public delegate void delSetMessage(enMsgType msgType, string strMessage, bool isLog);
		/// <summary>
		/// 에러 처리 델리게이트 : 타 폼이나 프로그램에서 모에 에러 처리 이용시..
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="strMethodName"></param>
		public delegate void delProcException(Exception ex, string strMethodName, bool isLog);

		/// <summary>
		/// 로그 클래스 -> 사용 폼에서 객체 생성을 해주어야 한다.
		/// </summary>
		protected Function.Util.Log clsLog;

		public delegate void delMessageChanged(bool isError, string strMessage);
		
		event delMessageChanged _onMessageChanged;

		/// <summary>
		/// 폼 닫임 여부 처리.
		/// </summary>
		protected bool _isFormClose = false;

		/// <summary>
		/// 노티파이아이퀀 컨택스트 메뉴
		/// </summary>
		protected ContextMenu NotifyContextMenu;


		/// <summary>
		/// 메시지 변경시 발생하는 이벤트
		/// </summary>
		public event delMessageChanged OnMessageChanged
		{
			add { _onMessageChanged += value; }
			remove { _onMessageChanged -= value; }
		}

		/// <summary>
		/// 창을 닫으면 창을 최소화 여부를 가져오거나 설정 합니다.
		/// </summary>
		bool _closeToMinimize = false;

		/// <summary>
		/// 창을 닫으면 창을 최소화 여부를 가져오거나 설정 합니다.</para>
		/// 창을 종료 할 경우 _isFormClose = true 로 설정후 종료 처리
		/// </summary>
		[Description("창을 닫으면 창을 최소화 여부를 가져오거나 설정 합니다.")]
		public bool CloseToMinimize
		{
			get{ return _closeToMinimize; }
			set
			{
				_closeToMinimize = value;
			}
		}


		/// <summary>
		/// 최소화 할 때 테스크바의 아이콘 숨김여부를 가져오거나 설정 합니다.
		/// </summary>
		bool _hideIconWhenMinimized = false;

		/// <summary>
		/// 최소화 할 때 테스크바의 아이콘 숨김여부를 가져오거나 설정 합니다.		
		/// </summary>
		[Description("최소화 할 때 테스크바의 아이콘 숨김여부를 가져오거나 설정 합니다.")]
		public bool HideIconWhenMinimized
		{
			get { return _hideIconWhenMinimized; }
			set
			{
				_hideIconWhenMinimized = value;
			}
		}

		
		public frmBaseForm()
		{
			LastMessage = string.Empty;

			InitializeComponent();

			dtFormStart = DateTime.Now;			
			//this.Icon = (System.Drawing.Icon)(Function.resIcon16.monitor_on.);

			this.MdiChildActivate += new EventHandler(frmBaseForm_MdiChildActivate);



		}

		/// <summary>
		/// 컨텍스트 메뉴에서 종료 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void M_Click(object sender, EventArgs e)
		{
			_isFormClose = true;
			this.Close();
		}


		void frmBaseForm_MdiChildActivate(object sender, EventArgs e)
		{
			subBaseForm frm = ActiveMdiChild as subBaseForm;
			if (frm == null) return;

			SetMessage_TextOnly(LastMessageError ? enMsgType.Error : enMsgType.None, LastMessage, false);

			//if(frm._onProcException == null) frm._onProcException = new delProcException(ProcException);
			//if(frm._onSetMessage == null) frm._onSetMessage = new delSetMessage(SetMessage);
			//if (frm._onSetLastMessage == null) frm._onSetLastMessage = new delSetMessage(SetLastMessage); 
		}


		private void frmBasic_Load(object sender, EventArgs e)
		{
			frmBaseForm_MdiChildActivate(this, null);


			//Nofyicon init
			if (Notifyicon_Visible && CloseToMinimize)
			{
				NotifyContextMenu = new ContextMenu();

				MenuItem m = NotifyContextMenu.MenuItems.Add("종료...");
				m.Click += M_Click;

				Notifyicon.ContextMenu = NotifyContextMenu;
			}

			stBar.DoubleClick += new EventHandler(stBar_DoubleClick);
			tmrDtNow = new System.Threading.Timer(new System.Threading.TimerCallback(Timer_1sec), null, 0, 1000);
			
			


			//해상도별 창위치 고정 
			if (LocationFix)
			{
				tmrScr.Interval = 1000;
				tmrScr.Tick += TmrScr_Tick;
				tmrScr.Enabled = true;
				tmrScr.Start();
			}



			if (clsLog != null) clsLog.WLog(pgmName + "을(를) 시작 합니다.");

		}



		/// <summary>
		/// 에러 처리를 한다. 로그기록 및 상태표시 창...
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="strMethodName"></param>
		/// <param name="showMessageBox">메세지 PopUp 표시 여부</param>
		protected virtual void ProcException(Exception ex, string strMethodName, bool showMessageBox = false)
		{
			if (clsLog != null) clsLog.WLog_Exception(strMethodName, ex);

			string msg;

			if (ex.Message.Equals(string.Empty))
			{
				msg = ex.InnerException.Message;
			}
			else
			{
				msg = ex.Message;
			}


			if (showMessageBox) clsFunction.ShowMsg(this, "오류 발행", msg, frmMessage.enMessageType.OK);

			SetMessage(true, msg, false);

			Console.WriteLine(string.Format("[{0}]{1}",msg, ex.ToString()));
		}



		/// <summary>
		/// 메시지 창을 클리어 한다.
		/// </summary>
		protected virtual void SetMessage_Clear()
		{
			SetMessage(false, string.Empty, false);
		}

		
		/// <summary>
		/// 메시지 창에 내용을 보여 준다.
		/// </summary>
		/// <param name="isError">에러 여부</param>
		/// <param name="strMessage"></param>
		/// <param name="isLog"></param>
		protected virtual void SetMessage(bool isError, string strMessage, bool isLog)
		{
			SetMessage(isError ? enMsgType.Error : enMsgType.OK, strMessage, isLog);
		}


		/// <summary>
		/// 메시지 창에 아이콘을 표시 합니다.
		/// </summary>
		/// <param name="msgType"></param>
		protected void SetMessageIcon(enMsgType msgType)
		{
			switch (msgType)
			{
				case enMsgType.Error:
					tsLabel.Image = ((System.Drawing.Image)(Function.resIcon16.stop));              //Function.Properties.Resources.stop));
					break;

				case enMsgType.OK:
					tsLabel.Image = ((System.Drawing.Image)(Function.resIcon16.button_green));      //(Function.Properties.Resources.button_green));
					break;

				default:
					tsLabel.Image = ((System.Drawing.Image)(Function.resIcon16.button_withe));      //(Function.Properties.Resources.button_green));
					break;

			}
		}



		/// <summary>
		/// 메시지 창에 내용을 보여 준다.(메시지 타입 사용)
		/// </summary>
		/// <param name="msgType"></param>
		/// <param name="strMessage"></param>
		/// <param name="isLog"></param>
		protected virtual void SetMessage(enMsgType msgType, string strMessage, bool isLog)
		{
			if (stBar.InvokeRequired)
			{
				stBar.Invoke(new delSetMessage(SetMessage), new object[] { msgType, strMessage, isLog });
				return;
			}

			

			if (msgType != enMsgType.Error && strMessage.Equals(string.Empty))
			{
				tsLabel.Image = null;
				tsLabel.Text = string.Empty;
				return;
			}

			//아이콘 설정
			SetMessageIcon(msgType);
			
			
			tsLabel.Text = LastMessage = string.Format("[{0}] {1}", Fnc.Date2String(DateTime.Now, Fnc.enDateType.Time), strMessage);
			LastMessageError = msgType == enMsgType.Error;

			if (clsLog != null && isLog)
				clsLog.WLog(strMessage);

			if (_onMessageChanged != null) _onMessageChanged(LastMessageError, strMessage);

		}



		/// <summary>
		/// 메시지 창에 내용을 보여준다. - 앞에 시간없음
		/// </summary>
		/// <param name="msgType"></param>
		/// <param name="strMessage"></param>
		/// <param name="isLog"></param>
		protected virtual void SetMessage_TextOnly(enMsgType msgType, string strMessage, bool isLog)
		{
			if (stBar.InvokeRequired)
			{
				stBar.Invoke(new delSetMessage(SetMessage), new object[] { msgType, strMessage, isLog });
				return;
			}
			

			if (msgType != enMsgType.Error && strMessage.Equals(string.Empty))
			{
				tsLabel.Image = null;
				tsLabel.Text = string.Empty;
				return;
			}

			//아이콘 설정
			SetMessageIcon(msgType);
			
			tsLabel.Text = LastMessage = strMessage;
			LastMessageError = msgType == enMsgType.Error;

			if (clsLog != null && isLog)
				clsLog.WLog(strMessage);

			if (_onMessageChanged != null) _onMessageChanged(LastMessageError, strMessage);

		}




		/// <summary>
		/// 메시지 창에 내용을 보여 준다.
		/// </summary>
		/// <param name="msgType">에러 여부</param>
		/// <param name="strMessage"></param>
		/// <param name="isLog"></param>
		protected virtual void SetLastMessage(enMsgType msgType, string strMessage, bool isLog)
		{
			if (stBar.InvokeRequired)
			{
				stBar.Invoke(new delSetMessage(SetLastMessage), new object[] { msgType, strMessage, isLog });
				return;
			}

			
			if (msgType != enMsgType.Error && strMessage.Equals(string.Empty))
			{
				tsLabel.Image = null;
				tsLabel.Text = string.Empty;
				return;
			}

			SetMessageIcon(msgType);
			
			tsLabel.Text = LastMessage = strMessage;
			LastMessageError = msgType == enMsgType.Error;

			//if (clsLog != null && isLog)
			//	clsLog.WLog(strMessage);

			if (_onMessageChanged != null) _onMessageChanged(LastMessageError, strMessage);

		}

		void tsLabel_DoubleClick(object sender, EventArgs e)
		{
			if (tsLabel.Text.Trim() == string.Empty) return;

			clsFunction.ShowMsg("메세지 확인", tsLabel.Text, frmMessage.enMessageType.OK);

		}

		void stBar_DoubleClick(object sender, EventArgs e)
		{
			if (tsLabel.Text.Trim() == string.Empty) return;

			clsFunction.ShowMsg("메세지 확인", tsLabel.Text, frmMessage.enMessageType.OK);
		}

		delegate void delSetProgressBar(int NowValue);

		/// <summary>
		/// 1초마다 실행 되는 폼 타이버 override하여 사용
		/// </summary>
		/// <param name="obj"></param>
		public virtual void Timer_1sec(object obj)
		{
			
		}



		/// <summary>
		/// 상태바에 값을 변경한다.
		/// </summary>
		/// <param name="NowValue"></param>
		protected void ProgressBar_Value(int NowValue)
		{
			if (stBar.InvokeRequired)
			{
				stBar.Invoke(new delSetProgressBar(ProgressBar_Value), new object[] { NowValue });
				return;
			}

			pgrBar.Value = NowValue;
		}

		/// <summary>
		/// 최대값을 설정한다.
		/// </summary>
		/// <param name="MaxValue"></param>
		protected void ProgressBar_MaxValue(int MaxValue)
		{
			if (stBar.InvokeRequired)
			{
				stBar.Invoke(new delSetProgressBar(ProgressBar_MaxValue), new object[] { MaxValue });
				return;
			}

			pgrBar.Maximum = MaxValue;
		}

		/// <summary>
		/// 최소값을 설정한다.
		/// </summary>
		/// <param name="MinValue"></param>
		protected void ProgressBar_MinValue(int MinValue)
		{
			if (stBar.InvokeRequired)
			{
				stBar.Invoke(new delSetProgressBar(ProgressBar_MinValue), new object[] { MinValue });
				return;
			}

			pgrBar.Minimum = MinValue;
		}

		/// <summary>
		/// 로그를 기록한다.
		/// </summary>
		/// <param name="msg"></param>
		protected void LogWrite(string msg)
		{
			if (clsLog != null) clsLog.WLog(msg);
		}

		


		private void frmBaseForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (clsLog != null) clsLog.WLog(pgmName + "을(를) 종료 합니다.");
		}

		private void frmBaseForm_Resize(object sender, EventArgs e)
		{
			tsLabel.Width = Width - 200;
		}


		private void frmBaseForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			
			if (!CloseToMinimize) return;
					
			
			if(_isFormClose)
			{
				return;
			}

			//종료를 취소 하고 최소화 시킨다.
			e.Cancel = true;
			WindowState = FormWindowState.Minimized;

			Notifyicon.ShowBalloonTip(1000, "프로그램이 실행 중 입니다.", string.Format("{0} 프로그램이 수행 중입니다.", this.pgmName) , ToolTipIcon.Info);
		}


		private void frmBaseForm_SizeChanged(object sender, EventArgs e)
		{
			if (!CloseToMinimize || !HideIconWhenMinimized) return;

			ShowInTaskbar = WindowState != FormWindowState.Minimized;
		}

		#region 해상도변경시 폼 위치 유지

		Timer tmrScr = new Timer();
		Rectangle[] screen = new Rectangle[0];
		Point lastLoc;
		bool isScrWork = false;

		bool _locationFix = false;

		[Description("해상도 변경시 창위치를 고정시키는 여부를 가져오거나 설정한다.")]
		/// <summary>
		/// 해상도 변경시 창위치를 고정시키는 여부를 가져오거나 설정한다.
		/// </summary>
		public bool LocationFix
		{
			get { return _locationFix; }
			set
			{
				_locationFix = value;
			}
		}
		protected enum enScr_chk_type
		{
			StartUp,
			Event,
			Timer
		}

		/// <summary>
		/// 스크린 위치를 확인한다.
		/// </summary>
		/// <param name="chkType"></param>
		/// <returns></returns>
		protected bool Scr_chk(enScr_chk_type chkType)
		{
			if (isScrWork) return false;

			try
			{
				Rectangle[] nScreen;
				int cnt = Screen.AllScreens.Length;
				bool isScrCh = false;
				int idx = 0;

				int fidx1;
				int fidx2;

				Point NewLoc = new Point();
				int x;
				int y;

				//Thread.Sleep(500);
				//Application.DoEvents();

				if (WindowState != FormWindowState.Normal) return false;

				//Console.WriteLine("[Scr_Chk]{0}", chkType);

				if (cnt < 1) return false;

				nScreen = new Rectangle[cnt];

				foreach (Screen s in Screen.AllScreens)
				{
					nScreen[idx] = s.Bounds;
					idx++;
				}


				if (chkType != enScr_chk_type.StartUp)
				{   //스크린이 변경 되었나 확인

					if (screen.Length != nScreen.Length)
					{
						isScrCh = true;
					}
					else
					{
						for (int i = 0; i < cnt; i++)
						{
							if (screen[i] != nScreen[i])
							{
								isScrCh = true;
								break;
							}
						}
					}

				}
				else
					isScrCh = true;


				if (chkType == enScr_chk_type.Timer && isScrCh)
				{ //변경이 되면 처리

					////테스트용
					//if (screen[0].Width >= 3800)
					//	lastLoc.X = 3854;
					//else
					//	lastLoc.X = 2574;



					//마지막 위치기준으로 scr idx구함
					fidx1 = ScrIdxGet(screen, lastLoc, this.Size);

					//현재 위치기준으로 scr idx구함
					fidx2 = ScrIdxGet(nScreen, this.Location, this.Size);

					//창의 스크린 idx가 변경
					if (fidx1 != fidx2 || fidx2 < 0)
					{
						idx = fidx1;
						x = lastLoc.X;
						y = lastLoc.Y;

						//기존창의 우측 위치 기준으로
						if (fidx1 >= 0 && fidx1 < screen.Length)
						{
							Rectangle rc = screen[fidx1];
							x = rc.X + rc.Width - x;
							y = rc.Y + rc.Height - y;
						}


						if (nScreen.Length <= idx || idx < 0)
						{
							//기존 있던창이 없어짐 위치 초기화
							idx = 0;
							NewLoc.X = 0;
							NewLoc.Y = 0;
						}
						else
						{
							NewLoc.X = nScreen[idx].X + nScreen[idx].Width - x;
							NewLoc.Y = nScreen[idx].Y + nScreen[idx].Height - y;
						}
						screen = nScreen;
						this.Location = NewLoc;

					}
					else
						screen = nScreen;


				}
				else if (isScrCh)
				{
					screen = nScreen;
				}

				lastLoc = this.Location;

				//Console.WriteLine("[Loc]{0}", lastLoc);

				return isScrCh;
			}
			catch
			{
				return false;
			}
			finally
			{
				isScrWork = false;
			}
		}
		

		/// <summary>
		/// 현재 폼의 스크린 위치를 구한다
		/// </summary>
		/// <param name="scr"></param>
		/// <param name="loc"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		private int ScrIdxGet(Rectangle[] scr, Point loc, Size size)
		{
			int idx = -1;
			int i = 0;

			foreach (Rectangle r in scr)
			{
				if (r.X <= loc.X && (r.X + r.Width) >= loc.X &&
					 r.Y <= loc.Y && (r.Y + r.Height) >= loc.Y)
					idx = i;

				i++;
			}

			return idx;

		}

		private void TmrScr_Tick(object sender, EventArgs e)
		{
			Scr_chk(enScr_chk_type.Timer);
		}



		private void frmBaseForm_LocationChanged(object sender, EventArgs e)
		{
			if(_locationFix) Scr_chk(enScr_chk_type.Event);
		}


		#endregion



		#region notifyIcon 관련


		/// <summary>
		/// NotifyIcon 표시 여부를 가져오거나 설정합니다. 직접 클래스늩 base.Notifyicon 로 접근 가능
		/// </summary>
		[Description("NotifyIcon 표시 여부를 가져오거나 설정합니다.")]
		public bool Notifyicon_Visible
		{
			get
			{
				return Notifyicon.Visible;
			}
			set
			{
				Notifyicon.Visible = value;
			}
		}

		/// <summary>
		/// NotifyIcon 현재 아이콘을 가져 오거나 설정합니다.. 직접 클래스늩 base.Notifyicon 로 접근 가능
		/// </summary>
		[Description("NotifyIcon 현재 아이콘을 가져 오거나 설정합니다.. 직접 클래스늩 base.Notifyicon 로 접근 가능")]
		public Icon Notifyicon_Icon
		{
			get
			{
				return Notifyicon.Icon;
			}
			set
			{
				Notifyicon.Icon = value;
			}
			
		}

		#endregion

		#region SysteShutDown 처리 관련

		private static int WM_QUERYENDSESSION = 0x11;
		
		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			if (m.Msg == WM_QUERYENDSESSION)
			{				
				_isFormClose = true;

				//창을 닫아 준다.
				this.Close();
			}

			// If this is WM_QUERYENDSESSION, the closing event should be
			// raised in the base WndProc.
			base.WndProc(ref m);

		} //WndProc 

		#endregion



		private void Notifyicon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (!CloseToMinimize || !HideIconWhenMinimized || WindowState != FormWindowState.Minimized ) return;

			if (WindowState == FormWindowState.Minimized) WindowState = FormWindowState.Normal;
			
		}


	}   //end class
}