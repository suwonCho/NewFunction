using System;
using System.Text;
using System.IO;





/// <summary>
/// Log�� �״� Ŭ����..
/// </summary>
class clsLog
{
	private string LogPath;
	private bool OnOff;
	private string strFileName;
	private string strLastDelDate;				//�α����� ���������� ���¥..
	private int intDays;						//�� ���ڰ� ���� Log File ����..
	

	/// <summary>
	/// Log Ŭ������ �����Ѵ�.
	/// </summary>
	/// <param name="_Directory">�α� ���丮 ���</param>
	/// <param name="FileName">���ϸ�</param>
	/// <param name="_DelDays">�α� ��������</param>
	/// <param name="_OnOff">�α� ��Ͽ���</param>
	public clsLog(string _Directory, string FileName,int _DelDays, bool _OnOff)
	{
		try
		{
			/// ���� ���� ���� �˻��Ѵ�.
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
	/// �α׸� ����Ѵ�.. ���ϸ��� ���� ���ϸ� "Log ��-��-��" ���Ϸ� ���
	/// </summary>
	/// <param name="StrMsg">����� string</param>
	/// <returns>��������</returns>
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
	/// �α׸� ���..
	/// </summary>
	/// <param name="strMsg">����� string</param>
	/// <param name="FileName">���ϸ�(�������� ���� ����)</param>
	/// <returns>��������</returns>
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
	/// �α� ������ ���� �Ⱓ ���� �α׵��� ���� �ش�...
	/// ��.."{fileName} ��-��-��" ���Ϸ� ��� ���¸�..
	/// </summary>
	/// <param name="Days">�Ⱓ(��)</param>
	public void LogFileClear()
	{
		try
		{
			if (intDays == 0) return;				//���� �Ⱓ�� 0�̸� ó������ �ʴ´�.

			string fn;
			DateTime Dline = DateTime.Now;
			string strDline = string.Format("{0:D4}{1:D2}{2:D2}", Dline.Year, Dline.Month, Dline.Day);
			
			if (strDline == this.strLastDelDate) return; //������ �Ϸ翡 �ѹ���...

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
						WLog("�α� ����(" + fn + ")�� ���� �Ͽ����ϴ�.");
					}

				}
				catch //�����̸��� ��¥�� ��ȭ�� ����ó�� 
				{
				}
			
			}

			this.strLastDelDate = strDline;
		}
		catch(Exception ex)
		{
			WLog("�α� ���� ������ ������ �߻� �Ͽ����ϴ�. ==>" + ex.Message);
		}
	}
	
	/// <summary>
	/// ���� �߻� �α� �����.
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

