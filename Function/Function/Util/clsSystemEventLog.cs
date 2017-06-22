using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Function.Util
{
	/// <summary>
	/// System Event Log 기록 Class
	/// </summary>
	public class clsSystemEventLog
	{
		EventLog log;


		/// <summary>
		/// 인스턴스 생성
		/// </summary>
		/// <param name="strSource"></param>
		/// <param name="strLog"></param>
		public clsSystemEventLog(string strSource, string strLog)
		{
			if (!EventLog.SourceExists(strSource))
			{
				EventLog.CreateEventSource(strSource, strLog);
			}

			log = new EventLog();
			log.Source = strSource;
			log.Log = strLog;

		}

		public void WriteEntry(string strLog)
		{
			log.WriteEntry(strLog);		

		}

		public void WriteEntry(string format, params object[] args)
		{
			log.WriteEntry(string.Format(format, args));

		}








	}
}
