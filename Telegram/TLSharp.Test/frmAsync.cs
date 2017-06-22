using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TLSharp.Test
{
	public partial class frmAsync : Form
	{
		public frmAsync()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Console.WriteLine("call ccc");
			ccc();

			Console.WriteLine("End call ccc");
		}

		private async void ccc()
		{
			for (int i = 0; i < 10; i++)
			{
				Console.WriteLine("call idx[{0}]", i);
				Task<int> rst = aTest(i);

				//int r = await rst;
				Console.WriteLine("end call idx[{0}]", i);
			}


		}


		private async Task<int> aTest(int idx)
		{
			Console.WriteLine("aTestStart idx[{0}]", idx);

			Thread.Sleep(5000);

			Console.WriteLine("aTestEnd idx[{0}]", idx);
			return idx;
		}

	}
}
