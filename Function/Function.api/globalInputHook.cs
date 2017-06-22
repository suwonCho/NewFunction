using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.ComponentModel;

namespace Function.api
{
	/// <summary>
	/// 전역 키보드 후킹 클래스<para/>
	/// KeyDown 키다운 / gkh.KeyUp 키업 이벤트 <para/>
	/// OnMouse~~ 마우스관련 이벤드<para/>
	/// </summary>
	public class globalInputHook : IDisposable
	{

		public enum enHookType
		{
			None = 0,
			KeyBoard = 1,
			Mouse = 2,
			KeyBoard_Mouse = 3
		}


		public enHookType HookType;

		


		#region Constant, Structure and Delegate Definitions
		/// <summary>
		/// defines the callback type for the hook
		/// </summary>
		public delegate int keyboardHookProc(int code, int wParam, ref IntPtr lParam);

		/// <summary>
		/// The MSLLHOOKSTRUCT structure contains information about a low-level keyboard input event. 
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		private struct MouseLLHookStruct
		{
			/// <summary>
			/// Specifies a Point structure that contains the X- and Y-coordinates of the cursor, in screen coordinates. 
			/// </summary>
			public POINT Point;
			/// <summary>
			/// If the message is WM_MOUSEWHEEL, the high-order word of this member is the wheel delta. 
			/// The low-order word is reserved. A positive value indicates that the wheel was rotated forward, 
			/// away from the user; a negative value indicates that the wheel was rotated backward, toward the user. 
			/// One wheel click is defined as WHEEL_DELTA, which is 120. 
			///If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP,
			/// or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
			/// and the low-order word is reserved. This value can be one or more of the following values. Otherwise, MouseData is not used. 
			///XBUTTON1
			///The first X button was pressed or released.
			///XBUTTON2
			///The second X button was pressed or released.
			/// </summary>
			public int MouseData;
			/// <summary>
			/// Specifies the event-injected flag. An application can use the following value to test the mouse Flags. Value Purpose 
			///LLMHF_INJECTED Test the event-injected flag.  
			///0
			///Specifies whether the event was injected. The value is 1 if the event was injected; otherwise, it is 0.
			///1-15
			///Reserved.
			/// </summary>
			public int Flags;
			/// <summary>
			/// Specifies the Time stamp for this message.
			/// </summary>
			public int Time;
			/// <summary>
			/// Specifies extra information associated with the message. 
			/// </summary>
			public int ExtraInfo;
		}

		/// <summary>
		/// The KBDLLHOOKSTRUCT structure contains information about a low-level keyboard input event. 
		/// </summary>
		/// <remarks>
		/// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookstructures/cwpstruct.asp
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		private struct KeyboardHookStruct
		{
			/// <summary>
			/// Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
			/// </summary>
			public int VirtualKeyCode;
			/// <summary>
			/// Specifies a hardware scan code for the key. 
			/// </summary>
			public int ScanCode;
			/// <summary>
			/// Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
			/// </summary>
			public int Flags;
			/// <summary>
			/// Specifies the Time stamp for this message.
			/// </summary>
			public int Time;
			/// <summary>
			/// Specifies extra information associated with the message. 
			/// </summary>
			public int ExtraInfo;
		}




		public enum enInputHookType
		{
			WH_KEYBOARD_LL = 13,

			/// <summary>
			/// Windows NT/2000/XP: Installs a hook procedure that monitors low-level mouse input events.
			/// </summary>
			WH_MOUSE_LL = 14,

			/// <summary>
			/// The WM_MOUSEMOVE message is posted to a window when the cursor moves. 
			/// </summary>
			WM_MOUSEMOVE = 0x200,

			/// <summary>
			/// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button 
			/// </summary>
			WM_LBUTTONDOWN = 0x201,

			/// <summary>
			/// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button
			/// </summary>
			WM_RBUTTONDOWN = 0x204,

