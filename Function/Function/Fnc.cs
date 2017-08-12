using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Data;
using System.Reflection;

using System.Linq;

namespace Function
{
	public class Fnc
	{
		/// <summary>
		/// 날짜 형식
		/// </summary>
		public enum enDateType
		{
			/// <summary>
			/// yyyy-MM-dd HH:mm:ss
			/// </summary>
			DateTime,
			/// <summary>
			/// MM-dd HH:mm:ss
			/// </summary>
			DateTimeShort,
			/// <summary>
			/// yyyy-MM-dd
			/// </summary>
			Date,
			/// <summary>
			/// HH:mm:ss
			/// </summary>
			Time,
			/// <summary>
			/// yyyy
			/// </summary>
			Year,
			/// <summary>
			/// MM
			/// </summary>
			Month,
			/// <summary>
			/// dd
			/// </summary>
			Day,
			/// <summary>
			/// HH
			/// </summary>
			Hour,
			/// <summary>
			/// mm
			/// </summary>
			Minute,
			/// <summary>
			/// ss
			/// </summary>
			Second,
			/// <summary>
			/// yyyyMMdd
			/// </summary>
			DBType,
			/// <summary>
			/// yyyyMMddHHmmss
			/// </summary>
			DBTypeLong,
			/// <summary>
			/// HHmm
			/// </summary>
			HourMinutes
		};

		public enum BitSetType
		{
			On,
			Off,
			Toggle
		}


		/// <summary>
		/// right함수 구현
		/// </summary>
		/// <param name="str"></param>
		/// <param name="intLen"></param>
		/// <returns></returns>
		public static string Right(string str, int intLen)
		{
			return str.Substring(str.Length - intLen, intLen);
		}

		/// <summary>
		/// 시간을 문자열로 변경 하여 준다.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="enDType"></param>
		/// <returns></returns>
		public static string Date2String(DateTime dt, enDateType enDType)
		{
			string strRst = string.Empty;

			switch (enDType)
			{
				case enDateType.DateTime:
					strRst = dt.ToString("yyyy-MM-dd HH:mm:ss");
					break;

				case enDateType.DateTimeShort:
					strRst = dt.ToString("MM-dd HH:mm:ss");
					break;

				case enDateType.Date:
					strRst = dt.ToString("yyyy-MM-dd");
					break;

				case enDateType.Time:
					strRst = dt.ToString("HH:mm:ss");
					break;

				case enDateType.Year:
					strRst = dt.ToString("yyyy");
					break;

				case enDateType.Month:
					strRst = dt.ToString("MM");
					break;

				case enDateType.Day:
					strRst = dt.ToString("dd");
					break;

				case enDateType.Hour:
					strRst = dt.ToString("HH");
					break;

				case enDateType.Minute:
					strRst = dt.ToString("mm");
					break;

				case enDateType.Second:
					strRst = dt.ToString("ss");
					break;

				case enDateType.DBType:
					strRst = dt.ToString("yyyyMMdd");
					break;

				case enDateType.DBTypeLong:
					strRst = dt.ToString("yyyyMMddHHmmss");
					break;

				case enDateType.HourMinutes:
					strRst = dt.ToString("HHmm");
					break;

				default:
					strRst = dt.ToString("yyyy-MM-dd HH:mm:ss");
					break;
			}

			return strRst;
		}

		/// <summary>
		/// object를 날짜로 변환 : 기준이 없는 형식 부분은 현재 시간 기준으로 변환
		/// </summary>
		/// <param name="o"></param>
		/// <param name="enDType"></param>
		/// <returns></returns>
		public static DateTime obj2Date(object o, enDateType enDType)
		{
			string s = Fnc.obj2String(o);

			return String2Date(s, enDType);
		}


