using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;


namespace Function.Util
{
	/// <summary>
	/// Key와 IV를 이용한 암호화/복호화 기능을 제공한다.
	/// </summary>
    public class cryptography
    {
		public cryptography()
		{
			_tdes = new TripleDESCryptoServiceProvider();
		}

        TripleDESCryptoServiceProvider _tdes;

		Encoding _encoding = Encoding.Default;
		/// <summary>
		/// 암호화에 사용할 인코딩 방식을 가져오거나 설정한다.
		/// </summary>
		public Encoding encoding
		{
			get { return _encoding; }
			set
			{
				_encoding = value;
			}
		}
		
		/// <summary>
        /// 암호화의 사용될 key를 가져오거나 설정한다.
        /// </summary>
        public string Key
        {
			get { return Convert.ToBase64String(_tdes.Key); }
            set
            {
				_tdes.Key = Convert.FromBase64String(value);
            }
        }

        /// <summary>
        /// 새로운 암호화 key를 생성한다.
        /// </summary>
        /// <returns></returns>
        public string GenKey()
        {
            _tdes.GenerateKey();

			return Key;
        }
        
        
        /// <summary>
        /// 암호화의 사용될 IV를 가져오거나 설정한다.
        /// </summary>
        public string IV
        {
			get { return Convert.ToBase64String(_tdes.IV); }
            set
            {
				_tdes.IV = Convert.FromBase64String(value);
            }
        }

        /// <summary>
        /// 새로운 암호화 IV를 생성한다.
        /// </summary>
        /// <returns></returns>
        public string GenIV()
        {
            _tdes.GenerateIV();

			return IV;
        }

		/// <summary>
		/// 암호화 한다.
		/// </summary>
		/// <param name="_strSouceString"></param>
		/// <returns></returns>
        public string Encrypting(string _strSouceString)
        {
			byte[] bytSource;

			using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
			{
				ICryptoTransform encrypto = _tdes.CreateEncryptor();
				CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);

				bytSource = encoding.GetBytes(_strSouceString);

				//if (encoding_codename == null)
				//	bytSource = Encoding.Default.GetBytes(_strSouceString);			//Convert.FromBase64String(_strSouceString); //encoding.GetBytes(_strSouceString);
				//else
				//	bytSource = Encoding.GetEncoding(encoding_codename).GetBytes(_strSouceString);

				cs.Write(bytSource, 0, bytSource.Length);
				cs.FlushFinalBlock();

				return Convert.ToBase64String(ms.ToArray());

				//return encoding.GetString(ms.ToArray());
			}
        }
		


		/// <summary>
		/// 복호화 한다.
		/// </summary>
		/// <param name="_strEncryptingString"></param>
		/// <returns></returns>
		public string Decrypting(string _strEncryptingString)
		{
			try
			{
				byte[] bytSource;

				bytSource = Convert.FromBase64String(_strEncryptingString);

				//bytSource = encoding.GetBytes(_strEncryptingString);

				/*
				System.IO.MemoryStream ms = new System.IO.MemoryStream(bytSource, 0, bytSource.Length);

				ICryptoTransform encrypto = _tdes.CreateDecryptor();
				CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

				System.IO.StreamReader sr = new System.IO.StreamReader(cs);

				return sr.ReadToEnd();
				*/

				using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytSource, 0, bytSource.Length))
				{
					ICryptoTransform encrypto = _tdes.CreateDecryptor();
					CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

					Encoding en;

					//if (encoding_codename == null)
					//	en = Encoding.Default;
					//else
					//	en = Encoding.GetEncoding(encoding_codename);


					using (System.IO.StreamReader sr = new System.IO.StreamReader(cs, encoding))
					{
						return sr.ReadToEnd();
					}


				}
			}
			catch
			{
				return string.Empty;
			}
			
		}

	




    }
}
