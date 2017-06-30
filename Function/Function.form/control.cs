using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Threading;


namespace Function.form
{

	/// <summary>
	/// Form control 관련 클래스..
	/// </summary>
	public class control
	{
		/// <summary>
		/// 폼에서 이름과 type이 일치하는 컨트롤을 찾는다.
		/// </summary>
		/// <param name="ctrl">폼에 아무 컨트롤을 넘긴다.</param>
		/// <param name="name">찾을 컨트롤 이름(string.Empty일경우 체크 않함)</param>
		/// <param name="type">찾을 컨트롤 type</param>
		/// <returns></returns>
		public static object find_Control(Control ctrl, string name, Type type)
		{

			foreach (Control c in ctrl.Controls)
			{
				if (name.Equals(string.Empty) || c.Name.Equals(name))
				{
					if (type == null) return c;

					if (c.GetType().Equals(type)) return c;
				}

				object o = find_Control(c, name, type);

				if (o != null) return o;

			}

			return null;

		}


		private static Mutex gM;

		/// <summary>
		/// 중복 실행 체크 한다. 중복:false
		/// </summary>
		/// <param name="strProgramName"></param>		
		/// <returns>false이면 이미 실행, true이면 미실행</returns>
		public static bool ProgramRunCheck(string strProgramName)
		{
			bool isCreateNew;

			gM = new Mutex(true, strProgramName, out isCreateNew);

			return isCreateNew;
		}


		/// <summary>
		/// 자식창을 부모창 중앙에 이동 시킨다.
		/// </summary>
		/// <param name="frm"></param>
		public static void MdiChildForm2Center(Form frm)
		{
			int x, y;

			Form pForm = frm.MdiParent;

			x = pForm.Width;
			y = pForm.Height;

			x = (x / 2) - (frm.Width / 2);
			y = (y / 2) - (frm.Height / 2);

			frm.Top = y;
			frm.Left = x;

		}

		/// <summary>
		/// 부모 컨트롤내에 자식컨트롤 위치를 중앙으로 이동 시킨다.
		/// </summary>
		/// <param name="frm"></param>
		public static void Control2Center(Control parent, Control child)
		{
			int x, y;
					

			x = parent.Width;
			y = parent.Height;

			x = (x / 2) - (child.Width / 2);
			y = (y / 2) - (child.Height / 2);

			if ((parent as Form) != null && (child as Form) != null)
			{   //폼에 경우는 기초 위치를 더해 준다.
				child.Top = parent.Top + y;
				child.Left = parent.Left + x;
			}
			else
			{
				child.Top = y;
				child.Left = x;
			}

		}


		/// <summary>
		/// 폼이나 컨트롤내에 포커스를 가진 컨트롤을 찾아 낸다.
		/// </summary>
		/// <param name="frm"></param>
		/// <param name="ctrl"></param>
		/// <returns></returns>
		public static Control FocuedControlFind(Form frm = null, Control ctrl = null)
		{
			if (frm == null && ctrl == null) return null;

			Control.ControlCollection cc;

			if (frm != null)
				cc = frm.Controls;
			else
			{
				if (ctrl.Focused) return ctrl;

				cc = ctrl.Controls;				
			}

			foreach (Control c in cc)
			{
				if (c.Focused) return c;

				Control ct = FocuedControlFind(null, c);
				if (ct != null) return ct;
			}

			return null;
		}


		public static enControlType ControlTypeGet(Control ctrl)
		{

			enControlType rtn = enControlType.CONTAINER;

			if (ctrl == null) return rtn;

			if(ctrl.GetType().ToString() ==  typeof(TextBox).ToString() ||
				ctrl.GetType().ToString() == typeof(ComboBox).ToString() )
					rtn = enControlType.INPUT;
			else if(ctrl.GetType().ToString() ==  typeof(Label).ToString())
					rtn = enControlType.LABEL;
			
			return rtn;

		}


#region 공용Control 관련 Invoke	


		delegate void delInvoke_Control_SetProperty(Control ctl, string PropertyName, object value);

		public static void Invoke_Control_SetProperty(Control ctl, string PropertyName, object value)
		{
			if (ctl.InvokeRequired)
			{
				ctl.Invoke(new delInvoke_Control_SetProperty(Invoke_Control_SetProperty), new object[] { ctl, PropertyName, value });
				return;
			}

			var type = ctl.GetType();
			var pt = type.GetProperty(PropertyName);

			pt.SetValue(ctl, value, null);
		}



		delegate void delInvoke_Control_Enabled(Control ctl, bool Enabled);
		/// <summary>
		/// Control Enabled 값을 변경한다.
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="isEnabled"></param>
		public static void Invoke_Control_Enabled(Control ctl, bool isEnabled)
		{
			if (ctl.InvokeRequired)
			{
				ctl.Invoke(new delInvoke_Control_Enabled(Invoke_Control_Enabled), new object[] { ctl, isEnabled });
				return;
			}

			ctl.Enabled = isEnabled;
		}


		delegate void delInvoke_Control_Text(Control ctl, string strText);
		/// <summary>
		/// Control에 TEXT를 변경한다.
		/// </summary>
		/// <param name="lbl"></param>
		/// <param name="strText"></param>
		public static void Invoke_Control_Text(Control ctl, string strText)
		{
			if (ctl.InvokeRequired)
				ctl.Invoke(new delInvoke_Control_Text(Invoke_Control_Text), new object[] { ctl, strText });
			else
				ctl.Text = strText;
			
		}



		delegate void delInvoke_Control_Tag(Control ctl, object tag);
		/// <summary>
		/// Control에 Tag를 변경한다.
		/// </summary>
		/// <param name="lbl"></param>
		/// <param name="tag"></param>
		public static void Invoke_Control_Tag(Control ctl, object tag)
		{
			if (ctl.InvokeRequired)
				ctl.Invoke(new delInvoke_Control_Text(Invoke_Control_Tag), new object[] { ctl, tag });
			else
				ctl.Tag = tag;

		}



