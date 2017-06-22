using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Function
{
	/// <summary>
	/// 공용 enum : 수정 타입
	/// </summary>
	public enum enModifyType
	{
		None,
		Add,
		Modify,
		Delete,
		Copy
	}

	public enum enControlType
	{
		INPUT,
		LABEL,
		CONTAINER,
		Null
	}

	/// <summary>
	/// 공용 enum : DB 수정 타입
	/// </summary>
	public enum enDBCommand
	{
		Add,
		Modify,
		Add_Modify,
		Delete
	}

	/// <summary>
	/// 공용 enum 
	/// </summary>
	public enum enWorkingStatus
	{
		Wating,
		Working,
		Pause,
		Stop,
		Finish,
		Canceled
	}

	/// <summary>
	/// 문자 위치
	/// </summary>
	public enum enStringLocation
	{
		Front,
		End
	}


}
