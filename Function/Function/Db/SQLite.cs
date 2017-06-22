using System;
using System.Data;
using System.Data.SQLite;


namespace Function.Db
{


	/// <summary>
	/// SQLite Control Class
	/// 사용시 [오류]'런타임에 로드할 수 없습니다.'발생시 참조하는 프로젝트 app.config의 [태그]'startup'을 'startup useLegacyV2RuntimeActivationPolicy="true"' 로 변경
	/// </summary>
	public class SQLite
	{

		/// <summary>
		/// DB Connection
		/// </summary>
		SQLiteConnection Dbconn = new SQLiteConnection();
		/// <summary>
		/// DB Command
		/// </summary>
		SQLiteCommand Dbcmd = new SQLiteCommand();

		string connectionString = string.Empty;
		/// <summary>
		/// sql 연결스트링
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
		/// DB연결 재시도 횟수 (범위 1~5)
		/// </summary>
		public int RetryTimes_Connecting
		{
			set
			{
				if (value > 5)
				{
					this.retryTimes_Connecting = 5;
					throw new Exception("DB연결 재시도 횟수의 범위는 1~5 입니다");
				}
				else if (value < 1)
				{
					this.retryTimes_Connecting = 1;
					throw new Exception("DB연결 재시도 횟수의 범위는 1~5 입니다");
				}
				else
				{
					this.retryTimes_Connecting = value;
				}

			}

			get { return this.retryTimes_Connecting; }
		}




		/// <summary>
		/// 쿼리 재시도 회수 (범위 1-5)
		/// </summary>
		int retryTimes_Query = 1;
		public int RetryTimes_Query
		{
			set
			{
				if (value > 5)
				{
					this.retryTimes_Query = 5;
					throw new Exception("쿼리 재시도 횟수의 범위는 1~5 입니다");
				}
				else if (value < 1)
				{
					this.retryTimes_Query = 1;
					throw new Exception("쿼리 재시도 횟수의 범위는 1~5 입니다");
				}
				else
				{
					this.retryTimes_Query = value;
				}

			}

			get { return this.retryTimes_Query; }
		}

		/// <summary>
		/// SQLite 클래스를 생성합니다.
		/// </summary>
		public SQLite()
		{
		}

		/// <summary>
		/// SQLite 클래스를 생성합니다.
		/// </summary>
		/// <param name="dbfile">DB File 경로</param>
		public SQLite(string dbfile)
		{
			Set_ConnectionString(dbfile);
		}


		/// <summary>
		/// 연결스트링을 설정합니다.
		/// </summary>
		/// <param name="dbfile">DB File 경로</param>
		public void Set_ConnectionString(string dbfile)
		{
			connectionString = string.Format("Data Source={0}", dbfile);
		}


		/// <summary>
		/// Db를 연결한다.
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
					throw new Exception("연결 스트링 설정이 되어 있지 않습니다.");
				}

				Dbconn.ConnectionString = this.connectionString;

				Dbconn.Open();