		delegate void delInvoke_Control_TextAdd(Control ctl, string strText, bool isClear, bool isFront);
		/// <summary>
		/// Control에 TEXT를 추가/변경한다.
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="strText"></param>
		/// <param name="isClear">기존 Text삭제 여부</param>
		/// <param name="isFront">Text추가시 앞부분에 추가 여부</param>
		public static void Invoke_Control_TextAdd(Control ctl, string strText, bool isClear, bool isFront)
		{
			if (ctl.InvokeRequired)
			{
				ctl.Invoke(new delInvoke_Control_TextAdd(Invoke_Control_TextAdd), new object[] { ctl, strText, isClear, isFront });
				return;
			}



			if (isClear)
				ctl.Text = strText;
			else if (isFront)
				ctl.Text = strText + ctl.Text;
			else
				ctl.Text += strText;
		}

		delegate void delInvoke_Control_Loc(Control ctl, int intLoc);
		/// <summary>
		/// Control에 Top 값을 변경한다.
		/// </summary>
		/// <param name="ctl"></param>
		/// <param name="intTop"></param>
		public static void Invoke_Control_Top(Control ctl, int intTop)
		{
			if (ctl.InvokeRequired)
				ctl.Invoke(new delInvoke_Control_Loc(Invoke_Control_Top), new object[] { ctl, intTop });
			else
				ctl.Top = intTop;
		}

		/// <summary>
		/// Control에 Left 값을 변경한다.
		/// </summary>
		/// <param name="ctl"></param>
		/// <param name="intLeft"></param>
		public static void Invoke_Control_Left(Control ctl, int intLeft)
		{
			if (ctl.InvokeRequired)
				ctl.Invoke(new delInvoke_Control_Loc(Invoke_Control_Left), new object[] { ctl, intLeft });
			else
				ctl.Left = intLeft;
		}

		/// <summary>
		/// Control에 Width 값을 변경한다.
		/// </summary>
		/// <param name="ctl"></param>
		/// <param name="intWidth"></param>
		public static void Invoke_Control_Width(Control ctl, int intWidth)
		{
			if (ctl.InvokeRequired)
				ctl.Invoke(new delInvoke_Control_Loc(Invoke_Control_Width), new object[] { ctl, intWidth });
			else
				ctl.Width = intWidth;

		}

		delegate void delInvoke_Control_Visible(Control ctl, bool isVisible);
		/// <summary>
		/// Control에 Visible을 변경한다.
		/// </summary>
		/// <param name="lbl"></param>
		/// <param name="isVisible"></param>
		public static void Invoke_Control_Visible(Control ctl, bool isVisible)
		{
			if (ctl.InvokeRequired)
				ctl.Invoke(new delInvoke_Control_Visible(Invoke_Control_Visible), new object[] { ctl, isVisible });
			else
				ctl.Visible = isVisible;		

		}

		delegate void delInvoke_Control_ContextMenuStrip(Control ctrl, ContextMenuStrip menus);
		/// <summary>
		/// Control에 ContextMenuStrip을 설정한다.
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="menus"></param>
		public static void Invoke_Control_ContextMenuStrip(Control ctrl, ContextMenuStrip menus)
		{
			if (ctrl.InvokeRequired)
				ctrl.Invoke(new delInvoke_Control_ContextMenuStrip(Invoke_Control_ContextMenuStrip), new object[] { ctrl, menus });
			else
				ctrl.ContextMenuStrip = menus;
		}



		delegate void delInvoke_Control_Color(Control lbl, object colFore, object colBack);

		/// <summary>
		/// Control에 ForeColor / BackColor를 변경한다.
		/// </summary>
		/// <param name="ctl"></param>
		/// <param name="colFore">글자 색(null 입력시 변경 없음)</param>
		/// <param name="colBack">배경 색(null 입력시 변경 없음)</param>
		public static void Invoke_Control_Color(Control ctl, object colFore, object colBack)
		{
			if (ctl.InvokeRequired)
			{
				ctl.Invoke(new delInvoke_Control_Color(Invoke_Control_Color), new object[] { ctl, colFore, colBack });
				return;
			}

			if (colBack != null) ctl.BackColor = (Color)colBack;
			if (colFore != null) ctl.ForeColor = (Color)colFore;

		}



		delegate void delInvoke_Control_DataBindings_Add(Control ctl, string strPropertyName, object objSource, string strDataMember);

		/// <summary>
		/// 바인딩 추가..
		/// </summary>
		/// <param name="ctl"></param>
		/// <param name="strPropertyName"></param>
		/// <param name="objSource"></param>
		/// <param name="strDataMember"></param>
		public static void Invoke_Control_DataBindings_Add(Control ctl, string strPropertyName, object objSource, string strDataMember)
		{
			if (ctl.InvokeRequired)
			{
				ctl.Invoke(new delInvoke_Control_DataBindings_Add(Invoke_Control_DataBindings_Add), new object[] { ctl, strPropertyName, objSource, strDataMember });
				return;
			}

			ctl.DataBindings.Add(strPropertyName, objSource, strDataMember);

		}



		delegate void delInvoke_Control_Font(Control ctl, Font lblFont);
		/// <summary>
		/// Control에 폰트를 변경한다.
		/// </summary>
		/// <param name="ctl"></param>
		/// <param name="ctlFont"></param>		
		public static void Invoke_Control_Font(Control ctl, Font ctlFont)
		{
			
			if (ctl.InvokeRequired)
			{
				ctl.Invoke(new delInvoke_Control_Font(Invoke_Control_Font), new object[] { ctl, ctlFont });
				return;
			}

			ctl.Font = ctlFont;
			
			
		}






		delegate void delInvoke_Control_ControlAdd(Control ctlParent, Control ctlChile);
		/// <summary>
		/// 컨트롤에 컨트롤을 추가 하여준다.
		/// </summary>
		/// <param name="pnl"></param>
		/// <param name="ctl"></param>
		public static void Invoke_Control_ControlAdd(Control ctlParent, Control ctlChild)
		{
			if (ctlParent.InvokeRequired)
			{
				ctlParent.Invoke(new delInvoke_Control_ControlAdd(Invoke_Control_ControlAdd), new object[] { ctlParent, ctlChild });
				return;
			}

			ctlParent.Controls.Add(ctlChild);

		}

