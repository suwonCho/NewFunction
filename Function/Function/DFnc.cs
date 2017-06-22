using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Function
{
	/// <summary>
	/// Dynamic 요소 처리 관련 클래스
	/// </summary>
	public class DFnc
	{
		/// <summary>
		/// 객체 프로 퍼티에 값을 설정한다.
		/// </summary>
		/// <param name="o"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public static void Property_Set_Value(object o, string name, object value)
		{
			o.GetType().GetProperty(name).SetValue(o, value, null);
		}

		/// <summary>
		/// 객체 프로 퍼티에 값을 설정한다.
		/// </summary>
		/// <param name="o"></param>
		/// <param name="name"></param>
		/// <param name="paramTypes"></param>
		/// <param name="paramValues"></param>
		/// <param name="value"></param>
		public static void Property_Set_Value(object o, string name, Type[] paramTypes, object[] paramValues, object value)
		{
			o.GetType().GetProperty("Item", paramTypes).SetValue(o, value, paramValues);
		}


		/// <summary>
		/// 객체 프로 퍼티에 값을 가져온다.
		/// </summary>
		/// <param name="o"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static object Property_Get_Value(object o, string name)
		{
			//try
			//{
			//	object m = o.GetType().GetProperty("Item", new Type[] { typeof(int) });

			//	object j = o.GetType().GetProperty("Item", new Type[] { typeof(int) }).GetValue(o, new object[] { 0 });
			//}
			//catch
			//{
			//}

			return o.GetType().GetProperty(name).GetValue(o, null);
		}

		/// <summary>
		/// 객체 프로 퍼티에 값을 가져온다.
		/// </summary>
		/// <param name="o"></param>
		/// <param name="name"></param>
		/// <param name="paramTypes"></param>
		/// <param name="paramValues"></param>
		/// <returns></returns>
		public static object Property_Get_Value(object o, string name, Type[] paramTypes, object[] paramValues)
		{
			return o.GetType().GetProperty("Item", paramTypes).GetValue(o, paramValues);
		}


		/// <summary>
		/// 객체 프로 퍼티에 값을 가져온다. (정해진 return type)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="o"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static T Property_Get_Value<T>(object o, string name)
		{
			return (T)o.GetType().GetProperty(name).GetValue(o, null);
		}

		/// <summary>
		/// 객체 메소트를 실행하고 실행결과를 가져 온다.
		/// </summary>
		/// <param name="o"></param>
		/// <param name="name"></param>
		/// <param name="param">실행 파라메터</param>
		/// <returns></returns>
		public static object Method_Excute(object o, string name, object [] param)
		{
			return o.GetType().GetMethod(name).Invoke(o, param);
		}

		/// <summary>
		/// 객체 메소트를 실행하고 실행결과를 가져 온다. (정해진 return type)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="o"></param>
		/// <param name="name"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		public static T Method_Excute<T>(object o, string name, object[] param)
		{
			return (T)o.GetType().GetMethod(name).Invoke(o, param);
		}




	}
}
