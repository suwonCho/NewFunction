using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

using CAP_PARSER2Lib;
using System.Net.Sockets;
using System.Threading;
using Function;



namespace Function.EpcMW
{
	/// <summary>
	/// ���� junoF ���� ��Ʈ�� Ŭ����..
	/// </summary>
	public partial class clsJuno : IDisposable
	{
		Socket socket;                      //���� ��ü�� �����մϴ�.
		ParserMainClass parser;             //�ļ� ���̺귯�� ��ü�� �����մϴ�.

		private Thread threadSockect = null;    //���ϰ��� �������Դϴ�.
        private Thread threadAlive = null;    //���ϰ��� �������Դϴ�.
		private bool bReadSocket = false;       //������ �÷��� �Դϴ�.

        private DateTime dtLastGetVersion = DateTime.Now;

		private Function.Util.Log clsLog;

		string strIP;
		int intPort;
		/// <summary>
		/// ��񿡼� �ö�¿µ�..
		/// </summary>
		int inttemp;

		public int intTemperature;


		public enum enTagType
		{
			/// <summary>
			/// ��ȯ ���� �ʴ´�.
			/// </summary>
			NONE,
			/// <summary>
			/// EPC CODE�� ��ȯ
			/// </summary>
			EPCCODE,
			/// <summary>
			/// ASCII CODE�� ��ȯ
			/// </summary>
			ASCII
		};

		/// <summary>
		/// �±� ��ȯ Ÿ��
		/// </summary>
		public enTagType TagType = enTagType.EPCCODE;

		struct stReadingTagInfo
		{
			/// <summary>
			/// ���� port ��ȣ
			/// </summary>
			public int intPortNo;
			/// <summary>
			/// ���� tagid
			/// </summary>
			public string strTagID;
		}

		/// <summary>
		/// ó���� ���� �ױ� id
		/// </summary>
		private stReadingTagInfo infoTagID = new stReadingTagInfo();

	
		/// <summary>
		/// �������� �ױ������� ������ ����� �ļ����� �м��Ͽ� Host�� �˷��ִ� �̺�Ʈ ��������Ʈ
		/// </summary>
		/// <param name="strEpcTagID">Tag Data (Hex)</param>
		/// <param name="isError">��������</param>
		/// <param name="strMsg">�޽���</param>
		/// <param name="readingPort">���׳� ��Ʈ</param>		
		public delegate void delOnTagReadCommandAck
					(string strTagID, bool isError, string strMsg,int intreadingPort);
		/// <summary>
		/// �������� �ױ������� ������ ����� �ļ����� �м��Ͽ� Host�� �˷��ִ� �̺�Ʈ �ݹ� �Լ�
		/// </summary>
		public delOnTagReadCommandAck evtOnTagReadCommandAck;

		/// <summary>
		/// ������ writing ��� �� ����� �޴� �̺�Ʈ ��������Ʈ
		/// </summary>
		/// <param name="isError"></param>
		/// <param name="intPort"></param>
		/// <param name="strMsg"></param>
		public delegate void delOnTagWriteCommandAck(bool isError, int intPort, string strMsg);
		/// <summary>
		/// ������ writing ��� �� ����� �޴� �̺�Ʈ �ݹ� �Լ�
		/// </summary>
		public delOnTagWriteCommandAck evtOnTagWriteCommandAck;

		/// <summary>
		/// ������ ���� ���� ������ �޴� �̺�Ʈ ��������Ʈ
		/// </summary>
		/// <param name="strAddr"></param>
		/// <param name="isError"></param>
		/// <param name="strMsg"></param>
		/// <param name="intaddrValue"></param>
		public delegate void delConfigurationAck(string strAddr, bool isError, string strMsg, int intaddrValue);
		/// <summary>
		/// Configuration ���� �̺�Ʈ �ݹ��Լ�
		/// </summary>
		delConfigurationAck evtGetConfigurationAck = null;
		/// <summary>
		/// Configuration ���� �� ���� ���� �̺�Ʈ �Դϴ�.
		/// </summary>
		delConfigurationAck evtSetConfigurationAck = null;

		/// <summary>
		/// ���� üũ��.. ������ ���Žÿ� ���� �ð����� ������Ʈ
		/// </summary>
		DateTime dtAliveCheck;

		/// <summary>
		/// ������ dio input port�� ���� ����(on) �̺�Ʈ ��������Ʈ
		/// </summary>
		/// <param name="Port1"></param>
		/// <param name="Port2"></param>
		/// <param name="Port3"></param>
		/// <param name="Port4"></param>
		public delegate void delDioInPortAck(bool Port1, bool Port2, bool Port3, bool Port4);

		/// <summary>
		/// ������ dio input port�� ���� ����(on) �̺�Ʈ ��������Ʈ
		/// </summary>
		public delDioInPortAck evtDioInPortAck;




