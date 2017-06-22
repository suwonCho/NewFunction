using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Function;
using Function.Util;


namespace Function.Device
{
	
    public class clsM80S : IDisposable
    {
		Function.Comm.Serial clsSn;
		public delegate void delReceiveBarcode(string strBarcode);
		
		/// <summary>
		/// 수신 시 발생 이벤트
		/// </summary>
		public delReceiveBarcode evtReceiveBarcode;
		/// <summary>
		/// 다음 신호를 기다리는 시간
		/// </summary>
		private int intWaitingTime = 100;


		/// <summary>
		/// PLC 접점 ON OFF 하기
		/// ENQ + "00WSB06%PW004010" + "000" +   EOT
		///                            접점상태
		/// </summary>
		private readonly byte[] O_OFF = new byte[] 
		{ 0x05, 0x30, 0x30, 0x57, 0x53, 0x42, 0x30, 0x36, 0x25, 0x50, 0x57, 0x30, 0x30, 0x34, 0x30, 0x31, 0x30, 0x30, 0x30, 0x30, 0x04 };
		// ENQ    0     0     W     S     B     0     6     %     P     W     0     0     4     0     1     0     0     0     0    EOT
		//  0     1     2     3     4     5     6     7     8     9    10    11    12    13    14    15    16    17    18    19    20    21

		private readonly byte[] O_GREEN = new byte[] { 0x05, 0x30, 0x30, 0x57, 0x53, 0x42, 0x30, 0x36, 0x25, 0x50, 0x57, 0x30, 0x30, 0x34, 0x30, 0x31, 0x30, 0x30, 0x32, 0x30, 0x04 };
		private readonly byte[] O_REDALARM = new byte[] { 0x05, 0x30, 0x30, 0x57, 0x53, 0x42, 0x30, 0x36, 0x25, 0x50, 0x57, 0x30, 0x30, 0x34, 0x30, 0x31, 0x30, 0x30, 0x35, 0x30, 0x04 };
		private readonly byte[] O_RED = new byte[] { 0x05, 0x30, 0x30, 0x57, 0x53, 0x42, 0x30, 0x36, 0x25, 0x50, 0x57, 0x30, 0x30, 0x34, 0x30, 0x31, 0x30, 0x30, 0x31, 0x30, 0x04 };
		// ENQ    0     0     W     S     B     0     6     %     P     W     0     0     4     0     1     0     0     0     0    EOT
		//  0     1     2     3     4     5     6     7     8     9    10    11    12    13    14    15    16    17    18    19    20    21

		string strCom = string.Empty;


		public clsM80S(XML xml)
		{
			xml.chNode2Root();
			
			xml.chSingleNode("SystemInfo");
			string strPROGID = xml.GetSingleNodeValue("PROGRAMID");

			InitXml(strPROGID, xml);

		}

		public clsM80S(string strSystemID, XML xml)
		{			
			InitXml(strSystemID, xml);
		}

		private void InitXml(string strSystemID, XML xml)
		{
			xml.chNode2Root();
			xml.chSingleNode(@"SETTING/" + strSystemID + "/MINIPLC");

			strCom = xml.GetSingleNodeValue("COMPORT");
			string strBaudrate = xml.GetSingleNodeValue("BAUDRATE");
			string strPARITY = xml.GetSingleNodeValue("PARITY");
			string strDATABITS = xml.GetSingleNodeValue("DATABITS");
			string strSTOPBITS = xml.GetSingleNodeValue("STOPBITS");

			clsSn = new Function.Comm.Serial(strCom, int.Parse(strBaudrate), strPARITY, int.Parse(strDATABITS), strSTOPBITS);
			clsSn.Open();
		}

		public clsM80S(string portName, int baudRate, string parity, int dataBits, string stopBits)
		{
			strCom = portName;
			clsSn = new  Function.Comm.Serial(portName, baudRate, parity, dataBits, stopBits);
			clsSn.Open();
		}

		public enum OrderType { Green, RedAlarm, Red, Off };
		
		struct tSt 
		{
			public OrderType otFirst;
			public OrderType otSecond;
			public int	intDuration;
		}

		public void Order2PLC(OrderType otFirst, OrderType otSecond, int intDuration)
		{
			tSt st = new tSt();

			st.otFirst = otFirst;
			st.otSecond = otSecond;

			st.intDuration = intDuration;

			ThreadPool.QueueUserWorkItem(new WaitCallback(order2plc), st);

			
		}


		private void order2plc(object obj)
		{
			tSt st = (tSt)obj;
			try
			{
				SetOrderType(st.otFirst);

				if (st.intDuration > 0)
				{
					Thread.Sleep(st.intDuration);
					SetOrderType(st.otSecond);
				}

			}
			catch
			{
			}

		}

		private void SetOrderType(OrderType ot)
		{
			try
			{
				switch (ot)
				{
					case OrderType.Green:
						SetGreen();
						break;


					case OrderType.Red:
						SetRed();
						break;


					case OrderType.RedAlarm:
						SetRedAlarm();
						break;


					default:
						SetOff();
						break;
				}
			}
			catch
			{
			}
		}

		public void SetRedAlarm()
		{
			clsSn.Write(O_REDALARM, 0, O_REDALARM.Length);
		}

		public void SetRed()
		{
			clsSn.Write(O_RED, 0, O_RED.Length);
		}

		public void SetGreen()
		{
	
			clsSn.Write(O_GREEN, 0, O_GREEN.Length);
		}

		public void SetOff()
		{
			clsSn.Write(O_OFF, 0, O_OFF.Length);
		}
		
        public void Open()
        {
			if(strCom != string.Empty)		clsSn.Open();
        }

        public void Close()
        {
			if (clsSn.IsOpen) clsSn.Close();			
        }

		public void Dispose()
		{
			if (clsSn.IsOpen) clsSn.Close();

			clsSn.Dispose();

		}



    }
}