			/// <summary>
			/// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button 
			/// </summary>
			WM_MBUTTONDOWN = 0x207,

			/// <summary>
			/// The WM_LBUTTONUP message is posted when the user releases the left mouse button 
			/// </summary>
			WM_LBUTTONUP = 0x202,

			/// <summary>
			/// The WM_RBUTTONUP message is posted when the user releases the right mouse button 
			/// </summary>
			WM_RBUTTONUP = 0x205,

			/// <summary>
			/// The WM_MBUTTONUP message is posted when the user releases the middle mouse button 
			/// </summary>
			WM_MBUTTONUP = 0x208,

			/// <summary>
			/// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button 
			/// </summary>
			WM_LBUTTONDBLCLK = 0x203,

			/// <summary>
			/// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button 
			/// </summary>
			WM_RBUTTONDBLCLK = 0x206,

			/// <summary>
			/// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button 
			/// </summary>
			WM_MBUTTONDBLCLK = 0x209,

			/// <summary>
			/// The WM_MOUSEWHEEL message is posted when the user presses the mouse wheel. 
			/// </summary>
			WM_MOUSEWHEEL = 0x020A,

			/// <summary>
			/// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem 
			/// key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.
			/// </summary>
			WM_KEYDOWN = 0x100,

			/// <summary>
			/// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem 
			/// key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, 
			/// or a keyboard key that is pressed when a window has the keyboard focus.
			/// </summary>
			WM_KEYUP = 0x101,

			/// <summary>
			/// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user 
			/// presses the F10 key (which activates the menu bar) or holds down the ALT key and then 
			/// presses another key. It also occurs when no window currently has the keyboard focus, 
			/// in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that 
			/// receives the message can distinguish between these two contexts by checking the context 
			/// code in the lParam parameter. 
			/// </summary>
			WM_SYSKEYDOWN = 0x104,

			/// <summary>
			/// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user 
			/// releases a key that was pressed while the ALT key was held down. It also occurs when no 
			/// window currently has the keyboard focus, in this case, the WM_SYSKEYUP message is sent 
			/// to the active window. The window that receives the message can distinguish between 
			/// these two contexts by checking the context code in the lParam parameter. 
			/// </summary>
			WM_SYSKEYUP = 0x105,
		}




		public struct stHookType
		{
			public const int WH_KEYBOARD_LL = 13;

			/// <summary>
			/// Windows NT/2000/XP: Installs a hook procedure that monitors low-level mouse input events.
			/// </summary>
			public const int WH_MOUSE_LL = 14;

			/// <summary>
			/// The WM_MOUSEMOVE message is posted to a window when the cursor moves. 
			/// </summary>
			public const int WM_MOUSEMOVE = 0x200;

			/// <summary>
			/// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button 
			/// </summary>
			public const int WM_LBUTTONDOWN = 0x201;

			/// <summary>
			/// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button
			/// </summary>
			public const int WM_RBUTTONDOWN = 0x204;

			/// <summary>
			/// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button 
			/// </summary>
			public const int WM_MBUTTONDOWN = 0x207;

			/// <summary>
			/// The WM_LBUTTONUP message is posted when the user releases the left mouse button 
			/// </summary>
			public const int WM_LBUTTONUP = 0x202;

			/// <summary>
			/// The WM_RBUTTONUP message is posted when the user releases the right mouse button 
			/// </summary>
			public const int WM_RBUTTONUP = 0x205;

			/// <summary>
			/// The WM_MBUTTONUP message is posted when the user releases the middle mouse button 
			/// </summary>
			public const int WM_MBUTTONUP = 0x208;

			/// <summary>
			/// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button 
			/// </summary>
			public const int WM_LBUTTONDBLCLK = 0x203;

			/// <summary>
			/// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button 
			/// </summary>
			public const int WM_RBUTTONDBLCLK = 0x206;

			/// <summary>
			/// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button 
			/// </summary>
			public const int WM_MBUTTONDBLCLK = 0x209;

