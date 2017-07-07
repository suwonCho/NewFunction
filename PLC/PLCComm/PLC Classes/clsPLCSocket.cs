using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;



namespace PLCComm
{

	/// <summary>
	/// Class1�� ���� ��� �����Դϴ�.
	/// </summary>
	class PLCSocket
	{

		public IPEndPoint remoteEP;

		// Server Socket
		public Socket objServer;

		// Client Socket
		public Socket objClient;

		// Client �� Server�� Send Buffer
		const int SEND_BUFFER_SIZE = 7168;
		private byte[] sendBuffer = new byte[SEND_BUFFER_SIZE];

		// Client �� Server�� Receive Buffer
		const int READ_BUFFER_SIZE = 7168;
		private byte[] readBuffer = new byte[READ_BUFFER_SIZE];

		// Server�� IP�� Port
		public string strServerIP = "";
		public int iPort = 0;


		// strCheckValue �� ����� ���
		// check �Ϸ��� strCheckValue�� ���� ���������� ���ŵǴ� ó���� �ϱ� ���Ͽ�
		// Temp �� ������ strReceive_Temp�� ����Ѵ�.
		public string strReceive_Temp = "", strReceive = "";

		public string strSerialNo = "";

		public delegate void Receive(byte[] yReceiveData);
		// Receive �̺�Ʈ�� ����ϱ� ���� ������ �����Ѵ�.
		public event Receive Received;

		private bool _is_cancel_beginreceive = true;

		public PLCSocket()
		{
			//stackReceiveData = new StackOverflowException();
			//
			// TODO: ���⿡ ������ ���� �߰��մϴ�.
			//
		}
		public PLCSocket(string pIP, int pPort)
		{
			strServerIP = pIP;
			iPort = pPort;

		}


		public bool client_Open()
		{
			try
			{
				// ���ŵǾ��� ������ �ʱ�ȭ�Ѵ�.
				ResetReceive();

				// Server�� IP Address�� Port �� ������ ����.
				remoteEP = new IPEndPoint(IPAddress.Parse(strServerIP), iPort);

				objClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

				// �ϴ� ���⼭�� Async �� ������� ����
				// ���� Client_ConnectCallback �Լ��� ���� ���� ��������� �ʴ´�.
				//objClient.BeginConnect(remoteEP, new AsyncCallback(Client_ConnectCallback), objClient);
				objClient.Connect(remoteEP);

				
				if (_is_cancel_beginreceive)
				{
					objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
					_is_cancel_beginreceive = false;
				}

				//strMessage = "Client ���� ����";
				return true;
			}
			catch (Exception ex)
			{
				//throw ex;
				return false;
			}
		}

