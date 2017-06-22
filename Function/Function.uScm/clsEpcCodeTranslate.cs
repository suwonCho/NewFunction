using System;
using System.Collections.Generic;
using System.Text;
using Function.uScm.WebReference;


namespace Function.uScm
{
    /// <summary>
    /// UPN EPC CODE 변화 - WEB SVR. 이용
    /// </summary>
	public class clsEpcCodeTranslate
	{
		UPNTDTService clsSvr = new UPNTDTService();

		public clsEpcCodeTranslate(string Url)
		{
			clsSvr.Url = Url;
		}

		public string translateHexaToCode(string strHexCode)
		{
			CodeSpecType cs = new CodeSpecType();

			cs.CodeString = strHexCode;
			cs.TagLength = "96";

			CodeResultType[] cr = clsSvr.translateHexaToCode(new CodeSpecType[] { cs });

			Console.WriteLine(cr[0].ResultString);

			return RemoveCode(cr[0].ResultString);

			//return "";
		}


		public string translateBinToCode(string strBinCode)
		{
			CodeSpecType cs = new CodeSpecType();

			cs.CodeString = strBinCode;
			cs.TagLength = "96";

			CodeResultType[] cr = clsSvr.translateBinToCode(new CodeSpecType[] { cs });

			Console.WriteLine(cr[0].ResultString);

			return cr[0].ResultString;

		}

		public string translateCodeToBin(string strCode)
		{
			CodeSpecType cs = new CodeSpecType();

			cs.CodeString = strCode;
			cs.TagLength = "96";

			CodeResultType[] cr = clsSvr.translateTagIdToBin(new CodeSpecType[] { cs });

			Console.WriteLine(cr[0].ResultString);

			return cr[0].ResultString;

		}
		/// <summary>
		/// tag id에서 길이와 체크value를 제거..
		/// </summary>
		/// <param name="strTagid"></param>
		/// <returns></returns>
		public static string RemoveCode(string strTagid)
		{	//urn:epc:tag:sscc-96:0.95100082.200000001
			string[] s = strTagid.Split(new string[] { ":" }, StringSplitOptions.None);

			s[2] = "id";

			s[3] = s[3].Substring(0, s[3].Length - 3);

			s[4] = s[4].Substring(2);

			string strRst = string.Empty;

			foreach (string str in s)
			{
				if (strRst != string.Empty) strRst += ":";

				strRst += str;

			}

			return strRst;			
		}

		/// <summary>
		/// tag id에서 길이와 체크value를 추가
		/// </summary>
		/// <param name="strTagid"></param>
		/// <returns></returns>
		public static string AddCode(string strTagid)
		{	//urn:epc:id:sscc:95100082.200000001
			string[] s = strTagid.Split(new string[] { ":" }, StringSplitOptions.None);

			s[2] = "tag";

			s[3] += "-96";

			s[4] = "0." + s[4];

			string strRst = string.Empty;

			foreach (string str in s)
			{
				if (strRst != string.Empty) strRst += ":";

				strRst += str;

			}

			return strRst;
		}



	}
}
