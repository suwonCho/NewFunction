using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Massenger
{

	/// <summary>
	/// 연락처 조회 필드
	/// </summary>
	public enum enContactInfoType
	{
		/// <summary>
		/// 고유넘버
		/// </summary>
		id,
		/// <summary>
		/// 사용자 이름(first_name, last_name like 조건으로)
		/// </summary>
		name,
		/// <summary>
		/// id
		/// </summary>
		username,
		/// <summary>
		/// 전화 번호로 조회-국가코드 포함 ex)8210~~~~~~
		/// </summary>
		phoneNumber
	}


	public enum enChatsInfoType
	{
		/// <summary>
		/// 고유넘버
		/// </summary>
		id,
		/// <summary>
		/// 채팅방 이름
		/// </summary>
		title
	}

	class fnc
	{

		/// <summary>
		/// 텔레그램에서 메시지 보낼때 사용하는 폰번호를 수정한다.
		/// </summary>
		/// <param name="phonenumber"></param>
		/// <returns></returns>
		public static string PhoneNumberInit(string phonenumber)
		{
			string rtn = phonenumber;

			//+로 시작하면 삭제
			if (rtn.StartsWith("+")) rtn = rtn.Substring(1);

			//국가번호가 없으면 입력하여 준다.
			if (rtn.StartsWith("01")) rtn = "82" + rtn.Substring(1);

			return rtn;
		}
	}
}
