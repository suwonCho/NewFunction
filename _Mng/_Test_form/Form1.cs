using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace _Test_form
{
	public partial class Form1 : Function.form.frmBaseForm
	{
		

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			//tmr.Interval = 1000;
			//tmr.Tick += Tmr_Tick;
			//tmr.Start();

			//this.Invoke(new MethodInvoker(Form_Init));
			//Form_Init();

			//usrGifPicbox1.Image = Function.resIcon16.loading_003;
		}

		private void Tmr_Tick(object sender, EventArgs e)
		{
			//tmrInit.Stop();
			//this.Invoke(new MethodInvoker(Form_Init));
		}

		protected override void Form_Init()
		{
			
			for (int i = 0; i < 5; i++)
			{
				label1.Text = i.ToString();
				//Application.DoEvents();
				Thread.Sleep(1500);
			}		
			
		}		

	}
}
