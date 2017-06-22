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
	/// Form control ���� Ŭ����..
	/// </summary>
	public class control
	{
		/// <summary>
		/// �ߺ� ���� üũ �Ѵ�.
		/// </summary>
		/// <param name="strProgramName"></param>
		/// <param name="isCreateNew"></param>
		/// <returns></returns>
		public static Mutex ProgramRunCheck(string strProgramName, out bool isCreateNew)
		{
			Mutex gM = new Mutex(true, strProgramName, out isCreateNew);

			//�޾Ƽ� false�̸� �̹� ����, true�̸� realease�� ���� �Ͽ� �ش�.
			return gM;
		}


		/// <summary>
		/// �ڽ�â�� �θ�â �߾ӿ� �̵� ��Ų��.
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

#region ����Control ���� Invoke		


		delegate void delInvoke_Control_Enabled(Control ctl, bool Enabled);
		/// <summary>
		/// Control Enabled ���� �����Ѵ�.
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
		/// Control�� TEXT�� �����Ѵ�.
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
		/// Control�� TEXT�� �߰�/�����Ѵ�.
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="strText"></param>
		/// <param name="isClear">���� Text���� ����</param>
		/// <param name="isFront">Text�߰��� �պκп� �߰� ����</param>
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
		/// Control�� Top ���� �����Ѵ�.
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
		/// Control�� Left ���� �����Ѵ�.
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
		/// Control�� Width ���� �����Ѵ�.
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
		/// Control�� Visible�� �����Ѵ�.
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
		/// Control�� ForeColor / BackColor�� �����Ѵ�.
		/// </summary>
		/// <param name="ctl"></param>
		/// <param name="colFore">���� ��(null �Է½� ���� ����)</param>
		/// <param name="colBack">��� ��(null �Է½� ���� ����)</param>
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
		/// Control�� ��Ʈ�� �����Ѵ�.
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
		/// ��Ʈ�ѿ� ��Ʈ���� �߰� �Ͽ��ش�.
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
		/// �׷� �ʿ� �ִ� ��Ʈ���� �ʱ�ȭ �Ѵ�.
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
		


#region �� invoke ����
		delegate void delInvoke_Form_ControlAdd(Form frm, Control ctl);
		/// <summary>
		/// �i�� ��Ʈ���� �߰� �Ѵ�.
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
		/// ���� ������ �����Ѵ�.
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
		/// ���� �ݴ´�
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
		/// mdi �θ�â�� �ڽ�â�� ����..
		/// </summary>
		/// <param name="frmMdi"></param>
		/// <param name="frmChild"></param>
		/// <param name="isCheckDuplication">�ߺ�üũ form�� text�� �Ѵ�.</param>
		public static void Invoke_Form_Add_MdiChild(Form frmMdi, Form frmChild, bool isCheckDuplication)
		{
			//�� �ߺ� üũ�� �Ѵ�.
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
		/// �� text�� �θ�â���� �ڽ� ���� ã�´�..
		/// </summary>
		/// <param name="frmMdi">�θ� ��</param>
		/// <param name="frmChild">�ڽ� ��</param>
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



#region Listview invoke ����

		delegate void delInvoke_ListView_ItemClear(ListView lv);
		/// <summary>
		/// listview�� item�� ��� ���� �Ѵ�.
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
		/// listview�� ���õ� item�� ��� ���� �Ѵ�.
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
		/// listview�� datatable���� item�� �߰� �Ѵ�.
		/// </summary>
		/// <param name="lv"></param>
		/// <param name="dt"></param>
		/// <param name="strColumn">datatabel �÷��� �������� filed �߰� : '__NO'�� count</param>
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
		/// ListView�� string �迭�� ���� item�� �߰� �Ѵ�.
		/// </summary>
		/// <param name="lv"></param>
		/// <param name="isClear">���������� ���� ����</param>
		/// <param name="strData">�߰� ������ string �迭</param>
		/// <param name="isToTop">listview�� ���� ������ �߰�</param>
		/// <param name="intMaxRowCount">�ִ� item ����</param>
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
		/// string �迭�� ���� item�� �߰� �� ������ �����Ѵ�.
		/// </summary>
		/// <param name="lv"></param>
		/// <param name="isClear">���������� ���� ����</param>
		/// <param name="strData">�߰� ������ string �迭</param>
		/// <param name="isToTop">listview�� ���� ������ �߰�</param>
		/// <param name="intMaxRowCount">�ִ� item ����</param>
		/// <param name="colForeColor">���ڻ� (null : �⺻��)</param>
		/// <param name="colBackColor">���� (null : �⺻��)</param>
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
		/// listvivew�� �÷��ص� �ؽ�Ʈ ���� �Ѵ�
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
		/// listvivew�� �÷��ص� �ʺ� ���� �Ѵ�
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
		/// ����Ʈ������ۿ� ��������� text ���� �Ѵ�
		/// </summary>
		/// <param name="li"></param>
		/// <param name="intSubItemIndex"></param>
		public static string ListViewItem_Get_SubItem(ListViewItem li, int intSubItemIndex)
		{
			return li.SubItems[intSubItemIndex].Text.Trim();
		}

#endregion



#region �޺��ڽ� invoke����
		delegate void delInvoke_ComboBox_DataSource(ComboBox cmb, DataTable dt, string strDisplayMember);
		/// <summary>
		/// �޺��ڽ��� Data Binding�Ѵ�
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
		/// �޺��ڽ��� �����͸� �߰��Ͽ� �ش�.
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
		/// �޺��ڽ��� SelectedIndex ���� �����Ѵ�.
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

			//if (cmb.Items.Count > intIndex)	--������ ó�� ����
			cmb.SelectedIndex = intIndex;

		}


		delegate void delInvoke_ComboBox_SelectedItem(ComboBox cmb, string strField, string strSelectText);
		/// <summary>
		/// ���ε��� �޺��ڽ��� Ư�� �ʵ忡 ������ ������ ����
		/// </summary>
		/// <param name="cmb"></param>
		/// <param name="strField">�ʵ��</param>
		/// <param name="strSelectText">ã�� ��</param>
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
		/// �޺� �ڽ��� ���õ� ���� �����´�.(Invoke �ƴ�)
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
		/// �޺��ڽ��� item�� ���� �����Ѵ�.
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
		/// statusstrip�� �� ���� �Ѵ�
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
		/// NumericUpDown�� �� ����
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
		/// üũ �ڽ��� ���� �����Ѵ�.
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
		/// ContextMenuStrip�� ������(���)�� �߰��Ѵ�.
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
		/// ProgressBar�� max���� ���簪�� �����Ѵ�.
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