			/// <summary>
			/// The WM_MOUSEWHEEL message is posted when the user presses the mouse wheel. 
			/// </summary>
			public const int WM_MOUSEWHEEL = 0x020A;

			/// <summary>
			/// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem 
			/// key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.
			/// </summary>
			public const int WM_KEYDOWN = 0x100;

			/// <summary>
			/// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem 
			/// key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, 
			/// or a keyboard key that is pressed when a window has the keyboard focus.
			/// </summary>
			public const int WM_KEYUP = 0x101;

			/// <summary>
			/// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user 
			/// presses the F10 key (which activates the menu bar) or holds down the ALT key and then 
			/// presses another key. It also occurs when no window currently has the keyboard focus; 
			/// in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that 
			/// receives the message can distinguish between these two contexts by checking the context 
			/// code in the lParam parameter. 
			/// </summary>
			public const int WM_SYSKEYDOWN = 0x104;

			/// <summary>
			/// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user 
			/// releases a key that was pressed while the ALT key was held down. It also occurs when no 
			/// window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent 
			/// to the active window. The window that receives the message can distinguish between 
			/// these two contexts by checking the context code in the lParam parameter. 
			/// </summary>
			public const int WM_SYSKEYUP = 0x105;
		}

		
		#endregion

		#region Instance Variables
		/// <summary>
		/// The collections of keys to watch for
		/// </summary>
		public List<Keys> HookedKeys = new List<Keys>();
		/// <summary>
		/// Handle to the hook, need this to unhook and call the next hook
		/// </summary>
		int hhook = 0;

		int mhook = 0;
		#endregion

		#region Events
		/// <summary>
		/// Occurs when one of the hooked keys is pressed
		/// </summary>
		public event KeyEventHandler KeyDown;
		/// <summary>
		/// Occurs when one of the hooked keys is released
		/// </summary>
		public event KeyEventHandler KeyUp;

		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="globalInputHook"/> class and installs the keyboard hook.
		/// </summary>
		public globalInputHook()
		{
			//hook();
			GlobalKeyboardHook();
		}

		HookProc khp = null;
		HookProc mhp = null;
		public void GlobalKeyboardHook()
		{
			HookType = enHookType.KeyBoard_Mouse;
			khp = new HookProc(hookProc_keyboard);
			mhp = new HookProc(hookProc_mouse);
			hook();
		}

		public globalInputHook(enHookType hookType)
		{
			HookType = hookType;

			if((HookType & enHookType.KeyBoard) > 0) khp = new HookProc(hookProc_keyboard);
			if ((HookType & enHookType.Mouse) > 0) mhp = new HookProc(hookProc_mouse);
			hook();

		}

//작동 안함 막음
		//public globalInputHook(enInputHookType hType)
		//{
		//	globalInputHook_Init(new enInputHookType[] { hType });
  //      }


		//public globalInputHook(enInputHookType[] hTypes)
		//{
		//	globalInputHook_Init(hTypes);
		//}


		//private void globalInputHook_Init(enInputHookType[] hTypes)
		//{
		//	bool mouse = false;
		//	bool keyboard = false;
		//	IntPtr hInstance = LoadLibrary("User32");

		//	foreach (enInputHookType t in hTypes)
		//	{
		//		switch(t)
		//		{
		//			case enInputHookType.WH_MOUSE_LL:
		//			case enInputHookType.WM_LBUTTONDBLCLK:
		//			case enInputHookType.WM_LBUTTONDOWN:
		//			case enInputHookType.WM_LBUTTONUP:
		//			case enInputHookType.WM_MBUTTONDBLCLK:
		//			case enInputHookType.WM_MBUTTONDOWN:
		//			case enInputHookType.WM_MBUTTONUP:
		//			case enInputHookType.WM_MOUSEMOVE:
		//			case enInputHookType.WM_MOUSEWHEEL:
		//			case enInputHookType.WM_RBUTTONDBLCLK:
		//			case enInputHookType.WM_RBUTTONDOWN:
		//			case enInputHookType.WM_RBUTTONUP:
		//				if(mhp == null) mhp = new HookProc(hookProc_mouse);
		//				mhook = SetWindowsHookEx((int)t, mhp, hInstance, 0);
		//				//mhook = SetWindowsHookEx(stHookType.WH_MOUSE_LL, mhp, hInstance, 0);
		//				mouse = true;
		//				break;

