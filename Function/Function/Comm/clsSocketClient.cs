using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

/// <summary>
/// Tcp/Serial ��Ű���
/// </summary>
namespace Function.Comm
{
	/// <summary>
	/// ���� ��� Ŭ���̾�Ʈ Ŭ����..
	/// </summary>
	public class SocketClient : IDisposable
	{	
		IPEndPoint remoteEP;
				
		/// <summary>
		/// Client Socket 
		/// </summary>
		Socket objClient;

		
		/// <summary>
		/// Server�� IP
		/// </summary>
		public string strServerIP = "";

		/// <summary>
		/// ���� ��Ʈ ��ȣ
		/// </summary>
		public int iPort = 0;

		// Client �� Server�� Send Buffer
		const int SEND_BUFFER_SIZE = 7168;
		private byte[] sendBuffer = new byte[SEND_BUFFER_SIZE];

		// Client �� Server�� Receive Buffer
		const int READ_BUFFER_SIZE = 7168;
		private byte[] readBuffer = new byte[READ_BUFFER_SIZE];
				
		public delegate void delReceive(byte[] yReceiveData);		
		/// <summary>
		/// Receive �̺�Ʈ�� ����ϱ� ���� ������ �����Ѵ�. 
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
		/// ������
		/// </summary>
		/// <param name="pIP">���� IP</param>
		/// <param name="pPort">���� Port</param>
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
			
				// Server�� IP Address�� Port �� ������ ����.
				remoteEP = new IPEndPoint(IPAddress.Parse(strServerIP), iPort);

				objClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

				// �ϴ� ���⼭�� Async �� ������� ����
				// ���� Client_ConnectCallback �Լ��� ���� ���� ��������� �ʴ´�.
				//objClient.BeginConnect(remoteEP, new AsyncCallback(Client_ConnectCallback), objClient);
				objClient.Connect(remoteEP);
				objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);

				//strMessage = "Client ���� ����";
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

				// Client �� ��������.
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
		// socket���� �����͸� �����ϴ� �κ���
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
								
				//������ ������ byte[]�� ����.
				byte[] tmpByte = new byte[intBytesRead];

				Array.Copy(readBuffer, 0, tmpByte, 0, intBytesRead);

				if (evtReceived != null)
					evtReceived(tmpByte);

				// Ensure that no other threads try to use the stream at the same time.
				objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
			}
			catch
			{
				// objTcpClient �� null �� �ƴϸ� ReceiveThread �� �����ؾ� �Ѵ�.
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
		// Client�� ���� �����͸� �����ϸ鼭 �� Receive �� �����ϴ� �κ�
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
		// Client�� ���� �����͸� �����ϴ� �κ�
		// Send �Լ��� 4���� �����ε� ��
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// �����͸� socket �� ����Ͽ� ��������
		// �����͸� �����ϴ� ���
		public bool Send(string strSendData)
		{
			try
			{
				//socket�� ����Ͽ� Server�� Data ������  
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

		// �����͸� socket �� ����Ͽ� ��������
		// �����͸� �����ϴ� ���
		public bool Send(byte[] byteSendData)
		{			
			try
			{
				//socket�� ����Ͽ� Server�� Data ������  
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