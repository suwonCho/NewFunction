using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraGrid.Columns;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using System.Data;
using Function.Component.DevExp.Class;
using DevExpress.Data.Filtering;

namespace Function.Component.DevExp
{
	public class fnc
	{

		/// <summary>
		/// 기다림 안내창을 띄우거나 내린다.
		/// </summary>
		/// <param name="frm"></param>
		public static void PleaseWaitForm_Show(Form frm)
		{

			DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(frm, typeof(Function.Component.DevExp.popPleaseWait));
		}


		public static void PleaseWaitForm_Close()
		{
			DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
		}


		/// <summary>
		/// 그리트 뷰에 필터 조건을 적용한다.
		/// </summary>
		/// <param name="gv"></param>
		/// <param name="value"></param>
		/// <param name="columnName"></param>
		/// <param name="GOType"></param>
		/// <param name="OType"></param>
		public static void GridView_FilterApply(GridView gv, object value, string columnName, GroupOperatorType GOType = GroupOperatorType.And, BinaryOperatorType OType = BinaryOperatorType.Equal)
		{

			if (value == null)
			{
				//gvBooks.Columns[columnName].FilterInfo.
				return;
			}

			if (OType == BinaryOperatorType.Like)
			{
				value = "%" + value + "%";
			}

			CriteriaOperator binaryFilter = new GroupOperator(GroupOperatorType.And,
						   new BinaryOperator(columnName, value, OType));

			//string filterDisplayText = String.Format("{0} = {1} ", columnName, value);

			//ColumnFilterInfo dateFilter = new ColumnFilterInfo(binaryFilter.ToString(), filterDisplayText);

			ColumnFilterInfo dateFilter = new ColumnFilterInfo(binaryFilter);

			gv.Columns[columnName].FilterInfo = dateFilter;
		}

		/// <summary>
		/// 그리트 뷰에 필터 Between 조건을 적용한다. 
		/// </summary>
		/// <param name="gv"></param>
		/// <param name="value"></param>
		/// <param name="columnName"></param>
		/// <param name="GOType"></param>
		/// <param name="OType1"></param>
		/// <param name="OType2"></param>
		public static void GridView_FilterApply(GridView gv, object value_from, object value_to, string columnName, GroupOperatorType GOType = GroupOperatorType.And, BinaryOperatorType OType1 = BinaryOperatorType.GreaterOrEqual, BinaryOperatorType OType2 = BinaryOperatorType.LessOrEqual)
		{

			if (value_from == null || value_to == null)
			{
				//gvBooks.Columns[columnName].FilterInfo.
				return;
			}
			

			CriteriaOperator binaryFilter = new GroupOperator(GroupOperatorType.And,
						   new BinaryOperator(columnName, value_from, OType1),
						   new BinaryOperator(columnName, value_to, OType2) );

			//string filterDisplayText = String.Format("{0} = {1} ", columnName, value);

			//ColumnFilterInfo dateFilter = new ColumnFilterInfo(binaryFilter.ToString(), filterDisplayText);

			ColumnFilterInfo dateFilter = new ColumnFilterInfo(binaryFilter);

			gv.Columns[columnName].FilterInfo = dateFilter;
		}



		/// <summary>
		/// 그리드 뷰를 셀 선택 view로 초기화 한다.
		/// </summary>
		/// <param name="gv"></param>
		public static void GridView_ViewInit(GridView gv, bool multiSelect = false, bool ColumnAutoWidth = false, bool ReadOnly = true)
		{
			gv.OptionsBehavior.ReadOnly = ReadOnly;
			gv.OptionsBehavior.Editable = false;
			
			gv.OptionsSelection.UseIndicatorForSelection = false;
			gv.OptionsSelection.EnableAppearanceFocusedCell = false;
			gv.OptionsSelection.EnableAppearanceHideSelection = false;
			gv.OptionsSelection.MultiSelect = multiSelect;

			gv.OptionsView.ColumnAutoWidth = ColumnAutoWidth;
			gv.OptionsView.ShowGroupPanel = false;
			//gv.OptionsView.ShowHorizontalLines = false;
			//gv.OptionsView.ShowVerticalLines = false;
			gv.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			gv.Appearance.FocusedRow.BackColor = Color.RoyalBlue;
			gv.Appearance.FocusedRow.ForeColor = Color.White;

			


		}

