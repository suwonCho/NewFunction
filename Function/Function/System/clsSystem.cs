using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Function
{
	/// <summary>
	/// System Function 관련 클래스
	/// </summary>
	public partial class system
	{
		[StructLayout(LayoutKind.Sequential)]
		private struct SYSTEMTIME
		{
			
			public ushort wYear;
			public ushort wMonth;
			public ushort wDayOfWeek;
			public ushort wDay;
			public ushort wHour;
			public ushort wMinute;
			public ushort wSecond;
			public ushort wMilliseconds;
		}

		[DllImport("Kernel32.dll")]
		private static extern int SetSystemTime(ref SYSTEMTIME IpSystemTime);

		/// <summary>
		/// 시스템 시간을 변경한다.
		/// </summary>
		/// <param name="dt"></param>
		public static void SystemTime_Change(DateTime dt)
		{
			SYSTEMTIME st = new SYSTEMTIME();

			dt = dt.AddHours(-9);

			st.wDayOfWeek = ushort.Parse(((int)dt.DayOfWeek).ToString());
			st.wYear = ushort.Parse(dt.Year.ToString());
			st.wMonth = ushort.Parse(dt.Month.ToString());
			st.wDay = ushort.Parse(dt.Day.ToString());
			st.wHour = ushort.Parse(dt.Hour.ToString());
			st.wMinute = ushort.Parse(dt.Minute.ToString());
			st.wSecond = ushort.Parse(dt.Second.ToString());
			st.wMilliseconds = ushort.Parse(dt.Millisecond.ToString());

			int i = SetSystemTime(ref st);

		}
	}
}
