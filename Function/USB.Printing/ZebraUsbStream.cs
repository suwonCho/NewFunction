
//-------------------------------
// ZebraUsbStream.cs
//-------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using Function;

namespace USB.Printing
{
    /// 
    /// Stream subclass which incorporates low-level USB access to Zebra printers
    /// 
    public class ZebraUsbStream : Stream, IDisposable
    {
        UsbPrinterConnector usb;

        XmlDocument xmlFormat;

		bool _chk_status;

        /// <summary>
        /// 테스트용 스트링..
        /// </summary>
        public readonly string strTestString = @"+RIB
+C 4
F
B 512 600 4 0 2 4 100 1 A12345
T 512 75 4 0 0 35 1 ZEBRA CARD PRINTER TEST
T 200 200 0 1 0 50 1 (c)csw
T 200 300 0 1 0 50 1 
T 200 400 0 1 0 50 1 2011.03.17
T 65 320 7 1 0 50 0 CARD PRINTING TEST
L 15 80 970 4 1
I";
		Thread thRead;

		private string _printer_name = string.Empty;
		private bool _use_window_print = false;
		private string csv_folder;
        /// <summary>
        /// 프린터 포트명으로 프린터 연결..
        /// </summary>
        /// <param name="port"></param>
        public ZebraUsbStream(string port)
        {
            usb = new UsbPrinterConnector(port);
            usb.IsConnected = true;
            //base.ReadTimeout = usb.ReadTimeout;
            //base.WriteTimeout = usb.WriteTimeout;      

			//start_Read_Monitor();
        }

        /// <summary>
        /// 자동으로 포트를 찾아 프린터를 연결
        /// </summary>
        public ZebraUsbStream()
        {
            System.Collections.Specialized.NameValueCollection devs =
            UsbPrinterConnector.EnumDevices(true, true, false);

            if (devs.Count < 1)
                throw new Exception("No Zebra printers found");

            usb = new UsbPrinterConnector(devs[0].ToString());
            usb.IsConnected = true;

			//start_Read_Monitor();
        }
		
		/// <summary>
		/// 윈도우 인쇄 모드 이용
		/// </summary>
		/// <param name="str"></param>
		public ZebraUsbStream(string printer_name, bool chk_status, string app_folder)
		{
			_printer_name = printer_name;
			_use_window_print = true;
			_chk_status = chk_status;
			csv_folder = app_folder + "\\window_prt_log\\";

			string msg = string.Empty;
			window_print_chk_status("Create", ref msg);
		}

		private void start_Read_Monitor()
		{
			thRead = new Thread(new ThreadStart(thread_Read_Monitor));
			thRead.Name = "Read_Monitor";
			thRead.IsBackground = true;
			thRead.Start();
		}

		private void thread_Read_Monitor()
		{
			byte[] bRst = new byte[1024];

			int iRead;
			while (true)
			{
				try
				{
					iRead =  Read(bRst, 0, 1024);

					Console.WriteLine(bRst);

				}
				catch
				{
				}
			}

		}




		/// <summary>
		/// 사용 않함....
		/// </summary>
		/// <param name="notuse"></param>
		public ZebraUsbStream(bool notuse)
		{			
		}

        /// <summary>
        /// Format xml 파일을 로드한다.
        /// </summary>
        /// <param name="strFilePath"></param>
        public void XmlFormat_Load(string strFilePath)
        {
            xmlFormat = new XmlDocument();
            xmlFormat.Load(strFilePath);
        }

		bool isPrintNodata = false;


		/// <summary>
		/// data없을 시 NoData인쇄 여부..
		/// </summary>
		public bool isPrintNoData
		{
			get
			{
				return isPrintNodata;
			}
			set
			{
				isPrintNodata = value;
			}
		}

		public void XmlFormat_Print(string strFormatName, string strData)
		{
			if (_use_window_print)
				window_print(strFormatName, strData);
			else
				xmlFormat_print(strFormatName, strData);
		}

        private void xmlFormat_print(string strFormatName, string strData)
        {
			xmlFormat.Load("CARD_FORMAT.XML");
            XmlNode xn = xmlFormat.SelectSingleNode(@"CARD_FORMAT/" + strFormatName);

			//Write(strTestString);

            foreach (XmlNode x in xn.ChildNodes)
            {
                string str = string.Empty;
                switch (x.Name.Substring(0,1))
                {
                    case "F":
                    case "H":
                    //헤더 풋더 처리
                        str = x.SelectSingleNode("CMD").InnerText;
                        break;
                    case "T":
                        //텍스트 처리..
                        str = string.Format("{0} {1}", x.SelectSingleNode("CMD").InnerText, x.SelectSingleNode("DATA").InnerText);
                        break;
                    case "D":
                        //데이터 처리..
						
                        int intStartIndex = int.Parse(x.SelectSingleNode("STARTINDEX").InnerText);
                        int intLength = int.Parse(x.SelectSingleNode("LENGTH").InnerText);
						string NoData = x.SelectSingleNode("NODATA").InnerText.Trim();

						str = Substring(strData, intStartIndex, intLength).Trim();

						if (NoData != string.Empty && str == string.Empty)
						{
							for (int i = 0; i < intLength; i++)
							{
								str += isPrintNodata ? NoData.Substring(0, 1) : " ";
							}
						}

						if (str == string.Empty)
							str = string.Empty;
						else
							str = string.Format("{0} {1}", x.SelectSingleNode("CMD").InnerText, str);
                        break;
                }

				if(str != string.Empty)  Write(str);

			

            }


        }

