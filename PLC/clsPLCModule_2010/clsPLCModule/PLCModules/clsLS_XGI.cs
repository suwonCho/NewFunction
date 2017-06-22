using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using System.Collections;

using PLCModule;

namespace  PLCModule.PLCModules
{
	class clsLS_XGI: clsPLCModuleInterface, IDisposable
	{
		Encoding _en = System.Text.Encoding.Default;

		internal readonly string _dontcare = System.Text.Encoding.Default.GetString(new byte[] { 0x00, 0x00 });

		internal enum enCmd_Req : int
		{
			RndRead = 0x54,
			RndWrite = 0x58
		}

		internal enum enData_Type : int
		{
			BIT = 0x00,
			BYTE = 0x01,
			WORD = 0x02,
			DWORD = 0x03,
			LWORD = 0x04,
			//DWORD = 0x03,
			//LWORD = 0x04,
			CONTINUE = 0x14
		}


		private byte[] yReceived;
		bool isReceived = false;

		private byte[] _c_header = new byte[] { 
			0x4c, 0x53, 0x49, 0x53, 0x2d, 0x58, 0x47, 0x54,			//00~07 : companyid
			0x00, 0x00,												//08~09 : 예약영역
			0x00, 0x00,												//10~11 : plc info
			0xa4,													//12    : cpu info
			0x33,													//13    : source of frame 
			0x00, 0x00,												//14~15 : invoke id
			0x00, 0x00,												//16~17 : length
			0x00,													//18	: fenet position
			0x00 };													//19    : 예약영역



		//class _c_header
		//{
			
		//    readonly string _companyid = "LSIS-XGT";
		//    string _reserved = System.Text.Encoding.Default.GetString(new byte[] { 0x00, 0x00 });
		//    string _plcinfo = System.Text.Encoding.Default.GetString(new byte[] { 0x00, 0x00 });
		//    string _cpuinfo = System.Text.Encoding.Default.GetString(new byte[] { 0xA0 });	//XGT
		//    string _sourceframe = System.Text.Encoding.Default.GetString(new byte[] { 0x33 });	//
		//    string _invokeid = System.Text.Encoding.Default.GetString(new byte[] { 0x00, 0x00 });	//
		//    string _length = System.Text.Encoding.Default.GetString(new byte[] { 0x00, 0x00 });	//
		//    string _fenetposition = System.Text.Encoding.Default.GetString(new byte[] { 0x00 });	//
		//    string _bcc = System.Text.Encoding.Default.GetString(new byte[] { 0x00 });	//

		//    public string Header_Get()
		//    {
		//        return _companyid + _reserved + _plcinfo + _cpuinfo + _sourceframe + _invokeid + _length + _fenetposition + _bcc;
		//    }
						
		//}


		//class _c_command
		//{
		//    public enCmd_Req _cmd_req;
		//    public enData_Type _data_type;

		//    public Dictionary<string, string> dicAddress = new Dictionary<string, string>();

		//    public _c_command(enCmd_Req cmd_req, enData_Type data_type)
		//    {
		//        _cmd_req = cmd_req;
		//        _data_type = data_type;
		//    }

		//    public string Command_Get()
		//    {
		//        byte[] byt = new byte[8];
				
		//        //명령어
		//        byt[0] = Convert.ToByte(_cmd_req);
		//        //datatype
		//        byt[2] = Convert.ToByte(_data_type);
		//        //변수개수
		//        byt[6] = Convert.ToByte(dicAddress.Count);

		//        string rst = string.Empty;

		//        byte[] b = new byte[2];
		//        foreach (string add in dicAddress.Keys)
		//        {
		//            b[0] = 0x00;
		//            b[1] = Convert.ToByte(add.Length);

		//            rst = rst + System.Text.Encoding.Default.GetString(b) + add;

		//        }

		//        rst = System.Text.Encoding.Default.GetString(byt) + rst;

		//        return rst;
		//    }

		//}

		


		#region Tcp/IP 통신 관련
		private PLCSocket Comm = new PLCSocket();


