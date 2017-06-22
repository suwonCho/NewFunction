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
	/// clsTcp�� ���� ��� �����Դϴ�.
	/// </summary>
	public class clsTcp
	{
		
		
		public static int port = 7000;
		


		public static string MyIp()
		{		

			string hostString = Dns.GetHostName();
			
			return ȣ��Ʈ�ּ�(hostString)[0];


		}


		public static string [] ȣ��Ʈ�ּ�(string DNS�ּ�)
		{
						
			try 
			{
				IPHostEntry hostInfo = Dns.GetHostByName(DNS�ּ�);

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
				
				/* ���⼭�� ��� ����
				///������ ������ ����
				byte [] data_size = new byte[4];
				data_size = BitConverter.GetBytes(size);
				Console.WriteLine("����" + data_size[0]);
				send_data = client.Send(data_size);
				*/

				//����������dat
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

		public static void send_data(Socket client, byte[] data, System.Net.EndPoint ipep)
		{
			try
			{
				
				
				int total = 0;
				int size = data.Length;
				int left_data = size;
				int send_data = 0;

				///������ ������ ����
				byte [] data_size = new byte[4];
				data_size = BitConverter.GetBytes(size);
				Console.WriteLine("����" + data_size[0]);
				send_data = client.Send(data_size);

				//����������dat
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

				//������ ũ��
				byte[] data_size = new byte [4];
				recv_data = client.Receive(data_size,0,4,SocketFlags.None);
				Console.WriteLine("����" + data_size[0]);
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
	
    /*
	/// <summary>
	/// ������� ������ ����� Ŭ����
	/// </summary>
	class TcpServer
	{
		
		private Thread server_th;
		Socket server = null;
		ArrayList Client_List = new ArrayList();
		public int port = 9000;
		public TextBox tbox = null;	
		private int Client_Count = 10;
		public Client [] client = null;
		public ComboBox cbox = null;
        public int Last_ClientNo = 0;
        public tcp_server.Client.del����ó�� delDataRecieve;
		public tcp_server.Client.del������� delDisconnect;
		
		public TcpServer()
		{
			client = new Client[Client_Count];
		}
		

		/// <summary>
		/// Ŭ���̾�Ʈ ���ڸ� �Ѱ��ش�.
		/// </summary>
		/// <returns></returns>
		public int client_�˻�()
		{
			int i = 0;
			try
			{
				for( i =0 ; i < Client_Count ; i++)
				{
					if (client[i] == null)
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
		/// ������� ��������
		/// </summary>
		private void ServerStart()
		{
			try
			{
				IPEndPoint ipep = new IPEndPoint(IPAddress.Any,port);
				server = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
				server.Bind(ipep);
				server.Listen(Client_Count);
				int i = this.client_�˻�();
				
				while(i != Client_Count)
				{
                    client[i] = new Client(server.Accept());
                    client[i].����ó�� = delDataRecieve;
                    client[i].������� = delDisconnect;
                    client[i].client_start(i);
                    this.Last_ClientNo = i;
					//Client_List.Add (string.Format("[{0}] {1}",i, ((IPEndPoint)client[i].client.RemoteEndPoint).Address.ToString()));
					//this.�޺���������(); 
					i = this.client_�˻�();

							
				}

			}
			catch(SocketException ex)
			{
				Console.WriteLine(ex.Message + "��������");
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message + "��������");
			}

		}
		

		/// <summary>
		/// ip��巹���� Ŭ���̾�Ʈ�� ���� ��Ų��.
		/// </summary>
		/// <param name="ip">Ŭ���̾�Ʈ ip</param>
		public void Client_Disconnected(string ip)
		{
			Client_List.Remove(ip);
			this.�޺���������();
			
										
		}


		public void Ŭ���̾�Ʈ��������(int i)
		{
			if ((client[i] != null) && (client[i].client != null))
			{
				client[i].������������();
			}
		}

		/// <summary>
		/// �޺��ڽ� ������ �ٽú����ش�.
		/// </summary>
		public void �޺���������()
		{
			if (cbox != null)
			{
				cbox.Items.Clear();
				this.�޺��������߰�(cbox,Client_List,false);
			}		
		}

		/// <summary>
		/// ComboBox�� ArrayList�� �Ѱ� �׸��߰�
		/// </summary>
		/// <param name="cmb">Combobox ��ü</param>
		/// <param name="str">arraylist �迭</param>
		/// <param name="�ڵ忩��">�׸� '[000]'��Ŀ� �ڵ� �߰�����</param>
		public void �޺��������߰�(System.Windows.Forms.ComboBox cmb,ArrayList str, bool �ڵ忩��)
		{
			int i = 0;
			string str2 = "";
			string ss;
			
			foreach(object s in str)
			{
				ss = (string) s;
				if (�ڵ忩��)
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

		public void ����ó�� (string data)
		{
			if (tbox != null)
			{
				tbox.AppendText(data + "\r\n");
   			}
		}
		


		/// <summary>
		/// ������ �����Ѵ�.
		/// </summary>
		public void ServerStop()
		{
			if ((this.server_th != null) && (this.server_th.IsAlive)) 
			{
				
				//��������
				server.Close();
				server_th.Abort();
				server_th.Join();
				//this.cbox.Items.Clear();
				Console.WriteLine(server_th.IsAlive.ToString());

				//�������ִ� Ŭ���̾�Ʈ�� ����
				int i = 0;
				try
				{
					for( i =0 ; i < Client_Count ; i++)
					{
						if (client[i].client != null)
						{
							client[i].������������();
							client[i] = null;
				
						}
					}
					
				}
				catch
				{
					
				}

			}
			Console.WriteLine("��������");
		}
		
		public bool SererStatus()
		{
				if ((this.server_th != null) && (this.server_th.IsAlive))
					return true;
				else
					return false;
			
			
		}

		

		/// <summary>
		/// ������ �����Ѵ�.
		/// </summary>
		public void svr_start()
		{
			if ((this.server_th != null) && (this.server_th.IsAlive)) server_th.Abort();

			this.server_th = new Thread(new ThreadStart(ServerStart));
			server_th.IsBackground = true;
			server_th.Start();
			Console.WriteLine("��������");
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
	*/



	public class TcpServer
	{
        public Socket client;
        public Socket Server = null;
		
        Thread th_ReadyRecieveData = null;
		Thread th_ReadyConnect = null;

		public delegate void delRecieveData(byte [] data);
		public delegate void delDisconnect();

        /// <summary>
        /// ���Ž� �߻��ϴ� ��������Ʈ
        /// </summary>
        public delRecieveData evtRecieveData = null;
		public delDisconnect evtDisconnect = null;

        IPEndPoint ipep;

        private bool EanbleToStart = false;

        private int portNo = 9000;
        public int PortNo
        {
            get { return portNo; }
            set
            {
                if ((th_ReadyConnect !=null) && (this.th_ReadyConnect.ThreadState == ThreadState.Background))
                {
                    MessageBox.Show("������ �����߿��� ��Ʈ ������ �Ұ����մϴ�.");
                    return;
                }
                portNo = value;                
            }
        }


        int Count = 0;
        string client_ip = "";
		
        
        public TcpServer()
		{
            
        }

        private void Bind_ipep()
        {
            try
            {
                if (ipep == null)
                    ipep = new IPEndPoint(IPAddress.Any, portNo);
                
                ipep.Port = PortNo;
                Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Server.Bind(ipep);
                Server.Listen(1);
                EanbleToStart = true;
            }
            catch
            {                
                EanbleToStart = false;
				throw;
            }
        }

		public bool InOpened
		{
			get
			{
				if (Server == null || th_ReadyConnect== null) return false;

				return th_ReadyConnect.ThreadState == ThreadState.Background;
			}
		}

        public bool Server_Stats
        {
            get
            {
                if (client == null) return false;
				return Server.Connected;
            }
        }
    

        /// <summary>
        /// Ŭ���̾�Ʈ �����带 �����Ѵ�.(���ú� & üũ)
        /// </summary>
        public bool Server_start()
        {
			Bind_ipep();

            if (this.EanbleToStart)
            {
                th_ReadyConnect = new Thread(new ThreadStart(th_Server_start));
                th_ReadyConnect.Name = "Tread Ready to Connect Clinet";
                th_ReadyConnect.IsBackground = true;
                th_ReadyConnect.Start();
                return true;
            }
            else
                return false;
        }

		
		private void th_Server_start()
		{
            while (true)
            {
                
                    Count++;
                    Console.WriteLine("������� {0}", this.Count);
                    client = Server.Accept();
                    client_Conect();
                
            }				
		}

        public void Server_Stop()
        {   
            
            if ((client != null)) this.client_Close();

            if ((th_ReadyConnect != null) && (th_ReadyConnect.ThreadState == ThreadState.Background))
            {
                Server.Close();
                th_ReadyConnect.Abort();
                th_ReadyConnect.Join();
            }            
        }

        public void Dispose()
        {
            this.Server_Stop();

            if (Server != null)
            {
                Server.Close();
                Server = null;
            }
            if (ipep != null)
            {
                ipep = null;
            }
        }

        public void client_Conect()
        {
            IPEndPoint ip = (IPEndPoint)this.client.RemoteEndPoint;
            this.client_ip = ip.Address.ToString();

            th_ReadyRecieveData = new Thread(new ThreadStart(Receive));
            th_ReadyRecieveData.Name = "Clinet :" + client_ip;
            th_ReadyRecieveData.IsBackground = true;
            th_ReadyRecieveData.Start();

            Console.WriteLine("����");

            
        }

		public void client_Close()
		{
			if ((client != null) && (client.Connected)) client.Close();
            
		}
        	
		
		/// <summary>
		/// �޼����� �޴� ���
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
						//Console.WriteLine("[���������ͼ���:{0}] (int){1} (char){2}",cou,(int) rec[0], Convert.ToChar(rec[0]));
                        byte[] re = new byte[cou];
                        
						for(int k = 0; k < cou; k++)
						{	
							//ttt += string.Format("{0:X2}", rec[k]);
							re[k] = rec[k];
							//tt += (int)(rec[k]) + ",";
						}

						//byte [] se = new byte[2];
						//se[0] = 0x01;
						//se[1] = 0xff;

                        //Console.WriteLine("����" + ttt);
                        

						//string data2 = Encoding.Default.GetString(se);
						//string data = string.Format("{2}[���������ͼ���:{0}] {1}",cou,ttt,Thread.CurrentThread.Name);
						//Console.Write(ttt);
                        this.evtRecieveData(re);
						
					
					}
					cou = 0;
					
					/*
					Console.WriteLine("���������ͼ��� : " + cou);

					string data = Encoding.Default.GetString(rec);
					//	Console.Write(Convert.ToChar(rec[0]) + "_" + (int) rec[0] + "_");
					
					for(int i = 0 ; i < cou ; i++)
					���� += (int) rec[i] + ".";
					*/
					
					

					//Console.WriteLine("���������� : " + ����);

					
					//����ó��(data);
					
					
				}
			}
			catch(SocketException ex)
			{
				Console.WriteLine(ex.Message + "(���ź�)");
			}
			catch(Exception ex)
			{
                Console.WriteLine(ex.Message + "(���ź�)");
			}
		}
		

		/// <summary>
		/// Ŭ���̾�Ʈ�� �������� ����
		/// </summary>
		/// <param name="msg">���� �޽���</param>
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

        /// <summary>
        /// Ŭ���̾�Ʈ�� �������� ����
        /// </summary>
        /// <param name="msg">���� �޽���</param>
        public void Send(byte [] msg)
        {

            try
            {
               clsTcp.send_data(client, msg);

            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

		



	}

}