		/// <summary>
		/// 컨트롤을 앞으로 이동 시킨다.
		/// </summary>
		/// <param name="ctl"></param>
		public static void Invoke_Control_BrintToFront(Control ctl)
		{
			if (ctl.InvokeRequired)
			{
				ctl.Invoke(new delControlInReset(Invoke_Control_BrintToFront), new object[] { ctl });
				return;
			}



			ctl.BringToFront();
		}




		delegate void delControlInReset(Control ctl);
		/// <summary>
		/// 그룹 않에 있는 컨트롤을 초기화 한다.
		/// </summary>
		/// <param name="ctl"></param>
		public static void ControlInReset(Control ctl)
		{
			if (ctl.InvokeRequired)
			{
				ctl.Invoke(new delControlInReset(ControlInReset), new object[] { ctl });
				return;
			}


			TextBox txt;
			ComboBox cmb;
			Label lbl;
			usrInputBox ib;
			usrTreeView_Ch tv;


			foreach (Control c in ctl.Controls)
			{
				txt = c as TextBox;

				if (txt != null)
				{
					txt.Enabled = true;
					Invoke_Control_TextAdd(txt, string.Empty, true, true);
					continue;
				}

				cmb = c as ComboBox;
				if (cmb != null)
				{
					Invoke_ComboBox_SelectedIndex(cmb, -1);
					continue;
				}


				lbl = c as Label;
				if (lbl != null)
				{
					if (lbl.Name.Substring(0, 3) == "lbl")
					{
						Invoke_Control_Text(lbl, string.Empty);
					}
				}

				ib = c as usrInputBox;

				if (ib != null)
				{
					ib.Text = string.Empty;					
				}

				tv = c as usrTreeView_Ch;

				if (tv != null)
				{
					tv.Nodes.Clear();					
				}

				ControlInReset(c);
			}

		}


		/// <summary>
		/// 컨트롤의 z-order를 맨앞으로 한다.
		/// </summary>
		/// <param name="ctl"></param>
		public static void Invoke_Control_BringToFront(Control ctl)
		{
			if (ctl.InvokeRequired)
			{
				ctl.Invoke(new delControlInReset(Invoke_Control_BringToFront), new object[] { ctl });
				return;
			}

			ctl.BringToFront();			

		}


		/// <summary>
		/// 컨트롤의 포커스를 가져간다.
		/// </summary>
		/// <param name="ctl"></param>
		public static void Invoke_Control_Focus(Control ctl)
		{
			if (ctl.InvokeRequired)
			{
				ctl.Invoke(new delControlInReset(Invoke_Control_Focus), new object[] { ctl });
				return;
			}

			ctl.Focus();

		}



#endregion
		


#region 폼 invoke 관련
		delegate void delInvoke_Form_ControlAdd(Form frm, Control ctl);
		/// <summary>
		/// 퐄에 컨트롤을 추가 한다.
		/// </summary>
		/// <param name="frm"></param>
		/// <param name="ctl"></param>
		public static void Invoke_Form_ControlAdd(Form frm, Control ctl)
		{
			if (frm.InvokeRequired)
			{
				frm.Invoke(new delInvoke_Form_ControlAdd(Invoke_Form_ControlAdd), new object[] { frm, ctl });
				return;
			}

			frm.Controls.Add(ctl);
		}




		delegate void delInvoke_Form_Opacity(Form frm, double dbl);
		/// <summary>
		/// 폼에 투명도를 변경한다.
		/// </summary>
		/// <param name="frm"></param>
		/// <param name="ctl"></param>
		public static void Invoke_Form_Opacity(Form frm, double dbl)
		{
			if (frm.InvokeRequired)
			{
				frm.Invoke(new delInvoke_Form_Opacity(Invoke_Form_Opacity), new object[] { frm, dbl });
				return;
			}

			frm.Opacity = dbl;

		}

		delegate void delInvoke_Form_Close(Form frm);
		/// <summary>
		/// 폼을 닫는다
		/// </summary>
		/// <param name="frm"></param>
		/// <param name="ctl"></param>
		public static void Invoke_Form_Close(Form frm)
		{
			if (frm.InvokeRequired)
			{
				frm.Invoke(new delInvoke_Form_Close(Invoke_Form_Close), new object[] { frm });
				return;
			}

			frm.Close();

		}


		/// <summary>
		/// mdi 부모창에 자식창을 띄운다..
		/// </summary>
		/// <param name="frmMdi"></param>
		/// <param name="frmChild"></param>
		/// <param name="isCheckDuplication">중복체크 form에 text로 한다.</param>
		public static void Invoke_Form_Add_MdiChild(Form frmMdi, Form frmChild, bool isCheckDuplication)
		{
			//폼 중복 체크를 한다.
			if (isCheckDuplication)
			{
				foreach (Form f in frmMdi.MdiChildren)
				{
					if (f.Text == frmChild.Text)
					{
						frmChild.Dispose();
						if (f.WindowState == FormWindowState.Minimized) f.WindowState = FormWindowState.Normal;
						f.BringToFront();
						f.Focus();
						return;
					}
				}

			}

			frmChild.MdiParent = frmMdi;
			frmChild.Show();
		}




		/// <summary>
		/// 폼 text로 부모창에서 자식 폼을 찾는다..
		/// </summary>
		/// <param name="frmMdi">부모 폼</param>
		/// <param name="frmChild">자식 폼</param>
		/// <returns></returns>
		public static Form Form_Find_MdiChild(Form frmMdi, Form frmChild)
		{
			foreach (Form f in frmMdi.MdiChildren)
			{
				if (f.Text == frmChild.Text)
				{					
					return f;
				}
			}

			return null;
		}





#endregion



#region Listview invoke 관련

		delegate void delInvoke_ListView_ItemClear(ListView lv);
		/// <summary>
		/// listview에 item을 모두 제거 한다.
		/// </summary>
		/// <param name="lv"></param>
		public static void Invoke_ListView_ItemClear(ListView lv)
		{
			if (lv.InvokeRequired)
			{
				lv.Invoke(new delInvoke_ListView_ItemClear(Invoke_ListView_ItemClear), new object[] { lv });
				return;
			}

			lv.Items.Clear();

		}


