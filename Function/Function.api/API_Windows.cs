using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace Function.api
{

	public class API_Windows : IDisposable
	{
		private static IntPtr _hookID = IntPtr.Zero;
		/// <summary>
		/// 윈도우 이름으로 Hwnd값을 구한다.
		/// </summary>
		/// <param name="lpClassName">null</param>
		/// <param name="lpWindowName"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		/// <summary>
		/// SendMessage 호출시 사용 되는 Msg Enum
		/// </summary>
		public enum enSendMeaageMsg
		{
			WM_NULL = 0x00,
			WM_CREATE = 0x01,
			WM_DESTROY = 0x02,
			WM_MOVE = 0x03,
			WM_SIZE = 0x05,
			WM_ACTIVATE = 0x06,
			WM_SETFOCUS = 0x07,
			WM_KILLFOCUS = 0x08,
			WM_ENABLE = 0x0A,
			WM_SETREDRAW = 0x0B,
			WM_SETTEXT = 0x0C,
			WM_GETTEXT = 0x0D,
			WM_GETTEXTLENGTH = 0x0E,
			WM_PAINT = 0x0F,
			WM_CLOSE = 0x10,
			WM_QUERYENDSESSION = 0x11,
			WM_QUIT = 0x12,
			WM_QUERYOPEN = 0x13,
			WM_ERASEBKGND = 0x14,
			WM_SYSCOLORCHANGE = 0x15,
			WM_ENDSESSION = 0x16,
			WM_SYSTEMERROR = 0x17,
			WM_SHOWWINDOW = 0x18,
			WM_CTLCOLOR = 0x19,
			WM_WININICHANGE = 0x1A,
			WM_SETTINGCHANGE = 0x1A,
			WM_DEVMODECHANGE = 0x1B,
			WM_ACTIVATEAPP = 0x1C,
			WM_FONTCHANGE = 0x1D,
			WM_TIMECHANGE = 0x1E,
			WM_CANCELMODE = 0x1F,
			WM_SETCURSOR = 0x20,
			WM_MOUSEACTIVATE = 0x21,
			WM_CHILDACTIVATE = 0x22,
			WM_QUEUESYNC = 0x23,
			WM_GETMINMAXINFO = 0x24,
			WM_PAINTICON = 0x26,
			WM_ICONERASEBKGND = 0x27,
			WM_NEXTDLGCTL = 0x28,
			WM_SPOOLERSTATUS = 0x2A,
			WM_DRAWITEM = 0x2B,
			WM_MEASUREITEM = 0x2C,
			WM_DELETEITEM = 0x2D,
			WM_VKEYTOITEM = 0x2E,
			WM_CHARTOITEM = 0x2F ,
			WM_SETFONT = 0x30,
			WM_GETFONT = 0x31,
			WM_SETHOTKEY = 0x32,
			WM_GETHOTKEY = 0x33,
			WM_QUERYDRAGICON = 0x37,
			WM_COMPAREITEM = 0x39,
			WM_COMPACTING = 0x41,
			WM_WINDOWPOSCHANGING = 0x46,
			WM_WINDOWPOSCHANGED = 0x47,
			WM_POWER = 0x48,
			WM_COPYDATA = 0x4A,
			WM_CANCELJOURNAL = 0x4B,
			WM_NOTIFY = 0x4E,
			WM_INPUTLANGCHANGEREQUEST = 0x50,
			WM_INPUTLANGCHANGE = 0x51,
			WM_TCARD = 0x52,
			WM_HELP = 0x53,
			WM_USERCHANGED = 0x54,
			WM_NOTIFYFORMAT = 0x55,
			WM_CONTEXTMENU = 0x7B,
			WM_STYLECHANGING = 0x7C,
			WM_STYLECHANGED = 0x7D,
			WM_DISPLAYCHANGE = 0x7E,
			WM_GETICON = 0x7F,
			WM_SETICON = 0x80 ,
			WM_NCCREATE = 0x81,
			WM_NCDESTROY = 0x82,
			WM_NCCALCSIZE = 0x83,
			WM_NCHITTEST = 0x84,
			WM_NCPAINT = 0x85,
			WM_NCACTIVATE = 0x86,
			WM_GETDLGCODE = 0x87,
			WM_NCMOUSEMOVE = 0xA0,
			WM_NCLBUTTONDOWN = 0xA1,
			WM_NCLBUTTONUP = 0xA2,
			WM_NCLBUTTONDBLCLK = 0xA3,
			WM_NCRBUTTONDOWN = 0xA4,
			WM_NCRBUTTONUP = 0xA5,
			WM_NCRBUTTONDBLCLK = 0xA6,
			WM_NCMBUTTONDOWN = 0xA7,
			WM_NCMBUTTONUP = 0xA8,
			WM_NCMBUTTONDBLCLK = 0xA9 ,
			WM_KEYFIRST = 0x100,
			WM_KEYDOWN = 0x100,
			WM_KEYUP = 0x101,
			WM_CHAR = 0x102,
			WM_DEADCHAR = 0x103,
			WM_SYSKEYDOWN = 0x104,
			WM_SYSKEYUP = 0x105,
			WM_SYSCHAR = 0x106,
			WM_SYSDEADCHAR = 0x107,
			WM_KEYLAST = 0x108 ,
			WM_IME_STARTCOMPOSITION = 0x10D,
			WM_IME_ENDCOMPOSITION = 0x10E,
			WM_IME_COMPOSITION = 0x10F,
			WM_IME_KEYLAST = 0x10F ,
			WM_INITDIALOG = 0x110,
			WM_COMMAND = 0x111,
			WM_SYSCOMMAND = 0x112,
			WM_TIMER = 0x113,
			WM_HSCROLL = 0x114,
			WM_VSCROLL = 0x115,
			WM_INITMENU = 0x116,
			WM_INITMENUPOPUP = 0x117,
			WM_MENUSELECT = 0x11F,
			WM_MENUCHAR = 0x120,
			WM_ENTERIDLE = 0x121 ,
			WM_CTLCOLORMSGBOX = 0x132,
			WM_CTLCOLOREDIT = 0x133,
			WM_CTLCOLORLISTBOX = 0x134,
			WM_CTLCOLORBTN = 0x135,
			WM_CTLCOLORDLG = 0x136,
			WM_CTLCOLORSCROLLBAR = 0x137,
			WM_CTLCOLORSTATIC = 0x138 ,
			WM_MOUSEFIRST = 0x200,
			WM_MOUSEMOVE = 0x200,
			WM_LBUTTONDOWN = 0x201,
			WM_LBUTTONUP = 0x202,
			WM_LBUTTONDBLCLK = 0x203,
			WM_RBUTTONDOWN = 0x204,
			WM_RBUTTONUP = 0x205,
			WM_RBUTTONDBLCLK = 0x206,
			WM_MBUTTONDOWN = 0x207,
			WM_MBUTTONUP = 0x208,
			WM_MBUTTONDBLCLK = 0x209,
			WM_MOUSEWHEEL = 0x20A,
			WM_MOUSEHWHEEL = 0x20E ,
			WM_PARENTNOTIFY = 0x210,
			WM_ENTERMENULOOP = 0x211,	
			WM_EXITMENULOOP = 0x212,
			WM_NEXTMENU = 0x213,
			WM_SIZING = 0x214,
			WM_CAPTURECHANGED = 0x215,
			WM_MOVING = 0x216,
			WM_POWERBROADCAST = 0x218,
			WM_DEVICECHANGE = 0x219 ,
			WM_MDICREATE = 0x220,
			WM_MDIDESTROY = 0x221,
			WM_MDIACTIVATE = 0x222,
			WM_MDIRESTORE = 0x223,
			WM_MDINEXT = 0x224,
			WM_MDIMAXIMIZE = 0x225,
			WM_MDITILE = 0x226,
			WM_MDICASCADE = 0x227,
			WM_MDIICONARRANGE = 0x228,
			WM_MDIGETACTIVE = 0x229,
			WM_MDISETMENU = 0x230,
			WM_ENTERSIZEMOVE = 0x231,
			WM_EXITSIZEMOVE = 0x232,
			WM_DROPFILES = 0x233,
			WM_MDIREFRESHMENU = 0x234 ,
			WM_IME_SETCONTEXT = 0x281,
			WM_IME_NOTIFY = 0x282,
			WM_IME_CONTROL = 0x283,
			WM_IME_COMPOSITIONFULL = 0x284,
			WM_IME_SELECT = 0x285,
			WM_IME_CHAR = 0x286,
			WM_IME_KEYDOWN = 0x290,
			WM_IME_KEYUP = 0x291 ,
			WM_MOUSEHOVER = 0x2A1,
			WM_NCMOUSELEAVE = 0x2A2,
			WM_MOUSELEAVE = 0x2A3 ,
			WM_CUT = 0x300,
			WM_COPY = 0x301,
			WM_PASTE = 0x302,
			WM_CLEAR = 0x303,
			WM_UNDO = 0x304 ,
			WM_RENDERFORMAT = 0x305,
			WM_RENDERALLFORMATS = 0x306,
			WM_DESTROYCLIPBOARD = 0x307,
			WM_DRAWCLIPBOARD = 0x308,
			WM_PAINTCLIPBOARD = 0x309,
			WM_VSCROLLCLIPBOARD = 0x30A,
			WM_SIZECLIPBOARD = 0x30B,
			WM_ASKCBFORMATNAME = 0x30C,
			WM_CHANGECBCHAIN = 0x30D,
			WM_HSCROLLCLIPBOARD = 0x30E,
			WM_QUERYNEWPALETTE = 0x30F,
			WM_PALETTEISCHANGING = 0x310,
			WM_PALETTECHANGED = 0x311 ,
			WM_HOTKEY = 0x312			,
			WM_PRINT = 0x317,
			WM_PRINTCLIENT = 0x318 ,
			WM_HANDHELDFIRST = 0x358,
			WM_HANDHELDLAST = 0x35F,
			WM_PENWINFIRST = 0x380,
			WM_PENWINLAST = 0x38F,
			WM_COALESCE_FIRST = 0x390,
			WM_COALESCE_LAST = 0x39F,
			WM_DDE_FIRST = 0x3E0,
			WM_DDE_INITIATE = 0x3E0,
			WM_DDE_TERMINATE = 0x3E1,
			WM_DDE_ADVISE = 0x3E2,
			WM_DDE_UNADVISE = 0x3E3,
			WM_DDE_ACK = 0x3E4,
			WM_DDE_DATA = 0x3E5,
			WM_DDE_REQUEST = 0x3E6,
			WM_DDE_POKE = 0x3E7,
			WM_DDE_EXECUTE = 0x3E8,
			WM_DDE_LAST = 0x3E8 ,
			WM_USER = 0x400,
			WM_APP = 0x8000
		}

		/// <summary>
		/// SendMessage 호출 Msg가 WM_SYSCOMMAND시 사용되는 wPram
		/// </summary>
		public enum enwPram_SysCommand
		{
			/// <summary>
			/// 윈도우를 닫는다.
			/// </summary>
			SC_CLOSE = 0xF060,			
			/// <summary>
			///  상황별 도움말 출력 상태가 되며 커서에 ?표시를 출력하고 사용자가 대화상자 컨트롤을 클릭하면 WM_HELP 메시지를 보낸다. 
			/// </summary>
			SC_CONTEXTHELP = 0xF180,
			/// <summary>
			/// 디폴트 메뉴 항목을 선택했거나 시스템 메뉴를 더블클릭했다.
			/// </summary>
			SC_DEFAULT = 0xF160 ,
			/// <summary>
			///  응용 프로그램이 정의한 핫키로 윈도우를 활성화였다.
			/// </summary>
			SC_HOTKEY = 0xF150,
			/// <summary>
			///  수평으로 스크롤한다. 
			/// </summary>
			SC_HSCROLL =  0xF080,
			/// <summary>
			///  키보드 입력으로 시스템 메뉴를 호출하였다. 보통 Alt+Space가 시스템 메뉴 출력키이다. 또는 Alt키와 단축키를 같이 누를 때도 이 명령이 전달되는데 이때 lParam은 단축키 문자값이 전달된다. 예를 들어 Alt+H를 누르면 lParam에는 'h'가 전달된다.
			/// </summary>
			SC_KEYMENU = 0xF100,
			/// <summary>
			///  윈도우를 최대화하였다. 
			/// </summary>
			SC_MAXIMIZE = 0xF030,
			/// <summary>
			///  윈도우를 최소화하였다. 
			/// </summary>
			SC_MINIMIZE = 0xF020,
			/// <summary>
			/// 출력장치의 상태를 설정한다. 이 명령은 전원 절약 기능이 있는 컴퓨터의 전원 절약 기능을 지원한다. lParam이 1이면 저전력 상태가 된 것이며 2이면 전원이 차단된 것이다. 
			/// </summary>
			SC_MONITORPOWER = 0xF170,
			/// <summary>
			/// 마우스 클릭으로 시스템 메뉴를 출력하였다. 
			/// </summary>
			SC_MOUSEMENU = 0xF090,
			/// <summary>
			/// 이동 항목을 선택하여 윈도우를 이동시킨다. 
			/// </summary>
			SC_MOVE = 0xF010,
			/// <summary>
			/// 다음 윈도우로 이동하였다. 
			/// </summary>
			SC_NEXTWINDOW = 0xF040,
			/// <summary>
			/// 이전 윈도우로 이동하였다. 
			/// </summary>
			SC_PREVWINDOW = 0xF050,
			/// <summary>
			/// 원래 위치로 복구하였다. 
			/// </summary>
			SC_RESTORE = 0xF120,
			/// <summary>
			/// 시스템에 등록된 스크린 세이버를 실행한다. 
			/// </summary>
			SC_SCREENSAVE = 0xF140,
			/// <summary>
			/// 윈도우의 크기를 조정한다. 
			/// </summary>
			SC_SIZE = 0xF000,
			/// <summary>
			/// 시작 메뉴를 활성화한다. 
			/// </summary>
			SC_TASKLIST = 0xF130,
			/// <summary>
			///  수직으로 스크롤한다. 
			/// </summary>
			SC_VSCROLL = 0xF070

		}


		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

		public static int SendMessage(IntPtr hWnd, enSendMeaageMsg msg, int wParam, int lParam)
		{
			return SendMessage(hWnd, (int)msg, wParam, lParam);
		}

		public static int SendMessage_SYSCOMMAND(IntPtr hWnd, enwPram_SysCommand cmd, int lParam)
		{
			return SendMessage(hWnd, (int)enSendMeaageMsg.WM_SYSCOMMAND, (int)cmd, lParam);
		}


		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		public static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		[DllImport("User32.dll")]
		public static extern Int32 SetForegroundWindow(int hWnd);

		[DllImport("User32.dll")]
		public static extern Int32 BringWindowToTop(int hWnd);

		[DllImport("User32.dll")]
		public static extern Int32 SetFocus(int hWnd);

		[DllImport("User32.dll")]
		public static extern Int32 SetActiveWindow(int hWnd);

		#region ShowWindow함수 cmdshow 변수들
		public enum enCmdSow
		{
			/// <summary>
			/// 보이지 않도록 합니다.
			/// </summary>
			SW_HIDE = 0,
			/// <summary>
			/// Window를 보이도록 하되 최대화 또는 최소화 되어 있으면 원래상태로 되돌립니다.
			/// </summary>
			SW_SHOWNORMAL = 1,
			/// <summary>
			/// Window를 활성화 하고 최소화 합니다.
			/// </summary>
			SW_SHOWMINIMIZED = 2,
			/// <summary>
			/// 최대화 합니다.
			/// </summary>
			SW_MAXIMIZE = 3,
			/// <summary>
			/// Window를 보이도록 하지만 활성화 하지는 않습니다.
			/// </summary>
			SW_SHOWNOACTIVATE = 4,
			/// <summary>
			/// Window를 보이도록 합니다.
			/// </summary>
			SW_SHOW = 5,
			/// <summary>
			/// 최소화 한 후 이전 Window를 활성화 합니다.
			/// </summary>
			SW_MINIMIZE = 6,
			/// <summary>
			/// Window를 최소화하지만 활성화 하지는 않습니다.
			/// </summary>
			SW_SHOWMINNOACTIVE = 7,
			/// <summary>
			/// Window를 보이도록 하지만 활성화 하지는 않습니다.
			/// </summary>
			SW_SHOWNA = 8,
			/// <summary>
			/// 원상태로 되돌립니다.
			/// </summary>
			SW_RESTORE = 9,
			/// <summary>
			/// -
			/// </summary>
			SW_SHOWDEFAULT = 10,
			/// <summary>
			/// 최소화 합니다.
			/// </summary>
			SW_FORCEMINIMIZE = 11,
		}
		#endregion

		[DllImport("user32")]
		public static extern int ShowWindow(int hwnd, int nCmdShow);

		public static int ShowWindow(int hwnd, enCmdSow cmd)
		{
			return ShowWindow(hwnd, (int)cmd);
		}

		/// <summary>
		/// 파일시스템의 아이콘을 Image로 가져온다.
		/// 이미지 관리는 Dictionary<Bitmap, int> _ImageStore = new Dictionary<Bitmap, int>(new Function.api.ImageComparer()); 를 사용
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static System.Drawing.Image GetFileSystemImage(string fileName)
		{
			SHFILEINFO shinfo = new SHFILEINFO();
			Win32.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON);
			System.Drawing.Image image = System.Drawing.Icon.FromHandle(shinfo.hIcon).ToBitmap();
			return image;
		}




		public API_Windows()
		{

		}

		public void Dispose()
		{

		}

	}
}
