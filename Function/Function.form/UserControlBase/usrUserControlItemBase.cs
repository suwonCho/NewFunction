using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Function.form
{
	delegate void delSeletedIndexChanged(int curr_idx);

	/// <summary>
	/// 사용자 정의 클래서 아이템(항목) 베이스 클래스
	/// </summary>
	public class usrUserControlItemBase : UserControl
	{

		internal usrUserControlItemBase()
		{
			this.Load += new EventHandler(usrUserControlItemBase_Load);
		}

		void usrUserControlItemBase_Load(object sender, EventArgs e)
		{
			init_event(this);
		}

		private void init_event(Control ctrl)
		{
			foreach (Control c in ctrl.Controls)
			{
				c.Click += new EventHandler(Click);
				c.Leave += new EventHandler(Leave);

				init_event(c);
			}
		}

		/// <summary>
		/// 포커스를 일었을경우 선택상태 체크 - 미사용
		/// </summary>
		/// <param name="ctrl"></param>
		/// <returns></returns>
		private bool chkFocused(Control ctrl)
		{
			bool seleted = false;

			foreach (Control c in ctrl.Controls)
			{
				if (c.Focused || chkFocused(c))
				{
					seleted = true;
					break;
				}				
			}

			return seleted;
		}

		

		internal object _value = null;


		public object Value
		{
			get
			{
				return _value;
			}
			set
			{

				if (_value != null && _value.Equals(value)) return;

				_value = value;

				if (_valueChanged != null)
					_valueChanged(this, new EventArgs());


			}
		}

		internal int _index = 0;
		
		public int Index
		{
			get { return _index; }
			set
			{
				_index = value;
			}
		}

		public void Index_Changed(int curr_idx)
		{
			isSelected = Index.Equals(curr_idx);
		}



		internal event EventHandler _valueChanged;

		/// <summary>
		/// Text값이 변경돼면 이벤트가 발생한다.
		/// </summary>
		public new event EventHandler ValueChanged
		{
			add
			{
				_valueChanged += value;
			}

			remove
			{
				_valueChanged -= value;
			}
		}

		internal event EventHandler _selectedChanged;
		/// <summary>
		/// 컨트롤의 선택 되었을때 이벤트가 발생한다.
		/// </summary>
		public event EventHandler SelectedChanged
		{
			add
			{
				_selectedChanged += value;
			}
			remove
			{
				_selectedChanged -= value;
			}
		}
		

		bool _isSeleted = false;

		internal bool isSelected
		{
			get { return _isSeleted; }
			set
			{
				if (_isSeleted == value) return;

				_isSeleted = value;

				if (_selectedChanged != null) _selectedChanged(this, new EventArgs());

				if (_isSeleted)
				{
					this.BackColor = ColorSelected;
				}
				else
				{
					this.BackColor = ColorNoSelected;
				}
			}
		}

		/// <summary>
		/// 항목선택
		/// </summary>
		public bool IsSelected
		{
			get { return _isSeleted; }
		}

		/// <summary>
		/// 라벨 클릭 -> 항목 선택 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Click(object sender, EventArgs e)
		{
			isSelected = true;
		}

		/// <summary>
		/// 포커스를 잃었을 경우 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Leave(object sender, EventArgs e)
		{
			if (!chkFocused(this))
			{
				if (_lostFocus != null) _lostFocus(this, new EventArgs());
			}
		}

		event EventHandler _lostFocus;

		/// <summary>
		/// 포커스를 잃었을경우 이벤트 발생
		/// </summary>
		internal event EventHandler LostFocus
		{
			add
			{
				_lostFocus += value;
			}
			remove
			{
				_lostFocus -= value;
			}
		}


		Color _colorNoSelected = System.Drawing.SystemColors.Control;

		/// <summary>
		/// 항목이 미선택되었을경우 컬러
		/// </summary>
		public Color ColorNoSelected
		{
			get { return _colorNoSelected; }
			set
			{
				if (_colorNoSelected.Equals(value)) return;

			}
		}

		Color _colorSelected = System.Drawing.Color.RoyalBlue;

		/// <summary>
		/// 항목이 선택되었을경우 컬러
		/// </summary>
		public Color ColorSelected
		{
			get { return _colorSelected; }
			set
			{
				if (_colorSelected.Equals(value)) return;

			}
		}



	}
}