		/// <summary>
		/// listview에 선택된 item을 모두 제거 한다.
		/// </summary>
		/// <param name="lv"></param>
		public static void Invoke_ListView_SelectedItemClear(ListView lv)
		{
			if (lv.InvokeRequired)
			{
				lv.Invoke(new delInvoke_ListView_ItemClear(Invoke_ListView_SelectedItemClear), new object[] { lv });
				return;
			}

			lv.SelectedItems.Clear();

		}



		delegate void delInvoke_DateTimePicker_Value(DateTimePicker dtp, DateTime value);
		/// <summary>
		/// Control에 Visible을 변경한다.
		/// </summary>
		/// <param name="lbl"></param>
		/// <param name="isVisible"></param>
		public static void Invoke_DateTimePicker_Value(DateTimePicker dtp, DateTime value)
		{
			if (dtp.InvokeRequired)
				dtp.Invoke(new delInvoke_DateTimePicker_Value(Invoke_DateTimePicker_Value), new object[] { dtp, value });
			else
				dtp.Value = value;
				

		}


		delegate void delInvoke_usrInputBox_Value(usrInputBox inp, string value);
		/// <summary>
		/// Control에 Visible을 변경한다.
		/// </summary>
		/// <param name="lbl"></param>
		/// <param name="isVisible"></param>
		public static void Invoke_usrInputBox_Value(usrInputBox inp, string value)
		{
			if (inp.InvokeRequired)
				inp.Invoke(new delInvoke_usrInputBox_Value(Invoke_usrInputBox_Value), new object[] { inp, value });
			else
				inp.Value = value;


		}


		delegate void delInvoke_ListView_AddItem(ListView lv, bool isClear, DataTable dt, string[] strColumn);

		/// <summary>
		/// listview에 datatable에서 item을 추가 한다.
		/// </summary>
		/// <param name="lv"></param>
		/// <param name="dt"></param>
		/// <param name="strColumn">datatabel 컬럼명 순서데로 filed 추가 : '__NO'는 count</param>
		public static void Invoke_ListView_AddItem(ListView lv, bool isClear, DataTable dt, string[] strColumn)
		{
			if (lv.InvokeRequired)
			{
				lv.Invoke(new delInvoke_ListView_AddItem(Invoke_ListView_AddItem), new object[] { lv, isClear, dt, strColumn });
				return;
			}

			if (isClear) lv.Items.Clear();

			string[] strValue = new string[strColumn.Length];

			int intRow = 1;
			foreach (DataRow dr in dt.Rows)
			{
				int intCol = 0;

				foreach (string strCol in strColumn)
				{
					if (strCol == string.Empty)
						strValue[intCol] = string.Empty;
					else if (strCol == "__NO")
						strValue[intCol] = (lv.Items.Count + 1).ToString();
					else
						strValue[intCol] = Fnc.obj2String(dr[strCol]);

					intCol++;
				}

				ListViewItem item = new ListViewItem(strValue);

				lv.Items.Add(item);

				intRow++;
			}

		}

		delegate void delInvoke_ListView_AddItem2(ListView lv, bool isClear, ListViewItem li, bool isToTop, int intMaxRowCount);

		public static void Invoke_ListView_AddItem(ListView lv, bool isClear, ListViewItem li, bool isToTop, int intMaxRowCount)
		{
			if (lv.InvokeRequired)
			{
				lv.Invoke(new delInvoke_ListView_AddItem2(Invoke_ListView_AddItem), new object[] { lv, isClear, li, isToTop, intMaxRowCount });
				return;
			}

			if (isClear) lv.Items.Clear();
			
			if (isToTop)
				lv.Items.Insert(0, li);
			else
				lv.Items.Add(li);

			while (lv.Items.Count > intMaxRowCount)
			{
				int intIndex;

				if (isToTop)
					intIndex = lv.Items.Count - 1;
				else
					intIndex = 0;

				lv.Items.RemoveAt(intIndex);

			}

		}

		delegate void delInvoke_ListView_SubItem_Text(ListView lv, ListViewItem li, int subitem_idx, string text);

		/// <summary>
		/// 리스트 뷰의 서브아이템 text변경한다.
		/// </summary>
		/// <param name="lv"></param>
		/// <param name="li"></param>
		/// <param name="subitem_idx"></param>
		/// <param name="text"></param>
		public static void Invoke_ListView_SubItem_Text(ListView lv, ListViewItem li, int subitem_idx, string text)
		{
			if (lv.InvokeRequired)
			{
				lv.Invoke(new delInvoke_ListView_SubItem_Text(Invoke_ListView_SubItem_Text), new object[] { lv, li, subitem_idx, text });
				return;
			}

			li.SubItems[subitem_idx].Text = text;
		}

		delegate void delInvoke_ListView_SubItem_Color(ListView lv, ListViewItem li, Color BackColor);

		/// <summary>
		/// 리스트 뷰의 서브아이템 Color변경한다.
		/// </summary>
		/// <param name="lv"></param>
		/// <param name="li"></param>
		/// <param name="subitem_idx"></param>
		/// <param name="text"></param>
		public static void Invoke_ListView_SubItem_Color(ListView lv, ListViewItem li, Color BackColor)
		{
			if (lv.InvokeRequired)
			{
				lv.Invoke(new delInvoke_ListView_SubItem_Color(Invoke_ListView_SubItem_Color), new object[] { lv, li, BackColor });
				return;
			}

			li.BackColor = BackColor;
		}



		delegate void delInvoke_ListView_AddItemString(ListView lv, bool isClear, string[] strData, bool isToTop, int intMaxRowCount);
		/// <summary>
		/// ListView에 string 배열로 부터 item을 추가 한다.
		/// </summary>
		/// <param name="lv"></param>
		/// <param name="isClear">기존데이터 삭제 여부</param>
		/// <param name="strData">추가 데이터 string 배열</param>
		/// <param name="isToTop">listview에 위에 데이터 추가</param>
		/// <param name="intMaxRowCount">최대 item 숫자</param>
		public static void Invoke_ListView_AddItemString(ListView lv, bool isClear, string[] strData, bool isToTop, int intMaxRowCount)
		{
			if (lv.InvokeRequired)
			{
				lv.Invoke(new delInvoke_ListView_AddItemString(Invoke_ListView_AddItemString), new object[] { lv, isClear, strData, isToTop, intMaxRowCount });
				return;
			}

			if (isClear) lv.Items.Clear();

			ListViewItem li = new ListViewItem(strData);

			if (isToTop)
				lv.Items.Insert(0, li);
			else
				lv.Items.Add(li);

			while (lv.Items.Count > intMaxRowCount)
			{
				int intIndex;

				if (isToTop)
					intIndex = lv.Items.Count - 1;
				else
					intIndex = 0;

				lv.Items.RemoveAt(intIndex);

			}


		}