				return true;
			}
			catch (SQLiteException ex)
			{
				throw new Exception(string.Format("DB Open Err(SQLite) : {0}", ex.Message), ex);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(일반) : {0}", ex.Message), ex);
			}
		}

		/// <summary>
		/// db를 닫는다.
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
			catch (SQLiteException ex)
			{
				throw new Exception(string.Format("DB Close Err(SQLite) : {0}", ex.Message), ex);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("DB Close Err(일반) : {0}", ex.Message), ex);
			}
		}


		/// <summary>
		/// db가 연결 상태인가를 체크
		/// </summary>
		/// <returns></returns>
		public bool DB_isOpen()
		{
			try
			{
				if (Dbconn.State == System.Data.ConnectionState.Open)
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
		/// Transaction을 시작 한다.
		/// </summary>
		/// <returns></returns>
		public bool BeginTransaction()
		{
			try
			{
				DB_isOpen();

				Dbcmd.Connection = Dbconn;
				Dbcmd.Transaction = Dbconn.BeginTransaction(IsolationLevel.ReadCommitted);

				isTaransaction = true;
				return true;
			}
			catch (SQLiteException ex)
			{
				throw new Exception(string.Format("DB Close Err(Oracle) : {0}", ex.Message), ex);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(일반) : {0}", ex.Message), ex);
			}
		}


		/// <summary>
		/// Tranasction을 처리(commint) 한다.
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
			catch (SQLiteException ex)
			{
				throw new Exception(string.Format("DB Close Err(Oracle) : {0}", ex.Message), ex);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(일반) : {0}", ex.Message), ex);
			}
			finally
			{
				this.DB_Close();
			}
		}


		/// <summary>
		/// Tranasction을 처리(rollback) 한다.
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
			catch (SQLiteException ex)
			{
				throw new Exception(string.Format("DB Close Err(Oracle) : {0}", ex.Message), ex);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(일반) : {0}", ex.Message), ex);
			}
			finally
			{
				if (!this.isTaransaction) this.DB_Close();
			}
		}






		/// <summary>
		/// 쿼리한 결과를 DataTable에 넣어 준다..
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
					throw new Exception("쿼리에 문제가 있습니다.(길이 0)");

				Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.Text;
				Dbcmd.CommandText = Query;


				DataSet ds = new DataSet();
				SQLiteDataAdapter da = new SQLiteDataAdapter(Dbcmd);

				da.Fill(ds);

				da.Dispose();

				return ds;
			}
			catch (SQLiteException ex)
			{
				throw new Exception(string.Format("dsExcute_Query(SQLite) : {0}", ex.Message), ex);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("dsExcute_Query(일반) : {0}", ex.Message), ex);
			}
			finally
			{
				DB_Close();
			}
		}




		/// <summary>
		/// 쿼리를 실행한다. @로 시작하는 파라메터 명을 사용한다.
		/// </summary>
		/// <param name="Query"></param>
		/// <param name="Params"></param>
		/// <param name="writelog"></param>
		/// <returns></returns>
		public DataSet dsExcute_Query(string Query, SQLiteParameter[] Params)
		{
			int Count = 0;
			while (this.retryTimes_Query >= Count)
			{
				try
				{
					Count++;

					return dsexcute_Query(Query, Params);

				}
				catch (Exception ex)
				{
					if (Count == this.RetryTimes_Query) throw ex;					
				}

			}

			return null;

		}

		

		private DataSet dsexcute_Query(string Query, SQLiteParameter[] Params)
		{
			try
			{

				DB_isOpen();

				string log = string.Empty;

				if (Query.Trim() == "")
				{
					throw new Exception("쿼리에 문제가 있습니다.(길이 0)");
				}

				Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.Text;
				Dbcmd.CommandText = Query;



				foreach (SQLiteParameter param in Params)
				{
					Dbcmd.Parameters.Add(param);					
				}

				DataSet ds = new DataSet();

				SQLiteDataAdapter da = new SQLiteDataAdapter(Dbcmd);

				da.Fill(ds);
				da.Dispose();

				return ds;
			}
			catch (SQLiteException ex)
			{
				throw new Exception(string.Format("dsExcute_Query(SQLite) : {0}", ex.Message), ex);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("dsExcute_Query(일반) : {0}", ex.Message), ex);
			}
			finally
			{
				DB_Close();
			}
		}




		/// <summary>
		/// 쿼리를 실행하고 영향 받은 행 개수를 리턴한다.
		/// </summary>
		/// <param name="Query"></param>
		/// <returns></returns>
		public int intExcute_Query(string Query)
		{
			int Count = 0;
			while (true)
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
					throw new Exception("쿼리에 문제가 있습니다.(길이 0)");

				Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.Text;
				Dbcmd.CommandText = Query;

				int intRst = Dbcmd.ExecuteNonQuery();

				return intRst;
			}
			catch (SQLiteException ex)
			{
				throw new Exception(string.Format("intExcute_Query(SQLite) : {0}", ex.Message), ex);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("intExcute_Query(일반) : {0}", ex.Message), ex);
			}
			finally
			{
				if (!this.isTaransaction) DB_Close();
			}
		}

		/// <summary>
		/// 쿼리를 실행하고 영향 받은 행 개수를 리턴한다.
		/// </summary>
		/// <param name="Query"></param>
		/// <param name="Params"></param>		
		/// <returns></returns>
		public int intExcute_Query(string Query, SQLiteParameter[] Params)
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
		private int intexcute_Query(string Query, SQLiteParameter[] Params)
		{
			try
			{

				this.DB_isOpen();

				if (Query.Trim() == "")
					throw new Exception("쿼리에 문제가 있습니다.(길이 0)");

				Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.Text;
				Dbcmd.CommandText = Query;

				foreach (SQLiteParameter param in Params)
				{
					Dbcmd.Parameters.Add(param);
				}

				return Dbcmd.ExecuteNonQuery();

			}
			catch (SQLiteException ex)
			{
				throw new Exception(string.Format("intExcute_Query(SQLite) : {0}", ex.Message), ex);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("intExcute_Query(일반) : {0}", ex.Message), ex);
			}
			finally
			{
				if (!this.isTaransaction) DB_Close();
			}
		}


		/// <summary>
		/// 스토어 프로시져를 실행 하고 영향받은 행에 갯수를 리턴
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="Params"></param>		
		/// <returns></returns>
		public int intExcute_StoredProcedure(string spName, SQLiteParameter[] Params)
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
		private int intexcute_StoredProcedure(string spName, SQLiteParameter[] Params)
		{
			try
			{

				this.DB_isOpen();


				if (spName.Trim() == "")
					throw new Exception("프로시져 이름이 공백입니다.");


				Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.StoredProcedure;
				Dbcmd.CommandText = spName;

				foreach (SQLiteParameter param in Params)
				{
					Dbcmd.Parameters.Add(param);
				}

				return Dbcmd.ExecuteNonQuery();

			}
			catch (SQLiteException ex)
			{
				throw new Exception(string.Format("intExcute_StoredProcedure(SQLite) : {0}", ex.Message), ex);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("intExcute_StoredProcedure(일반) : {0}", ex.Message), ex);
			}
			finally
			{
				if (!this.isTaransaction) DB_Close();
			}
		}

		/// <summary>
		/// 스토어 프로시져를 실행 하고 결과를 DataSet으로 리턴한다.
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="Params"></param>		
		/// <returns></returns>
		public DataSet dsExcute_StoredProcedure(string spName, SQLiteParameter[] Params)
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
		private DataSet dsexcute_StoredProcedure(string spName, SQLiteParameter[] Params)
		{
			try
			{

				this.DB_isOpen();


				if (spName.Trim() == "")
					throw new Exception("프로시져 이름이 공백 입니다.");


				Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.StoredProcedure;
				Dbcmd.CommandText = spName;

				foreach (SQLiteParameter param in Params)
				{
					Dbcmd.Parameters.Add(param);
				}

				SQLiteDataAdapter da = new SQLiteDataAdapter(Dbcmd);
				DataSet ds = new DataSet();

				da.Fill(ds);
				da.Dispose();



				return ds;
			}
			catch (SQLiteException ex)
			{
				throw new Exception(string.Format("dsExcute_StoredProcedure(SQLite) : {0}", ex.Message), ex);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("dsexcute_StoredProcedure(일반) : {0}", ex.Message), ex);
			}
			finally
			{
				if (!this.isTaransaction) DB_Close();
			}
		}


	}
}
