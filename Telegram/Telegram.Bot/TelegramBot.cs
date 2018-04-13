using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace Telegram
{
	/// <summary>
	/// 텔래그램 봇 사용 클래스
	/// </summary>
	public class TelegramBot
	{

#region Queue Send Msg
		static System.Threading.Timer tmrTelegram = null;
		static Queue<TelegramQueData> queTelegram = new Queue<TelegramQueData>();
		static DateTime dtmlastTelegramSend;
		static bool isTelegram = false;

		public static string _token_30min = null;
		public static string _chatid_30min = null;

		

		/// <summary>
		/// 큐를 이용히여 텔레그램에 메시지를 보낸다.
		/// </summary>
		/// <param name="token"></param>
		/// <param name="chatid"></param>
		/// <param name="msg"></param>
		public static void QueTelegramSend(string token, string chatid, string msg)
		{
			try
			{
				TelegramQueData q = new TelegramQueData(token, chatid, msg);
				queTelegram.Enqueue(q);

				//첫 실행시 카카오 쓰레드 시작
				if (tmrTelegram == null)
				{
					dtmlastTelegramSend = DateTime.Now;
					tmrTelegram = new System.Threading.Timer(new System.Threading.TimerCallback(thQueueSend), null, 0, 500);
				}


			}
			catch (Exception ex)
			{

			}
		}


		static void thQueueSend(object obj)
		{
			if (isTelegram) return;

			string msg;
			string rst = string.Empty;

			try
			{
				isTelegram = true;

				while (queTelegram.Count > 0)
				{
					try
					{
						TelegramQueData q = queTelegram.Dequeue();

						SendMessage(q.Token, q.ChatId, q.Msg, out rst);
						dtmlastTelegramSend = DateTime.Now;
						System.Threading.Thread.Sleep(500);
					}
					catch
					{

					}
				}

				TimeSpan sp = DateTime.Now - dtmlastTelegramSend;

				//30분 이상 없을경우
				if (sp.TotalMinutes >= 30 && _token_30min != null)
				{
					msg = "WebMonitoring 30min Check";
					SendMessage(_token_30min, _chatid_30min, msg, out rst);
					dtmlastTelegramSend = DateTime.Now;					
				}

			}
			catch { }
			finally
			{
				isTelegram = false;
			}
		}




		#endregion






		private static string _baseUrl = "https://api.telegram.org/bot";

		/// <summary>
		/// 기본 bot url를 설정 하거나 가져온다. 기본)https://api.telegram.org/bot
		/// </summary>
		public static string BaseUrl
		{
			get { return _baseUrl; }
			set
			{
				_baseUrl = value;
			}
		}


		/// <summary>
		/// bot Token을 가져오거나 설정한다. ex)12345678:abcdefghijklmn
		/// </summary>
		public string Token
		{
			get; set;
		}


		/// <summary>
		/// 챗팅 방 id를 가져오거나 설정한다.
		/// </summary>
		public string ChatId
		{
			get; set;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="token">bot Token</param>
		/// <param name="chatId">챗팅 방 id</param>
		/// <param name="baseUrl">기본 bot url</param>
		public TelegramBot(string token, string chatId)
		{
			Token = token;
			ChatId = chatId;

			//if (baseUrl != null) BaseUrl = baseUrl;
		}
		
		/// <summary>
		/// 텔레그램봇에게 메시지를 보냅니다.
		/// </summary>
		/// <param name="text">보낼 메시지</param>
		/// <param name="errorMessage">오류 메시지</param>
		/// <returns>결과</returns>
		public bool SendMessage(string text, out string errorMessage)
		{
			return SendMessage(Token, ChatId, text, out errorMessage);
		}

		/// <summary>
		/// 텔레그램봇에게 메시지를 보냅니다.
		/// </summary>
		/// <param name="chatId">chat id</param>
		/// <param name="text">보낼 메시지</param>
		/// <param name="errorMessage">오류 메시지</param>
		/// <returns>결과</returns>
		public static bool SendMessage(string token, string chatId, string text, out string errorMessage)
		{
			string url = string.Format("{0}{1}/sendMessage", BaseUrl, token);

			HttpWebRequest req = WebRequest.Create(new Uri(url)) as HttpWebRequest;
			req.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
			req.Timeout = 30 * 1000;
			req.Method = "POST";
			req.ContentType = "application/json";

			string json = String.Format("{{\"chat_id\":\"{0}\", \"text\":\"{1}\"}}", chatId, EncodeJsonChars(text));
			byte[] data = UTF8Encoding.UTF8.GetBytes(json);
			req.ContentLength = data.Length;
			using (Stream stream = req.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
				stream.Flush();
			}

			HttpWebResponse httpResponse = null;
			try
			{
				httpResponse = req.GetResponse() as HttpWebResponse;
				if (httpResponse.StatusCode == HttpStatusCode.OK)
				{
					string responseData = null;
					using (Stream responseStream = httpResponse.GetResponseStream())
					{
						using (StreamReader reader = new StreamReader(responseStream, UTF8Encoding.UTF8))
						{
							responseData = reader.ReadToEnd();
						}
					}

					if (0 < responseData.IndexOf("\"ok\":true"))
					{
						errorMessage = String.Empty;
						return true;
					}
					else
					{
						errorMessage = String.Format("결과 json 파싱 오류 ({0})", responseData);
						return false;
					}
				}
				else
				{
					errorMessage = String.Format("Http status: {0}", httpResponse.StatusCode);
					return false;
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return false;
			}
			finally
			{
				if (httpResponse != null)
					httpResponse.Close();
			}
		}

		private static string EncodeJsonChars(string text)
		{
			return text.Replace("\b", "\\\b")
				.Replace("\f", "\\\f")
				.Replace("\n", "\\\n")
				.Replace("\r", "\\\r")
				.Replace("\t", "\\\t")
				.Replace("\"", "\\\"")
				.Replace("\\", "\\\\");
		}
	}
	



}
