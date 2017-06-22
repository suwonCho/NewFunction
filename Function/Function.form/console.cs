using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Function
{
	public static class console
	{
		static Function.form.frmConsoleLog _clog = null;

		/// <summary>
		/// 콘솔 로그 볼수 있는 창을 띠운다.
		/// </summary>
		public static void LogWindow_Show()
		{
			if (_clog == null || _clog.IsDisposed || _clog.Disposing)
			{
				_clog = new form.frmConsoleLog();
				_clog.Show();
			}

			_clog.TopMost = true;
			_clog.BringToFront();
			_clog.Focus();

			Application.DoEvents();

			_clog.TopMost = false;

		}

		private static void log_Write(string s)
		{
			if (_clog == null || _clog.IsDisposed || _clog.Disposing) return;

			_clog.Log_Write(s);
		}

		public static void Write(string value)
		{
			Console.Write(value);

			log_Write(value);
		}
		
		public static void Write(string format, params object[] arg )
		{
			Console.Write(format, arg);

			log_Write(string.Format(format, arg));
		}

		public static void WriteLine(string format, params object[] arg)
		{
			Console.WriteLine(format, arg);

			log_Write(string.Format(format + "\r\n", arg));
		}


		public static void WriteLine(string value)
		{
			Console.WriteLine(value);

			log_Write(value + "\r\n");
		}

	}
}
