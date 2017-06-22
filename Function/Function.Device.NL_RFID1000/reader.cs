using nesslab.reader.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Function.Device.NL_RFID1000
{

	/// <summary>
	///  동작모드
	/// </summary>
	public enum Action
	{
		Start,
		Stop
	}

	/// <summary>
	/// 운영 타입
	/// </summary>
	public enum enOP_Type
	{
		/// <summary>
		/// 계속리딩
		/// </summary>
		N,
		/// <summary>
		/// 센싱 후 인벤토리 시간 동안리딩
		/// </summary>
		I
	}

	public delegate void delReaderAction(object sender, Action act);

	/// <summary>
	/// 리더 관리 클래스
	/// </summary>
	public class reader : Function.Device._DeviceBaseClass, IDisposable
	{

		/// <summary>
		///  리더객체
		/// </summary>
		private Reader dev;
		

		string _ip_Address = null;
		

		/// <summary>
		/// RFID 리더 접속 ip 주소 정보
		/// </summary>
		public string Ip_Address
		{
			get
			{
				return _ip_Address;
			}

			set
			{
				_ip_Address = value;
			}
		}

		event delTagReceive _onTagReceive;


		/// <summary>
		/// 테그 수신시 이벤트가 발생
		/// </summary>
		public event delTagReceive OnTagReceive
		{
			add { _onTagReceive += value;  }
			remove { _onTagReceive -= value; }
		}
		
		/// <summary>
		/// 리더 이름을 가지고 오거나 설정한다.
		/// </summary>
		public string ReaderName
		{
			get; set;
		}


		/// <summary>
		/// 마지막 수신 테그 아디
		/// </summary>
		string LastTagID = string.Empty;

		/// <summary>
		/// 태그 중복 방지 시간(분) - 같은 테그가 같으 시간내에 들어오면 무시한다. - 0이면 사용 안함
		/// </summary>
		public int Tag_duplicate_min
		{
			get
			{
				return _tag_duplicate_min;
			}

			set
			{
				_tag_duplicate_min = value;
			}
		}

		int _port = 5578;
		/// <summary>
		/// tcp/ip 포트 값을 설정하거나 가져옵니다. 기본:5578
		/// </summary>
		public int Port
		{
			get
			{
				return _port;
			}

			set
			{
				_port = value;
			}
		}


		int _tag_duplicate_min = 5;
		Dictionary<string, DateTime> dicTag = new Dictionary<string, DateTime>();

		/// <summary>
		/// 1초 마다 작업 타이머
		/// </summary>
		System.Threading.Timer tmr1sec = null;


		Action _inventory_Status = Action.Stop;
		
		/// <summary>
		/// 인벤토리 상태여부
		/// </summary>
		public Action Inventory_Status
		{
			get
			{
				return _inventory_Status;
			}

			set
			{
				_inventory_Status = value;

				if (_onInventory_Status_Changed != null) _onInventory_Status_Changed(this, _inventory_Status);

			}
		}

	

		event delReaderAction _onInventory_Status_Changed;

		/// <summary>
		/// 인벤토리 상태 여부 변경 이벤트
		/// </summary>
		public event delReaderAction OnInventory_Status_Changed
		{
			add { _onInventory_Status_Changed += value; }
			remove { _onInventory_Status_Changed -= value; }
		}


		enOP_Type op_Type = enOP_Type.N;

		/// <summary>
		/// 리더 운영 타입
		/// </summary>
		public enOP_Type Op_Type
		{
			get
			{
				return op_Type;
			}

			set
			{
				op_Type = value;
			}
		}

		int _inv_time = 300;

		/// <summary>
		/// 인벤토리 지속 시간(초)
		/// </summary>
		public int Inv_time
		{
			get
			{
				return _inv_time;
			}

			set
			{
				_inv_time = value;
			}
		}


		int _lamp_time = 0;


		/// <summary>
		/// 테그리딩후 램프 작동시간(초) 0:사용안함
		/// </summary>
		public int Lamp_time
		{
			get
			{
				return _lamp_time;
			}

			set
			{
				_lamp_time = value;
			}
		}

		int tryConCnt = 0;
        
		int inv_cnt = 0;

		int lamp_cnt = 0;



		/// <summary>
		/// 객체 생성
		/// </summary>
		/// <param name="readerName"></param>
		public reader(string readerName)
		{
			ReaderName = readerName;
			string path = string.Format(@"{0}\LOG_NL_RFID1000\{1}", System.Windows.Forms.Application.StartupPath, readerName);
			Log = new Util.Log(path, readerName, 30, true);

			Log.WLog(string.Format("NL RFID 1000 관리 객체를 생성합니다. [ReaderName]{0}", ReaderName));
		}

		public void Dispose()
		{			
			Disconnect();

			Log.WLog("클래스를 해제 합니다.");
		}

		/// <summary>
		/// RFID 리더와 접속한다.
		/// </summary>
		public void Connect()
		{
            try
			{
				Log.WLog(string.Format("[IP]{0} [Port]{0} 로 연결을 시도합니다.", Ip_Address, _port));
				
				// 리더객체 TCP 타입으로 생성한다.
				dev = new Reader(ConnectType.Tcp);
				// 사용될 리더의 모델타입을 지정한다.
				dev.ModelType = ModelType.NL_RF1000;
				// 이벤트 핸들러를 등록한다.
				dev.ReaderEvent += new ReaderEventHandler(OnReaderEvent);
				// IP, 관리포트(5578)로 Socket 연결을 시작한다.
				dev.ConnectSocket(Ip_Address, _port);

				tryConCnt = 0;

				//10초마다 연결이 끊어 졌을때 재접속을 시도하는 타이머
				tmr1sec = new System.Threading.Timer(new System.Threading.TimerCallback(Work_1Sec), null, 1000, 1000);
				
			}
			catch(Exception ex)
			{
				Log.WLog_Exception("Connect", ex);
				IsConnected = enConnectionStatus.Disconnected;
				//throw ex;
			}
		}


		bool isWork1Sec = false;


        /// <summary>
        /// 램프를 끄거나 껸다
        /// </summary>
        /// <param name="onoff"></param>
        private void Lamp_Order(bool onoff)
        {
            if(onoff)
                dev.SetSioOutputControl(0, 2);
            else
                dev.SetSioOutputControl(0, 0);
        }

        /// <summary>
        /// 센싱여부
        /// </summary>
        bool isSensing = false;

		/// <summary>
		/// 1초 타이머 자업
		/// </summary>
		/// <param name="obj"></param>
		private void Work_1Sec(object obj)
		{
			if (isWork1Sec) return;

			isWork1Sec = true;

			try
			{


				if (IsConnected == enConnectionStatus.Connected)
				{


                    //dev.GetGpioOutputEnable();

                    //dev.GetGpioInputEnable();

                    //dev.SetGpioOutputState(1, true);

                    //dev.SetSioOutputControl(0, 0);
                    



                    //램프 off확인

                    if(lamp_cnt < Lamp_time)
                    {
                        lamp_cnt++;
                    }
                    else
                    {
                        if(lamp_cnt == Lamp_time)
                        {
                            Lamp_Order(false);
                            lamp_cnt++;
                        }
                    }
                    
                    //인벤토리 스톱 - 센싱이 살아 있으면 스톱하지 안는다

                    if (Inventory_Status == Action.Start)
					{

						switch (op_Type)
						{
							case enOP_Type.I:

								inv_cnt++;

								//
								if (inv_cnt >= Inv_time)
								{
									//센싱확인
                                    if(!isSensing)
									    //인벤토리 정지
									    Inventory_Stop();
								}

								break;
						}
					}
				}

#if (!DEBUG)
                //재시도
                DoTryReconnection(null);
#endif
			}
			catch
			{

			}
			finally
			{
				isWork1Sec = false;
			}
		}




		/// <summary>
		/// 접속이 끊어질경우 재시도
		/// </summary>
		/// <param name="obj"></param>
		private void DoTryReconnection(object obj)
		{
			if(tryConCnt < 10)
			{
				tryConCnt++;
				return;
			}

			try
			{
				if (IsConnected == enConnectionStatus.Connected)
				{
					dev.GetAntennaState();
					Console.WriteLine("[{0}] [IP]{1} [Port]{2} 안테나 정보 요청.", DateTime.Now.ToString("HH:mm:ss"), Ip_Address, Port);
					return;
				}

				Console.WriteLine("[{0}] [IP]{1} [Port]{2} 재접속을 시도 합니다.", DateTime.Now.ToString("HH:mm:ss"), Ip_Address, Port );


				tryConCnt = 0;

				dev.ConnectSocket(Ip_Address, _port);

			}
			catch(Exception ex)
			{
				ProcException(ex, "DoTryReconnection");
			}
		}


		/// <summary>
		/// RFID 리더와 접속을 끊는다
		/// </summary>
		public void Disconnect()
		{
			if (dev != null)
			{
                if (dev.IsHandling)
                {// 리더 쓰레드가 동작중인가?

                    //램프를 끈다
                    Lamp_Order(false);

                    dev.Close(CloseType.Close);
                }

				dev.ReaderEvent -= new ReaderEventHandler(OnReaderEvent);
				tmr1sec.Dispose();
			}

		}

		/// <summary>
		/// 인벤토리 작업 시작(리딩 시작)
		/// </summary>
		public void Inventory_Start()
		{
			// ISO18000_6C_GEN2 타입으로 Multi Inventory를 수행합니다.
			dev.ReadTagId(TagType.ISO18000_6C_GEN2, ReadType.MULTI);			
			Inventory_Status = Action.Start;
			inv_cnt = 0;

			Log.WLog("테그 리딩을 시작 합니다.");
		}

		/// <summary>
		/// 인벤토리 작업 종료(리딩 종료)
		/// </summary>
		public void Inventory_Stop()
		{
			bool returnvalue = dev.StopOperationToSync();			
			Inventory_Status = Action.Stop;

			Log.WLog("테그 리딩을 종료 합니다.");

		}
        
		void OnReaderEvent(object sender, ReaderEventArgs e)
		{

            string mmm;
            if (e.Payload != null)
            {
                mmm = Encoding.ASCII.GetString(e.Payload);

                Console.WriteLine("[{0}]{1}", e.Kind, mmm);
            }

            // sender : Reader 객체
            // e.Kind : 발생된 이벤트 종류
            // e.Message : 이벤트 발생에 대한 설명
            // e.Payload : 리더로부터 수신된 바이트 배열
            // e.CloseType : 닫기 유형
            string payload;

			switch (e.Kind)
			{
				// 연결이 정상 처리 되면 발생합니다.
				case ReaderEventKind.Connected:
					Log.WLog("장비와 연결 되었습니다.");
                    
                    //램프를 끈다
                    Lamp_Order(false);
                        
					IsConnected = enConnectionStatus.Connected;

                    dev.GetSioInputControl();				
					break;
				// 연결이 정상적으로 해제되면 발생합니다.
				case ReaderEventKind.Disconnected:
					Log.WLog("장비와 연결이 종료 되었습니다.");
					IsConnected = enConnectionStatus.Disconnected;
					break;
				// 일정시간 동안 응답이 없는 경우 발생합니다.
				case ReaderEventKind.timeout:
					break;
				// 리더의 버전정보 수신시 발생합니다.
				case ReaderEventKind.Version:					
					break;


				// 리더의 안테나 정보 수신시 발생합니다.
				case ReaderEventKind.AntennaState:
				// 리더의 안테나 파워값 수신시 발생합니다.
				case ReaderEventKind.Power:					
					break;


				// Inventory시 Tag ID 수신시 발생합니다.
				case ReaderEventKind.TagId:
					// 리더로부터 수신된 데이터는 바이트 배열에 들어 있으며 문자열로 Decode하여 사용합니다.
					// 한개 이상의 수신 데이터가 들어 있을 수 있습니다.
					// 하기와 같이 "\r\n>" 기준으로 분리하여 사용합니다.

					payload = Encoding.ASCII.GetString(e.Payload);
					string[] tagIds = payload.Split(new string[] { "\r\n>" }, StringSplitOptions.RemoveEmptyEntries);

					foreach (string tagid in tagIds)
					{
						//this.lvwInventory.Items.Add(tagid);
						// 앞의 2자 (1T)는 응답코드이므로 제거합니다.
						string epc = tagid.Substring(2);
						
						
						//HEX 형태를 문자열로 표시해 준다.
						string txt = string.Empty;
						if (dev.TagType == TagType.ISO18000_6C_GEN2)
						{
							// PC 값 "HHHH" 을 제거하고 처리한다 - PC(HHHH)
							int p = 4;
							if (tagid.Length > p)
							{
								//201608 값 그대로 사용 하면 된다.
								//string hex = epc.Substring(p, epc.Length - p);
								//txt = dev.MakeTextFromHex(hex);

								txt = epc;

							}
						}

						//중복수신 체크
						if (Tag_duplicate_min < 1)
							dicTag.Clear();
						else
						{
						
							string[] tags = dicTag.Keys.ToArray<string>();

							//시간이 지난 정보 삭제
							foreach(string tag in tags)
							{
								TimeSpan span = DateTime.Now - dicTag[tag];
								if (span.TotalMinutes > Tag_duplicate_min) dicTag.Remove(tag);							
							}
							
							if(dicTag.ContainsKey(txt)|| LastTagID.Equals(txt))
							{
								Log.WLog("RFID TAG 중복수신 - 처리 무시 [ROW]{0} [TXT]{1}", epc, txt);
								return;
							}

							//중복수신 방지 
							dicTag.Add(txt, DateTime.Now);

							LastTagID = txt;
						}
						

						Log.WLog("RFID TAG 수신 [ROW]{0} [TXT]{1}", epc, txt);

                        //램프를 켠다
                        Lamp_Order(true);
                        lamp_cnt = 0;

                        if (_onTagReceive != null) _onTagReceive(this, epc, txt);

					}
					break;


				// Memory Bank 값 Read시 발생
				case ReaderEventKind.GetTagMemory:					
					break;
				// Memory Bank 값 Write, Lock, Kill 등 작업에 대한 응답
				case ReaderEventKind.TagResponseCode:
					// C : Error등 응답코드					
					break;

                case ReaderEventKind.SioInputStatus:
                case ReaderEventKind.SioInputTrigger:

                    if (Op_Type == enOP_Type.I)
                    {
                        payload = Encoding.ASCII.GetString(e.Payload);


                        switch(payload)
                        {
                            
                            case "#PPACBF00#":
                            case "#PPACIS0000000000#":
                                isSensing = true;
                                break;

                            default:
                                isSensing = false;
                                break;
                        }

                        //isSensing = payload.Equals("#PPACBF00#") || payload.Equals("#PPACOS0000000000#");

                        Console.WriteLine("센스확인 {0}", isSensing);

                        Log.WLog("센스확인 {0}", isSensing);

                        inv_cnt = 0;

                        if(isSensing && Inventory_Status != Action.Start)
                        {
                            Inventory_Start();
                        }
                    }

                   
                    
                     
                    
                    break;
			}
		}






	}	//END CLASS
}