		/// <summary>
		/// string를 날짜로 변환 : 기준이 없는 형식 부분은 현재 시간 기준으로 변환
		/// </summary>
		/// <param name="strDate"></param>
		/// <param name="enDType"></param>
		/// <returns></returns>
		public static DateTime String2Date(string strDate, enDateType enDType)
		{
			DateTime drRst = DateTime.Now;

			int Y = drRst.Year;
			int M = drRst.Month;
			int D = drRst.Day;

			int H = drRst.Hour;
			int MI = drRst.Minute;
			int S = drRst.Second;



			switch (enDType)
			{
				case enDateType.DateTime:
					Y = int.Parse(strDate.Substring(0, 4));
					M = int.Parse(strDate.Substring(5, 2));
					D = int.Parse(strDate.Substring(8, 2));

					H = int.Parse(strDate.Substring(11, 2));
					MI = int.Parse(strDate.Substring(14, 2));
					S = int.Parse(strDate.Substring(17, 2));
					break;

				case enDateType.DateTimeShort:
					M = int.Parse(strDate.Substring(0, 2));
					D = int.Parse(strDate.Substring(3, 2));

					H = int.Parse(strDate.Substring(6, 2));
					MI = int.Parse(strDate.Substring(9, 2));
					S = int.Parse(strDate.Substring(12, 2));

					break;

				case enDateType.Date:
					Y = int.Parse(strDate.Substring(0, 4));
					M = int.Parse(strDate.Substring(5, 2));
					D = int.Parse(strDate.Substring(8, 2));
					break;

				case enDateType.Time:
					H = int.Parse(strDate.Substring(0, 2));
					MI = int.Parse(strDate.Substring(3, 2));
					S = int.Parse(strDate.Substring(6, 2));
					break;

				case enDateType.Year:
					Y = int.Parse(strDate);
					break;

				case enDateType.Month:
					M = int.Parse(strDate);
					break;

				case enDateType.Day:
					D = int.Parse(strDate);
					break;

				case enDateType.Hour:
					H = int.Parse(strDate);
					break;

				case enDateType.Minute:
					MI = int.Parse(strDate);
					break;

				case enDateType.Second:
					S = int.Parse(strDate);
					break;

				case enDateType.DBType:
					Y = int.Parse(strDate.Substring(0, 4));
					M = int.Parse(strDate.Substring(4, 2));
					D = int.Parse(strDate.Substring(6, 2));
					break;

				case enDateType.DBTypeLong:
					Y = int.Parse(strDate.Substring(0, 4));
					M = int.Parse(strDate.Substring(4, 2));
					D = int.Parse(strDate.Substring(6, 2));

					H = int.Parse(strDate.Substring(8, 2));
					MI = int.Parse(strDate.Substring(10, 2));
					S = int.Parse(strDate.Substring(12, 2));
					break;

				case enDateType.HourMinutes:
					Y = DateTime.Now.Year;
					M = int.Parse(strDate.Substring(0, 2));
					D = int.Parse(strDate.Substring(2, 2));
					break;



				default:
					Y = int.Parse(strDate.Substring(0, 4));
					M = int.Parse(strDate.Substring(5, 2));
					D = int.Parse(strDate.Substring(8, 2));

					H = int.Parse(strDate.Substring(11, 2));
					MI = int.Parse(strDate.Substring(14, 2));
					S = int.Parse(strDate.Substring(17, 2));
					break;
			}

			return new DateTime(Y, M, D, H, MI, S);
		}