		public clsJuno(string strip, int intport, string strLogFileName)
		{

			strIP = strip;
			intPort = intport;

			clsLog = new Function.Util.Log(@".\JunoF", strLogFileName, 10, true);

			if (!InitRFSet()) //RF parser ���̺귯���� ����ϱ� ���� �ʱ�ȭ �����۾��Դϴ�. �̺�Ʈ�ݹ��Լ��� ���⼭ ����մϴ�.
			{
				throw new Exception("RF Set error");
			}


			//// Write�� MaxLength �� �����ϴ� UI ���� �����Դϴ�.
			//int nSize = Convert.ToInt32(tbx_write_size.Text) * 2;
			//if (rbn_writeTag_Hex.Checked == true)
			//    tbx_writeTag_Input.MaxLength = nSize * 2;
			//else
			//    tbx_writeTag_Input.MaxLength = nSize;

		}

		/// <summary>
		/// �ļ� ���� ��ü�� �޸𸮿� �ε��մϴ�. �̺�Ʈ�ݹ��Լ��� ����մϴ�.
		/// </summary>
		/// <returns></returns>
		private bool InitRFSet()
		{
			bool init_state = false;

			if (!init_state)
			{
				parser = new ParserMainClass();             // �ļ� ��ü�� �޸𸮿� �ε��մϴ�.
				parser.CapVersion = CeyonProtocol.CAP22;    // JUNOF ������ ������ ���������� CAP2.2 �Դϴ�. 
				parser.Tag_Type = Tag_Type.Gen2;            // Tag Ÿ���� 'Gen2'�Դϴ�.

				//�̺�Ʈ �ݹ��Լ� ��� : Tag ������ ������� ���� ������ Host�� �˷��ݴϴ�.
                parser.OnGetInformationAck += new _IParserMainEvents_OnGetInformationAckEventHandler(parser_OnGetInformationAck);

				parser.OnControlCommandAck += new _IParserMainEvents_OnControlCommandAckEventHandler(parser_OnControlCommandAck);

				parser.OnExternDeviceControlAck += new _IParserMainEvents_OnExternDeviceControlAckEventHandler(parser_OnExternDeviceControlAck);

				parser.OnTagReadCommandAck += new _IParserMainEvents_OnTagReadCommandAckEventHandler(parser_OnTagReadCommandAck);

				parser.OnTagWriteCommandAck += new _IParserMainEvents_OnTagWriteCommandAckEventHandler(parser_OnTagWriteCommandAck);

				parser.OnGetConfigurationAck += new _IParserMainEvents_OnGetConfigurationAckEventHandler(parser_OnGetConfigurationAck); //Configuration ���� �̺�Ʈ �Դϴ�.

				parser.OnSetConfigurationAck += new _IParserMainEvents_OnSetConfigurationAckEventHandler(parser_OnSetConfigurationAck); //Configuration ���� �� ���� ���� �̺�Ʈ �Դϴ�.

                

				init_state = true;
			}
			else
			{
				init_state = false;
			}

			return init_state;
		}


		bool boolAckDio = false;
		/// <summary>
		/// ���� ��Ʈ�ѽ� ����
		/// </summary>
		/// <param name="Port"></param>
		/// <param name="DeviceType"></param>
		/// <param name="sts"></param>
		void parser_OnExternDeviceControlAck(short Port, short DeviceType, Status sts)
		{
			if (Port == 0 && DeviceType == 0x3 && sts == Status.No_Error)
			{	//output port ��Ʈ�ѿ� ���� ����
				clsLog.WLog("Dio Output Port ó�� ���� ����");
				boolAckDio = true;
			}
			else if(Port == 4)
			{	//input port ���¿� ��ȭ(on)�� ���� �̺�Ʈ
				bool Port1, Port2, Port3, Port4;

				Port1 = Port2 = Port3 = Port4 = false;

				if ((DeviceType & 0x1) == 0x1 )
					Port1 = true;

				if ((DeviceType & 0x2) == 0x2)
					Port2 = true;

				if ((DeviceType & 0x4) == 0x4)
					Port3 = true;

				if ((DeviceType & 0x8) == 0x8)
					Port4 = true;

				clsLog.WLog(string.Format("Dio Input Port ���º��� : [Port1]{0} [Port2]{1} [Port3]{2} [Port4]{3}", Port1, Port2, Port3, Port4) );


				if (evtDioInPortAck != null)
					evtDioInPortAck(Port1, Port2, Port3, Port4);			


			}
		}


		void parser_OnControlCommandAck(ControlCommand cmd, Status sts)
		{
			clsLog.WLog("CommandAck:" + cmd.ToString());
		}


		/// <summary>
		/// ���� ��û�� ���� ����.. ���� �߹��� üũ�� �̿�..
		/// </summary>
		/// <param name="info"></param>
		/// <param name="infoString"></param>
		void parser_OnGetInformationAck(Information info, string infoString)
		{
			if (Information.Get_Firmware_Version == info && infoString != string.Empty)
			{
				clsLog.WLog(string.Format("Ack Req Firmware info : {0}", infoString));
			}

		}




