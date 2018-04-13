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
	/// �⺻ �� ��Ÿ�� 
	/// ������ ������ ���ؼ��� SavePosition_Setting ������ ����� ������Ƽ Ŭ������ �Ҵ��ϰ�<para/>
	/// (ex)SavePosition_Setting = Properties.Settings.Default / [����]SavePosition = true �Ҵ�<para/>
	///		������Ƽ Ŭ������ BF_FORMPOSITION�׸� �߰��Ѵ�(������ string) 
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
		/// ����â ǥ�� ���θ� �����´�.
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
		/// ������ �޼��� ����
		/// </summary>		
		public string LastMessage { get; set; }
		/// <summary>
		/// ������ �޼��� ���� ����
		/// </summary>
		public bool LastMessageError { get; set; }

		
		/// <summary>
		/// Form�� ����ð� �κ� ������Ʈ
		/// </summary>
		protected System.Threading.Timer tmrDtNow;
		/// <summary>
		/// �� ���� �ð�.
		/// </summary>
		protected DateTime dtFormStart;

		/// <summary>
		/// ���α׷� �̸�
		/// </summary>
		string pgmName = string.Empty;


		/// <summary>
		/// ���α׷� �̸��� ���� �ϰų� ���� �ɴϴ�.
		/// </summary>
		[Description("���α׷� �̸��� ���� �ϰų� ���� �ɴϴ�.")]
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
		/// �޼��� ǥ�� ��������Ʈ : Ÿ ���̳� ���α׷����� ���� �޼��� ǥ�ý� �̿�.
		/// </summary>
		/// <param name="msgType"></param>
		/// <param name="strMessage"></param>
		/// <param name="isLog"></param>		
		public delegate void delSetMessage(enMsgType msgType, string strMessage, bool isLog);
		/// <summary>
		/// ���� ó�� ��������Ʈ : Ÿ ���̳� ���α׷����� �� ���� ó�� �̿��..
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="strMethodName"></param>
		public delegate void delProcException(Exception ex, string strMethodName, bool isLog);

		/// <summary>
		/// �α� Ŭ���� -> ��� ������ ��ü ������ ���־�� �Ѵ�.
		/// </summary>
		protected Function.Util.Log clsLog;

		public delegate void delMessageChanged(bool isError, string strMessage);
		
		event delMessageChanged _onMessageChanged;

		/// <summary>
		/// �� ���� ���� ó��.
		/// </summary>
		protected bool _isFormClose = false;

		/// <summary>
		/// ��Ƽ���̾����� ���ý�Ʈ �޴�
		/// </summary>
		protected ContextMenu NotifyContextMenu;


		/// <summary>
		/// �޽��� ����� �߻��ϴ� �̺�Ʈ
		/// </summary>
		public event delMessageChanged OnMessageChanged
		{
			add { _onMessageChanged += value; }
			remove { _onMessageChanged -= value; }
		}

		/// <summary>
		/// â�� ������ â�� �ּ�ȭ ���θ� �������ų� ���� �մϴ�.
		/// </summary>
		bool _closeToMinimize = false;

		/// <summary>
		/// â�� ������ â�� �ּ�ȭ ���θ� �������ų� ���� �մϴ�.</para>
		/// â�� ���� �� ��� _isFormClose = true �� ������ ���� ó��
		/// </summary>
		[Description("â�� ������ â�� �ּ�ȭ ���θ� �������ų� ���� �մϴ�.")]
		public bool CloseToMinimize
		{
			get{ return _closeToMinimize; }
			set
			{
				_closeToMinimize = value;
			}
		}


		/// <summary>
		/// �ּ�ȭ �� �� �׽�ũ���� ������ ���迩�θ� �������ų� ���� �մϴ�.
		/// </summary>
		bool _hideIconWhenMinimized = false;

		/// <summary>
		/// �ּ�ȭ �� �� �׽�ũ���� ������ ���迩�θ� �������ų� ���� �մϴ�.		
		/// </summary>
		[Description("�ּ�ȭ �� �� �׽�ũ���� ������ ���迩�θ� �������ų� ���� �մϴ�.")]
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
		/// ���ؽ�Ʈ �޴����� ���� Ŭ��
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

				MenuItem m = NotifyContextMenu.MenuItems.Add("����...");
				m.Click += M_Click;

				Notifyicon.ContextMenu = NotifyContextMenu;
			}

			stBar.DoubleClick += new EventHandler(stBar_DoubleClick);
			tmrDtNow = new System.Threading.Timer(new System.Threading.TimerCallback(Timer_1sec), null, 0, 1000);
			
			


			//�ػ󵵺� â��ġ ���� 
			if (LocationFix)
			{
				tmrScr.Interval = 1000;
				tmrScr.Tick += TmrScr_Tick;
				tmrScr.Enabled = true;
				tmrScr.Start();
			}



			if (clsLog != null) clsLog.WLog(pgmName + "��(��) ���� �մϴ�.");

		}



		/// <summary>
		/// ���� ó���� �Ѵ�. �αױ�� �� ����ǥ�� â...
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="strMethodName"></param>
		/// <param name="showMessageBox">�޼��� PopUp ǥ�� ����</param>
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


			if (showMessageBox) clsFunction.ShowMsg(this, "���� ����", msg, frmMessage.enMessageType.OK);

			SetMessage(true, msg, false);

			Console.WriteLine(string.Format("[{0}]{1}",msg, ex.ToString()));
		}



		/// <summary>
		/// �޽��� â�� Ŭ���� �Ѵ�.
		/// </summary>
		protected virtual void SetMessage_Clear()
		{
			SetMessage(false, string.Empty, false);
		}

		
		/// <summary>
		/// �޽��� â�� ������ ���� �ش�.
		/// </summary>
		/// <param name="isError">���� ����</param>
		/// <param name="strMessage"></param>
		/// <param name="isLog"></param>
		protected virtual void SetMessage(bool isError, string strMessage, bool isLog)
		{
			SetMessage(isError ? enMsgType.Error : enMsgType.OK, strMessage, isLog);
		}


		/// <summary>
		/// �޽��� â�� �������� ǥ�� �մϴ�.
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
		/// �޽��� â�� ������ ���� �ش�.(�޽��� Ÿ�� ���)
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

			//������ ����
			SetMessageIcon(msgType);
			
			
			tsLabel.Text = LastMessage = string.Format("[{0}] {1}", Fnc.Date2String(DateTime.Now, Fnc.enDateType.Time), strMessage);
			LastMessageError = msgType == enMsgType.Error;

			if (clsLog != null && isLog)
				clsLog.WLog(strMessage);

			if (_onMessageChanged != null) _onMessageChanged(LastMessageError, strMessage);

		}



		/// <summary>
		/// �޽��� â�� ������ �����ش�. - �տ� �ð�����
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

			//������ ����
			SetMessageIcon(msgType);
			
			tsLabel.Text = LastMessage = strMessage;
			LastMessageError = msgType == enMsgType.Error;

			if (clsLog != null && isLog)
				clsLog.WLog(strMessage);

			if (_onMessageChanged != null) _onMessageChanged(LastMessageError, strMessage);

		}




		/// <summary>
		/// �޽��� â�� ������ ���� �ش�.
		/// </summary>
		/// <param name="msgType">���� ����</param>
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

			clsFunction.ShowMsg("�޼��� Ȯ��", tsLabel.Text, frmMessage.enMessageType.OK);

		}

		void stBar_DoubleClick(object sender, EventArgs e)
		{
			if (tsLabel.Text.Trim() == string.Empty) return;

			clsFunction.ShowMsg("�޼��� Ȯ��", tsLabel.Text, frmMessage.enMessageType.OK);
		}

		delegate void delSetProgressBar(int NowValue);

		/// <summary>
		/// 1�ʸ��� ���� �Ǵ� �� Ÿ�̹� override�Ͽ� ���
		/// </summary>
		/// <param name="obj"></param>
		public virtual void Timer_1sec(object obj)
		{
			
		}



		/// <summary>
		/// ���¹ٿ� ���� �����Ѵ�.
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
		/// �ִ밪�� �����Ѵ�.
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
		/// �ּҰ��� �����Ѵ�.
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
		/// �α׸� ����Ѵ�.
		/// </summary>
		/// <param name="msg"></param>
		protected void LogWrite(string msg)
		{
			if (clsLog != null) clsLog.WLog(msg);
		}

		


		private void frmBaseForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (clsLog != null) clsLog.WLog(pgmName + "��(��) ���� �մϴ�.");
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

			//���Ḧ ��� �ϰ� �ּ�ȭ ��Ų��.
			e.Cancel = true;
			WindowState = FormWindowState.Minimized;

			Notifyicon.ShowBalloonTip(1000, "���α׷��� ���� �� �Դϴ�.", string.Format("{0} ���α׷��� ���� ���Դϴ�.", this.pgmName) , ToolTipIcon.Info);
		}


		private void frmBaseForm_SizeChanged(object sender, EventArgs e)
		{
			if (!CloseToMinimize || !HideIconWhenMinimized) return;

			ShowInTaskbar = WindowState != FormWindowState.Minimized;
		}

		#region �ػ󵵺���� �� ��ġ ����

		Timer tmrScr = new Timer();
		Rectangle[] screen = new Rectangle[0];
		Point lastLoc;
		bool isScrWork = false;

		bool _locationFix = false;

		[Description("�ػ� ����� â��ġ�� ������Ű�� ���θ� �������ų� �����Ѵ�.")]
		/// <summary>
		/// �ػ� ����� â��ġ�� ������Ű�� ���θ� �������ų� �����Ѵ�.
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
		/// ��ũ�� ��ġ�� Ȯ���Ѵ�.
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
				{   //��ũ���� ���� �Ǿ��� Ȯ��

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
				{ //������ �Ǹ� ó��

					////�׽�Ʈ��
					//if (screen[0].Width >= 3800)
					//	lastLoc.X = 3854;
					//else
					//	lastLoc.X = 2574;



					//������ ��ġ�������� scr idx����
					fidx1 = ScrIdxGet(screen, lastLoc, this.Size);

					//���� ��ġ�������� scr idx����
					fidx2 = ScrIdxGet(nScreen, this.Location, this.Size);

					//â�� ��ũ�� idx�� ����
					if (fidx1 != fidx2 || fidx2 < 0)
					{
						idx = fidx1;
						x = lastLoc.X;
						y = lastLoc.Y;

						//����â�� ���� ��ġ ��������
						if (fidx1 >= 0 && fidx1 < screen.Length)
						{
							Rectangle rc = screen[fidx1];
							x = rc.X + rc.Width - x;
							y = rc.Y + rc.Height - y;
						}


						if (nScreen.Length <= idx || idx < 0)
						{
							//���� �ִ�â�� ������ ��ġ �ʱ�ȭ
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
		/// ���� ���� ��ũ�� ��ġ�� ���Ѵ�
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



		#region notifyIcon ����


		/// <summary>
		/// NotifyIcon ǥ�� ���θ� �������ų� �����մϴ�. ���� Ŭ�����p base.Notifyicon �� ���� ����
		/// </summary>
		[Description("NotifyIcon ǥ�� ���θ� �������ų� �����մϴ�.")]
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
		/// NotifyIcon ���� �������� ���� ���ų� �����մϴ�.. ���� Ŭ�����p base.Notifyicon �� ���� ����
		/// </summary>
		[Description("NotifyIcon ���� �������� ���� ���ų� �����մϴ�.. ���� Ŭ�����p base.Notifyicon �� ���� ����")]
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

		#region SysteShutDown ó�� ����

		private static int WM_QUERYENDSESSION = 0x11;
		
		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			if (m.Msg == WM_QUERYENDSESSION)
			{				
				_isFormClose = true;

				//â�� �ݾ� �ش�.
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