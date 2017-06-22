using System;
using System.Collections.Generic;
using System.Text;

namespace Function.EpcMW
{
	public class clsEpcCode
	{
		//bin > dec hex2epc사용
		private static string Bin2Dec(string bin, int start, int le)
		{
			string code = bin.Substring(start, le);
			code = Convert.ToInt32(code, 2).ToString();
			return code;
		}

		// hex2epc 출력 형식 함수
		private static string Lastcode(string strBinValue, int le, int le2, int itemle)
		{
			string strCocode = "";
			string strItem = "";
			string strLastCode = "";
			int start = 14;

			strCocode = Bin2Dec(strBinValue, start, le);
			start = start + le;

			strItem = Bin2Dec(strBinValue, start, le2);
			strLastCode = string.Format("{0}.{1}", strCocode, strItem.PadLeft(itemle, '0'));
			return strLastCode;
		}

		//10진수 2진수 변환 epc2hex 사용
		private static string Dec2Bin(string value)
		{
			int intBin = 0;
			intBin = int.Parse(value);
			value = Convert.ToString(intBin, 2);
			return value;
		}

		//epc2hex 사용 2진코드 완성하기
		private static string FullBin(string value, string strFilterValue, string strPartitionValue, string strCoValue, string strItemValue, int intBit)
		{
			string strLastBin = "00110001" + strFilterValue + strPartitionValue + strCoValue + strItemValue;
			strLastBin = strLastBin.PadRight(intBit, '0');
			return strLastBin;
		}

