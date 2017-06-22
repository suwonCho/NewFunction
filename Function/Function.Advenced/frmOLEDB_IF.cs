using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Function;
using Function.Db;
using Function.form;

namespace Function.Advenced
{
	/// <summary>
	/// oledb remote server form
	/// </summary>
	public partial class frmOLEDB_IF : Function.form.frmBaseForm
	{
		/// <summary>
		/// 리모트 서버 클래스
		/// </summary>
		clsRemoting clsRemote;


		/// <summary>
		/// 현재 설정된 포트 값을 가져옵니다.
		/// </summary>
		public int intPORT
		{
			get
			{
				return intPort;
			}

		}
		int intPort = 5051;

		/// <summary>
		/// I/F에 CONNECTION STRING을 설정 하거나 가져 옵니다.
		/// </summary>
		public string strConn
		{
			get { return clsOLEDB_IF.strConn; }
			set { clsOLEDB_IF.strConn = value; }
			

		}


		/// <summary>
		/// 설정된 URI 값을 가져 옵니다.
		/// </summary>
		public string strURI
		{
			get { return strURI; }
		}
		
		string strUri = "OLEDB_IF.SOAP";


		
		public frmOLEDB_IF()
		{
			InitializeComponent();
			InitForm();
		}

		bool isHttpProtocol = true;
		bool isSingleton = false;

		/// <summary>
		/// uri / 포트값을 설정하면서 객체를 생성 합니다.
		/// </summary>
		/// <param name="struri"></param>
		/// <param name="intport"></param>
		public frmOLEDB_IF(string struri, int intport)
		{
			InitializeComponent();
			strUri = struri;
			intPort = intport;
			InitForm();
		}

		public frmOLEDB_IF(string struri, int intport, bool IsHttpProtocol, bool IsSingleton)
		{
			InitializeComponent();
			strUri = struri;
			intPort = intport;

			isHttpProtocol = IsHttpProtocol;
			isSingleton = IsSingleton;

			InitForm();
		}



		/// <summary>
		/// 포 초기화 작업을 한다.
		/// </summary>
		private void InitForm()
		{
			try
			{
				strPrgName = "OLEDB I/F";
				clsLog = new Function.Util.Log(Application.StartupPath + "\\Log_OleDB_IF", "OleDB_IF", 30, true);

				clsRemote = new clsRemoting(intPort, strUri);
				
				control.Invoke_Control_Text(lblSvrInfo, "URI : " + clsRemote.strFullUri);
				control.Invoke_Control_Color(lblSvrInfo, Color.Yellow, null);

				clsRemote.Channel_SeverLoad(isHttpProtocol, typeof(Function.Advenced.clsOLEDB_IF), false, isSingleton);

				control.Invoke_Control_Text(lblSvrStatus, "서버\r\n작동중");
				control.Invoke_Control_Color(lblSvrStatus, null, Color.RoyalBlue);

				Function.Component.Spread.Invoke_RowCount(fpLog, fpLog.ActiveSheet, 0);


				clsOLEDB_IF.strConn = OleDB.Set_connectionString(OleDB.enProvider.MSSql, "admin\\sqlexpress",
				"namsun", "sa", "piss");

				clsOLEDB_IF.evtdsExcute = new clsOLEDB_IF.deldsExcute(evtdsExcute);
				clsOLEDB_IF.evtdsExcuteError = new clsOLEDB_IF.deldsExcuteError(evtdsExcuteError);


				SetMessage(false, "서버를 시작 하였습니다.", false);

			}
			catch (Exception ex)
			{
				ProcException(ex, "InitForm");
			}		

			

		}

		void frmOLEDB_IF_TextChanged(object sender, EventArgs e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		/// <summary>
		/// client가 쿼리 실행시 발생..
		/// </summary>
		/// <param name="strClientname"></param>
		/// <param name="strQuery"></param>
		public void evtdsExcute(string strClientname, string strQuery)
		{
			Function.Component.Spread.Invoke_AddRowData(fpLog, fpLog.ActiveSheet, 0, 500,
				new Object[] { DateTime.Now, strClientname, strQuery, string.Empty });

			clsLog.WLog(string.Format("CLIENT[{0}] QUERY실행 : \r\n{1}", strClientname, strQuery));
		}

		/// <summary>
		/// client 쿼리 실행 실패..
		/// </summary>
		/// <param name="strClientname"></param>
		/// <param name="strQuery"></param>
		/// <param name="ex"></param>
		public void evtdsExcuteError(string strClientname, string strQuery, Exception ex)
		{
			Function.Component.Spread.Invoke_AddRowData(fpLog, fpLog.ActiveSheet, 0, 500,
				new Object[] { DateTime.Now, strClientname, strQuery, "오류:" + ex.Message });

			Function.Component.Spread.Invoke_Row_SetColor(fpLog, fpLog.ActiveSheet, 0,
				null, Color.Red);

			clsLog.WLog(string.Format("CLIENT[{0}] QUERY실행 오류 : {1}\r\n{2}", strClientname, ex.ToString(), strQuery));
		}


		public override void Timer_1min(object obj)
		{
			Function.form.control.Invoke_Control_Text(lblFormTime,
				string.Format("시작: {0}\r\n종료: {1}", Fnc.Date2String(dtFormStart, Fnc.enDateType.DateTime), Fnc.Date2String(DateTime.Now, Fnc.enDateType.DateTime)));

		}


	}



	/// <summary>
	/// remote if용 oledb class
	/// </summary>
	public class clsOLEDB_IF : MarshalByRefObject
	{
		/// <summary>
		/// 연결 스트링
		/// </summary>
		public static string strConn =  string.Empty;

		/// <summary>
		/// remote if용 oledb class객체를 생성 한다.
		/// </summary>		
		public clsOLEDB_IF()
		{

		}



		public delegate void deldsExcute(string strClientname, string strQuery);
		public static deldsExcute evtdsExcute = null;

		public delegate void deldsExcuteError(string strClientname, string strQuery, Exception ex);
		public static deldsExcuteError evtdsExcuteError = null;


		/// <summary>
		/// 쿼리를 실행하고 리턴값을 넘긴다.
		/// </summary>
		/// <param name="strQuery"></param>
		/// <returns></returns>
		public DataSet dsExcute(string strClientName, string strQuery)
		{
			OleDB clsDb = new OleDB(strConn);

			try
			{
				DataSet ds = clsDb.dsExcute_Query(strQuery);

				if (evtdsExcute != null)
					evtdsExcute(strClientName, strQuery);

				return ds;

			}
			catch (Exception ex)
			{
				if (evtdsExcuteError != null)
					evtdsExcuteError(strClientName, strQuery, ex);

				throw;
			}


		}

	}




}