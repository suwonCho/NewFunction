using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Function
{

	/// <summary>
	/// 설정관리의 항목타입 enum
	/// </summary>
	public enum enSettingValueType
	{
		Value,
		Info1,
		Info2,
		Info3
	}


	/// <summary>
	/// 설정관리 클래스(DataSet으로 처리)
	/// </summary>
	public class Setting
	{
		public struct stSettingRow
		{
			public string Name;
			public string Value;
			public string Info1;
			public string Info2;
			public string Info3;
		}


		DataSet _setting = null;

		DataTable _crurr = null;

		string _filepath;

		/// <summary>
		/// 파일이 생성여부
		/// </summary>
		public bool IsChanged = false;



		/// <summary>
		/// 그룹이름을 가져온다
		/// </summary>
		/// <returns></returns>
		public string[] GroupNames
		{
			get
			{
				string[] names = new string[_setting.Tables.Count];
				int idx = 0;

				foreach (DataTable dt in _setting.Tables)
				{
					names[idx] = dt.TableName;
					idx++;
				}

				return names;
			}
		}




		/// <summary>
		/// 파일을 로드하면서 클래스를 생성한다.
		/// 파일이 없을경우 생성한다.
		/// </summary>
		/// <param name="filepath">파일경로</param>
		/// <param name="isCreate">파일이 없을경우 생성한다.</param>
		public Setting(string filepath, bool isCreate = true)
		{
			_filepath = filepath;

			if (!Function.system.clsFile.FileExists(filepath))
			{
				_setting = new DataSet();
				_setting.WriteXml(filepath, XmlWriteMode.WriteSchema);
				IsChanged = true;
			}
			else
			{
				_setting = new DataSet();
				_setting.ReadXml(filepath, XmlReadMode.Auto);
			}
			
		}


		


		private DataTable group_select(string groupName)
		{
			groupName = groupName.ToUpper();
			foreach(DataTable t in _setting.Tables)
			{
				if(t.TableName.Equals(groupName))
				{
					return t;
				}
			}

			return null;
		}

		


		/// <summary>
		/// 설정 그룸을 추가 합니다.
		/// (데이터 테이블 추가)
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>
		public DataTable Group_Create(string groupName)
		{
			groupName = groupName.ToUpper();
			DataTable dt = group_select(groupName);

			if (dt != null) return dt;

			dt = new DataTable();
			dt.TableName = groupName;
			dt.Columns.Add("Name", typeof(System.String));
			dt.Columns.Add("Value", typeof(System.String));
			dt.Columns.Add("info1", typeof(System.String));
			dt.Columns.Add("info2", typeof(System.String));
			dt.Columns.Add("info3", typeof(System.String));

			_setting.Tables.Add(dt);

			IsChanged = true;

			return dt;
		}

		/// <summary>
		/// 그룹을 이름으로 찾고 현재 그룹으로 선택 합니다.
		/// </summary>
		/// <param name="groupName"></param>
		/// <param name="isCreate"></param>
		/// <returns></returns>
		public DataTable Group_Select(string groupName, bool isCreate = true)
		{
			groupName = groupName.ToUpper();
			DataTable dt = group_select(groupName);

			if (dt == null)
			{
				if (isCreate) dt = Group_Create(groupName);
			}

			_crurr = dt;

			return _crurr;

		}


		/// <summary>
		/// 그룹 설정에 필드를 추가합니다.
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="type"></param>
		public void Group_Column_Add(string columnName, Type type)
		{
			Group_Column_Add(_crurr.TableName, columnName, type);
		}

		/// <summary>
		/// 그룹 설정에 필드를 추가합니다.
		/// </summary>
		/// <param name="groupName"></param>
		/// <param name="columnName"></param>
		/// <param name="type"></param>
		public void Group_Column_Add(string groupName, string columnName, Type type)
		{
			DataTable dt = group_select(groupName);

			if(!dt.Columns.Contains(columnName))
			{
				dt.Columns.Add(columnName, type);

			}
		}


		/// <summary>
		/// 설정값을 가져온다.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="name"></param>
		/// <param name="defalutValue">기본값 - 설정값일 없을 경우 이 값으로 설정을 추가한다. null 일경우 추가 하지 않고 null 리턴</param>
		/// <returns></returns>
		private string value_get(DataTable dt, string name, enSettingValueType valueType = enSettingValueType.Value, string defalutValue = null)
		{
			name = name.ToUpper();

			string field;


			switch(valueType)
			{
				case enSettingValueType.Info1:
					field = "Info1";
					break;

				case enSettingValueType.Info2:
					field = "Info2";
					break;

				case enSettingValueType.Info3:
					field = "Info3";
					break;

				default:
					field = "Value";
					break;
			}



			DataRow[] dr = dt.Select(string.Format("Name = '{0}'", name));
			if (dr.Length < 1)
			{
				if (defalutValue == null)
					return null;
				else
				{
					DataRow r = dt.NewRow();
					r["Name"] = name;
					r["Value"] = string.Empty;
					r[field] = defalutValue;
					dt.Rows.Add(r);
					IsChanged = true;
					return defalutValue;
				}

			}

			return Fnc.obj2String(dr[0][field]);
		}


		private stSettingRow Row_get(DataTable dt, string name)
		{
			stSettingRow row = new stSettingRow();

			name = name.ToUpper();

			DataRow[] dr = dt.Select(string.Format("Name = '{0}'", name));
			if (dr.Length < 1)
			{
				row.Name = null;
			}
			else
			{
				row.Name = Fnc.obj2String(dr[0]["name"]);
				row.Value = Fnc.obj2String(dr[0]["value"]);

				row.Info1 = Fnc.obj2String(dr[0]["info1"]);
				row.Info2 = Fnc.obj2String(dr[0]["info2"]);
				row.Info3 = Fnc.obj2String(dr[0]["info3"]);
			}

			return row;
		}

		/// <summary>
		/// 현재 그룹의 값 DDataRowCollection을 가지고 온다	<para/>
		/// 필드 : name, value, info1, info2, info3
		/// </summary>
		/// <returns></returns>
		public DataRowCollection Group_GetValues()
		{
			return _crurr.Rows;
        }


		/// <summary>
		/// 설정값을 가져온다.
		/// </summary>
		/// <param name="groupName">그룹이름</param>
		/// <param name="name">설정이름</param>
		/// <param name="valueType">설정타입</param>
		/// <param name="defalutValue">기본값 - 설정값일 없을 경우 이 값으로 설정을 추가한다. null 일경우 추가 하지 않고 null 리턴</param>
		/// <returns></returns>
		public string Value_Get(string groupName, string name, enSettingValueType valueType, string defalutValue = null)
		{
			DataTable dt = group_select(groupName);
			return value_get(dt, name, valueType, defalutValue);
		}


		/// <summary>
		/// 설정값을 가져온다.
		/// </summary>
		/// <param name="groupName">그룹이름</param>
		/// <param name="name">설정이름</param>
		/// <param name="defalutValue">기본값 - 설정값일 없을 경우 이 값으로 설정을 추가한다. null 일경우 추가 하지 않고 null 리턴</param>
		/// <returns></returns>
		public string Value_Get(string groupName, string name, string defalutValue = null)
		{
			DataTable dt = group_select(groupName);
			return value_get(dt, name, enSettingValueType.Value,  defalutValue);
		}

		/// <summary>
		/// 현재 그룹의 설정값을 가져온다.
		/// </summary>
		/// <param name="name">설정이름</param>
		/// <param name="defalutValue">기본값 - 설정값일 없을 경우 이 값으로 설정을 추가한다. null 일경우 추가 하지 않고 null 리턴</param>
		/// <returns></returns>
		public string Value_Get(string name, string defalutValue = null)
		{
			return value_get(_crurr, name, enSettingValueType.Value,  defalutValue);
		}


		/// <summary>
		/// 설정값을 저장하거나 추가 합니다.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="name"></param>
		/// <param name="Value"></param>
		private void value_set(DataTable dt, string name, string Value, string info1 = null, string info2 = null, string info3 = null)
		{
			name = name.ToUpper();

			DataRow[] dr = dt.Select(string.Format("Name = '{0}'", name));

			if (dr.Length < 1)
			{
				DataRow r = dt.NewRow();
				r["Name"] = name;
				r["Value"] = Value;

				r["info1"] = info1;
				r["info2"] = info2;
				r["info3"] = info3;

				dt.Rows.Add(r);
			}
			else
			{
				dr[0]["Value"] = Value;

				if (info1 != null) dr[0]["info1"] = info1;
				if (info2 != null) dr[0]["info2"] = info2;
				if (info3 != null) dr[0]["info3"] = info3;

			}

			IsChanged = true;
		}


		/// <summary>
		/// 설정값을 저장하거나 추가 합니다.
		/// </summary>
		/// <param name="groupName">그룹이름</param>
		/// <param name="name">설정이름</param>
		/// <param name="Value">기본값 - 설정값일 없을 경우 이 값으로 설정을 추가한다. null 일경우 추가 하지 않고 null 리턴</param>
		/// <returns></returns>
		public void Value_Set(string groupName, string name, string Value, string info1 = null, string info2 = null, string info3 = null)
		{
			DataTable dt = group_select(groupName);
			value_set(dt, name, Value, info1, info2, info3);
		}

		/// <summary>
		/// 설정값을 저장하거나 추가 합니다.
		/// </summary>
		/// <param name="name">설정이름</param>
		/// <param name="defalutValue">기본값 - 설정값일 없을 경우 이 값으로 설정을 추가한다. null 일경우 추가 하지 않고 null 리턴</param>
		/// <returns></returns>
		public void Value_Set(string name, string Value, string info1 = null, string info2 = null, string info3 = null)
		{
			value_set(_crurr, name, Value, info1, info2, info3);
		}


		public void Setting_Save(string filePath)
		{

			DataView dv;
			DataSet ds = new DataSet();

			for(int i=0; i < _setting.Tables.Count;i++)
			{
				_setting.Tables[i].DefaultView.Sort = "Name asc";
				_setting.Tables[i].AcceptChanges();

				//ds.Tables.Add(_setting.Tables[i].DefaultView.ToTable(_setting.Tables[i].TableName));
			}

			//_setting = ds;

			_setting.WriteXml(filePath, XmlWriteMode.WriteSchema);
		}


		public void Setting_Save()
		{
			Setting_Save(_filepath);
		}

	}
}
