using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using Shell32;
using System.IO;

namespace Function.api
{
	/// <summary>
	/// 휴지통 처리 관련 클래스
	/// </summary>
	public class Recycle
	{
		/// <summary>
		/// 파일을 삭제 합니다.
		/// </summary>
		/// <param name="path">파일 패스</param>
		/// <param name="showDialog">삭제확인 표시 문구 표시 여부</param>
		/// <param name="SendToRecycleBin"></param>
		/// <returns>작업 취소 여부 true:처리 / false:취소</returns>
		public static bool DeleteFile(string path, bool showDialog, bool SendToRecycleBin)
		{
			UIOption uiopt = showDialog ? UIOption.AllDialogs : UIOption.OnlyErrorDialogs;

			RecycleOption rOption = SendToRecycleBin ? RecycleOption.SendToRecycleBin : RecycleOption.DeletePermanently;

			try
			{
				FileSystem.DeleteFile(path, uiopt, rOption);
			}
			catch (OperationCanceledException)
			{
				return false;
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return true;
		}

		/// <summary>
		/// 폴더를 삭제 합니다.
		/// </summary>
		/// <param name="path">파일 패스</param>
		/// <param name="showDialog">삭제확인 표시 문구 표시 여부</param>
		/// <param name="SendToRecycleBin"></param>
		/// <returns>작업 취소 여부 true:처리 / false:취소</returns>
		public static bool DeleteFolder(string path, bool showDialog, bool SendToRecycleBin)
		{
			UIOption uiopt = showDialog ? UIOption.AllDialogs : UIOption.OnlyErrorDialogs;

			RecycleOption rOption = SendToRecycleBin ? RecycleOption.SendToRecycleBin : RecycleOption.DeletePermanently;

			try
			{
				FileSystem.DeleteDirectory(path, uiopt, rOption);
			}
			catch (OperationCanceledException)
			{
				return false;
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return true;
		}

		public static void GetRcycleBinItems()
		{
			Shell shl = new Shell();
			Folder rec = shl.NameSpace(10);

			for (int i = 0; i < rec.Items().Count; i++)
			{

			}

		}

		public static bool Restore(string Item)
		{
			Shell Shl = new Shell();
			Folder Recycler = Shl.NameSpace(10);
			for (int i = 0; i < Recycler.Items().Count; i++)
			{
				FolderItem FI = Recycler.Items().Item(i);
				string FileName = Recycler.GetDetailsOf(FI, 0);
				if (Path.GetExtension(FileName) == "") FileName += Path.GetExtension(FI.Path);
				//Necessary for systems with hidden file extensions.
				string FilePath = Recycler.GetDetailsOf(FI, 1);
				if (Item == Path.Combine(FilePath, FileName))
				{
					DoVerb(FI, "ESTORE");
					return true;
				}
			}
			return false;
		}

		private static bool DoVerb(FolderItem Item, string Verb)
		{
			foreach (FolderItemVerb FIVerb in Item.Verbs())
			{
				if (FIVerb.Name.ToUpper().Contains(Verb.ToUpper()))
				{
					FIVerb.DoIt();
					return true;
				}
			}
			return false;
		}



	}
}
