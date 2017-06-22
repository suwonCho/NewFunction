using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace Function.api
{
	/// <summary>
	/// 프로세스 처리 관련 클래스
	/// </summary>
	public class process
	{
		public delegate void delRtnIntPtr(IntPtr rst);

		static int _intSleepTime = 5000;
		/// <summary>
		/// 윈도우의 할성화된 프로세스 IntPtr을 구한다.
		/// </summary>
		/// <param name="sleeptime">구하기전 대기 시간(창선택 시간)</param>
		/// <returns></returns>
		public static void Current_WindowIntPtr_Get(int sleeptime, delRtnIntPtr evt)
		{
			//string processname = null;

			_intSleepTime = sleeptime;
			Thread th = new Thread(new ParameterizedThreadStart(_current_WindowIntPtr_Get));
			th.IsBackground = true;
			th.Start(evt);

		}


		public static IntPtr Current_WindowIntPtr_Get()
		{
			//string processname = null;

			return API_Windows.GetForegroundWindow();

		}




		private static void _current_WindowIntPtr_Get(object obj)
		{
			//string processname = null;

			Thread.Sleep(_intSleepTime);

			IntPtr hwnd = API_Windows.GetForegroundWindow();

			delRtnIntPtr evt = obj as delRtnIntPtr;

			if (obj == null) return;

			evt(hwnd);

		}

		/// <summary>
		/// 프로세스 IntPtr로 프로세스 이름을 가져온다
		/// </summary>
		/// <param name="hwnd"></param>
		/// <returns></returns>
		public static string WindowProcessName_Get_byHWnd(IntPtr hwnd)
		{
			try
			{
				uint pid;
				API_Windows.GetWindowThreadProcessId(hwnd, out pid);

				System.Diagnostics.Process cur = System.Diagnostics.Process.GetProcessById((int)pid);

				if (cur == null)
					return string.Empty;

				return cur.ProcessName;
			}
			catch
			{
				return string.Empty;
			}

		}

		/// <summary>
		/// 프로세스 IntPtr로 프로세스를 가져온다
		/// </summary>
		/// <param name="hwnd"></param>
		/// <returns></returns>
		public static System.Diagnostics.Process WindowProcess_Get_byHWnd(IntPtr hwnd)
		{
			try
			{
				uint pid;
				API_Windows.GetWindowThreadProcessId(hwnd, out pid);

				System.Diagnostics.Process cur = System.Diagnostics.Process.GetProcessById((int)pid);

				if (cur == null)
					return null;

				return cur;
			}
			catch
			{
				return null;
			}

		}


		public static IntPtr Hwnd_Get_byProcessName(string name)
		{
			try
			{


				System.Diagnostics.Process[] cur = System.Diagnostics.Process.GetProcessesByName(name);

				if (cur.Length < 1)
					return IntPtr.Zero;

				return cur[0].MainWindowHandle;
			}
			catch
			{
				return IntPtr.Zero;
			}

		}



	}	//end class

}