		public clsLS_XGI(string strIPAddress, int intPort, string _strDeviceType)
		{
			
			Comm.Received += new PLCSocket.Receive(this.Received);

			Comm.strServerIP = strIPAddress;
			Comm.iPort = intPort;

			delStart = new delScanThread(Start_PLCScan);

		}

	

		public override bool Open()
		{

			string strMsg = string.Format("(IP:{0} / Port : {1})", Comm.strServerIP, Comm.iPort);

			try
			{
				//Console.WriteLine("Retry Connect");

				if (open())
				{	
					delStart(true);

					StartScan();

					strMsg = "Open 성공" + strMsg;
					return true;
				}

				strMsg = "Open 실패" + strMsg;
				return false;

			}
			catch (Exception ex)
			{
				strMsg = "Open 실패 :" + ex.Message + strMsg + "\r\n" + ex.ToString();
				return false;
			}
			finally
			{
				PLCModule.clsPLCModule.LogWrite("Open", strMsg);
			}


		}




		protected override bool open()
		{
			if (Comm.client_Open())
			{
				this.ChConnection_Status(true);
				return true;
			}

			return false;
			
		}

		public override bool Close()
		{
			string strMsg = string.Empty;
			try
			{

				//명시적으로 닫을때 재시도를 하지 안는다.
				_isClose = true;

				Start_PLCScan(false);
				StopScan();

				//닫혀 있으면...
				if (!this.isConnected) return true;

				//RetryOpen(false);
				if (close())
				{
					this.ChConnection_Status(false);
					strMsg = "Sucess closing connection  ";
					return true;
				}
				else
				{
					strMsg = "Fail closing connection ";
					return false;
				}
			}
			catch (Exception ex)
			{
				strMsg = "Connection close fail : " + ex.Message + "\r\n" + ex.ToString();
				return false; //throw ex;
			}
			finally
			{
				PLCModule.clsPLCModule.LogWrite("Close", strMsg);
			}
		}

		protected override bool close()
		{
			try
			{
				return Comm.client_Close();
			}
			catch
			{
				return false;
			}
			finally { this.ChConnection_Status(false); }

		}


		//	public override bool isConnected
		//	{
		//		get
		//		{ return Comm.objClient.Connected; }
		//	}

		#endregion


		private int intTimeOut = 3000;

		private string strMsg = string.Empty;

		/// <summary>
		/// TimeOut Time Set (ms)
		/// </summary>
		public int IntTimeOut
		{
			get
			{
				return intTimeOut;
			}
			set
			{
				intTimeOut = value;
			}
		}

		private int intScanRate = 500;
		/// <summary>
		/// ScanRate Time Set (ms)
		/// </summary>
		public int IntScanRate
		{
			get
			{
				return IntScanRate;
			}
			set
			{
				IntScanRate = value;
			}
		}


		/// <summary>
		/// 등록 되어 있는 어드레스들을 주기적으로 값을 확인 하는 쓰레드를 시작한다.
		/// </summary>
		/// <param name="isStart"></param>
		private void Start_PLCScan(bool isStart)
		{
			if (this.thPLCScan != null && thPLCScan.IsAlive)
			{
				thPLCScan.Abort();
				thPLCScan.Join();
			}

			thPLCScan = null;

			if (isStart)
			{
				thPLCScan = new Thread(new ThreadStart(this.threadScanPLC));
				thPLCScan.Name = "PLC Address Scan";
				thPLCScan.IsBackground = true;
				thPLCScan.Start();
			}

		}


