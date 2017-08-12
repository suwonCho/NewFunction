using System;
using System.Data;
using System.Data.SqlClient;

namespace Function.Db
{
	
	/// <summary>
	/// Ms Sql Class
	/// </summary>
	public class MsSQL
	{

		#region MsSQL���� ���� ��ȸ ó�� ��

		public static DataTable DBListGet(strConnect conn)
		{
			string qry = "SELECT NAME FROM SYS.databases ORDER BY NAME";
			MsSQL sql = new MsSQL(conn);
			sql.retryTimes_Connecting = 1;
			sql.RetryTimes_Query = 1;

			using(DataSet ds = sql.Excute_Query(qry, ""))
			{
				return ds.Tables[0];
			}

		}


		public static DataTable TableListGet(strConnect conn)
		{
			string qry = "SELECT name FROM SYS.tables order by name";
			MsSQL sql = new MsSQL(conn);

			using (DataSet ds = sql.Excute_Query(qry, ""))
			{
				return ds.Tables[0];
			}

		}


		public static DataTable SPListGet(strConnect conn)
		{
			string qry = "SELECT name FROM SYS.procedures order by name";
			MsSQL sql = new MsSQL(conn);

			using (DataSet ds = sql.Excute_Query(qry, ""))
			{
				return ds.Tables[0];
			}

		}


