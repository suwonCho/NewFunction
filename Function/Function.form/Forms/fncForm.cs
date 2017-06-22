using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Function.form
{
	public class fncForm
	{
		/// <summary>
		/// 셑팅 문자열을 가지고 온다. 
		/// 포맷) /*폼이름:셑팅문자열*/
		/// </summary>
		/// <param name="value"></param>
		/// <param name="formName"></param>
		/// <returns></returns>
		public static string getSettingString(string value, string formName)
		{
			string rtn = string.Empty;

			string formNM = string.Format("/*{0}:", formName.ToUpper());
			int iSt = value.IndexOf(formNM);
			if (iSt < 0) return rtn;
			int ied = value.IndexOf("*/", iSt + 1);

			rtn = value.Substring(iSt + formNM.Length, ied - (iSt + formNM.Length));

			return rtn;

		}

		/// <summary>
		/// 설정값에서 해당 폼의 설정값을 제거하고 가져온다.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="formName"></param>
		/// <returns></returns>
		public static string revSettingString(string value, string formName)
		{
			
			string formNM = string.Format("/*{0}:", formName.ToUpper());

			//폼내용은 전부 삭제
			while (true)
			{
				int iSt = value.IndexOf(formNM);
				if (iSt < 0) break;
				int ied = value.IndexOf("*/", iSt + 1);

				if (ied < 0)
					value = value.Substring(0, iSt);
				else
					value = value.Substring(0, iSt) + (ied + 2 >= value.Length ? string.Empty : value.Substring(ied + 2));
			}

			return value;
		}


		/// <summary>
		/// 컨트롤 크기 및 위치 정볼를 불러온다.
		/// </summary>
		/// <param name="setting_object"></param>
		/// <param name="propertyName"></param>
		public static void control_Size_Load(object setting_object, string propertyName, Form frm)
		{
			try
			{
				//폼내용은 '/*폼이름:컨트롤정보들...*/' 구성
				string value = Fnc.obj2String(Function.DFnc.Property_Get_Value(setting_object, propertyName));

				value = getSettingString(value, frm.Name);

				//컨트롤정보는 '컨트롤이름:top:left:너비:높이;' 로구분
				string[] cInfo = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				string[] cValue;
				Control C;

				foreach (string info in cInfo)
				{
					cValue = info.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
					if (cValue.Length < 5) continue;

					try
					{
						C = frm.Controls[cValue[0]];

						if (C == null) continue;

						C.Top = Fnc.obj2int(cValue[1]);
						C.Left = Fnc.obj2int(cValue[2]);
						C.Width = Fnc.obj2int(cValue[3]);
						C.Height = Fnc.obj2int(cValue[4]);

						if (C.GetType().Equals(typeof(SplitContainer)))
						{
							((SplitContainer)C).SplitterDistance = Fnc.obj2int(cValue[5]);
						}

					}
					catch
					{
						continue;
					}
				}

			}
			catch
			{
			}
		}


		public static void control_Size_Save(object setting_object, string propertyName, Form frm)
		{
			try
			{
				//폼내용은 '/*폼이름:컨트롤정보들...*/' 구성
				string value = Fnc.obj2String(Function.DFnc.Property_Get_Value(setting_object, propertyName));
				string formNM = string.Format("/*{0}:", frm.Name.ToUpper());

				value = revSettingString(value, frm.Name);

				value += formNM;

				foreach (Control c in frm.Controls)
				{
					value += string.Format("{0}:{1}:{2}:{3}:{4}", c.Name, c.Top, c.Left, c.Width, c.Height);

					if (c.GetType().Equals(typeof(SplitContainer)))
					{
						value += string.Format(":{0}", ((SplitContainer)c).SplitterDistance);
					}

					value += ";";
				}


				value += "*/";

				Function.DFnc.Property_Set_Value(setting_object, propertyName, value);

				Function.DFnc.Method_Excute(setting_object, "Save", null);


			}
			catch
			{
			}
		}


		/// <summary>
		/// 컨트롤 이름으로 컨트롤을 찾는다.
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="ctrlName"></param>
		/// <returns></returns>
		public static Control FindControlInControl(Control ctrl, string ctrlName)
		{
			Control c;

			c = ctrl.Controls[ctrlName];

			if (c == null && ctrl.Controls.Count > 0)
			{
				foreach (Control cc in ctrl.Controls)
				{
					c = FindControlInControl(cc, ctrlName);
					if (c != null) break;
				}
			}

			return c;


		}


	
	}// end class
}
