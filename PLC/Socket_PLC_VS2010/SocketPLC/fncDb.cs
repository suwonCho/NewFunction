using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SocketPLC
{
	class fncDb
	{
		public static string sqldb_path = System.Windows.Forms.Application.StartupPath + @"\SocketPLC_DB.sqliteDB";

		/// <summary>
		/// 어드레스 타입 종류를 조회 한다.
		/// </summary>
		/// <param name="PLCType"></param>
		/// <returns></returns>
		public static DataTable AddType_Get(string PLCType)
		{
			
			Function.Db.SQLite db = new Function.Db.SQLite(sqldb_path);

			string sql = string.Format(@"SELECT PLCTYPE, ADDTYPE, DESC, '[' || ADDTYPE || ']' || DESC Expression
FROM PLC_ADDTYPE
WHERE PLCTYPE = '{0}'
ORDER BY PRIORITY", PLCType);

			return db.dsExcute_Query(sql).Tables[0];


		}


		/// <summary>
		/// 어드레스 타입별 등록된 어드레스들을 가져온다.
		/// </summary>
		/// <param name="PLCType"></param>
		/// <param name="AddType"></param>
		/// <returns></returns>
		public static DataTable Address_Get(string PLCType, string AddType)
		{
			Function.Db.SQLite db = new Function.Db.SQLite(sqldb_path);

			string sql = string.Format(@"SELECT *
FROM PLC_Addresses
WHERE PLCTYPE = '{0}'
AND ADDTYPE = '{1}'
ORDER BY PRIORITY", PLCType, AddType);

			return db.dsExcute_Query(sql).Tables[0];


		}

		public static void Address_Set(string PLCType, string AddType, DataTable dtAdd)
		{
			if (AddType == null || AddType.Equals(string.Empty)) return;

			Function.Db.SQLite db = new Function.Db.SQLite(sqldb_path);

			try
			{
				db.BeginTransaction();

				string sql = string.Format(@"DELETE FROM PLC_Addresses
WHERE PLCTYPE = '{0}'
AND ADDTYPE = '{1}'", PLCType, AddType);

				db.intExcute_Query(sql);

				foreach (DataRow dr in dtAdd.Rows)
				{
					sql = string.Format(@"INSERT INTO PLC_Addresses(
PLCType, ADDType, Address,
Value, HexValue, Priority,
Desc, Desc2, AddGroup)
VALUES 
( '{0}', '{1}', '{2}',
'{3}', '{4}', {5},
'{6}', '{7}', '{8}' )", PLCType, AddType, dr["Address"],
			   dr["Value"], dr["hexValue"], dr["Priority"],
			   dr["Desc"], dr["Desc2"], dr["AddGroup"]);

					db.intExcute_Query(sql);
				}

				db.CommitTransaction();

			}
			catch
			{
				db.RollBackTransaction();
				throw;
			}
			



		}


		// <summary>
		/// 어드레스 타입별 등록된 값 관리테이블을 가져온다.
		/// </summary>
		/// <param name="PLCType"></param>
		/// <param name="AddType"></param>
		/// <returns></returns>
		public static DataTable PLC_ValueMng_Get(string PLCType, string AddType)
		{
			Function.Db.SQLite db = new Function.Db.SQLite(sqldb_path);

			string sql = string.Format(@"SELECT *
FROM PLC_Value_Mng
WHERE PLCTYPE = '{0}'
AND ADDTYPE = '{1}'
ORDER BY PRIORITY", PLCType, AddType);

			return db.dsExcute_Query(sql).Tables[0];


		}



		public static void PLC_ValueMng_Save(string PLCType, string AddType, DataTable dtMng)
		{
			if (AddType == null || AddType.Equals(string.Empty)) return;

			Function.Db.SQLite db = new Function.Db.SQLite(sqldb_path);

			try
			{
				db.BeginTransaction();

				string sql = string.Format(@"DELETE FROM PLC_Value_Mng
WHERE PLCTYPE = '{0}'
AND ADDTYPE = '{1}'", PLCType, AddType);

				db.intExcute_Query(sql);

				foreach (DataRow dr in dtMng.Rows)
				{
					sql = string.Format(@"INSERT INTO PLC_Value_Mng(
PLCType, ADDType, Priority, 
Mng_Type, Address, Address_Length, 
Value, ValueType, Condition, 
isUse, Desc)
VALUES 
( '{0}', '{1}', {2},
'{3}', '{4}', '{5}',
'{6}', '{7}', '{8}',
'{9}', '{10}' )", PLCType, AddType, dr["Priority"],
			   dr["Mng_Type"], dr["Address"], dr["Address_Length"],
			   dr["Value"], dr["ValueType"], dr["Condition"],
			   dr["isUse"], dr["Desc"]);

					db.intExcute_Query(sql);
				}

				db.CommitTransaction();

			}
			catch
			{
				db.RollBackTransaction();
				throw;
			}




		}

	} // end class

}
