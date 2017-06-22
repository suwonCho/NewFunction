using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Function.form
{

	/// <summary>
	/// 폼 위치 설정 타입
	/// </summary>
	public enum enFormPositionType
	{
		/// <summary>
		/// 현재 마우스 위치가 폼의 중심으로 오도록 하여 위치를 잡는다.
		/// </summary>
		MousePosition_Center
	}



	public class vari
	{
		/// <summary>
		/// 원안에 숫자 배열 0~9
		/// </summary>
		public static string[] circle_numbers = new string[] { "ⓞ", "①", "②", "③", "④", "⑤", "⑥", "⑦", "⑧", "⑨" };

		/// <summary>
		/// 안이 비어있는 원
		/// </summary>
		public static string circle_white = "○";
		/// <summary>
		/// 안이 차있는 원
		/// </summary>
		public static string circle_black = "●";


	}
}
