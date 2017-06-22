using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace Function.form
{
	/// <summary>
	/// �� �̺�Ʈ ���� ���� Ŭ����
	/// </summary>
	public class controlEvent
	{

		private static void TextBox_ImeModeChanged_OnlyAlpha(object sender, EventArgs e)
		{
			TextBox tb = (TextBox)sender;

			if (tb.ImeMode != ImeMode.AlphaFull)
			{
				tb.ImeMode = ImeMode.AlphaFull;
			}
		}


		/// <summary>
		/// �ؽ�Ʈ �ڽ� �Է½� �ѹ��� �Է� �� �� �ֵ��� �Ѵ�.
		/// </summary>
		public static void TextBox_Press_NumberOnly(TextBox tb)
		{
			tb.KeyPress += new KeyPressEventHandler(TextBox_KeyPress_NumberOnly);
			//tb.ImeMode = ImeMode.On;
			tb.ImeModeChanged += new EventHandler(TextBox_ImeModeChanged_OnlyAlpha);
		}

		/// <summary>
		/// �ؽ�Ʈ �ڽ� �Է½� �ѹ��� �Է� �� �� �ִ� �̺�Ʈ�� ���� �Ѵ�.
		/// </summary>
		/// <param name="tb"></param>
		public static void TextBox_Press_NumberOnly_Remove(TextBox tb)
		{
			tb.KeyPress -= new KeyPressEventHandler(TextBox_KeyPress_NumberOnly);
			//tb.ImeMode = ImeMode.On;
			tb.ImeModeChanged -= new EventHandler(TextBox_ImeModeChanged_OnlyAlpha);
		}
		
		private static void TextBox_KeyPress_NumberOnly(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar >= 48 && e.KeyChar <= 0x57 || e.KeyChar == 0x08 || e.KeyChar == 0x2E
				|| e.KeyChar == 0x2E || e.KeyChar == 0x2D || e.KeyChar == 44)		//---- 0 ~ 9 / bSpc / . / - / ,
			{
				e.Handled = false;
			}
			else
			{
				e.Handled = true;
			}
		}

		public static void TextBox_Press_None(TextBox tb)
		{
			tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);			
		}

		public static void TextBox_Press_None_Remove(TextBox tb)
		{
			tb.KeyPress -= new KeyPressEventHandler(tb_KeyPress);
		}


		static void tb_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = true;
		}


		/// <summary>
		/// ��Ʈ�� �ȿ� �ؽ�Ʈ �ڽ��� �����̺�Ʈ �߰��Ѵ�
		/// </summary>
		/// <param name="ctl"></param>
		public static void Control_Set_inTextBox_Enter2NextControl(Control ctl)
		{
			foreach (Control c in ctl.Controls)
			{
				TextBox tb = c as TextBox;
				if (tb != null) TextBox_Enter2NextControl(tb);
			}
		}

		/// <summary>
		/// �ؽ�Ʈ �ڽ����� ���ʹ����� ���� ��Ʈ�ѷ� �̵� ��Ų��
		/// </summary>
		/// <param name="tb"></param>
		public static void TextBox_Enter2NextControl(TextBox tb)
		{
			tb.KeyDown += new KeyEventHandler(TextBox_KeyDown);			
		}

		static void TextBox_KeyDown(object sender, KeyEventArgs e)
		{

			if (e.KeyCode == Keys.Enter)
			{
				TextBox tb = (TextBox)sender;

				Control ctl = tb.FindForm().GetNextControl(tb, true);

				ctl.Focus();
			}
			
		}


		static System.Collections.Generic.Dictionary<Control, System.Drawing.ContentAlignment> SetAlignment_ArrList  = null;
		static System.Collections.Generic.List<Control> SetAlignment_ParArrList = null;

		/// <summary>
		/// ��Ʈ�� ��ġ�� �����Ѵ�. - �θ� ��Ʈ�� ����� ����� ���� ������ �Ͽ� �ش�
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="ali"></param>
		public static void Control_SetAlignment(Control ctrl, System.Drawing.ContentAlignment ali)
		{
			if (ctrl.Parent == null) return;

			if (SetAlignment_ArrList == null)
				SetAlignment_ArrList = new System.Collections.Generic.Dictionary<Control, System.Drawing.ContentAlignment>();

			if (SetAlignment_ParArrList == null)
				SetAlignment_ParArrList = new System.Collections.Generic.List<Control>();

			if (!SetAlignment_ParArrList.Contains(ctrl.Parent))
			{
				ctrl.Parent.SizeChanged += Parent_SizeChanged;
				SetAlignment_ParArrList.Add(ctrl.Parent);
            }

			if (!SetAlignment_ArrList.ContainsKey(ctrl))
			{				
				SetAlignment_ArrList.Add(ctrl, ali);
			}
			
		}

		private static void Parent_SizeChanged(object sender, EventArgs e)
		{
			Control ctrl = sender as Control;

			if (ctrl == null) return;

			foreach (Control c in ctrl.Controls)
			{
				if(SetAlignment_ArrList.ContainsKey(c))
				{
					formFunction.ControlAlign(c, SetAlignment_ArrList[c]);
                }
            }
        }
	}
}
