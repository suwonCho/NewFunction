using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Function.Device.NL_RFID1000
{
	public partial class Form1 : Form
	{
		reader dev;


		public Form1()
		{
			InitializeComponent();

			dev = new reader("테스트");

			dev.Tag_duplicate_min = 1;
			dev.OnConnectionChanged += Dev_OnConnectionChanged;
			dev.OnTagReceive += Dev_OnTagReceive;

		}

		private void Dev_OnTagReceive(object sender, string rowdata, string textdata)
		{
			Console.WriteLine("태그수신 [Row]{0} [Text]{1}", rowdata, textdata);

			//txtTag.Text = string.Format("[{0}]{1} / {2}", DateTime.Now, rowdata, textdata);

			Function.form.control.Invoke_Control_SetProperty(txtTag, "Text", string.Format("[{0}]{1} / {2}", DateTime.Now, rowdata, textdata));
		}

		private void Dev_OnConnectionChanged(object sender, enConnectionStatus status)
		{
			try
			{
				Console.WriteLine("[상태변경]{0}", status);

				Function.form.control.Invoke_Control_SetProperty(txtConn, "Text", string.Format("[{0}]{1}", DateTime.Now, status));
				//txtConn.Text = string.Format("[{0}]{1}", DateTime.Now, status);
			}
			catch(Exception ex)
			{

			}
		}

		private void btnConn_Click(object sender, EventArgs e)
		{
			dev.Ip_Address = "210.100.103.85";
			dev.Connect();
		}

		private void btnDisconn_Click(object sender, EventArgs e)
		{
			dev.Disconnect();
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			dev.Inventory_Start();
		}

		private void btnEnd_Click(object sender, EventArgs e)
		{
			dev.Inventory_Stop();
		}
	}
}
