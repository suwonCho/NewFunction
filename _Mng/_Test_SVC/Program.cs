using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;

namespace _Test
{
	static class Program
	{
		///// <summary>
		///// 해당 응용 프로그램의 주 진입점입니다.
		///// </summary>
		//[STAThread]
		//static void Main(string[] args)
		//{
		//	//Application.EnableVisualStyles();
		//	//Application.SetCompatibleTextRenderingDefault(false);
		//	//Application.Run(new Form1());


		//	ServiceBase[] ServicesToRun;
		//	ServicesToRun = new ServiceBase[]
		//	{
		//		new MyNewSVC(new string[] { "Test" } )
		//	};
		//	ServiceBase.Run(ServicesToRun);

		//}


		[STAThread]
		static void Main()
		{
			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Form1());


			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[]
			{
				new MyNewSVC(new string[] { "Test" } )
			};
			ServiceBase.Run(ServicesToRun);

		}



	}
}
