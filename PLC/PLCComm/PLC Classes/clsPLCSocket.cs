using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;



namespace PLCComm
{

	/// <summary>
	/// Class1에 대한 요약 설명입니다.
	/// </summary>
	class PLCSocket
	{

		public IPEndPoint remoteEP;

		// Server Socket
		public Socket objServer;

		// Client Socket
		public Socket objClient;

		// Client 및 Server의 Send Buffer
		const int SEND_BUFFER_SIZE = 7168;
		private byte[] sendBuffer = new byte[SEND_BUFFER_SIZE];

		// Client 및 Server의 Receive Buffer
		const int READ_BUFFER_SIZE = 7168;
		private byte[] readBuffer = new byte[READ_BUFFER_SIZE];

		// Server의 IP와 Port
		public string strServerIP = "";
		public int iPort = 0;


		// strCheckValue 을 사용할 경우
		// check 하려는 strCheckValue의 값이 마지막으로 수신되는 처리를 하기 위하여
		// Temp 성 변수인 strReceive_Temp을 사용한다.
		public string strReceive_Temp = "", strReceive = "";

		public string strSerialNo = "";

		public delegate void Receive(byte[] yReceiveData);
		// Receive 이벤트를 사용하기 위한 변수를 선언한다.
		public event Receive Received;

		private bool _is_cancel_beginreceive = true;

		public PLCSocket()
		{
			//stackReceiveData = new StackOverflowException();
			//
			// TODO: 여기에 생성자 논리를 추가합니다.
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
				// 수신되었던 정보를 초기화한다.
				ResetReceive();

				// Server의 IP Address와 Port 을 변수에 담자.
				remoteEP = new IPEndPoint(IPAddress.Parse(strServerIP), iPort);

				objClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

				// 일단 여기서는 Async 을 사용하지 말자
				// 따라서 Client_ConnectCallback 함수는 만들어만 놓고 사용하지는 않는다.
				//objClient.BeginConnect(remoteEP, new AsyncCallback(Client_ConnectCallback), objClient);
				objClient.Connect(remoteEP);

				
				if (_is_cancel_beginreceive)
				{
					objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
					_is_cancel_beginreceive = false;
				}

				//strMessage = "Client 연결 성공";
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
				// 수신되었던 정보를 초기화한다.
				ResetReceive();

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
				//throw ex;
				return false;
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// socket에서 데이터를 수신하는 부분임
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
					strMessage = "수신된 정보의 자리수가 0 입니다.";

					//if no bytes were read server has close.  Disable input window.
					// Start a new asynchronous read into readBuffer.
					objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
					return;
				}

				// 수신한 정보를 변수에 담자
				//Encoding.Default.GetString(readBuffer, 0, intBytesRead);		

				//수신한 정보를 byte[]에 담자.
				byte[] tmpByte = new byte[intBytesRead];
				for (int i = 0; i < intBytesRead; i++)
				{
					tmpByte[i] = readBuffer[i];
				}


