using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoUpdater
{
	public enum enSeverType
	{
		ORACLE,
		WEB
	}


	class fnc
	{
		public static readonly string UpdateTypeSchema_FileName = "UpdateTypeSchema.xml";


		/// <summary>
		/// 그룹 설정 테이블 이름
		/// </summary>
		public static readonly string GroupTable_Name = "Group";

		/// <summary>
		/// 시스템 관리상 Upload불가 파일명
		/// </summary>
		public static readonly string[] NoUpload_FileName = new string[] { "UpdateDataSet.xml" };

		/// <summary>
		/// 시스템 관리상 Upload불가 폴더명
		/// </summary>
		public static readonly string[] NoUpload_FolderName = new string[] { "_History", "_Temp" };


	}
}
