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
	/// Remotiong ���� Class
	/// </summary>
	public class clsRemoting : IDisposable
	{
		
		/// <summary>
		/// RemotionService class
		/// </summary>
		public object RemotingObject = null;

		/// <summary>
		/// ���� ä��..
		/// </summary>
		//IChannel ch;
		IChannelReceiver ch;

		/// <summary>
		/// ���񽺳���(����) Ǯ������ �����մϴ�.
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
		/// ���񽺸�
		/// </summary>
		public string strUri;


		/// <summary>
		/// ���� ���� IP
		/// </summary>
		public string strIP;


		/// <summary>
		/// ���� port/���� ���� Port
		/// </summary>
		public int intPort;

		/// <summary>
		/// ���� ���� ��ü ����
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
		/// ����/Client ���� ��ü ����
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
		/// ���� ä���� ����Ѵ�.
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
		/// http ���� ä���� ����Ѵ�.
		/// </summary>
		/// <param name="type">��ü type : typeof(Object)</param>		
		/// <param name="ensureSecurity">���ȼ�������</param>		
		/// <param name="isSingeton">��ü�� ���� ��������(true)/Ŭ���̾�Ʈ ���� ���� ����(false)</param>
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
		/// tcp ���� ä���� ����Ѵ�.
		/// </summary>
		/// <param name="type">��ü type : typeof(Object)</param>		
		/// <param name="ensureSecurity">���ȼ�������</param>		
		/// <param name="isSingeton">��ü�� ���� ��������(true)/Ŭ���̾�Ʈ ���� ���� ����(false)</param>
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
		/// HttpChannel�� �ݽ��ϴ�.
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
		/// Channel�� �ݴ´�.
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
		/// Remote Client�� ���� �Ѵ�
		/// </summary>
		/// <param name="type">��ü type : typeof(Object)</param>
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