				strReceive = this.byteToString(tmpByte);
				// 수신한 정보를 로그로 남기자
				strMessage = "[" + strReceive + "]수신";
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
				strMessage = "[" + strRecv + "]수신 중 오류" + ex.Message;
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
					//소켓 오픈시 재시작을 위해
					_is_cancel_beginreceive = true;
				}
			}

		}


		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Client을 통해 데이터를 전송하면서 그 Receive 을 선언하는 부분
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		private void SendCallback(IAsyncResult ar)
		{
			string strMessage = "";
			//objClient = (Socket) ar.AsyncState;

			try
			{
				int bytesSent = objClient.EndSend(ar);

				//objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);

				strMessage = "Client_SendCallback을 정상적으로 처리했습니다";
			}
			catch (Exception ex)
			{
				strMessage = "Client_SendCallback을 처리하는 중에 오류가 발생하였습니다. => " + ex.Message;
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Client을 통해 데이터를 전송하는 부분
		// Send 함수는 4개의 오버로드 임
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// 데이터를 socket 을 사용하여 전송하자
		// 데이터만 전송하는 경우
		public bool Send(string strSendData, ref string strMessage)
		{
			try
			{
				if (_is_cancel_beginreceive)
				{
					objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
					_is_cancel_beginreceive = false;
				}

				// 전송 전에 수신되었던 정보를 초기화한다.
				ResetReceive();

				//socket을 사용하여 Server에 Data 보내기  
				sendBuffer = Encoding.Default.GetBytes(strSendData);
				objClient.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), objClient);

				// 전송 성공했슴
				strMessage = strSendData + " 정보를 전송하였습니다.";

#if(LOG)
				PLCModule.clsPLCModule.LogWrite("ReceiveThread", string.Format("[Received]:{0}", byteToString(sendBuffer)));
#endif

				return true;
			}
			catch (Exception ex)
			{
				strMessage = strSendData + " 정보를 전송하는 중에 오류가 발생했습니다. ==> " + ex.Message;
				return false;
			}
		}

		// 데이터를 socket 을 사용하여 전송하자
		// 데이터만 전송하는 경우
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


				// 전송 전에 수신되었던 정보를 초기화한다.
				ResetReceive();

				//socket을 사용하여 Server에 Data 보내기  
				sendBuffer = byteSendData;
				objClient.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), objClient);

				// 전송 성공했슴
				strMessage = "[" + byteToString(byteSendData) + "]전송";

				Console.WriteLine(strMessage);

				return true;
			}
			catch (Exception ex)
			{
				strMessage = strTmp + strSendData + " 정보를 전송하는 중에 오류가 발생하였습니다. => " + ex.ToString();
				return false;
			}
		}

		// 데이터를 socket 을 사용하여 전송하자
		// 데이터만 전송하고 그 결과를 기다렸다 수신정보를 확인하는 경우
		public bool Send(string strSendData, int intWaitTime, ref string strReceiveData, ref string strMessage)
		{
			try
			{
				if (_is_cancel_beginreceive)
				{
					objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
					_is_cancel_beginreceive = false;
				}

				// 전송 전에 수신되었던 정보를 초기화한다.
				ResetReceive();

				//socket을 사용하여 Server에 Data 보내기  
				sendBuffer = Encoding.Default.GetBytes(strSendData);
				objClient.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), objClient);

				// 전송 성공했슴
				strMessage = strSendData + " 정보를 전송하였습니다.";

				// 전송 후 Receive 을 위한 시간을 벌자
				Thread.Sleep(intWaitTime);

				// 결과 수신
				strReceiveData = strReceive;

				strMessage = strSendData + "(" + strReceiveData + ")" + " 전송결과를 수신하였습니다.";

				return true;
			}
			catch (Exception ex)
			{
				strMessage = strSendData + "(" + strReceiveData + ")" + " 정보를 전송하는 중에 오류가 발생했습니다. ==> " + ex.Message;
				return false;
			}
		}

		// 데이터를 socket 을 사용하여 전송하자
		// 데이터만 전송하고 그 결과를 기다렸다 수신정보를 확인하는 경우
		public bool Send(byte[] byteSendData, int intWaitTime, ref string strReceiveData, ref string strMessage)
		{
			try
			{
				if (_is_cancel_beginreceive)
				{
					objClient.BeginReceive(readBuffer, 0, readBuffer.Length, 0, new AsyncCallback(ReceiveThread), objClient);
					_is_cancel_beginreceive = false;
				}

				// 전송 전에 수신되었던 정보를 초기화한다.
				ResetReceive();

				//socket을 사용하여 Server에 Data 보내기  
				sendBuffer = byteSendData;
				objClient.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), objClient);

				// 전송 성공했슴
				strMessage = this.byteToString(sendBuffer) + " 정보를 전송하였습니다.";
				//Console.WriteLine(strMessage);


				// 전송 후 Receive 을 위한 시간을 벌자
				Thread.Sleep(intWaitTime);

				// 결과 수신
				strReceiveData = strReceive;

				strMessage = Encoding.ASCII.GetString(byteSendData) + "(" + strReceiveData + ")" + " 전송결과를 수신하였습니다.";

				return true;
			}
			catch (Exception ex)
			{
				strMessage = Encoding.ASCII.GetString(byteSendData) + "(" + strReceiveData + ")" + " 정보를 전송하는 중에 오류가 발생했습니다. ==> " + ex.Message;
				return false;
			}
		}

		public bool SetCheckValue(string strCheck)
		{
			string strMessage = "";

			try
			{
				// Check Value을 적용한다.
				//strCheckValue = strCheck;

				// 수신되었던 정보를 초기화한다.
				ResetReceive();

				//strMessage = strCheckValue + " check value을 처리했습니다";
				return true;
			}
			catch (Exception ex)
			{
				//strMessage = strCheckValue + " check value을 처리하는 중에 오류 발생하였습니다. => " + ex.Message;
				return false;
			}
		}

		public bool ResetReceive()
		{
			string strMessage = "";

			try
			{
				// 수신되었던 정보를 초기화한다.
				strReceive = "";
				strReceive_Temp = "";

				strMessage = "수신되었던 정보를 Reset 처리했습니다";
				return true;
			}
			catch (Exception ex)
			{
				strMessage = "수신되었던 정보를 Reset 처리하는 중에 오류 발생하였습니다. => " + ex.Message;
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
