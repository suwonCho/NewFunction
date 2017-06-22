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
	public delegate void EventClickButton(object sender, object value);

	public partial class usrNumberInput : UserControl
	{
		string buttons = "btn7;btn8;btn9;btn4;btn5;btn6;btn1;btn2;btn3;btn0;btnC;btnB;";

		bool lastComma = false;

		string _number = string.Empty;
		/// <summary>
		/// 현재 숫자를 가져오거나 설정한다.
		/// </summary>
		public string Number
		{
			get { return _number; }
			set
			{
				if (!Fnc.isNumeric(value)) return;
				_number = Convert.ToDouble(value).ToString();
				if (!_showZero && _number.Equals("0"))
					txtNumber.Text = string.Empty;
				else
					txtNumber.Text = _number;

				if (_onNumberChanged != null) _onNumberChanged(this, new EventArgs());
			}
		}

		/// <summary>
		/// 초기 값(변경전 값)
		/// </summary>
		public string Value = "0";



		event EventHandler _onNumberChanged;
		/// <summary>
		/// 넘버가 변경 되면 발생한다.
		/// </summary>
		public event EventHandler OnNumberChanged
		{
			add { _onNumberChanged += value; }
			remove { _onNumberChanged -= value; }
		}

		event EventClickButton _onNumberButtonClick;

		public event EventClickButton OnNumberButtonClick
		{
			add { _onNumberButtonClick += value; }
			remove { _onNumberButtonClick -= value; }
		}



		bool _showNumberText = true;

		/// <summary>
		/// 입력 숫자 표시 여부
		/// </summary>
		public bool ShowNumberText
		{
			get { return _showNumberText; }
			set
			{
				_showNumberText = value;
				spMain.Panel1Collapsed = !_showNumberText;
				usrNumberInput_Resize(null, null);
			}
		}


		bool _showZero = true;

		/// <summary>
		/// 값이 '0'일 경우 표시 여부를 가져오거나 설정한다.
		/// </summary>
		public bool ShowZero
		{
			get { return _showZero; }
			set
			{
				_showZero = value;

				Number = _number;
			}
		}

		bool _showOkButton = true;

		/// <summary>
		/// 값이 '0'일 경우 표시 여부를 가져오거나 설정한다.
		/// </summary>
		public bool ShowOkButton
		{
			get { return _showOkButton; }
			set
			{
				_showOkButton = value;

				usrNumberInput_Resize(null, null);
			}
		}

		event EventHandler _btnOk_Click;
		/// <summary>
		/// ok 버튼을 누르면 발생한다.
		/// </summary>
		public event EventHandler BtnOk_Click
		{
			add { _btnOk_Click += value; }
			remove { _btnOk_Click -= value; }
		}


		bool _showComma = true;

		/// <summary>
		/// 콤마('.')표시 여부를 가져오거나 설정한다.
		/// </summary>
		public  bool ShowComma
		{
			get { return _showComma;  }
			set
			{
				_showComma = value;
				btnC.Visible = _showComma;
			}
		}



		public usrNumberInput()
		{
			InitializeComponent();

			Number = "0";
			
			
		}

		public usrNumberInput(double value, bool showComma = true)
		{
			Number = Value = value.ToString();
	
		}


		private void init()
		{
			controlEvent.TextBox_Press_NumberOnly(txtNumber);
			this.Resize += usrNumberInput_Resize;
			usrNumberInput_Resize(null, null);
			txtNumber.SelectionLength = 0;
		}
		
		/// <summary>
		/// 숫자표시창 폰트를 조정한다.
		/// </summary>
		void txtNumber_FontResize()
		{
			string str = "9";
			str = str.PadLeft(txtNumber.MaxLength, '9');

			txtNumber.Font = Function.form.control.Font_Control_Resize_Get(txtNumber, control.enControl_Criteria.width, str, 0.65f);

		}

		void usrNumberInput_Resize(object sender, EventArgs e)
		{
			txtNumber_FontResize();

			int span = 5;
			int height = spMain.Panel2.Height;


			if (ShowOkButton)
			{
				btnOk.Visible = true;
				btnOk.Height = height * 15 / 100;
				btnOk.Font = Function.form.control.Font_Control_Resize_Get(btnOk, control.enControl_Criteria.height, btnOk.Text, 0.75f);				
				height = height - btnOk.Height;
			}
			else
				btnOk.Visible = false;

			height = (height - (span * 5)) / 4;

			string[] btns = buttons.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
			int top = span;
			int left = span;
			int width = (spMain.Panel2.Width - (span * 4)) / 3;
			Button btn;
			int col = 1;
			Font btnFont = null;

			txtNumber.TextAlign = HorizontalAlignment.Left;
			txtNumber.TextAlign = HorizontalAlignment.Right;

			for(int i = 0; i < btns.Length; i++)
			{
				btn = spMain.Panel2.Controls[btns[i]] as Button;

				if (btn == null) continue;

				btn.Top = top;
				btn.Left = left;
				btn.Height = height;
				btn.Width = width;
				
				if(i == 0)
				{
					btnFont = Function.form.control.Font_Control_Resize_Get(btn, control.enControl_Criteria.height, "9", 0.75f);
				}

				btn.Font = btnFont;

				col++;

				if(col == 4)
				{
					col = 1;
					top += span + height;
					left = span;
				}
				else
				{
					left += span + width;					
				}
			}
		}



		/// <summary>
		/// 입력 받을 숫자 길이 설정 (1~15)
		/// </summary>
		public int MaxLength
		{
			get { return txtNumber.MaxLength; }
			set
			{
				if (value < 1)
					txtNumber.MaxLength = 1;
				else if (value > 15)
					txtNumber.MaxLength = 15;
				else
					txtNumber.MaxLength = value;

				txtNumber_FontResize();

			}
		}


		/// <summary>
		/// 번호판 클릭..
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NumberClick(object sender, EventArgs e)
		{

			string strNumber = ((Button)sender).Name;
			strNumber = strNumber.Substring(3, 1);
			if (_onNumberButtonClick != null)
			{
				_onNumberButtonClick(this, strNumber);
			}


			//최대 길이가 넘지 않게..
			if (txtNumber.Text.Length >= txtNumber.MaxLength) return;
			
			string newvalue = Number;
			
			if(strNumber.Equals("C"))
			{
				if (newvalue.IndexOf('.') < 0) lastComma = true;
			}			
			else if (char.IsDigit(strNumber, 0))
			{
				if(lastComma)
				{
					newvalue += ".";
					lastComma = false;
				}
				newvalue += strNumber;
			}

			Number = newvalue;
		}

		private void btnBack_Click(object sender, EventArgs e)
		{
			string newvalue = txtNumber.Text;

			if (_onNumberButtonClick != null)
			{
				_onNumberButtonClick(this, Keys.Back);
			}


			if (newvalue.Length < 2)
				newvalue = "0";
			else			
				newvalue = newvalue.Substring(0, newvalue.Length - 1);

			Number = newvalue;

		}


		private void btnOk_Click(object sender, EventArgs e)
		{
			if (_btnOk_Click == null) return;
			_btnOk_Click(this, e);
		}

		private void usrNumberInput_Load(object sender, EventArgs e)
		{
			init();
		}





	}// end class
}
