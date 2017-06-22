using System;
using System.Text;
using System.IO;





/// <summary>
/// Log를 쌓는 클래스..
/// </summary>
class clsLog
{
	private string LogPath;
	private bool OnOff;
	private string strFileName;
	private string strLastDelDate;				//로그파일 마지막으로 지운날짜..
	private int intDays;						//이 일자가 지난 Log File 삭제..
	

	/// <summary>
	/// Log 클래스를 생성한다.
	/// </summary>
	/// <param name="_Directory">로그 디렉토리 경로</param>
	/// <param name="FileName">파일명</param>
	/// <param name="_DelDays">로그 보관일자</param>
	/// <param name="_OnOff">로그 기록여부</param>
	public clsLog(string _Directory, string FileName,int _DelDays, bool _OnOff)
	{
		try
		{
			/// 폴더 존재 여부 검사한다.
			if (!Directory.Exists(_Directory))
				Directory.CreateDirectory(_Directory);
			
			if (_Directory.Substring(_Directory.Length - 1, 1) != @"\")
				LogPath = _Directory + @"\";
			else
				LogPath = _Directory;
		


			if (FileName.Trim().Length < 1)
				this.strFileName = "Log";
			else
				this.strFileName = FileName.Trim();

			intDays = _DelDays;

			OnOff = _OnOff;

			this.LogFileClear();
		}
		catch(Exception  ex)
		{
			throw new Exception(ex.Message, ex);
		}

	}

	
	/// <summary>
	/// 로그를 기록한다.. 파일명을 지정 안하면 "Log 년-월-일" 파일로 기록
	/// </summary>
	/// <param name="StrMsg">기록할 string</param>
	/// <returns>성공여부</returns>
	public bool WLog(String StrMsg)
	{
		DateTime dt = DateTime.Now;
		string mon = "00" + dt.Month;
		mon = mon.Substring(mon.Length - 2, 2);
		string day = "00" + dt.Day;
		day = day.Substring(day.Length - 2, 2);
		string FileName = string.Format(@"{0} {1}-{2}-{3}.txt", strFileName, dt.Year, mon, day);

		return WLog(StrMsg, FileName);
	}


	/// <summary>
	/// 로그를 기록..
	/// </summary>
	/// <param name="strMsg">기록할 string</param>
	/// <param name="FileName">파일명(폴더명을 쓰지 말것)</param>
	/// <returns>성공여부</returns>
	public bool WLog(string strMsg, string FileName)
	{
		this.LogFileClear();

		if (!OnOff)
			return true;


		lock(this)
		{
			try
			{
				DateTime dt = DateTime.Now;
				string Msg = string.Format("[{0}:{1}:{2}] {3}\r\n", dt.ToString("HH"), dt.ToString("mm"), dt.ToString("ss"),strMsg);
				byte [] bytMsg = Encoding.Default.GetBytes(Msg);
				
				FileStream writer = new FileStream(LogPath + FileName, FileMode.Append, FileAccess.Write, FileShare.Write);
				
				writer.Write(bytMsg,0,bytMsg.Length); 
				writer.Close();

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
		
	/// <summary>
	/// 로그 폴더에 일정 기간 지난 로그들을 지워 준다...
	/// 단.."{fileName} 년-월-일" 파일로 기록 형태만..
	/// </summary>
	/// <param name="Days">기간(일)</param>
	public void LogFileClear()
	{
		try
		{
			if (intDays == 0) return;				//삭제 기간이 0이면 처리하지 않는다.

			string fn;
			DateTime Dline = DateTime.Now;
			string strDline = string.Format("{0:D4}{1:D2}{2:D2}", Dline.Year, Dline.Month, Dline.Day);
			
			if (strDline == this.strLastDelDate) return; //삭제는 하루에 한번만...

			Dline = Dline.AddDays((intDays * -1));
			DateTime FDate;

			int fYear, fMonth, fDay;
			int intFLen = strFileName.Length + 1;
			DirectoryInfo di = new DirectoryInfo(this.LogPath);

			foreach(FileInfo fi in di.GetFiles(this.strFileName + "*.txt"))
			{
				fn = fi.Name;
			
				try
				{
					fYear = Convert.ToInt32(fn.Substring(intFLen,4),10);  //4
					fMonth = Convert.ToInt32(fn.Substring(intFLen + 5,2),10);
					fDay = Convert.ToInt32(fn.Substring(intFLen + 8,2),10);

					FDate = new DateTime(fYear, fMonth, fDay, 23, 59, 59);
					
					if (FDate < Dline)
					{
						fi.Delete();
						WLog("로그 파일(" + fn + ")을 삭제 하였습니다.");
					}

				}
				catch //파일이름을 날짜로 변화시 오류처리 
				{
				}
			
			}

			this.strLastDelDate = strDline;
		}
		catch(Exception ex)
		{
			WLog("로그 파일 삭제시 문제가 발생 하였습니다. ==>" + ex.Message);
		}
	}
	
	/// <summary>
	/// 에러 발생 로그 남긴다.
	/// </summary>
	/// <param name="strModule"></param>
	/// <param name="e"></param>
	public void WLog_Exception(string strModule, Exception e)
	{
		string strLog;
		strLog = string.Format("Error in {0} : {1} - {2}",strModule, e.Message, e.ToString()); 
		this.WLog(strLog);
	}
	
}

