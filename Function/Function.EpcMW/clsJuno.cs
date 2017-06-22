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
	/// 세연 junoF 직접 컨트롤 클래스..
	/// </summary>
	public partial class clsJuno : IDisposable
	{
		Socket socket;                      //소켓 객체를 생성합니다.
		ParserMainClass parser;             //파서 라이브러리 객체를 생성합니다.

		private Thread threadSockect = null;    //소켓감시 쓰레드입니다.
        private Thread threadAlive = null;    //소켓감시 쓰레드입니다.
		private bool bReadSocket = false;       //쓰레드 플레그 입니다.

        private DateTime dtLastGetVersion = DateTime.Now;

		private Function.Util.Log clsLog;

		string strIP;
		int intPort;
		/// <summary>
		/// 장비에서 올라온온도..
		/// </summary>
		int inttemp;

		public int intTemperature;


		public enum enTagType
		{
			/// <summary>
			/// 변환 하지 않는다.
			/// </summary>
			NONE,
			/// <summary>
			/// EPC CODE로 변환
			/// </summary>
			EPCCODE,
			/// <summary>
			/// ASCII CODE로 변환
			/// </summary>
			ASCII
		};

		/// <summary>
		/// 태그 변환 타입
		/// </summary>
		public enTagType TagType = enTagType.EPCCODE;

		struct stReadingTagInfo
		{
			/// <summary>
			/// 읽은 port 번호
			/// </summary>
			public int intPortNo;
			/// <summary>
			/// 읽은 tagid
			/// </summary>
			public string strTagID;
		}

		/// <summary>
		/// 처리시 읽은 테그 id
		/// </summary>
		private stReadingTagInfo infoTagID = new stReadingTagInfo();

	
		/// <summary>
		/// 리더에서 테그정보를 수집한 결과를 파서에서 분석하여 Host로 알려주는 이벤트 델리게이트
		/// </summary>
		/// <param name="strEpcTagID">Tag Data (Hex)</param>
		/// <param name="isError">에러여부</param>
		/// <param name="strMsg">메시지</param>
		/// <param name="readingPort">안테나 포트</param>		
		public delegate void delOnTagReadCommandAck
					(string strTagID, bool isError, string strMsg,int intreadingPort);
		/// <summary>
		/// 리더에서 테그정보를 수집한 결과를 파서에서 분석하여 Host로 알려주는 이벤트 콜백 함수
		/// </summary>
		public delOnTagReadCommandAck evtOnTagReadCommandAck;

		/// <summary>
		/// 리더에 writing 명령 후 결과를 받는 이벤트 델리게이트
		/// </summary>
		/// <param name="isError"></param>
		/// <param name="intPort"></param>
		/// <param name="strMsg"></param>
		public delegate void delOnTagWriteCommandAck(bool isError, int intPort, string strMsg);
		/// <summary>
		/// 리더에 writing 명령 후 결과를 받는 이벤트 콜백 함수
		/// </summary>
		public delOnTagWriteCommandAck evtOnTagWriteCommandAck;

		/// <summary>
		/// 리더에 설정 관련 응답을 받는 이벤트 델리게이트
		/// </summary>
		/// <param name="strAddr"></param>
		/// <param name="isError"></param>
		/// <param name="strMsg"></param>
		/// <param name="intaddrValue"></param>
		public delegate void delConfigurationAck(string strAddr, bool isError, string strMsg, int intaddrValue);
		/// <summary>
		/// Configuration 상태 이벤트 콜백함수
		/// </summary>
		delConfigurationAck evtGetConfigurationAck = null;
		/// <summary>
		/// Configuration 세팅 후 관련 상태 이벤트 입니다.
		/// </summary>
		delConfigurationAck evtSetConfigurationAck = null;

		/// <summary>
		/// 상태 체크용.. 데이터 수신시에 현재 시간으로 업데이트
		/// </summary>
		DateTime dtAliveCheck;

		/// <summary>
		/// 리더에 dio input port에 대한 변경(on) 이벤트 델리게이트
		/// </summary>
		/// <param name="Port1"></param>
		/// <param name="Port2"></param>
		/// <param name="Port3"></param>
		/// <param name="Port4"></param>
		public delegate void delDioInPortAck(bool Port1, bool Port2, bool Port3, bool Port4);

		/// <summary>
		/// 리더에 dio input port에 대한 변경(on) 이벤트 델리게이트
		/// </summary>
		public delDioInPortAck evtDioInPortAck;




		public clsJuno(string strip, int intport, string strLogFileName)
		{

			strIP = strip;
			intPort = intport;

			clsLog = new Function.Util.Log(@".\JunoF", strLogFileName, 10, true);

			if (!InitRFSet()) //RF parser 라이브러리를 사용하기 위한 초기화 세팅작업입니다. 이벤트콜백함수도 여기서 등록합니다.
			{
				throw new Exception("RF Set error");
			}


			//// Write시 MaxLength 를 설정하는 UI 관련 로직입니다.
			//int nSize = Convert.ToInt32(tbx_write_size.Text) * 2;
			//if (rbn_writeTag_Hex.Checked == true)
			//    tbx_writeTag_Input.MaxLength = nSize * 2;
			//else
			//    tbx_writeTag_Input.MaxLength = nSize;

		}

		/// <summary>
		/// 파서 관련 객체를 메모리에 로딩합니다. 이벤트콜백함수를 등록합니다.
		/// </summary>
		/// <returns></returns>
		private bool InitRFSet()
		{
			bool init_state = false;

			if (!init_state)
			{
				parser = new ParserMainClass();             // 파서 객체를 메모리에 로딩합니다.
				parser.CapVersion = CeyonProtocol.CAP22;    // JUNOF 고정형 리더기 프로토콜은 CAP2.2 입니다. 
				parser.Tag_Type = Tag_Type.Gen2;            // Tag 타입은 'Gen2'입니다.

				//이벤트 콜백함수 등록 : Tag 정보를 리더기로 부터 받으면 Host로 알려줍니다.
                parser.OnGetInformationAck += new _IParserMainEvents_OnGetInformationAckEventHandler(parser_OnGetInformationAck);

				parser.OnControlCommandAck += new _IParserMainEvents_OnControlCommandAckEventHandler(parser_OnControlCommandAck);

				parser.OnExternDeviceControlAck += new _IParserMainEvents_OnExternDeviceControlAckEventHandler(parser_OnExternDeviceControlAck);

				parser.OnTagReadCommandAck += new _IParserMainEvents_OnTagReadCommandAckEventHandler(parser_OnTagReadCommandAck);

				parser.OnTagWriteCommandAck += new _IParserMainEvents_OnTagWriteCommandAckEventHandler(parser_OnTagWriteCommandAck);

				parser.OnGetConfigurationAck += new _IParserMainEvents_OnGetConfigurationAckEventHandler(parser_OnGetConfigurationAck); //Configuration 상태 이벤트 입니다.

				parser.OnSetConfigurationAck += new _IParserMainEvents_OnSetConfigurationAckEventHandler(parser_OnSetConfigurationAck); //Configuration 세팅 후 관련 상태 이벤트 입니다.

                

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
		/// 접점 컨트롤시 응답
		/// </summary>
		/// <param name="Port"></param>
		/// <param name="DeviceType"></param>
		/// <param name="sts"></param>
		void parser_OnExternDeviceControlAck(short Port, short DeviceType, Status sts)
		{
			if (Port == 0 && DeviceType == 0x3 && sts == Status.No_Error)
			{	//output port 컨트롤에 대한 응답
				clsLog.WLog("Dio Output Port 처리 응답 받음");
				boolAckDio = true;
			}
			else if(Port == 4)
			{	//input port 상태에 변화(on)에 대한 이벤트
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

				clsLog.WLog(string.Format("Dio Input Port 상태변경 : [Port1]{0} [Port2]{1} [Port3]{2} [Port4]{3}", Port1, Port2, Port3, Port4) );


				if (evtDioInPortAck != null)
					evtDioInPortAck(Port1, Port2, Port3, Port4);			


			}
		}


		void parser_OnControlCommandAck(ControlCommand cmd, Status sts)
		{
			clsLog.WLog("CommandAck:" + cmd.ToString());
		}


		/// <summary>
		/// 정보 요청에 대한 응답.. 현재 펌버젼 체크시 이용..
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
		///// Write 실행 후 상태를 알려주는 이벤트입니다.
		///// </summary>
		///// <param name="Port"></param>
		///// <param name="sts"></param>
		//void parser_OnTagWriteCommandAck(short Port, Status sts)
		//{
		//    tbx_writeTagResult.Text = fctConvertState(sts);
		//}


		// CAP Parser의 Satae 값의 Description 리턴
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
		/// Rfid와 소켓연결 실행하
		/// </summary>		
		public void Open()
		{
           
            if (socket != null && socket.Connected == true) return;

            clsLog.WLog(string.Format("접속을 시도 합니다. [IP]{0} [PORT]{1}", strIP, intPort));

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(strIP, intPort);

            if (socket.Connected == false)
            {
                clsLog.WLog(string.Format("접속실패 : [IP]{0} [PORT]{1}", strIP, intPort));
                return;
            }

            clsLog.WLog(string.Format("접속성공. [IP]{0} [PORT]{1}", strIP, intPort));

            if (!bReadSocket)
           { 
                bReadSocket = true;
                threadSockect = new Thread(new ThreadStart(T_ReadSocket)); //소켓쓰레드를 시작합니다. 
                threadSockect.IsBackground = true;
                threadSockect.Start();


                threadAlive = new Thread(new ThreadStart(AliveCheck)); //소켓쓰레드를 시작합니다. 
                threadAlive.IsBackground = true;
                threadAlive.Start();
            }

			dtAliveCheck = DateTime.Now;

		}

        /// <summary>
        /// 상태 체크 스래드 : 주기적으로 GetFirmwareVersion data를 보내 상태 체크를 한다.
        /// </summary>
        private void AliveCheck()
        {

			bool isReOpen = false;

			while (bReadSocket)
			{
				try
				{
					//현재시간 보다 통신체크한 시간이 뒤..
					if (dtAliveCheck > DateTime.Now)
						dtAliveCheck = DateTime.Now;


					//30초이상 통신이 없으면 통신 체크를 한다.
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

					//10초 한번씩..
					Thread.Sleep(10000);

				}
				catch (Exception ex)
				{
					clsLog.WLog_Exception("AliveCheck", ex);
				}


				//1분이상 통신이 없으면 에러 처리를 한다.
				if (dtAliveCheck < DateTime.Now.AddSeconds(-60))
				{
					try
					{
						if (!isReOpen) clsLog.WLog("TCP Conection 오류 3회 이상 발생 : 소켓 종료 후 재 접속시도");

						isReOpen = true;

						if (socket.Connected)
						{
							socket.Shutdown(SocketShutdown.Both);  //보내기 와 받기 모두 사용할 수 없도록 합니다.
							socket.Close();
						}

						Thread.Sleep(500);

						Open();

					}
					catch
					{
						clsLog.WLog("TCP Conection Open 실패");
						Thread.Sleep(5000);
					}

				}


			}
        }





		/// <summary>
		/// 소켓쓰레드입니다. TCP단으로 넘어오는 패킷을 감시합니다. 들어오는 패킷이 있으면 parser 로 보냅니다. 
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
						int nPacket = socket.Available; //네트워크에서 받아서 읽을 수 있는 데이터 양을 리턴합니다.

						if (nPacket > 0)
						{
							//상태 체크시간 업데이트 - 신호를 받으면 업데이트
							dtAliveCheck = DateTime.Now;

							byte[] bytePacket = new byte[nPacket];
							socket.Receive(bytePacket, nPacket, 0);


							object obj = (object)bytePacket;

							if (socket.Connected == true)
								parser.FctSetReceiveData(ref obj);  //TCP통신단에서 리더기로부터 받은 모든 패킷을 파서로 보냅니다.

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
                //    clsLog.WLog("TCP Conection 오류 3회 이상 발생 : 소켓 종료 후 재 접속시도");
					
                //    socket.Shutdown(SocketShutdown.Both);  //보내기 와 받기 모두 사용할 수 없도록 합니다.
                //    socket.Close();
					
                //    Thread.Sleep(500);
                //    Open();

                //    intError = 0;

                //}
			}
		}



		/// <summary>
		/// 소켓으로 데이터를 보내는 공용함수입니다.
		/// </summary>
		/// <param name="bytePacket">리더기에 보낼 패킷입니다.</param>
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
		/// 리더에 리딩상태를 설정하는 명령을 보내는 함수입니다. 
		/// </summary>
		/// <param name="status">T:AutoRead F:Verbos</param>
		public void SetReadState(bool status)
		{
			Byte[] byteDate;


			if (status)
			{
				byteDate = (Byte[])parser.EasySetConfiguration(SpecialSetting.Set_AutoRead); //AutoRead 상태
				clsLog.WLog("AUTOREAD 상태로 변경 되었습니다");
			}
			else
			{			
				byteDate = (Byte[])parser.EasySetConfiguration(SpecialSetting.Set_VerboseRead); //Verbose 상태
				clsLog.WLog("Verbose 상태로 변경 되었습니다");
			}

			Send_Packet(byteDate);
		}



		/// <summary>
		/// 리더에서 테그정보를 수집한 결과를 파서에서 분석하여 Host로 알려주는 이벤트 콜백 함수
		/// </summary>
		/// <param name="data">Tag Data (Hex)</param>
		/// <param name="readmode"></param>
		/// <param name="tagType"></param>
		/// <param name="sts">Read 상태값 , No_error 일 경우 정상 리딩 완료</param>
		/// <param name="readingPort">Read 작업을 한 안테나 포트</param>
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
					// 12byte가 아닌 데이터는 EPC코드변환 불가이므로 필터링합니다. 
					return;
				}

				// 12byte EPC 영역만 축출합니다.
				for (int i = 2; i < btData.Length; i++)
				{
					stData[i - 2] = btData[i];
				}

				string strTagID = string.Empty;

				rfData = fctByteArray2HexString(stData, string.Empty); // 화면에 보여주기 위해 string 타입으로 변경하는 함수입니다.

				switch(TagType)
				{
					case enTagType.EPCCODE:
						

						strTagID = clsEpcCode.Hex2EpcCode(rfData);

						clsLog.WLog(string.Format("Tag수신:[EPC]{0} [HEX]{1}", strTagID, rfData));						

						break;
					 
					case enTagType.ASCII:

						for (int i = 0; i < rfData.Length; i += 2)
						{
							int intHex = Convert.ToInt32(rfData.Substring(i, 2), 16);
							strTagID += Convert.ToChar(intHex).ToString();
						}

						clsLog.WLog(string.Format("Tag수신:[ASCII]{0} [HEX]{1}", strTagID, rfData));	

						break;

					case enTagType.NONE:
						strTagID = rfData;
						clsLog.WLog(string.Format("Tag수신:[HEX]{0}", strTagID, rfData));
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
				clsLog.WLog(string.Format("Tag수신에러:[ECODE]{0} [HEX]{1}", fctConvertState(sts), fctByteArray2HexString((byte[])data, string.Empty)));
				if(evtOnTagReadCommandAck != null)
					evtOnTagReadCommandAck(string.Empty, true, fctConvertState(sts), readingPort);
			}

		}


		/// <summary>
		/// 안테나로 부터 테그를 읽는다.
		/// </summary>
		/// <param name="intPorts">port번호들 1~4</param>
		/// <param name="intDuration">수신시간(ms)</param>
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
		/// Byte 배열을 Hex 스트링으로 변환(Hex 스트링 사이에 구분문자 입력 처리 추가) 
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
		/// 실제 소켓을 닫는 코드가 있는 함수입니다.
		/// </summary>
		public void Close()
		{

			if (socket != null && socket.Connected)
			{
				//연결을 끊을때는 버보스 모드로 무조건 변경..
				//SetReadState(false);

                bReadSocket = false;

                Thread.Sleep(500);

				threadSockect.Abort();
				threadSockect.Join();

                threadAlive.Abort();
                threadAlive.Join();

				bReadSocket = false;

				socket.Shutdown(SocketShutdown.Both);  //보내기 와 받기 모두 사용할 수 없도록 합니다.
				socket.Close();
				Thread.Sleep(100);

			}

		}


		public void Dispose()
		{
			Close();
		}

		/// <summary>
		/// 4채널 안테나를 세팅합니다. true값이면 해당 포트가 활성화되도록 세팅됩니다.
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
			
			clsLog.WLog(string.Format("안테나 세팅 변경:[1]{0} [2]{1} [3]{2} [4]{3}",
							p1, p2, p3, p4));
		}

		/// <summary>
		/// Verbose 모드에서 테그 수신 명령을 보낸다.
		/// </summary>
		/// <param name="shtPort">안테나 포트</param>		
		private void InventoryRead(short shtPort)
		{
			Byte[] byteData;


			parser.readingPort = shtPort;
			byteData = (Byte[])parser.ReadInventory();

			clsLog.WLog(string.Format("InventoryRead : {0}", Fnc.Bytes2String(byteData)));

			Send_Packet(byteData);

		}


	
		/// <summary>
		/// Vto 타임을 설정한다. Verbose 모드시 테그 수신시간..
		/// </summary>
		/// <param name="intTime">ms : 1초이상 설정..</param>
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
		///  Vto 타임을 가져온다. Verbose 모드시 테그 수신시간..
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
		/// TagId를 기록한다.
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
				
				//ascii code 처리시
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


		// Hex 스트링을 Byte 배열로 변환
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

		// 스트링을 Hex 스트링으로 변환
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
		/// dio 접점을 On한다.
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
					//0.3초 단위로 응답이 들어 왔는지 확인한다.
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

					//0.3초 단위로 응답이 들어 왔는지 확인한다.
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
			/// 포트 번호
			/// </summary>
			public short shtDioPort;
			/// <summary>
			/// 유지 시간..
			/// </summary>
			public int intDuration;

		}

		/// <summary>
		/// 일정 기간동안 dioport를 연다..(비동기 실행)
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
		/// 일정 기간동안 dioport를 연다..
		/// </summary>
		/// <param name="obj">param_DIOSetOnDuration 구조체</param>
		public void DIOSetOnDuration(object obj)
		{
			lock (this)
			{
				try
				{
					param_DIOSetOnDuration p = (param_DIOSetOnDuration)obj;

					bool isAck = false;

					//on신호는 3번 보낸다. 실패시 메소드 종료.
					for (int i = 1; i < 3; i++)
					{
						isAck = DIOSetOn(p.shtDioPort);
						if (isAck) break;
					}

					Thread.Sleep(p.intDuration);

					//off신호는 on 성공시만..
					if (isAck)
					{
						//off신호는 성공시 까지...
						while (true)
						{
							//작업시만..
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