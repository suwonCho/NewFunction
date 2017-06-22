using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace Function.form
{
	/// <summary>
	/// 폼에서 사용할 api
	/// </summary>
	class api
	{

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


		/// <summary>
		/// 마우스 커서 위치를 조회 한다.
		/// </summary>
		/// <param name="lpPoint"></param>
		/// <returns></returns>
		[DllImport("user32.dll", EntryPoint = "GetCursorPos")]
		static public extern bool GetCursorPos(ref POINT lpPoint);




		internal const UInt32 EM_SETTABSTOPS = 0x00CB;
		internal const int unitsPerCharacter = 4;

		[DllImport("user32.dll")]
		internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, ref IntPtr lParam);

	}
}
