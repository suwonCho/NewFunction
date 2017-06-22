using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Function;
using Function.Db;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Data.SqlClient;

namespace Function.uScm
{
	class clsDBFunc
	{
		/// <summary>
		/// 차종명으로 차종을 조회 한다.
		/// </summary>
		/// <param name="clsDB"></param>
		/// <param name="strOrderDate"></param>
		/// <returns></returns>
		public static DataSet Get_Cartype(OracleDB.strConnect strConn, string strWord)
		{
			OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

            
			OracleParameter[] param = new OracleParameter[] { 
							                    new OracleParameter("ps_Word", OracleDbType.Varchar2, 100),
												new OracleParameter("pi_RowCount", OracleDbType.Int32, 8),
                                                new OracleParameter("T_CURSOR", OracleDbType.RefCursor)      };


			param[2].Direction = ParameterDirection.Output;

			param[0].Value = strWord;
			param[1].Value = 100;

			return clsDB.dsExcute_StoredProcedure("SP_ORDER_CARTYPE_GET", param);

		}


		/// <summary>
		/// 차종별 사용 컬러를 조회 한다.
		/// </summary>
		/// <param name="clsDB"></param>
		/// <param name="strCar"></param>
		/// <returns></returns>
		public static DataSet Get_CartypeColor(OracleDB.strConnect strConn, string strCar)
		{
			OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

			OracleParameter[] param = new OracleParameter[] { 
							                    new OracleParameter("ps_Car", OracleDbType.Varchar2, 15),												
                                                new OracleParameter("T_CURSOR", OracleDbType.RefCursor)      };


			param[1].Direction = ParameterDirection.Output;

			param[0].Value = strCar;


			return clsDB.dsExcute_StoredProcedure("SP_ORDER_COLOR_GET", param);

		}


		/// <summary>
		/// PartCode목록을 조회한다.
		/// </summary>
		/// <param name="clsDB"></param>
		/// <returns></returns>
		public static DataSet Get_PartCode(OracleDB.strConnect strConn, string strCode)
		{
			OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

			OracleParameter[] param = new OracleParameter[] { 
												new OracleParameter("ps_Code", OracleDbType.Varchar2)   ,
                                                new OracleParameter("T_CURSOR", OracleDbType.RefCursor)   
												};

			param[0].Value = strCode;
			param[1].Direction = ParameterDirection.Output;

			return clsDB.dsExcute_StoredProcedure("SP_GET_CODEDETAILLIST", param);

		}


		public static bool Insert_Order(OracleDB.strConnect strConn, System.Windows.Forms.ListView lv, string strOrderType, Util.Log clsLog, string strStationID, string strUser)
		{
			OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

			OracleParameter[] param = new OracleParameter[] { 
                                                new OracleParameter("ps_ORDERDATE", OracleDbType.Varchar2, 8),
												new OracleParameter("PS_ORDERTYPE", OracleDbType.Varchar2, 8),
												new OracleParameter("PS_STATIONID", OracleDbType.Varchar2, 10),
												new OracleParameter("PS_CARTYPE", OracleDbType.Varchar2, 15),
                                                new OracleParameter("PS_PARTCODE", OracleDbType.Varchar2, 15),
												new OracleParameter("PS_COLOR", OracleDbType.Varchar2, 10),
                                                new OracleParameter("PI_TARGET_CNT", OracleDbType.Int32, 8),
												new OracleParameter("ps_USER", OracleDbType.Varchar2, 50),
                                                new OracleParameter("PS_ALARM", OracleDbType.Varchar2, 2),
                                                new OracleParameter("PS_ALARMMSG", OracleDbType.Varchar2, 500) };



			param[8].Direction = ParameterDirection.Output;
			param[9].Direction = ParameterDirection.Output;


			string strLog = string.Empty;

			try
			{
				clsDB.BeginTransaction();

				strLog = "ORDER를저장을 시작 합니다.\r\n";

				foreach (System.Windows.Forms.ListViewItem li in lv.Items)
				{
					string strField = string.Empty;

					param[0].Value = li.SubItems[1].Text.ToString();
					param[1].Value = strOrderType;
					param[2].Value = strStationID;
					param[3].Value = li.SubItems[3].Text.ToString();
					if (strOrderType == "I")
					{	//사출 오더
						param[4].Value = li.SubItems[5].Text.ToString();	//Partcode
						param[5].Value = DBNull.Value;						//color
						strField = "PARTCODE";
					}
					else
					{	//도장 오더
						param[4].Value = DBNull.Value;						//Partcode
						param[5].Value = li.SubItems[5].Text.ToString(); 	//color
						strField = "COLOR";
					}

					int intV = int.Parse(li.SubItems[6].Text.ToString());

					param[6].Value = intV;
					param[7].Value = strUser;

					strLog += string.Format("\t\t ORDERDATE [{0}] ORDERTYPE[{4}] STATIONID[{6}] CARTYPE [{1}] {5} [{2}] TARGET_CNT [{3}]"
									, param[0].Value, param[1].Value, param[3].Value, param[3].Value, strOrderType, strField, strStationID);

					clsDB.intExcute_StoredProcedure("SP_ORDER_INSERT", param);



					if (param[8].Value.ToString() != "00")
					{
						strLog += "\t==>저장실패 :" + param[9].Value.ToString() + "\r\n======>Rollback처리를 합니다.";
						clsDB.RollBackTransaction();
						throw new Exception(param[9].Value.ToString());
					}

					strLog += "\t==>저장성공\r\n";
				}

				strLog += "======>Commit처리를 합니다.";
				clsDB.CommitTransaction();

				return true;
			}
			catch
			{
				throw;
			}
			finally
			{
				clsLog.WLog(strLog);
			}
		}



