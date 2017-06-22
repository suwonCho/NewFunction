using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Function.form
{

	public partial class frmTest : Form
	{
		public frmTest()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			usrSimpleGrid1.Column_Count = 6;

			//usrSimpleGrid1[1].Column_Width = 100;

			textBox1.Text = "";
			label1.TextAlign = ContentAlignment.BottomCenter;

			
		}
	}


	static class Program
	{
		/// <summary>
		/// 해당 응용 프로그램의 주 진입점입니다.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmTest());
		}
	}

}
