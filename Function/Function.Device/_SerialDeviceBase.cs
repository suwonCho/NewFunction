using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Function;
using Function.Util;

namespace Function.Device
{
	public abstract class _SerialDeviceBase : _DeviceBaseClass, IDisposable
	{
		public Function.Comm.Serial clsSn;

		public delegate void delReceiveData(byte[] data);

		/// <summary>
		/// 수신 시 발생 이벤트
		/// </summary>
		event delReceiveData _onReceiveData;

		/// <summary>
		/// 수신 시 발생 이벤트
		/// </summary>
		internal event delReceiveData OnReceiveData
		{
			add { _onReceiveData += value; }
			remove { _onReceiveData += value; }
		}

		/// <summary>
		/// 장비이름을 설정하거나 가져온다.
		/// </summary>
		public string DeviceName
		{
			get;
			set;
		}

		/// <summary>
		/// Tag정보를 가져오거나 설정한다.
		/// </summary>
		public object Tag
		{
			get;
			set;
		}

		string strCom = string.Empty;




		public _SerialDeviceBase(XML xml, string strNode)
		{
			xml.chNode2Root();

			string strPROGID = xml.GetSingleNodeValue(strNode);

			InitXml(strPROGID, xml);
		}

		public _SerialDeviceBase(string strSystemID, XML xml)
		{
			InitXml(strSystemID, xml);
		}

		private void InitXml(string strSystemID, XML xml)
		{
			xml.chNode2Root();
			xml.chSingleNode(@"SETTING/" + strSystemID + "/BARCODE");

			strCom = xml.GetSingleNodeValue("COMPORT");
			string strBaudrate = xml.GetSingleNodeValue("BAUDRATE");
			string strPARITY = xml.GetSingleNodeValue("PARITY");
			string strDATABITS = xml.GetSingleNodeValue("DATABITS");
			string strSTOPBITS = xml.GetSingleNodeValue("STOPBITS");

			if (strCom == null || strCom == string.Empty) return;

			clsSn = new Function.Comm.Serial(strCom, int.Parse(strBaudrate), strPARITY, int.Parse(strDATABITS), strSTOPBITS);
			clsSn.OnDataReceived += new Function.Comm.Serial.delReceive(clsSn_DataReceived);

		}

		/// <summary>
		/// 시리얼 설정을 한다.
		/// </summary>
		/// <param name="setting">PortNo;boudrate;parity;databits;stopbits</param>
		public _SerialDeviceBase(string setting)
		{
			try
			{
				string[] set = setting.Split(';');

				int portNo = int.Parse(set[0]);
				int baudRate = int.Parse(set[1]);
				string parity = set[2];
				int dataBits = int.Parse(set[3]);
				string stopBits = set[4];

				initSerial(portNo, baudRate, parity, dataBits, stopBits);
				
			}
			catch
			{
				LastLog = "시리얼 포트 설정중에 오류가 발생했습니다 - _SerialDeviceBase(string setting)";
				throw new Exception("시리얼 포트 설정중에 오류가 발생했습니다 - _SerialDeviceBase(string setting)");
			}

		}

		public _SerialDeviceBase(int portNo, int baudRate, string parity, int dataBits, string stopBits)
		{
			initSerial(portNo, baudRate, parity, dataBits, stopBits);
		}

		private void initSerial(int portNo, int baudRate, string parity, int dataBits, string stopBits)
		{
			strCom = string.Format("COM{0}", portNo);
			clsSn = new Function.Comm.Serial(strCom, baudRate, parity, dataBits, stopBits);
			clsSn.OnDataReceived += new Function.Comm.Serial.delReceive(clsSn_DataReceived);
		}


		private void clsSn_DataReceived(Function.Comm.Serial port, byte[] byts)
		{
			
			//Console.WriteLine("[time]{0} / {1}", Fnc.Bytes2String(byts), Encoding.Default.GetString(byts));
			
			if (byts.Length < 1) return;

			_onReceiveData(byts);

		}

		public void SendData(byte[] data)
		{
			clsSn.Write(data, 0, data.Length);
		}


		public void Open()
		{
			if (clsSn == null || strCom == string.Empty) return;

			try
			{
				LastLog = string.Format("장비와 연결을 시작합니다. [Port]{0} [BaudRate]{1} [Parity]{2} [DataBits]{3} [StopBits]{4}",
					clsSn.PortName, clsSn.BaudRate, clsSn.Parity, clsSn.DataBits, clsSn.StopBits);

				clsSn.Open();

				IsConnected = enConnectionStatus.Connected;
			}
			catch(Exception ex)
			{
				IsConnected = enConnectionStatus.Disconnected;
				LastLog = ex.Message;
				throw ex;
			}

			
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



		public void Close()
		{
			if (clsSn.IsOpen) clsSn.Close();

			IsConnected = enConnectionStatus.Disconnected;

			LastLog = "포트를 종료합니다.";
		}

		public void Dispose()
		{
			if (clsSn.IsOpen) clsSn.Close();

			clsSn.Dispose();

		}



	}



}