		/// <summary>
		/// 그리드 뷰를 셀 선택 view로 초기화 한다.
		/// </summary>
		/// <param name="gv"></param>
		/// <param name="multiSelect"></param>
		/// <param name="ColumnAutoWidth"></param>
		public static void GridView_ViewCellInit(GridView gv, bool multiSelect = false, bool ColumnAutoWidth = false, bool ShowNavigatorButton = false)
		{
			gv.OptionsBehavior.ReadOnly = true;
			gv.OptionsBehavior.Editable = false;

			gv.OptionsSelection.UseIndicatorForSelection = false;
			gv.OptionsSelection.EnableAppearanceFocusedCell = false;
			gv.OptionsSelection.EnableAppearanceHideSelection = false;
			gv.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
			gv.OptionsSelection.MultiSelect = multiSelect;
			gv.OptionsSelection.EnableAppearanceFocusedRow = false;

			gv.OptionsView.ColumnAutoWidth = ColumnAutoWidth;
			gv.OptionsView.ShowGroupPanel = false;
			//gv.OptionsView.ShowHorizontalLines = false;
			//gv.OptionsView.ShowVerticalLines = false;

			gv.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			gv.Appearance.FocusedRow.BackColor = Color.RoyalBlue;
			gv.Appearance.FocusedRow.ForeColor = Color.White;


			if (ShowNavigatorButton)
			{
				//그리드 하단 NaviBar 설정한다
				gv.GridControl.EmbeddedNavigator.Buttons.BeginUpdate();
				gv.GridControl.UseEmbeddedNavigator = true;
				gv.GridControl.EmbeddedNavigator.Buttons.NextPage.Visible = true;
				gv.GridControl.EmbeddedNavigator.Buttons.PrevPage.Visible = true;

				gv.GridControl.EmbeddedNavigator.Buttons.Append.Visible = false;
				gv.GridControl.EmbeddedNavigator.Buttons.Remove.Visible = false;
				gv.GridControl.EmbeddedNavigator.Buttons.Edit.Visible = false;
				gv.GridControl.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
				gv.GridControl.EmbeddedNavigator.Buttons.EndEdit.Visible = false;


				gv.GridControl.EmbeddedNavigator.Buttons.EndUpdate();
			}
			else
			{
				gv.GridControl.UseEmbeddedNavigator = false;
			}


		}

