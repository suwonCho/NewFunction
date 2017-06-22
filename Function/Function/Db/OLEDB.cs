using System;
using System.Data;
using System.Data.OleDb;

namespace Function.Db
{
	

	/// <summary>
	/// OldDB Control Class
	/// </summary>
	public class OleDB
	{

		/// <summary>
		/// OleDb ���� Db Type
		/// </summary>
		public enum enProvider
		{
			/// <summary>
			/// MS Sql
			/// </summary>
			MSSql, 
			/// <summary>
			/// ����Ŭ DB
			/// </summary>
			Oracle, 
			/// <summary>
			/// MS Access
			/// </summary>
			MDB, 
			/// <summary>
			/// MS Excel
			/// </summary>
			Excel, 
			/// <summary>
			/// ,���е� TextFile <para/>
			/// ip:�������, ��ȸ�Ҷ� select * from ���ϸ�
			/// </summary>
			CSV, 
			/// <summary>
			/// Sybase DataBase : Sybase IQ 15 ��ġ �� ���
			/// </summary>
			Sybase,
            /// <summary>
            /// DB2
            /// </summary>
            db2,
			/// <summary>
			/// MS Access DB(NEW)
			/// </summary>
			ACCDB

		};

		/// <summary>
		/// DB Connection
		/// </summary>
		OleDbConnection	Dbconn = new OleDbConnection(); 
		/// <summary>
		/// DB Command
		/// </summary>
		OleDbCommand Dbcmd = new OleDbCommand();
		
		string connectionString = string.Empty;
		/// <summary>
		/// sql ���ὺƮ��
		/// </summary>
		public string ConnectionString
		{
			get { return this.connectionString; }
		}

		string ip_address = string.Empty;
		string db = string.Empty;
		string id = string.Empty;
		string pass = string.Empty;


		int retryTimes_Connecting = 1;
		/// <summary>
		/// DB���� ��õ� Ƚ�� (���� 1~5)
		/// </summary>
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
		/// ��ü�� ���� �ϸ鼭 Connection String �����.
		/// </summary>
		/// <param name="strIP"></param>
		/// <param name="strDB"></param>
		/// <param name="strID"></param>
		/// <param name="strPassword"></param>
		public OleDB(enProvider enPro, string strIP, string strDB, string strID, string strPassword)
		{
			Set_ConnectionString(enPro, strIP, strDB, strID, strPassword);
		}

		/// <summary>
		/// ���ؼ� ������ �޴´�..
		/// </summary>
		/// <param name="strConnectionString"></param>
		public OleDB(string strConnectionString)
		{
			connectionString = strConnectionString;
		}

		
		public OleDB()
		{			
		}

		
		public bool load_DBSetting(string xmlFilePath)
		{
			try
			{
				DataSet ds = new DataSet();
				ds.ReadXml(xmlFilePath);
				
				DataTable dt = ds.Tables["DB"];

				enProvider enPro;

				switch(dt.Rows[0]["DBType"].ToString().ToLower())
				{
					case "mssql":
						enPro = enProvider.MSSql;
						break;
					case "oracle":
						enPro = enProvider.Oracle;
						break;
					case "excel":
						enPro = enProvider.Excel;
						break;
					case "mdb":
						enPro = enProvider.MDB;
						break;
					case "csv":
						enPro = enProvider.CSV;
						break;
                    case "db2":
                        enPro = enProvider.db2;
                        break;
					default:
						return false;
				}
				

				this.Set_ConnectionString(enPro, dt.Rows[0]["IP"].ToString(), dt.Rows[0]["DataBase"].ToString(), 
								dt.Rows[0]["ID"].ToString(), dt.Rows[0]["pass"].ToString());


				return true;
			}
			catch
			{
				return false;
			}
		}
		