		/// <summary>
		/// 문자열이 Date형식인지 여부를 검사한다.
		/// </summary>
		/// <param name="strDate"></param>
		/// <param name="enDType"></param>
		/// <returns></returns>
		public static bool StringIsDate(string strDate, enDateType enDType)
		{
			try
			{
				String2Date(strDate, enDType);
				return true;
            }
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 요일명 타입
		/// </summary>
		public enum enDayNameType { Kor, Kor_Long, Eng, Eng_Long, Han, Han_Long };

		/// <summary>
		/// 요일명을 구한다..
		/// </summary>
		/// <param name="DayNameType"></param>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string DayName_Get(enDayNameType DayNameType, DateTime dt)
		{
			string strFormat = string.Empty;
			string strCulture = Culture_Get(DayNameType);

			switch (DayNameType)
			{

				case enDayNameType.Eng_Long:
				case enDayNameType.Kor_Long:
				case enDayNameType.Han_Long:
					strFormat = "dddddd";
					break;

				default:
					strFormat = "ddd";
					break;

			}


			return dt.ToString(strFormat, new System.Globalization.CultureInfo(strCulture));


		}

		/// <summary>
		/// 달명을 구한다.  
		/// </summary>
		/// <param name="DayNameType"></param>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string MonthName_Get(enDayNameType DayNameType, DateTime dt)
		{
			string strFormat = string.Empty;
			string strCulture = Culture_Get(DayNameType);

			switch (DayNameType)
			{

				case enDayNameType.Eng_Long:
				case enDayNameType.Kor_Long:
				case enDayNameType.Han_Long:
					strFormat = "MMMMMM";
					break;

				default:
					strFormat = "MMM";
					break;

			}


			return dt.ToString(strFormat, new System.Globalization.CultureInfo(strCulture));


		}






		private static string Culture_Get(enDayNameType DayNameType)
		{
			string strCulture = string.Empty;

			switch (DayNameType)
			{
				case enDayNameType.Eng:
				case enDayNameType.Eng_Long:
					strCulture = "en-US";
					break;


				case enDayNameType.Han_Long:
				case enDayNameType.Han:
					strCulture = "ja-JP";
					break;

				default:
					strCulture = "ko-KR";
					break;

			}


			return strCulture;
		}





		/// <summary>
		/// object값을 string으로 변환하여 준다.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string obj2String(object obj)
		{
			if (obj == null)
				return string.Empty;
			else
			{				
				return obj.ToString().Trim();
			}
		}


		/// <summary>
		/// byte 배열을 string으로 변환하여 반환한다.
		/// </summary>
		/// <param name="byt"></param>
		/// <returns></returns>
		public static string Bytes2String(byte[] byt)
		{
			string str = string.Empty;

			foreach (byte bt in byt)
			{
				str += string.Format("{0:x2} ", bt);
			}

			str = str.Trim();

			return str;
		}

		/// <summary>
		/// object값을 int으로 변환하여 준다.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static int obj2int(object obj)
		{
			try
			{
				if (obj == null || obj.ToString() == string.Empty)
					return 0;
				else
				{
					if (isNumeric(obj.ToString()))
						return Convert.ToInt32(string.Format("{0:F0}", obj));
					else
						return Convert.ToInt32(obj.ToString());
				}
			}
			catch
			{
				return 0;
			}
		}


		/// <summary>
		/// Byte 배열을 Hex 스트링으로 변환(Hex 스트링 사이에 구분문자 입력 처리 추가) 
		/// </summary>
		/// <param name="bytePacket"></param>
		/// <param name="cDelimiter"></param>
		/// <returns></returns>
		public static string ByteArray2HexString(Byte[] bytePacket, string strDelimiter)
		{
			string sReturn = "";
			try
			{
				int nCount = bytePacket.Length;

				for (int i = 0; i < nCount; i++)
				{
					if (i == 0)
						sReturn += String.Format("{0:X2}", bytePacket[i]);
					else
						sReturn += String.Format("{0}{1:X2}", strDelimiter, bytePacket[i]);
				}
			}
			catch
			{
				sReturn = "";
			}
			return sReturn;
		}


		/// <summary>
		///  object값을 long으로 변환하여 준다.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static long obj2Long(object obj)
		{
			try
			{
				if (obj == null || obj.ToString() == string.Empty)
					return 0;
				else
					return Convert.ToInt64(obj);
			}
			catch
			{
				return 0;
			}
		}


		/// <summary>
		///  object값을 Double으로 변환하여 준다.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static double obj2Double(object obj)
		{
			try
			{
				if (obj == null || obj.ToString() == string.Empty)
					return 0;
				else
					return Convert.ToDouble(obj);
			}
			catch
			{
				return 0;
			}
		}


		/// <summary>
		///  object값을 float으로 변환하여 준다.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static float obj2Float(object obj)
		{
			try
			{
				if (obj == null || obj.ToString() == string.Empty)
					return 0;
				else
					return float.Parse(obj.ToString());
			}
			catch
			{
				return 0;
			}
		}


		/// <summary>
		/// object값을 boolean으로 변환하여 준다.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool obj2Bool(object obj)
		{
			if (obj == null || obj.ToString() == string.Empty)
				return false;
			else
				return bool.Parse(obj.ToString());
		}





		/// <summary>
		/// hex string으로 Color를 만든다.
		/// </summary>
		/// <param name="strColorHex"></param>
		/// <returns></returns>
		public static Color String2Color(string strColorHex)
		{
			if (strColorHex.Length != 6) return Color.White;

			try
			{
				int intR = Convert.ToInt32(strColorHex.Substring(0, 2), 16);
				int intG = Convert.ToInt32(strColorHex.Substring(2, 2), 16);
				int intB = Convert.ToInt32(strColorHex.Substring(4, 2), 16);


				return Color.FromArgb(intR, intG, intB);
			}
			catch
			{
				return Color.White;
			}
		}

		/// <summary>
		/// hex string으로 Color를 만든다.
		/// </summary>
		/// <param name="strColorHex"></param>
		/// <param name="DefaultColor">에러 발생시 리턴 컬러</param>
		/// <returns></returns>
		public static Color String2Color(string strColorHex, Color DefaultColor)
		{
			if (strColorHex.Length != 6) return Color.White;

			try
			{
				int intR = Convert.ToInt32(strColorHex.Substring(0, 2), 16);
				int intG = Convert.ToInt32(strColorHex.Substring(2, 2), 16);
				int intB = Convert.ToInt32(strColorHex.Substring(4, 2), 16);


				return Color.FromArgb(intR, intG, intB);
			}
			catch
			{
				return DefaultColor;
			}
		}

		/// <summary>
		/// 문자열에 포함되어 있는 검색 문자 수를 리턴한다.
		/// </summary>
		/// <param name="strData">문자열</param>
		/// <param name="strFind">검색할 문자</param>
		/// <returns></returns>
		public static int inStr(string strData, string strFind)
		{
			int intCnt = 0;
			int intResult = 0;

			while (intResult >= 0 && (intResult + 1) < strData.Length)
			{
				intResult = strData.IndexOf(strFind, intResult + 1);
				if (intResult > 0) intCnt++;
			}

			return intCnt;
		}

		/// <summary>
		/// 문자열에 문자열을 더한다.
		/// </summary>
		/// <param name="strData">더할 대상 문자</param>
		/// <param name="strAddData">더할 문자</param>
		/// <param name="strSpreator">전면구분자</param>
		/// <param name="strSpreatorBack">후면구분다</param>
		/// <returns></returns>
		public static string StringAdd(string strData, string strAddData, string strSpreator, string strSpreatorBack = "")
		{
			if (strData == string.Empty)
				strData = strAddData;
			else if (strAddData != string.Empty)
				strData += strSpreator + strAddData + strSpreatorBack;

			return strData;
		}

		/// <summary>
		/// 문자배열을 문자로 반환한다.
		/// </summary>
		/// <param name="strData">문자배열</param>
		/// <param name="strSpreator">문자열구분자</param>
		/// <param name="StartIndex">시작 Index</param>
		/// <param name="Length">길이 -1로 설정시 최대 길이</param>
		/// <returns></returns>
		public static string StringArray2String(string[] strData, string strSpreator = "", int StartIndex = 0, int Length = -1)
		{
			int endIndex = StartIndex + Length;
			string rst = string.Empty;

			if (Length <= -1)
			{
				endIndex = strData.Length;
			}

			for (int i = StartIndex; i < endIndex; i++)
			{
				if (i >= strData.Length) break;

				rst = StringAdd(rst, strData[i], strSpreator);
			}

			return rst;




		}


		/// <summary>
		/// DateTime의 날짜 부분과 timespan의 시간 부분을 합친다.
		/// c1 input control의 datepicker와 timepicker 합칠 사용하면 유용.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="ts"></param>
		/// <returns></returns>
		public static DateTime DateTimeMergeTimeSpan(DateTime dt, TimeSpan ts)
		{
			return new DateTime(dt.Year, dt.Month, dt.Day,
								ts.Hours, ts.Minutes, ts.Seconds);

		}




		/// <summary>
		/// 리소스에서 Stream을 추출한다.
		/// </summary>
		/// <param name="thisExe">'System.Reflection.Assembly.GetExecutingAssembly()' 내용 그대로 넘길것..</param>
		/// <param name="strResourceName">네임스페이스를 포함한 이름 ex)네임스페이스.이름</param>
		/// <returns></returns>
		public static System.IO.Stream GetResource2Stream(System.Reflection.Assembly thisExe, string strResourceName)
		{
			//thisExe = System.Reflection.Assembly.GetExecutingAssembly();

			//string[] resources = thisExe.GetManifestResourceNames();

			return thisExe.GetManifestResourceStream(strResourceName);
		}

		/// <summary>
		/// 리소스에서 스트링 추출 한다
		/// </summary>
		/// <param name="thisExe">'System.Reflection.Assembly.GetExecutingAssembly()' 내용 그대로 넘길것..</param>
		/// <param name="strResourceName">네임스페이스를 포함한 이름 ex)네임스페이스.이름</param>
		/// <returns></returns>
		public static string GetResource2string(System.Reflection.Assembly thisExe, string strResourceName)
		{
			System.IO.Stream st = GetResource2Stream(thisExe, strResourceName);

			int intLength = Convert.ToInt32(st.Length);

			Byte[] byt = new byte[intLength];

			st.Read(byt, 0, intLength);

			return Encoding.Default.GetString(byt);

		}



		/// <summary>
		/// 2개에 바이트 배열을 합쳐준다.
		/// </summary>
		/// <param name="byt01"></param>
		/// <param name="byt02"></param>
		/// <returns></returns>
		public static byte[] BytesMerge(byte[] byt01, byte[] byt02)
		{
			//총 바이트 길이 확인
			int intLen = byt01.Length + byt02.Length;

			byte[] byt = new byte[intLen];

			Array.Copy(byt01, 0, byt, 0, byt01.Length);

			Array.Copy(byt02, 0, byt, byt01.Length, byt02.Length);

			return byt;

		}

		/// <summary>
		/// 신호에 Etx 추가 - 길이, 체크섬, etx
		/// </summary>
		/// <param name="byt"></param>
		/// <returns></returns>
		public static byte[] BytesSetEtx(byte[] byt)
		{
			int intLen = byt.Length;
			string strLen = string.Format("{0:x4}", intLen);
			byte[] bt = new byte[6];

			bt[0] = Convert.ToByte(strLen.Substring(0, 2), 16);
			bt[1] = Convert.ToByte(strLen.Substring(2, 2), 16);
			bt[2] = GetCheckSum(byt, 0, intLen);
			bt[3] = 0x03; //etx
			bt[4] = 0x0D; //CR
			bt[5] = 0x0A; //LF

			return BytesMerge(byt, bt);
		}

		/// <summary>
		/// 2개의 바이트 배열의 일부구간을 서로 비교하여 같은지 확인 한다.
		/// </summary>
		/// <param name="Source_Bytes"></param>
		/// <param name="Source_Bytes_ST_Idx"></param>
		/// <param name="Target_Bytes"></param>
		/// <param name="Target_Bytes_ST_Idx"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static bool bytesEqual(byte[] Source_Bytes, int Source_Bytes_ST_Idx, byte[] Target_Bytes, int Target_Bytes_ST_Idx, int length)
		{
			bool rtn = true;
			int sor_idx = Source_Bytes_ST_Idx;
			int trg_idx = Target_Bytes_ST_Idx;

			for (int idx = 0; idx < length; idx++)
			{
				if (Source_Bytes.Length <= sor_idx || Target_Bytes.Length <= trg_idx)
				{
					rtn = false;
					break;
				}


				if (Source_Bytes[sor_idx] != Target_Bytes[trg_idx])
				{
					rtn = false;
					break;
				}


				sor_idx++;
				trg_idx++;
			}

			return rtn;


		}



		/// <summary>
		/// 체크섬을 생성한다.
		/// </summary>
		/// <param name="byt"></param>
		/// <param name="intFromIndex">시작 인덱스</param>
		/// <param name="intLength">길이</param>
		/// <returns></returns>
		public static byte GetCheckSum(byte[] byt, int intFromIndex, int intLength)
		{

			long lngChk = 0;

			for (int i = intFromIndex; i < intFromIndex + intLength; i++)
			{
				lngChk += byt[i];
			}

			string strChk = string.Format("{0:x2}", lngChk);
			strChk = strChk.Substring(strChk.Length - 2, 2);
			return Convert.ToByte(strChk, 16);

		}

		/// <summary>
		/// byte배열을 string으로 반환 한다. 시작index =0, 길이 =0 으로 설정시 전체를 변환
		/// </summary>
		/// <param name="byt"></param>
		/// <param name="intFromIndex"></param>
		/// <param name="intLength"></param>
		/// <returns></returns>
		public static String Bytes2String(byte[] byt, int intFromIndex, int intLength)
		{
			if (intFromIndex == 0 && intLength == 0)
				intLength = byt.Length;

			return Encoding.Default.GetString(byt, intFromIndex, intLength);
		}

		/// <summary>
		/// byte배열을 int로 반환 한다. 시작index =0, 길이 =0 으로 설정시 전체를 변환
		/// </summary>
		/// <param name="byt"></param>
		/// <param name="intFromIndex"></param>
		/// <param name="intLength"></param>
		/// <returns></returns>
		public static int Bytes2Int(byte[] byt, int intFromIndex, int intLength)
		{
			if (intFromIndex == 0 && intLength == 0)
				intLength = byt.Length;

			string str = string.Empty;
			for (int i = intFromIndex; i < intFromIndex + intLength; i++)
			{
				str += string.Format("{0:x2}", byt[i]);
			}

			return Convert.ToInt32(str, 16);


		}


		/// <summary>
		/// string 배열을 byt배열로 만들어 준다.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="isWithLength">byt앞에 길이 바이트 추가 여부.</param>
		/// <returns></returns>
		public static byte[] String2Byte(string[] str, bool isWithLength)
		{
			byte[] byt = new byte[0];

			foreach (string s in str)
			{
				if (s == null) break;

				byte[] bytData = Encoding.Default.GetBytes(s);

				if (isWithLength)
				{
					byte[] bytLen = new byte[2];

					int i = bytData.Length;
					string sLen = string.Format("{0:x4}", i);

					bytLen[0] = Convert.ToByte(sLen.Substring(0, 2), 16);
					bytLen[1] = Convert.ToByte(sLen.Substring(2, 2), 16);

					bytData = BytesMerge(bytLen, bytData);

				}

				byt = BytesMerge(byt, bytData);
			}

			return byt;
		}


		/// <summary>
		/// wav 파일을 재생한다.
		/// </summary>
		/// <param name="strFilePath"></param>
		public static void PlayWaveSound(string strFilePath)
		{
			try
			{
				System.Media.SoundPlayer p = new System.Media.SoundPlayer(strFilePath);

				p.Play();
			}
			catch(Exception ex)
			{
			}
		}






		/// <summary>
		/// 문자열이 숫자 인지 검사 한다
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool isNumeric(string str)
		{

			if (str == string.Empty) return false;

			foreach (char c in str.ToCharArray())
			{
				if (!Char.IsDigit(c) && !'.'.Equals(c))
					return false;
			}

			return true;

		}

		/// <summary>
		/// object가 숫자 인지 검사 한다
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool isNumeric(object str)
		{

			return isNumeric(Fnc.obj2String(str));

		}

		/// <summary>
		/// 데이타 테이블 2개를 비교 하여 데이터가 다른지 여부 확인한다
		/// </summary>
		/// <param name="dt1"></param>
		/// <param name="dt2"></param>
		/// <returns></returns>
		public static bool DataTables_isEquals(DataTable dt1, DataTable dt2)
		{
			//null 확인
			if (dt1 == null || dt2 == null) return false;

			//row, col 수 같은지 비교
			if (dt1.Rows.Count != dt2.Rows.Count || dt1.Columns.Count != dt2.Columns.Count)
				return false;

			int intRows = dt1.Rows.Count;
			int intCols = dt1.Columns.Count;

			//각 field에 데이터 확인...
			for (int intRow = 0; intRow < intRows; intRow++)
			{
				for (int intCol = 0; intCol < intCols; intCol++)
				{
					if (dt1.Rows[intRow][intCol].ToString() != dt2.Rows[intRow][intCol].ToString()) return false;
				}
			}


			return true;

		}



		/// <summary>
		/// 데이터테이블 컬럼을 추가 한다 - 같은 이름이 있을경우 추가하지 않는다
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="cols"></param>
		/// <param name="types"></param>
		/// <returns>추가여부를 반환한다</returns>
		public static bool DataTable_ColumnsAdd(DataTable dt, string[] cols, Type[] types, object[] DefaultValue)
		{
			bool rtn = false;
			int idx = 0;
			Type typ;
			object dValue;

			foreach (string col in cols)
			{
				if (!dt.Columns.Contains(col))
				{
					rtn = true;

					if (types == null || types.Length <= idx)
						typ = null;
					else
						typ = types[idx];


					dt.Columns.Add(col, typ);

					if (DefaultValue == null || DefaultValue.Length <= idx)
						dValue = null;
					else
						dValue = DefaultValue[idx];

					foreach(DataRow dr in dt.Rows)
					{
						dr[col] = dValue;
					}


				}

				idx++;
			}


			return rtn;

		}



		/// <summary>
		/// 데이터테이블 컬럼을 추가 한다 - 같은 이름이 있을경우 추가하지 않는다
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="cols"></param>
		/// <param name="types"></param>
		/// <returns>추가여부를 반환한다</returns>
		public static bool DataTable_ColumnsAdd(DataTable dt, string[] cols, string[] types, object[] DefaultValue)
		{

			Type[] type = new Type[types.Length];
			int idx = 0;
			foreach(string t in types)
			{
				type[idx] = Type.GetType(t);
				idx++;
			}


			return DataTable_ColumnsAdd(dt, cols, type, DefaultValue);
		}



		/// <summary>
		/// enum을 string 배열로 변환하여 반화 한다.
		/// </summary>
		/// <param name="em">열거형을 new로 생성 하여 입력</param>
		/// <returns>열거형 값을 string 배열</returns>
		public static string[] EnumItems2Strings(Enum em)
		{
			Type type = em.GetType();
			Array arr = Enum.GetValues(type);
			string[] items = new string[arr.Length];

			int i = 0;
			foreach (object e in Enum.GetValues(type))   //Parity.GetType()))
			{
				items[i] = e.ToString();
				i++;
			}

			return items;
		}



		/// <summary>
		/// string 값을 enum item으로 변경한다.
		/// </summary>
		/// <param name="em">열거형을 new로 생성 하여 입력</param>
		/// <param name="strItem">item의 string 값</param>
		/// <returns>object 열거형으로 형변환 하여 사용</returns>
		public static object enumItem2Object(Enum em, string strItem)
		{
			Type type = em.GetType();

			object o = null;

			//foreach (object e in Enum.GetValues(type))
			//{
			//    if (strItem == e.ToString())
			//    {
			//        o = e;
			//        break;
			//    }
			//}

			return Enum.Parse(em.GetType(), strItem);

		}




		public static object String2Enum(Enum em, string strValue)
		{

			strValue = strValue.ToUpper();

			Type type = em.GetType();


			foreach (object e in Enum.GetValues(type))   //Parity.GetType()))
			{
				if (strValue == e.ToString().ToUpper())
				{
					return e;
				}
			}

			return null;

		}


		/// <summary>
		/// 숫자를 올림 처리 한다.
		/// </summary>
		/// <param name="dValue"></param>
		/// <param name="intDigit"></param>
		/// <returns></returns>
		public static double dblToRoundUp(double dValue, int intDigit)
		{
			double dCoef = System.Math.Pow(10, intDigit);

			return dValue > 0 ? System.Math.Ceiling(dValue * dCoef) / dCoef : System.Math.Floor(dValue * dCoef) / dCoef;

		}


		/// <summary>
		/// Double을 int로 변경 하여 준다.
		/// </summary>
		/// <param name="dValue"></param>
		/// <returns></returns>
		public static int dblToInt(double dValue)
		{
			return int.Parse(dValue.ToString());
		}


		public static int decimalToInt(decimal dValue)
		{
			return int.Parse(dValue.ToString());
		}

		/// <summary>
		/// float을 int로 변경 하여 준다.
		/// </summary>
		/// <param name="fValue"></param>
		/// <returns></returns>
		public static int floatToInt(float fValue)
		{
			return int.Parse(fValue.ToString("F0"));
		}


		/// <summary>
		/// alpha값이 있는 color를 string값으로 변환 : 형태 - a,r,g,b
		/// </summary>
		/// <param name="col"></param>
		/// <returns></returns>
		public static String AColor2String(Color col)
		{
			return string.Format("{0},{1},{2},{3}", col.A, col.R, col.G, col.B);
		}


		/// <summary>
		/// string 값을 Alpha Color값으로 변환
		/// </summary>
		/// <param name="str">형태 - a,r,g,b</param>
		/// <returns></returns>
		public static Color String2AColor(string str)
		{
			string[] s = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

			return Color.FromArgb(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]), int.Parse(s[3]));
		}