		/// <summary>
		/// 등록 되어 있는 어드레스에 값을 확인..
		/// </summary>
		private void threadScanPLC()
		{
			int intErrCount = 0;
			string strMsg = string.Empty;
			bool isWriteByte = true;

			
			byte[] bytSend;
			int intSendLength;
			int idx;
			int intVLength = 0;
			byte[] tmp;

			while (true)
			{
				try
				{
					if (this.dtWriteOrder.Rows.Count > 0)
						SendWriteOrder();

					int intRows = this.dtAddress.Rows.Count;
					intSendLength = 20 + 8; 

					for (int i = 0; i < intRows; i += 16)
					{
						int intScanCount = (i + 15 > intRows ? intRows - i : 15);
						
						this.isReceived = false;


						//어드레스 자리수 계산, 2(변수명길이) + 변수명
						for (int j = i; j < i + intScanCount; j++)
						{
							intSendLength += 2 + dtAddress.Rows[j]["Address"].ToString().Trim().Length;
						}

						bytSend = new byte[intSendLength];

						_c_header.CopyTo(bytSend,0);
						
						idx = 28;
						for (int j = i; j < i + intScanCount; j++)
						{
							bytSend[idx] = Convert.ToByte(dtAddress.Rows[j]["Address"].ToString().Trim().Length);

							tmp = _en.GetBytes(dtAddress.Rows[j]["Address"].ToString());
							idx += 2;
							tmp.CopyTo(bytSend, idx);
							idx += tmp.Length;
						}
												

						//invoke id
						bytSend[15] = 0x1;
						//command길이
						bytSend[16] = Convert.ToByte(intSendLength - 20);
						//cmd : read request
						bytSend[20] = 0x54;
						//datatype
						bytSend[22] = 0x02;
						//변수개수
						bytSend[26] = Convert.ToByte(intScanCount);

						Console.WriteLine(ByteToString(bytSend));


						Comm.Send(bytSend, ref strMsg);
						WaitRecieved();

						//에러체크
						strMsg = chk_Error_byte(yReceived, 0x55);

						if(strMsg != null)
						{
							throw new Exception(string.Format("Address Reading 오류 :{0}", strMsg));
						}

						isWriteByte = true;
						idx = 30;

						for (int j = i; j < i + intScanCount; j++)
						{
							//변수값의 길이..
							intVLength = Convert.ToInt32(ByteToInt(new byte[] { yReceived[idx], yReceived[idx+1] }));

							idx += 2;
							tmp = new byte[intVLength];
							for (int k = 0; k < intVLength; k++)
							{
								tmp[k] = yReceived[idx + k];
							}

							idx = idx + intVLength;

						    isWriteByte = !ChangeddAddressValue(dtAddress.Rows[j], Convert.ToInt32(ByteToInt(tmp)),isWriteByte, bytSend, yReceived);

						}
					}

					intErrCount = 0;

					if (this.evtPLCScan != null) ThevtPLCScan();

					if (this.dtWriteOrder.Rows.Count > 0)
						SendWriteOrder();
					else
						Thread.Sleep(this.intScanRate);
				}
				catch (Exception ex)
				{

					intErrCount++;
					strMsg = "주소 값 검색 오류 [ErrorCount:" + intErrCount.ToString() + "] : " + ex.Message + "\r\n" + ex.ToString();
					PLCModule.clsPLCModule.LogWrite("threadScanPLC", strMsg);

					if (intErrCount > 1)
					{

						strMsg = "주소 값 검색 오류 [ErrorCount:" + intErrCount.ToString() + "]로 인한 연결을 재설정 합니다.";
						PLCModule.clsPLCModule.LogWrite("threadScanPLC", strMsg);

						RetryOpen(true);
						return;
					}

				}
			}

		}

		private string chk_Error_byte(byte[] byt, byte cmd)
		{
			//에러체크
			if (byt[20] != cmd || byt[26] != 0x00)
				return chk_Error_code(byt[26]);			
			else
				return null;
		}