		private void parser_OnTagWriteCommandAck(short Port, Status sts)
		{
			bool isError = false;
			if (sts != Status.No_Error)
				isError = true;
			
			if(evtOnTagWriteCommandAck != null)
				evtOnTagWriteCommandAck(isError, (int)Port, fctConvertState(sts));

		}



		void parser_OnSetConfigurationAck(RegisterAddr addr, Status st, short addrValue)
		{
			if (addr == RegisterAddr.VTO)
			{
				bool isError = false;
				if (st != Status.No_Error) isError = true;

				if (evtSetConfigurationAck != null)
					evtSetConfigurationAck(addr.ToString(), isError, fctConvertState(st), addrValue);
			}


		}


		void parser_OnGetConfigurationAck(RegisterAddr addr, Status st, short addrValue)
		{
			bool isError = false;
			if (st == Status.No_Error) isError = true;

			if (evtSetConfigurationAck != null)
					evtSetConfigurationAck(addr.ToString(), isError, fctConvertState(st), addrValue);
			
		}

		///// <summary>
		///// Write ���� �� ���¸� �˷��ִ� �̺�Ʈ�Դϴ�.
		///// </summary>
		///// <param name="Port"></param>
		///// <param name="sts"></param>
		//void parser_OnTagWriteCommandAck(short Port, Status sts)
		//{
		//    tbx_writeTagResult.Text = fctConvertState(sts);
		//}


		// CAP Parser�� Satae ���� Description ����
		public string fctConvertState(Status status)
		{
			string sReturn = "";
			switch ((int)status)
			{
				case 0:
					sReturn = "No Error"; break;
				case 1:
					sReturn = "Unknown command"; break;
				case 2:
					sReturn = "Invalid register address"; break;
				case 3:
					sReturn = "Invalid register value"; break;
				case 4:
					sReturn = "No tag"; break;
				case 5:
					sReturn = "Tag CRC check fail"; break;
				case 6:
					sReturn = "Writing fail"; break;
				case 7:
					sReturn = "FCS/CRC16 mismatch"; break;
				case 8:
					sReturn = "Unknown tag command"; break;
				case 9:
					sReturn = "Invalid tag command data"; break;
				case 0x0A:
					sReturn = "Invalid command data length"; break;
				case 0x0B:
					sReturn = "Event report mode error"; break;
				case 0x0C:
					sReturn = "RF off"; break;
				case 0x0D:
					sReturn = "Scan timer expire"; break;
				case 0x0E:
					sReturn = "Password mismatch"; break;
				case 0x0F:
					sReturn = "RF Disable Channel"; break;
				case 0x10:
					sReturn = "RF None Channel"; break;
				case 0x11:
					sReturn = "Mode error - User/Super user mode"; break;
				case 0x20:
					sReturn = "RF mode error"; break;
				case 0x21:
					sReturn = "RF Reader error"; break;
				case 0x2A:
					sReturn = "Sensor Active (Load)"; break;
				case 0x2B:
					sReturn = "Sensor Inactive(Unload)"; break;
				case 0x30:
					sReturn = "RF Antenna Port Diagnosis Error"; break;
				case 0x31:
					sReturn = "RF Antenna Diagnosis Error"; break;
				case 0x32:
					sReturn = "RF AMBIENT Temperature Alarm"; break;//[Temperature]
				case 0x33:
					sReturn = "RF Power Amp Temperature Alarm"; break;//[Temperature]
				case 0x34:
					sReturn = "RF Chip Temperature Alarm(1)"; break;//[Temperature]
				case 0x35:
					sReturn = "RF Chip Temperature Alarm(2)"; break;//[Temperature]
				default:
					sReturn = "Unknown command"; break;
			}
			return sReturn;
		}



		/// <summary>
		/// Rfid�� ���Ͽ��� ������
		/// </summary>		
		public void Open()
		{
           
            if (socket != null && socket.Connected == true) return;

            clsLog.WLog(string.Format("������ �õ� �մϴ�. [IP]{0} [PORT]{1}", strIP, intPort));

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(strIP, intPort);

            if (socket.Connected == false)
            {
                clsLog.WLog(string.Format("���ӽ��� : [IP]{0} [PORT]{1}", strIP, intPort));
                return;
            }

            clsLog.WLog(string.Format("���Ӽ���. [IP]{0} [PORT]{1}", strIP, intPort));

            if (!bReadSocket)
           { 
                bReadSocket = true;
                threadSockect = new Thread(new ThreadStart(T_ReadSocket)); //���Ͼ����带 �����մϴ�. 
                threadSockect.IsBackground = true;
                threadSockect.Start();


                threadAlive = new Thread(new ThreadStart(AliveCheck)); //���Ͼ����带 �����մϴ�. 
                threadAlive.IsBackground = true;
                threadAlive.Start();
            }

			dtAliveCheck = DateTime.Now;

		}

