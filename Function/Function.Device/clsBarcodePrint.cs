using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using System.Data;
using Function;
using Function.Util;

namespace Function.Device
{
	public class clsBarcodePrinter :IDisposable
	{
		Function.Comm.Serial clsSn;
		Function.Comm.SocketClient clsTcp;

		string strformat = string.Empty;
		string strfromatName = string.Empty;

		bool isChangedFormat = false;

		Encoding encoding = Encoding.Default;


		readonly string strStatusCheck = "~HS";
		readonly int intAckWatingTime = 3000;

		string strCom = string.Empty;

		public DataTable dtFormatItem = new DataTable();


		private void InitXml(string strNode, XML xml)
		{
			Init();

			
			xml.chNode2Root();
			xml.chSingleNode(strNode);

			LoadFormatFromXml(xml.GetSingleNodeValue("FORMAT"));

			if (xml.GetSingleNodeValue("TYPE") == "TCP")
			{
				xml.chSingleNode(@"TCP");
				string strIP = xml.GetSingleNodeValue("IP");
				int intPort = int.Parse(xml.GetSingleNodeValue("PORT"));


				clsTcp = new  Function.Comm.SocketClient(strIP, intPort);
				clsTcp.evtReceived += new  Function.Comm.SocketClient.delReceive(ReceiveAck);
				clsSn = null;
			}
			else
			{
				xml.chSingleNode(@"SERIAL");
				strCom = xml.GetSingleNodeValue("COMPORT");
				string strBaudrate = xml.GetSingleNodeValue("BAUDRATE");
				string strPARITY = xml.GetSingleNodeValue("PARITY");
				string strDATABITS = xml.GetSingleNodeValue("DATABITS");
				string strSTOPBITS = xml.GetSingleNodeValue("STOPBITS");

				clsSn = new  Function.Comm.Serial(strCom, int.Parse(strBaudrate), strPARITY, int.Parse(strDATABITS), strSTOPBITS);
				clsSn.OnDataReceived += new  Function.Comm.Serial.delReceive(ReceiveAck);
				clsTcp = null;

				Open();
			}			
		}
		
		/// <summary>
		/// Serial ����� ��ü ����
		/// </summary>
		/// <param name="portName"></param>
		/// <param name="baudRate"></param>
		/// <param name="parity"></param>
		/// <param name="dataBits"></param>
		/// <param name="stopBits"></param>
		public clsBarcodePrinter(string portName, int baudRate, string parity, int dataBits, string stopBits)
		{
			strCom = portName;
			clsSn = new	 Function.Comm.Serial(portName, baudRate, parity, dataBits, stopBits);
			clsSn.OnDataReceived += new  Function.Comm.Serial.delReceive(ReceiveAck);
			clsTcp = null;
			Init();
		}

		/// <summary>
		/// TCP/IP ����� ��ü ����
		/// </summary>
		/// <param name="strIP"></param>
		/// <param name="intPort"></param>
		public clsBarcodePrinter(string strIP, int intPort)
		{
			clsTcp = new  Function.Comm.SocketClient(strIP, intPort);
			clsTcp.evtReceived = new Function.Comm.SocketClient.delReceive(ReceiveAck);
			clsSn = null;
			Init();
		}

		private void Init()
		{
			dtFormatItem.Columns.Add("Function");
			dtFormatItem.Columns.Add("Col");
		}



		public void LoadFormatFromXml(string strFormatName)
		{
			try
			{
				if (strFormatName == string.Empty) return;

				XML xml = new XML(XML.enXmlType.File, @"./BARCODE_FORMAT.XML");

				xml.chSingleNode(strFormatName);

				strformat = xml.GetSingleNodeValue("FORMAT");

				xml.chSingleNode("FUNCTION");


				XmlNode xn = xml.GetXmlNode;


				dtFormatItem.Rows.Clear();

				foreach (XmlNode n in xn.ChildNodes)
				{
					DataRow dr = dtFormatItem.NewRow();

					dr[0] = n.Name;
					dr[1] = n.InnerText.Trim();

					dtFormatItem.Rows.Add(dr);
				}

				xml.chNode2Root();
				xml.chSingleNode(strFormatName);
				xml.chSingleNode("OFFSET");

				int x = int.Parse(xml.GetSingleNodeValue("X"));
				int y = int.Parse(xml.GetSingleNodeValue("Y"));

				Offset(x, y);

				isChangedFormat = true;

			}
			catch
			{
			}
		}

