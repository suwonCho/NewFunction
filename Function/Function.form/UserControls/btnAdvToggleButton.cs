using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Function.form
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="current_value"></param>
	/// <param name="new_value"></param>
	/// <returns>취소여부</returns>
	public delegate bool Del_AdvValue_Changing(object sender, object current_value, object new_value);


	public partial class btnAdvToggleButton : UserControl
	{
		/// <summary>
		/// 버튼 값이 변경 후 발생하는 이벤트
		/// </summary>
		public event EventHandler On_Value_Changed;
		/// <summary>
		/// 버튼 값이 변경 전 발생하는 이벤트
		/// 이벤트 취소 시 :false 리턴
		/// </summary>
		public event Del_AdvValue_Changing On_Value_Changing;

		//event System.EventHandler _doubleClick;
		
		///// <summary>
		///// 두번 클릭할 때 발생합니다.
		///// </summary>
		//public event System.EventHandler DoubleClick
		//{
		//	add { _doubleClick += value; }
		//	remove { _doubleClick -= value; }
		//}

		event System.EventHandler _click;


		public event EventHandler Click
		{
			add { _click += value; }
			remove { _click += value; }
		}
		


		private enToggleButtonType _buttonType = enToggleButtonType.Image;

		/// <summary>
		/// item 집합
		/// </summary>
		private Dictionary<int, AdvToggleButtonValue> _items = new Dictionary<int,AdvToggleButtonValue>();

		/// <summary>
		/// 현재 선택된 아이템 인덱스
		/// </summary>
		private int _index = -1;

		public int Index
		{
			get { return _index; }
			set
			{
				if(_index == value) return;

				_index = value;

				change_Value(_items[_index]);
			}
		}


		private bool _click_Enabled = false;
		
		/// <summary>
		/// 클릭시 토글 처리 여부
		/// </summary>
		public bool Click_Enabled
		{
			get { return _click_Enabled; }
			set
			{
				_click_Enabled = value;
			}
		}


		/// <summary>
		/// 현재 아이템을 가져오거나 설정합니다.
		/// </summary>
		private AdvToggleButtonValue Item
		{
			get 
			{ 
				if (_index < 0)
					return null;
				else
					return _items[_index];
			}
			set
			{
				value.Index = _index;

				_items[_index] = value;

				change_Value(value);

			}
		}

		



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

				if (_buttonType == enToggleButtonType.Text || _buttonType == enToggleButtonType.ImageText)
				{
					BackgroundImage = Properties.Resources.BTN_NONE;
					lblText.Visible = true;
				}
				else
				{
					lblText.Visible = false;
				}

				
			}
		}

		/// <summary>
		/// 현재 값의 Text를 가져 온다
		/// </summary>
		public object CurrentText
		{
			get
			{
				if (_index < 0)
					return null;
				else
					return _items[_index].Text;
			}
			
		}
						
		private object _value = null;

		/// <summary>
		/// 버튼의 현재 값을 가져오거나 설정합니다.
		/// </summary>
		public object Value
		{
			get 
			{ 
				if (_index < 0)
					return null;
				else
					return _items[_index].Value; 
			}
			set
			{
				Change_Value(value);
			}
		}

		delegate void delInvoke_Value(object value);
		/// <summary>
		/// Control에 TEXT를 변경한다.
		/// </summary>
		/// <param name="lbl"></param>
		/// <param name="strText"></param>
		public void Invoke_Value(object value)
		{
			if (this.InvokeRequired)
				this.Invoke(new delInvoke_Value(Invoke_Value), new object[] { value });
			else
				Value = value;

		}







		public void Items_Add(object value, Image img, string text)
		{
			AdvToggleButtonValue i = new AdvToggleButtonValue(_items.Count, value, img, text);

			_items.Add(_items.Count, i);

			if (Index < 0) Index = 0;
		}

		public void Items_Add(AdvToggleButtonValue newItem)
		{
			newItem.Index = _items.Count;

			_items.Add(_items.Count, newItem);

			if (Index < 0) Index = 0;
		}



		/// <summary>
		/// 다음 항목을 가져옵니다.
		/// </summary>
		/// <returns></returns>
		private int next_index_Get()
		{
			int next_idx = -1;

			if (_index < 0)
			{
				if (_items.Count > 0) next_idx = 0;
			}
			else
			{
				next_idx = _index + 1;

				if (next_idx >= _items.Count) next_idx = 0;
			}

			return next_idx;

		}


		/// <summary>
		/// 다음 항목을 가져옵니다.
		/// </summary>
		/// <returns></returns>
		private AdvToggleButtonValue next_item_Get()
		{
			int next_idx = next_index_Get();

			return next_idx < 0 ? null : _items[next_idx];

		}

		private void change_Value(AdvToggleButtonValue newValue)
		{
			if (On_Value_Changing != null && !On_Value_Changing(this, Item.Value, newValue.Value)) return;

			if (Item == null)
			{
				BackgroundImage = null;
				lblText.Text = string.Empty;
				return;
			}

			_index = newValue.Index;

			if (ButtonType == enToggleButtonType.Image || ButtonType == enToggleButtonType.ImageText)
			{
				BackgroundImage = Item.Img;				
			}
			if(ButtonType == enToggleButtonType.Text || ButtonType == enToggleButtonType.ImageText)
			{				
				lblText.Text = Item.Text;

			}

		}

		public void Change_Value(object newValue)
		{
			AdvToggleButtonValue new_item = null;

			foreach(AdvToggleButtonValue i in _items.Values)
			{
				if(i.Value.Equals(newValue))
				{
					new_item = i;
					break;
				}
			}

			//if (new_item == null) throw (new Exception("해당 값이 항목에 등록되어 있지 않습니다."));


			change_Value(new_item);

		}



		public btnAdvToggleButton()
		{
			InitializeComponent();

			base.Click += btnToggleButton_Click;
			lblText.Click += btnToggleButton_Click;
		}

		private void btnToggleButton_Click(object sender, EventArgs e)
		{
			if (Click_Enabled)
			{
				AdvToggleButtonValue next_item = next_item_Get();

				change_Value(next_item);
			}

			if (_click != null) _click(sender, e);
		}

		private void btnToggleButton_FontChanged(object sender, EventArgs e)
		{
			lblText.Font = this.Font;
		}

		private void lblText_DoubleClick(object sender, EventArgs e)
		{
			//if (_doubleClick == null) return;

			//_doubleClick(this, e);
		}


		

		

	}


	


	public class AdvToggleButtonValue
	{
		public int Index;
		public object Value;
		public Image Img;
		public string Text;

		public AdvToggleButtonValue(int _index, object _value, Image _img, string _text)
		{
			Index = _index;
			Value = _value;
			Img = _img;
			Text = _text;
		}
	}
	


}