        /// <summary>
        /// ���� üũ ������ : �ֱ������� GetFirmwareVersion data�� ���� ���� üũ�� �Ѵ�.
        /// </summary>
        private void AliveCheck()
        {

			bool isReOpen = false;

			while (bReadSocket)
			{
				try
				{
					//����ð� ���� ���üũ�� �ð��� ��..
					if (dtAliveCheck > DateTime.Now)
						dtAliveCheck = DateTime.Now;


					//30���̻� ����� ������ ��� üũ�� �Ѵ�.
					if (dtAliveCheck < DateTime.Now.AddSeconds(-30))
					{
						if (socket.Connected)
						{
							byte[] byt = (byte[])parser.GetFirmwareVersion();
							clsLog.WLog("Alive Check - Send Firmware Check");
							Send_Packet(byt);
						}
					}
					else
					{
						isReOpen = false;
					}

					//10�� �ѹ���..
					Thread.Sleep(10000);

				}
				catch (Exception ex)
				{
					clsLog.WLog_Exception("AliveCheck", ex);
				}


				//1���̻� ����� ������ ���� ó���� �Ѵ�.
				if (dtAliveCheck < DateTime.Now.AddSeconds(-60))
				{
					try
					{
						if (!isReOpen) clsLog.WLog("TCP Conection ���� 3ȸ �̻� �߻� : ���� ���� �� �� ���ӽõ�");

						isReOpen = true;

						if (socket.Connected)
						{
							socket.Shutdown(SocketShutdown.Both);  //������ �� �ޱ� ��� ����� �� ������ �մϴ�.
							socket.Close();
						}

						Thread.Sleep(500);

						Open();

					}
					catch
					{
						clsLog.WLog("TCP Conection Open ����");
						Thread.Sleep(5000);
					}

				}


			}
        }





		/// <summary>
		/// ���Ͼ������Դϴ�. TCP������ �Ѿ���� ��Ŷ�� �����մϴ�. ������ ��Ŷ�� ������ parser �� �����ϴ�. 
		/// </summary>
		private void T_ReadSocket()
		{
			int intError = 0;
			while (bReadSocket)
			{
				try
				{
					if (socket.Connected)
					{
						int nPacket = socket.Available; //��Ʈ��ũ���� �޾Ƽ� ���� �� �ִ� ������ ���� �����մϴ�.

						if (nPacket > 0)
						{
							//���� üũ�ð� ������Ʈ - ��ȣ�� ������ ������Ʈ
							dtAliveCheck = DateTime.Now;

							byte[] bytePacket = new byte[nPacket];
							socket.Receive(bytePacket, nPacket, 0);


							object obj = (object)bytePacket;

							if (socket.Connected == true)
								parser.FctSetReceiveData(ref obj);  //TCP��Ŵܿ��� ������κ��� ���� ��� ��Ŷ�� �ļ��� �����ϴ�.

							intError = 0;

						}
						else
						{
							Thread.Sleep(10);
						}
					}
					else
					{
						Thread.Sleep(10);
						intError++;
					}

				}
				catch (Exception ex)
				{
					clsLog.WLog_Exception("T_ReadSocket", ex);
					intError++;
				}


                //if (intError > 3)
                //{
                //    clsLog.WLog("TCP Conection ���� 3ȸ �̻� �߻� : ���� ���� �� �� ���ӽõ�");
					
                //    socket.Shutdown(SocketShutdown.Both);  //������ �� �ޱ� ��� ����� �� ������ �մϴ�.
                //    socket.Close();
					
                //    Thread.Sleep(500);
                //    Open();

                //    intError = 0;

                //}
			}
		}



		/// <summary>
		/// �������� �����͸� ������ �����Լ��Դϴ�.
		/// </summary>
		/// <param name="bytePacket">�����⿡ ���� ��Ŷ�Դϴ�.</param>
		private bool Send_Packet(byte[] bytePacket)
		{
			if (socket != null && socket.Connected == true)
			{
				socket.Send(bytePacket);

				return true;
			}
			else
				return false;
		}


		/// <summary>
		/// ������ �������¸� �����ϴ� ����� ������ �Լ��Դϴ�. 
		/// </summary>
		/// <param name="status">T:AutoRead F:Verbos</param>
		public void SetReadState(bool status)
		{
			Byte[] byteDate;


			if (status)
			{
				byteDate = (Byte[])parser.EasySetConfiguration(SpecialSetting.Set_AutoRead); //AutoRead ����
				clsLog.WLog("AUTOREAD ���·� ���� �Ǿ����ϴ�");
			}
			else
			{			
				byteDate = (Byte[])parser.EasySetConfiguration(SpecialSetting.Set_VerboseRead); //Verbose ����
				clsLog.WLog("Verbose ���·� ���� �Ǿ����ϴ�");
			}

			Send_Packet(byteDate);
		}



