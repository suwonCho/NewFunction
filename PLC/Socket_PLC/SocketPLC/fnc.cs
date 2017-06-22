using System;
using System.Collections.Generic;
using System.Text;
using Function;
using System.Data;

namespace SocketPLC
{
	class fnc
	{
		public static DataTable LogDataTable_Create()
		{
			DataTable dt = new DataTable("Log");

			dt.Columns.Add("Time", typeof(String));
			dt.Columns.Add("Type", typeof(String));
			dt.Columns.Add("Log", typeof(String));
			dt.Columns.Add("Err", typeof(bool));

			return dt;
		}


		public static string BytesToAscii(byte[] bytByt, int idx = 0, int length = -1)
		{
			if (length < 0) length = bytByt.Length - idx;

			string strValue = string.Empty;

			strValue = ASCIIEncoding.ASCII.GetString(bytByt, idx, length);

			//for (int i = idx + length - 1 ; i >= idx; i -= 2)
			//{
			//    strValue += ASCIIEncoding.ASCII.GetString(bytByt, i - 1, 2);
			//}

			return strValue;
		}


		private static string _byteToString(byte[] yDATA, int idx = 0, int length = -1)
		{
			int yLen = length;

			if (length < 0) yLen = yDATA.Length;

			string strData = string.Empty;
			for (int i = idx; i < idx + yLen; i++)
			{
				strData += yDATA[i].ToString("X2");
			}

			return strData;
		}

		public static string ByteToString(byte[] bytByt, int idx = 0, int length = -1)
		{
			if (length < 0) length = bytByt.Length;

			string strByte = _byteToString(bytByt, idx, length);
			string strValue = string.Empty;



			for (int i = 0 ; i < strByte.Length; i += 2)
			{
				strValue += strByte.Substring(strByte.Length - (i + 2), 2);
			}

			return strValue;
		}

		public static long ByteToLong(byte[] bytByt, int idx = 0, int length = -1)
		{
			return Convert.ToInt64(ByteToString(bytByt, idx, length), 16);
		}


		public static int ByteToInt(byte[] bytByt, int idx = 0, int length = -1)
		{
			return Convert.ToInt32(ByteToString(bytByt, idx, length), 16);
		}

		/// <summary>
		/// 주소(데이터)타입을 적용하여 변경 처리 한다.
		/// </summary>
		/// <param name="addr"></param>
		/// <returns></returns>
		public static string Address_SetAddType(string addr, int addType)
		{

			if (addType < 2) return addr;

			string rtn = string.Empty;
			try
			{
				addr = addr.Trim();
				int idx = addr.Length;
				int add;

				for (int i = addr.Length - 1; i >= 0; i--)
				{
					if (Char.IsDigit(addr[i]))
						idx = i;
					else
						break;
				}

				add = int.Parse(addr.Substring(idx)) / addType;

				rtn = string.Format("{0}{1}", addr.Substring(0, idx), add);

				return rtn;
			}
			catch
			{
				return rtn;
			}

		}



		/// <summary>
		/// 다음 주소를 구한다.
		/// </summary>
		/// <param name="addr"></param>
		/// <returns></returns>
		public static string Address_NetGet(string addr, int nextStep = 1)
		{
			string rtn = string.Empty;
			try
			{
				addr = addr.Trim();
				int idx = addr.Length;
				int add;

				for (int i = addr.Length - 1; i >= 0; i--)
				{
					if (Char.IsDigit(addr[i]))
						idx = i;
					else
						break;
				}

				add = int.Parse(addr.Substring(idx)) + nextStep;

				rtn = string.Format("{0}{1}", addr.Substring(0, idx), add);

				return rtn;
			}
			catch
			{
				return rtn;
			}

		}

		/// <summary>
		/// 주소가 범위 안에 있는지 확인 한다.
		/// </summary>
		/// <param name="addr"></param>
		/// <param name="addrRange"></param>
		/// <param name="addrRange_Length"></param>
		/// <returns></returns>
		public static bool Address_InRange(string addr, string addrRange, int addrRange_Length)
		{
			stPLCAddress tar = new stPLCAddress(addr);
			stPLCAddress min = new stPLCAddress(addrRange);
			stPLCAddress max = min;
			max.Address += addrRange_Length - 1;

			return min <= tar && max >= tar;

		}

		/// <summary>
		/// plc 주소 처리 구조분
		/// </summary>
		public struct stPLCAddress
		{
			public string Header;
			public int Address;

			public stPLCAddress(string addr)
			{
				addr = addr.Trim();
				int idx = addr.Length;
				
				int add;

				for (int i = addr.Length - 1; i >= 0; i--)
				{
					if (Char.IsDigit(addr[i]))
						idx = i;
					else
						break;
				}

				Address = Fnc.obj2int(addr.Substring(idx));
				Header = addr.Substring(0, idx);
			}

			public static bool operator !=(stPLCAddress a, stPLCAddress b)
			{
				return !(a.Header.Equals(b.Header) && a.Address.Equals(b.Address));
			}


			public static bool operator ==(stPLCAddress a, stPLCAddress b)
			{
				return a.Header.Equals(b.Header) && a.Address.Equals(b.Address);				
			}

			public static bool operator >(stPLCAddress a, stPLCAddress b)
			{
				return a.Header.Equals(b.Header) && a.Address > b.Address;
			}

			public static bool operator <(stPLCAddress a, stPLCAddress b)
			{
				return a.Header.Equals(b.Header) && a.Address < b.Address;
			}

			public static bool operator >=(stPLCAddress a, stPLCAddress b)
			{
				return a.Header.Equals(b.Header) && a.Address >= b.Address;
			}

			public static bool operator <=(stPLCAddress a, stPLCAddress b)
			{
				return a.Header.Equals(b.Header) && a.Address <= b.Address;
			}

			public string ToString()
			{
				return String.Format("{0}{1}", Header, Address);
			}

		}




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


		public static void ByteSetIntValue(byte[] bytByt, int intStartLength, int intLength, int intValue)
		{
			byte[] bytSet = IntToByte(intValue, intLength);

			for (int i = intStartLength; i < intStartLength + intLength; i++)
			{
				bytByt[i] = bytSet[i - intStartLength];
			}

		}

        /// <summary>
        /// 문자열값을 plc 주소용 값으로 변환 하여 준다. ASCII 값으로 변경
        /// </summary>
        /// <param name="data">문자열</param>
        /// <param name="idx">가져올값의 문자열 index 길이는 2</param>
        /// <param name="nullValue">값이 없을경으 기본값</param>
        /// <returns></returns>
        public static int String2AddValue(string data, int idx, string nullValue = " ")
        {
            string hex;
            
            if(data.Length <= idx + 1)
                hex = char.ConvertToUtf32(nullValue, 0).ToString("X2");
            else
                hex = char.ConvertToUtf32(data, idx+1).ToString("X2");


            if (data.Length <= idx)
                hex = hex + char.ConvertToUtf32(nullValue, 0).ToString("X2");
            else
                hex = hex + char.ConvertToUtf32(data, idx).ToString("X2");


            return int.Parse(hex, System.Globalization.NumberStyles.HexNumber);


        }
	
	}
}
