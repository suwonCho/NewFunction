using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Threading;


namespace USB.Printing
{
    public partial class Form1 : Form
    {
        //ZebraUsbStream usb;
		UsbPrinterConnector ub;
        Encoding encoding = Encoding.Default;

        public Form1()
        {
            InitializeComponent();

            //usb = new ZebraUsbStream("Port_#0002.Hub_#0006");

			//usb = new ZebraUsbStream();

			//ub = new UsbPrinterConnector();
			ub = new UsbPrinterConnector("Port_#0002.Hub_#0002");
			ub.IsConnected = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //usb.Close();
			ub.IsConnected = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string str = @"A01234    (C)CSW    12345678902011.03.17 ";

            //str = @"A 1";
            //usb.Write(usb.strTestString);

            //usb.XmlFormat_Load("Card_Format.xml");
            //usb.XmlFormat_Print("TEST", str);

			//ub.Write("Text");

			BarCodePrint6("", "140864RTAQ", "140864RTAQ", "Y/김선영", "RTAQ", "1658", 1);

        }


		public char STX = Convert.ToChar(0x2);
		public char ETX = Convert.ToChar(0xD);
		public string strSend;


		public bool BarCodePrint6(string printPort, string data1, string data2, string data3, string data4, string data5, int Cnt)
		{
			string rt1 = "", rt2 = "";

			try
			{
				printPort = "Port_#0002.Hub_#0002";

				strSend = Convert.ToChar(string.Format("{0:x}", 2)) + "qA" + string.Format("{0}", ETX);         //Convert.ToChar(string.Format("{0:x}", 13));
				ub.Write(strSend);


				//if (data2.Length > 0)
				//    rt1 = WinfontPut(printPort, 1, "바탕체", 16, true, 0, data2);
				//else
				//    rt1 = "";
				//if (data3.Length > 0)
				//    rt2 = WinfontPut(printPort, 2, "바탕체", 16, true, 0, data3);
				//else
				//    rt2 = "";


				strSend = string.Format("{0}", STX) + "m" + string.Format("{0}", ETX);
				ub.Write(strSend);

				strSend = string.Format("{0}", STX) + "L" + string.Format("{0}", ETX);
				ub.Write(strSend);

				strSend = "D11" + string.Format("{0}", ETX);
				ub.Write(strSend);

				strSend = "H13" + string.Format("{0}", ETX);
				ub.Write(strSend);


				strSend = string.Format("1a6220001400090{0}{1}", data1, ETX);                    // BarCode Code 39
				//strSend = string.Format("1e6220003600210{0}" , data1 , ETX);                    // BarCode  Code 128
				ub.Write(strSend);

				strSend = string.Format("131200000000490{0}{1}", data4, ETX);
				ub.Write(strSend);

				if (rt1.Length > 0)
				{
					strSend = string.Format("1Y2200000850035{0}{1}", rt1, ETX);
					ub.Write(strSend);
				}
				if (rt2.Length > 0)
				{
					strSend = string.Format("1Y2200000030090{0}{1}", rt2, ETX);
					ub.Write(strSend);
				}

				strSend = string.Format("131200000000320{0}{1}", data5, ETX);
				ub.Write(strSend);


				strSend = string.Format("Q{0:0000}", Cnt) + string.Format("{0}", ETX);
				ub.Write(strSend);

				strSend = "E" + string.Format("{0}", ETX);
				ub.Write(strSend);

				Thread.Sleep(500);
				return true;
			}
			catch
			{
				return false;
			}
		}
       

    }
}
