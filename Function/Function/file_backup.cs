using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Function
{
	/// <summary>
	/// 파일을 주기적으로 백업 하는 관리 클래스
	/// </summary>
	public class file_backup
	{
		/// <summary>
		/// 1시간 마다 백업 확인
		/// </summary>
		System.Threading.Timer _tmr;


		Dictionary<string, DateTime> _dicFiles = new Dictionary<string, DateTime>();

		Function.Util.Log _log;

		/// <summary>
		/// 백업 폴더
		/// </summary>
		string _backupFolder;

		/// <summary>
		/// 백업 작업 진행 여부
		/// </summary>
		bool _isDoBackup = false;

		/// <summary>
		/// 마지막 삭제 작업 일시
		/// </summary>
		DateTime _dtLastDelWork;

		/// <summary>
		/// 클래스 초기화(폼 로드하기 전에 하는게 좋다.)
		/// </summary>
		/// <param name="frm"></param>
		public file_backup(Form frm, string backupFolder, Function.Util.Log log = null)
		{
			frm.Load += Frm_Load;
			frm.FormClosed += Frm_FormClosed;

			//1시간에 1번씩 체크
			//_tmr = new System.Threading.Timer(new TimerCallback(DoBackup), null, 1000 * 60 * 60, 1000 * 60 * 60);

			//테스트용 1분
			_tmr = new System.Threading.Timer(new TimerCallback(DoBackup), null, 1000 * 60, 1000 * 60);


			_backupFolder = backupFolder;

			if(!Directory.Exists(_backupFolder))
			{
				Directory.CreateDirectory(_backupFolder);
			}

			_dtLastDelWork = new DateTime(2000, 1, 1);


			if (log != null)
				_log = log;
			else
				log = new Util.Log(backupFolder, "BackupLog", 30, true);
		}

		/// <summary>
		/// 파일을 추가 합니다.
		/// </summary>
		/// <param name="file"></param>
		public void FilesAdd(string file)
		{
			_dicFiles.Add(file, new DateTime(2000, 1, 1));
		}



		private void Frm_FormClosed(object sender, FormClosedEventArgs e)
		{
			DoBackup(null);
		}

		private void Frm_Load(object sender, EventArgs e)
		{
			DoBackup(null);
		}

		/// <summary>
		/// 백업을 수행한다.
		/// </summary>
		/// <param name="obj"></param>
		private void DoBackup(object obj)
		{
			if (_isDoBackup) return;

			_isDoBackup = true;

			try
			{

				if (!Directory.Exists(_backupFolder))
				{
					Directory.CreateDirectory(_backupFolder);
				}


				//파일의 속성이 바뀌거나 날짜가 바뀌면 삭제 하고, 1달 이상의 백업은 말일자만 남겨 놓는다.
				foreach (string f in _dicFiles.Keys)
				{
					BackUpProc(f);
				}

				DelProc();
			}
			catch(Exception ex)
			{
				if (_log != null) _log.WLog_Exception("DoBackup", ex);
			}
			finally
			{
				_isDoBackup = false;
			}
		}


		private void BackUpProc(string fileName)
		{
			try
			{

				FileInfo fi = new FileInfo(fileName);
				FileInfo bk;
				bool isCopy = true;

				string[] bkNm = Function.system.clsFile.FileSplit_NameExtention(fi.Name);

				string bn = string.Format("{0}\\{1}_{2}.{3}", _backupFolder, bkNm[0], DateTime.Now.ToShortDateString(), bkNm[1]);

				bk = new FileInfo(bn);

				if (bk.Exists)
				{
					isCopy = !(fi.CreationTime == bk.CreationTime && fi.LastWriteTime == bk.LastWriteTime);
				}

				if (isCopy)
				{
					fi.CopyTo(bk.FullName, true);

					bk.CreationTime = fi.CreationTime;
					fi.LastWriteTime = fi.LastWriteTime;
				}


				
			}
			catch (Exception ex)
			{
				if (_log != null) _log.WLog_Exception("BackUpProc", ex);
			}
		}


		/// <summary>
		/// 백업파일을 삭제 한다. 30일 이내는 삭제 하지 않고, 30일이 지난경우 말일자만 남겨 놓는다. 1년 지나면 전부 삭제.
		/// </summary>
		private void DelProc()
		{
			try
			{
				//하루에 한번
				//if (DateTime.Now.ToShortDateString() == _dtLastDelWork.ToShortDateString()) return;

				DirectoryInfo di = new DirectoryInfo(_backupFolder);

				DateTime tt;

				foreach(FileInfo fi in di.GetFiles())
				{
					string[] bkNm = Function.system.clsFile.FileSplit_NameExtention(fi.Name);

					string[] fn = bkNm[0].Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

					try
					{
						DateTime dn = Fnc.String2Date(fn[fn.Length - 1], Fnc.enDateType.Date);

						//30일 지난것만
						if (dn > DateTime.Now.AddDays(-30)) continue;

						if (dn > DateTime.Now.AddYears(-1))
						{
							//해당월 마지막 날짜를 구한다.
							tt = new DateTime(dn.Year, dn.Month, 1).AddMonths(1).AddDays(-1);
							if (dn.Day == tt.Day) continue;
						}

						fi.Delete();

						_log.WLog("[백업File]{0} 삭제 하였습니다.", fi.Name);

					}
					catch
					{

					}

				}

				_dtLastDelWork = DateTime.Now;

			}
			catch (Exception ex)
			{
				if (_log != null) _log.WLog_Exception("DelProc", ex);
			}
		}



	}	//end class
}