		public bool client_Close()
		{
			try
			{
				// ���ŵǾ��� ������ �ʱ�ȭ�Ѵ�.
				ResetReceive();

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
				//throw ex;
				return false;
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// socket���� �����͸� �����ϴ� �κ���
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void ReceiveThread(IAsyncResult ar)
		{
			int intBytesRead = 0;
			string strRecv = "", strMessage = "";
			//objClient = (Socket) ar.AsyncState;

			try
			{
				intBytesRead = objClient.EndReceive(ar);

				if (intBytesRead < 1)
				{
					strMessage = "���ŵ� ������ �ڸ����� 0 �Դϴ�.";

					//if no bytes were read server has close.  Disable input window.
					// Start a new asynchronous read into readBuffer.
					objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
					return;
				}

				// ������ ������ ������ ����
				//Encoding.Default.GetString(readBuffer, 0, intBytesRead);		

				//������ ������ byte[]�� ����.
				byte[] tmpByte = new byte[intBytesRead];
				for (int i = 0; i < intBytesRead; i++)
				{
					tmpByte[i] = readBuffer[i];
				}


				strReceive = this.byteToString(tmpByte);
				// ������ ������ �α׷� ������
				strMessage = "[" + strReceive + "]����";
				//Console.WriteLine(strMessage);
#if(LOG)
				PLCModule.clsPLCModule.LogWrite("ReceiveThread", string.Format("[Received]:{0}", byteToString(tmpByte)));
#endif

				if (Received != null)
					Received(tmpByte);

			}
			catch (SocketException ex)
			{
				
			}
			catch (Exception ex)
			{
				strMessage = "[" + strRecv + "]���� �� ����" + ex.Message;
				//				clsLogwrite.LogWrite(clsLogwrite.ModeHour,
				//					CommonClass.IPC_CODE,
				//					CommonClass.APP_NAME, 
				//					CommonClass.strAppPath, 
				//					clsLogwrite.ETC,
				//					strMessage,
				//					"ReceiveThread : Exception",
				//					CommonClass.strLocalConn,
				//					clsLogwrite.ErrorModeFILE,
				//					ref strRecv);
				return;
			}
			finally
			{
				try
				{
					if (objClient != null && objClient.Connected)
					{
						objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
						_is_cancel_beginreceive = false;
					}
					else
						_is_cancel_beginreceive = true;
				}
				catch
				{
					//���� ���½� ������� ����
					_is_cancel_beginreceive = true;
				}
			}

		}


		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Client�� ���� �����͸� �����ϸ鼭 �� Receive �� �����ϴ� �κ�
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SendCallback(IAsyncResult ar)
		{
			string strMessage = "";
			//objClient = (Socket) ar.AsyncState;

			try
			{
				int bytesSent = objClient.EndSend(ar);

				//objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);

				strMessage = "Client_SendCallback�� ���������� ó���߽��ϴ�";
			}
			catch (Exception ex)
			{
				strMessage = "Client_SendCallback�� ó���ϴ� �߿� ������ �߻��Ͽ����ϴ�. => " + ex.Message;
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Client�� ���� �����͸� �����ϴ� �κ�
		// Send �Լ��� 4���� �����ε� ��
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// �����͸� socket �� ����Ͽ� ��������
		// �����͸� �����ϴ� ���
		public bool Send(string strSendData, ref string strMessage)
		{
			try
			{
				if (_is_cancel_beginreceive)
				{
					objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
					_is_cancel_beginreceive = false;
				}

				// ���� ���� ���ŵǾ��� ������ �ʱ�ȭ�Ѵ�.
				ResetReceive();

				//socket�� ����Ͽ� Server�� Data ������  
				sendBuffer = Encoding.Default.GetBytes(strSendData);
				objClient.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), objClient);

				// ���� �����߽�
				strMessage = strSendData + " ������ �����Ͽ����ϴ�.";

#if(LOG)
				PLCModule.clsPLCModule.LogWrite("ReceiveThread", string.Format("[Received]:{0}", byteToString(sendBuffer)));
#endif

				return true;
			}
			catch (Exception ex)
			{
				strMessage = strSendData + " ������ �����ϴ� �߿� ������ �߻��߽��ϴ�. ==> " + ex.Message;
				return false;
			}
		}

		// �����͸� socket �� ����Ͽ� ��������
		// �����͸� �����ϴ� ���
		public bool Send(byte[] byteSendData, ref string strMessage)
		{
			string strTmp = "[PLC] [Send Data] : ";
			string strSendData = "";
			string strReturn = "";
			try
			{

				if (_is_cancel_beginreceive)
				{
					objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
					_is_cancel_beginreceive = false;
				}


				// ���� ���� ���ŵǾ��� ������ �ʱ�ȭ�Ѵ�.
				ResetReceive();

				//socket�� ����Ͽ� Server�� Data ������  
				sendBuffer = byteSendData;
				objClient.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), objClient);

				// ���� �����߽�
				strMessage = "[" + byteToString(byteSendData) + "]����";

				Console.WriteLine(strMessage);

				return true;
			}
			catch (Exception ex)
			{
				strMessage = strTmp + strSendData + " ������ �����ϴ� �߿� ������ �߻��Ͽ����ϴ�. => " + ex.ToString();
				return false;
			}
		}

