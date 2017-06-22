using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Threading;


namespace AutoUpdateClient
{
	/// <summary>
	/// Form control 관련 클래스..
	/// </summary>
	public class control
	{
		/// <summary>
		/// 중복 실행 체크 한다.
		/// </summary>
		/// <param name="strProgramName"></param>
		/// <param name="isCreateNew"></param>
		/// <returns></returns>
		public static Mutex ProgramRunCheck(string strProgramName, out bool isCreateNew)
		{
			Mutex gM = new Mutex(true, strProgramName, out isCreateNew);

			//받아서 false이면 이미 실행, true이면 realease를 실행 하여 준다.
			return gM;
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

#region 공용Control 관련 Invoke		


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
				ctl.Top = intLeft;
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

				ControlInReset(c);
			}

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

#endregion



#region 콤보박스 invoke관련
		delegate void delInvoke_ComboBox_DataSource(ComboBox cmb, DataTable dt, string strDisplayMember);
		/// <summary>
		/// 콤보박스에 Data Binding한다
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="dt"></param>
		/// <param name="strDisplayMember"></param>
		public static void Invoke_ComboBox_DataSource(ComboBox cmb, DataTable dt, string strDisplayMember)
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



		delegate void delInvoke_ContextMenuStrip_ItemAdd(ContextMenuStrip cms, string strName, string strText, Image img, EventHandler e);
		/// <summary>
		/// ContextMenuStrip에 아이템(목록)을 추가한다.
		/// </summary>
		/// <param name="cms"></param>
		/// <param name="strName"></param>
		/// <param name="strText"></param>
		/// <param name="img"></param>
		/// <param name="e"></param>
		public static void Invoke_ContextMenuStrip_ItemAdd(ContextMenuStrip cms, string strName, string strText, Image img, EventHandler e)
		{
			if (cms.InvokeRequired)
			{
				cms.Invoke(new delInvoke_ContextMenuStrip_ItemAdd(Invoke_ContextMenuStrip_ItemAdd), new object[] { cms, strName, strText, img, e });
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
				tsi.Click += e;
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


	}
}