		/// <summary>
		/// �������� �ױ������� ������ ����� �ļ����� �м��Ͽ� Host�� �˷��ִ� �̺�Ʈ �ݹ� �Լ�
		/// </summary>
		/// <param name="data">Tag Data (Hex)</param>
		/// <param name="readmode"></param>
		/// <param name="tagType"></param>
		/// <param name="sts">Read ���°� , No_error �� ��� ���� ���� �Ϸ�</param>
		/// <param name="readingPort">Read �۾��� �� ���׳� ��Ʈ</param>
		/// <param name="remainedCount"></param>
		void parser_OnTagReadCommandAck(ref object data, TagReadMode readmode, Tag_Type tagType, Status sts, int readingPort, int remainedCount)
		{
			if (sts == Status.No_Error)
			{
				byte[] btData = (byte[])data;
				byte[] stData = new byte[12];
				DateTime dt = DateTime.Now;
				string temptTime = dt.ToString("o");


				string rfData = "";

				if ((btData.Length != 14))      // PC(2) + EPC(12)
				{
					// 12byte�� �ƴ� �����ʹ� EPC�ڵ庯ȯ �Ұ��̹Ƿ� ���͸��մϴ�. 
					return;
				}

				// 12byte EPC ������ �����մϴ�.
				for (int i = 2; i < btData.Length; i++)
				{
					stData[i - 2] = btData[i];
				}

				string strTagID = string.Empty;

				rfData = fctByteArray2HexString(stData, string.Empty); // ȭ�鿡 �����ֱ� ���� string Ÿ������ �����ϴ� �Լ��Դϴ�.

				switch(TagType)
				{
					case enTagType.EPCCODE:
						

						strTagID = clsEpcCode.Hex2EpcCode(rfData);

						clsLog.WLog(string.Format("Tag����:[EPC]{0} [HEX]{1}", strTagID, rfData));						

						break;
					 
					case enTagType.ASCII:

						for (int i = 0; i < rfData.Length; i += 2)
						{
							int intHex = Convert.ToInt32(rfData.Substring(i, 2), 16);
							strTagID += Convert.ToChar(intHex).ToString();
						}

						clsLog.WLog(string.Format("Tag����:[ASCII]{0} [HEX]{1}", strTagID, rfData));	

						break;

					case enTagType.NONE:
						strTagID = rfData;
						clsLog.WLog(string.Format("Tag����:[HEX]{0}", strTagID, rfData));
						break;
				}


				if (sts == Status.No_Error)
				{
					infoTagID.intPortNo = readingPort;
					infoTagID.strTagID = strTagID;
				}

				if(evtOnTagReadCommandAck != null)
					evtOnTagReadCommandAck(strTagID, false, fctConvertState(sts), readingPort);

			}
			else
			{
				clsLog.WLog(string.Format("Tag���ſ���:[ECODE]{0} [HEX]{1}", fctConvertState(sts), fctByteArray2HexString((byte[])data, string.Empty)));
				if(evtOnTagReadCommandAck != null)
					evtOnTagReadCommandAck(string.Empty, true, fctConvertState(sts), readingPort);
			}

		}


		/// <summary>
		/// ���׳��� ���� �ױ׸� �д´�.
		/// </summary>
		/// <param name="intPorts">port��ȣ�� 1~4</param>
		/// <param name="intDuration">���Žð�(ms)</param>
		/// <returns></returns>
		public string Reading_Sync(int [] intPorts, int intDuration)
		{
			short shtPort = 0x00;

			foreach (int intPort in intPorts)
			{
				switch (intPort)
				{
					case 1:
						shtPort |= 0x01;
						break;

					case 2:
						shtPort |= 0x02;
						break;

					case 3:
						shtPort |= 0x04;
						break;

					case 4:
						shtPort |= 0x08;
						break;

				}
				


			}

			infoTagID.intPortNo = 0;
			infoTagID.strTagID = string.Empty;

			//InventoryRead(shtPort);

			SetReadState(true);

			for (int i = 0; i < intDuration; i+= 200)
			{
				if (infoTagID.strTagID != string.Empty) break;

				Thread.Sleep(200);

			}

			SetReadState(false);

			if (infoTagID.strTagID != string.Empty)
				return string.Empty;
			else
				return infoTagID.strTagID;




		}



		/// <summary>
		/// Byte �迭�� Hex ��Ʈ������ ��ȯ(Hex ��Ʈ�� ���̿� ���й��� �Է� ó�� �߰�) 
		/// </summary>
		/// <param name="bytePacket"></param>
		/// <param name="cDelimiter"></param>
		/// <returns></returns>
		private static string fctByteArray2HexString(Byte[] bytePacket, string strDelimiter)
		{
			string sReturn = "";
			try
			{
				int nCount = bytePacket.Length;

				for (int i = 0; i < nCount; i++)
				{
					if (i == 0)
						sReturn += String.Format("{0:X2}", bytePacket[i]);
					else
						sReturn += String.Format("{0}{1:X2}", strDelimiter, bytePacket[i]);
				}
			}
			catch (Exception)
			{
				sReturn = "";
			}
			return sReturn;
		}



