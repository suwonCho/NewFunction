using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
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
	public partial class frmOracle_IF : Function.form.frmBaseForm
	{
		/// <summary>
		/// ����Ʈ ���� Ŭ����
		/// </summary>
		clsRemoting clsRemote;

		/// <summary>
		/// ���� ������ ��Ʈ ���� �����ɴϴ�.
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
		/// I/F�� CONNECTION STRING�� ���� �ϰų� ���� �ɴϴ�.
		/// </summary>
		public OracleDB.strConnect strConn
		{
			get { return clsOracle_IF.strConn; }
			set { clsOracle_IF.strConn = value; }	

		}


		/// <summary>
		/// ������ URI ���� ���� �ɴϴ�.
		/// </summary>
		public string strURI
		{
			get { return strURI; }
		}
		
		string strUri = "Oracle_IF.SOAP";


		
		public frmOracle_IF()
		{
			InitializeComponent();
			InitForm();
		}

		/// <summary>
		/// uri / ��Ʈ���� �����ϸ鼭 ��ü�� ���� �մϴ�.
		/// </summary>
		/// <param name="struri"></param>
		/// <param name="intport"></param>
		public frmOracle_IF(string struri, int intport)
		{
			InitializeComponent();
			strUri = struri;
			intPort = intport;
			InitForm();
		}

		/// <summary>
		/// �� �ʱ�ȭ �۾��� �Ѵ�.
		/// </summary>
		private void InitForm()
		{
			try
			{
				strPrgName = "ORACLE I/F";
				clsLog = new Function.Util.Log(Application.StartupPath + "\\Log_Oracle_IF", "Oracle_IF", 30, true);

				clsRemote = new clsRemoting(intPort, strUri);
				
				control.Invoke_Control_Text(lblSvrInfo, "URI : " + clsRemote.strFullUri);
				control.Invoke_Control_Color(lblSvrInfo, Color.Yellow, null);

				clsRemote.HttpChannel_SeverLoad(typeof(Function.Advenced.clsOracle_IF), false, false);

				control.Invoke_Control_Text(lblSvrStatus, "Svr\r\nStarting");
				control.Invoke_Control_Color(lblSvrStatus, null, Color.RoyalBlue);

				//Function.Component.Spread.Invoke_RowCount(fpLog, fpLog.ActiveSheet, 0);


				//clsOracle_IF.strConn = OleDB.Set_connectionString(OleDB.enProvider.MSSql, "admin\\sqlexpress", "namsun", "sa", "piss");

				clsOracle_IF.isServer = true;
				clsOracle_IF.evtdsExcute = new clsOracle_IF.deldsExcute(evtdsExcute);
				clsOracle_IF.evtdsExcuteError = new clsOracle_IF.deldsExcuteError(evtdsExcuteError);


				SetMessage(false, "Server Started.", false);

			}
			catch (Exception ex)
			{
				ProcException(ex, "InitForm");
			}		

			

		}

		

		/// <summary>
		/// client�� ���� ����� �߻�..
		/// </summary>
		/// <param name="strClientname"></param>
		/// <param name="strQuery"></param>
		public void evtdsExcute(string strClientname, string strQuery)
		{
			try
			{
				//Function.Component.Spread.Invoke_AddRowData(fpLog, fpLog.ActiveSheet, 0, 500,
				//new Object[] { DateTime.Now, strClientname, strQuery, string.Empty });

				clsLog.WLog(string.Format("CLIENT[{0}] QUERY_Exe : \r\n{1}", strClientname, strQuery));
			}
			catch			{
				
			}
			
		}

		/// <summary>
		/// client ���� ���� ����..
		/// </summary>
		/// <param name="strClientname"></param>
		/// <param name="strQuery"></param>
		/// <param name="ex"></param>
		public void evtdsExcuteError(string strClientname, string strQuery, Exception ex)
		{
			//Function.Component.Spread.Invoke_AddRowData(fpLog, fpLog.ActiveSheet, 0, 500,
			//	new Object[] { DateTime.Now, strClientname, strQuery, "Error:" + ex.Message });

			//Function.Component.Spread.Invoke_Row_SetColor(fpLog, fpLog.ActiveSheet, 0,
			//	null, Color.Red);

			clsLog.WLog(string.Format("CLIENT[{0}] QUERYExe Error : {1}\r\n{2}", strClientname, ex.ToString(), strQuery));
		}


		//public override void Timer_1min(object obj)
		//{
		//	Function.form.control.Invoke_Control_Text(lblFormTime,
		//		string.Format("Start: {0}\r\nFinish: {1}", Fnc.Date2String(dtFormStart, Fnc.enDateType.DateTime), Fnc.Date2String(DateTime.Now, Fnc.enDateType.DateTime)));

		//}


	}



	/// <summary>
	/// remote if�� oledb class
	/// </summary>
	public class clsOracle_IF : MarshalByRefObject
	{

		/// <summary>
		/// �����ϰ�� ���� �߻� ���ϰ�..
		/// </summary>
		public static bool isServer = false;


		/// <summary>
		/// ���� ��Ʈ��
		/// </summary>
		public static OracleDB.strConnect strConn;

		/// <summary>
		/// remote if�� oledb class��ü�� ���� �Ѵ�.
		/// </summary>		
		public clsOracle_IF()
		{

		}


		public delegate void deldsExcute(string strClientname, string strQuery);
		public static deldsExcute evtdsExcute = null;

		public delegate void deldsExcuteError(string strClientname, string strQuery, Exception ex);
		public static deldsExcuteError evtdsExcuteError = null;


		/// <summary>
		/// ������ �����ϰ� ���ϰ��� �ѱ��.
		/// </summary>
		/// <param name="strQuery"></param>
		/// <returns></returns>
		public DataSet dsExcute(string strClientName, string strQuery)
		{
			OracleDB clsDb = new OracleDB(strConn);

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

				if(!isServer) 
					throw;
				else
					return null;
			}


		}


		[Serializable]
		public class OracleParam
		{
			public string ParameterName;
			public OracleDbType OracleDbType;
			public ParameterDirection Direction;
			public object Value;


			public OracleParam()
			{
				Direction = ParameterDirection.Input;
			}

			public OracleParam(string _ParameterName, OracleDbType _OracleDbType)
			{
				ParameterName = _ParameterName;
				OracleDbType = _OracleDbType;
				Direction = ParameterDirection.Input;
			}

		}


		public DataSet dsExcute_Procedure(string strClientName, string strProcedureName, OracleParam[] Ps)
		{
			OracleDB clsDb = new OracleDB(strConn);


			string strQuery = "ProcedureName : " + strProcedureName + "\r\nParams : ";

			OracleParameter[] Params = new OracleParameter[Ps.Length];
			
			int i = 0;

			foreach (OracleParam p in Ps)
			{
				strQuery += string.Format("[{0}]{1} ", p.ParameterName, p.Value);

				Params[i] = new OracleParameter(p.ParameterName, p.OracleDbType);
				if(p.Direction == ParameterDirection.Input || p.Direction == ParameterDirection.InputOutput)				
					Params[i].Value = p.Value;
				Params[i].Direction = p.Direction;
				
				i++;
			}

			

			try
			{
				DataSet ds = clsDb.dsExcute_StoredProcedure(strProcedureName, Params);


				if (evtdsExcute != null)
					evtdsExcute(strClientName, strQuery);

				return ds;

			}
			catch (Exception ex)
			{
				if (evtdsExcuteError != null)
					evtdsExcuteError(strClientName, strQuery, ex);

				if (!isServer)
					throw;
				else
					return null;
			}

		}


		public DataSet dsExcute_Procedure(string strClientName, string strProcedureName, OracleParameter[] Params)
		{
			OracleDB clsDb = new OracleDB(strConn);


			string strQuery = "ProcedureName : " + strProcedureName + "\r\nParams:";

			foreach (OracleParameter p in Params)
			{
				strQuery += string.Format("[Name]{0} - {1} ", p.ParameterName, p.Value);

				
			}

			

			try
			{

				DataSet ds = clsDb.dsExcute_StoredProcedure(strProcedureName, Params);


				if (evtdsExcute != null)
					evtdsExcute(strClientName, strQuery);

				return ds;

			}
			catch (Exception ex)
			{
				if (evtdsExcuteError != null)
					evtdsExcuteError(strClientName, strQuery, ex);

				if (!isServer)
					throw;
				else
					return null;
			}

		}


	}




}