		delegate void delInvoke_ListView_AddItemStringColor(ListView lv, bool isClear, string[] strData, bool isToTop, int intMaxRowCount, object colForeColor, object colBackColor);
		/// <summary>
		/// string 배열로 부터 item을 추가 및 색상을 설정한다.
		/// </summary>
		/// <param name="lv"></param>
		/// <param name="isClear">기존데이터 삭제 여부</param>
		/// <param name="strData">추가 데이터 string 배열</param>
		/// <param name="isToTop">listview에 위에 데이터 추가</param>
		/// <param name="intMaxRowCount">최대 item 숫자</param>
		/// <param name="colForeColor">글자색 (null : 기본색)</param>
		/// <param name="colBackColor">배경색 (null : 기본색)</param>
		public static void Invoke_ListView_AddItemStringColor(ListView lv, bool isClear, string[] strData, bool isToTop, int intMaxRowCount, object colForeColor, object colBackColor)
		{
			if (lv.InvokeRequired)
			{
				lv.Invoke(new delInvoke_ListView_AddItemStringColor(Invoke_ListView_AddItemStringColor), new object[] { lv, isClear, strData, isToTop, intMaxRowCount, colForeColor, colBackColor });
				return;
			}

			if (isClear) lv.Items.Clear();

			ListViewItem li = new ListViewItem(strData);

			if (colForeColor != null) li.ForeColor = (Color)colForeColor;
			if (colBackColor != null) li.BackColor = (Color)colBackColor;


			if (isToTop)
				lv.Items.Insert(0, li);
			else
				lv.Items.Add(li);

			while (lv.Items.Count > intMaxRowCount)
			{
				int intIndex;

				if (isToTop)
					intIndex = lv.Items.Count - 1;
				else
					intIndex = 0;

				lv.Items.RemoveAt(intIndex);

			}


		}



		delegate void delInvoke_ListView_ColumnHeader_Text(ListView li, ColumnHeader ch, string strText);

		/// <summary>
		/// listvivew에 컬럼해드 텍스트 변경 한다
		/// </summary>
		/// <param name="li"></param>
		/// <param name="ch"></param>
		/// <param name="strText"></param>
		public static void Invoke_ListView_ColumnHeader_Text(ListView li, ColumnHeader ch, string strText)
		{
			if (li.InvokeRequired)
			{
				li.Invoke(new delInvoke_ListView_ColumnHeader_Text(Invoke_ListView_ColumnHeader_Text), new object[] { li, ch, strText });
				return;
			}

			ch.Text = strText;
		}




		delegate void delInvoke_ListView_ColumnHeader_Width(ListView li, ColumnHeader ch, int intWidth);

		/// <summary>
		/// listvivew에 컬럼해드 너비 변경 한다
		/// </summary>
		/// <param name="li"></param>
		/// <param name="ch"></param>
		/// <param name="strText"></param>
		public static void Invoke_ListView_ColumnHeader_Width(ListView li, ColumnHeader ch, int intWidth)
		{
			if (li.InvokeRequired)
			{
				li.Invoke(new delInvoke_ListView_ColumnHeader_Width(Invoke_ListView_ColumnHeader_Width), new object[] { li, ch, intWidth });
				return;
			}

			ch.Width = intWidth;
		}


		/// <summary>
		/// 리스트뷰아이템에 서브아이템 text 리턴 한다
		/// </summary>
		/// <param name="li"></param>
		/// <param name="intSubItemIndex"></param>
		public static string ListViewItem_Get_SubItem(ListViewItem li, int intSubItemIndex)
		{
			return li.SubItems[intSubItemIndex].Text.Trim();
		}


		/// <summary>
		/// 리스트뷰의 항목 높이을 조절한다.
		/// </summary>
		/// <param name="lst"></param>
		/// <param name="height"></param>
		public static void ListView_ItemHeight_Set(ListView lst, int height)
		{
			ImageList dummyImageList = new ImageList();
			dummyImageList.ImageSize = new System.Drawing.Size(1, height);
			lst.SmallImageList = dummyImageList;
		}

#endregion



#region 콤보박스 invoke관련
		delegate void delInvoke_ComboBox_DataSource(ComboBox cmb, DataView dt, string strDisplayMember);
		/// <summary>
		/// 콤보박스에 Data Binding한다
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="dt"></param>
		/// <param name="strDisplayMember"></param>
		public static void Invoke_ComboBox_DataSource(ComboBox cmb, DataView dt, string strDisplayMember)
		{
			if (cmb.InvokeRequired)
			{
				cmb.Invoke(new delInvoke_ComboBox_DataSource(Invoke_ComboBox_DataSource), new object[] { cmb, dt, strDisplayMember });
				return;
			}

			cmb.DataSource = dt;
			cmb.DisplayMember = strDisplayMember;
			//cmb.SelectedIndex = -1;
		}



		delegate void delInvoke_ComboBox_AddItem(ComboBox cmb, string[] strItems);
		/// <summary>
		/// 콤보박스에 데이터를 추가하여 준다.
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="strItems"></param>
		public static void Invoke_ComboBox_AddItem(ComboBox cmb, string[] strItems)
		{

			if (cmb.InvokeRequired)
			{
				cmb.Invoke(new delInvoke_ComboBox_AddItem(Invoke_ComboBox_AddItem), new object[] { cmb, strItems });
				return;
			}

			foreach (string s in strItems)
			{
				cmb.Items.Add(s);
			}

		}

		/// <summary>
		/// Enum item을 콤보 박스에 넣는다.
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="e"></param>
		public static void Invoke_ComboBox_AddItem(ComboBox cmb, Enum e)
		{
			string[] values = Enum.GetNames(e.GetType());

			if (cmb.InvokeRequired)
				cmb.Invoke(new delInvoke_ComboBox_AddItem(Invoke_ComboBox_AddItem), new object[] { cmb, values });
			else
				Invoke_ComboBox_AddItem(cmb, values);
			
		}


		