		private void Offset(int x, int y)
		{
			if (x == 0 && y == 0) return;

			int intLen = strformat.Length;
			int intFrom = 0;
			int intTo = 0;

			while(true)
			{
				intFrom = strformat.IndexOf("^FO", intFrom);

				if (intFrom < 0) break;

				intTo = strformat.IndexOf("^", intFrom + 1);

				string s = strformat.Substring(intFrom, intTo - intFrom + 1);

				s = s.Replace("^", "");
				s = s.Replace("FO", "");

				string[] f = s.Split(new string[] { "," }, StringSplitOptions.None	);

				f[0] = (int.Parse(f[0]) + x).ToString();
				f[1] = (int.Parse(f[1]) + y).ToString();

				s = string.Format("^FO{0},{1}^", f[0], f[1]);

				strformat = strformat.Remove(intFrom, intTo - intFrom + 1);
				strformat = strformat.Insert(intFrom, s);

				intFrom += s.Length;
			
			}


		}


		/// <summary>
		/// ���� ������¸� �����´�.
		/// </summary>
		public bool isConnected
		{
			get
			{
				if (clsSn == null)
						return clsTcp.isConnected;
				else
						return clsSn.IsOpen;
			}
		}

		/// <summary>
		/// ������ ����.
		/// </summary>
		public void Open()
		{
			if (clsSn == null)
				clsTcp.Open();
			else
			{
				if (strCom != string.Empty)	clsSn.Open();
			}
		}

		/// <summary>
		/// ������ �ݴ´�.
		/// </summary>
		public void Close()
		{
			if (clsSn == null)
			{
				if (clsTcp.isConnected)
					clsTcp.Close();
			}
			else
				if (clsSn.IsOpen) clsSn.Close();


				
		}


		/// <summary>
		/// ������ �����Ѵ�.
		/// </summary>
		public string strFormat
		{
			get { return strformat; }
			set
			{
				if (strformat != value)
				{
					isChangedFormat = true;
					strformat = value;
				}
			}
		}

		/// <summary>
		/// ������ �����Ѵ�.
		/// </summary>
		private void SendFormat()
		{
			if (!isChangedFormat) return;

			//������ ����Ǹ� �����̸��� ����ɼ� �ִ�.
			int intFrom = strformat.IndexOf("^DFR:", 0);

			if (intFrom > 0)
			{ //���˿� �̸��� ����.

				int intTo = strformat.IndexOf("^", intFrom + 1);

				string s = strformat.Substring(intFrom, intTo - intFrom + 1);

				s = s.Replace("^", "");
				s = s.Replace("DFR:", "");

				strfromatName = s;
			}
			else
				strfromatName = String.Empty;

			SendStringData(strformat);
		}

		/// <summary>
		/// �����͸� ���� �Ѵ�.
		/// </summary>
		/// <param name="strData"></param>
		/// <param name="isWithFormat">format���ۿ���</param>
		public void SendData(string strData, bool isWithFormat)
		{
			try
			{
				//tcp/ip ���ῡ ��� ���۽� ���� ������ �ϰ�, serial�� ������� ���¿��� ����Ѵ�.
				if (!isConnected)
					Open();

				//����üũ�� �Ѵ�.
				CheckStatus();


				//�������ۿ��� - ������ ���� �Ǹ� ������.
				if (isWithFormat)
				{
					SendFormat();
				}

				SendStringData(strData);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (clsSn == null && isConnected ) Close();
			}
		}

		public void SendData(DataRow d, string strAdd, bool isWithFormat)
		{
			try
			{
				//tcp/ip ���ῡ ��� ���۽� ���� ������ �ϰ�, serial�� ������� ���¿��� ����Ѵ�.
				if (!isConnected)
					Open();

				//�������ۿ��� - ������ ���� �Ǹ� ������.
				if (isWithFormat)
				{
					SendFormat();
				}

				string strData = "^XA";	//���
				strData += "^XFR:" + strfromatName;
				strData += strAdd;		//�߰� ������

				foreach (DataRow dr in dtFormatItem.Rows)
				{
					string strCol = Fnc.obj2String(dr["COL"]);
					
					strData += string.Format("^{0}^FD{1}^FS", dr["FUNCTION"], d[strCol]);
				}

				strData += "^XZ";

				SendStringData(strData);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (clsSn == null && isConnected) Close();
			}
		}

