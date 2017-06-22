using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Function;

namespace AppAuth
{
	/// <summary>
	/// 클래스 사용 인증 확인 클래스<para/>
	/// 사용하는 클래스와 생성자의 주석 앞에 '(인증필요)' 주석 추가
	/// </summary>
	public class Auth
	{
		/// <summary>
		/// 사용appName, 대상appName
		/// </summary>
		static Dictionary<string, string> _dicAuth = null;

		/// <summary>
		/// 로그 처리 클래스
		/// </summary>
		static Function.Util.Log _log = null;

		static Function.Util.cryptography _cryp = null;

		static readonly string _key = "6wNF/ww+7MMl8Xv5BIsR4FF7w+8RmjZ7";
		static readonly string _iv = "ugZGGm0WZNM=";

		/// <summary>
		/// 인증 정보를 추가 한다.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>		
		public static bool Add(string key)
		{
			try
			{
				if(_dicAuth == null)
				{
					_dicAuth = new Dictionary<string, string>();
				}

				if(_cryp == null)
				{
					_cryp = new Function.Util.cryptography();
					_cryp.Key = _key;
					_cryp.IV = _iv;
				}

				//사용appName:;대상appName
				string[] keys = _cryp.Decrypting(key).Split(new string[] { ":;" }, StringSplitOptions.RemoveEmptyEntries);

				if (keys.Length != 2) return false;

				if (_dicAuth.ContainsKey(keys[0]))
					_dicAuth[keys[0]] = keys[1];
				else
					_dicAuth.Add(keys[0], keys[1]);

				return true;
			}
			catch(Exception ex)
			{

				return false;
			}
		}

		/// <summary>
		/// 인증 정보를 체크 한다.
		/// </summary>
		/// <param name="obj">대상 클래스를 넘긴다. 보통 this로 넘기면 된다.</param>
		/// <returns></returns>
		public static bool Check(object obj)
		{

			//대상appName
			string value = obj.GetType().Name;

			//사용appName
			string key = Application.ProductName;


			Console.WriteLine("[AppAuth Check]{0}:;{1}", key, value);

			if (_dicAuth == null || _cryp == null)
			{
				throw new Exception("[Auth Check]인증 정보가 등록 되어 있지 않습니다.");
			}
					

			if (_dicAuth.ContainsKey(key) && _dicAuth[key].Equals(value))
			{
				return true;
			}		

			throw new Exception("[Auth Check]인증 정보가 등록 되어 있지 않습니다.");
			
			
		}

		static void WLog(string log)
		{
#if (!DEBUG)
			return;
#endif

		}

		static void WLog_Exception(string strModule, Exception ex)
		{
#if (!DEBUG)
			return;
#endif


		}




	}
}
