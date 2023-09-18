
//-------------------------------
// ZebraUsbStream.cs
//-------------------------------
using System;
using System.Text;
using System.IO;
using System.Threading;
using System.Data;
using System.Runtime.InteropServices;

namespace function.Device.ZPL
{
    /// 
    /// Stream subclass which incorporates low-level USB access to Zebra printers
    /// 
    public class ZPL : Stream, IDisposable
    {
        UsbPrinterConnector usb;

		bool _chk_status;

		Thread thRead;

		private string _printer_name = string.Empty;
		private bool _use_window_print = false;
		private string csv_folder;
        /// <summary>
        /// 프린터 포트명으로 프린터 연결..
        /// </summary>
        /// <param name="port"></param>
        public ZPL(string port)
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
        public ZPL()
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
		public ZPL(string printer_name, bool chk_status, string app_folder)
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
		public ZPL(bool notuse)
		{			
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
		
		string _win_formatName;
		string _strData;
		

		public bool window_print_chk_status(string header, ref string err_msg)
		{
			DataTable dt = function.Util.WMI.InQuery("root\\CIMV2", "Win32_Printer",
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
			

			if (!function.system.clsFile.FileExists(csv_folder + filename))
			{
				function.system.clsFile.FileCopy("prt_status.csv", csv_folder + filename);
			}

			DataRow dr = dt.Rows[0];

			function.Db.OleDB db = new function.Db.OleDB(function.Db.OleDB.enProvider.CSV, csv_folder, string.Empty, string.Empty, string.Empty);

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

				GC.Collect();

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
        public void WriteString(string strData)
        {
            string[] s = strData.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string ss in s)
            {
				if (ss.Trim().Length < 1) continue;
                
                byte[] byt = Encoding.Default.GetBytes(ss);
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