		/// <summary>
		/// ���� ������ �ݴ� �ڵ尡 �ִ� �Լ��Դϴ�.
		/// </summary>
		public void Close()
		{

			if (socket != null && socket.Connected)
			{
				//������ �������� ������ ���� ������ ����..
				//SetReadState(false);

                bReadSocket = false;

                Thread.Sleep(500);

				threadSockect.Abort();
				threadSockect.Join();

                threadAlive.Abort();
                threadAlive.Join();

				bReadSocket = false;

				socket.Shutdown(SocketShutdown.Both);  //������ �� �ޱ� ��� ����� �� ������ �մϴ�.
				socket.Close();
				Thread.Sleep(100);

			}

		}


		public void Dispose()
		{
			Close();
		}

		/// <summary>
		/// 4ä�� ���׳��� �����մϴ�. true���̸� �ش� ��Ʈ�� Ȱ��ȭ�ǵ��� ���õ˴ϴ�.
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="p3"></param>
		/// <param name="p4"></param>
		public void Set_Current_Antenna(bool p1, bool p2, bool p3, bool p4)
		{
			Register900M register900M = Register900M.A_TPE0;
			byte[] value = new byte[1];


			byte nValue = 0x00;

			if (p1 == true) nValue |= 0x01;
			if (p2 == true) nValue |= 0x02;
			if (p3 == true) nValue |= 0x04;
			if (p4 == true) nValue |= 0x08;




			value[0] = nValue;
			object obj = (object)value;

			Byte[] bytePacket = (Byte[])parser.SetGen2Configuration(register900M, ref obj);
			Send_Packet(bytePacket);
			
			clsLog.WLog(string.Format("���׳� ���� ����:[1]{0} [2]{1} [3]{2} [4]{3}",
							p1, p2, p3, p4));
		}

		/// <summary>
		/// Verbose ��忡�� �ױ� ���� ����� ������.
		/// </summary>
		/// <param name="shtPort">���׳� ��Ʈ</param>		
		private void InventoryRead(short shtPort)
		{
			Byte[] byteData;


			parser.readingPort = shtPort;
			byteData = (Byte[])parser.ReadInventory();

			clsLog.WLog(string.Format("InventoryRead : {0}", Fnc.Bytes2String(byteData)));

			Send_Packet(byteData);

		}


	
		/// <summary>
		/// Vto Ÿ���� �����Ѵ�. Verbose ���� �ױ� ���Žð�..
		/// </summary>
		/// <param name="intTime">ms : 1���̻� ����..</param>
		public void Set_Vto(int intTime)
		{
			try
			{
				Byte[] bytePacket;

				RegisterAddr addr = RegisterAddr.VTO;

				if(intTime < 1000) intTime = 1000;
				intTime = intTime / 100;

				short value = Convert.ToInt16(intTime.ToString());

				bytePacket = (Byte[])parser.SetConfiguration(addr, value);

				Send_Packet(bytePacket);

			}
			catch (Exception ex)
			{				
				clsLog.WLog_Exception("Set_Vto", ex);
				throw;
			}
		}

		/// <summary>
		///  Vto Ÿ���� �����´�. Verbose ���� �ױ� ���Žð�..
		/// </summary>
		public void Get_Vto()
		{
			try
			{
				Byte[] bytePacket;
				RegisterAddr addr = RegisterAddr.VTO;

				bytePacket = (Byte[])parser.GetConfiguration(addr);

				Send_Packet(bytePacket);

			}
			catch (Exception ex)
			{				
				clsLog.WLog_Exception("Get_Vto", ex);
				throw;
			}
		}

		/// <summary>
		/// TagId�� ����Ѵ�.
		/// </summary>
		/// <param name="strTag"></param>
		public void WriteTag(string strTag, short shtPort, short shtStartPointer, short sthWritingSize)
		{			
			try
			{
				Byte[] bytePacket = null;
				int nTextSize;
				short nStart = shtStartPointer;
				short nSize = sthWritingSize;

				parser.WritingPort = shtPort; //clsEpcCode.EpcCode2Hex(strTag);
				parser.memoryBank = MemoryBankGen2.EPC;



				nTextSize = strTag.Length / 2;
				if (nSize * parser.TagBlockSize < nTextSize)
				{
					throw new Exception("Write_TagDataEx Exception : Data Size is not corrected.\n\nWritingSize(" + nSize.ToString()
									+ ") * TagBlockSize(" + parser.TagBlockSize.ToString()
									+ ")< Input Data Size(" + nTextSize.ToString() + ")");					
					
				}
				object obj = (object)fctHexString2ByteArray(strTag);
				bytePacket = (Byte[])parser.WriteTagDataEx(nStart, nSize, ref obj);
				
				//ascii code ó����
				//else
				//{
				//    nTextSize = tbx_writeTag_Input.Text.Length;
				//    if (Convert.ToInt16(tbx_write_size.Text) * parser.TagBlockSize < nTextSize)
				//    {
				//        MessageBox.Show("Data Size is not corrected.\n\nWritingSize(" + tbx_write_size.Text
				//                        + ") * TagBlockSize(" + parser.TagBlockSize.ToString()
				//                        + ")< Input Data Size(" + nTextSize.ToString() + ")\t", "Write_TagDataEx Exception");

				//        tbx_writeTag_Input.Focus();
				//        return;
				//    }

				//    object obj = (object)fctHexString2ByteArray(fctString2HexString(tbx_writeTag_Input.Text));
				//    bytePacket = (Byte[])parser.WriteTagDataEx(nStart, nSize, ref obj);
				//    Send_Packet(bytePacket);
				//}
			}
			catch (System.Exception ex)
			{
				clsLog.WLog_Exception("WriteTag", ex);
				throw;
			}

		}


