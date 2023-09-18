using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace function.Device
{
	public enum enKGCPaperType
	{
		/// <summary>
		/// 작은 용지
		/// </summary>
		Small,
		/// <summary>
		/// 큰용지
		/// </summary>
		Large
	}


    public class Sewoo
    {

		// Dll file name.
		public const string SEWOODIR = "LKBSDK.dll";		

		#region Method Declaration
		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_OpenPrinter")]
		public static extern Int32 LK_OpenPrinter(string PrinterName);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_ClosePrinter")]
		public static extern Int32 LK_ClosePrinter();

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_StartPage")]
		public static extern Int32 LK_StartPage();

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_EndPage")]
		public static extern Int32 LK_EndPage();

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_SetupPrinter")]
		public static extern Int32 LK_SetupPrinter(string LabelWidth, string LabelLength, Int32 MediaType, string GapHeight, string Offset, Int32 Density, Int32 Speed, Int32 Copies);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_PrintWindowsFont")]
		public static extern Int32 LK_PrintWindowsFont(Int32 PosX, Int32 PosY, Int32 Degree, Int32 Height, Int32 Weight, Int32 Italic, Int32 Underline, string TypeFace, string Data);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_PrintDeviceFont")]
		public static extern Int32 LK_PrintDeviceFont(Int32 PosX, Int32 PosY, Int32 Rotation, Int32 FontNumber, Int32 HorExpand, Int32 VerExpand, Int32 Reverse, string Data);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_PrintBMP")]
		public static extern Int32 LK_PrintBMP(Int32 PosX, Int32 PosY, string FileName);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_PrintPCX")]
		public static extern Int32 LK_PrintPCX(Int32 PosX, Int32 PosY, string FileName);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_PrintBarCode")]
		public static extern Int32 LK_PrintBarCode(Int32 PosX, Int32 PosY, Int32 Rotation, string BarCode, Int32 NarrowWidth, Int32 WideWidth, Int32 BarHeight, Int32 Readable, string lpszStr);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_PrintLine")]
		public static extern Int32 LK_PrintLine(Int32 PosX, Int32 PosY, Int32 HoriSize, Int32 VertSize);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_PrintDiagonalLine")]
		public static extern Int32 LK_PrintDiagonalLine(Int32 StartX, Int32 StartY, Int32 EndX, Int32 EndY, Int32 Thick);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_PrintBox")]
		public static extern Int32 LK_PrintBox(Int32 StartX, Int32 StartY, Int32 EndX, Int32 EndY, Int32 Thick);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_PrintDate")]
		public static extern Int32 LK_PrintDate(Int32 PosX, Int32 PosY, Int32 Degree, Int32 Height, Int32 Weight, Int32 Italic, Int32 Underline, string TypeFace, Int32 DateFormat);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_PrintTime")]
		public static extern Int32 LK_PrintTime(Int32 PosX, Int32 PosY, Int32 Degree, Int32 Height, Int32 Weight, Int32 Italic, Int32 Underline, string TypeFace, Int32 TimeFormat);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_SetupPrinterCutter")]
		public static extern Int32 LK_SetupPrinterCutter(string LabelWidth, string LabelLength, Int32 MediaType, string GapHeight, string Offset, Int32 Density, Int32 Speed, Int32 Copies, Int32 Rotation, Int32 Cutting, Int32 CutMethod, Int32 CutPageInterval, string FeedAfterCut);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_DrawLine")]
		public static extern Int32 LK_DrawLine(Int32 LineType, Int32 sx, Int32 sy, Int32 ex, Int32 ey, Int32 Thick);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_Rectangle")]
		public static extern Int32 LK_Rectangle(Int32 LineType, Int32 sx, Int32 sy, Int32 ex, Int32 ey, Int32 Thick);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_Ellipse")]
		public static extern Int32 LK_Ellipse(Int32 LineType, Int32 sx, Int32 sy, Int32 ex, Int32 ey, Int32 Thick);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_PrintWindowsFontAlign")]
		public static extern Int32 LK_PrintWindowsFontAlign(Int32 Alignment, Int32 PosY, Int32 Degree, Int32 Height, Int32 Weight, Int32 Italic, Int32 Underline, string TypeFace, string Data);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_PrintWindowsFontPitch")]
		public static extern Int32 LK_PrintWindowsFontPitch(Int32 PosX, Int32 PosY, Int32 Degree, Int32 Height, Int32 Width, Int32 Weight, Int32 Italic, Int32 Underline, string TypeFace, string Data);

		[DllImport(SEWOODIR, SetLastError = true, EntryPoint = "LK_DirectCommand")]
		public static extern Int32 LK_DirectCommand(string lpstr);

		#endregion

		#region Constant Declaration
		//Method return value
		public const Int32 LK_SUCCESS = 0;

		public const Int32 PS_SOLID = 0;
		public const Int32 PS_DASH = 1;
		public const Int32 PS_DOT = 2;
		public const Int32 PS_DASHDOT = 3;
		public const Int32 PS_DASHDOTDOT = 4;

		public const string PS_PORTNAME = "SEWOO Label Printer";
		#endregion


		public static void PrinterOpen(enKGCPaperType PaperType)
		{
			int rtn = LK_OpenPrinter(PS_PORTNAME);

			if (rtn != 0) throw new Exception($"바코드 프린터 연결에 실패 하였습니다. [RTN]{rtn}");

			switch(PaperType)
			{
				case enKGCPaperType.Large:
					rtn = LK_SetupPrinter("100", "130", 0, "3.1", "0", 12, 3, 1);
					break;

				case enKGCPaperType.Small:
					rtn = LK_SetupPrinter("55", "70", 0, "3.1", "0", 12, 3, 1);
					break;
				
			}


			if (rtn != 0)
			{				
				throw new Exception($"바코드 프린터 설정 시 오류가 발생 하였습니다. [RTN]{rtn}");
			}
		}

	}
}
