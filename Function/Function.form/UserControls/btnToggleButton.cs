using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Function.form
{
	public delegate bool Del_Value_Changing(object sender, bool current_value);
	public enum enToggleButtonType { Text, Image, ImageText };

	public partial class btnToggleButton : UserControl
	{
		/// <summary>
		/// 버튼 값이 변경 후 발생하는 이벤트
		/// </summary>
		public event EventHandler On_Value_Changed;
		/// <summary>
		/// 버튼 값이 변경 전 발생하는 이벤트
		/// 이벤트 취소 시 :false 리턴
		/// </summary>
		public event Del_Value_Changing On_Value_Changing;


		private enToggleButtonType _buttonType = enToggleButtonType.Image;

		/// <summary>
		/// 버튼 타입을 변경하거나 가져 온다.
		/// </summary>
		public enToggleButtonType ButtonType
		{
			get { return _buttonType; }
			set
			{
				if (value == _buttonType) return;

				_buttonType = value;

				if (_buttonType == enToggleButtonType.Text)
				{
					BackgroundImage = Properties.Resources.BTN_NONE;
					lblText.Visible = true;
				}
				else
				{
					lblText.Visible = false;
				}

				image_change();
			}
		}

		private string _on_text = "O N";
		/// <summary>
		/// 버튼 타입이 Text일때 value가 true일 경우 표시 되는 Text
		/// </summary>
		public string On_Text
		{
			get { return _on_text; }
			set
			{
				if (_on_text.Equals(value)) return;

				_on_text = value;

				image_change();
			}
		}


		private string _off_text = "OFF";
		/// <summary>
		/// 버튼 타입이 Text일때 value가 false일 경우 표시 되는 Text
		/// </summary>
		public string Off_Text
		{
			get { return _off_text; }
			set
			{
				if (_off_text.Equals(value)) return;

				_off_text = value;

				image_change();
			}
		}


		/// <summary>
		/// 현재 표시 되는 텍스트를 가져 옵니다.
		/// </summary>
		public string CurrentText
		{
			get
			{
				return _value ? On_Text : Off_Text;
			}
		}



				
		private bool _value = true;

		/// <summary>
		/// 버튼의 현재 값
		/// </summary>
		public bool Value
		{
			get { return _value; }
			set
			{
				if(_value != value)
				{
					_value = value;
					image_change();

					if (On_Value_Changed != null) On_Value_Changed(this, null);					
				}
			}
		}

		private Image _on_image = null;

		/// <summary>
		/// Value가 true일 경우 이미지
		/// </summary>
		public Image On_Image
		{
			get
			{
				return _on_image;
			}
			set
			{
				_on_image = value;

				image_change();
			}
		}

		private Image _off_image = null;

		/// <summary>
		/// Value가 true일 경우 이미지
		/// </summary>
		public Image Off_Image
		{
			get
			{
				return _off_image;
			}
			set
			{
				_off_image = value;

				image_change();
			}
		}


		private void image_change()
		{

			if (ButtonType == enToggleButtonType.Image)
			{
				if (_value)
					BackgroundImage = On_Image == null ? Properties.Resources.BTN_ON : On_Image;
				else
					BackgroundImage = Off_Image == null ? Properties.Resources.BTN_OFF : Off_Image;
			}
			else
			{
				lblText.Text = CurrentText;
			}

		}


		public btnToggleButton()
		{
			InitializeComponent();			
		}

		private void btnToggleButton_Click(object sender, EventArgs e)
		{
			if (On_Value_Changing != null && !On_Value_Changing(this, Value)) return;

			Value = !Value;
		}

		private void btnToggleButton_FontChanged(object sender, EventArgs e)
		{
			lblText.Font = this.Font;
		}

		

		

	}
}
