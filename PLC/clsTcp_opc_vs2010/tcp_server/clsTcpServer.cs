using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Collections;

namespace tcp_server
{
	/// <summary>
	/// clsTcp에 대한 요약 설명입니다.
	/// </summary>
	public class clsTcp
	{
		
		
		public static int port = 7000;
		


		public static string MyIp()
		{		

			string hostString = Dns.GetHostName();
			
			return 호스트주소(hostString)[0];


		}


		public static string [] 호스트주소(string DNS주소)
		{
						
			try 
			{
				IPHostEntry hostInfo = Dns.GetHostByName(DNS주소);

				System.Net.IPAddress [] host = hostInfo.AddressList ;
			
				string [] add = new string[host.Length];
			
				int i = 0;
				foreach(IPAddress ip in host)
				{
					add[i] = ip.ToString();
					i++;
				}
			
				return add;

				

			}

			catch(SocketException e) 

			{

				Console.WriteLine("SocketException caught!!!");

				Console.WriteLine("Source : " + e.Source);

				Console.WriteLine("Message : " + e.Message);

			}

			catch(ArgumentNullException e)

			{

				Console.WriteLine("ArgumentNullException caught!!!");

				Console.WriteLine("Source : " + e.Source);

				Console.WriteLine("Message : " + e.Message);

			}

			catch(Exception e)

			{

				Console.WriteLine("Exception caught!!!");

				Console.WriteLine("Source : " + e.Source);

				Console.WriteLine("Message : " + e.Message);

			}
			
			return null;
			


		}