		/// <summary>
		/// Font를 string 값으로 변환 : 형태 - fontname, size, style
		/// </summary>
		/// <param name="fnt"></param>
		/// <returns></returns>
		public static string Font2String(Font fnt)
		{
			return string.Format("{0},{1},{2}", fnt.Name, fnt.Size, fnt.Style);
		}


		/// <summary>
		/// string값을 Font로 변환
		/// </summary>
		/// <param name="str">형태 - fontname, size, style</param>
		/// <returns></returns>
		public static Font String2Font(string str)
		{
			string[] s = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

			return new Font(s[0], float.Parse(s[1]), (FontStyle)String2Enum(new FontStyle(), s[2]));
		}

		/// <summary>
		/// 비트 값을 설정 합니다.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="bit"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static int SetBit(int value, int bit, BitSetType type)
		{
			int b = 1 << bit;

			int iOn = (value & b);

			if(iOn > 0)
			{
				if(type == BitSetType.Off || type == BitSetType.Toggle)
				{
					value = value - iOn;
				}
			}
			else
			{
				if (type == BitSetType.On || type == BitSetType.Toggle)
				{
					value = value + (1 << bit);
				}
			}

			return value;
		}


		/// <summary>
		/// 비트 On 여부를 가져 온다.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="bit"></param>
		/// <returns></returns>
		public static bool GetBitOn(int value, int bit)
		{
			int b = 1 << bit;

			return (value & b) > 0;

		}