		string _win_formatName;
		string _strData;
		private void window_print(string strFormatName, string strData)
		{
			string err_msg = string.Empty;

			if (!window_print_chk_status("print", ref err_msg) && _chk_status)
			{
				throw new Exception(string.Format("[{0}]{1}", _printer_name, err_msg));
			}

			_win_formatName = strFormatName;
			_strData = strData;

			PrintDocument prnDoc = new PrintDocument();

			PaperSize psize = new PaperSize("infocard", 212, 335);			
			
			//prnDoc.DefaultPageSettings.PaperSize.Kind = PaperKind.Custom;
			//prnDoc.DefaultPageSettings.PaperSize.Height = 335;
			//prnDoc.DefaultPageSettings.PaperSize.Width = 212;
			prnDoc.DefaultPageSettings.PaperSize = psize;
			prnDoc.DefaultPageSettings.Landscape = true;

			prnDoc.PrintPage += new PrintPageEventHandler(prnDoc_PrintPage);

			prnDoc.PrinterSettings.PrinterName = _printer_name;

			prnDoc.Print();
		}

		public bool window_print_chk_status(string header, ref string err_msg)
		{
			DataTable dt = Function.Util.WMI.InQuery("root\\CIMV2", "Win32_Printer",
				string.Format("NAME = '{0}'", _printer_name));

			

			if (dt.Rows.Count < 1)
			{
				err_msg = "프린터에 상태 정보를 확인 할 수 없습니다.(wmi no_row return)";
				return false;
			}

			print_status_log_write(header, dt);

			DataRow dr = dt.Rows[0];

			if (Fnc.obj2String(dr["WorkOffline"]).ToUpper().Equals("TRUE"))
			{
				err_msg = "프린터가 Offline입니다. 연결 상태를 확인하여 주십시요.(offline:True)";
				return false;
			}

			if (!Fnc.obj2String(dr["PrinterState"]).ToUpper().Equals("0"))
			{
				err_msg = string.Format("프린터가 상태가 정상이 아님니다. 상태를 화인하여 주십시요.(PrinterState:{0})"
					, dr["PrinterState"]);
				return false;
			}
			
			return true;
		}

		private void print_status_log_write(string header, DataTable dt)
		{
			string filename = string.Format("window_prt_log_{0}.csv", DateTime.Now.ToString("yyyyMM"));
			

			if (!Function.system.clsFile.FileExists(csv_folder + filename))
			{
				Function.system.clsFile.FileCopy("prt_status.csv", csv_folder + filename);
			}

			DataRow dr = dt.Rows[0];

			Function.Db.OleDB db = new Function.Db.OleDB(Function.Db.OleDB.enProvider.CSV, csv_folder, string.Empty, string.Empty, string.Empty);

			string q = string.Format("select top 1 * from [{0}] ", filename);
			//string v = string.Empty;
			
			using (DataSet ds = db.dsExcute_Query(q))
			{
				/*db 방식으로 처리 하려 하였으나 길이문제(예상)으로 인해 쿼리 오류..
				q = string.Format("insert into [{0}]  ( ", filename);
				q = q + "CreateDate, Header";
				v = string.Format(" Values ( '{0}', '{1}'", Fnc.Date2String(DateTime.Now, Fnc.enDateType.DateTime), header);

				foreach (DataColumn c in ds.Tables[0].Columns)
				{
					if (c.ColumnName.ToUpper().Equals("CREATEDATE") || c.ColumnName.ToUpper().Equals("HEADER")) continue;
					

					try
					{
						if (Fnc.obj2String(dr[c.ColumnName]) == string.Empty) continue;

						q = q + string.Format(", {0}", c.ColumnName);
						v = v + string.Format(", '{0}'", dr[c.ColumnName]);
					}
					catch
					{
						
					}
				}

				q = q + " ) ";
				v = v + " ) ";

				db.intExcute_Query(q + v);
				 */
				string str = string.Empty;
				using (FileStream fs = new FileStream(csv_folder + filename, FileMode.Open, FileAccess.Write))
				{
					fs.Position = fs.Length;

					using(StreamWriter sw = new StreamWriter(fs))
					{
						
						
						foreach (DataColumn c in ds.Tables[0].Columns)
						{
							if (c.ColumnName.ToUpper().Equals("CREATEDATE"))
							{
								str = csv_string_get( Fnc.Date2String(DateTime.Now, Fnc.enDateType.DateTime));
								continue;
							}

							if (c.ColumnName.ToUpper().Equals("HEADER"))
							{
								str = str + "," + csv_string_get(header);
								continue;
							}


							try
							{
								str = str + string.Format(", {0}", csv_string_get(Fnc.obj2String(dr[c.ColumnName])));
							}
							catch
							{
								str = str + ",";
							}
						}

						//str = str + "\\r\\n";

						sw.WriteLine(str);
						sw.Close();

					}

					fs.Close();
				}

				GC.Collect();

			}
		}