		// �����͸� socket �� ����Ͽ� ��������
		// �����͸� �����ϰ� �� ����� ��ٷȴ� ���������� Ȯ���ϴ� ���
		public bool Send(string strSendData, int intWaitTime, ref string strReceiveData, ref string strMessage)
		{
			try
			{
				if (_is_cancel_beginreceive)
				{
					objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
					_is_cancel_beginreceive = false;
				}

				// ���� ���� ���ŵǾ��� ������ �ʱ�ȭ�Ѵ�.
				ResetReceive();

				//socket�� ����Ͽ� Server�� Data ������  
				sendBuffer = Encoding.Default.GetBytes(strSendData);
				objClient.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), objClient);

				// ���� �����߽�
				strMessage = strSendData + " ������ �����Ͽ����ϴ�.";

				// ���� �� Receive �� ���� �ð��� ����
				Thread.Sleep(intWaitTime);

				// ��� ����
				strReceiveData = strReceive;

				strMessage = strSendData + "(" + strReceiveData + ")" + " ���۰���� �����Ͽ����ϴ�.";

				return true;
			}
			catch (Exception ex)
			{
				strMessage = strSendData + "(" + strReceiveData + ")" + " ������ �����ϴ� �߿� ������ �߻��߽��ϴ�. ==> " + ex.Message;
				return false;
			}
		}

		// �����͸� socket �� ����Ͽ� ��������
		// �����͸� �����ϰ� �� ����� ��ٷȴ� ���������� Ȯ���ϴ� ���
		public bool Send(byte[] byteSendData, int intWaitTime, ref string strReceiveData, ref string strMessage)
		{
			try
			{
				if (_is_cancel_beginreceive)
				{
					objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
					_is_cancel_beginreceive = false;
				}

				// ���� ���� ���ŵǾ��� ������ �ʱ�ȭ�Ѵ�.
				ResetReceive();

				//socket�� ����Ͽ� Server�� Data ������  
				sendBuffer = byteSendData;
				objClient.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), objClient);

				// ���� �����߽�
				strMessage = this.byteToString(sendBuffer) + " ������ �����Ͽ����ϴ�.";
				//Console.WriteLine(strMessage);


				// ���� �� Receive �� ���� �ð��� ����
				Thread.Sleep(intWaitTime);

				// ��� ����
				strReceiveData = strReceive;

				strMessage = Encoding.ASCII.GetString(byteSendData) + "(" + strReceiveData + ")" + " ���۰���� �����Ͽ����ϴ�.";

				return true;
			}
			catch (Exception ex)
			{
				strMessage = Encoding.ASCII.GetString(byteSendData) + "(" + strReceiveData + ")" + " ������ �����ϴ� �߿� ������ �߻��߽��ϴ�. ==> " + ex.Message;
				return false;
			}
		}

		public bool SetCheckValue(string strCheck)
		{
			string strMessage = "";

			try
			{
				// Check Value�� �����Ѵ�.
				//strCheckValue = strCheck;

				// ���ŵǾ��� ������ �ʱ�ȭ�Ѵ�.
				ResetReceive();

				//strMessage = strCheckValue + " check value�� ó���߽��ϴ�";
				return true;
			}
			catch (Exception ex)
			{
				//strMessage = strCheckValue + " check value�� ó���ϴ� �߿� ���� �߻��Ͽ����ϴ�. => " + ex.Message;
				return false;
			}
		}

		public bool ResetReceive()
		{
			string strMessage = "";

			try
			{
				// ���ŵǾ��� ������ �ʱ�ȭ�Ѵ�.
				strReceive = "";
				strReceive_Temp = "";

				strMessage = "���ŵǾ��� ������ Reset ó���߽��ϴ�";
				return true;
			}
			catch (Exception ex)
			{
				strMessage = "���ŵǾ��� ������ Reset ó���ϴ� �߿� ���� �߻��Ͽ����ϴ�. => " + ex.Message;
				return false;
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
