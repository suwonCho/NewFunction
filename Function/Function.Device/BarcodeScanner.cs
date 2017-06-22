using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Function;
using Function.Util;

namespace Function.Device
{
	public class BarcodeScanner : _SerialDeviceBase
    {

		private event delReceiveStrData _onReceiveBarcode;

		public event delReceiveStrData OnReceiveBarcode
		{
			add { _onReceiveBarcode += value; }
			remove { _onReceiveBarcode -= value; }
		}

		string strCom = string.Empty;

		public BarcodeScanner(string setting) : base(setting)
		{
			initClass();
		}

		private void initClass()
		{
			OnReceiveData += clsSn_DataReceived;
		}

		public BarcodeScanner(XML xml, string strNode) : base(xml, strNode)
		{
			initClass();
		}

		public BarcodeScanner(string strSystemID, XML xml) : base(strSystemID, xml)
		{
			initClass();
		}


		public BarcodeScanner(int portNo, int baudRate, string parity, int dataBits, string stopBits) : base(portNo, baudRate, parity, dataBits, stopBits)
		{
			initClass();
		}

		private void clsSn_DataReceived(byte[] byts)
		{
			if (byts.Length < 1) return;

			//byte[] bytReceive = new byte[clsSn.BytesToRead];
			//int intReceived = clsSn.Read(bytReceive, 0, clsSn.BytesToRead);

			string data = Encoding.ASCII.GetString(byts);

			ThreadPool.QueueUserWorkItem(new WaitCallback(ReceiveBarcode), data);
			
		}

		private void ReceiveBarcode(object obj)
		{
			string strBarcode = (string)obj;

			if (_onReceiveBarcode != null)
			{
				strBarcode = strBarcode.Replace("\r", string.Empty);
				strBarcode = strBarcode.Replace("\n", string.Empty);

				_onReceiveBarcode(this, strBarcode);
			}
		}
		
        public void Open()
        {
			if (clsSn == null) return; 
			
			clsSn.Open();

			IsConnected = IsOpen;
        }


		/// <summary>
		/// 직열포트 연결상태를 가져온다.
		/// </summary>
		public enConnectionStatus IsOpen
		{
			get
			{
				if (clsSn == null) return enConnectionStatus.Disconnected;
				return clsSn.IsOpen ? enConnectionStatus.Connected : enConnectionStatus.Disconnected;
			}
		}
		


        public void Close()
        {
			if (clsSn.IsOpen) clsSn.Close();

			IsConnected = IsOpen;
        }

		public void Dispose()
		{
			if (clsSn.IsOpen) clsSn.Close();

			clsSn.Dispose();

		}



    }


 
}
