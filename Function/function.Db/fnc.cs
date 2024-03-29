﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace function.Db
{	
	/// <summary>
	/// 데이터 베이스 형태
	/// </summary>
	public enum enDBType
	{
		None = 0,
		MsSQL = 1,
		Oracle = 3,
		SQLite = 5
	}

    public enum enShowQueryResultType
    {
        /// <summary>
        /// 알림 없음
        /// </summary>
        None,
        /// <summary>
        /// 쿼리 종료 만 알림
        /// </summary>
        Msg,
        /// <summary>
        /// 건수 알림
        /// </summary>
        Count,
        /// <summary>
        /// 에러 발생 - 알림 없음
        /// </summary>
        Error
    }

    public enum enDBExcuteType
    {
        /// <summary>
        /// 쿼리 수행 / 응답 : 데이터테이블
        /// </summary>
        QueryTable = 1,
        /// <summary>
        /// 쿼리 수행 / 응답 : 건수
        /// </summary>
        QueryCnt = 2,
        /// <summary>
        /// 프로시져 수행 / 응답 : 성공-변경건수 / 실패 Exception
        /// </summary>
        ProcedureRtn = 8,
        /// <summary>
        /// 프로시져 수행 / 응답 : 데이터테이블
        /// </summary>
        ProcedureTable = 9
    }



    public class fnc
	{
		/// <summary>
		/// Null값 이나 string.empty값을 dbnull로 변경
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static object NullStringEmpty2DBnull(object obj)
		{
			if (obj == null)
				return DBNull.Value;
			else if(obj.ToString().Trim().Equals(string.Empty))
				return DBNull.Value;
			else
			{
				return obj;
			}
		}


		/// <summary>
		/// 텍스트를 seperator를 구분자로 쪼개어 seperatconditon로 연결 한 쿼리 조건을 리턴한다.
		/// </summary>
		/// <param name="text">문자열</param>
		/// <param name="field">조회 조건 필드</param>
		/// <param name="textSeperator">문자열 구분자</param>
		/// <param name="condition">검색조건</param>
		/// <param name="seperatconditon">검색조건 연결자 : AND / OR</param>
		/// <returns></returns>
		public static string Query_ConditionBuilder(string text, string field, string textSeperator = null, string condition = "=", string seperatconditon = "AND")
		{
			string rtn = string.Empty;

			if (text == null) return rtn;
			if (textSeperator == null) textSeperator = ((char)3).ToString();			

			string[] con = text.Split(new string[] { textSeperator, "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			if (con.Length < 1) return string.Empty;

			string prompt = condition.ToUpper() == "LIKE" ? "%" : string.Empty;

			foreach (string c in con)
			{
				rtn = Fnc.StringAdd(rtn, string.Format("{0} {2} '{3}{1}{3}'", field, c, condition, prompt), " " + seperatconditon + " ");
			}

			return string.Format(" ( {0} ) ", rtn);
		}

		// <summary>
		/// 텍스트를 seperator를 구분자로 쪼개어 seperatconditon로 연결 한 쿼리 조건을 리턴한다.(조회 필드 2개 이상)
		/// </summary>
		/// <param name="text">조회 문자열</param>
		/// <param name="field">조회 조건 필드</param>
		/// <param name="testSeperator">문자열 구분자</param>
		/// <param name="testSeperator">필드간 검색조건 연결자 : AND / OR</param>
		/// <param name="condition">검색조건</param>
		/// <param name="seperatconditon">검색조건 연결자 : AND / OR</param>
		/// <returns></returns>
		public static string Query_ConditionBuilder(string text, string[] field, string testSeperator = null, string fieldConditon = "OR",  string condition = "=", string seperatconditon = "AND")
		{
			string rtn = string.Empty;

			if (text == null) return rtn;
			if (testSeperator == null) testSeperator = ((char)3).ToString();

			string[] con = text.Split(new string[] { testSeperator, "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			if (con.Length < 1)
				return string.Empty;

			string prompt = condition.ToUpper() == "LIKE" ? "%" : string.Empty;
			int cIdx = 0;

			foreach (string f in field)
			{
				if (rtn != "") rtn += $" {fieldConditon} ";

				rtn += "( ";

				cIdx = 0;

				foreach (string c in con)
				{
					

					rtn += (cIdx > 0 ? " " + seperatconditon + " ": "") + string.Format("{0} {2} '{3}{1}{3}'", f, c, condition, prompt);

					cIdx++;
				}

				rtn += $" ) ";
			}

			return string.Format(" ( {0} ) ", rtn);
		}



		public enum enQueryType
		{
			Select,
			Insert,
			Update
		}

		static string getMarker(enDBType dbType)
		{
			string marker = string.Empty;

			switch (dbType)
			{
				case enDBType.MsSQL:
					marker = "@";
					break;

				case enDBType.Oracle:
					marker = ":";
					break;
			}

			return marker;
		}


		/// <summary>
		/// 데이터 테이블에서 쿼리를 만든다.
		/// </summary>
		/// <param name="dbType"></param>
		/// <param name="dt"></param>
		/// <param name="TableName"></param>
		/// <param name="QueryType"></param>
		/// <param name="isParam"></param>
		/// <returns></returns>
		public static string Query_BuildbyDataTable(enDBType dbType, DataTable dt, string TableName, enQueryType QueryType, bool isParam)
		{
			string[] cols = new string[dt.Columns.Count];
			int idx = 0;

			foreach(DataColumn c in dt.Columns)
			{
				cols[idx] = c.ColumnName;
				idx++;
			}


			return Query_Build(dbType, cols, TableName, QueryType, isParam);
		}



		/// <summary>
		/// 쿼리를 생성 한다
		/// </summary>
		/// <param name="dbType"></param>
		/// <param name="Columns"></param>
		/// <param name="TableName"></param>
		/// <param name="QueryType"></param>
		/// <param name="isParam"></param>
		/// <returns></returns>
		public static string Query_Build(enDBType dbType, string[] Columns, string TableName, enQueryType QueryType,bool isParam)
		{
			string q1 = string.Empty;
			string q2 = string.Empty;
			string marker = getMarker(dbType);
			

			int idx = 0;
			string rst = string.Empty;

			foreach(string c in Columns)
			{
				string col = "\"" + c + "\"";

				switch (QueryType)
				{
					case enQueryType.Select:
						q1 = Fnc.StringAdd(q1, col, ", ");
						break;

					case enQueryType.Insert:
						q1 = Fnc.StringAdd(q1, col, ", ");

						if(isParam)
							q2 = Fnc.StringAdd(q2, marker + c, ", ");
						else
							q2 = Fnc.StringAdd(q2, "'{" + idx + "}'", ", ");

						break;

					case enQueryType.Update:
						q1 = Fnc.StringAdd(q1, string.Format("{0} = {1}", col, isParam ? marker + c: "'{" + idx + "}'"), ",");
						break;

				}

				idx++;

				if ( (idx%3) == 0 )
				{
					q1 += "\r\n";
					q2 += "\r\n";
				}

				

			}

			switch(QueryType)
			{
				case enQueryType.Select:
					rst = string.Format("SELECT {0} FROM {1}", q1, TableName);
					break;

				case enQueryType.Insert:
					rst = string.Format(@"INSERT INTO {0} 
(	{1}  ) 
VALUES
(	{2}	)", TableName, q1, q2);
					break;


				case enQueryType.Update:
					rst = string.Format(@"UPDATE {0} 
SET {1}", TableName, q1);
					break;

			}
			



			return rst;
			
		}





	}	//end class
}