		private string chk_Error_code(byte errcode)
		{
			string err_msg;

			switch(errcode)
			{
				case 0x01:
					err_msg = "개별 읽기/쓰기 요청시 블록 수가 16 보다 큼";
					break;

				case 0x02:
					err_msg = "X,B,W,D,L 이 아닌 데이터 타입을 수신했음";
					break;

				case 0x03:
					err_msg = "서비스 되지 않는 디바이스를 요구한 경우(XGK : P, M, L, K, R, , XGI : I, Q, M..)";
					break;

				case 0x04: 
					err_msg = "각 디바이스별 지원하는 영역을 초과해서 요구한 경우";
					break;

				case 0x05:
					err_msg = "한번에 최대 1400byes 까지 읽거나 쓸 수 있는데 초과해서 요청한 경우(개별 블록 사이즈)";
					break;

				case 0x06:
					err_msg = "한번에 최대 1400byes 까지 읽거나 쓸 수 있는데 초과해서 요청한 경우(블록별 총 사이즈)";
					break;

				case 0x10:
					err_msg = "컴퓨터 통신모듈의 위치가 잘못 지정되었을 경우";
					break;

				case 0x11:
					err_msg = "SLOT_NO에 장착된 통신 모듈의 초기화 에러";
					break;

				case 0x12:
					err_msg = "입력 파라미터 설정 에러";
					break;

				case 0x13:
					err_msg = "변수 길이 에러";
					break;

				case 0x14:
					err_msg = "상대국에서 잘못된 응답 수신";
					break;

				case 0x15:
					err_msg = "타임아웃 에러(Time Out)(컴퓨터 통신 모듈로부터 응답을 수신하지 못했을 경우)";
					break;

				case 0x50: 
					err_msg = "Disconnection Error(접속 끊기 에러)";
					break;

				case 0x52:
					err_msg = "Not Received Frame(정의한 프레임이 수신되지 않았음)";
					break;

				case 0x54:
					err_msg = "Data Count Error(펑션블록의 입력에 사용된 데이터 개수가 프레임에 정의한 데이터 개수와 맞지 않거나 작음)";
					break;

				case 0x57:
					err_msg = "Not Connected(채널이 맺어지지 않았음)";
					break;

				case 0x59:
					err_msg = "Im TCP Send Error(즉시 응답 에러)";
					break;

				case 0x5A:
					err_msg = "Im UDP Send Error(즉시 응답 에러)";
					break;

				case 0x5B:
					err_msg = "Socket Error";
					break;

				case 0x5C:
					err_msg = "Channel Disconnected(채널 끊어짐)";
					break;

				case 0x5D:
					err_msg = "기본 파라미터 및 프레임이 설정되지 않았음";
					break;

				case 0x5E:
					err_msg = "채널 설립 에러";
					break;

				case 0x60:
					err_msg = "이미 채널 설립된 상태";
					break;

				case 0x61:
					err_msg = "Method Input Error(펑션블록의 입력에 사용된 Method가 바르지 않음)";
					break;

				case 0x65:
					err_msg = "채널 번호 설정 에러";
					break;

				case 0x66: 
					err_msg = "상대국 설정 에러(재 설정)";
					break;

				case 0x67:
					err_msg = "커넥션 대기";
					break;

				case 0x68:
					err_msg = "설정된 IP를 가진 상대국이 네트워크에 존재하지 않음";
					break;

				case 0x69:
					err_msg = "상대국이 PASSIVE 포트를 오픈하지 않았음";
					break;

				case 0x6A:
					err_msg = "대기 시간에 의한 채널 해제";
					break;

				case 0x6B:
					err_msg = "채널 설립 개수 초과(채널 설립 개수 = 16 - 전용 접속 개수)";
					break;

				case 0x6C:
					err_msg = "최대 송신 개수 초과(아스키 데이터 개수 = 헥사 데이터 개수 * 2 이므로 아스키 데이터 개수가 1,400 바이트를 초가하면 안됩니다)";
					break;

				case 0x75:
					err_msg = "전용 서비스에서 프레임 헤더의 선두 부분이 잘못된 경우(‘LSIS-GLOFA’)";
					break;

				case 0x76:
					err_msg = "전용 서비스에서 프레임 헤더의 Length가 잘못된 경우";
					break;

				case 0x77:
					err_msg = "전용 서비스에서 프레임 헤더의 Checksum이 잘못된 경우";
					break;

				case 0x78:
					err_msg = "전용 서비스에서 명령어가 잘못된 경우";
					break;

				default:
					err_msg = "해당에러코드가 존재 하지 않습니다.";
					break;

			}

			return string.Format("[{0x{0}]{1}", errcode, err_msg);
		}


