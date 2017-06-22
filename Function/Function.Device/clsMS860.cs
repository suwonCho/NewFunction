using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using Function;
using Function.Comm;

namespace Function.Device
{
	/// <summary>
	/// MicroScan 860 : AutoScanner
	/// </summary>
	public class clsMS860
	{
		Function.Comm.Serial clsSn;

		public delegate void delBarcodeReceived(string strBarcode);
		public delBarcodeReceived evtBarcodeReceived = null;



		public clsMS860(Function.Util.XML xml, string strNode)
		{
			xml.chNode2Root();
			xml.chSingleNode(strNode);

			string strCom = xml.GetSingleNodeValue("COMPORT");
			string strBaudrate = xml.GetSingleNodeValue("BAUDRATE");
			string strPARITY = xml.GetSingleNodeValue("PARITY");
			string strDATABITS = xml.GetSingleNodeValue("DATABITS");
			string strSTOPBITS = xml.GetSingleNodeValue("STOPBITS");

			clsSn = new Function.Comm.Serial(strCom, int.Parse(strBaudrate), strPARITY, int.Parse(strDATABITS), strSTOPBITS);			
			clsSn.OnDataReceived += new Serial.delReceive(clsSn_DataReceived);
			
		}

		public clsMS860(string strPort, BaudRate baudRate, Parity parity, DataBits databits, StopBits stopbits)
		{
			clsSn = new Serial(strPort, baudRate, parity, databits, stopbits);
			clsSn.OnDataReceived += new Serial.delReceive(clsSn_DataReceived);
		}




		private void clsSn_DataReceived(Function.Comm.Serial port, byte[] byts)
		{
			if (evtBarcodeReceived == null) return;

			if (byts.Length < 1) return;

			string data = Encoding.ASCII.GetString(byts);

			evtBarcodeReceived(data);

		}


		/// <summary>
		/// 직렬 포트틀 연다..
		/// </summary>
		public void Open(bool isSendSetting)
		{
			if (clsSn == null) return;

			clsSn.Open();

			if (isSendSetting)
				Send_All_Setting();
		}

		/// <summary>
		/// 직열포트 연결상태를 가져온다.
		/// </summary>
		public bool IsOpen
		{
			get
			{
				if (clsSn == null) return false;
				return clsSn.IsOpen;
			}
		}

		/// <summary>
		/// 직렬 포트를 닫는다.
		/// </summary>
		public void Close()
		{
			clsSn.Close();
		}

		/// <summary>
		/// 모든 셑팅을 기기에 내려 보낸다..
		/// </summary>
		public void Send_All_Setting()
		{
			if (clsSn.IsOpen)
			{
				SetLazerWatingMode = SetLazerWatingMode;
				SetMotorWatingMode = SetMotorWatingMode;
				SetRasterWatingMode = SetRasterWatingMode;
				SetTriggerLetter = SetTriggerLetter;
			}
		}





		public void Write(string str)
		{
			if (clsSn != null && clsSn.IsOpen)
				clsSn.Write(str);
		}


		private int intendReadCycleTime;
		/// <summary>
		/// 리드사이클 앤드 설정시 타임 아웃 설정시 시간 입력(ms)
		/// </summary>
		public int SetEndReadCycleTime
		{
			get
			{
				return intendReadCycleTime * 10;
			}
			set
			{
				if (value < 10)
				{
					intendReadCycleTime = 1;
				}
				else
				{
					intendReadCycleTime = value / 10;
				}

				Write(string.Format("<K220,,{0}>", intendReadCycleTime));
			}

			
		}


		public enum enWatingMode : int { 
					/// <summary>
					/// 항시작동
					/// </summary>
					Always = 0, 
					/// <summary>
					/// 대기모드
					/// </summary>
					Wating };

		private enWatingMode enLazerWatingMode = enWatingMode.Wating;
		/// <summary>
		/// 레이져 대기모드 상태를 가져오거나 셑팅한다.
		/// </summary>
		public enWatingMode SetLazerWatingMode
		{
			set
			{
				Write(string.Format("<K700,{0}>", (int)value));
				enLazerWatingMode = value;
				
			}
			get
			{
				return enLazerWatingMode;
			}
		}


		private enWatingMode enRasterWatingMode = enWatingMode.Wating;
		/// <summary>
		/// 조준경 대기모드 상태를 가져오거나 셑팅한다.
		/// </summary>
		public enWatingMode SetRasterWatingMode
		{
			set
			{
				Write(string.Format("<K506,,,,,{0}>", (int)value));
				enRasterWatingMode = value;

			}
			get
			{
				return enRasterWatingMode;
			}
		}

		private enWatingMode enMotorWatingMode = enWatingMode.Wating;
		/// <summary>
		/// 모터 대기모드 상태를 가져오거나 셑팅한다.
		/// </summary>
		public enWatingMode SetMotorWatingMode
		{
			set
			{				

				if (value == enWatingMode.Wating)
				{
					Write("<K501,>");
				}
				else
				{
					Write("<K500,>");
				}

				enMotorWatingMode = value;

			}
			get
			{
				return enMotorWatingMode;
			}
		}





		public enum enPorwer : int
		{
			Low = 0,
			Medium = 1,
			High = 2
		}


