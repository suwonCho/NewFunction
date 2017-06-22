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
	partial class frmConsoleLog : Form
	{
		public frmConsoleLog()
		{
			InitializeComponent();
		}

		public void Log_Write(string s)
		{
			richTextBox1.AppendText(s);
		}

	}
}
