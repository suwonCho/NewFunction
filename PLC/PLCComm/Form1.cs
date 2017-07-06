using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections.Generic;
using System.Threading;

namespace PLCComm
{
	public partial class Form1 : Form
	{

		delegate void delTest(int idx);



		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{

			delTest d;

			for(int i=1; i < 11; i++)
			{
				d = new delTest(test);				

				d.BeginInvoke(i, null, null);

				Thread.Sleep(1000);
			}

		}

		private void test(int idx)
		{
			Console.WriteLine($"{idx:D2}번 시작...");

			Thread.Sleep(8000);


			Console.WriteLine($"{idx:D2}번 종료...");

		}

		private void button2_Click(object sender, EventArgs e)
		{
			byte b = 0xff;

			object o = b;
			int i = b;

			Console.WriteLine(o.Equals(i));

		}
	}
}