		delegate void delInvoke_ComboBox_SelectedIndex(ComboBox cmb, int intIndex);
		/// <summary>
		/// 콤보박스에 SelectedIndex 값을 설정한다.
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="intIndex"></param>
		public static void Invoke_ComboBox_SelectedIndex(ComboBox cmb, int intIndex)
		{

			if (cmb.InvokeRequired)
			{
				cmb.Invoke(new delInvoke_ComboBox_SelectedIndex(Invoke_ComboBox_SelectedIndex), new object[] { cmb, intIndex });
				return;
			}

			//if (cmb.Items.Count > intIndex)	--에러를 처리 삭제
			cmb.SelectedIndex = intIndex;

		}


		delegate void delInvoke_ComboBox_SelectedItem(ComboBox cmb, string strField, string strSelectText);
		/// <summary>
		/// 바인딩된 콤보박스에 특정 필드에 값으로 아이템 선택
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="strField">필드명</param>
		/// <param name="strSelectText">찾는 값</param>
		public static void Invoke_ComboBox_SelectedItem(ComboBox cmb, string strField, string strSelectText)
		{

			if (cmb.InvokeRequired)
			{
				cmb.Invoke(new delInvoke_ComboBox_SelectedItem(Invoke_ComboBox_SelectedItem), new object[] { cmb, strField, strSelectText });
				return;
			}

			try
			{
				foreach (object obj in cmb.Items)
				{
					DataRowView dv = (DataRowView)obj;

					if (dv[strField].ToString() == strSelectText)
					{
						cmb.SelectedItem = obj;
						return;
					}
				}

				cmb.SelectedIndex = -1;
			}
			catch
			{
				cmb.SelectedIndex = -1;
			}

		}

		/// <summary>
		/// 콤보 박스에 선택된 값을 가져온다.(Invoke 아님)
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="strField"></param>
		public static string Invoke_ComboBox_GetSelectValue(ComboBox cmb, string strField)
		{
			if (cmb.SelectedIndex < 0) return string.Empty;

			DataRowView dv = (DataRowView)cmb.SelectedItem;

			return Fnc.obj2String(dv[strField]);

		}



		delegate void delInvoke_ComboBox_ClearItem(ComboBox cmb);
		/// <summary>
		/// 콤보박스에 item를 전부 삭제한다.
		/// </summary>
		/// <param name="cmb"></param>
		public static void Invoke_ComboBox_ClearItem(ComboBox cmb)
		{
			if (cmb.InvokeRequired)
			{
				cmb.Invoke(new delInvoke_ComboBox_ClearItem(Invoke_ComboBox_ClearItem), new object[] { cmb });
				return;
			}

			if (cmb.DataSource != null)
				cmb.DataSource = null;
			else
				cmb.Items.Clear();

		}
#endregion




		delegate void delInvoke_StatusStrip_LabelText(StatusStrip st, ToolStripLabel tl, string str, Image img);
		/// <summary>
		/// statusstrip에 라벨 변경 한다
		/// </summary>
		/// <param name="st"></param>
		/// <param name="tl"></param>
		/// <param name="str"></param>
		/// <param name="img"></param>
		public static void Invoke_StatusStrip_LabelText(StatusStrip st, ToolStripLabel tl, string str, Image img)
		{
			if (st.InvokeRequired)
			{
				st.Invoke(new delInvoke_StatusStrip_LabelText(Invoke_StatusStrip_LabelText), new object[] { st, tl, str, img });
				return;
			}

			tl.Text = str;

			tl.Image = img;


		}

		delegate void delInvoke_ToolStrip_LabelText(ToolStrip st, ToolStripLabel tl, string str, Image img);
		/// <summary>
		/// statusstrip에 라벨 변경 한다
		/// </summary>
		/// <param name="st"></param>
		/// <param name="tl"></param>
		/// <param name="str"></param>
		/// <param name="img"></param>
		public static void Invoke_ToolStrip_LabelText(ToolStrip st, ToolStripLabel tl, string str, Image img)
		{
			if (st.InvokeRequired)
			{
				st.Invoke(new delInvoke_StatusStrip_LabelText(Invoke_ToolStrip_LabelText), new object[] { st, tl, str, img });
				return;
			}

			tl.Text = str;

			tl.Image = img;


		}



		delegate void delInvoke_NumericUpDown_Value(NumericUpDown nm, int intValue);
		/// <summary>
		/// NumericUpDown의 값 변경
		/// </summary>
		/// <param name="nm"></param>
		/// <param name="intValue"></param>
		public static void Invoke_NumericUpDown_Value(NumericUpDown nm, int intValue)
		{
			if (nm.InvokeRequired)
			{
				nm.Invoke(new delInvoke_NumericUpDown_Value(Invoke_NumericUpDown_Value), new object[] { nm, intValue });
				return;
			}

			if (nm.Minimum > intValue)
				nm.Value = nm.Minimum;
			else if(nm.Maximum < intValue)
				nm.Value = nm.Maximum;
			else
				nm.Value = intValue;
		}



		delegate void delInvoke_CheckBox_Checked(CheckBox cb, bool value);
		/// <summary>
		/// 체크 박스에 값을 변경한다.
		/// </summary>
		/// <param name="cb"></param>
		/// <param name="value"></param>
		public static void Invoke_CheckBox_Checked(CheckBox cb, bool value)
		{
			if (cb.InvokeRequired)
			{
				cb.Invoke(new delInvoke_CheckBox_Checked(Invoke_CheckBox_Checked), new object[] { cb, value });
				return;
			}

			cb.Checked = value;
		}