		// Hex ��Ʈ���� Byte �迭�� ��ȯ
		private Byte[] fctHexString2ByteArray(string sData)
		{
			int nLen;
			sData = sData.Replace(" ", "");

			Byte[] buffer = new byte[sData.Length / 2];

			if (sData.Length % 2 == 0)
				nLen = sData.Length;
			else
				nLen = sData.Length - 1;

			for (int i = 0; i < nLen; i += 2)
			{
				buffer[i / 2] = (byte)Convert.ToByte(sData.Substring(i, 2), 16);
			}
			return buffer;
		}

		// ��Ʈ���� Hex ��Ʈ������ ��ȯ
		private string fctString2HexString(string sData)
		{
			string sHex = "";
			char[] chArray = sData.ToCharArray(0, sData.Length);

			for (int i = 0; i < chArray.Length; i++)
			{
				sHex += String.Format("{0:X2}", (byte)chArray[i]);
			}
			return sHex;
		}

		//private void tbx_write_startpointer_TextChanged(object sender, EventArgs e)
		//{
		//    if (tbx_write_size.Text == "") return;
		//    int nSize = Convert.ToInt32(tbx_write_size.Text) * 2;

		//    if (rbn_writeTag_Hex.Checked == true)
		//        tbx_writeTag_Input.MaxLength = nSize * 2;
		//    else
		//        tbx_writeTag_Input.MaxLength = nSize;
		//}

		//private void tbx_writeTag_Input_KeyPress(object sender, KeyPressEventArgs e)
		//{
		//    if (rbn_writeTag_Hex.Checked == true)
		//    {
		//        if (!(char.IsDigit(e.KeyChar) || e.KeyChar == '\b' || (e.KeyChar >= 'A' && e.KeyChar <= 'F') || (e.KeyChar >= 'a' && e.KeyChar <= 'f')))
		//            e.Handled = true;
		//    }
		//    else
		//    {
		//        if (e.KeyChar == '\r' || !((e.KeyChar >= '!' && e.KeyChar <= '~') || e.KeyChar == '\b')) e.Handled = true;
		//    }
		//}

		//private void tbx_write_size_KeyPress(object sender, KeyPressEventArgs e)
		//{
		//    if (!(char.IsDigit(e.KeyChar) || e.KeyChar == '\b')) e.Handled = true;
		//}

		//private void rbn_writeTag_Hex_CheckedChanged(object sender, EventArgs e)
		//{
		//    tbx_writeTag_Input.Text = "";

		//    if (rbn_writeTag_Hex.Checked == true)
		//        tbx_writeTag_Input.CharacterCasing = CharacterCasing.Upper;
		//    else
		//        tbx_writeTag_Input.CharacterCasing = CharacterCasing.Normal;

		//    int nSize = Convert.ToInt32(tbx_write_size.Text) * 2;

		//    if (rbn_writeTag_Hex.Checked == true)
		//        tbx_writeTag_Input.MaxLength = nSize * 2;
		//    else
		//        tbx_writeTag_Input.MaxLength = nSize;
		//}


		int intMaxDuration = 1500;

