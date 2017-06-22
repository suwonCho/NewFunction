using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppAuth
{
	/// <summary>
	/// 클래스 사용 인증 확인 클래스-상속받아 사용한다.<para/>
	/// 사용하는 클래스와 생성자의 주석 앞에 '(인증필요)' 주석 추가<para/>
	/// 상속받은 클래스는 static 메소드 Auto.Add를 이용하여 인증 정보 추가 가능
	/// </summary>
	public abstract class aAuth
	{
		public aAuth()
		{
			AppAuth.Auth.Check(this);
		}


		/// <summary>
		/// 클래스 사용 인증 정보를 추가 
		/// </summary>
		public static class Auth
		{
			/// <summary>
			/// 인증 정보를 추가 한다.
			/// </summary>
			/// <param name="key"></param>
			/// <returns></returns>		
			public static bool Add(string key)
			{
				return AppAuth.Auth.Add(key);
			}
		}

	}
}