		public static DataTable TableInfoGet(strConnect conn, string tablename)
		{
			string qry = string.Format(@"
SELECT OBJECT_ID, P.name, column_id, P.system_type_id, T.name datatype	 
FROM SYS.columns p
	LEFT OUTER JOIN SYS.types T ON P.system_type_id = T.system_type_id AND P.USER_type_id = T.USER_type_id
where object_id = 
	(SELECT object_id FROM SYS.tables WHERE name = '{0}')
ORDER BY column_id", tablename);

			MsSQL sql = new MsSQL(conn);

			using (DataSet ds = sql.Excute_Query(qry, ""))
			{
				return ds.Tables[0];
			}

		}


		public static DataTable SPInfoGet(strConnect conn, string spname)
		{

			string qry = string.Format(@"
SELECT OBJECT_ID, P.name, parameter_id, P.system_type_id, T.name datatype	
FROM SYS.all_parameters P
	LEFT OUTER JOIN SYS.types T ON P.system_type_id = T.system_type_id AND P.USER_type_id = T.USER_type_id
where object_id = 
	(SELECT object_id FROM SYS.procedures WHERE name = '{0}')
order by parameter_id", spname);

			MsSQL sql = new MsSQL(conn);

			using (DataSet ds = sql.Excute_Query(qry, ""))
			{
				return ds.Tables[0];
			}

		}

		#endregion


		private Function.Util.Log _log = null;

		public string strIP = string.Empty;
		public string strDataBase = string.Empty;
		public string strID = string.Empty;
		public string strPass = string.Empty;

		/// <summary>
		/// ID/IP�� ��� ������ �������� ����
		/// </summary>
		public struct strConnect
		{
			public string strIP;
			public string strDataBase;
			public string strID;
			public string strPass;		
		}


		public bool load_DBSetting(string xmlFilePath)
		{
			try
			{
				DataSet ds = new DataSet();
				ds.ReadXml(xmlFilePath);
				
				DataTable dt = ds.Tables["DB"];

				strIP = dt.Rows[0]["IP"].ToString();
				strDataBase = dt.Rows[0]["DataBase"].ToString();
				strID = dt.Rows[0]["ID"].ToString();
				strPass = dt.Rows[0]["pass"].ToString();

				return true;
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message, ex);
			}
		}


		/// <summary>
		/// DB Connection
		/// </summary>
		SqlConnection	Dbconn = new SqlConnection(); 
		
		/// <summary>
		/// DB Command
		/// </summary>
		SqlCommand		Dbcmd = new SqlCommand();

		/// <summary>
		/// sql ���ὺƮ��
		/// </summary>
		string connectionString = string.Empty;
		public string ConnectionString
		{
			get { return this.ConnectionString; }
		}

        /// <summary>
        /// Ʈ����� ����
        /// </summary>
        bool isTaransaction = false;

		
		/// <summary>
		/// DB���� ��õ� Ƚ�� (���� 1~5)
		/// </summary>
		int retryTimes_Connecting = 1;

		public int RetryTimes_Connecting
		{
			set
			{
				if(value > 5)
				{
					this.retryTimes_Connecting = 5;
					throw new Exception("DB���� ��õ� Ƚ���� ������ 1~5 �Դϴ�");
				}
				else if (value < 1)
				{
					this.retryTimes_Connecting = 1;
					throw new Exception("DB���� ��õ� Ƚ���� ������ 1~5 �Դϴ�");
				}
				else
				{
					this.retryTimes_Connecting = value;
				}
				
			}

			get { return this.retryTimes_Connecting; }
		}
		


		
		/// <summary>
		/// ���� ��õ� ȸ�� (���� 1-5)
		/// </summary>
		int retryTimes_Query = 1;
		public int RetryTimes_Query 
		{
			set
			{
				if(value > 5)
				{
					this.retryTimes_Query = 5;
					throw new Exception("���� ��õ� Ƚ���� ������ 1~5 �Դϴ�");
				}
				else if (value < 1)
				{
					this.retryTimes_Query = 1;
					throw new Exception("���� ��õ� Ƚ���� ������ 1~5 �Դϴ�");
				}
				else
				{
					this.retryTimes_Query = value;
				}
				
			}

			get { return this.retryTimes_Query; }
		}


		/// <summary>
		/// SQL DB ���� Class ����
		/// </summary>
		/// <param name="strServer">���� IP / �̸�</param>
		/// <param name="strDB">DataBase�̸�</param>
		/// <param name="strID">ID</param>
		/// <param name="strPassword">Password</param>
		public MsSQL(string strServer, string strDB, string strID, string strPassword, Function.Util.Log log = null)
		{
			_log = log;
			//id�� ��ȣ�� ���� �Ǿ� ���� ������ ������ ����
			if(strID.Equals(string.Empty) && strPassword.Equals(string.Empty))
				Set_ConnectionString_ByWindowsAut(strServer, strDB);
			else
				Set_ConnectionString(strServer, strDB, strID, strPassword);
		}

		public MsSQL(Function.Util.Log log = null)
		{
			_log = log;

			if (strIP != string.Empty)
				Set_ConnectionString(strIP, strDataBase, strID, strPass);
		}


		public MsSQL(strConnect str, Function.Util.Log log = null)
        {
			_log = log;

            Set_ConnectionString(str);
        }


		/// <summary>
		/// ���ὺƮ���� �����մϴ�.
		/// </summary>
        /// <param name="strServer">���� IP / �̸�</param>
        /// <param name="strDB">DataBase�̸�</param>
        /// <param name="strID">ID</param>
        /// <param name="strPassword">Password</param>
		public void Set_ConnectionString(string strServer, string strDB, string strID, string strPassword)
		{

			if (strID.Equals(string.Empty) && strPassword.Equals(string.Empty))
				Set_ConnectionString_ByWindowsAut(strServer, strDB);
			else
				this.connectionString = string.Format("Server={0};database={1};uid={2};pwd={3}", strServer, strDB, strID, strPassword);

			//this.connectionString = string.Format("Server={0};database={1};Integrated Security=SSPI", strServer, strDB);

		}

		public void Set_ConnectionString_ByWindowsAut(string strServer, string strDB)
		{
			this.connectionString = string.Format("Server={0};database={1};Integrated Security=SSPI", strServer, strDB);
		}


		public void Set_ConnectionString(strConnect strCon)
		{
			Set_ConnectionString(strCon.strIP, strCon.strDataBase, strCon.strID, strCon.strPass);
		}
		

		/// <summary>
		/// Db�� �����Ѵ�.
		/// </summary>
		/// <returns></returns>
		public bool DB_Open()
		{
			int Count = 1;
			while ( this.retryTimes_Connecting >= Count)
			{
				try
				{
					Count++;

					if (db_Open())
						return true;
				}
				catch(Exception ex)
				{
					if (Count == retryTimes_Connecting) throw ex;
				}

			}
			
			return false;

		}
		private bool db_Open()
		{
			try
			{
				if (this.connectionString == string.Empty)
					throw new Exception("���� ��Ʈ�� ������ �Ǿ� ���� �ʽ��ϴ�.");
				
				Dbconn.ConnectionString = this.connectionString;
				
				Dbconn.Open();
				
				return true;
			}
			catch(SqlException ex)
			{
				throw new Exception(ex.Message, ex);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// db�� �ݴ´�.
		/// </summary>
		/// <param name="strMsg"></param>
		/// <returns></returns>
		public bool DB_Close()
		{
			try
			{
				
				Dbconn.Close();
				
				return true;
			}
			catch(SqlException ex)
			{
                throw new Exception(ex.Message, ex);
			}
			catch(Exception ex)
			{
                throw ex;
			}
		}
		

		/// <summary>
		/// db�� ���� �����ΰ��� üũ
		/// </summary>
		/// <returns></returns>
		public bool DB_isOpen()
		{
			try
			{
				if (Dbconn.State  == System.Data.ConnectionState.Open)
					return true;
				else
					return this.db_Open();				
			}
			catch(SqlException ex)
			{
                throw new Exception(ex.Message, ex);
			}
			catch(Exception ex)
			{
                throw ex;
			}
		}

        /// <summary>
        /// Transaction�� ���� �Ѵ�.
        /// </summary>
        /// <returns></returns>
        public bool BeginTransaction()
        {
            try
            {
                if (!this.DB_isOpen())
                {
                    throw new Exception("DB ������ ������ �־� Taransaction�� ���� �� �� �����ϴ�.");
                }
                Dbcmd.Connection = Dbconn;
                Dbcmd.Transaction = Dbconn.BeginTransaction(IsolationLevel.ReadCommitted);

                isTaransaction = true;
                return true;
            }
            catch (SqlException ex)
            {
                throw new Exception(string.Format("BeginTransction Err(SQL) : {0}", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("BeginTransction Err(�Ϲ�) : {0}", ex.Message), ex);
            }
        }


        /// <summary>
        /// Tranasction�� ó��(commint) �Ѵ�.
        /// </summary>
        /// <returns></returns>
        public bool CommitTransaction()
        {
            try
            {
                if (Dbcmd.Transaction != null) Dbcmd.Transaction.Commit();
                Dbcmd.Transaction = null;
                isTaransaction = false;
                DB_Close();
                return true;
            }
            catch (SqlException ex)
            {
                throw new Exception(string.Format("CommitTransaction Err(SQL) : {0}", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("CommitTransaction Err(�Ϲ�) : {0}", ex.Message), ex);
            }
          
        }

        /// <summary>
        /// Tranasction�� ó��(rollback) �Ѵ�.
        /// </summary>
        /// <returns></returns>
        public bool RollBackTransaction()
        {
            try
            {
                if (Dbcmd.Transaction != null) Dbcmd.Transaction.Rollback();
                Dbcmd.Transaction = null;
                isTaransaction = false;
                this.DB_Close();
                return true;
            }
            catch (SqlException ex)
            {
                throw new Exception(string.Format("RollBackTransaction Err(SQL) : {0}", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("RollBackTransaction Err(�Ϲ�) : {0}", ex.Message), ex);
            }
        }

		/// <summary>
		/// ������ ����� DataSet�� �־� �ش�..
		/// </summary>
		/// <param name="Query"></param>
		/// <param name="strTable"></param>
		/// <returns></returns>
		public DataSet Excute_Query(string Query,string strTable, bool writelog = false)
		{			
			int Count = 0;
			while ( this.retryTimes_Query >= Count)
			{
				
				
				try
				{
                    Count++;
					return excute_Query(Query, strTable, writelog);
				}
				catch(Exception ex)
				{
					if (_log != null & writelog) _log.WLog_Exception(string.Format("SQLDB.Excute_Query[{0}��°", Count), ex);
					
					if (Count >= retryTimes_Query) throw new Exception(string.Empty, ex);
				}
                
			}
			
			return null;
		}
		private DataSet excute_Query(string Query, string strTable, bool writelog = false)
		{
            try
            {

                this.DB_isOpen();

                if (Query.Trim() == "")
                    throw new Exception("������ ������ �ֽ��ϴ�.(���� 0)");

                this.Dbcmd.Connection = this.Dbconn;
                Dbcmd.CommandType = CommandType.Text;
                Dbcmd.CommandText = Query;

				if (_log != null & writelog) _log.WLog("[SQLDB ���� ����]{0}", Query);

                DataSet ds = new DataSet();

                SqlDataAdapter da = new SqlDataAdapter(Dbcmd);

                if (strTable != null && strTable != string.Empty)
                    da.Fill(ds, strTable);
                else
                    da.Fill(ds);

                da.Dispose();

                return ds;
            }
            catch (SqlException ex)
            {
                throw new Exception(
                       string.Format("excute_Query Err(SQL) : {0} / ���� : {1}", ex.Message, Query), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("excute_Query Err(�Ϲ�) : {0} / ���� : {1}", ex.Message, Query), ex);
            }
            finally
            {
                if (!isTaransaction) DB_Close();
            }
		}

		/// <summary>
		/// ����� ���ν����� ���� �մϴ�..
		/// </summary>
		/// <param name="spName">���ν��� �̸�</param>
		/// <param name="Params">�Ķ���͵�</param>
		/// <param name="strTable">Table ��Ī �̸�..</param>
		/// <returns></returns>
		public DataSet Excute_StoredProcedure(string spName, SqlParameter[] Params, string strTable = "", bool writelog = false)
		{			
			int Count = 0;
			while ( this.retryTimes_Query >= Count)
			{				
				try
				{
                    Count++;
					return excute_StoredProcedure(spName, Params, strTable, writelog);
				}
				catch(Exception ex)
				{
					if (_log != null & writelog) _log.WLog_Exception(string.Format("SQLDB.Excute_StoredProcedure[{0}��°", Count), ex);

					if (Count == retryTimes_Query) throw new Exception(ex.Message, ex);
				}
                
			}

			return null;
		
		}
		private DataSet excute_StoredProcedure(string spName, SqlParameter[] Params, string strTable, bool writelog = false)
		{
			try
			{				
				
				this.DB_isOpen();

				if(spName.Trim() == "") 
						throw new Exception("���ν��� �̸��� ������ �ֽ��ϴ�(����:0).");

				string log = string.Empty;
			    Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.StoredProcedure;
				Dbcmd.CommandText = spName;

                if (Params != null)
                {

                    foreach (SqlParameter param in Params)
                    {
                        Dbcmd.Parameters.Add(param);
                        if (_log != null && writelog) log += string.Format("\r\n\t\t{0} = '{1}'", param.ParameterName, param.Value);
                    }
                }

				if (_log != null && writelog)
				{
					log = string.Format("[SQLDB ���ν��� ����]EXEC {0}{1};", spName, log);
					_log.WLog(log);
				}

				SqlDataAdapter da = new SqlDataAdapter(Dbcmd);
				
				DataSet ds = new DataSet();

				if (strTable != null && strTable != string.Empty)
					da.Fill(ds, strTable);
				else
					da.Fill(ds);

				da.Dispose();

				return ds;
				
			}
			catch(SqlException ex)
			{
				throw new Exception(
                    string.Format("excute_StoredProcedure Err(SQL) : {0} / ���ν��� : {1}", ex.Message, spName), ex);	
			}
			catch(Exception ex)
			{	
				throw new Exception(
                    string.Format("excute_StoredProcedure Err(�Ϲ�) : {0} / ���ν��� : {1}", ex.Message, spName), ex);	
			}
            finally
            {
                if (!isTaransaction) DB_Close();
            }

		}




		/// <summary>
		/// (���)sql db���� üũ�� �ð���ȸ
		/// </summary>
		/// <returns></returns>
		public static DateTime fnc_date_get(MsSQL.strConnect conn)
		{
			MsSQL sql = new MsSQL(conn);

			string qry = string.Format("SELECT GETDATE()");

			DataTable dt = sql.Excute_Query(qry, "").Tables[0];

			return (DateTime)dt.Rows[0][0];
		}

		/// <summary>
		/// (���)�⺻(master) ���� ��θ� ��ȸ �Ѵ�.
		/// </summary>
		/// <param name="conn"></param>
		/// <returns></returns>
		public static string fnc_filepath_get(MsSQL.strConnect conn)
		{
			MsSQL sql = new MsSQL(conn);

			string qry = @"select filename
from [master].[dbo].sysfiles
where fileid = 1";

			DataTable dt = sql.Excute_Query(qry, "").Tables[0];

			string[] path = Function.system.clsFile.FileSplitPath_Name(Fnc.obj2String(dt.Rows[0]["filename"]));

			return path[0];
		}


		/// <summary>
		/// (���)DataBase ���� ���θ� Ȯ���Ѵ�.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="dbName"></param>
		/// <returns></returns>
		public static bool fnc_DataBase_Exists(MsSQL.strConnect conn, string dbName)
		{
			MsSQL sql = new MsSQL(conn);

			string qry = $@"SELECT *
FROM model.[sys].[databases]
WHERE 1=1 
AND NAME = '{dbName}'";

			DataTable dt = sql.Excute_Query(qry, "").Tables[0];

			return dt.Rows.Count > 0;
		}

		/// <summary>
		///  (���)DataBase ���� �Ѵ�.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="dbName"></param>
		public static void fnc_DataBase_drop(MsSQL.strConnect conn, string dbName)
		{
			MsSQL sql = new MsSQL(conn);

			string qry = $@"DROP database {dbName}";

			sql.Excute_Query(qry, "");

		}

        /// <summary>
        /// (���)����� ���� ����� ��ȸ �Ѵ�.
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static DataTable fnc_LinkedServers(MsSQL.strConnect conn)
        {
            MsSQL sql = new MsSQL(conn);

            string qry = "sp_linkedservers";

            return sql.Excute_StoredProcedure(qry, new SqlParameter[] { }, string.Empty, false).Tables[0];
        }

        /// <summary>
        /// (���)����� ������ �׽�Ʈ �Ѵ�. ���� �� exception �߻�
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="linkedServerName"></param>
        public static void fnc_LinkedServer_Test(MsSQL.strConnect conn, string linkedServerName)
        {
            MsSQL sql = new MsSQL(conn);

            string qry = $"exec sp_testlinkedserver {linkedServerName}";

            sql.Excute_Query(qry, "", false);
        }

        /// <summary>
        /// (���)����� ���� �̸����� ���翩�θ� Ȯ���Ѵ�.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="linkedServerName"></param>
        /// <returns></returns>
        public static bool fnc_LinkedServer_Exist(MsSQL.strConnect conn, string linkedServerName)
        {
            DataTable dt = fnc_LinkedServers(conn);

            DataRow[] rows = dt.Select($"SRV_NAME = '{linkedServerName}'");

            return rows.Length > 0;

        }

        /// <summary>
        /// (���)����� ������ ���� �Ѵ�.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="linkedServerName"></param>
        public static void fnc_LinkedServer_Drop(MsSQL.strConnect conn, string linkedServerName)
        {
            MsSQL sql = new MsSQL(conn);

            string qry = string.Format("IF  EXISTS (SELECT srv.name FROM sys.servers srv WHERE srv.server_id != 0 AND srv.name = N'{0}')EXEC master.dbo.sp_dropserver @server=N'{0}', @droplogins='droplogins'",
                linkedServerName);

            sql.Excute_Query(qry, "", false);
        }

        /// <summary>
        /// (���)���ν����� ���� �Ѵ�.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="SPName"></param>
        public static void fnc_Procedure_Drop(MsSQL.strConnect conn, string SPName)
        {
            MsSQL sql = new MsSQL(conn);

            string qry = string.Format(@"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{0}') AND type in (N'P', N'PC'))
DROP PROCEDURE {0}",  SPName);

            sql.Excute_Query(qry, "", false);
        }

    }
}
