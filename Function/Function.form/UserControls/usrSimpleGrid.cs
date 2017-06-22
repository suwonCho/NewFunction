using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Design;

namespace Function.form
{
	public partial class usrSimpleGrid : UserControl
	{
		Dictionary<int, SimpleGridColumn> _columns = new Dictionary<int, SimpleGridColumn>();

		public SimpleGridColumn this[int index]
		{
			get 
			{
				return _columns[index];
			}
			set
			{
				_columns[index] = value;
			}
		}


		[Localizable(true)]
		[MergableProperty(false)]		
		[RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		public Array Columns
		{
			get { return _columns.Values.ToArray<SimpleGridColumn>(); }
			set
			{
				int idx=0;

				_columns.Clear();

				foreach(object o in value)
				{
					SimpleGridColumn col = o as SimpleGridColumn;
					if (col != null)
					{
						col.Index = idx;
						col.OnPropertiesChannged += col_OnPropertiesChannged;
						_columns.Add(idx, col);
						idx++;
					}
				}
					
				this.Refresh();
			}
		}


		Color _grid_Line_Color = Color.Black;
		[Description("선 색")]
		public Color Grid_Line_Color
		{
			get { return _grid_Line_Color; }
			set
			{
				if (value == _grid_Line_Color) return;

				_grid_Line_Color = value;

				this.Refresh();
			}
		}


		int _grid_Line_Width = 1;
		[Description("선 두께")]
		public int Grid_Line_Width
		{
			get { return _grid_Line_Width; }
			set
			{
				if (_grid_Line_Width == value) return;

				_grid_Line_Width = value;

				Refresh();
			}
		}


		int _grid_Header_Height = 30;
		[Description("헤더 높이")]
		public int Grid_Header_Height
		{
			get { return _grid_Header_Height; }
			set
			{
				if (_grid_Header_Height == value) return;

				_grid_Header_Height = value;

				Refresh();
			}
		}


		int _rows_Count = 1;
		[Description("로우 숫자")]
		public int Rows_Count
		{
			get { return _rows_Count; }
			set
			{

				if (_rows_Count == value || value <= 0) return;

				_rows_Count = value;

				Refresh();
			}
		}
		

		public usrSimpleGrid()
		{
			InitializeComponent();

			SimpleGridColumn col = new SimpleGridColumn();
			_column_Add(col);


			_grid_Header_View.OnPropertiesChannged += col_OnPropertiesChannged;
			_gird_View.OnPropertiesChannged += col_OnPropertiesChannged;
		}
		
		void _column_Add(SimpleGridColumn col)
		{
			//순서가 바뀌지 않음
			col.OnPropertiesChannged += col_OnPropertiesChannged;
			if(col.Index == -1 || col.Index >= _columns.Count)
			{
				col.Index = _columns.Count;
				_columns.Add(col.Index, col);
				return;
			}

			//새로 드러올 항목이 인덱스가 중복.. 정리 
			SimpleGridColumn[] cols = _columns.Values.ToArray<SimpleGridColumn>();

			_columns[col.Index] = col;

			for (int idx = col.Index; idx < cols.Length; idx++ )
			{
				cols[idx].Index = idx + 1;

				if(_columns.ContainsKey(idx+1))
				{					
					_columns[idx + 1] = cols[idx];
				}
				else
				{
					_columns.Add(idx + 1, cols[idx]);
				}
			}

			

		}



		public void Column_Add(SimpleGridColumn col)
		{
			_column_Add(col);

			Refresh();
		}

		int _column_Count = 0;

		[DefaultValue(-1)]
		[RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		[Description("컬럼 숫자")]
		public int Column_Count
		{
			get
			{
				return _columns.Count;
			}
			set
			{
				if (_columns.Count == value) return;

				int len = _columns.Count > value ? _columns.Count : value;

				SimpleGridColumn[] cols = _columns.Values.ToArray<SimpleGridColumn>();

				try
				{
					properties_OnChanging = true;

					if(value < _columns.Count)
					{	//컬럼수 축소 -> 삭제 처리
						for (int idx = len - 1; idx >= 0; idx--)
						{
							if (_columns.Count > idx && value <= idx)
							{	//항목 숫자 감소 삭제 처리..
								_columns.Remove(idx);
							}							
						}
					}
					else if(value > _columns.Count)
					{	//컬럼증가
						for (int idx = 0; idx < len; idx++)
						{

							if (_columns.Count > idx && value > idx)
							{ //그대로 추가
								continue;
							}							
							else if (_columns.Count <= idx && value > idx)
							{	//신규 추가	
								SimpleGridColumn col = new SimpleGridColumn();
								col.Index = idx;
								col.OnPropertiesChannged += col_OnPropertiesChannged;
								_columns.Add(idx, col);
							}
						}
					}

					

					Refresh();
				}
				catch
				{

				}
				finally
				{
					properties_OnChanging = false;
				}
			}
		}

		ControlViewInfo _grid_Header_View = new ControlViewInfo();

		[MergableProperty(true)]
		[RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		[Description("해더 뷰 설정")]
		public ControlViewInfo Grid_Header_View
		{
			get { return _grid_Header_View; }
			set
			{

				value.OnPropertiesChannged += col_OnPropertiesChannged;				
				_grid_Header_View = value;


				Refresh();
				
			}
		}

		


		ControlViewInfo _gird_View = new ControlViewInfo();

		[MergableProperty(true)]
		[RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		[Description("그리드 뷰 설정")]
		public ControlViewInfo Grid_View
		{
			get { return _gird_View; }
			set
			{
				value.OnPropertiesChannged += col_OnPropertiesChannged;
				_gird_View = value;

				Refresh();
				
			}
		}




		bool properties_OnChanging = false;

		/// <summary>
		/// 컬럼의 프로퍼티 변경 시 처리...
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void col_OnPropertiesChannged(object sender, EventArgs e)
		{
			if (properties_OnChanging) return;
			try
			{
				properties_OnChanging = true;

				Refresh();
			}
			catch
			{
				
			}
			finally
			{
				properties_OnChanging = false;
			}

		}

		void grid_init(int idx = -1)
		{
			int left = 0;
			int width = this.Width;

			foreach(SimpleGridColumn col in _columns.Values)
			{				
				if (col.Column_Width_Unit == enUnit.Pixel)
					col.Width = col.Column_Width;
				else
					col.Width = width * col.Column_Width / 100;

				if(col.Index == (Column_Count-1))
				{
					col.Width = width - left;

					if (col.Column_Width_Unit == enUnit.Pixel)
						col.Column_Width = col.Width;
				}
				
				//left보정은 여기서, 폭 보정을 그릴때
				col.Left = left + (_grid_Line_Width /2);

				if (col.Visible) left += col.Width;

			}
			
		}

		string _value = string.Empty;

		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]		
		[Description(@"값을 설정합니다.
1라인에 한로우, 값과 값사이는 ','로 구분")]
		public string Values
		{
			get { return _value; }
			set
			{
				if (_value == value) return;

				_value = value;

				Refresh();
			}
		}


		object _datasource = null;

		[DefaultValue(null)]
		public object DataSource
		{
			get { return _datasource; }
			set
			{
				_datasource = value;
				Refresh();
			}
		}



		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			using (
				Graphics gSquare = this.CreateGraphics()
				//,gCircle = this.circlePanel.CreateGraphics()
				)
			{

				grid_init();
				int width = _grid_Line_Width;
				Pen p = new Pen(_grid_Line_Color, width);

				//가장 외각 테두리를 그린다.
				//gSquare.DrawRectangle(
				//	p,
				//	1,
				//	1,
				//	this.Width - width ,
				//	this.Height - width 
				//	);

				int idx = 0;
				int top = 0;
				int grdH = 0;
				int grd = 0;
				int grdW = 0;
				int left = 0 ;
				int gap = width / 2;
				gap = gap < 1 ? 1 : gap;

				foreach (SimpleGridColumn col in _columns.Values)
				{
					if (!col.Visible) continue;
					//if (col.Index > 0) gSquare.DrawLine(p, col.Left, 1, col.Left, this.Height - width);

					//gSquare.DrawString(col.ColumnName, Font, Brushes.Black, col.Left, 10);

					top = gap;

					if (idx != 0)
					{
						if (col.Index == (Column_Count - 1))
						{
							left = width;
							grdW = col.Width;
						}
						else
						{
							left = gap;
							grdW = col.Width - gap + left;
						}

					}
					else
						grdW = col.Width - gap + left;

					//헤더를 그리고
					DrawStringInCenter(col.ColumnName, Grid_Header_View.Font, Grid_Header_View.ForeColor,
						new Rectangle(col.Left - left, top, grdW, Grid_Header_Height - width), Grid_Header_View.BackColor,
						gSquare, p, col.Header_Alignment);


					top += Grid_Header_Height - width + 1;
					//그리드 전체 높이
					grdH = this.Height - top;
					//한개 그리드 높이
					grd = grdH / Rows_Count;

					string[] rows = null;
					string[] row;
					string txt = string.Empty;
					bool useDs = DataSource != null;
					
					if(!useDs)	rows = Values.Split(new string[] { "\r\n" }, StringSplitOptions.None);

					for (int i = 0; i < Rows_Count; i++)
					{
						if (i == (Rows_Count - 1)) grd = this.Height - top - gap;
						txt = string.Empty;

						if(useDs)
						{
							txt = TextGet_FormDataSource(col.DataFieldName, 0);
						}
						else if(rows.Length > i)
						{
							row = rows[i].Split(new string[] { "," }, StringSplitOptions.None);

							if(row.Length > col.Index)
							{
								txt = row[col.Index];
							}

						}


						//그리드
						DrawStringInCenter(txt, Grid_View.Font, Grid_View.ForeColor,
							new Rectangle(col.Left - left, top, grdW, grd), Grid_View.BackColor,
							gSquare, p, col.Grid_Alignment);

						idx++;

						top += grd;
					}

					
				}

			}
		}

		public string TextGet_FormDataSource(string fieldName, int row)
		{
			DataRow dr;

			try
			{
				dr = DataSource as DataRow;

				if(dr == null)
				{
					DataTable dt = DataSource as DataTable;

					dr = dt.Rows[row];
				}

				if (dr == null) return string.Empty;

				return Fnc.obj2String(dr[fieldName]);


			}
			catch
			{
				return string.Empty;
			}
		}


		private void DrawStringInCenter(String txt, Font fnt, Color fnt_color, Rectangle rec, Color rec_fill_color, Graphics e, Pen line, ContentAlignment ali )
		{

			SizeF stringSize = new SizeF();
			int min = 3;

			stringSize = e.MeasureString(txt, fnt);
			SolidBrush br = new SolidBrush(rec_fill_color);
			e.FillRectangle(br, rec);

			e.DrawRectangle(line, rec.X, rec.Y, rec.Width, rec.Height);
			int left;
			int top;
			
			//높이 지정
			switch(ali)
			{ 
				case ContentAlignment.TopCenter:
				case ContentAlignment.TopLeft:
				case ContentAlignment.TopRight:
					top = rec.Y + min;
					break;


				case ContentAlignment.MiddleLeft:
				case ContentAlignment.MiddleRight:
				case ContentAlignment.MiddleCenter:
					top = (int)(rec.Y + (rec.Height / 2 - stringSize.Height / 2)) + min;
					break;


				default:
					top = (int)(rec.Y + rec.Height - stringSize.Height + min);
					break;
			}

			switch(ali)
			{
				case ContentAlignment.BottomLeft:
				case ContentAlignment.MiddleLeft:
				case ContentAlignment.TopLeft:
					left = rec.X + min;
					break;


				case ContentAlignment.BottomRight:
				case ContentAlignment.MiddleRight:
				case ContentAlignment.TopRight:
					left = (int)(rec.X + rec.Width - stringSize.Width - min);
					break;

				default:
					left = (int)(rec.X + (rec.Width / 2 - stringSize.Width / 2));
					break;
			}

			
			if ((top + rec.Y) < min) top = min;
			if ((left +rec.X) < min) left = min;

			br = new SolidBrush(fnt_color);

			string[] txts = txt.Split(new string[] { "\r\n" },  StringSplitOptions.None);
			
			if(txts.Length < 2)
				e.DrawString(txt, fnt, br, left, top);
			else
			{
				foreach(string t in txts)
				{
					stringSize = e.MeasureString(t, fnt);
					switch (ali)
					{
						case ContentAlignment.BottomLeft:
						case ContentAlignment.MiddleLeft:
						case ContentAlignment.TopLeft:
							left = rec.X + min;
							break;


						case ContentAlignment.BottomRight:
						case ContentAlignment.MiddleRight:
						case ContentAlignment.TopRight:
							left = (int)(rec.X + rec.Width - stringSize.Width - min);
							break;

						default:
							left = (int)(rec.X + (rec.Width / 2 - stringSize.Width / 2));
							break;
					}


					e.DrawString(t, fnt, br, left, top);
					top += Fnc.floatToInt(stringSize.Height);
				}
				
			}

		}






	}	//end class


	[TypeConverter(typeof(TypeConverter_Properties))]
	public class SimpleGridColumn
	{
		event EventHandler _onPropertiesChannged;

		[Description("프로퍼티 값이 변경 되었을 경우 발새하는 이벤트")]
		public event EventHandler OnPropertiesChannged
		{
			add { _onPropertiesChannged += value; }
			remove { _onPropertiesChannged -= value; }
		}


		int _index = -1;
		[DefaultValue(-1)]
		[RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		[Description("Index")]
		public int Index
		{
			get { return _index; }
			set
			{
				if (_index == value) return;

				_index = value;

				if (_columnName == string.Empty)
					_columnName = string.Format("Column{0}", _index);

				properties_changed();

			}
		}		

		string _columnName = string.Empty;
		[DefaultValue("")]
		[RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Description("컬럼 이름")]
		public string ColumnName
		{
			get { return _columnName; }	
			set
			{
				_columnName = value;

				properties_changed();
			}
		}


		enUnit _column_Width_Unit = enUnit.Pixel;

		[DefaultValue(0)]
		[RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		[Description("컬럼 폭 단위")]
		public enUnit Column_Width_Unit
		{
			get { return _column_Width_Unit; }
			set
			{
				if (_column_Width_Unit == value) return;

				_column_Width_Unit = value;

				properties_changed();
			}
		}

		int _column_width = 30;

		[DefaultValue(30)]
		[RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		[Description("컬럼 폭 값")]
		public int Column_Width
		{
			get { return _column_width; }
			set
			{
				if (_column_width == value || value < 0) return;

				if (_column_Width_Unit == enUnit.Percent && value > 100) return;

				_column_width = value;

				properties_changed();
			}			
		}

		ContentAlignment _header_Alignment = ContentAlignment.MiddleCenter;
		
		[Description("헤더 정렬")]
		public ContentAlignment Header_Alignment
		{
			get { return _header_Alignment; }
			set
			{
				if (_header_Alignment == value) return;

				_header_Alignment = value;

				properties_changed();
			}
		}


		ContentAlignment _gird_Alignment = ContentAlignment.MiddleCenter;

		[Description("표 내용 정렬")]
		public ContentAlignment Grid_Alignment
		{
			get { return _gird_Alignment; }
			set
			{
				if (_gird_Alignment == value) return;

				_gird_Alignment = value;

				properties_changed();
			}
		}


		string _dataFieldName = string.Empty;
		[Description("바인딩시 필드 명")]
		public string DataFieldName
		{
			get { return _dataFieldName; }
			set
			{
				if (_dataFieldName == value) return;
				_dataFieldName = value;

				properties_changed();
			}
		}

		bool _visible = true;

		[DefaultValue(true)]
		public bool Visible
		{
			get { return _visible; }
			set
			{
				_visible = value;
				properties_changed();
			}
		}

		public int Width = 30;

		public int Left = 0;





		private void properties_changed()
		{
			if (_onPropertiesChannged == null) return;

			_onPropertiesChannged(this, null);
		}

		public override string ToString()
		{
			return string.Format("[{0}]{1}", _index, ColumnName);
		}


		


	}	//end class

	

}
