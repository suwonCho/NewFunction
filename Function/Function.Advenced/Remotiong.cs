using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;

namespace Function.Advenced
{	
	/// <summary>
	/// Remotiong 관련 Class
	/// </summary>
	public class clsRemoting : IDisposable
	{
		
		/// <summary>
		/// RemotionService class
		/// </summary>
		public object RemotingObject = null;

		/// <summary>
		/// 서비스 채널..
		/// </summary>
		//IChannel ch;
		IChannelReceiver ch;

		/// <summary>
		/// 서비스네임(서버) 풀네임을 리턴합니다.
		/// </summary>
		public string strFullUri
		{
			get
			{
				string strip;

				if (strIP == null)
				{
					strip = System.Net.Dns.GetHostName();
					strip = System.Net.Dns.GetHostAddresses(strip)[0].ToString();
				}
				else
					strip = strIP;


				return string.Format("http://{0}:{1}/{2}", strip, intPort, strUri);
			}

		}

		/// <summary>
		/// 서비스명
		/// </summary>
		public string strUri;


		/// <summary>
		/// 서버 접속 IP
		/// </summary>
		public string strIP;


		/// <summary>
		/// 서비스 port/서버 접속 Port
		/// </summary>
		public int intPort;

		/// <summary>
		/// 서버 모드로 객체 생성
		/// </summary>
		/// <param name="intport"></param>
		/// <param name="struri"></param>
		public clsRemoting(int intport, string struri)
		{
			strIP = null;
			intPort = intport;
			strUri = struri;
		}

		/// <summary>
		/// 서버/Client 모드로 객체 생성
		/// </summary>
		/// <param name="intport"></param>
		/// <param name="struri"></param>
		public clsRemoting(string strip, int intport, string struri)
		{
			strIP = strip;
			intPort = intport;
			strUri = struri;
		}


		/// <summary>
		/// 원결 채널을 등록한다.
		/// </summary>
		/// <param name="isHttp"></param>
		/// <param name="type"></param>
		/// <param name="ensureSecurity"></param>
		/// <param name="isSingeton"></param>
		public void Channel_SeverLoad(bool isHttp, Type type, bool ensureSecurity, bool isSingeton)
		{
			if (isHttp)
				HttpChannel_SeverLoad(type, ensureSecurity, isSingeton);
			else
				TcpChannel_SeverLoad(type, ensureSecurity, isSingeton);
		}


		/// <summary>
		/// http 원격 채널을 등록한다.
		/// </summary>
		/// <param name="type">객체 type : typeof(Object)</param>		
		/// <param name="ensureSecurity">보안설정여부</param>		
		/// <param name="isSingeton">객체를 개만 생성할지(true)/클라이언트 별로 생성 할지(false)</param>
		public void HttpChannel_SeverLoad(Type type, bool ensureSecurity, bool isSingeton)
		{
			ch = new HttpChannel(intPort);

			if (ChannelServices.RegisteredChannels.Length < 1) ChannelServices.RegisterChannel(ch, ensureSecurity);


			if(isSingeton)
				//client 
				RemotingConfiguration.RegisterWellKnownServiceType(type, strUri, WellKnownObjectMode.Singleton);
			else
				RemotingConfiguration.RegisterWellKnownServiceType(type, strUri, WellKnownObjectMode.SingleCall);			

		}

		/// <summary>
		/// tcp 원격 채널을 등록한다.
		/// </summary>
		/// <param name="type">객체 type : typeof(Object)</param>		
		/// <param name="ensureSecurity">보안설정여부</param>		
		/// <param name="isSingeton">객체를 개만 생성할지(true)/클라이언트 별로 생성 할지(false)</param>
		public void TcpChannel_SeverLoad(Type type, bool ensureSecurity, bool isSingeton)
		{
			ch = new TcpChannel(intPort);

			if (ChannelServices.RegisteredChannels.Length < 1) ChannelServices.RegisterChannel(ch, ensureSecurity);

			if (isSingeton)
				//client 
				RemotingConfiguration.RegisterWellKnownServiceType(type, strUri, WellKnownObjectMode.Singleton);
			else
				RemotingConfiguration.RegisterWellKnownServiceType(type, strUri, WellKnownObjectMode.SingleCall);

		}


		/// <summary>
		/// HttpChannel을 닫습니다.
		/// </summary>
		/// <param name="c"></param>
		public void HttpChannel_Close()
		{
			if (ch != null)
			{
				ch.StopListening(strUri);				
				if (ChannelServices.RegisteredChannels.Length < 1) ChannelServices.UnregisterChannel(ch);
			}

		}


		/// <summary>
		/// Channel을 닫는다.
		/// </summary>
		public void Channel_Close()
		{
			//HttpChannel hc = ch as HttpChannel;
			//TcpChannel tc = ch as TcpChannel;

			
			if (ch != null)
			{
				ch.StopListening(strUri);
				if (ChannelServices.RegisteredChannels.Length < 1) ChannelServices.UnregisterChannel(ch);
			}

		}



		/// <summary>
		/// Remote Client를 생성 한다
		/// </summary>
		/// <param name="type">객체 type : typeof(Object)</param>
		/// <param name="strIP"></param>
		/// <param name="intPort"></param>
		/// <param name="strServiceUri"></param>
		/// <returns></returns>
		public object HttpChannel_ClientLoad(Type type)
		{
			ch = (IChannelReceiver)new  HttpClientChannel();
			
			if (ChannelServices.RegisteredChannels.Length < 1) ChannelServices.RegisterChannel(ch, false);

			return Activator.GetObject(type, strFullUri );
			

		}

		public void Dispose()
		{
			HttpChannel_Close();
		}
	}
}