		//			default:
		//				if (khp == null) khp = new HookProc(hookProc_keyboard);
		//				hhook = SetWindowsHookEx((int)t, khp, hInstance, 0);
		//				keyboard = true;
		//				break;
  //              }
		//	}
			

		//	if (mouse && keyboard)
		//		HookType = enHookType.KeyBoard_Mouse;
		//	else if (mouse)
		//		HookType = enHookType.Mouse;
		//	else if (keyboard)
		//		HookType = enHookType.KeyBoard;
		//	else
		//		HookType = enHookType.None;
			
		//}


		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="globalInputHook"/> is reclaimed by garbage collection and uninstalls the keyboard hook.
		/// </summary>
		~globalInputHook()
		{
			unhook();
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Installs the global hook
		/// </summary>
		private void hook()
		{
			IntPtr hInstance = LoadLibrary("User32");
			if ((HookType & enHookType.KeyBoard) > 0) hhook = SetWindowsHookEx(stHookType.WH_KEYBOARD_LL, khp, hInstance, 0);
			if ((HookType & enHookType.Mouse) > 0)  mhook = SetWindowsHookEx(stHookType.WH_MOUSE_LL, mhp, hInstance, 0);

			//Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);

			//if (mhook == IntPtr.Zero)
			//{
			//    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
			//    int errorCode = Marshal.GetLastWin32Error();
			//    //do cleanup

			//    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
			//    //throw new Win32Exception(errorCode);
			//}
		}

		/// <summary>
		/// Uninstalls the global hook
		/// </summary>
		public void unhook()
		{
			if ((HookType & enHookType.KeyBoard) > 0) UnhookWindowsHookEx(hhook);
			if ((HookType & enHookType.Mouse) > 0) UnhookWindowsHookEx(mhook);
		}

		public void Dispose()
		{
			unhook();
		}

		/// <summary>
		/// The callback for the keyboard hook
		/// </summary>
		/// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
		/// <param name="wParam">The event type</param>
		/// <param name="lParam">The keyhook event information</param>
		/// <returns></returns>
		public int hookProc_keyboard(int code, int wParam, IntPtr lParam)
		{
			if (code >= 0)
			{

				KeyboardHookStruct HookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

				Keys key = (Keys)HookStruct.VirtualKeyCode;
				


				if( (Control.ModifierKeys & Keys.Alt) == Keys.Alt)
				{
					key = key | Keys.Alt;					
				}


				if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
				{
					key = key | Keys.Control;
				}


				if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
				{
					key = key | Keys.Shift;
				}




				KeyEventArgs kea = new KeyEventArgs(key);
				
				if ((wParam == stHookType.WM_KEYDOWN || wParam == stHookType.WM_SYSKEYDOWN) && (KeyDown != null))
				{
					KeyDown(this, kea);
				}
				else if ((wParam == stHookType.WM_KEYUP || wParam == stHookType.WM_SYSKEYUP) && (KeyUp != null))
				{
					KeyUp(this, kea);
				}
				if (kea.Handled)
					return 1;

			}
			return CallNextHookEx(hhook, code, wParam, lParam);
		}



		public event MouseEventHandler onMouseMove;

		public event MouseEventHandler onMouseClick;

		public event MouseEventHandler onMouseDown;

		public event MouseEventHandler onMouseUp;

		public event MouseEventHandler onMouseWheel;

		public event MouseEventHandler onMouseDoubleClick;



		int m_OldX;
		int m_OldY;

		public int hookProc_mouse(int code, int wParam, IntPtr lParam)
		{
			if (code >= 0)
			{
				MouseLLHookStruct mouseHookStruct = (MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));

				MouseButtons button = MouseButtons.None;
				short mouseDelta = 0;
				int clickCount = 0;
				bool mouseDown = false;
				bool mouseUp = false;


				switch (wParam)
				{
					case stHookType.WM_LBUTTONDOWN:
						mouseDown = true;
						button = MouseButtons.Left;
						clickCount = 1;
						break;
					case stHookType.WM_LBUTTONUP:
						mouseUp = true;
						button = MouseButtons.Left;
						clickCount = 1;
						break;
					case stHookType.WM_LBUTTONDBLCLK:
						button = MouseButtons.Left;
						clickCount = 2;
						break;
					case stHookType.WM_RBUTTONDOWN:
						mouseDown = true;
						button = MouseButtons.Right;
						clickCount = 1;
						break;
					case stHookType.WM_RBUTTONUP:
						mouseUp = true;
						button = MouseButtons.Right;
						clickCount = 1;
						break;
					case stHookType.WM_RBUTTONDBLCLK:
						button = MouseButtons.Right;
						clickCount = 2;
						break;
					case stHookType.WM_MOUSEWHEEL:
						//If the message is WM_MOUSEWHEEL, the high-order word of MouseData member is the wheel delta. 
						//One wheel click is defined as WHEEL_DELTA, which is 120. 
						//(value >> 16) & 0xffff; retrieves the high-order word from the given 32-bit value
						mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);

						//TODO: X BUTTONS (I havent them so was unable to test)
						//If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP, 
						//or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
						//and the low-order word is reserved. This value can be one or more of the following values. 
						//Otherwise, MouseData is not used. 
						break;
				}

				//generate event 
				MouseEventArgs e = new MouseEventArgs(
												   button,
												   clickCount,
												   mouseHookStruct.Point.X,
												   mouseHookStruct.Point.Y,
												   mouseDelta);

				//Mouse up
				if (onMouseUp != null && mouseUp)
				{
					onMouseUp.Invoke(null, e);
				}

				//Mouse down
				if (onMouseDown != null && mouseDown)
				{
					onMouseDown.Invoke(null, e);
				}

				//If someone listens to click and a click is heppened
				if (onMouseClick != null && clickCount > 0)
				{
					onMouseClick.Invoke(null, e);
				}

				//If someone listens to double click and a click is heppened
				if (onMouseDoubleClick != null && clickCount == 2)
				{
					onMouseDoubleClick.Invoke(null, e);
				}

				//Wheel was moved
				if (onMouseWheel != null && mouseDelta != 0)
				{
					onMouseWheel.Invoke(null, e);
				}

				//If someone listens to move and there was a change in coordinates raise move event
				if ((onMouseMove != null) && (m_OldX != mouseHookStruct.Point.X || m_OldY != mouseHookStruct.Point.Y))
				{
					m_OldX = mouseHookStruct.Point.X;
					m_OldY = mouseHookStruct.Point.Y;
					if (onMouseMove != null)
					{
						onMouseMove.Invoke(null, e);
					}

				}



			}
			return CallNextHookEx(mhook, code, wParam, lParam);
		}



