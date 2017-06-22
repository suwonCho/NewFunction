using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO.Ports;
using Function;

namespace Function.Device
{
	public delegate void delReceiveStrData(object sender, string data);

	public class fnc
	{
		/// <summary>
		/// plc설정파일의 테이블 이름.
		/// </summary>
		public static string sPLCSetTable = "PLCSettring";

		internal static readonly string _defalt_rs232_Setting_fileName = "RS232_SET.XML";
		internal static readonly string _defalt_rs232_Setting_devName = "default";

		/// <summary>
		/// Setting 파일을 로딩한다. 없으면 생성 하여 준다.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="plcName"></param>
		/// <returns></returns>
		public static DataSet InitSettingFile(string fileName, string plcName)
		{
			DataSet ds = new DataSet();

			//파일명이 존재 하면 파일을 로드하고 
			if (Function.system.clsFile.FileExists(fileName))
			{
				ds.ReadXml(fileName, XmlReadMode.ReadSchema);
			}

			DataTable dt;
			if(ds.Tables.Contains(sPLCSetTable))
			{
				dt = ds.Tables[sPLCSetTable];
				DataRow[] dr = dt.Select(string.Format("plcname = '{0}'", plcName));
				if (dr.Length > 0) return ds;
			}
			else
			{
				dt = new DataTable(sPLCSetTable);
				dt.Columns.Add("PLCNAME", Type.GetType("System.String"));
				dt.Columns.Add("COMPORT", Type.GetType("System.Int32"));
				dt.Columns.Add("BAUDRATE", Type.GetType("System.String"));
				dt.Columns.Add("DATABIT", Type.GetType("System.String"));
				dt.Columns.Add("STOPBIT", Type.GetType("System.String"));
				dt.Columns.Add("PARITY", Type.GetType("System.String"));
				dt.Columns.Add("FLOW", Type.GetType("System.String"));

				ds.Tables.Add(dt);

			}

			DataRow d = dt.NewRow();

			d["PLCNAME"] = plcName;
			d["COMPORT"] = 1;
			d["BAUDRATE"] = BaudRate.b38400.ToString();
			d["DATABIT"] = DataBits.Bit8.ToString();
			d["STOPBIT"] = StopBits.One.ToString();
			d["PARITY"] = Parity.None.ToString();
			d["FLOW"] = Handshake.None.ToString();

			dt.Rows.Add(d);

			return ds;

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="ds"></param>
		public static void SaveSettingFile(string fileName, string DevName, DataSet dTg)
		{
			//파일명이 존재 하지 안으면 새로 파일을 생성하고..
			if (!Function.system.clsFile.FileExists(fileName))
			{
				dTg.WriteXml(fileName, XmlWriteMode.WriteSchema);
				return;
			}

			DataSet dSc = new DataSet();

			//파일을 읽어서..
			dSc.ReadXml(fileName, XmlReadMode.ReadSchema);

			//저장할 대상을 찾는다.
			DataRow drTg = dTg.Tables[sPLCSetTable].Select(string.Format("plcname = '{0}'", DevName))[0];

			//원본에서 해당 dev가 존재 하는지 확인.
			DataRow[] drSc = dSc.Tables[sPLCSetTable].Select(string.Format("plcname = '{0}'", DevName));

			DataRow drS;
			//원본에 있으면 update 없으면 insert
			if (drSc.Length > 0)
				drS = drSc[0];
			else
				drS = dSc.Tables[sPLCSetTable].NewRow();


			foreach (DataColumn dc in dSc.Tables[sPLCSetTable].Columns)
			{
				drS[dc.ColumnName] = drTg[dc.ColumnName];
			}

			if (drSc.Length <= 0)
				dSc.Tables[sPLCSetTable].Rows.Add(drS);


			dSc.WriteXml(fileName, XmlWriteMode.WriteSchema);
		}


		/// <summary>
		/// Setting 파일을 로딩한여 PLC설정 정보를 넘긴다.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="plcName"></param>
		/// <returns></returns>
		public static DataRow GetSetting(string fileName, string plcName)
		{
			DataSet ds = new DataSet();

			if (fileName == string.Empty) fileName = fnc._defalt_rs232_Setting_fileName;
			if (plcName == string.Empty) plcName = fnc._defalt_rs232_Setting_devName;

			//파일명이 존재 하면 파일을 로드하고 
			if (!Function.system.clsFile.FileExists(fileName)) return null;
			
			ds.ReadXml(fileName, XmlReadMode.ReadSchema);
			
			DataTable dt;
			if (!ds.Tables.Contains(sPLCSetTable)) return null;
			
			dt = ds.Tables[sPLCSetTable];
			DataRow[] dr = dt.Select(string.Format("plcname = '{0}'", plcName));
			if (dr.Length < 1) return null;

			return dr[0];

		}

		/// <summary>
		/// 시리얼 포트를 연결 설정을 하여 준다.
		/// </summary>
		/// <param name="serial">객체 생성한 후 넘길것</param>
		/// <param name="d">설정 datarow</param>
		public static void SerialPort_Init(Function.Comm.Serial serial, DataRow d)
		{
			serial.PortName = string.Format("COM{0}", d["COMPORT"]);
			serial.BaudRate = (BaudRate)Fnc.String2Enum(new BaudRate(), d["BAUDRATE"].ToString());
			serial.DataBits = (DataBits)Fnc.String2Enum(new DataBits(), d["DATABIT"].ToString());
			serial.StopBits = (StopBits)Fnc.String2Enum(new StopBits(), d["STOPBIT"].ToString());
			serial.Parity = (Parity)Fnc.String2Enum(new Parity(), d["PARITY"].ToString());
			serial.Handshake = (Handshake)Fnc.String2Enum(new Handshake(), d["FLOW"].ToString());			
		}


	}
}