		/// <summary>
		/// ���ὺƮ���� �����մϴ�.
		/// </summary>
		/// <param name="enPro">DB����</param>
		/// <param name="strServer"></param>
		/// <param name="strDB"></param>
		/// <param name="strID"></param>
		/// <param name="strPassword"></param>
		public void Set_ConnectionString(enProvider enPro, string strIP, string strDB, string strID, string strPass)
		{
			switch (enPro)
			{
				case enProvider.MSSql:
                    this.connectionString = string.Format("Provider=SQLOLEDB;Data Source={0};initial catalog={1};User Id={2};Password={3}",strIP, strDB, strID, strPass);
					break;
				case enProvider.Oracle:
                    this.connectionString = string.Format("Provider=MsdaOra;Data Source={0};User Id={2};Password={3}", strIP, strDB, strID, strPass);
					break;
				case enProvider.MDB:
					this.connectionString = string.Format("Provider=Micorosoft.jet.OLEDB.4.0;Data Source={0}",strIP, strDB, strID, strPass);
					break;
				case enProvider.Excel:
					this.connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='{0}';Extended Properties=Excel 8.0",strIP, strDB, strID, strPass);
                    //this.connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Extended Properties=Excel 12.0;Imex=1", strIP);

                    //this.connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\" ", strIP);

					break;
				case enProvider.CSV:
					this.connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='{0}';Extended Properties='text;HDR=Yes;FMT=Delimited;'",strIP, strDB, strID, strPass);
					break;

				case enProvider.Sybase:
					this.connectionString = string.Format("Provider=ASEOLEDB;Data Source={0};Catalog={1};User Id={2};Password={3};",strIP, strDB, strID, strPass);
					//string.Format("Provider=ASEOLEDB;Server Name={0};Server Port Address={1};Initial Catalog=DIRECTORY;User ID={2};Password={3}", strIP, strDB, strID, strPass);
					break;
                case enProvider.db2:
                    this.connectionString = string.Format("Provider=IBMDA400;Data Source={0};User id={2};password={3};Default Collection={1};Catalog Library List=*LIBL;", strIP, strDB, strID, strPass);
                    break;

				case enProvider.ACCDB:
					this.connectionString = string.Format("provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}", strIP, strDB, strID, strPass);
					break;
			}

		}


		/// <summary>
		/// ���ὺƮ���� �����մϴ�.
		/// </summary>
		/// <param name="enPro">DB����</param>
		/// <param name="strServer"></param>
		/// <param name="strDB"></param>
		/// <param name="strID"></param>
		/// <param name="strPassword"></param>
		public static string Set_connectionString(enProvider enPro, string strIP, string strDB, string strID, string strPass)
		{
			string strConn = string.Empty;
			switch (enPro)
			{
				case enProvider.MSSql:
					strConn = string.Format("Provider=SQLOLEDB;Data Source={0};initial catalog={1};User Id={2};Password={3}", strIP, strDB, strID, strPass);
					break;
				case enProvider.Oracle:
                    strConn = string.Format("Provider=MsdaOra;Data Source={0};User Id={2};Password={3}", strIP, strDB, strID, strPass);
					break;
				case enProvider.MDB:
					strConn = string.Format("Provider=Micorosoft.jet.OLEDB.4.0;Data Source={0}", strIP, strDB, strID, strPass);
					break;
				case enProvider.Excel:
					//strConn = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\" ", strIP);
					strConn = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='{0}';Extended Properties=Excel 8.0", strIP, strDB, strID, strPass);
					break;
				case enProvider.CSV:
					strConn = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='text;HDR=Yes;FMT=Delimited", strIP, strDB, strID, strPass);
					break;
				case enProvider.Sybase:
					strConn = string.Format("Provider=ASEOLEDB;Data Source={0};Catalog={1};User Id={2};Password={3};", strIP, strDB, strID, strPass);
					//string.Format("Provider=ASEOLEDB;Server Name={0};Server Port Address={1};Initial Catalog=DIRECTORY;User ID={2};Password={3}", strIP, strDB, strID, strPass);
					break;
                case enProvider.db2:
                    strConn = string.Format("Provider=IBMDA400;Data Source={0};User id={2};password={3};Default Collection={1};Catalog Library List=*LIBL;", strIP, strDB, strID, strPass);
                    //string.Format("Provider=ASEOLEDB;Server Name={0};Server Port Address={1};Initial Catalog=DIRECTORY;User ID={2};Password={3}", strIP, strDB, strID, strPass);
                    break;
			}

			return strConn;

		}
		

		/// <summary>
		/// Db�� �����Ѵ�.
		/// </summary>
		/// <returns></returns>
		public bool DB_Open()
		{
			int Count = 0;

            while (true)
            {
                try
                {
                    Count++;

                    return db_Open();
                }
                catch 
                {
                    if (this.retryTimes_Connecting >= Count) throw;
                }
            }
			

		}

