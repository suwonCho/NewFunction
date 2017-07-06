using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Function;

namespace PLCComm
{
	class fnc
	{

		/// <summary>
		/// int값을 Hex값으로 변환한다.
		/// </summary>
		/// <param name="intValue"></param>
		/// <returns></returns>
		public static string IntToHex(int intValue)
		{
			try
			{
				string strTemp = string.Format("{0:X4}", intValue);
				strTemp = strTemp.Substring(strTemp.Length - 4, 4);
				return strTemp;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Hex값을 Int로 변환한다.
		/// </summary>
		/// <param name="strValue"></param>
		/// <returns></returns>
		public static int HexToInt(string strValue)
		{
			return Convert.ToInt16(strValue, 16);
		}

		/// <summary>
		/// byte배열에 값(int)을 대입한다.
		/// </summary>
		/// <param name="bytByt"></param>
		/// <param name="intStartLength"></param>
		/// <param name="intLength"></param>
		/// <param name="intValue"></param>
		public static void ByteSetValue(byte[] bytByt, int intStartLength, int intLength, int intValue)
		{
			byte[] bytSet = IntToByte(intValue, intLength);

			for (int i = intStartLength; i < intStartLength + intLength; i++)
			{
				bytByt[i] = bytSet[i - intStartLength];
			}

		}


		/// <summary>
		/// int를 byte배열로 변환 한다.
		/// </summary>
		/// <param name="intValue"></param>
		/// <param name="intLength"></param>
		/// <returns></returns>
		public static byte[] IntToByte(int intValue, int intLength)
		{
			int IntLength = intLength * 2;
			byte[] bytValue = new byte[intLength];
			string strValue = intValue.ToString("X" + IntLength.ToString());

			for (int i = 0; i < IntLength; i += 2)
			{
				bytValue[i / 2] = Convert.ToByte(strValue.Substring(strValue.Length - (i + 2), 2), 16);
			}

			return bytValue;

		}


		/// <summary>
		/// byte 배열을 long으로 변환한다.
		/// </summary>
		/// <param name="bytByt"></param>
		/// <returns></returns>
		public static long ByteToLong(byte[] bytByt)
		{
			string strByte = ByteToHex(bytByt);
			string strValue = string.Empty;

			for (int i = 0; i < strByte.Length; i += 2)
			{
				strValue += strByte.Substring(strByte.Length - (i + 2), 2);
			}

			return Convert.ToInt16(strValue, 16);
		}
		

		/// <summary>
		/// 바이트 배열을 string숫자 연결 형태로
		/// </summary>
		/// <param name="b"></param>
		public static string ByteToHex(byte[] bb)
		{
			string temp = "";
			foreach (byte b in bb)
			{
				temp += b.ToString("X2"); //string.Format("{0:X2}", Convert.ToInt32(b)) + " ";
			}

			return temp;
		}

		/// <summary>
		/// Hex값을 Ascii 값 기준으로 문자열로 변경 하여 준다.
		/// </summary>
		/// <param name="hex"></param>
		/// <returns></returns>
		public static string HexToString(string hex)
		{
			string rtn = string.Empty;
			int val;

			while (!hex.Equals(string.Empty))
			{
				val = HexToInt(Fnc.StringGet(ref hex, 2));
				rtn += Convert.ToChar(val);
			}

			return rtn;

		}
	}
}
