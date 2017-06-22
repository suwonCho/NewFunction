using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Function.api
{
	public partial class API_TestForm : Form
	{
		public API_TestForm()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			lblResult.Text = "";
			string path = @"C:\SCCAPP.ini";

			path = @"D:\_Task\2012 Ginseng\01.Source\GsSccSource\SCCAPP.ini";

			lblResult.Text = ProfileInI.GetINInString("LOCAL", "PORT", path, "NoFound");

		}

		private void button2_Click(object sender, EventArgs e)
		{
			ProfileInI.WriteInIString("LOCAL", "PORT", textBox1.Text, @"C:\SCCAPP.ini");
		}
	}
}
