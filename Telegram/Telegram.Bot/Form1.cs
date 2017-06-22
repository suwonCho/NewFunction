using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Telegram
{
	partial class Form1 : Form
	{

		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Console.WriteLine("Telegram Remote Alert v0.1\n\n");

			//if (args.Length == 0)
			//{
			//	Console.WriteLine("사용 예) TelegramRemoteAlert.exe \"보낼 내용\"");
			//	return;
			//}

			string text = "test http://www.naver.com";
			string errorMessage = null;

			string chatid = "-1001071680690";
			string token = "185429576:AAEXEyfvPalLku4FAkUDsTFJM89jmIyQJG8";

			bool ret = TelegramBot.SendMessage(token, chatid, text, out errorMessage);

			if (ret)
				Console.WriteLine(String.Format("발송: {0}", text));
			else
				Console.WriteLine(String.Format("오류: {0}", errorMessage));



		}

		private void button2_Click(object sender, EventArgs e)
		{

	
			

		}

		private void button3_Click(object sender, EventArgs e)
		{
			
			
		}



	}
}