		delegate void delInvoke_ContextMenuStrip_ItemAdd(ContextMenuStrip cms, string strName, string strText, Image img, EventHandler[] e, Keys shortkey = Keys.None);
		/// <summary>
		/// ContextMenuStrip에 아이템(목록)을 추가한다.
		/// </summary>
		/// <param name="cms"></param>
		/// <param name="strName"></param>
		/// <param name="strText"></param>
		/// <param name="img"></param>
		/// <param name="e"></param>
		public static void Invoke_ContextMenuStrip_ItemAdd(ContextMenuStrip cms, string strName, string strText, Image img, EventHandler[] e, Keys shortkey = Keys.None)
		{
			if (cms.InvokeRequired)
			{
				cms.Invoke(new delInvoke_ContextMenuStrip_ItemAdd(Invoke_ContextMenuStrip_ItemAdd), new object[] { cms, strName, strText, img, e, shortkey });
				return;
			}

			if (strName == string.Empty && strText == string.Empty)
			{
				ToolStripSeparator Spe = new ToolStripSeparator();
				cms.Items.Add(Spe);
			}
			else
			{
				ToolStripMenuItem tsi = new ToolStripMenuItem();
				tsi.Name = strName;
				tsi.Text = strText;
				tsi.Image = img;

				if (shortkey != Keys.None)
					tsi.ShortcutKeys = shortkey;

				foreach (EventHandler h in e)
				{
					tsi.Click += h;
				}
				cms.Items.Add(tsi);
			}

		}


		public delegate void delInvoke_ProgressBar_Value(ProgressBar bar, int intMaximum, int intValue);
		/// <summary>
		/// ProgressBar의 max값과 현재값을 변경한다.
		/// </summary>
		/// <param name="bar"></param>
		/// <param name="intMaximum"></param>
		/// <param name="intValue"></param>
		public static void Invoke_ProgressBar_Value(ProgressBar bar, int intMaximum, int intValue)
		{
			if (bar.InvokeRequired)
			{
				bar.Invoke(new delInvoke_ProgressBar_Value(Invoke_ProgressBar_Value), new object[] { bar, intMaximum, intValue });
				return;
			}

			bar.Maximum = intMaximum;
			bar.Value = intValue;
		}



		public enum enControl_Criteria { height, width };


		/// <summary>
		/// 컨트롤 사이즈로 원하는 폰트 크기를 넘겨 준다.
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="ccr">기존 사이즈 너비, 높이</param>
		/// <param name="criteria_string">기준으로 할 문자.</param>
		/// <param name="ratio">원하는 비율 0.01~1</param>
		/// <returns></returns>
		public static Font Font_Control_Resize_Get(Control ctrl, enControl_Criteria ccr, string criteria_string, float ratio)
		{

			Graphics g = Graphics.FromHwnd(ctrl.Handle);
			float s = 0;
			int cnt = 0;
			bool lastincrese = true;
			bool roop = true;
			Font rstFont = ctrl.Font;

			if (ratio > 1f)
				ratio = 1f;
			else if (ratio < 0.01f)
				ratio = 0.01f;

			while (roop)
			{
				if ((rstFont.Size + s) <= 0)
				{
					rstFont = ctrl.Font;
					break;
				}

				Font f = new Font(rstFont.FontFamily, rstFont.Size + s, rstFont.Style);

				float h, rt;
				if (ccr == enControl_Criteria.height)
				{
					h = g.MeasureString(criteria_string, f).Height;
					rt = h / ctrl.Height;
				}
				else
				{
					h = g.MeasureString(criteria_string, f).Width;
					rt = h / ctrl.Width;
				}



				bool increase = true;

				if (rt > (ratio - 0.05f) && rt < (ratio + 0.05f))
				{
					rstFont = f;
					roop = false;
				}
				else if (rt > ratio)
				{
					s--;
					increase = false;
				}
				else
				{
					s++;
				}

				if (cnt == 0)
					lastincrese = increase;
				else if (lastincrese != increase)
				{
					rstFont = f;
					roop = false;
				}

				cnt++;


			}


			return rstFont;

		}


		/// <summary>
		/// 컨트롤에서 사용할 문자의 길이를 구한다.
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="ccr"></param>
		/// <param name="criteria_string"></param>
		/// <returns></returns>
		public static float Font_Control_String_Size_Get(Control ctrl, enControl_Criteria ccr, string criteria_string )
		{
			Graphics g = Graphics.FromHwnd(ctrl.Handle);
			Font f = ctrl.Font;
			float h;

			if (ccr == enControl_Criteria.height)
			{
				h = g.MeasureString(criteria_string, f).Height;				
			}
			else
			{
				h = g.MeasureString(criteria_string, f).Width;				
			}

			return h;

		}



		static List<Control> _last_init_control = new List<Control>();

		/// <summary>
		/// 폼에 입력 항목을 초기화 한다.
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="parents">null이 아니면, 이 컨틀롤(컨데이너) 자식 컨트롤만 초기화 한다.</param>
		public static void InputForm_Init(Control ctrl, Control[] parents = null )
		{
			try
			{
				bool isInpCtrl = false;
				bool rtn = inputControl_Clear(ctrl, out isInpCtrl, parents);
				int idx;

				//입력항목 초기화 된다.
				if (rtn)
				{
					_last_init_control.Clear();
					_last_init_control.Add(ctrl);
					return;
				}
				else
				{

					if (_last_init_control.Count < 1 || !_last_init_control[0].Equals(ctrl))
					{	//첫입력임
						_last_init_control.Clear();
						_last_init_control.Add(ctrl);
						//입력창은 종료
						if (isInpCtrl) return;
					}

				}

				


				idx = _last_init_control.Count - 1;
				Control tCtrl = _last_init_control[idx].Parent;

				if (tCtrl == null) return;

				container_Clear(tCtrl, parents);
				_last_init_control.Add(tCtrl);
			}
			catch { }
			finally
			{
				ctrl.Focus();
			}
		}


		static void container_Clear(Control ctrl, Control[] parents = null)
		{
			bool isInp = false;

			inputControl_Clear(ctrl, out isInp);

			foreach (Control c in ctrl.Controls)
			{
				inputControl_Clear(c, out isInp, parents);
				container_Clear(c);
			}

		}

