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
		/// ���� ��ƮƲ ����..
		/// </summary>
		public void Open(bool isSendSetting)
		{
			if (clsSn == null) return;

			clsSn.Open();

			if (isSendSetting)
				Send_All_Setting();
		}

		/// <summary>
		/// ������Ʈ ������¸� �����´�.
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
		/// ���� ��Ʈ�� �ݴ´�.
		/// </summary>
		public void Close()
		{
			clsSn.Close();
		}

		/// <summary>
		/// ��� �V���� ��⿡ ���� ������..
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
		/// �������Ŭ �ص� ������ Ÿ�� �ƿ� ������ �ð� �Է�(ms)
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
					/// �׽��۵�
					/// </summary>
					Always = 0, 
					/// <summary>
					/// �����
					/// </summary>
					Wating };

		private enWatingMode enLazerWatingMode = enWatingMode.Wating;
		/// <summary>
		/// ������ ����� ���¸� �������ų� �V���Ѵ�.
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
		/// ���ذ� ����� ���¸� �������ų� �V���Ѵ�.
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
		/// ���� ����� ���¸� �������ų� �V���Ѵ�.
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
		/// ������ ���� ���¸� �������ų� �V���Ѵ�.
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
			/// �׻� ���� ��� : ���ذ�,������,���͸�带 Always�� ����
			/// </summary>
			ContinuousRead = 0,
			/// <summary>
			/// 1ȸ���Žñ��� ��� ���� : ���ذ�,������,���͸�带 Always�� ����
			/// </summary>
			ContinuousRead1Output  = 1,
			/// <summary>
			/// �ܺ� Ʈ���� ���
			/// </summary>
			ExternalLevel = 2,
			/// <summary>
			/// �ܺ� Ʈ���� ����� ���� ���
			/// </summary>
			ExternalEdge = 3,
			/// <summary>
			///  Ʈ���� ���Ž� ����: ���ذ�,������,���͸�带 Wating���� ����
			/// </summary>
			SerialData = 4,
			/// <summary>
			/// Ʈ���� ���� �� �ܺ�Ʈ���� ��� : ���ذ�,������,���͸�带 Wating���� ����
			/// </summary>
			SerialData_ExternalEdge = 5
		}
		private enReadTriggerMode emReadTriggerMode = enReadTriggerMode.SerialData;

		/// <summary>
		/// ���� ����� �������ų� �����մϴ�.
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
		/// Ʈ���� �ø��󸮵� ��� �� ����� ����(�빮��)�� �������ų� �����մϴ�.
		/// </summary>
		/// <param name="value"></param>
		public string SetTriggerLetter
		{
			get{ return strTriggerLetter; }
			set
			{
				if (value.Length != 1)
				{
					throw new Exception("Ʈ���� ���ڴ� 1���� �̿��� �մϴ�.");
				}

				Write(string.Format("<K201,{0}>", value.ToUpper()));
				strTriggerLetter = value.ToUpper();
			}
		}

		/// <summary>
		/// ���ڵ� ���� Ʈ���� ��ȣ�� �����ϴ�.
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
		/// ���� Ÿ��(���� ���ڵ� ���ýð�)�� �������ų� �����մϴ�. (��Ƽ�� �����ϰ��)
		/// 0.1 ~ 500ms
		/// </summary>
		public int setFilterTime
		{
			//10 = 1mS ���� 0.1mS - 500mS(1 ~ 5000)
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
		/// ���� ����Ŭ�� �������ų� �����մϴ�.
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
	

		//RS-232/422 BaudRate �¾� 
		private void BaudRateSetup(int value)
		{
			clsSn.Write(string.Format("<K100,{0}>", value));
			//0 = 600, 1 = 1200, 2 = 2400, 3 = 4800, 4 = 9600, 5 = 19.2K, 6 = 38.4K, 7 = 57.6K, 8 = 115.2K 
		}
	}
}
