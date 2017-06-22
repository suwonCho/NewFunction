using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

/// <summary>
/// Tcp/Serial 통신관련
/// </summary>
namespace Function.Comm
{
	/// <summary>
	/// 소켓 통신 클라이언트 클래스..
	/// </summary>
	public class SocketClient : IDisposable
	{	
		IPEndPoint remoteEP;
				
		/// <summary>
		/// Client Socket 
		/// </summary>
		Socket objClient;

		
		/// <summary>
		/// Server의 IP
		/// </summary>
		public string strServerIP = "";

		/// <summary>
		/// 서버 포트 번호
		/// </summary>
		public int iPort = 0;

		// Client 및 Server의 Send Buffer
		const int SEND_BUFFER_SIZE = 7168;
		private byte[] sendBuffer = new byte[SEND_BUFFER_SIZE];

		// Client 및 Server의 Receive Buffer
		const int READ_BUFFER_SIZE = 7168;
		private byte[] readBuffer = new byte[READ_BUFFER_SIZE];
				
		public delegate void delReceive(byte[] yReceiveData);		
		/// <summary>
		/// Receive 이벤트를 사용하기 위한 변수를 선언한다. 
		/// </summary>
		public delReceive evtReceived;

		public void Dispose()
		{
			this.Close();

			objClient = null;
			
		}

		public SocketClient()
		{
		}

		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="pIP">서버 IP</param>
		/// <param name="pPort">서버 Port</param>
		public SocketClient(string pIP, int pPort)
		{
			strServerIP = pIP;
			iPort = pPort;

		}

		public bool isConnected
		{
			get
			{
				if (objClient == null)
					return false;
				else
					return objClient.Connected;
			}
		}


		public bool Open()
		{
			try
			{
			
				// Server의 IP Address와 Port 을 변수에 담자.
				remoteEP = new IPEndPoint(IPAddress.Parse(strServerIP), iPort);

				objClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

				// 일단 여기서는 Async 을 사용하지 말자
				// 따라서 Client_ConnectCallback 함수는 만들어만 놓고 사용하지는 않는다.
				//objClient.BeginConnect(remoteEP, new AsyncCallback(Client_ConnectCallback), objClient);
				objClient.Connect(remoteEP);
				objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);

				//strMessage = "Client 연결 성공";
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public bool Close()
		{
			try
			{

				// Client 을 종료하자.
				if (objClient != null)
				{
					objClient.Shutdown(SocketShutdown.Both);
					objClient.Close();
					objClient = null;
				}
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// socket에서 데이터를 수신하는 부분임
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void ReceiveThread(IAsyncResult ar)
		{
			int intBytesRead = 0;
			
			try
			{
				intBytesRead = objClient.EndReceive(ar);

				if (intBytesRead < 1)
				{
					
					//if no bytes were read server has close.  Disable input window.
					// Start a new asynchronous read into readBuffer.
					objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
					return;
				}
								
				//수신한 정보를 byte[]에 담자.
				byte[] tmpByte = new byte[intBytesRead];

				Array.Copy(readBuffer, 0, tmpByte, 0, intBytesRead);

				if (evtReceived != null)
					evtReceived(tmpByte);

				// Ensure that no other threads try to use the stream at the same time.
				objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
			}
			catch
			{
				// objTcpClient 가 null 이 아니면 ReceiveThread 을 실행해야 한다.
				if (objClient != null)
				{
					// Start a new asynchronous read into readBuffer.
					objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
				}

				ar = null;
				return;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Client을 통해 데이터를 전송하면서 그 Receive 을 선언하는 부분
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SendCallback(IAsyncResult ar)
		{
			//objClient = (Socket) ar.AsyncState;

			try
			{
				int bytesSent = objClient.EndSend(ar);

				objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);				
			}
			catch (SocketException ex)
			{
				throw new Exception(ex.Message, ex);
			}
			catch
			{
				//throw;
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Client을 통해 데이터를 전송하는 부분
		// Send 함수는 4개의 오버로드 임
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 데이터를 socket 을 사용하여 전송하자
		// 데이터만 전송하는 경우
		public bool Send(string strSendData)
		{
			try
			{
				//socket을 사용하여 Server에 Data 보내기  
				sendBuffer = Encoding.Default.GetBytes(strSendData);
				//objClient.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), objClient);

				return true;
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

		// 데이터를 socket 을 사용하여 전송하자
		// 데이터만 전송하는 경우
		public bool Send(byte[] byteSendData)
		{			
			try
			{
				//socket을 사용하여 Server에 Data 보내기  
				sendBuffer = byteSendData;
				//objClient.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), objClient);
								
				return true;
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

		public bool Send_Sync(byte[] byteSendData)
		{
			try
			{
				objClient.Send(byteSendData);

				return true;
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

		private string byteToString(byte[] yDATA)
		{
			int yLen = yDATA.Length;
			string strData = string.Empty;
			for (int i = 0; i < yLen; i++)
			{
				strData += yDATA[i].ToString("X2");
			}

			return strData;
		}



	}

}