		/// <summary>
		/// 그리드 뷰를 셀 Edit view로 초기화 한다.
		/// </summary>
		/// <param name="gv"></param>
		/// <param name="useEditForm"></param>
		/// <param name="DoubleClick_EditMode">더블클릭과 엔터키만 수정모드로 들어갈수 있도록 처리 한다</param>
		public static void GridView_EditInit(GridView gv, bool useEditForm = true, bool DoubleClick_EditMode = false)
		{
			gv.OptionsBehavior.ReadOnly = false;

			if (useEditForm)
				gv.OptionsBehavior.EditingMode = GridEditingMode.EditForm;
			else
				gv.OptionsBehavior.EditingMode = GridEditingMode.Inplace;

			gv.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseUp;

			if (DoubleClick_EditMode)
			{
				gv.OptionsBehavior.Editable = false;
				gv.DoubleClick += new EventHandler(gv_DoubleClick);
				gv.KeyDown += new KeyEventHandler(gv_KeyDown);
				gv.HiddenEditor += new EventHandler(gv_HiddenEditor);

				gv.GridControl.EmbeddedNavigator.MouseHover += new EventHandler(EmbeddedNavigator_MouseHover);
				gv.GridControl.EmbeddedNavigator.MouseLeave += new EventHandler(EmbeddedNavigator_MouseLeave);
			}
			else
				gv.OptionsBehavior.Editable = true;
			
			gv.OptionsSelection.UseIndicatorForSelection = false;
			gv.OptionsSelection.EnableAppearanceFocusedCell = false;
			gv.OptionsSelection.EnableAppearanceHideSelection = false;
			gv.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
			gv.OptionsSelection.MultiSelect = false;
			gv.OptionsSelection.EnableAppearanceFocusedRow = false;

			gv.OptionsView.ColumnAutoWidth = false;
			gv.OptionsView.ShowGroupPanel = false;
			//gv.OptionsView.ShowHorizontalLines = false;
			//gv.OptionsView.ShowVerticalLines = false;

			gv.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			gv.Appearance.FocusedRow.BackColor = Color.RoyalBlue;
			gv.Appearance.FocusedRow.ForeColor = Color.White;

			//그리드 하단 NaviBar 설정한다
			gv.GridControl.EmbeddedNavigator.Buttons.BeginUpdate();
			gv.GridControl.UseEmbeddedNavigator = true;
			gv.GridControl.EmbeddedNavigator.Buttons.Append.Visible = true;
			gv.GridControl.EmbeddedNavigator.Buttons.Remove.Visible = true;
			gv.GridControl.EmbeddedNavigator.Buttons.Edit.Visible = true;
			gv.GridControl.EmbeddedNavigator.Buttons.CancelEdit.Visible = true;
			gv.GridControl.EmbeddedNavigator.Buttons.EndUpdate();

			//그리드 셀 값이 변경되면 선택된 셀의 값도 변경 시켜준다.
			gv.CellValueChanged += gv_CellValueChanged;

		}

		static bool isCellValueUpdating = false;
		static void gv_CellValueChanged(object sender, CellValueChangedEventArgs e)
		{
			if (isCellValueUpdating) return;
			GridView gv = sender as GridView;
			if (gv == null) return;			

			string column = e.Column.FieldName;
			GridCell[] gcs = gv.GetSelectedCells();
			if (gcs.Length < 2) return;

			gv.BeginUpdate();

			isCellValueUpdating = true;
			try
			{

				foreach (GridCell gc in gcs)
				{
					if (!gc.Column.FieldName.Equals(column)) continue;

					gv.SetRowCellValue(gc.RowHandle, gc.Column, e.Value);

				}
			}
			catch { }
			finally
			{
				isCellValueUpdating = false;
			}



			gv.EndUpdate();
			


		}

		/// <summary>
		/// 그리드 뷰에서 특정 값으로 로우를 찾는다
		/// </summary>
		/// <param name="gv"></param>
		/// <param name="SearchColumn"></param>
		/// <param name="SearchValue"></param>
		/// <param name="StartRowIndex"></param>
		public static int GridView_SelectRowBySearch(GridView gv, string SearchColumn, string SearchValue, int StartRowIndex)
		{
			
			int Stidx = 0;
			int edidx = gv.DataRowCount;
			int rst = -1;
			bool loop = true;
			DataRow dr;


			if (edidx > StartRowIndex && StartRowIndex >= 0) Stidx = StartRowIndex;

			while (loop)
			{
				for (int idx = Stidx; idx < gv.DataRowCount; idx++)
				{
					dr = gv.GetDataRow(idx);

					if(Fnc.obj2String(dr[SearchColumn]).Equals(SearchValue))
					{
						rst = idx;
						gv.FocusedRowHandle = idx;
						break;
					}

				}

				if (rst >= 0) break;

				if (Stidx != 0)
				{
					edidx = Stidx - 1;
					Stidx = 0;
					
				}
				else
					loop = false;
			}


			return rst;

		}


		static void EmbeddedNavigator_MouseLeave(object sender, EventArgs e)
		{
			GridControlNavigator n = sender as GridControlNavigator;

			GridControl gc = n.Parent as GridControl;

			((GridView)gc.FocusedView).OptionsBehavior.Editable = false;

		}