		private bool db_Open()
		{
			try
			{
				if (this.connectionString == string.Empty)
				{	
					throw new Exception ("���� ��Ʈ�� ������ �Ǿ� ���� �ʽ��ϴ�.");
				}

				Dbconn.ConnectionString = this.connectionString;
				
				Dbconn.Open();			

				return true;
			}
			catch( OleDbException ex)
			{
				throw new Exception(string.Format("DB Open Err(OLEDB) : {0}",ex.Message), ex);				
			}
			catch(Exception ex)
			{
                throw new Exception(string.Format("DB Open Err(�Ϲ�) : {0}", ex.Message), ex);				
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
				
                if (Dbconn != null && Dbconn.State == System.Data.ConnectionState.Open)
				    Dbconn.Close();

				return true;
			}
            catch (OleDbException ex)
            {
                throw new Exception(string.Format("DB Close Err(OLEDB) : {0}", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("DB Close Err(�Ϲ�) : {0}", ex.Message), ex);
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
				{					
					return this.db_Open();
				}
			}
			catch
			{
                throw;
			}
		}

		bool isTaransaction = false;

		/// <summary>
		/// Transaction�� ���� �Ѵ�.
		/// </summary>
		/// <returns></returns>
		public bool BeginTransaction()
		{
			try
			{
				DB_isOpen();

				Dbcmd.Connection = Dbconn;
				Dbconn.BeginTransaction(IsolationLevel.ReadCommitted);

				isTaransaction = true;
				return true;
			}
			catch (OleDbException ex)
			{
				throw new Exception(string.Format("DB Close Err(Oracle) : {0}", ex.Message), ex);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(�Ϲ�) : {0}", ex.Message), ex);
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

				isTaransaction = false;
				return true;
			}
			catch (OleDbException ex)
			{
				throw new Exception(string.Format("DB Close Err(Oracle) : {0}", ex.Message), ex);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(�Ϲ�) : {0}", ex.Message), ex);
			}
			finally
			{
				this.DB_Close();
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
				isTaransaction = false;
				return true;
			}
			catch (OleDbException ex)
			{
				throw new Exception(string.Format("DB Close Err(Oracle) : {0}", ex.Message), ex);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(�Ϲ�) : {0}", ex.Message), ex);
			}
			finally
			{
				if (!this.isTaransaction) this.DB_Close();
			}
		}





		
		/// <summary>
		/// ������ ����� DataTable�� �־� �ش�..
		/// </summary>
		/// <param name="Query"></param>		
		/// <returns></returns>
		public DataSet dsExcute_Query(string Query)
		{			
			int Count = 0;

            while (true)
            {
                try
                {
                    Count++;
                    return dsexcute_Query(Query);
                }
                catch
                {
                    if (this.retryTimes_Query >= Count) throw;
                }
            }			
			
		}
		private DataSet dsexcute_Query(string Query)
		{
            try
            {
                this.DB_isOpen();

                if (Query.Trim() == "")
                    throw new Exception("������ ������ �ֽ��ϴ�.(���� 0)");

                Dbcmd.Parameters.Clear();
                this.Dbcmd.Connection = this.Dbconn;
                Dbcmd.CommandType = CommandType.Text;
                Dbcmd.CommandText = Query;


                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter(Dbcmd);

				

                da.Fill(ds);

                da.Dispose();

                return ds;
            }
            catch (OleDbException ex)
            {
                throw new Exception(string.Format("dsExcute_Query(OLEDB) : {0}", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("dsExcute_Query(�Ϲ�) : {0}", ex.Message), ex);
            }
            finally
            {
                DB_Close();
            }
		}


		/// <summary>
		/// ������ �����ϰ� ���� ���� �� ������ �����Ѵ�.
		/// </summary>
		/// <param name="Query"></param>
		/// <returns></returns>
		public int intExcute_Query(string Query)
		{			
			int Count = 0;
			while(true)
			{
                try
                {
                    Count++;
                    return intexcute_Query(Query);
                }
                catch
                {
                    if (this.retryTimes_Query >= Count) throw;
                }
				
			}				
			
		}
		private int intexcute_Query(string Query)
		{
            try
            {

                this.DB_isOpen();

                if (Query.Trim() == "")
                    throw new Exception("������ ������ �ֽ��ϴ�.(���� 0)");

                Dbcmd.Parameters.Clear();
                this.Dbcmd.Connection = this.Dbconn;
                Dbcmd.CommandType = CommandType.Text;
                Dbcmd.CommandText = Query;

                int intRst = Dbcmd.ExecuteNonQuery();

                return intRst;
            }
            catch (OleDbException ex)
            {
                throw new Exception(string.Format("intExcute_Query(OLEDB) : {0}", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("intExcute_Query(�Ϲ�) : {0}", ex.Message), ex);
            }
            finally
            {
				if (!this.isTaransaction) DB_Close();
            }
		}

		/// <summary>
        /// ������ �����ϰ� ���� ���� �� ������ �����Ѵ�.
		/// </summary>
		/// <param name="Query"></param>
		/// <param name="Params"></param>		
		/// <returns></returns>
        public int intExcute_Query(string Query, OleDbParameter[] Params)
		{			
			int Count = 0;
			while (true)
			{
                try
                {
                    Count++;
                    return intexcute_Query(Query, Params);
                }
                catch
                {
                    if (this.retryTimes_Query >= Count) throw;
                }
			}
				
			
		}
		private int intexcute_Query(string Query, OleDbParameter[] Params)
		{
			try
			{

                this.DB_isOpen();

				if(Query.Trim() == "") 
				    throw new Exception("������ ������ �ֽ��ϴ�.(���� 0)");
					
			    Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.Text;
				Dbcmd.CommandText = Query;
				
				foreach(OleDbParameter param in Params)
				{
					Dbcmd.Parameters.Add(param);
				}
				
				return Dbcmd.ExecuteNonQuery();
				
			}
            catch (OleDbException ex)
            {
                throw new Exception(string.Format("intExcute_Query(OLEDB) : {0}", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("intExcute_Query(�Ϲ�) : {0}", ex.Message), ex);
            }
            finally
            {
				if (!this.isTaransaction) DB_Close();
            }
		}

		
		/// <summary>
		/// ����� ���ν����� ���� �ϰ� ������� �࿡ ������ ����
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="Params"></param>		
		/// <returns></returns>
		public int intExcute_StoredProcedure(string spName,OleDbParameter[] Params)
		{
            int Count = 0;
            
			while (true)
			{
                try
                {
                    Count++;
                    return intexcute_StoredProcedure(spName, Params);
                }
                catch
                {
                    if (this.retryTimes_Query >= Count) throw;
                }
			}
				
			
		}
        private int intexcute_StoredProcedure(string spName, OleDbParameter[] Params)
		{
			try
			{				
				
				this.DB_isOpen();
				

				if(spName.Trim() == "") 
				    throw new Exception("���ν��� �̸��� �����Դϴ�.");
				
				
			    Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.StoredProcedure;
				Dbcmd.CommandText = spName;

				foreach(OleDbParameter param in Params)
				{
					Dbcmd.Parameters.Add(param);
				}

				return Dbcmd.ExecuteNonQuery();

			}
            catch (OleDbException ex)
            {
                throw new Exception(string.Format("intExcute_StoredProcedure(OLEDB) : {0}", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("intExcute_StoredProcedure(�Ϲ�) : {0}", ex.Message), ex);
            }
            finally
            {
				if (!this.isTaransaction) DB_Close();
            }
		}

		/// <summary>
		/// ����� ���ν����� ���� �ϰ� ����� DataSet���� �����Ѵ�.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="Params"></param>		
		/// <returns></returns>
		public DataSet dsExcute_StoredProcedure(string spName, OleDbParameter[] Params)
		{
            int Count = 0;

            while (true)
            {
                try
                {
                    Count++;
                    return dsexcute_StoredProcedure(spName, Params);
                }
                catch
                {
                    if (this.retryTimes_Query >= Count) throw;
                }
            }
			
		}
		private DataSet dsexcute_StoredProcedure(string spName, OleDbParameter[] Params)
		{
			try
			{				
				
				this.DB_isOpen();
				

				if(spName.Trim() == "") 
					throw new Exception("���ν��� �̸��� ���� �Դϴ�.");
				
				
			    Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.StoredProcedure;
				Dbcmd.CommandText = spName;

				foreach(OleDbParameter param in Params)
				{
					Dbcmd.Parameters.Add(param);
				}

				OleDbDataAdapter da = new OleDbDataAdapter(Dbcmd);
                DataSet ds = new DataSet();

                da.Fill(ds);
				da.Dispose();

				

				return ds;
			}
            catch (OleDbException ex)
            {
                throw new Exception(string.Format("dsExcute_StoredProcedure(OLEDB) : {0}", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("dsexcute_StoredProcedure(�Ϲ�) : {0}", ex.Message), ex);
            }
            finally
            {
				if (!this.isTaransaction) DB_Close();
            }
		}


	}
}
