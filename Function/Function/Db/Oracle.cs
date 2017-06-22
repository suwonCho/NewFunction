using System;
using System.Data;
using System.Threading;
using System.IO;

namespace Function.Db
{
	/*
	/// <summary>
	/// Oracle Class : Framework 지원
	/// </summary>
	public class Oracle
	{
        public struct strConnect
        {
            public string strTNS;
            public string strID;
            public string strPass;
        }

		/// <summary>
		/// string empty를 db null로 반환 한다.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static object StringEmpty2DbNull(string str)
		{
			if (str == string.Empty)
			{
				return DBNull.Value;
			}
			else
			{
				return str;
			}
		}


		/// <summary>
		/// DB Connection
		/// </summary>
		public OracleConnection	Dbconn = new OracleConnection();
		/// <summary>
		/// DB Command
		/// </summary>
		public OracleCommand		Dbcmd = new OracleCommand();

		bool isTaransaction = false;
		
		string connectionString = string.Empty;

		/// <summary>
		/// sql 연결스트링
		/// </summary>
		public string ConnectionString
		{
			get { return this.ConnectionString; }
		}

		string TNS = string.Empty;
		string id = string.Empty;
		string pass = string.Empty;


		int retryTimes_Connecting = 1;

		readonly int retrySleepTime = 300;
		/// <summary>
		/// DB연결 재시도 횟수 (범위 1~5)
		/// </summary>
		public int RetryTimes_Connecting
		{
			set
			{
				if(value > 5)
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
		int retryTimes_Query = 3;
		public int RetryTimes_Query 
		{
			set
			{
				if(value > 5)
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
		/// 객체를 생성 하면서 Connection String 만든다.
		/// </summary>
		/// <param name="strIP"></param>
		/// <param name="strDB"></param>
		/// <param name="strID"></param>
		/// <param name="strPassword"></param>
		public Oracle(string strTNS, string strID, string strPassword)
		{
			Set_ConnectionString(strTNS, strID, strPassword);
		}

		/// <summary>
		/// 객체를 생성 하면서 Connection String 만든다.
		/// </summary>
		/// <param name="strConn"></param>
		public Oracle(strConnect strConn)
		{
			Set_ConnectionString(strConn.strTNS, strConn.strID, strConn.strPass);
		}

		
		public Oracle()
		{			
		}

		public bool load_DBSetting(string xmlFilePath)
		{
			try
			{
				DataSet ds = new DataSet();
				ds.ReadXml(xmlFilePath);
				
				DataTable dt = ds.Tables["DB"];
				
				this.Set_ConnectionString(dt.Rows[0]["TNS"].ToString(), dt.Rows[0]["ID"].ToString(), dt.Rows[0]["pass"].ToString());


				return true;
			}
			catch(Exception ex)
			{
				throw new Exception("DB Setting Loading Error :" + xmlFilePath + " 읽는 중 오류 발생 - " + ex.Message, ex);
			}
		}
		
		/// <summary>
		/// 연결스트링을 설정합니다.
		/// </summary>
		/// <param name="strTNS"></param>
		/// <param name="strID"></param>
		/// <param name="strPassword"></param>
		public void Set_ConnectionString(string strTNS, string strID, string strPass)
		{
			this.connectionString = string.Format("data source={0};user id={1};password={2}",strTNS, strID, strPass);
		}
		

		/// <summary>
		/// Db를 연결한다.
		/// </summary>
		/// <returns></returns>
		public bool DB_Open()
		{
			int Count = 0;
			while ( this.retryTimes_Connecting >= Count)
			{
				try
				{
                    Count++;

					if (db_Open())	return true;					
					
				}
				catch(Exception ex)
				{
					if (retryTimes_Connecting == Count) throw ex;
										
					Thread.Sleep(retrySleepTime);
				}
			}

            return false;
			
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
			catch( OracleException ex)
			{
				throw new Exception(string.Format("DB Close Err(Oracle) : {0}",ex.Message),ex);
			}
			catch(Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(일반) : {0}",ex.Message), ex);
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
				
				Dbconn.Close();
				return true;
			}
			catch( OracleException ex)
			{
				throw new Exception(string.Format("DB Close Err(Oracle) : {0}",ex.Message),ex);
			}
			catch(Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(일반) : {0}",ex.Message), ex);
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
				if (Dbconn.State  == System.Data.ConnectionState.Open)
					return true;
				else
				{					
					return this.db_Open();
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message, ex);
			}
		}
		
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
				Dbcmd.Transaction  = Dbconn.BeginTransaction(IsolationLevel.ReadCommitted);
				
				isTaransaction = true;
				return true;
			}
			catch( OracleException ex)
			{
				throw new Exception(string.Format("DB Close Err(Oracle) : {0}",ex.Message),ex);
			}
			catch(Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(일반) : {0}",ex.Message), ex);
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
				Dbcmd.Transaction = null;
				isTaransaction = false;
				return true;
			}
			catch( OracleException ex)
			{
				throw new Exception(string.Format("DB Close Err(Oracle) : {0}",ex.Message),ex);
			}
			catch(Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(일반) : {0}",ex.Message), ex);
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
				Dbcmd.Transaction = null;
				isTaransaction = false;
				return true;
			}
			catch( OracleException ex)
			{
				throw new Exception(string.Format("DB Close Err(Oracle) : {0}",ex.Message),ex);
			}
			catch(Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(일반) : {0}",ex.Message), ex);
			}
			finally
			{
				if (!this.isTaransaction) this.DB_Close();
			}
		}

		
		/// <summary>
		/// 쿼리 실행한다.
		/// </summary>
		/// <param name="Query"></param>
		/// <param name="dt"></param>
		/// <param name="strMsg"></param>
		/// <returns></returns>
		public DataSet dsExcute_Query(string Query)
		{			
			int Count = 0;
			while ( this.retryTimes_Query >= Count)
			{
				try
				{
                    Count++;

                    return dsexcute_Query(Query);
					
				}
				catch(Exception ex)
				{
					if (Count == this.RetryTimes_Query) throw ex;

					Thread.Sleep(retrySleepTime);
				}

			}

            return null;
			
		}

		private DataSet dsexcute_Query(string Query)
		{
			try
			{

                DB_isOpen();

				if(Query.Trim() == "") 
				{
					throw new Exception("쿼리에 문제가 있습니다.(길이 0)");
				}

                Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.Text;
				Dbcmd.CommandText = Query;

                DataSet ds = new DataSet();

				OracleDataAdapter da = new OracleDataAdapter(Dbcmd);
				
                da.Fill(ds);
				da.Dispose();
				
				return ds;
			}
			catch( OracleException ex)
			{
				throw new Exception(string.Format("쿼리 실행 Err(Oracle) : {0} / 쿼리 : {1}",ex.Message, Query),ex);
			}
			catch(Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(일반) : {0}",ex.Message), ex);
			}
			finally
			{
				if (!this.isTaransaction) this.DB_Close();
			}
		}


        /// <summary>
        /// 쿼리를 실행한다..
        /// </summary>
        /// <param name="Query"></param>
        /// <returns>영향 받은 데이터 수</returns>
		public int intExcute_Query(string Query)
		{			
			int Count = 0;
			while ( this.retryTimes_Query >= Count)
			{
				try
				{
                    Count++;
					
                    return intexcute_Query(Query);
					
				}
				catch(Exception ex)
				{
					if (Count == this.RetryTimes_Query) throw ex;

					Thread.Sleep(retrySleepTime);
				}
			}

            return 0;
			
		}
		private int intexcute_Query(string Query)
		{
			try
			{

                DB_isOpen();

				if(Query.Trim() == "") 
				{
					throw new Exception("쿼리에 문제가 있습니다.(길이 0)");
				}

                Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.Text;
				Dbcmd.CommandText = Query;

                int intRst = Dbcmd.ExecuteNonQuery();

                Dbcmd.Dispose();

                return intRst;
			}
			catch( OracleException ex)
			{
				throw new Exception(string.Format("쿼리 실행 Err(Oracle) : {0} / 쿼리 : {1}",ex.Message, Query),ex);
			}
			catch(Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(일반) : {0}",ex.Message), ex);
			}
			finally
			{
				if (!this.isTaransaction) this.DB_Close();
			}
		}

		/// <summary>
        /// 쿼리를 실행한다..
		/// </summary>
		/// <param name="Query"></param>
		/// <param name="Params"></param>
        /// <returns>영향 받은 데이터 수</returns>
		public int intExcute_Query(string Query, OracleParameter[] Params)
		{			
			int Count = 0;
			while ( this.retryTimes_Query >= Count)
			{
				try
				{
                    Count++;

                    return intexcute_Query(Query, Params);
					
				}
				catch(Exception ex)
				{
					if (Count == this.RetryTimes_Query) throw ex;

					Thread.Sleep(retrySleepTime);
				}
			}

            return 0;
		}

		private int intexcute_Query(string Query, OracleParameter[] Params)
		{
			try
			{

                this.DB_isOpen();

				if(Query.Trim() == "") 
				{
					throw new Exception("쿼리에 문제가 있습니다.(길이 0)");
				}
				
			    Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.Text;
				Dbcmd.CommandText = Query;
				
				foreach(OracleParameter param in Params)
				{
					Dbcmd.Parameters.Add(param);
				}
				
				int intRst = Dbcmd.ExecuteNonQuery();

                Dbcmd.Dispose();

                return intRst;
			}
			catch( OracleException ex)
			{
				throw new Exception(string.Format("쿼리 실행 Err(Oracle) : {0} / 쿼리 : {1}",ex.Message, Query),ex);
			}
			catch(Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(일반) : {0}",ex.Message), ex);
			}
			finally
			{
				if (!this.isTaransaction) this.DB_Close();
			}
		}

		
		/// <summary>
		/// 스토어 프로시져를 실행 합니다..
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="Params"></param>		
		/// <returns></returns>
		public int intExcute_StoredProcedure(string spName,OracleParameter[] Params)
		{			
			int Count = 0;
			while ( this.retryTimes_Query >= Count)
			{
				try
				{
                    Count++;

                    return intexcute_StoredProcedure(spName, Params);					
				}
				catch(Exception ex)
				{
					if (Count == this.RetryTimes_Query) throw new Exception(ex.Message, ex);

					Thread.Sleep(retrySleepTime);
				}
			}

            return 0;
		}
        private int intexcute_StoredProcedure(string spName, OracleParameter[] Params)
		{
			try
			{				
				
				this.DB_isOpen();
				
				if(spName.Trim() == "") 
				{
					throw new Exception("프로시져 이름이 공백입니다.");
				}
				
			    Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.StoredProcedure;
				Dbcmd.CommandText = spName;

				foreach(OracleParameter param in Params)
				{
					Dbcmd.Parameters.Add(param);
				}

				int intRst = Dbcmd.ExecuteNonQuery();

                Dbcmd.Dispose();

                return intRst;
			}
			catch( OracleException ex)
			{
				throw new Exception(string.Format("SP 실행 Err(Oracle) : {0} / SP : {1}" ,ex.Message, spName),ex);
			}
			catch(Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(일반) : {0}",ex.Message), ex);
			}
			finally
			{
				if (!this.isTaransaction) this.DB_Close();
			}
		}

		/// <summary>
		/// 스토어 프로시져를 실행 합니다..
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="Params"></param>		
		/// <returns></returns>
		public DataSet dsExcute_StoredProcedure(string spName, OracleParameter[] Params)
		{			
			int Count = 0;
			while ( this.retryTimes_Query >= Count)
			{
				try
				{
                    Count++;

                    return dsexcute_StoredProcedure(spName, Params);
					
				}
				catch(Exception ex)
				{
					if (Count == this.RetryTimes_Query) throw new Exception(ex.Message, ex);
					Thread.Sleep(retrySleepTime);
				}
			}

            return null;
			
		}

		private DataSet dsexcute_StoredProcedure(string spName, OracleParameter[] Params)
		{
			try
			{

                this.DB_isOpen();

				if(spName.Trim() == "") 
				{
					throw new Exception("프로시져 이름이 공백 입니다.");
				}
				
			    Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.StoredProcedure;
				Dbcmd.CommandText = spName;

				foreach(OracleParameter param in Params)
				{
					Dbcmd.Parameters.Add(param);
				}
                DataSet ds = new DataSet();

				OracleDataAdapter da = new OracleDataAdapter(Dbcmd);
				
                da.Fill(ds);

				da.Dispose();

				return ds;
			}
			catch( OracleException ex)
			{
				throw new Exception(string.Format("SP 실행 Err(Oracle) : {0} / SP : {1}" ,ex.Message, spName),ex);
			}
			catch(Exception ex)
			{
				throw new Exception(string.Format("DB Open Err(일반) : {0}",ex.Message), ex);
			}

			finally
			{
				if (!this.isTaransaction) this.DB_Close();
			}
		}




		/// <summary>
		/// 스토어 프로시져를 실행 합니다..
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="Params"></param>		
		/// <returns></returns>
		public void Excute_StoredProcedure(string spName, OracleParameter[] Params, int intLob_ParamCnt, FileInfo fi)
		{
			int Count = 0;
			while (this.retryTimes_Query >= Count)
			{
				try
				{
					Count++;

					excute_StoredProcedure(spName, Params, intLob_ParamCnt, fi);

				}
				catch (Exception ex)
				{
					if (Count == this.RetryTimes_Query) throw new Exception(ex.Message, ex);
					Thread.Sleep(retrySleepTime);
				}
			}

			

		}



		private void excute_StoredProcedure(string spName, OracleParameter[] Params, int intLob_ParamCnt, FileInfo fi)
		{
			try
			{

				this.DB_isOpen();

				if (spName.Trim() == "")
				{
					throw new Exception("프로시져 이름이 공백 입니다.");
				}

				Dbcmd.Parameters.Clear();
				this.Dbcmd.Connection = this.Dbconn;
				Dbcmd.CommandType = CommandType.StoredProcedure;
				Dbcmd.CommandText = spName;

				//Params[intLob_ParamCnt].Value = Lob;

				foreach (OracleParameter param in Params)
				{
					Dbcmd.Parameters.Add(param);
				}



				OracleLob Lob = (OracleLob)Params[intLob_ParamCnt].Value;	//= new OracleLob();			
				
				int intBlockSize = 1000;

				FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
				BinaryReader br = new BinaryReader(fs);

				try
				{					
					byte[] bytes = new byte[intBlockSize];
					int intBytes;

					while ((intBytes = br.Read(bytes, 0, bytes.Length)) > 0)
					{
						Lob.Write(bytes, 0, bytes.Length);
					}
				}
				catch
				{
					throw;
				}
				finally
				{
					br.Close();
					fs.Close();

					fs.Dispose();	
				}

				
				Dbcmd.ExecuteNonQuery();

				
			}
			catch (OracleException ex)
			{
				throw new Exception(string.Format("SP 실행 Err(Oracle) : {0} / SP : {1}", ex.Message, spName), ex);
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

	}
	*/


}