		/// <summary>
		/// dio ������ On�Ѵ�.
		/// </summary>
		/// <param name="shtDioPort"></param>
		public bool DIOSetOn(short shtDioPort)
		{
			try
			{
				Byte[] byteData;

				switch(shtDioPort)
				{
					case 1:				
						byteData = new byte [] { 0x10, 0x02, 0x02, 0x01, 0x63, 0x00, 0x05, 0x00, 0x03, 0x01, 0x01, 0x00, 0xAC, 0xB8, 0x10, 0x03 };
						break;

					case 2:
						byteData = new byte [] { 0x10, 0x02, 0x02, 0x01, 0x63, 0x00, 0x05, 0x00, 0x03, 0x02, 0x01, 0x00, 0xF5, 0xE8, 0x10, 0x03 };
						break;

					case 3:
						byteData = new byte [] { 0x10, 0x02, 0x02, 0x01, 0x63, 0x00, 0x05, 0x00, 0x03, 0x04, 0x01, 0x00, 0x47, 0x48, 0x10, 0x03 };
						break;
					
					case 4:
						byteData = new byte[] { 0x10, 0x02, 0x02, 0x01, 0x63, 0x00, 0x05, 0x00, 0x03, 0x08, 0x01, 0x00, 0x32, 0x29, 0x10, 0x03 };
						break;
					
					default:
						byteData = new byte[] { };
						break;
				}


				clsLog.WLog(string.Format("DIOSetOn Port[{0}] : {1}", shtDioPort,  Fnc.Bytes2String(byteData)));

				boolAckDio = false;

				if (Send_Packet(byteData))
				{
					//0.3�� ������ ������ ��� �Դ��� Ȯ���Ѵ�.
					for (int i = 0; i < intMaxDuration; i += 300)
					{
						if (boolAckDio)
							break;

						Thread.Sleep(300);
					}

					return boolAckDio;
				}
				else
					return false;

			}
			catch (Exception ex)
			{
				clsLog.WLog_Exception("DIOSetOn", ex);
				throw ex;
			}
		}

		public bool DIOSetOff(short shtDioPort)
		{
			try
			{
				Byte[] byteData; 

				switch(shtDioPort)
				{
					case 1:
						byteData = new byte [] { 0x10, 0x02, 0x02, 0x01, 0x63, 0x00, 0x05, 0x00, 0x03, 0x01, 0x00, 0x00, 0x9F, 0x89, 0x10, 0x03 };						
						break;

					case 2:
						byteData = new byte[] { 0x10, 0x02, 0x02, 0x01, 0x63, 0x00, 0x05, 0x00, 0x03, 0x02, 0x00, 0x00, 0xC6, 0xD9, 0x10, 0x03 };
						break;

					case 3:
						byteData = new byte[] { 0x10, 0x02, 0x02, 0x01, 0x63, 0x00, 0x05, 0x00, 0x03, 0x04, 0x00, 0x00, 0x74, 0x79, 0x10, 0x03 };
						break;
					
					case 4:
						byteData = new byte[] { 0x10, 0x02, 0x02, 0x01, 0x63, 0x00, 0x05, 0x00, 0x03, 0x08, 0x00, 0x00, 0x01, 0x18, 0x10, 0x03 };
						break;

					default:
						byteData = new byte[] { };
						break;
				}

				clsLog.WLog(string.Format("DIOSetOff Port[{0}] : {1}", shtDioPort, Fnc.Bytes2String(byteData)));

				boolAckDio = false;

				if (Send_Packet(byteData))
				{

					//0.3�� ������ ������ ��� �Դ��� Ȯ���Ѵ�.
					for (int i = 0; i < intMaxDuration; i += 300)
					{
						if (boolAckDio)
							break;

						Thread.Sleep(300);
					}

					return boolAckDio;
				}
				else
					return false;

			}
			catch (Exception ex)
			{
				clsLog.WLog_Exception("DIOSetOff", ex);
				throw ex;
			}
		}



		public struct param_DIOSetOnDuration
		{
			/// <summary>
			/// ��Ʈ ��ȣ
			/// </summary>
			public short shtDioPort;
			/// <summary>
			/// ���� �ð�..
			/// </summary>
			public int intDuration;

		}

		/// <summary>
		/// ���� �Ⱓ���� dioport�� ����..(�񵿱� ����)
		/// </summary>
		/// <param name="shtDioPort"></param>
		/// <param name="intDuration"></param>
		public void DIOSetOnDuration_Async(short shtDioPort, int intDuration)
		{
			param_DIOSetOnDuration p = new param_DIOSetOnDuration();

			p.intDuration = intDuration;
			p.shtDioPort = shtDioPort;

			ThreadPool.QueueUserWorkItem(new WaitCallback(DIOSetOnDuration), p);
		}

		/// <summary>
		/// ���� �Ⱓ���� dioport�� ����..
		/// </summary>
		/// <param name="obj">param_DIOSetOnDuration ����ü</param>
		public void DIOSetOnDuration(object obj)
		{
			lock (this)
			{
				try
				{
					param_DIOSetOnDuration p = (param_DIOSetOnDuration)obj;

					bool isAck = false;

					//on��ȣ�� 3�� ������. ���н� �޼ҵ� ����.
					for (int i = 1; i < 3; i++)
					{
						isAck = DIOSetOn(p.shtDioPort);
						if (isAck) break;
					}

					Thread.Sleep(p.intDuration);

					//off��ȣ�� on �����ø�..
					if (isAck)
					{
						//off��ȣ�� ������ ����...
						while (true)
						{
							//�۾��ø�..
							if (!bReadSocket) break;

							if (socket.Connected && DIOSetOff(p.shtDioPort)) break;
						}
					}


				}
				catch (Exception ex)
				{
					clsLog.WLog_Exception("DIOSetOnDuration", ex);
				}
			}
		}







	}
}