		/// <summary>
		/// Object의 프로퍼티로 데이터테이블 스키마를 만들어 준다.
		/// </summary>
		/// <param name="DataTableName"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static DataTable DataTable_SchemaByObject(string DataTableName, object obj)
		{
			DataTable dt = new DataTable();
			dt.TableName = DataTableName;

			List<Type> lst = new List<Type>();

			lst.Add(typeof(System.Int32));
			lst.Add(typeof(System.Int32?));
			lst.Add(typeof(System.Int64));
			lst.Add(typeof(System.Int64?));
			lst.Add(typeof(System.String));
			lst.Add(typeof(System.Boolean));

			DataColumn col;
			Type typ;
			

			foreach (PropertyInfo i in	obj.GetType().GetProperties())
			{
				Console.WriteLine(i.Name);
				if (!lst.Contains(i.PropertyType)) continue;

				typ = i.PropertyType;

				if (typ.Equals(typeof(System.Int32?)))
					typ = typeof(System.Int32);
				else if (typ.Equals(typeof(System.Int64?)))
					typ = typeof(System.Int64);

				col = new DataColumn(i.Name, typ);
				dt.Columns.Add(col);
			}

			return dt;

		}


		/// <summary>
		/// object IEnumerable 데이터를 DataTable에 넣는다
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="obj"></param>
		public static void DataTable_InsDataFromObject(DataTable dt, object obj)
		{
			

			//IEnumerator<object> ie = obj as IEnumerator<object>;
			IEnumerable<object> ie = obj as IEnumerable<object>;
			if (ie == null) return;

			DataRow dr;

			foreach (object c in ie)
			{

				try
				{
					dr = dt.NewRow();

					DataRow_InsDataFromObject(dr, c);

					dt.Rows.Add(dr);

				}
				catch
				{

				}

			}
		}