		public delegate void evtMousePos_Change(POINT pos);

		private event evtMousePos_Change _onMousePos_Change;

		Thread thMousePos_Change;

		/// <summary>
		/// 전역 마우스 위치 변경 이벤트
		/// </summary>
		public event evtMousePos_Change OnMousePos_Change
		{
			add
			{
				_onMousePos_Change += value;

				//Thread.Sleep(50);

				//if (thMousePos_Change == null || !thMousePos_Change.IsAlive)
				//{
				//    thMousePos_Change = new Thread(new ThreadStart(mousePos_th));
				//    thMousePos_Change.IsBackground = true;
				//    thMousePos_Change.Name = "thMousePos_Change";
				//    thMousePos_Change.Start();
				//}

			}
			remove
			{
				_onMousePos_Change -= value;

			}
		}

		/// <summary>
		/// 현재 마우스 위치를 구한다.
		/// </summary>
		/// <returns></returns>
		public POINT MousePos_Get()
		{
			POINT pos = new POINT();

			GetCursorPos(ref pos);

			return pos;
		}

		//private void mousePos_th()
		//{
		//    POINT last = new POINT();
		//    POINT curr = new POINT();
		//    while (true)
		//    {				
		//        try
		//        {
		//            GetCursorPos(ref curr);
		//            if (_onMousePos_Change == null) return;

