using System;
using System.Data;
using System.Threading;
using System.IO;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace AutoUpdateClient
{
	
	/// <summary>
	/// Oracle Class : Oracle ���� dll ���
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
        /// sql ���ὺƮ��
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
        /// DB���� ��õ� Ƚ�� (���� 1~5)
        /// </summary>
        public int RetryTimes_Connecting
        {
            set
            {
                if (value > 5)
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
        int retryTimes_Query = 3;
        public int RetryTimes_Query
        {
            set
            {
                if (value > 5)
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
        public OracleDB(string strTNS, string strID, string strPassword)
        {
            Set_ConnectionString(strTNS, strID, strPassword);
        }

        /// <summary>
        /// ��ü�� ���� �ϸ鼭 Connection String �����.
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
                throw new Exception("DB Setting Loading Error :" + xmlFilePath + " �д� �� ���� �߻� - " + ex.Message, ex);
            }
        }

        /// <summary>
        /// ���ὺƮ���� �����մϴ�.
        /// </summary>
        /// <param name="strTNS"></param>
        /// <param name="strID"></param>
        /// <param name="strPassword"></param>
        public void Set_ConnectionString(string strTNS, string strID, string strPass)
        {
            this.connectionString = string.Format("data source={0};user id={1};password={2}", strTNS, strID, strPass);
        }


        /// <summary>
        /// Db�� �����Ѵ�.
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
                    throw new Exception("���� ��Ʈ�� ������ �Ǿ� ���� �ʽ��ϴ�.");
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

                Dbconn.Close();
                return true;
            }
            catch (OracleException ex)
            {
                throw new Exception(string.Format("DB Close Err(Oracle) : {0}", ex.Message), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("DB Open Err(�Ϲ�) : {0}", ex.Message), ex);
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
            catch (OracleException ex)
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
            catch (OracleException ex)
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
            catch (OracleException ex)
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
        /// ���� �����Ѵ�.
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
                    throw new Exception("������ ������ �ֽ��ϴ�.(���� 0)");
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
                throw new Exception(string.Format("���� ���� Err(Oracle) : {0} / ���� : {1}", ex.Message, Query), ex);
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
        /// ������ �����Ѵ�..
        /// </summary>
        /// <param name="Query"></param>
        /// <returns>���� ���� ������ ��</returns>
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
                    throw new Exception("������ ������ �ֽ��ϴ�.(���� 0)");
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
                throw new Exception(string.Format("���� ���� Err(Oracle) : {0} / ���� : {1}", ex.Message, Query), ex);
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
        /// ������ �����Ѵ�..
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="Params"></param>
        /// <returns>���� ���� ������ ��</returns>
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
                    throw new Exception("������ ������ �ֽ��ϴ�.(���� 0)");
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
                throw new Exception(string.Format("���� ���� Err(Oracle) : {0} / ���� : {1}", ex.Message, Query), ex);
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
        /// ����� ���ν����� ���� �մϴ�..
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
                    throw new Exception("���ν��� �̸��� �����Դϴ�.");
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
                throw new Exception(string.Format("SP ���� Err(Oracle) : {0} / SP : {1}", ex.Message, spName), ex);
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
        /// ����� ���ν����� ���� �մϴ�..
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
                    throw new Exception("���ν��� �̸��� ���� �Դϴ�.");
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
                throw new Exception(string.Format("SP ���� Err(Oracle) : {0} / SP : {1}", ex.Message, spName), ex);
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
        /// ����� ���ν����� ���� �մϴ�..
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
                    throw new Exception("���ν��� �̸��� ���� �Դϴ�.");
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
                throw new Exception(string.Format("SP ���� Err(Oracle) : {0} / SP : {1}", ex.Message, spName), ex);
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
        /// ����� ���ν����� ���� �մϴ�..
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
                    throw new Exception("���ν��� �̸��� ���� �Դϴ�.");
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
                throw new Exception(string.Format("SP ���� Err(Oracle) : {0} / SP : {1}", ex.Message, spName), ex);
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
        /// ����Ŭ DB���� ����ϴ� Function Method
        /// </summary>
        public static class Fnc
        {


            /// <summary>
            /// string.empty���� dbnull�� ��ȯ �Ͽ� �ش�.
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
            /// oracle object���� string���� ��ȯ�Ͽ� �ش�.
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
            /// oracle object���� int���� ��ȯ�Ͽ� �ش�.
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
            /// oracle object���� long���� ��ȯ�Ͽ� �ش�.
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
            /// oracle object���� float���� ��ȯ�Ͽ� �ش�.
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
            /// ����Ŭ�� ���̺��� �ִ��� �˻縦 �Ͽ� �ش�.
            /// </summary>
            /// <param name="strConn"></param>
            /// <param name="strTableName"></param>
            /// <returns></returns>
            public static bool Table_Check_Exists(OracleDB.strConnect strConn, string strTableName)
            {
                OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

                //TAUTOUPDATE ���̺� ���� �˻� �� ������ ���� �Ͽ� �ش�.
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
            /// ����Ŭ�� Package�� �ִ��� �˻縦 �Ͽ� �ش�.
            /// </summary>
            /// <param name="strConn"></param>
            /// <param name="strTableName"></param>
            /// <returns></returns>
            public static bool PACKAGE_Check_Exists(OracleDB.strConnect strConn, string strPackageName)
            {
                OracleDB clsDB = new OracleDB(strConn.strTNS, strConn.strID, strConn.strPass);

                //TAUTOUPDATE ���̺� ���� �˻� �� ������ ���� �Ͽ� �ش�.
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

