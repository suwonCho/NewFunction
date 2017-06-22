using System;
using System.Data;
using System.Threading;
using System.IO;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace AutoUpdateClient
{
	
	/// <summary>
	/// Oracle Class : Oracle 제공 dll 사용
	/// </summary>
    public class OracleDB
    {
        public struct strConnect
        {
            public string strTNS;
            public string strID;
            public string strPass;
        }






        /// <summary>
        /// DB Connection
        /// </summary>
        public OracleConnection Dbconn = new OracleConnection();
        /// <summary>
        /// DB Command
        /// </summary>
        public OracleCommand Dbcmd = new OracleCommand();

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
        int retryTimes_Query = 3;
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
        /// 객체를 생성 하면서 Connection String 만든다.
        /// </summary>
        /// <param name="strIP"></param>
        /// <param name="strDB"></param>
        /// <param name="strID"></param>
        /// <param name="strPassword"></param>
        public OracleDB(string strTNS, string strID, string strPassword)
        {
            Set_ConnectionString(strTNS, strID, strPassword);
        }

        /// <summary>
        /// 객체를 생성 하면서 Connection String 만든다.
        /// </summary>
        /// <param name="strConn"></param>
        public OracleDB(strConnect strConn)
        {
            Set_ConnectionString(strConn.strTNS, strConn.strID, strConn.strPass);
        }



        public OracleDB()
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
            catch (Exception ex)
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
            this.connectionString = string.Format("data source={0};user id={1};password={2}", strTNS, strID, strPass);
        }


        /// <summary>
        /// Db를 연결한다.
        /// </summary>
        /// <returns></returns>
        public bool DB_Open()
        {
            int Count = 0;
            while (this.retryTimes_Connecting >= Count)
            {
                try
                {
                    Count++;

                    if (db_Open()) return true;

                }
                catch (Exception ex)
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
            catch (OracleException ex)
            {
                throw new Exception(string.Format("DB Close Err(Oracle) : {0}", ex.Message), ex);
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

                Dbconn.Close();
                return true;
            }
            catch (OracleException ex)
            {
                throw new Exception(string.Format("DB Close Err(Oracle) : {0}", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("DB Open Err(일반) : {0}", ex.Message), ex);
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
            catch (Exception ex)
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
                Dbconn.BeginTransaction(IsolationLevel.ReadCommitted);

                isTaransaction = true;
                return true;
            }
            catch (OracleException ex)
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
            catch (OracleException ex)
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
            catch (OracleException ex)
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
        /// 쿼리 실행한다.
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="dt"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public DataSet dsExcute_Query(string Query)
        {
            int Count = 0;
            while (this.retryTimes_Query >= Count)
            {
                try
                {
                    Count++;

                    return dsexcute_Query(Query);

                }
                catch (Exception ex)
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

                if (Query.Trim() == "")
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
            catch (OracleException ex)
            {
                throw new Exception(string.Format("쿼리 실행 Err(Oracle) : {0} / 쿼리 : {1}", ex.Message, Query), ex);
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
        /// 쿼리를 실행한다..
        /// </summary>
        /// <param name="Query"></param>
        /// <returns>영향 받은 데이터 수</returns>
        public int intExcute_Query(string Query)
        {
            int Count = 0;
            while (this.retryTimes_Query >= Count)
            {
                try
                {
                    Count++;

                    return intexcute_Query(Query);

                }
                catch (Exception ex)
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

                if (Query.Trim() == "")
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
            catch (OracleException ex)
            {
                throw new Exception(string.Format("쿼리 실행 Err(Oracle) : {0} / 쿼리 : {1}", ex.Message, Query), ex);
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
        /// 쿼리를 실행한다..
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="Params"></param>
        /// <returns>영향 받은 데이터 수</returns>
        public int intExcute_Query(string Query, OracleParameter[] Params)
        {
            int Count = 0;
            while (this.retryTimes_Query >= Count)
            {
                try
                {
                    Count++;

                    return intexcute_Query(Query, Params);

                }
                catch (Exception ex)
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

                if (Query.Trim() == "")
                {
                    throw new Exception("쿼리에 문제가 있습니다.(길이 0)");
                }

                Dbcmd.Parameters.Clear();
                this.Dbcmd.Connection = this.Dbconn;
                Dbcmd.CommandType = CommandType.Text;
                Dbcmd.CommandText = Query;

                foreach (OracleParameter param in Params)
                {
                    Dbcmd.Parameters.Add(param);
                }

                int intRst = Dbcmd.ExecuteNonQuery();

                Dbcmd.Dispose();

                return intRst;
            }
            catch (OracleException ex)
            {
                throw new Exception(string.Format("쿼리 실행 Err(Oracle) : {0} / 쿼리 : {1}", ex.Message, Query), ex);
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
        /// 스토어 프로시져를 실행 합니다..
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="Params"></param>		
        /// <returns></returns>
        public int intExcute_StoredProcedure(string spName, OracleParameter[] Params)
        {
            int Count = 0;
            while (this.retryTimes_Query >= Count)
            {
                try
                {
                    Count++;

                    return intexcute_StoredProcedure(spName, Params);
                }
                catch (Exception ex)
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

                if (spName.Trim() == "")
                {
                    throw new Exception("프로시져 이름이 공백입니다.");
                }

                Dbcmd.Parameters.Clear();
                this.Dbcmd.Connection = this.Dbconn;
                Dbcmd.CommandType = CommandType.StoredProcedure;
                Dbcmd.CommandText = spName;

                foreach (OracleParameter param in Params)
                {
                    Dbcmd.Parameters.Add(param);
                }

                int intRst = Dbcmd.ExecuteNonQuery();

                Dbcmd.Dispose();

                return intRst;
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

        /// <summary>
        /// 스토어 프로시져를 실행 합니다..
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="Params"></param>		
        /// <returns></returns>
        public DataSet dsExcute_StoredProcedure(string spName, OracleParameter[] Params)
        {
            int Count = 0;
            while (this.retryTimes_Query >= Count)
            {
                try
                {
                    Count++;

                    return dsexcute_StoredProcedure(spName, Params);

                }
                catch (Exception ex)
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

                if (spName.Trim() == "")
                {
                    throw new Exception("프로시져 이름이 공백 입니다.");
                }

                Dbcmd.Parameters.Clear();
                this.Dbcmd.Connection = this.Dbconn;
                Dbcmd.CommandType = CommandType.StoredProcedure;
                Dbcmd.CommandText = spName;

                foreach (OracleParameter param in Params)
                {
                    Dbcmd.Parameters.Add(param);
                }
                DataSet ds = new DataSet();

                OracleDataAdapter da = new OracleDataAdapter(Dbcmd);

                da.Fill(ds);

                da.Dispose();

                return ds;
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

                    return;

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

                foreach (OracleParameter param in Params)
                {
                    Dbcmd.Parameters.Add(param);
                }

                OracleBlob Lob = new OracleBlob(Dbconn);

                int intBlockSize = 15000;
                //int intFileSize = Convert.ToInt32(fi.Length);


                FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                try
                {
                    byte[] bytes = new byte[intBlockSize];

                    /*
                    int intReadSize = 0;

                    if (intFileSize > intBlockSize)
                        intReadSize = intBlockSize;
                    else
                        intReadSize = intFileSize;
                    */

                    int intBytes;

                    while ((intBytes = br.Read(bytes, 0, bytes.Length)) > 0)
                    {
                        Lob.Write(bytes, 0, intBytes);
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

                Dbcmd.Parameters[intLob_ParamCnt].Value = Lob;	//DBNull.Value;	//Lob;

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


        /// <summary>
        /// 스토어 프로시져를 실행 합니다..
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="Params"></param>
        /// <param name="intLob_ParamCnt"></param>
        /// <param name="fi"></param>
        /// <param name="evtP"></param>
        public void Excute_StoredProcedure(string spName, OracleParameter[] Params, int intLob_ParamCnt, FileInfo fi, delExcuteProcedure_Progress evtP)
        {
            int Count = 0;
            while (this.retryTimes_Query >= Count)
            {
                try
                {
                    Count++;

                    excute_StoredProcedure(spName, Params, intLob_ParamCnt, fi, evtP);

                    return;

                }
                catch (Exception ex)
                {
                    if (Count == this.RetryTimes_Query) throw new Exception(ex.Message, ex);
                    Thread.Sleep(retrySleepTime);
                }
            }



        }


        public delegate void delExcuteProcedure_Progress(int intMax, int intValue);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="Params"></param>
        /// <param name="intLob_ParamCnt"></param>
        /// <param name="fi"></param>
        /// <param name="evtP"></param>
        private void excute_StoredProcedure(string spName, OracleParameter[] Params, int intLob_ParamCnt, FileInfo fi, delExcuteProcedure_Progress evtP)
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

                foreach (OracleParameter param in Params)
                {
                    Dbcmd.Parameters.Add(param);
                }

                OracleBlob Lob = new OracleBlob(Dbconn);

                int intBlockSize = 15000;
                //int intFileSize = Convert.ToInt32(fi.Length);


                FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                try
                {
                    byte[] bytes = new byte[intBlockSize];

                    int intBytes;
                    int intTotalBytes = 0;

                    while ((intBytes = br.Read(bytes, 0, bytes.Length)) > 0)
                    {
                        Lob.Write(bytes, 0, intBytes);

                        intTotalBytes += intBytes;

                        if (evtP != null)
                            evtP(int.Parse(fi.Length.ToString()), intTotalBytes);
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

                Dbcmd.Parameters[intLob_ParamCnt].Value = Lob;	//DBNull.Value;	//Lob;

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


















        /// <summary>
        /// 오라클 DB사용시 사용하는 Function Method
        /// </summary>
        public static class Fnc
        {


            /// <summary>
            /// string.empty값을 dbnull로 변환 하여 준다.
            /// </summary>
            /// <param name="param"></param>
            /// <param name="str"></param>
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
            /// oracle object값을 string으로 변환하여 준다.
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public static string obj2String(object obj)
            {
                if (obj == null || obj.ToString() == "null" || obj.ToString() == string.Empty)
                    return string.Empty;
                else
                    return obj.ToString().Trim();
            }



            /// <summary>
            /// oracle object값을 int으로 변환하여 준다.
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public static int obj2int(object obj)
            {
                if (obj == null || obj.ToString() == string.Empty)
                    return 0;
                else
                    return Convert.ToInt32(obj.ToString());
            }


            /// <summary>
            /// oracle object값을 long으로 변환하여 준다.
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public static long obj2Long(object obj)
            {
                if (obj == null || obj.ToString() == "null" || obj.ToString() == string.Empty)
                    return 0;
                else
                    return Convert.ToInt64(obj);
            }


            /// <summary>
            /// oracle object값을 float으로 변환하여 준다.
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public static float obj2Float(object obj)
            {
                if (obj == null || obj.ToString() == "null" || obj.ToString() == string.Empty)
                    return 0;
                else
                    return float.Parse(obj.ToString());
            }


            /// <summary>
            /// 오라클에 테이블이 있는지 검사를 하여 준다.
            /// </summary>
            /// <param name="strConn"></param>
            /// <param name="strTableName"></param>
            /// <returns></returns>
            public static bool Table_Check_Exists(OracleDB.strConnect strConn, string strTableName)
            {
                OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

                //TAUTOUPDATE 테이블 존재 검사 후 없으면 생성 하여 준다.
                string Sql = string.Format(@"SELECT COUNT(*)								
							FROM USER_OBJECTS
							WHERE OBJECT_TYPE ='TABLE'
								AND OBJECT_NAME = '{0}'", strTableName);


                using (DataSet ds = clsDB.dsExcute_Query(Sql))
                {
                    int intTableCnt = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                    if (intTableCnt < 1)
                        return false;
                    else
                        return true;
                }
            }


            /// <summary>
            /// 오라클에 Package가 있는지 검사를 하여 준다.
            /// </summary>
            /// <param name="strConn"></param>
            /// <param name="strTableName"></param>
            /// <returns></returns>
            public static bool PACKAGE_Check_Exists(OracleDB.strConnect strConn, string strPackageName)
            {
                OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

                //TAUTOUPDATE 테이블 존재 검사 후 없으면 생성 하여 준다.
                string Sql = string.Format(@"SELECT COUNT(*)								
							FROM USER_OBJECTS
							WHERE OBJECT_TYPE ='PACKAGE'
								AND OBJECT_NAME = '{0}'", strPackageName);


                using (DataSet ds = clsDB.dsExcute_Query(Sql))
                {
                    int intTableCnt = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                    if (intTableCnt < 1)
                        return false;
                    else
                        return true;
                }
            }




        }


    }



}

