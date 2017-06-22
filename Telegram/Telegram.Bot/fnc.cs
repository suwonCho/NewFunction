using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telegram
{

	public struct TelegramQueData
	{
		public string Token;
		public string ChatId;
		public string Msg;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="token"></param>
		/// <param name="chatid"></param>
		/// <param name="msg"></param>
		public TelegramQueData(string token, string chatid, string msg)
		{
			Token = token;
			ChatId = chatid;
			Msg = msg;
		}

	}



}