		/// <summary>
		/// object 내용을 DataRow에 입력 하여 준다.
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="obj"></param>
		public static void DataRow_InsDataFromObject(DataRow dr, object obj)
		{
			object value;
			foreach(DataColumn c in dr.Table.Columns)
			{

				try
				{
					value = DFnc.Property_Get_Value(obj, c.ColumnName);

					dr[c.ColumnName] = value;
				}
				catch
				{

				}

			}
		}

		/// <summary>
		/// 문자열에서 앞/뒤 에서 원하는 위치에서 문자열을 반환하고 해당 문자열은 원본에서 삭제 한다.
		/// </summary>
		/// <param name="txt">원본 문자열</param>
		/// <param name="length">길이</param>
		/// <param name="startIdx">위치</param>
		/// <param name="loc">앞/뒤</param>
		/// <returns></returns>
		public static string StringGet(ref string txt, int length, int startIdx = 0, enStringLocation loc = enStringLocation.Front)
		{
			
			string rtn = string.Empty;
			int x, y;

			if(txt.Length < length)
			{
				rtn = txt;
				txt = string.Empty;
				return rtn;
			}


			switch (loc)
			{
				case enStringLocation.Front:
					rtn = txt.Substring(startIdx, length);
					txt = txt.Substring(0, startIdx) + txt.Substring(startIdx + length);
					break;
                    
				case enStringLocation.End:
					x = txt.Length - startIdx;
					if (x + length > txt.Length)
						y = txt.Length - x;
					else
						y = length;

					rtn = txt.Substring(x, y);
					txt = txt.Substring(0, x) + txt.Substring(x + y);
					break;

				default:
					break;
			}
            
			return rtn;
			
		}