		/// <summary>
		/// 항목값을 초기화 한다.
		/// </summary>
		/// <param name="ctrl">초기화 여부</param>
		/// <returns></returns>
		static bool inputControl_Clear(Control ctrl, out bool isInpCtrl, Control[] parents = null)
		{
			bool rtn = false;
			isInpCtrl = false;
			bool chkParents = false;

			if (parents == null)
				chkParents = true;
			else
			{
				foreach (Control c in parents)
				{
					if (c.Equals(ctrl.Parent))
					{
						chkParents = true;
						break;
					}
				}
			}
			


			//값을 확인하고 있으면 초기화 한다.
			switch (ctrl.GetType().ToString())
			{
				case "System.Windows.Forms.ComboBox":
					ComboBox b = ctrl as ComboBox;
					isInpCtrl = true;
					if (chkParents && b.SelectedIndex != -1)
					{
						b.SelectedIndex = -1;
						rtn = true;
					}
					break;

				case "System.Windows.Forms.TextBox":
				case "System.Windows.Forms.MaskedTextBox":
					//TextBox t = ctrl as TextBox;
					isInpCtrl = true;
					if (chkParents && !ctrl.Text.Equals(string.Empty))
					{
						ctrl.Text = string.Empty;
						rtn = true;
					}
					break;				

				case "Function.form.usrInputBox":
					usrInputBox ib = ctrl as usrInputBox;
					isInpCtrl = true;
					if (chkParents)
					{
						if ((ib.Value.Equals(string.Empty) && ib.Text != string.Empty))
						{
							ib.Text = string.Empty;
							rtn = true;
						}
						else if (ib.Value != ib.Text)
						{
							ib.Value = ib.Text;
							rtn = true;
						}
					}
					break;
			}

			return rtn;
		}



		/// <summary>
		/// 입력 컨트롤 여부를 확인한다.
		/// </summary>
		/// <param name="ctrl"></param>
		/// <returns></returns>
		static bool isInputContol(Control ctrl)
		{
			bool rtn = false;
			switch (ctrl.GetType().ToString())
			{
				case "ComboBox":
				case "TextBox":
				case "usrInputBox":
					rtn = true;
					break;
			}

			return rtn;
		}


		/// <summary>
		/// 컨트롤에 탭문자 길이를 설정 한다.
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="length"></param>
		public static void SetTabStopsLength(Control ctrl, int len)
		{
			// 1 means all tab stops are the the same length
			// This means lParam must point to a single integer that contains the desired tab length
			const uint regularLength = 1;

			// A dialog unit is 1/4 of the average character width
			int length = len * api.unitsPerCharacter;

			// Pass the length pointer by reference, essentially passing a pointer to the desired length
			IntPtr lengthPointer = new IntPtr(length);
			api.SendMessage(ctrl.Handle, api.EM_SETTABSTOPS, (IntPtr)regularLength, ref lengthPointer);
		}


	}



	/// <summary>
	/// 폼내에 콘트롤을 사이즈 조절 할 수 있도록 하여 준다..
	/// </summary>
	public class Sizable_Control_Set
	{
		bool isCursorWSize = false;
		bool isCursorHSize = false;
		bool isClick = false;
		int iX = 0;
		int iW = 0;
		int iH = 0;
		int iY = 0;
		Dictionary<Control, Control> dicControl = new Dictionary<Control, Control>();
		Control selectedControl = null;
		Control mdiclient = null;
		Form mainForm;

		/// <summary>
		/// 인스턴스 생성 하면서 
		/// </summary>
		/// <param name="f"></param>
		public Sizable_Control_Set(Form f)
		{
			//mdiClient를 찾아야 한다.
			mainForm = f;

			foreach (Control s in f.Controls)
			{
				if (s.GetType() == typeof(MdiClient))
				{
					mdiclient = s;

					s.MouseMove += new MouseEventHandler(c_MouseMove);
					s.MouseUp += new MouseEventHandler(c_MouseUp);
					s.MouseDown += new MouseEventHandler(c_MouseDown);

					break;
				}
			}

			if (mdiclient == null) throw new Exception("MdiClient control을 찾을 수 없습니다.");

		}

		/// <summary>
		/// 사이즈 조정할 컨트롤을 추가 한다.
		/// </summary>
		/// <param name="c"></param>
		public void Control_Add(Control c)
		{
			c.MouseMove += new MouseEventHandler(c_MouseMove);
			c.MouseUp += new MouseEventHandler(c_MouseUp);
			c.MouseDown += new MouseEventHandler(c_MouseDown);

			dicControl.Add(c, c);
		}
		


		void c_MouseDown(object sender, MouseEventArgs e)
		{
			if ( (!isCursorWSize & !isCursorHSize) || selectedControl == null) return;

			isClick = true;
			iW = selectedControl.Width;
			iH = selectedControl.Height;
			iX = e.X + selectedControl.Left;
			iY = e.Y + selectedControl.Top;
			
		}

		void c_MouseUp(object sender, MouseEventArgs e)
		{
			if (isClick) isClick = false;
		}		

		void c_MouseMove(object sender, MouseEventArgs e)
		{
			Control c = (Control)sender;		


			//if (!dicControl.ContainsKey(c)) return;
			int X = c.Left + e.X;
			int Y = c.Top + e.Y;

			if (isClick && selectedControl != null)
			{

				if (e.Button != MouseButtons.Left)
				{
					isClick = false;
					c.Cursor = Cursors.Default;
					return;
				}

				
				int iOffset = 0;

				if (isCursorWSize)
				{

					iOffset = iW + X - iX;
					if (iOffset < 5)
						iOffset = 5;
					else if (iOffset > mainForm.Width)
						iOffset = mainForm.Width;

					selectedControl.Width = iOffset;
				}
				else if (isCursorHSize)
				{
					iOffset = iH + Y - iY;
					if (iOffset < 5)
						iOffset = 5;
					else if (iOffset > mainForm.Height)
						iOffset = mainForm.Height;

					selectedControl.Height = iOffset;
				}

				return;
			}

			int iRange = 12;

			foreach (Control t in dicControl.Keys)
			{
				//if (X > t.Left + t.Width - iRange && X < t.Left + t.Width)
                if (X > t.Left + t.Width && X < t.Left + t.Width + iRange)
				{
					c.Cursor = Cursors.SizeWE;
					selectedControl = t;
					isCursorWSize = true;
					return;
				}
				else if (Y > t.Top + t.Height - iRange && Y < t.Top + t.Height)
				{
					c.Cursor = Cursors.SizeNS;
					selectedControl = t;
					isCursorHSize = true;
					return;
				}
				
			}

			c.Cursor = Cursors.Default;
			isCursorWSize = false;
			
		}



	}







}
