using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Data;


namespace AutoUpdateClient
{
	public class Fnc
	{
		/// <summary>
		/// 날짜 형식
		/// </summary>
		public enum enDateType { 
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
			/// HHmm
			/// </summary>
			HourMinutes };


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
		/// object값을 string으로 변환하여 준다.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string obj2String(object obj)
		{
			if (obj == null)
				return string.Empty;
			else
				return obj.ToString().Trim();
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
			if (obj == null || obj.ToString() == string.Empty)
				return 0;
			else
				return Convert.ToInt32(obj.ToString());
		}


		/// <summary>
		///  object값을 long으로 변환하여 준다.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static long obj2Long(object obj)
		{
			if (obj == null || obj.ToString() == string.Empty)
				return 0;
			else
				return Convert.ToInt64(obj);
		}


		/// <summary>
		///  object값을 float으로 변환하여 준다.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static float obj2Float(object obj)
		{
			if (obj == null || obj.ToString() == string.Empty)
				return 0;
			else
				return float.Parse(obj.ToString());
		}


		
		/// <summary>
		/// 메시지 창을 띄운다..
		/// </summary>
		/// <param name="win"></param>
		/// <param name="strMsg1"></param>
		/// <param name="strMsg2"></param>
		/// <param name="enType"></param>
		/// <returns></returns>
		public static DialogResult ShowMsg(IWin32Window win, string strMsg1, string strMsg2, frmMessage.enMessageType enType)
		{

			frmMessage frmMsg = new frmMessage(strMsg1, strMsg2, enType);

			if (((Form)win).InvokeRequired)
				return frmMsg.ShowDialog();
			else
				return frmMsg.ShowDialog(win);


		}


		public static DialogResult ShowMsg(string strMsg1, string strMsg2, frmMessage.enMessageType enType)
		{

			frmMessage frmMsg = new frmMessage(strMsg1, strMsg2, enType);
			return frmMsg.ShowDialog();
		}


		public static DialogResult ShowMsg(string strMsg1, string strMsg2, frmMessage.enMessageType enType, int intSecClose)
		{
			frmMessage frmMsg = new frmMessage(strMsg1, strMsg2, enType, intSecClose);
			return frmMsg.ShowDialog();
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
		/// <param name="strData"></param>
		/// <param name="strAddData"></param>
		/// <param name="strSpreator"></param>
		/// <returns></returns>
		public static string StringAdd(string strData, string strAddData, string strSpreator)
		{
			if (strData == string.Empty)
				strData = strAddData;
			else
				strData += strSpreator + strAddData;

			return strData;
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

				p.PlaySync();
			}
			catch
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
				if (!Char.IsDigit(c))
					return false;
			}

			return true;

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

			foreach (object e in Enum.GetValues(type))
			{
				if (strItem == e.ToString())
				{
					o = e;
					break;
				}
			}

			return o;
			
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

			return dValue > 0 ? System.Math.Ceiling(dValue * dCoef) / dCoef :  System.Math.Floor(dValue * dCoef) / dCoef;

		}

	}
}
