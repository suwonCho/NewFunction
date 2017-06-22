using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Management;

namespace Function.Util
{
	public class WMI
	{
		public static DataTable InQuery(string namespc, string classes, string condition)
		{
			try
			{
				DataTable dt = new DataTable();
				bool first = true;
				condition = condition == string.Empty ? string.Empty : "where " + condition;

				ManagementObjectSearcher searcher =
					new ManagementObjectSearcher(namespc,
					string.Format("SELECT * FROM {0} {1}", classes, condition));

				foreach (ManagementObject queryObj in searcher.Get())
				{
					if (first)
					{
						foreach (PropertyData p in queryObj.Properties)
						{
							dt.Columns.Add(p.Name);							
						}

						first = false;
					}

					DataRow dr = dt.NewRow();

					foreach (PropertyData p in queryObj.Properties)
					{
						dr[p.Name] = Fnc.obj2String(p.Value);
					}

					dt.Rows.Add(dr);

				}

				return dt;
			}
			catch (ManagementException e)
			{
				throw e;
			}
		}
	}
}
