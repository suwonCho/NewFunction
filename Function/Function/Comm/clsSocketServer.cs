using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;



namespace Function.Comm
{
	/// <summary>
	/// 소켓서버 생성 Class
	/// </summary>
	public class clsSocketServer
	{
		int intPort = 7000;
		int intMaxConnection = 5;

		public AsyncCallback pfnWorkerCallBack;
		public Socket m_socListener;
		public Socket m_socWorker;

		public delegate void delClientConnect(Socket soc);
		public delClientConnect evtClentConnect;
		private void thClientConnect(object obj)
		{
			if (evtClentConnect != null)
			{
				evtClentConnect((Socket)obj);
			}
			
		}

		public delegate void delClientDisconnect(Socket soc);
		public delClientDisconnect evtClientDisconnect;
		private void thClientDisconnect(object obj)
		{
			if (evtClientDisconnect != null)
			{
				CSocketPacket c = (CSocketPacket)obj;
				evtClientDisconnect(c.thisSocket);
			}

		}

		public delegate void delReceiveRequest(Socket soc, byte[] bytData);
		public delReceiveRequest evtReceiveRequest;
		private void thCilentSend(object obj)
		{
			if (evtReceiveRequest != null)
			{
				CSocketPacket c = (CSocketPacket)obj;
				evtReceiveRequest(c.thisSocket, c.dataBuffer);
			}

		}


		/// <summary>
		/// 현재 주소를 가져온다.
		/// </summary>
		/// <returns></returns>
		public static string[] MyIp(System.Net.Sockets.AddressFamily addFamily = System.Net.Sockets.AddressFamily.InterNetwork)
		{

			try
			{
				string hostString = Dns.GetHostName();

				var ip = Dns.GetHostAddresses(hostString).Where(s => s.AddressFamily == addFamily);

				int cnt = ip.Count();
				int idx = 0;
				string[] rtn = new string[cnt];
				
				foreach(IPAddress i in ip )
				{
					rtn[idx] = i.ToString();
					idx++;
				}


				return rtn;

			}
			catch
			{
				return null;
			}

		}

		/// <summary>
		/// 서버의 주소를 가져온다.
		/// </summary>
		/// <param name="strAddress"></param>
		/// <returns></returns>
		public string[] GetHostAddress(string strAddress)
		{
		
				System.Net.IPAddress[] host = Dns.GetHostAddresses(strAddress);

				string[] add = new string[host.Length];

				int i = 0;
				foreach (IPAddress ip in host)
				{
					add[i] = ip.ToString();
					i++;
				}

				return add;

		}

		public clsSocketServer(int intport, int intMaxconnection)
		{
			intPort = intport;
			intMaxConnection = intMaxconnection;
		}

		/// <summary>
		/// 서버를 시작 한다.
		/// </summary>
		public void Start()
		{
			try
			{
				m_socListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, intPort);
				//bind to local IP Address...
				m_socListener.Bind(ipLocal);
				//start listening...
				m_socListener.Listen(intMaxConnection);
				// create the call back for any client connections...
				m_socListener.BeginAccept(new AsyncCallback(OnClientConnect), null);
				
			}
			catch (SocketException ex)
			{
				throw new Exception(ex.Message, ex);
			}
			catch
			{
				throw;
			}

		}

		/// <summary>
		/// 서버를 종료 한다.
		/// </summary>
		public void Stop()
		{
			try
			{
				//m_socListener.Shutdown(SocketShutdown.Both);			

				m_socListener.Close();			

			}
			catch (SocketException ex)
			{
				throw new Exception(ex.Message, ex);
			}
			catch
			{
				throw;
			}

		}

		/// <summary>
		/// 클라이언트 접속시 발생 한다.
		/// </summary>
		/// <param name="asyn"></param>
		public void OnClientConnect(IAsyncResult asyn)
		{
			try
			{
				m_socWorker = m_socListener.EndAccept(asyn);

				ThreadPool.QueueUserWorkItem(thClientConnect, m_socWorker);

				m_socWorker.ReceiveBufferSize = 3000;
								
				WaitForData(m_socWorker);

				m_socListener.BeginAccept(new AsyncCallback(OnClientConnect), null);
			}
			catch (ObjectDisposedException)
			{
				System.Diagnostics.Debugger.Log(0, "1", "\n OnClientConnection: Socket has been closed\n");
			}
			catch (SocketException ex)
			{
				throw new Exception(ex.Message, ex);
			}
			catch
			{
				throw;
			}

		}


		public class CSocketPacket
		{
			public System.Net.Sockets.Socket thisSocket;
			public byte[] dataBuffer = new byte[5000];
		}

		/// <summary>
		/// 클라이언트로 부터 데이터 전송을 기다린다
		/// </summary>
		/// <param name="soc"></param>
		public void WaitForData(System.Net.Sockets.Socket soc)
		{
			try
			{
				if (pfnWorkerCallBack == null)
				{
					pfnWorkerCallBack = new AsyncCallback(OnDataReceived);
				}
				CSocketPacket theSocPkt = new CSocketPacket();
				theSocPkt.thisSocket = soc;
				// now start to listen for any data...
				soc.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, pfnWorkerCallBack, theSocPkt);
			}
			catch (SocketException ex)
			{
				throw new Exception(ex.Message, ex);
			}
			catch
			{
				throw;
			}

		}
		
		/// <summary>
		/// 클라이언트로 부터 데이터 수신시 발생 한다.
		/// </summary>
		/// <param name="asyn"></param>
		public void OnDataReceived(IAsyncResult asyn)
		{
			CSocketPacket theSockId = (CSocketPacket)asyn.AsyncState;

			int ErrorCnt = 0;

			try
			{
				
				
				//end receive...
				int iRx = 0;

				if (!theSockId.thisSocket.Connected)
				{
					theSockId.thisSocket.Close();					
					return;
				}

				Thread.Sleep(50);
				iRx = theSockId.thisSocket.EndReceive(asyn);
				/*
				char[] chars = new char[iRx + 1];
				System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
				int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
				System.String szData = new System.String(chars);
				*/

				if (iRx != 0)
				{
										
					byte[] byt = new byte[iRx];

					Array.Copy(theSockId.dataBuffer, 0, byt, 0, iRx);
					theSockId.dataBuffer = byt;

					//데이터 수신처리..
					ThreadPool.QueueUserWorkItem(thCilentSend, theSockId);
				}


				ErrorCnt = 0;

				WaitForData(theSockId.thisSocket);
			}
			catch (ObjectDisposedException)
			{
				System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
			}
			catch (SocketException)
			{
				ErrorCnt++;
				//throw new Exception(ex.Message, ex);
				if (ErrorCnt < 50 && theSockId.thisSocket.Connected)
					WaitForData(theSockId.thisSocket);
			}
			catch
			{
				ErrorCnt++;
				if (ErrorCnt < 50 && theSockId.thisSocket.Connected)
					WaitForData(theSockId.thisSocket);
			}
		}



	}
}