		static void EmbeddedNavigator_MouseHover(object sender, EventArgs e)
		{
			GridControlNavigator n = sender as GridControlNavigator;

			GridControl gc = n.Parent as GridControl;

			((GridView)gc.FocusedView).OptionsBehavior.Editable = true;
		}

		static DateTime dtEdit;

		static void gv_DoubleClick(object sender, EventArgs e)
		{
			gv_EditorOn(sender);		
		}

		static void gv_KeyDown(object sender, KeyEventArgs e)
		{
			DevExpress.XtraGrid.Views.Grid.GridView gv = sender as DevExpress.XtraGrid.Views.Grid.GridView;
			if (gv.IsEditing) return;

			if (e.KeyCode == Keys.Enter) gv_EditorOn(sender);
		}

		static void gv_EditorOn(object sender)
		{
			DevExpress.XtraGrid.Views.Grid.GridView gv = sender as DevExpress.XtraGrid.Views.Grid.GridView;

			gv.OptionsBehavior.Editable = true;			
			dtEdit = DateTime.Now;
			gv.ShowEditor();
		}

		/// <summary>
		/// view에 편집이 끝날 경우 처리.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		static void gv_HiddenEditor(object sender, EventArgs e)
		{
			DevExpress.XtraGrid.Views.Grid.GridView gv = sender as DevExpress.XtraGrid.Views.Grid.GridView;

			TimeSpan t = DateTime.Now - dtEdit;
			if (t.TotalMilliseconds > 500)
			{	
				gv.OptionsBehavior.Editable = false;
			}
		}

		


		/// <summary>
		/// 그리드 뷰의 필터 정보를 초기화 한다.
		/// </summary>
		/// <param name="gv"></param>
		public static void GridView_ColsFilterInit(GridView gv)
		{
			foreach (GridColumn col in gv.Columns)
			{
				col.SortOrder = DevExpress.Data.ColumnSortOrder.None;
				col.FilterInfo = new ColumnFilterInfo();
			}
		}


		/// <summary>
		/// 그리드 뷰에서 클립 보드 복사를 셀을 할 수 있도록 설정한다.
		/// </summary>
		/// <param name="gv"></param>
		public static void GridView_SetClipboard_Cell(GridView gv)
		{
			gv.KeyDown += new System.Windows.Forms.KeyEventHandler(gridView_SetClipboard_Cell_KeyDown);
		}

		/// <summary>
		/// 그리드 뷰에서 클립 보드 복사를 셀을 할 수 있도록 처리 한다.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		static void gridView_SetClipboard_Cell_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (!e.Control || e.KeyCode != System.Windows.Forms.Keys.C || e.Alt) return;

			GridView gv = sender as GridView;
			if (gv == null) return;

			gv.OptionsBehavior.CopyToClipboardWithColumnHeaders = true;


			if (gv.OptionsSelection.MultiSelectMode != GridMultiSelectMode.CellSelect) return;



			
			int c = gv.GetSelectedCells().Length;
			int a = 0, b=0;

			if (c > 0)
			{
				a = gv.GetSelectedCells()[0].Column.AbsoluteIndex;
				b = gv.GetSelectedCells()[c - 1].Column.AbsoluteIndex;
				c = gv.Columns.Count - 1;

				if ( (c == a || c == b) && (0 == a || 0 == b) )return;
			}

			

			gv.OptionsBehavior.CopyToClipboardWithColumnHeaders = false;
			
			//string value = Fnc.obj2String(gv.GetRowCellValue(gv.GetSelectedRows()[0], gv.FocusedColumn));

			//Clipboard.SetText(value);

			//e.Handled = true;


		}

		/// <summary>
		/// 그리드의 컬럼 폭을 조정하여 준다.
		/// </summary>
		/// <param name="gv"></param>
		/// <param name="AddtionalRatio">추가로 계산할 비율(%)</param>
		public static void GridView_SetColumnAutoWidth(GridView gv, int AddtionalRatio = 0)
		{
			int width;
			foreach (GridColumn col in gv.Columns)
			{
				width = col.GetBestWidth();
				width += width * AddtionalRatio / 100;
				col.Width = width;
			}
		}