		private string csv_string_get(string s)
		{
			if(s.IndexOf(',') > 0) return string.Format(@"""{0}""", s);

			return s;
		}


		void prnDoc_PrintPage(object sender, PrintPageEventArgs e)
		{

			xmlFormat.Load("CARD_FORMAT.XML");
			XmlNode xn = xmlFormat.SelectSingleNode(@"CARD_FORMAT/" + _win_formatName);
			
			float left, top, width, height, mleft, mtop;
			Font dFont = Fnc.String2Font("Arial,9,Regular");
			Font iFont;
			string tmp;

			mleft = 0;
			mtop = 0;

			Graphics grfx = e.Graphics;

			grfx.PageUnit = GraphicsUnit.Millimeter;
				
			foreach (XmlNode x in xn.ChildNodes)
			{
				string str = string.Empty;
					
				switch (x.Name.Substring(0, 1))
				{	
					case "S":
						mleft = float.Parse(x.SelectSingleNode("LEFT").InnerText);
						mtop = float.Parse(x.SelectSingleNode("TOP").InnerText);
						dFont = Fnc.String2Font(x.SelectSingleNode("FONT").InnerText);
						break;

					case "T":
						//텍스트 처리..
						str = string.Format("{0}",x.SelectSingleNode("DATA").InnerText);							
						break;

					case "D":
						//데이터 처리..

						int intStartIndex = int.Parse(x.SelectSingleNode("STARTINDEX").InnerText);
						int intLength = int.Parse(x.SelectSingleNode("LENGTH").InnerText);
						string NoData = x.SelectSingleNode("NODATA").InnerText.Trim();

						str = Substring(_strData, intStartIndex, intLength).Trim();

						if (NoData != string.Empty && str == string.Empty)
						{
							for (int i = 0; i < intLength; i++)
							{
								str += isPrintNodata ? NoData.Substring(0, 1) : " ";
							}
						}

						if (str == string.Empty)
							str = string.Empty;
						else
							str = string.Format("{0}", str);
						break;
				}

				if (str == string.Empty) continue;

				left = float.Parse(x.SelectSingleNode("LEFT").InnerText) + mleft;
				top = float.Parse(x.SelectSingleNode("TOP").InnerText) + mtop;
				width = float.Parse(x.SelectSingleNode("WIDTH").InnerText);
				height = float.Parse(x.SelectSingleNode("HEIGHT").InnerText);

				if (x.SelectSingleNode("FONT").InnerText.Trim().Equals(string.Empty))
					iFont = dFont;
				else
					iFont = Fnc.String2Font(x.SelectSingleNode("FONT").InnerText);

				Function.Imaging.DrawText(grfx, str, Color.Black, iFont, left, top, width, height, StringAlignment.Near, StringAlignment.Near);
					

			}
			
		}


		private string Substring(string data, int Start_Index, int Length)
		{
			int len = data.Length;

			if(len >= (Start_Index + Length - 1))
				return data.Substring(Start_Index, Length);
			else if(len >= Start_Index)
				return data.Substring(Start_Index, len - Start_Index + 1);
			else
				return string.Empty;
			 
		}



        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanTimeout
        {
            get { return true; }
        }

        public override void Flush()
        {
            ;
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return usb.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 프린터 포트에 BYTE배열 명령을 날린다.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if(usb != null) usb.Send(buffer, offset, count);
        }

        /// <summary>
        /// 프린터 포트에 string 명령을 날린다.
        /// </summary>
        /// <param name="strData"></param>
        public void Write(string strData)
        {
            string[] s = strData.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string ss in s)
            {
                string s1 = " " + ss + " ";
                byte[] byt = Encoding.Default.GetBytes(s1);
                byt[0] = 0x1B;
                byt[byt.Length - 1] = 0x0d;

                Write(byt, 0, byt.Length);
            }
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if(usb != null)usb.IsConnected = false;
        }

        public override void Close()
        {
            base.Close();
			if (usb != null && usb.IsConnected)
                usb.IsConnected = false;
        }

        public override int ReadTimeout
        {
            get
            {
                return usb.ReadTimeout;
            }
            set
            {
                usb.ReadTimeout = value;
            }
        }

        public override int WriteTimeout
        {
            get
            {
                return usb.WriteTimeout;
            }
            set
            {
                usb.WriteTimeout = value;
            }
        }




    }
}