		public static void send_data(Socket client, byte[] data)
		{
			try
			{
				
				
				int total = 0;
				int size = data.Length;
				int left_data = size;
				int send_data = 0;
				
				/* 여기서는 사용 않함
				///전송할 데이터 전송
				byte [] data_size = new byte[4];
				data_size = BitConverter.GetBytes(size);
				Console.WriteLine("전송" + data_size[0]);
				send_data = client.Send(data_size);
				*/

				//데이터전송dat
				while(total < size)
				{
					send_data = client.Send(data, total, left_data, SocketFlags.None);
					total += send_data;
					left_data += send_data;
				}


			}
			catch(System.Net.Sockets.SocketException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			
		}
		public static void send_data2(Socket client, byte[] data, System.Net.EndPoint ipep)
		{
			try
			{
				
				
				int total = 0;
				int size = data.Length;
				int left_data = size;
				int send_data = 0;

				///전송할 데이터 전송
				byte [] data_size = new byte[4];
				data_size = BitConverter.GetBytes(size);
				Console.WriteLine("전송" + data_size[0]);
				send_data = client.Send(data_size);

				//데이터전송dat
				while(total < size)
				{
					//send_data = client.Send(data, total, left_data, SocketFlags.None);
					send_data = client.SendTo(data, total, left_data, SocketFlags.None, ipep);

					total += send_data;
					left_data += send_data;
				}


			}
			catch(System.Net.Sockets.SocketException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			
		}
		
		public static byte[] recive_data(Socket client)
		{
			try
			{
				int total = 0;
				int size = 0;
				int left_data = 0;
				int recv_data = 0;

				//수신할 크기
				byte[] data_size = new byte [4];
				recv_data = client.Receive(data_size,0,4,SocketFlags.None);
				Console.WriteLine("수신" + data_size[0]);
				size = BitConverter.ToInt16(data_size,0);
				left_data = size;

				byte [] data = new byte[size];

				while(total < size)
				{
					recv_data = client.Receive(data, total, left_data, SocketFlags.None);
					if(recv_data == 0) break;
					total += recv_data;
					left_data -= recv_data;
					Console.WriteLine(Encoding.Default.GetString(data));
				}

				return data;

			}
			catch(System.Net.Sockets.SocketException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return null;


		}
	}
	

	/// <summary>
	/// 소켓통신 서버를 만드는 클래스
	/// </summary>
	class Network
	{
		
		private Thread server_th;
		Socket server = null;
		ArrayList Client_List = new ArrayList();
		public int port = 9000;
		public TextBox tbox = null;	
		private int Client_Count = 100;
		Client [] client = null;
		public ComboBox cbox = null;
		
		public Network()
		{
			client = new Client[Client_Count];
		}
		

		/// <summary>
		/// 클라이언트 숫자를 넘겨준다.
		/// </summary>
		/// <returns></returns>
		public int client_검사()
		{
			int i = 0;
			try
			{
				for( i =0 ; i < Client_Count ; i++)
				{
					if (client[i].client == null)
					{
						return i;
				
					}
				}
				return Client_Count;
			}
			catch
			{
				return i;
			}

		}
		


		/// <summary>
		/// 쓰레드용 서버시작
		/// </summary>
		private void ServerStart()
		{
			try
			{
				IPEndPoint ipep = new IPEndPoint(IPAddress.Any,port);
				server = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
				server.Bind(ipep);
				server.Listen(Client_Count);
				int i = this.client_검사();
				
				while(i != Client_Count)
				{
					
					client[i] = new Client(server.Accept(),i);
					client[i].수신처리 = new tcp_server.Client.del수신처리(수신처리);
					client[i].연결끊김 = new tcp_server.Client.del연결끊김(this.Client_Disconnected);
					Client_List.Add (string.Format("[{0}] {1}",i, ((IPEndPoint)client[i].client.RemoteEndPoint).Address.ToString()));
					this.콤보리프레쉬();
 
					i = this.client_검사();

							
				}

			}
			catch(SocketException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

		}
		

		/// <summary>
		/// ip어드레스로 클라이언트를 종료 시킨다.
		/// </summary>
		/// <param name="ip">클라이언트 ip</param>
		public void Client_Disconnected(string ip)
		{
			Client_List.Remove(ip);
			this.콤보리프레쉬();
			
										
		}


		public void 클라이언트접속해제(int i)
		{
			if ((client[i] != null) && (client[i].client != null))
			{
				client[i].강제접속해제();
			}
		}

		/// <summary>
		/// 콤보박스 정보를 다시보여준다.
		/// </summary>
		public void 콤보리프레쉬()
		{
			if (cbox != null)
			{
				cbox.Items.Clear();
				this.콤보데이터추가(cbox,Client_List,false);
			}		
		}

		/// <summary>
		/// ComboBox에 ArrayList를 넘겨 항목추가
		/// </summary>
		/// <param name="cmb">Combobox 객체</param>
		/// <param name="str">arraylist 배열</param>
		/// <param name="코드여부">항목에 '[000]'양식에 코드 추가여부</param>
		public void 콤보데이터추가(System.Windows.Forms.ComboBox cmb,ArrayList str, bool 코드여부)
		{
			int i = 0;
			string str2 = "";
			string ss;
			
			foreach(object s in str)
			{
				ss = (string) s;
				if (코드여부)
				{
					i ++;
					if (i < 10 )
					{
						str2 = string.Format("[00{0}] {1}",i.ToString(),ss);

					}
					else if (i < 100)
					{
						str2 = string.Format("[0{0}] {1}",i.ToString(),ss);
					}
					else
					{
						str2 = string.Format("[{0}] {1}",i.ToString(),ss);
					}


					cmb.Items.Add(str2);			

				}
				else
				{
					cmb.Items.Add(ss);
				}
			}
			cmb.SelectedIndex = -1 ;
												
		}

		public void 수신처리 (string data)
		{
			if (tbox != null)
			{
				tbox.AppendText(data + "\r\n");
   			}
		}
		


		/// <summary>
		/// 서버를 중지한다.
		/// </summary>
		public void ServerStop()
		{
			if ((this.server_th != null) && (this.server_th.IsAlive)) 
			{
				
				//서버중지
				server.Close();
				server_th.Abort();
				server_th.Join();
				this.cbox.Items.Clear();
				Console.WriteLine(server_th.IsAlive.ToString());

				//접속해있는 클라이언트들 정리
				int i = 0;
				try
				{
					for( i =0 ; i < Client_Count ; i++)
					{
						if (client[i].client != null)
						{
							client[i].강제접속해제();
							client[i] = null;
				
						}
					}
					
				}
				catch
				{
					
				}

			}
			Console.WriteLine("서버종료");
		}
		
		public bool SererStatus()
		{
				if ((this.server_th != null) && (this.server_th.IsAlive))
					return true;
				else
					return false;
			
			
		}

		

		/// <summary>
		/// 서버를 시작한다.
		/// </summary>
		public void svr_start()
		{
			if ((this.server_th != null) && (this.server_th.IsAlive)) server_th.Abort();

			this.server_th = new Thread(new ThreadStart(ServerStart));
			server_th.IsBackground = true;
			server_th.Start();
			Console.WriteLine("서버시작");
		}

		public string Get_MyIP()
		{
			return clsTcp.MyIp();
		}

		public void Send_all(string msg)
		{			
			try
			{
				for(int i = 0; i < Client_Count; i++)
				{
					if((client[i] != null) && (client[i].client != null))
					{
						byte [] data = Encoding.Default.GetBytes(msg);
						clsTcp.send_data(client[i].client,data);
					}
				}
				
			}
			catch(SocketException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		

	}
	



	/// <summary>
	/// 클라이언트
	/// </summary>
	public class Client
	{
		public Socket client = null;
		Thread th = null;
		int client_no;
		string client_ip = "";
		Thread chkClient = null;
		public delegate void del수신처리(string data);
		public delegate void del연결끊김(string i);
		public del수신처리 수신처리 = null;
		public del연결끊김 연결끊김 = null;
		private string 숫자;

		public Client(Socket cc, int i)
		{
			client = cc;
			client_no = i;
				
			/*
			Thread client_th = new Thread(new ThreadStart(client_start));
			client_th.IsBackground = true;
			client_th.Start();
			*/
			client_start(i);

		}
		

		/// <summary>
		/// 클라이언트 쓰레드를 시작한다.(리시브 & 체크)
		/// </summary>
		/// <param name="i">콤보박스 번호</param>
		public void client_start(int i)
		{
			IPEndPoint ip = (IPEndPoint)this.client.RemoteEndPoint;
			this.client_ip = ip.Address.ToString();

			th = new Thread(new ThreadStart(Receive));
			th.Name = "[" + i + "]" + client_ip;
			th.IsBackground = true;
			th.Start();
            
			Console.WriteLine("성공");

			chkClient = new Thread(new ThreadStart(this.ckh_Client));
			chkClient.Name = Thread.CurrentThread.Name;
			chkClient.IsBackground = true;
			chkClient.Start();
		}

		public void 강제접속해제()
		{
			client.Close();
		}

		

		/// <summary>
		/// 클라이언트가 접속에 문제 있는지 체크 - 1초 단위
		/// </summary>
		public void ckh_Client()
		{	
			int i = 0 ;
			while (client.Connected)
			{
				i ++;
				Thread.Sleep(1000);
				//Console.WriteLine(Thread.CurrentThread.Name + "_연결이상무....");
			}

			연결끊김(string.Format("[{0}] {1}",client_no, this.client_ip));
			client.Close();
			client = null;
			
			Thread.CurrentThread.Abort();
			

		}
		
		/// <summary>
		/// 메세지를 받는 경우
		/// </summary>
		public void Receive()
		{
			
			try
			{
				while((client != null) && (client.Connected))
				{
					/*
					recv_data = client.Receive(data, total, left_data, SocketFlags.None);
					if(recv_data == 0) break;
					total += recv_data;
					left_data -= recv_data;
					Console.WriteLine(Encoding.Default.GetString(data));
					*/

					int cou;
					byte [] rec = new byte[100];
					cou = client.Receive(rec,0, 100,System.Net.Sockets.SocketFlags.None);
					
					if (cou !=0)
					{
						//Console.WriteLine("[받은데이터숫자:{0}] (int){1} (char){2}",cou,(int) rec[0], Convert.ToChar(rec[0]));
						byte [] re = new byte[cou];
						string tt = "";
						string ttt = "";
						for(int k = 0; k < cou; k++)
						{	
							ttt += string.Format("{0:X2}", rec[k]);
							re[k] = rec[k];
							tt += (int)(rec[k]) + ",";
						}

						byte [] se = new byte[2];
						se[0] = 0x01;
						se[1] = 0xff;

						string data2 = Encoding.Default.GetString(se);
						string data = string.Format("{2}[받은데이터숫자:{0}] {1}",cou,ttt,Thread.CurrentThread.Name);
						//Console.Write(ttt);
						수신처리(data);
						Send(data2);
					
					}
					cou = 0;
					
					/*
					Console.WriteLine("받은데이터숫자 : " + cou);

					string data = Encoding.Default.GetString(rec);
					//	Console.Write(Convert.ToChar(rec[0]) + "_" + (int) rec[0] + "_");
					
					for(int i = 0 ; i < cou ; i++)
					숫자 += (int) rec[i] + ".";
					*/
					
					

					//Console.WriteLine("받은데이터 : " + 숫자);

					
					//수신처리(data);
					
					
				}
			}
			catch(SocketException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		

		/// <summary>
		/// 클라이언트에 메제지를 보냄
		/// </summary>
		/// <param name="msg">보낼 메시지</param>
		public void Send(string msg)
		{
			
			try
			{
				byte [] data = Encoding.Default.GetBytes(msg);
				clsTcp.send_data(client,data);
				
			}
			catch(SocketException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		



	}

}