		//            if (!last.Equals(curr))
		//            {
		//                last = curr;
		//                _onMousePos_Change(last);
		//            }
		//        }
		//        catch
		//        {
		//        }

		//        Thread.Sleep(20);

		//    }
		//}

		/// <summary>
		/// 현재 커서 정보를 가지고 온다.
		/// </summary>
		/// <returns></returns>
		public static CURSORINFO GetCursorInfo()
		{
			globalInputHook.CURSORINFO info;
			info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(globalInputHook.CURSORINFO));

			GetCursorInfo(out info);

			return info;
		}



		#endregion

		#region DLL imports
		private delegate int HookProc(int nCode, int wParam, IntPtr lParam);
		/// <summary>
		/// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
		/// </summary>
		/// <param name="idHook">The id of the event you want to hook</param>
		/// <param name="callback">The callback.</param>
		/// <param name="hInstance">The handle you want to attach the event to, can be null</param>
		/// <param name="threadId">The thread you want to attach the event to, can be null</param>
		/// <returns>a handle to the desired hook</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto,
			CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		static extern int SetWindowsHookEx(int idHook, HookProc callback, IntPtr hInstance, uint threadId);


		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern short GetKeyState(int keyCode);


		/// <summary>
		/// Unhooks the windows hook.
		/// </summary>
		/// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
		/// <returns>True if successful, false otherwise</returns>
		[DllImport("user32.dll")]
		static extern bool UnhookWindowsHookEx(int hInstance);

		/// <summary>
		/// Calls the next hook.
		/// </summary>
		/// <param name="idHook">The hook id</param>
		/// <param name="nCode">The hook code</param>
		/// <param name="wParam">The wparam.</param>
		/// <param name="lParam">The lparam.</param>
		/// <returns></returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

		/// <summary>
		/// Loads the library.
		/// </summary>
		/// <param name="lpFileName">Name of the library</param>
		/// <returns>A handle to the library</returns>
		[DllImport("kernel32.dll")]
		static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("user32.dll")]
		public static extern uint keybd_event(byte bvk, byte bscan, uint dwflags, ref int dwExtrainfo);

		[DllImport("user32.dll", EntryPoint = "GetCursorPos")]
		static public extern bool GetCursorPos(ref POINT lpPoint);


		[DllImport("user32.dll")]
		static extern bool GetCursorInfo(out CURSORINFO pci);

		public struct POINT
		{
			public int X;
			public int Y;

			public override bool Equals(object obj)
			{
				POINT pnt = (POINT)obj;


				if (X == pnt.X && Y == pnt.Y) return true;

				return false;

			}

		}

		[StructLayout(LayoutKind.Sequential)]
		public struct CURSORINFO
		{
			public Int32 cbSize;
			public Int32 flags;
			public IntPtr hCursor;
			public POINTAPI ptScreenPos;
		}



		[StructLayout(LayoutKind.Sequential)]
		public struct POINTAPI
		{
			public int x;
			public int y;
		}


		#endregion




	}
}
