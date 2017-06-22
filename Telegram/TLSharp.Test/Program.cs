using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TLSharp.Test
{
	static class Program
	{
		/// <summary>
		/// 해당 응용 프로그램의 주 진입점입니다.
		/// </summary>
		[STAThread]
		static void Main()
		{
			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				//Application.Run(new Form1());
				Application.Run(new frmMassenger());
				//Application.Run(new frmAsync());
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}
