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
	public partial class usrEditLabel : usrUserControlItemBase
	{
		/// <summary>
		/// 현재 수정 모드 여부
		/// </summary>
		bool _isEditMode = false;		


		public usrEditLabel()
		{
			InitializeComponent();

			ValueChanged += new EventHandler(usrEditLabel_ValueChanged);

			LostFocus += new EventHandler(usrEditLabel_LostFocus);
			_selectedChanged += new EventHandler(usrEditLabel__selectedChanged);
		}

		void usrEditLabel__selectedChanged(object sender, EventArgs e)
		{
			if (!textBox.Visible || IsSelected) return;

			textBox.Visible = false;
		}

		void usrEditLabel_LostFocus(object sender, EventArgs e)
		{
			if (!textBox.Visible) return;

			textBox.Visible = false;
		}

		void usrEditLabel_ValueChanged(object sender, EventArgs e)
		{
			label.Text = Fnc.obj2String(Value);
		}

		private void itmEditLabel_FontChanged(object sender, EventArgs e)
		{
			label.Font = textBox.Font = this.Font;
		}

		/// <summary>
		/// 컨트롤에 왼쪽 여백을 가져오거나 설정한다.
		/// </summary>
		public int MarginLeft
		{
			get { return pnlleft.Width; }
			set
			{
				pnlleft.Width = value;
			}
		}

		/// <summary>
		/// 컨트롤에 왼쪽 여백을 가져오거나 설정한다.
		/// </summary>
		public int MarginRight
		{
			get { return pnlRight.Width; }
			set
			{
				pnlRight.Width = value;
			}
		}

		

		/// <summary>
		/// 라벨 더블클릭 -> 항목 수정 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void label_DoubleClick(object sender, EventArgs e)
		{
			textBox.Text = Fnc.obj2String(_value);
			textBox.Visible = true;
			textBox.BringToFront();

			textBox.SelectAll();
			textBox.Focus();
		}

		private void textBox_KeyDown(object sender, KeyEventArgs e)
		{
			bool end = true;
			bool cancel = true;
			string text = textBox.Text.Trim();

			switch (e.KeyCode)
			{
				case Keys.Enter:
					cancel = textBox.Equals(string.Empty);					
					break;

				case Keys.Escape:					
					break;

				default:
					end = false;
					break;
			}

			if (!end) return;

			//값 변경 처리
			if (!cancel)
			{
				Value = text;
			}

			textBox.Visible = false;

		}



	}
}
