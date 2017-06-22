using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Function.form
{
	/// <summary>
	/// textbox에 포커스를 가지면 항목 입력한 별도의 컨트롤을 보여 입력
	/// 입력중 tab을 누르거나 컨트롤을 벗어나면 입력 되고 esc를 누르면 취소
	/// 사용법 : Function.form.usrMultiTextItem.Set(textbox);
	/// 입력된 데이터는 : Text.Split(new char[] { (char)3 });
	/// </summary>
	public partial class usrMultiTextItem : UserControl
	{

		static TextBox _curr = null;
		static usrMultiTextItem _lastti = null;

		/// <summary>
		/// textbox에 포커스를 가지면 항목 입력한 별도의 컨트롤을 보여 입력
		/// 입력중 tab을 누르거나 컨트롤을 벗어나면 입력 되고 esc를 누르면 취소
		/// 사용법 : Function.form.usrMultiTextItem.Set(textbox);
		/// 입력된 데이터는 : Text.Split(new char[] { (char)3 });
		/// </summary>
		/// <param name="tBox"></param>
		public static void Set(TextBox tBox)
		{
			tBox.GotFocus += new EventHandler(tBox_GotFocus);
			tBox.TextChanged += new EventHandler(tBox_TextChanged);
		}		

		public static void Remove(TextBox tBox)
		{
			tBox.GotFocus -= new EventHandler(tBox_GotFocus);			
			tBox.TextChanged -= new EventHandler(tBox_TextChanged);
		}

		static bool _isInit = false;

		/// <summary>
		/// 코드로 자동 폼데이터 처리시 오류발행 할 수 있으므루 BeginInit ~ EndInit를 사용하여 방지 하여 준다.
		/// </summary>
		public static void BeginInit()
		{
			_isInit = true;
		}

		
		public static void EndInit()
		{
			_isInit = false;
		}

		static void tBox_TextChanged(object sender, EventArgs e)
		{
			if (_isInit || _lastti == null || !_lastti.IsDisposed) return;

			usrMultiTextItem ti = new usrMultiTextItem((TextBox)sender);
			_lastti = ti;
		}

		static void tBox_GotFocus(object sender, EventArgs e)
		{
			if (_isInit) return;

			TextBox tbox = (TextBox)sender;
			if ( (_curr != null & tbox.Equals(_curr)) ) // || (_lastti != null && tbox.Equals(_lastti.tbox)) ) 
			{
				return;
			}

			_curr = tbox;
			usrMultiTextItem ti = new usrMultiTextItem(tbox);			
			_lastti = ti;
		}



		Form frm;
		TextBox tbox;
		static char _seperator = (char)3;

		/// <summary>
		/// 항목간 구분자를 가져온다.
		/// </summary>
		public static char Seperator
		{
			get { return _seperator; }
		}

		
		bool _canceled = false;

		string _old_value;

		//public static bool operator == (usrMultiTextItem left, usrMultiTextItem right)
		//{
		//    if (left == null)
		//    {
		//        return right == null;
		//    }
		//    return left.tbox.Equals(right.tbox);
		//}

		//public static bool operator !=(usrMultiTextItem left, usrMultiTextItem right)
		//{
		//    return !left.tbox.Equals(right.tbox);
		//}

		public bool Equals(usrMultiTextItem TextItem)
		{
			return this == TextItem;
		}

		public usrMultiTextItem(TextBox tBox)
		{
			InitializeComponent();

			tbox = tBox;
			frm = tbox.FindForm();

			frm.Controls.Add(this);

			Point loc = Absolute_Location_Get(tbox);
			loc.Y += tbox.Height;
			this.Location = loc;
			this.BringToFront();
			this.Width = tbox.Width;
			_old_value = tbox.Text;
			textBox1.Focus();

			textBox1.Leave += new EventHandler(textBox1_Leave);
			textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
		}

		void textBox1_TextChanged(object sender, EventArgs e)
		{
			string[] st = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			tbox.Text = string.Join(_seperator.ToString(), st);
		}

		private Point Absolute_Location_Get(Control ctrl)
		{
			Form f = ctrl.Parent as Form;
			if (f != null && !f.IsMdiChild)
			{
				return new Point(0, 0);
			}

			Control p = ctrl.Parent as Control;

			if (p == null)
			{
				return p.Location;
			}

			Point pnt = Absolute_Location_Get(p);
			pnt.X += ctrl.Location.X;
			pnt.Y += ctrl.Location.Y;

			return pnt;
		}


		private void textBox1_Leave(object sender, EventArgs e)
		{
			if (!_canceled)
			{
				string[] st = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

				tbox.Text = string.Join(_seperator.ToString(), st);
				_curr = null;
			}
			else
			{
				tbox.Text = _old_value;
				tbox.Focus();
			}

			Control ct = form.control.FocuedControlFind(frm);
			//Console.WriteLine("before remove:{0}", ct.Name);

			frm.Controls.Remove(this);

			//tbox.Focus();

			//tbox.Parent.Focus();

			//ct = form.control.FocuedControlFind(frm);
			//if(ct != null) Console.WriteLine("after remove:{0}", ct.Name);

			//Control c = tbox.Parent.Controls["txtIpAddress"];    //.GetNextControl(tbox, false);
			//if (c != null)
			//{
			//    Console.WriteLine(c.Name);
			//    c.Focus();
			//}
			//else
			//{
			//    Console.WriteLine("Null");
			//}

			//ct = form.control.FocuedControlFind(frm);
			//Console.WriteLine("after focus:{0}", ct.Name);


			//tbox.Parent.SelectNextControl(tbox, true, true, true, false);

			

			this.Dispose();

			//ct = form.control.FocuedControlFind(frm);
			//Console.WriteLine("after dispose:{0}", ct.Name);

		}

		private void usrMultiTextItem_Load(object sender, EventArgs e)
		{			

			string[] st = tbox.Text.Split(new char[] { _seperator });

			textBox1.Text = string.Join("\r\n", st);

			textBox1.SelectAll();

			

		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case Keys.Escape:
					_canceled = true;
					textBox1_Leave(null, null);
					break;

			}
		}

		
	}
}
