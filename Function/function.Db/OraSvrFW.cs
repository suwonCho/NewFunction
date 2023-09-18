using function.Util;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace function.Db
{
    /// <summary>
    /// IIS통한 DB(방화벽 우회)
    /// </summary>
    public class OraSvrFW
    {
        public function.Util.Log Log = null;

        static Encoding encoding = UTF8Encoding.UTF8;

        /// <summary>
		/// db 처리 명령 테이블
		/// </summary>
		DataTable ExcDt = null;

        /// <summary>
        /// 서버 연결 클래스
        /// </summary>
        object Svr;

        public static string param_sperator;

        public static string item_seperator;


        /// <summary>
        /// 암호/복호 Key
        /// </summary>
        internal readonly string EncKey = "NcxaQ5hh/aPMCto1csH1q3v78JpILl6J";

        /// <summary>
        /// 암호/복호 IV
        /// </summary>
        internal readonly string EncIV = "bWg5XHFdZeo=";


        /// <summary>
        /// ip주소 요청 시 서버에 전송함(필수)
        /// </summary>
        public string IPAddress { get; set; } = string.Empty;
        /// <summary>
        /// DB에 등록 된 POP CD  - 요청 시 서버에 전송함(필수)
        /// </summary>
        public string POP_CD { get; set; } = string.Empty;


        public OraSvrFW(object svr, string pop_cd, Log log)
        {
            //서버를 아래와 같이 생성해 넘겨 준다.
            //Svr = new DB_POP_SERVICE.DB_POP_SERVICE();
            //Svr.Url = DBUrl;

            POP_CD = pop_cd;
            IPAddress = Fnc.IPAddressGet();

            Svr = svr;
            Log = log;



            //ExcDt = Svr.Excute_Table_Get();
            //item_seperator = Svr.item_sperator_get();
            //param_sperator = Svr.param_sperator_get();

            var type = svr.GetType();

            var mi = type.GetMethod("Excute_Table_Get");
            ExcDt = (DataTable)mi.Invoke(svr, null);

            mi = type.GetMethod("item_sperator_get");
            item_seperator = (string)mi.Invoke(svr, null);

            mi = type.GetMethod("param_sperator_get");
            param_sperator = (string)mi.Invoke(svr, null);
        }


        public OraSvrFW(object svr, string pop_cd, Log log, string ipAddress)
        {

            POP_CD = pop_cd;
            IPAddress = ipAddress; // Fnc.IPAddressGet();

            Svr = svr;
            Log = log;

            //ExcDt = Svr.Excute_Table_Get();
            //item_seperator = Svr.item_sperator_get();
            //param_sperator = Svr.param_sperator_get();

            var type = svr.GetType();

            var mi = type.GetMethod("Excute_Table_Get");
            ExcDt = (DataTable)mi.Invoke(svr, null);

            mi = type.GetMethod("item_sperator_get");
            item_seperator = (string)mi.Invoke(svr, null);

            mi = type.GetMethod("param_sperator_get");
            param_sperator = (string)mi.Invoke(svr, null);
        }




        /// <summary>
        /// 암호/복호 관리 클래스
        /// </summary>
        function.Util.cryptography cry = null;


        /// <summary>
        /// 복호화 한다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string CrypDec(string value)
        {
            if (cry == null)
            {
                cry = new function.Util.cryptography();
                cry.encoding = UTF8Encoding.UTF8;
                cry.IV = EncIV;
                cry.Key = EncKey;
            }

            string txt = cry.DecryptingFromHex(value);

            return txt;

        }


        /// <summary>
        /// 암호화 한다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string CrypEnc(string value)
        {
            if (cry == null)
            {
                cry = new function.Util.cryptography();
                cry.encoding = UTF8Encoding.UTF8;
                cry.IV = EncIV;
                cry.Key = EncKey;
            }

            return cry.EncryptingToHex(value);
        }



        /// <summary>
        /// 서버 요청 내역을 로그로 남긴다.
        /// </summary>
        /// <param name="MethodName"></param>
        /// <param name="param"></param>
        public void ServerReq_Log_Write(string MethodName, List<OracleParameter> param)
        {

            if (Log == null) return;

            string pLog = string.Empty;

            if (param != null)
            {

                foreach (OracleParameter p in param)
                {
                    pLog += $" [{p.ParameterName}]{p.Value}";
                }
            }

            Log.WLog($"서버요청[{MethodName}]{pLog}");

        }

        /// <summary>
        /// 결과 테이블을 생성하고 결과를 넣어 준다.
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="DateTime"></param>
        /// <param name="ReturnMsg"></param>
        /// <returns></returns>
        public DataTable ExcuteTable_Set(string IPAddress, string POP_CD, DateTime dTime, enDBExcuteType ExcuteType, string Query, List<OracleParameter> param, DataTable baseDt = null)
        {
            DataTable rst;
            if (baseDt != null)
                rst = baseDt;
            else
                rst = ExcDt.Clone();

            DataRow dr = rst.NewRow();
            dr["IPAddress"] = IPAddress;
            dr["POP_CD"] = POP_CD;
            dr["DateTime"] = dTime;
            dr["ExcuteType"] = (int)ExcuteType;

            dr["Query"] = Query;
            dr["Params"] = ParamsToXml(param);              //ParamsToString(param);			

            rst.Rows.Add(dr);

            ServerReq_Log_Write(Query, param);

            return rst;
        }


        /// <summary>
        /// 서버에 요청 하고 DatTable로 응답을 받는다.
        /// </summary>
        /// <param name="dTime"></param>
        /// <param name="ExcuteType"></param>
        /// <param name="Query"></param>
        /// <param name="param"></param>
        /// <param name="baseDt"></param>
        /// <returns></returns>
        public DataTable ExcuteTable_RtnTable(DateTime dTime, enDBExcuteType ExcuteType, string Query, List<OracleParameter> param, DataTable baseDt = null, enShowQueryResultType rType = enShowQueryResultType.Count)
        {
            int cnt = 0;

            try
            {
                excute_before();

                //요청 테이블 생성
                DataTable dt = ExcuteTable_Set(DateTime.Now, enDBExcuteType.QueryTable, Query, param, baseDt);

                DataRow dr = dt.Rows[dt.Rows.Count - 1];

                string tLog = $"[IPAddress]{dr["IPAddress"]} [POP_CD]{dr["POP_CD"]} [DateTime]{dr["DateTime"]} [ExcuteType]{ExcuteType.ToString()}\r\n{dr["Query"]}\r\n\t[Params]{dr["Params"]}\r\n";

                Console.WriteLine(tLog);

                if (Log != null) Log.WLog(tLog);

                //테이블 암호화
                string encTxt = DataTable_Enc(dt);


                //서버 요청
                string rtn = ""; // Svr.Excute_Table(encTxt);

                var type = Svr.GetType();
                object[] obj = new object[] { encTxt };
                var mi = type.GetMethod("Excute_Table");
                rtn = (string)mi.Invoke(Svr, obj);

                //암호 복호화 후 테이블화
                DataTable rst = DataSet_Dec(rtn).Tables[0];

                cnt = rst.Rows.Count;

                return rst;
            }
            catch (Exception ex)
            {
                Log.WLog_Exception("ExcuteTable_RtnTable", ex);
                rType = enShowQueryResultType.Error;
                throw ex;
            }
            finally
            {
                excute_after(cnt, rType);
            }
        }


        /// <summary>
        /// 결과 테이블을 생성하고 결과를 넣어 준다.(IPAddress와 POP_CD를 현재 설절정보에서 조회 하여 추가한다)
        /// </summary>
        /// <param name="dTime"></param>
        /// <param name="ExcuteType"></param>
        /// <param name="Query"></param>
        /// <param name="param"></param>
        /// <param name="baseDt"></param>
        /// <returns></returns>
        public DataTable ExcuteTable_Set(DateTime dTime, enDBExcuteType ExcuteType, string Query, List<OracleParameter> param, DataTable baseDt = null)
        {
            return ExcuteTable_Set(IPAddress, POP_CD, dTime, ExcuteType, Query, param, baseDt);
        }

        /// <summary>
        /// DataTable 암호화 한다.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string DataTable_Enc(DataTable dt)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Tables.Add(dt);

                return DataSet_Enc(ds);
            }
        }



        /// <summary>
        /// dataset을 암호화 한다.
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public string DataSet_Enc(DataSet ds)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            ds.WriteXml(sw, XmlWriteMode.WriteSchema);

            sw.Flush();

            string txt = sb.ToString();

            return CrypEnc(txt);
        }


        /// <summary>
        /// dataset을 복호화 한다.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public DataSet DataSet_Dec(string xml)
        {
            //TextReader txt = new StreamReader(fn.CrypDec(xml));

            Stream txt = new MemoryStream(encoding.GetBytes(CrypDec(xml)));


            using (DataSet ds = new DataSet())
            {
                ds.ReadXml(txt);

                return ds;
            }
        }

        /// <summary>
        /// xml로 부터 Datatable 데이터를 가져온다.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public DataTable DataTable_Get_FromText(DataTable dt, string xml)
        {
            Stream txt = new MemoryStream(encoding.GetBytes(xml));

            dt.ReadXml(txt);

            return dt;
        }

        /// <summary>
        /// 오라클 파라메터 값을 string으로 변환 합니다.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string ParamsToString(List<OracleParameter> param)
        {
            if (param == null) return string.Empty;

            string rtn = string.Empty;

            foreach (Oracle.DataAccess.Client.OracleParameter p in param)
            {
                if (rtn != string.Empty) rtn += param_sperator;

                rtn += $"{p.ParameterName}{item_seperator}{(int)p.OracleDbType}{item_seperator}{p.Value}";
            }

            return rtn;
        }


        /// <summary>
        /// 오라클 파라메터 값을 xml으로 변환 합니다.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string ParamsToXml(List<OracleParameter> param)
        {
            if (param == null || param.Count < 1) return string.Empty;

            StringBuilder rtn = new StringBuilder();
            rtn.Append(@"<DocumentElement>
  <Params>");

            string s = "";

            foreach (Oracle.DataAccess.Client.OracleParameter p in param)
            {
                s = Fnc.obj2String(p.Value);

                s = s.Replace("<", "&lt;");
                s = s.Replace(">", "&gt;");

                rtn.Append($"\r\n    <{p.ParameterName}>{s}{item_seperator}{(int)p.OracleDbType}{item_seperator}{(int)p.Direction}</{p.ParameterName}>");
            }

            rtn.Append(@"
  </Params>
</DocumentElement>");


            return rtn.ToString();
        }



        /// <summary>
        /// DataTable 결과에 Seq를 설정 및 Checked 필드를 추가한다.
        /// </summary>
        /// <param name="dt"></param>
        public void DataTableRst_SetSeq(DataTable dt, string fieldName, bool AddChecked = false)
        {
            int seq = 1;

            if (AddChecked)
            {
                dt.Columns.Add("Checked", typeof(int));
            }

            if (fieldName != null && fieldName != string.Empty)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    dr[fieldName] = seq;
                    seq++;

                    if (AddChecked) dr["Checked"] = 0;

                }
            }


        }

        event delObjectNone onExcute_before;

        /// <summary>
        /// 서버의 요청전에 발생하는 이벤트
        /// </summary>
        public event delObjectNone OnExcute_before
        {
            add { onExcute_before += value;  }
            remove { onExcute_before += value; }
        }

        /// <summary>
        /// db 서버에 요청전 해야 할일
        /// </summary>
        public void excute_before()
        {
            //object activeform = fnc.activeform_get();

            //if (activeform == null) return;

            ////메세지 창을 지운다.
            //function.DFnc.Method_Excute(info_PGM.MainForm, "setMessage", new object[] { false, string.Empty, false });

            ////웨이트 창을 띠운다.
            //function.DFnc.Method_Excute(activeform, "PleaseWaitForm_Show", new object[] { true });

            ////System.Threading.Thread.Sleep(5000);

            if (onExcute_before != null) onExcute_before.Invoke();

        }


        event delObjectNone onExcute_after;

        /// <summary>
        /// 쿼리 요청 응답 수신 후 발생 이벤트
        /// </summary>
        public event delObjectNone OnExcute_after
        {
            add { onExcute_after += value; }
            remove { onExcute_after += value; }
        }


        /// <summary>
        /// 쿼리 요청 응답후 해야 할일
        /// </summary>
        /// <param name="cnt"></param>
        /// <param name="type"></param>
        public void excute_after(int cnt, enShowQueryResultType type)
        {

            if (onExcute_after != null) onExcute_after.Invoke();

            //object activeform = fnc.activeform_get();

            //if (activeform == null) return;

            ////웨이트 창을 닫는다.
            //function.DFnc.Method_Excute(activeform, "PleaseWaitForm_Show", new object[] { false });

            //string msg;

            //switch (type)
            //{
            //    case enShowQueryResultType.Count:
            //        msg = string.Format("{0}건의 데이터가 조회 되었습니다.", cnt);
            //        break;

            //    case enShowQueryResultType.Error:       //에러메세지는 exception 단에서 표시 하므로 메시지 표시 안함
            //    case enShowQueryResultType.None:
            //        return;


            //    default:
            //        msg = "처리가 완료 되었습니다.";
            //        break;

            //}

            //function.DFnc.Method_Excute(info_PGM.MainForm, "setMessage", new object[] { false, msg, false });


        }



        /// <summary>
        /// 데이터테이블에 Checked 필드를 추가한다.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable dt_AddCheckedField(DataTable dt)
        {
            dt.Columns.Add("Checked", typeof(int));

            return dt;
        }

        /// <summary>
        /// Where 조건을 추가 한다. value가없으면 추가 안한다(Value는 parameter로 추가 해야 한다). field명이 파라메터 명된다.
        /// </summary>
        /// <param name="field">필드/파라메터 명</param>
        /// <param name="value"></param>
        /// <param name="paramName">필드 조건과 파라메터명이 다를 경우 넣어 준다.</param>
        /// <returns></returns>
        public string QueryWhereAdd(string field, string value, string paramName = null)
        {
            if (value == null || value.Trim() == string.Empty)
                return string.Empty;

            if (paramName == null) paramName = field;

            return $" AND {field.ToUpper()} = :{paramName.ToUpper()}";
        }



        /// <summary>
        /// 프로시져를 수행 하고 결고를 dataset으로 리턴 받는다.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="Params"></param>
        /// <param name="type"></param>
        /// <param name="writelog"></param>
        /// <returns></returns>
        public DataTable dsExcute_StoredProcedure(string spName, OracleParameter[] Params = null, enShowQueryResultType type = enShowQueryResultType.Count, bool writelog = false)
        {
            int cnt = 0;

            try
            {
                excute_before();                                
                DataTable dt = ExcuteTable_RtnTable(DateTime.Now, enDBExcuteType.ProcedureRtn, spName, Params == null ? null : Params.ToList());
                cnt = dt.Rows.Count;

                return dt;

            }
            catch (Exception ex)
            {
                cnt = -1;
                throw (ex);
            }
            finally
            {
                if (cnt != -1) excute_after(cnt, type);
            }
        }

        public DataTable dsExcute_Query(string Query, OracleParameter[] Params = null, enShowQueryResultType type = enShowQueryResultType.Count, bool writelog = false)
        {
            int cnt = 0;

            try
            {
                excute_before();
                DataTable dt = ExcuteTable_RtnTable(DateTime.Now, enDBExcuteType.ProcedureRtn, Query, Params == null ? null : Params.ToList());
                cnt = dt.Rows.Count;

                return dt;
            }
            catch (Exception ex)
            {
                cnt = -1;
                throw (ex);
            }
            finally
            {
                if (cnt != -1) excute_after(cnt, type);
            }
        }

        /// <summary>
        /// 오라클 파라메터를 만들고 추가 하여 준다.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static List<OracleParameter> ParamListBuild(string Name, OracleDbType type, string value, List<OracleParameter> lst = null)
        {
            if (lst == null)
                lst = new List<OracleParameter>();

            if (value != null && value != string.Empty)
            {
                OracleParameter p = new OracleParameter(Name, type);
                p.Value = value;
                lst.Add(p);
            }

            return lst;
        }

        
    }   //end class
}