		private enPorwer enLazerPower = enPorwer.Medium;
		/// <summary>
		/// 레이져 세기 상태를 가져오거나 셑팅한다.
		/// </summary>
		public enPorwer SetLazerPower
		{
			set
			{
				Write(string.Format("<K700,,,,,{0}>", (int)value));
				enLazerPower = value;

			}
			get
			{
				return enLazerPower;
			}
		}


		public enum enReadTriggerMode : int
		{
			/// <summary>
			/// 항상 수신 모드 : 조준경,레이져,모터모드를 Always로 변경
			/// </summary>
			ContinuousRead = 0,
			/// <summary>
			/// 1회수신시까지 계속 수신 : 조준경,레이져,모터모드를 Always로 변경
			/// </summary>
			ContinuousRead1Output  = 1,
			/// <summary>
			/// 외부 트리거 모드
			/// </summary>
			ExternalLevel = 2,
			/// <summary>
			/// 외부 트리거 종료시 수신 모드
			/// </summary>
			ExternalEdge = 3,
			/// <summary>
			///  트리거 수신시 수신: 조준경,레이져,모터모드를 Wating으로 변경
			/// </summary>
			SerialData = 4,
			/// <summary>
			/// 트리거 수신 및 외부트리거 모드 : 조준경,레이져,모터모드를 Wating으로 변경
			/// </summary>
			SerialData_ExternalEdge = 5
		}
		private enReadTriggerMode emReadTriggerMode = enReadTriggerMode.SerialData;

		/// <summary>
		/// 리드 방식을 가져오거나 설정합니다.
		/// </summary>
		public enReadTriggerMode SetReadTriggerMode
		{
			get { return emReadTriggerMode; }
			set
			{
				switch (value)
				{
					case enReadTriggerMode.ContinuousRead:
					case enReadTriggerMode.ContinuousRead1Output:
						SetLazerWatingMode = enWatingMode.Always;
						SetMotorWatingMode = enWatingMode.Always;
						SetRasterWatingMode = enWatingMode.Always;
						break;
					case enReadTriggerMode.SerialData:
					case enReadTriggerMode.SerialData_ExternalEdge:
						SetLazerWatingMode = enWatingMode.Wating;
						SetMotorWatingMode = enWatingMode.Wating;
						SetRasterWatingMode = enWatingMode.Wating;
						break;
				}
				
				Write(string.Format("<K200,{0}>", (int)value));
				emReadTriggerMode = value;
			}
		}


		private string strTriggerLetter = "R";
		/// <summary>
		/// 트리거 시리얼리드 방식 에 사용할 문자(대문자)를 가져오거나 설정합니다.
		/// </summary>
		/// <param name="value"></param>
		public string SetTriggerLetter
		{
			get{ return strTriggerLetter; }
			set
			{
				if (value.Length != 1)
				{
					throw new Exception("트리거 문자는 1글자 이여야 합니다.");
				}

				Write(string.Format("<K201,{0}>", value.ToUpper()));
				strTriggerLetter = value.ToUpper();
			}
		}

		/// <summary>
		/// 바코드 리딩 트리거 신호를 보냅니다.
		/// </summary>
		public void SendTrigger()
		{
			if (SetMotorWatingMode != enWatingMode.Always)
			{
				SetMotorWatingMode = enWatingMode.Always;
				Thread.Sleep(1000);
			}
			Write(string.Format("<{0}>", strTriggerLetter));
		}


		private int intFilterTime = 1000; //100ms

		/// <summary>
		/// 필터 타임(같은 바코드 무시시간)을 가져오거나 설정합니다. (컨티뉴 리드일경우)
		/// 0.1 ~ 500ms
		/// </summary>
		public int setFilterTime
		{
			//10 = 1mS 범위 0.1mS - 500mS(1 ~ 5000)
			get { return intFilterTime * 10; }
			set
			{
				int intTemp;
				if (value > 50000)
					intTemp = 5000;
				else if (value < 0.1)
					intTemp = 1;
				else
					intTemp = value / 10;

				Write(string.Format("<K200,,{0}>", intTemp));

				intFilterTime = intTemp;
			}
		}

		public enum enEndReadCycle : int
		{
			TimeOut = 0,
			NewTrigger = 1,
			TimeOut_NewTrigger = 2
		}

		private enEndReadCycle emSetEndReadCycle = enEndReadCycle.TimeOut_NewTrigger;

	
		/// <summary>
		/// 리드 사이클을 가져오거나 설정합니다.
		/// </summary>
		/// <param name="value"></param>
		public enEndReadCycle SetEndReadCycle
		{
			get
			{
				return emSetEndReadCycle;
			}
			set
			{
				Write(string.Format("<K220,{0}>", (int)value));
				emSetEndReadCycle = value;
			}
		
		}
	

		//RS-232/422 BaudRate 셋업 
		private void BaudRateSetup(int value)
		{
			clsSn.Write(string.Format("<K100,{0}>", value));
			//0 = 600, 1 = 1200, 2 = 2400, 3 = 4800, 4 = 9600, 5 = 19.2K, 6 = 38.4K, 7 = 57.6K, 8 = 115.2K 
		}
	}
}
