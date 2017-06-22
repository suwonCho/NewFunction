using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Function.form
{
	public class usrUserControlListBase : UserControl
	{
		int _selected_index = -1;

		/// <summary>
		/// 선택된 항목 인덱스
		/// </summary>
		public int Selected_Index
		{
			get { return _selected_index; }			
		}

		object _selected_item_value = null;

		/// <summary>
		/// 선택한 항목의 값
		/// </summary>
		public object Selected_item_value
		{
			get { return _selected_item_value; }			
		}

		event delSeletedIndexChanged _item_index_changed;

		SortedDictionary<int, usrUserControlItemBase> _items = new SortedDictionary<int, usrUserControlItemBase>();

		int _itemHeight = 30;

		/// <summary>
		/// 각 항목의 높이를 가져오거나 설정합니다.
		/// </summary>
		public int ItemHeight
		{
			get { return _itemHeight; }
			set
			{
				if (_itemHeight == value) return;

				_itemHeight = value;
			}
		}

		public void ItemsClear(Control ctrl)
		{
			ctrl.Controls.Clear();
			_items.Clear();
		}

		public usrUserControlListBase()
		{
			this.Load += new EventHandler(usrUserControlListBase_Load);
		}

		void usrUserControlListBase_Load(object sender, EventArgs e)
		{
			this.ControlAdded += new ControlEventHandler(usrUserControlListBase_ControlAdded);
			this.ControlRemoved += new ControlEventHandler(usrUserControlListBase_ControlRemoved);
			

			init_event(this);
		}

		void init_event(Control ctrl)
		{
			foreach (Control c in ctrl.Controls)
			{
				c.ControlAdded += new ControlEventHandler(usrUserControlListBase_ControlAdded);
				c.ControlRemoved += new ControlEventHandler(usrUserControlListBase_ControlRemoved);			
				init_event(c);
			}
			
		}		


		void usrUserControlListBase_ControlRemoved(object sender, ControlEventArgs e)
		{
			usrUserControlItemBase item = e.Control as usrUserControlItemBase;

			if (item == null) return;

			_items.Remove(item.Index);

			item.Dispose();
		}

		void usrUserControlListBase_ControlAdded(object sender, ControlEventArgs e)
		{
			usrUserControlItemBase item = e.Control as usrUserControlItemBase;

			if (item == null) return;

			_items.Add(item.Index, item);

			item.SelectedChanged += new EventHandler(item_SelectedChanged);

			_item_index_changed += item.Index_Changed;

			setControlLocation(item, item.Index);

		}

		/// <summary>
		/// 항목에서 선택 항목이 변경 되었을 경우
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void item_SelectedChanged(object sender, EventArgs e)
		{
			usrUserControlItemBase item = sender as usrUserControlItemBase;

			if (item == null) return;

			if (!item.IsSelected) return;

			if (item.Index == Selected_Index) return;

			_selected_index = item.Index;

			if (_item_index_changed != null) _item_index_changed(_selected_index);
			
		}

		void setControlLocation(Control ctrl, int idx)
		{
			ctrl.Left = 0;
			ctrl.Width = ctrl.Parent.Width;

			int cnt = (from item in _items.Keys
					   where item < idx
					   select item).Count();

			ctrl.Top = cnt * ItemHeight;
			ctrl.Height = ItemHeight;

			ctrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		}


		object _dataSource = null;

		/// <summary>
		/// 데이터 소스
		/// </summary>
		public object DataSource
		{
			get { return _dataSource; }
			set
			{
				if (_dataSource != null && _dataSource.Equals(value)) return;

				_dataSource = value;

				if (_dataSource != null) _textValues = null;

				if (_dataSourceChanged != null) _dataSourceChanged(this, new EventArgs());
			}
		}

		event EventHandler _dataSourceChanged;

		/// <summary>
		/// DataSource나 textValue가 변경돼면 발생 하는 이벤트
		/// </summary>
		public event EventHandler DataSourceChanged
		{
			add
			{
				_dataSourceChanged += value;
			}

			remove
			{
				_dataSourceChanged -= value;
			}
		}

		string[] _textValues = null;


		public string[] TextValues
		{
			get { return _textValues; }
			set
			{
				if (_textValues != null && _textValues.Equals(value)) return;

				_textValues = value;

				if (_textValues != null) _dataSource = null;

				if (_dataSourceChanged != null) _dataSourceChanged(this, new EventArgs());
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
