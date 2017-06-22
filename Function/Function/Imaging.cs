using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Function
{
	public class Imaging
	{
		public enum ImageSize { Zoom, Strech };

		/// <summary>
		/// 그래픽에 텍스트를 표시한다.
		/// </summary>
		/// <param name="e"></param>
		/// <param name="text"></param>
		/// <param name="color"></param>
		/// <param name="font"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="iw">표시구역 너비</param>
		/// <param name="ih">표시구역 높이</param>
		public static void DrawText(Graphics e, string text, Color color, Font font, int x, int y, int iw, int ih )
		{

			SolidBrush brush = new SolidBrush(color);

			RectangleF Rect = new RectangleF(x, y, iw, ih);

			StringFormat format = new StringFormat();

			format.Alignment = StringAlignment.Center;
			//format.LineAlignment = StringAlignment.Center;


			e.DrawString(text, font, brush, Rect, format);

			e.Save();

		}

		/// <summary>
		/// 그래픽에 텍스트를 표시한다.
		/// </summary>
		/// <param name="e"></param>
		/// <param name="text"></param>
		/// <param name="color"></param>
		/// <param name="font"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="iw">표시구역 너비</param>
		/// <param name="ih">표시구역 높이</param>
		/// <param name="Alignment">가로정렬</param>
		/// <param name="LineAlignment">세로정렬</param>
		public static void DrawText(Graphics e, string text, Color color, Font font, int x, int y, int iw, int ih, StringAlignment Alignment, StringAlignment LineAlignment)
		{

			SolidBrush brush = new SolidBrush(color);

			RectangleF Rect = new RectangleF(x, y, iw, ih);

			StringFormat format = new StringFormat();

			format.Alignment =  Alignment;
			format.LineAlignment = LineAlignment;


			e.DrawString(text, font, brush, Rect, format);

			e.Save();

		}


		public static void DrawText(Graphics e, string text, Color color, Font font, float x, float y, float iw, float ih, StringAlignment Alignment, StringAlignment LineAlignment)
		{

			SolidBrush brush = new SolidBrush(color);

			RectangleF Rect = new RectangleF(x, y, iw, ih);

			StringFormat format = new StringFormat();

			format.Alignment = Alignment;
			format.LineAlignment = LineAlignment;


			e.DrawString(text, font, brush, Rect, format);

			e.Save();

		}



		/// <summary>
		/// 이미지를 그려넣는다..
		/// </summary>
		/// <param name="g"></param>
		/// <param name="PicPath"></param>
		public static void DrawImage(Graphics g, string strPicPath, float x, float y, float iw, float ih, ImageSize Is)
		{
			if (strPicPath != null && strPicPath != string.Empty)
			{
				using (Bitmap bit = new Bitmap(strPicPath))
				{
					g.DrawImage(bit, x, y, iw, ih);
				}
			}

		}


		public static void DrawLine(Graphics e, Color color, int x, int y, int iw, int ih)
		{

			SolidBrush brush = new SolidBrush(color);

			e.DrawLine(new Pen(brush,0.1f) , x, y, iw, ih);

			e.Save();

		}

		/// <summary>
		/// BitMap을 Icon으로 변경한다.
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Icon Bitmap2Icon(Bitmap b)
		{
			IntPtr bi = b.GetHicon();

			return Icon.FromHandle(bi);		

		}



	}
}