		/// <summary>
		/// 문자열을 원하는 숫자 만큼 반복하여 연결 후 리턴한다.
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="cnt"></param>
		/// <returns></returns>
		public static string StringRepeat(string txt, int cnt)
		{
			string rtn = string.Empty;

			for(int i=0; i<cnt;i++)
			{
				rtn += txt;
			}

			return rtn;
		}

		/// <summary>
		/// 텍스트 내에 검색어의 갯수를 찾아 내어 준다.
		/// </summary>
		/// <param name="txt">텍스트</param>
		/// <param name="searchText">검색어</param>
		/// <param name="stIdx">찾기 시작할 index</param>
		/// <param name="compareCase">비교 조건</param>
		/// <returns></returns>
		public static int TextSearchCnt(string txt, string searchText, int stIdx = 0, StringComparison compareCase = StringComparison.InvariantCultureIgnoreCase)
		{
			int idx = stIdx;
			int cnt = 0;

			while(true)
			{
				idx = txt.IndexOf(searchText, idx, compareCase);

				if (idx < 0) break;

				cnt++;
				idx += searchText.Length;									

			}

			return cnt;

		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="txt">텍스트</param>
		/// <param name="searchText">검색어</param>
		/// <param name="stIdx">찾기 시작할 index</param>
		/// <param name="Last2Start">끝까지 검색후 처음부터 검색 여부</param>
		/// <param name="isForward">방향 true:앞 false:뒤</param>
		/// <param name="compareCase">비교 조건</param>
		/// <returns></returns>
		public static int TextSearch(string txt, string searchText, int stIdx = 0, bool isForward = true, bool Last2Start = false, StringComparison compareCase = StringComparison.InvariantCultureIgnoreCase)
		{
			int idx;

			if(isForward)
			{
				//앞으로 검색일 경우
				while(true)
				{
					idx = txt.IndexOf(searchText, stIdx, compareCase);

					if (idx >= 0 || !Last2Start)		//찾았거나, 앞부터재검색이 아니면
						break;
					else if (Last2Start && stIdx != 0)	//앞부터 재검색일 경우 첨음부터 검색이 아니면 첨부터 재검색
						stIdx = 0;
					else
						break;
						
				}
			}
			else
			{
				//뒤로 검색
				int lstIdx = -100;

				idx = -1;
				//시작 인덱스 정리
				stIdx = stIdx != 0 ? stIdx : txt.Length;

				while(true)
				{
					lstIdx = txt.IndexOf(searchText, (lstIdx == -100 ? 0 : lstIdx + searchText.Length), compareCase);

					if(lstIdx >= 0)
					{   //찾았을 경우

						if (lstIdx < stIdx)
							//기준 인덱스 보다 전에 있음
							idx = lstIdx;
						else
						{
							//기준 인덱스 보다 후에 있음
							if (idx >= 0)
								//찾은게 있음 - 찾기 종료
								break;
							//찾았으나 기준보다 뒤에 있을경우
							else if (Last2Start && idx < 0)
							{
								if (stIdx == txt.Length) break;
								
								//재검색일 경우
								lstIdx = -100;
								stIdx = txt.Length;

							}

						}


					}
					else
					{   //못찾을 경우

						break; //찾기 종료						
					}

					
				}


			}
			
			return idx;
		}


		/// <summary>
		/// value와 comp값을 비교하여 그순서에 맛는 values의 값을 리턴한다.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="comp"></param>
		/// <param name="values"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static object ArrayGetIndexValue(object value, object[] comp, object[] values, object defaultValue)
		{

			object rtn = null;
			int idx = 0;
			try
			{
				switch(value.GetType().ToString())
				{
					case "System.String":
						foreach (object c in comp)
						{
							if (c.ToString().Equals(value.ToString()))
							{
								rtn = values[idx];
								break;
							}

							idx++;
						}
						break;

					case "System.Int16":
					case "System.Int32":
					case "System.Int64":
					case "System.Double":
					case "System.Single":
						foreach (object c in comp)
						{
							double c1 = (double)c;
							double c2 = (double)value;
							
							if (c1.Equals(c2))
							{
								rtn = values[idx];
								break;
							}

							idx++;
						}
						break;


					default:
						foreach(object c in comp)
						{
							if(c.Equals(value))
							{
								rtn = values[idx];
								break;
							}

							idx++;
						}
						break;
				}


				if (rtn == null) rtn = defaultValue;
				return rtn;
			}
			catch
			{
				return defaultValue;
			}
		}



	}	//end Fnc Class
}