		public static bool Insert_Order_Paint(OracleDB.strConnect strConn, System.Windows.Forms.ListView lv, Util.Log clsLog, string strStationID, string strUser)
		{
			OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

			OracleParameter[] param = new OracleParameter[] { 
                                                new OracleParameter("ps_ORDERDATE", OracleDbType.Varchar2, 8),				//0										
												new OracleParameter("PS_STATIONID", OracleDbType.Varchar2, 10),			//1
												new OracleParameter("PS_CARTYPE_FRT", OracleDbType.Varchar2, 15),			//2
												new OracleParameter("PS_CARTYPE_RR", OracleDbType.Varchar2, 15),			//3
                                                new OracleParameter("PS_PARTCODE", OracleDbType.Varchar2, 10),				//4
												new OracleParameter("PS_COLOR", OracleDbType.Varchar2, 10),				//5
                                                new OracleParameter("PI_TARGET_CNT", OracleDbType.Int32, 8),				//6
												new OracleParameter("ps_USER", OracleDbType.Varchar2, 50),					//7
                                                new OracleParameter("PS_ALARM", OracleDbType.Varchar2, 2),					//8
                                                new OracleParameter("PS_ALARMMSG", OracleDbType.Varchar2, 500) };			//9



			param[8].Direction = ParameterDirection.Output;
			param[9].Direction = ParameterDirection.Output;


			string strLog = string.Empty;

			try
			{
				clsDB.BeginTransaction();

				strLog = "도장 투입 ORDER를저장을 시작 합니다.\r\n";

				foreach (System.Windows.Forms.ListViewItem li in lv.Items)
				{
					string strField = string.Empty;

					param[0].Value = li.SubItems[1].Text.ToString();					
					param[1].Value = strStationID;
					param[2].Value = li.SubItems[3].Text.ToString();
					param[3].Value = li.SubItems[5].Text.ToString();
					
					param[4].Value = DBNull.Value;						//Partcode
					param[5].Value = li.SubItems[7].Text.ToString(); 	//color
					
					int intV = int.Parse(li.SubItems[8].Text.ToString());

					param[6].Value = intV;
					param[7].Value = strUser;

					strLog += string.Format("\t\t ORDERDATE [{0}] STATIONID[{1}] FRT CARTYPE [{2}] RR CARTYPE [{3}] COLOR [{4}] TARGET_CNT [{5}]"
									, param[0].Value, param[1].Value, li.SubItems[2].Text, li.SubItems[4].Text, li.SubItems[6], intV);

					clsDB.intExcute_StoredProcedure("SP_ORDER_INSERT_PAINT", param);
					
					if (param[8].Value.ToString() != "00")
					{
						strLog += "\t==>저장실패 :" + param[9].Value.ToString() + "\r\n======>Rollback처리를 합니다.";
						clsDB.RollBackTransaction();
						throw new Exception(param[9].Value.ToString());
					}

					strLog += "\t==>저장성공\r\n";
				}

				strLog += "======>Commit처리를 합니다.";
				clsDB.CommitTransaction();

				return true;
			}
			catch
			{
				throw;
			}
			finally
			{
				clsLog.WLog(strLog);
			}
		}
	

	}
}
