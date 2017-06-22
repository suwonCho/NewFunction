using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using System.Data;

namespace Function.Component.DevExp
{

	public enum enGridSelectMode
	{
		None,
		Single,
		Multi
	}

	public delegate void delSelectedRowChanged(object sender, int row);

	/// <summary>
	/// 그리드뷰에 터치를 이용한 스크롤을 할 수 있도록 처리 한다.<para/>
	/// 처리로 인해 그리드에 Selection 변경관련 이벤트는 무시해야한다.<para/>
	/// OnSelectedRowChanged 이벤트에 등록 하여 로우변경 이벤트 처리 할것
	/// </summary>
	public class GridControlTouchScrollHelper
	{
		
		event delSelectedRowChanged _onSelectedRowChanged;
		
		public enGridSelectMode RowSelectMode = enGridSelectMode.None;
		string _checkfieldName = null;

		/// <summary>
		/// 로우가 선택(클릭) 되었을경우 발생 한다.
		/// </summary>
		public event delSelectedRowChanged OnSelectedRowChanged
		{
			add { _onSelectedRowChanged += value; }
			remove { _onSelectedRowChanged -= value; }
		}


		private readonly GridView _View;
		public GridControlTouchScrollHelper(GridView view)
		{
			_View = view;
			InitViewProperties();
		}

		public GridControlTouchScrollHelper(GridView view, enGridSelectMode selectmode, string checkfieldName)
		{
			_View = view;
			RowSelectMode = selectmode;
			_checkfieldName = checkfieldName;
			InitViewProperties();
		}



		private void InitViewProperties()
		{
			_View.OptionsBehavior.Editable = false;
			_View.GridControl.Cursor = Cursors.Hand;
			_View.OptionsSelection.EnableAppearanceFocusedRow = false;
			_View.OptionsSelection.EnableAppearanceFocusedCell = false;
			_View.FocusRectStyle = DrawFocusRectStyle.None;
			_View.OptionsView.ShowIndicator = false;
			_View.MouseDown += _View_MouseDown;
			_View.MouseMove += _View_MouseMove;
			_View.MouseUp += _View_MouseUp;
			_View.Layout += new EventHandler(_View_Layout);
		}

		void _View_Layout(object sender, EventArgs e)
		{
			IsDragging = false;
		}

		bool _doingScroll = false;
		int startDragRowHandle = -1;
		int topRowIndex = -1;
		DateTime startTime;
		private int _TopRowDelta;
		private bool _IsDragging;
		public bool IsDragging
		{
			get { return _IsDragging; }
			set
			{
				_IsDragging = value;
				startTime = DateTime.MinValue;
			}
		}

		private int GetRowUnderCursor(Point location)
		{
			DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hitInfo = _View.CalcHitInfo(location);
			if (hitInfo.InRow)
				return hitInfo.RowHandle;
			return GridControl.InvalidRowHandle;
		}
		private void CheckTime()
		{
			if (startTime == DateTime.MinValue)
				startTime = DateTime.Now;
		}
		private void CheckInertion()
		{
			if (startTime == DateTime.MinValue)
				return;
			DateTime currentTime = DateTime.Now;
			TimeSpan delta = currentTime - startTime;
			_TopRowDelta = _View.TopRowIndex - topRowIndex;
			int absValue = Math.Abs(_TopRowDelta);
			if (delta.TotalMilliseconds < 300 && absValue > 15)
			{
				DoInertion();
			}


		}
		private void DoInertion()
		{
			Timer inertionTimer = new Timer();
			inertionTimer.Interval = 25;
			inertionTimer.Tick += inertionTimer_Tick;
			inertionTimer.Start();
		}

		void inertionTimer_Tick(object sender, EventArgs e)
		{
			_View.TopRowIndex += _TopRowDelta;
			Timer timer = sender as Timer;
			timer.Interval += 25;
			if (timer.Interval == 200)
				timer.Stop();
		}

		private void DoScroll(int delta)
		{
			if (delta == 0)
				return;
			CheckTime();
			_View.TopRowIndex += delta;
			_doingScroll = true;
		}

		void _View_MouseUp(object sender, MouseEventArgs e)
		{
			CheckInertion();
			//이동이 없으면 선택으로 본다.
			if (!_doingScroll && startDragRowHandle > -1)
			{
				try
				{
					if (RowSelectMode != enGridSelectMode.None)
					{
						DataRow dr = _View.GetFocusedDataRow();
						
						if(RowSelectMode == enGridSelectMode.Multi)
						{
							try
							{
								dr[_checkfieldName] = dr[_checkfieldName] == null ? true : !(bool)dr[_checkfieldName];
							}
							catch
							{
								dr[_checkfieldName] = true;
							}
						}
						else
						{
							DataTable dt;

							if(_View.DataSource.GetType().Equals(typeof(DataTable)))
							{
								dt = (DataTable)_View.DataSource;
							}
							else
							{
								dt = ((DataView)_View.DataSource).Table;
							}


							DataRow[] rows = dt.Select(string.Format("{0} = True", _checkfieldName));

							foreach(DataRow r in rows)
							{
								r[_checkfieldName] = false;
							}


							dr[_checkfieldName] = true;
						}
					}
				}
				catch { }
				
				if (_onSelectedRowChanged != null) _onSelectedRowChanged(_View, startDragRowHandle);
			}

			IsDragging = false;
			_doingScroll = false;
		}

		void _View_MouseMove(object sender, MouseEventArgs e)
		{
			if (IsDragging)
			{
				int newRow = GetRowUnderCursor(e.Location);
				if (newRow < 0)
					return;
				int delta = startDragRowHandle - newRow;
				DoScroll(delta);
			}
		}

		void _View_MouseDown(object sender, MouseEventArgs e)
		{
			IsDragging = true;
			_doingScroll = false;
			startDragRowHandle = GetRowUnderCursor(e.Location);
			topRowIndex = _View.TopRowIndex;
		}
	}
}