		private void SendStringData(string strData)
		{
			byte[] bytData = encoding.GetBytes(strData);
			SendBytesData(bytData);
		}
		private void SendBytesData(byte[] bytData)
		{
			if (clsSn == null)
			{				
				clsTcp.Send_Sync(bytData);				
			}
			else
				clsSn.Write(bytData, 0, bytData.Length);
		}


		public void CheckStatus()
		{
#if(NO_PRINT)
			return;
#endif
			//tcp/ip ���ῡ ��� ���۽� ���� ������ �ϰ�, serial�� ������� ���¿��� ����Ѵ�.
			try
			{
				//tcp/ip ���ῡ ��� ���۽� ���� ������ �ϰ�, serial�� ������� ���¿��� ����Ѵ�.
				if (!isConnected)
					Open();

				strAck = string.Empty;
				isStatusChecking = true;
				SendStringData(strStatusCheck);

				int i = 0;

				while (i < intAckWatingTime)
				{
					int j = intAckWatingTime - i;
					if (j > 300) j = 300;

					Thread.Sleep(j);
					i += j;
					//ack����
					if (!isStatusChecking) break;
				}

				//ack�� �� ����
				if (isStatusChecking)
				{
					isStatusChecking = false;
					throw new Exception("Barcode �����ͷ� ���� ������ �����ϴ�.");
				}

				strAck = strAck.Replace(strStx, string.Empty);

				string[] ack = strAck.Split(new string[] { strEtx }, StringSplitOptions.None);
				string[] strDetail;
				string strMsg = string.Empty;
				string strSeparator = "/";

				//String1 : ParperOut / pause flagȮ�� / bufferfull
				strDetail = ack[0].Split(new string[] { "," }, StringSplitOptions.None);

				
				//paperout
				if (strDetail[1] == "1") strMsg = Fnc.StringAdd(strMsg, "PaperOut", strSeparator);
				//pause
				//if (strDetail[2] == "1") strMsg = Fnc.StringAdd(strMsg, "Pause", strSeparator);
				//bufferfull
				if (strDetail[5] == "1") strMsg = Fnc.StringAdd(strMsg, "BufferFull", strSeparator);



				//String2 : HeadUp/RibbonOut
				strDetail = ack[1].Split(new string[] { "," }, StringSplitOptions.None);
				//HeadUp
				if (strDetail[2] == "1") strMsg = Fnc.StringAdd(strMsg, "HeadUp", strSeparator);
				//RibbonOut
				if (strDetail[3] == "1") strMsg = Fnc.StringAdd(strMsg, "RibbonOut", strSeparator);

				if (strMsg != string.Empty)
				{
					strMsg = "Barcode ������ ���� üũ ���� : " + strMsg;
					throw new Exception(strMsg);
				}

			}
			catch
			{
				throw;
			}
			finally
			{
				if (clsSn == null && isConnected) Close();
			}
		
			
		}


		string strAck = string.Empty;
		string strStx = Encoding.Default.GetString(new byte[] { 0x02 }); //stx
		string strEtx = Encoding.Default.GetString(new byte [] { 0x03, 0x0D, 0x0A });	//etx, cr, lf
		bool isStatusChecking = false;



		private void ReceiveAck(Function.Comm.Serial port, byte[] bytData)
		{
			if (!isStatusChecking) return;

			strAck += encoding.GetString(bytData);

			if (Fnc.inStr(strAck, strEtx) < 3)
				return;

			isStatusChecking = false;
		}


		private void ReceiveAck(byte[] bytData)
		{
			if (!isStatusChecking) return;

			strAck += encoding.GetString(bytData);
						
			if (Fnc.inStr(strAck, strEtx) < 3) 
				return;

			isStatusChecking = false;
		}






		public void Dispose()
		{
			this.Close();

			if (clsSn == null)
				clsTcp = null;
			else
				clsSn = null;

		}
	}
}
