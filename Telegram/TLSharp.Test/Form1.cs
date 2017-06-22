using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TeleSharp.TL;


namespace TLSharp.Test
{
	public partial class Form1 : Function.form.frmBaseForm
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				connect();
			}
			catch(Exception ex)
			{

			}
		}


		private async Task connect()
		{
			try
			{
				Telegram.Massenger.Client client = new Telegram.Massenger.Client("+821072919103", fnc.apiId, fnc.apiHash, this);

				await client.Connect();

				//await client.SendMessageByPhoneNumber("821026799103", "20161109");
			}
			catch(Exception ex)
			{
				ProcException(ex, "connect");
			}
			
		}

		private void button2_Click(object sender, EventArgs e)
		{
			conn();
			
		}

		private async void conn()
		{
			TLSharp.Core.TelegramClient client = new Core.TelegramClient(fnc.apiId, fnc.apiHash);

			await client.ConnectAsync();

			Auth(client, "+821072919103");

			SendMessageByPhoneNumber(client, "821026799103", "20161109");
		}

		private async void Auth(TLSharp.Core.TelegramClient client, string PhoneNumber)
		{
			if (client.IsUserAuthorized()) return;

			//인증-한번 받으면 된다
			var hash = await client.SendCodeRequestAsync(PhoneNumber);
			var code = "20948"; // you can change code in debugger
			var user = await client.MakeAuthAsync(PhoneNumber, hash, code);


		}


		/// <summary>
		/// 전화 번호로
		/// </summary>
		/// <param name="client"></param>
		/// <param name="PhoneNumber"></param>
		/// <param name="msg"></param>
		private async void SendMessageByPhoneNumber(TLSharp.Core.TelegramClient client, string PhoneNumber, string msg)
		{
			//연락처를 받아 온다
			var rst = await client.GetContactsAsync();

			//사용자를 찾는다.
			var usr = rst.users.lists
				.Where(x => x.GetType() == typeof(TLUser))
				.Cast<TLUser>()
				.FirstOrDefault(x => x.phone.EndsWith(PhoneNumber));


			TLUser tl;


			await client.SendMessageAsync(new TLInputPeerUser() { user_id = usr.id }, msg);

		}


	}
}