		/// <summary>
		/// 그리드의 컬럼에 enmu값으로 콤보박스로 바인딩한다.
		/// </summary>
		/// <param name="gv"></param>
		/// <param name="col"></param>
		/// <param name="em">열거형을 new로 생성 하여 입력</param>
		public static void GridView_Column_SetEnumAsComboBox(GridView gv, GridColumn col, Enum em)
		{
			RepositoryItemComboBox item = new RepositoryItemComboBox();

			string[] items = Fnc.EnumItems2Strings(em);

			GridView_Column_SetStringsAsComboBox(gv, col, items);

		}

		/// <summary>
		/// 그리드의 컬럼에 string배열을 콤보박스로 바인딩한다.
		/// </summary>
		/// <param name="gv"></param>
		/// <param name="col"></param>
		/// <param name="items"></param>
		public static void GridView_Column_SetStringsAsComboBox(GridView gv, GridColumn col, string[] items)
		{
			RepositoryItemComboBox item = new RepositoryItemComboBox();
			item.Items.AddRange(items);

			//item.Items.AddRange(items);

			gv.GridControl.RepositoryItems.Add(item);
			col.ColumnEdit = item;

		}

		/// <summary>
		/// 그리드의 컬럼에 버튼 형식을 바인딩한다.
		/// </summary>
		/// <param name="gv"></param>
		/// <param name="col"></param>
		/// <param name="buttonCaption">버튼위에 보여줄 문자</param>
		/// <param name="editStyle">수정 방식(기본:문자를 보여주지 않음)</param>
		/// <returns>리턴 받은 item으로 이벤트를 등록 가능 click은 포커스시에만 일어나므로, GridControl의 Click이벤트도 같이 등록한다.</returns>
		public static RepositoryItemButtonEdit GridView_Column_Button(GridView gv, GridColumn col, 
			DevExpress.XtraEditors.Controls.TextEditStyles editStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor ,string buttonCaption = "...")
		{
			RepositoryItemButtonEdit item = new RepositoryItemButtonEdit();

			item.Buttons.Clear();

			item.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, buttonCaption, -1, true, true, false, 
				DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None))});

			item.TextEditStyle = editStyle;
			//item.Buttons[0].Caption = "TEST";

			gv.GridControl.RepositoryItems.Add(item);			
			col.ColumnEdit = item;			

			return item;

		}


		/// <summary>
		/// 그리드의 컬럼에 체크박스 형식을 바인딩한다.
		/// </summary>
		/// <param name="gv"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		public static RepositoryItemCheckEdit GridView_Column_CheckEdit(GridView gv, GridColumn col, object ValueChecked = null, object ValueUnchecked= null,
			object ValueGrayed = null)
		{
			RepositoryItemCheckEdit item = new RepositoryItemCheckEdit();

			if(ValueChecked == null) ValueChecked = true;
			if (ValueUnchecked == null) ValueUnchecked = false;


			item.ValueGrayed = ValueGrayed;
			item.ValueChecked = ValueChecked;
			item.ValueUnchecked = ValueUnchecked;
			
			gv.GridControl.RepositoryItems.Add(item);
			item.AllowGrayed = false;
			col.ColumnEdit = item;
			
			return item;

		}


		public static GroupEditProvider GridView_GroupEditor_Set(GridView gv, GridColumn default_colum = null, bool ShowGroupEditorOnMouseHover = true,
			bool SingleClick = true, bool EnableGroupEditing = true)
		{
			GroupEditProvider ge = new GroupEditProvider(gv);
			ge.ShowGroupEditorOnMouseHover = ShowGroupEditorOnMouseHover;
			ge.SingleClick = SingleClick;
			if(EnableGroupEditing) ge.EnableGroupEditing();

			if(default_colum != null) default_colum.GroupIndex = 0;

			return ge;
		}


	

	} //end classs
}