		private void SendWriteOrder()
		{
			int intErrCount = 0;
			while (true)
			{
				try
				{
					int intRows = dtWriteOrder.Rows.Count;
					int intWritePointLength = 16;
					string strChanged = string.Empty;
					int intLength = 20 + 2 + 2 + 2 + 2;
					int idx;
					byte[] tmp;

					for (int i = 0; i < intRows; i += intWritePointLength)
					{
						int intWriteCount = (i + 10 > intRows ? intRows - i : intWritePointLength);


						//어드레스 자리수 계산, 2(변수명길이) + 변수명 + 데이터크기(2) + 데이터
						for (int j = i; j < i + intWriteCount; j++)
						{
							intLength += 2 + dtWriteOrder.Rows[j]["Address"].ToString().Trim().Length;
							intLength += 2 + 2;
						}

						byte[] bytSend = new byte[intLength];
						_c_header.CopyTo(bytSend, 0);

						idx = 28;
						//주소 설정
						for (int j = i; j < i + intWriteCount; j++)
						{
							bytSend[idx] = Convert.ToByte(dtWriteOrder.Rows[j]["Address"].ToString().Trim().Length);
							idx += 2;
							tmp = _en.GetBytes(dtWriteOrder.Rows[j]["Address"].ToString());							
							tmp.CopyTo(bytSend, idx);
							idx += tmp.Length;

							strChanged += (strChanged == string.Empty ? "" : " / ") +
								string.Format("[{0}] WorteValue : {1:X4}", dtWriteOrder.Rows[j]["Address"], dtWriteOrder.Rows[j]["Value"]);
						}

						//값설정
						for (int j = i; j < i + intWriteCount; j++)
						{
							bytSend[idx] = 0x2;
							idx += 2;

							ByteSetValue(bytSend, idx, 2, Convert.ToInt32(dtWriteOrder.Rows[j]["Value"].ToString()));							
							idx += 2;
						}

						//invoke id
						bytSend[15] = 0x1;
						//command길이
						bytSend[16] = Convert.ToByte(intLength - 20);
						//cmd : write request
						bytSend[20] = 0x58;
						//datatype
						bytSend[22] = 0x02;
						//변수개수
						bytSend[26] = Convert.ToByte(intWriteCount);

						Console.WriteLine(ByteToString(bytSend));


						Comm.Send(bytSend, ref strMsg);
						WaitRecieved();


						//에러체크
						strMsg = chk_Error_byte(yReceived, 0x59);

						if (strMsg != null)
						{
							throw new Exception(string.Format("Address Reading 오류 :{0}", strMsg));
						}

						WroteAddressValue(strChanged, bytSend, yReceived);

					}

					dtWriteOrder.Rows.Clear();

					return;

				}
				catch (Exception ex)
				{
					intErrCount++;

					strMsg = "주소 값 쓰기 오류 [ErrorCount:" + intErrCount.ToString() + "] : " + ex.Message + "\r\n" + ex.ToString();
					PLCModule.clsPLCModule.LogWrite("SendWriteOrder", strMsg);

					if (intErrCount > 4)
					{
						strMsg = "주소 값 쓰기 오류 [ErrorCount:" + intErrCount.ToString() + "]로 인한 연결을 재설정 합니다.";
						PLCModule.clsPLCModule.LogWrite("SendWriteOrder", strMsg);

						dtWriteOrder.Clear();
						RetryOpen(true);
						return;
					}
				}
			}
		}


		private void Received(byte[] bytByte)
		{
			this.yReceived = new Byte[bytByte.Length];
			bytByte.CopyTo(yReceived, 0);

			Console.WriteLine("[수신]:{0}", ByteToString(yReceived));
			//PLCModule.clsPLCModule.LogWrite("SendWriteOrder", string.Format("[수신]:{0}", ByteToString(yReceived)));
			this.isReceived = true;

		}

		private void WaitRecieved()
		{
			try
			{
				DateTime dtSendTime = DateTime.Now;

				TimeSpan dtSpan;
				do
				{
					Thread.Sleep(300);
					dtSpan = DateTime.Now - dtSendTime;
					if (isReceived) break;
				} while (dtSpan.TotalMilliseconds < this.IntTimeOut);

				if (isReceived)
				{
					return;
				}
				else
				{
					throw new Exception("PLC Time Out...");
				}

			}
			catch (Exception ex)
			{
				throw new Exception("PLC Time Out...", ex);
			}
		}





		#region IDisposable 멤버

		public void Dispose()
		{
			this.Close();

		}

		#endregion




	}
}