		//bin>핵사코드변환epc2hex 사용
		private static string LastHex(string value, int le)
		{
			string strLastHex = "";
			int laststart = le;
			int j = 0;
			string[] strlast = new string[le / 8];
			for (int i = 0; i < laststart; i = i + 8)
			{
				strlast[j] = value.Substring(i, 8);
				int k = Convert.ToInt32(strlast[j], 2);
				strlast[j] = Convert.ToString(k, 16);
				strlast[j] = strlast[j].PadLeft(2, '0');
				strLastHex += strlast[j];
				j = j + 1;
			}
			return strLastHex;
		}

		
		/// <summary>
		/// epc코드 > 핵사값 만들기 메인코드
		/// </summary>
		/// <param name="strCode"></param>
		/// <returns></returns>
		public static string EpcCode2Hex(string strCode)
		{
			//변수선언 해더값선언
			string[] strHeaderArry = strCode.Split(':');
			string[] str;
			string strHeader = strHeaderArry[3];
			string strFilterValue = "";
			string strPartitionValue = "";
			string strCoValue = "";
			string strItemValue = "";
			string strValue = "";
			string strLastBin = "";
			string strLastHex = "";


			//문자열 각 코드별로 자르는부분
			int intStart = 0;
			int intEnd = strCode.Length;

			//필터, 업체, 아이템 값 구하기
			for (int i = 0; i <= 3; i++)
			{
				intStart = intStart + strHeaderArry[i].Length;
			}
			intStart = intStart + 4;
			intEnd = intEnd - intStart;
			strValue = strCode.Substring(intStart, intEnd);
			str = strValue.Split('.');
			strFilterValue = str[0];
			strCoValue = str[1];
			strItemValue = str[2];


			switch (strHeader)
			{
				case "sscc-96":
					//필터 2진변환
					strFilterValue = Dec2Bin(strFilterValue);
					strFilterValue = strFilterValue.PadLeft(3, '0');
					//업체코드2진변환및 파티션번호 생성
					//업체 코드 2진길이에의해 파티션 번호가 정해진다.
					strCoValue = Dec2Bin(strCoValue);

					switch (strCoValue.Length)
					{

						case 40:
							strPartitionValue = "000";
							strItemValue = Dec2Bin(strItemValue);
							strItemValue = strItemValue.PadLeft(18, '0');
							strLastBin = FullBin("00110000", strFilterValue, strPartitionValue, strCoValue, strItemValue, 96);
							break;

						case 37:
							strPartitionValue = "001";
							strItemValue = Dec2Bin(strItemValue);
							strItemValue = strItemValue.PadLeft(21, '0');
							strLastBin = FullBin("00110000", strFilterValue, strPartitionValue, strCoValue, strItemValue, 96);
							break;

						case 34:
							strPartitionValue = "010";
							strItemValue = Dec2Bin(strItemValue);
							strItemValue = strItemValue.PadLeft(24, '0');
							strLastBin = FullBin("00110000", strFilterValue, strPartitionValue, strCoValue, strItemValue, 96);
							break;

						case 30:
							strPartitionValue = "011";
							strItemValue = Dec2Bin(strItemValue);
							strItemValue = strItemValue.PadLeft(28, '0');
							strLastBin = FullBin("00110000", strFilterValue, strPartitionValue, strCoValue, strItemValue, 96);
							break;

						case 27:
							strPartitionValue = "100";
							strItemValue = Dec2Bin(strItemValue);
							strItemValue = strItemValue.PadLeft(31, '0');
							strLastBin = FullBin("00110000", strFilterValue, strPartitionValue, strCoValue, strItemValue, 96);
							break;

						case 24:
							strPartitionValue = "101";
							strItemValue = Dec2Bin(strItemValue);
							strItemValue = strItemValue.PadLeft(34, '0');
							strLastBin = FullBin("00110000", strFilterValue, strPartitionValue, strCoValue, strItemValue, 96);
							break;

						case 20:
							strPartitionValue = "110";
							strItemValue = Dec2Bin(strItemValue);
							strItemValue = strItemValue.PadLeft(38, '0');
							strLastBin = FullBin("00110000", strFilterValue, strPartitionValue, strCoValue, strItemValue, 96);
							break;

						default:
							throw new Exception("입력받은 업체번호가 규격형식이 아닙니다");

					}
					break;

				case "sgtin-96":
					{
						//필터 2진변환
						strFilterValue = Dec2Bin(strFilterValue);
						strFilterValue = strFilterValue.PadLeft(3, '0');
						//업체코드2진변환및 파티션번호 생성
						strCoValue = Dec2Bin(strCoValue);

						switch (strCoValue.Length)
						{
							case 40:
								strPartitionValue = "000";
								strItemValue = Dec2Bin(strItemValue);
								strItemValue = strItemValue.PadLeft(4, '0');
								strLastBin = FullBin("00110000", strFilterValue, strPartitionValue, strCoValue, strItemValue, 96);
								break;

							case 37:
								strPartitionValue = "001";
								strItemValue = Dec2Bin(strItemValue);
								strItemValue = strItemValue.PadLeft(7, '0');
								strLastBin = FullBin("00110000", strFilterValue, strPartitionValue, strCoValue, strItemValue, 96);
								break;

							case 34:
								strPartitionValue = "010";
								strItemValue = Dec2Bin(strItemValue);
								strItemValue = strItemValue.PadLeft(10, '0');
								strLastBin = FullBin("00110000", strFilterValue, strPartitionValue, strCoValue, strItemValue, 96);
								break;

							case 30:
								strPartitionValue = "011";
								strItemValue = Dec2Bin(strItemValue);
								strItemValue = strItemValue.PadLeft(14, '0');
								strLastBin = FullBin("00110000", strFilterValue, strPartitionValue, strCoValue, strItemValue, 96);
								break;

							case 27:
								strPartitionValue = "100";
								strItemValue = Dec2Bin(strItemValue);
								strItemValue = strItemValue.PadLeft(17, '0');
								strLastBin = FullBin("00110000", strFilterValue, strPartitionValue, strCoValue, strItemValue, 96);
								break;

							case 24:
								strPartitionValue = "101";
								strItemValue = Dec2Bin(strItemValue);
								strItemValue = strItemValue.PadLeft(20, '0');
								strLastBin = FullBin("00110000", strFilterValue, strPartitionValue, strCoValue, strItemValue, 96);
								break;

							case 20:

								strPartitionValue = "110";
								strItemValue = Dec2Bin(strItemValue);
								strItemValue = strItemValue.PadLeft(24, '0');
								strLastBin = FullBin("00110000", strFilterValue, strPartitionValue, strCoValue, strItemValue, 96);
								break;

							default:
								throw new Exception("입력받은 업체번호가 규격형식이 아닙니다");

						}
						break;
					}
			}
			//16진수 RFID 코드만들기
			strLastHex = LastHex(strLastBin, strLastBin.Length);
			return strLastHex.ToUpper();
		}
		
		
		/// <summary>
		/// 16진수 태그값>EPC코드 변환 메인 코드
		/// </summary>
		/// <param name="strHexValue"></param>
		/// <returns></returns>
		public static string Hex2EpcCode(string strHexValue)
		{
			//변수 선언
			string strValue;
			string strCode = "";
			string[] str = new string[strHexValue.Length];
			int intEnd = 0;
			int intBin = 0;

			string strDecValue = "";
			string strBinValue = "";

			//hex값을 bin/dec 값으로 변환
			for (int start = 0; start < str.Length; start = start + 2)
			{
				//리딩값 2자리씩 배열입력(16)
				str[intEnd] = strHexValue.Substring(start, 2);

				//읽어들인 값 변환

				//10진수 변환 및 변수 설정(전체 10진 코드 변수 입력)
				strValue = "000" + Convert.ToInt32(str[intEnd], 16).ToString();

				//변환값 3자리수일때 구분 해서 변수입력
				strDecValue += strValue.Substring(strValue.Length - 3, 3);

				//2진수 변환및 변수설정(전체 2진 코드 변수 입력)
				intBin = int.Parse(strValue);
				strValue = Convert.ToString(intBin, 2);
				strBinValue += strValue.PadLeft(8, '0').ToString();

				intEnd = intEnd + 1;
			}

			//EPCIS 코드 분석 10진수 변환후 비교
			intBin = Convert.ToInt32(str[0], 16);
			string strPartitionValue = strBinValue.Substring(11, 3);
			string strFilterValue = strBinValue.Substring(8, 3);
			string strLastCode = "";
			int intLe = 0;
			int intLe2 = 0;

			//2진 코드 자리수 분석으로 이상 코드 분류후 이상없을시 규격코드 비교 시작
			if (!(
				strBinValue.Length == 96
				|| strBinValue.Length == 64
				|| strBinValue.Length == 198
				|| strBinValue.Length == 170
				|| strBinValue.Length == 202
				|| strBinValue.Length == 195
				|| strBinValue.Length == 113
				)) throw new Exception("입력받은 형식은 EPC규격형식이 아님니다");



			if (intBin < 08)
			{
				strCode = "Reserved for Future Use";
				return strCode;
			}
			else if (intBin == 08)
			{
				strCode = "Reserved until 64bit Sunset SSCC-64";
				return strCode;
			}
			else if (intBin == 09)
			{
				strCode = "Reserved until 64bit Sunset SGLN-64";
				return strCode;
			}
			else if (intBin == 10)
			{
				strCode = "Reserved until 64bit Sunset GRAI-64";
				return strCode;
			}
			else if (intBin == 11)
			{
				strCode = "Reserved until 64bit Sunset GIAI-64";
				return strCode;
			}
			else if (12 <= intBin && 15 >= intBin)
			{
				strCode = "Reserved until 64 bit Sunset\r\nDue to 64 bit encoding rule in Gen 1";
				return strCode;
			}
			else if (16 <= intBin && 43 >= intBin)
			{
				strCode = "Reserved for Future Use";
				return strCode;
			}
			else if (intBin == 44)
			{
				strCode = "GDTI-96";
				return strCode;
			}
			else if (intBin == 45)
			{
				strCode = "GSRN-96";
				return strCode;
			}
			else if (intBin == 46)
			{
				strCode = "Reserved for Future Use";
				return strCode;
			}
			else if (intBin == 47)
			{
				strCode = "DoD-96";
				return strCode;
			}
			else if (intBin == 48)     //SGTIN-96
			{
				int itemle = 5;

				switch (strPartitionValue)
				{
					case "000":
						intLe = 40;
						intLe2 = 4;
						strLastCode = Lastcode(strBinValue, intLe, intLe2, itemle);
						strCode = string.Format("urn:epc:id:sgtin:{0}", strLastCode);
						break;

					case "001":
						intLe = 37;
						intLe2 = 7;
						strLastCode = Lastcode(strBinValue, intLe, intLe2, itemle);
						strCode = string.Format("urn:epc:id:sgtin:{0}", strLastCode);
						break;

					case "010":
						intLe = 34;
						intLe2 = 10;
						strLastCode = Lastcode(strBinValue, intLe, intLe2, itemle);
						strCode = string.Format("urn:epc:id:sgtin:{0}", strLastCode);
						break;

					case "011":
						intLe = 30;
						intLe2 = 14;
						strLastCode = Lastcode(strBinValue, intLe, intLe2, itemle);
						strCode = string.Format("urn:epc:id:sgtin:{0}", strLastCode);
						break;

					case "100":
						intLe = 27;
						intLe2 = 17;
						strLastCode = Lastcode(strBinValue, intLe, intLe2, itemle);
						strCode = string.Format("urn:epc:id:sgtin:{0}", strLastCode);
						break;

					case "101":
						intLe = 24;
						intLe2 = 20;
						strLastCode = Lastcode(strBinValue, intLe, intLe2, itemle);
						strCode = string.Format("urn:epc:id:sgtin:{0}", strLastCode);
						break;

					case "110":
						intLe = 20;
						intLe2 = 24;
						strLastCode = Lastcode(strBinValue, intLe, intLe2, itemle);
						strCode = string.Format("urn:epc:id:sgtin:{0}", strLastCode);
						break;

					default:
						throw new Exception("입력받은 코드가  규격형식이 아닙니다");
				}
				return strCode;

			}
			else if (intBin == 49)     //SSCC-96
			{
				int itemle = 9;
				switch (strPartitionValue)
				{
					case "000":
						intLe = 40;
						intLe2 = 18;
						strLastCode = Lastcode(strBinValue, intLe, intLe2, itemle);
						strCode = string.Format("urn:epc:id:sscc:{0}", strLastCode);
						break;

					case "001":
						intLe = 37;
						intLe2 = 21;
						strLastCode = Lastcode(strBinValue, intLe, intLe2, itemle);
						strCode = string.Format("urn:epc:id:sscc:{0}", strLastCode);
						break;

					case "010":
						intLe = 34;
						intLe2 = 24;
						strLastCode = Lastcode(strBinValue, intLe, intLe2, itemle);
						strCode = string.Format("urn:epc:id:sscc:{0}", strLastCode);
						break;

					case "011":
						intLe = 30;
						intLe2 = 28;
						strLastCode = Lastcode(strBinValue, intLe, intLe2, itemle);
						strCode = string.Format("urn:epc:id:sscc:{0}", strLastCode);
						break;

					case "100":
						intLe = 27;
						intLe2 = 31;
						strLastCode = Lastcode(strBinValue, intLe, intLe2, itemle);
						strCode = string.Format("urn:epc:id:sscc:{0}", strLastCode);
						break;

					case "101":

						intLe = 24;
						intLe2 = 34;
						strLastCode = Lastcode(strBinValue, intLe, intLe2, itemle);
						strCode = string.Format("urn:epc:id:sscc:{0}", strLastCode);
						break;

					case "110":
						intLe = 20;
						intLe2 = 38;
						strLastCode = Lastcode(strBinValue, intLe, intLe2, itemle);
						strCode = string.Format("urn:epc:id:sscc:{0}", strLastCode);
						break;

					default:
						throw new Exception("입력받은 코드가  규격형식이 아닙니다");

				}
				return strCode;

			}
			else if (intBin == 50)
			{
				strCode = "SGLN-96";
				return strCode;
			}
			else if (intBin == 51)
			{
				strCode = "GRAI-96";
				return strCode;
			}
			else if (intBin == 52)
			{
				strCode = "GIAI-96";
				return strCode;
			}
			else if (intBin == 53)
			{
				strCode = "GID-96";
				return strCode;
			}
			else if (intBin == 54)
			{
				strCode = "SGTIN-198";
				return strCode;
			}
			else if (intBin == 55)
			{
				strCode = "GRAI-170";
				return strCode;
			}
			else if (intBin == 56)
			{
				strCode = "GIAI-202";
				return strCode;
			}
			else if (intBin == 57)
			{
				strCode = "SGLN-195";
				return strCode;
			}
			else if (intBin == 58)
			{
				strCode = "GDTI-113";
				return strCode;
			}
			else if (intBin >= 59 && intBin <= 63)
			{
				strCode = "Reserved for future Header values";
				return strCode;
			}
			else if (intBin >= 64 && intBin <= 127)
			{
				strCode = "Reserved until 64 bit Sunset";
				return strCode;
			}
			else if (intBin >= 128 && intBin <= 191)
			{
				strCode = "Reserved until 64 bit Sunset <SGTIN-64>\r\n(64 header values)";
				return strCode;
			}
			else if (intBin >= 192 && intBin <= 205)
			{
				strCode = "Reserved until 64 bit Sunset";
				return strCode;
			}
			else if (intBin == 206)
			{
				strCode = "Reserved until 64 bit Sunset <DoD-64>";
				return strCode;
			}
			else if (intBin >= 207 && intBin <= 254)
			{
				strCode = "Reserved until 64 bit Sunset";
				return strCode;
			}
			else if (intBin == 255)
			{
				strCode = "Reserved for future headers longer than 8 bits";
				return strCode;
			}
			else
			{
				throw new Exception("입력받은 코드가  EPC규격형식이 아닙니다");
			}


		}
	}

}