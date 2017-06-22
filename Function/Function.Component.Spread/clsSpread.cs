using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FarPoint.Win.Spread;
using System.Drawing;
using System.Windows.Forms;
using Function;

namespace Function.Component
{
	/// <summary>
	/// Farpoint Spread관련 처리 Class
	/// </summary>
	public class Spread
	{
		/// <summary>
		/// 현재 시트에 선택된 row의 datarowview를 리턴한다.
		/// </summary>
		/// <param name="sv"></param>
		/// <returns></returns>
		public static DataRowView SelectRow_DataRowView_Get(FarPoint.Win.Spread.SheetView sv)
		{
			if (sv.DataSource == null) return null;
			int intRow = sv.ActiveRowIndex;
			if (intRow < 0) return null;

			return ((DataView)sv.DataSource)[intRow];
		}

		delegate void delInvoke_Cell_Value(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int Y, int X, object objValue);

		private static Function.Util.Log log = new Function.Util.Log("_Spread", "sp", 30, true);

		/// <summary>
		/// 셀에 값을 변경한다.
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="Y"></param>
		/// <param name="X"></param>
		/// <param name="objValue"></param>
		public static void Invoke_Cell_Value(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int Y, int X, object objValue)
		{
			if (fp.InvokeRequired)
			{
				fp.Invoke(new delInvoke_Cell_Value(Invoke_Cell_Value), new object[] { fp, sv, Y, X, objValue });
				return;
			}

			try
			{
				sv.Cells[Y, X].Value = objValue;
			}
			catch (Exception ex)
			{
				log.WLog_Exception(string.Format("{0}[sv]{1}[y}{2}[x]{3][obj]{4}", "Invoke_Cell_Value", fp.Name, Y, X, objValue), ex);
			}



		}

		delegate void delInvoke_CellRange_Value(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, 
					int Y, int X, int Y2, int X2, object objValue);

		/// <summary>
		/// 셀 범위에 값을 변경한다.
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="Y"></param>
		/// <param name="X"></param>
		/// <param name="Y2"></param>
		/// <param name="X2"></param>
		/// <param name="objValue"></param>
		public static void Invoke_CellRange_Value(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, 
			int Y, int X, int Y2, int X2, object objValue)
		{
			if (fp.InvokeRequired)
			{
				fp.Invoke(new delInvoke_CellRange_Value(Invoke_CellRange_Value), new object[] { fp, sv, Y, X, Y2, X2, objValue });
				return;
			}

			sv.Cells[Y, X, Y2, X2].Value = objValue;
		}





		delegate void delInvoke_AddRow(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int intAddIndex, int intAddRowCount);

		/// <summary>
		/// fpSheet에 Row를 추가 하여준다.
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="intAddIndex">들어갈 위치 : '-1'이면 마지막에 위치 시킨다.</param>
		/// <param name="intAddRowCount"></param>
		public static void Invoke_AddRow(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int intAddIndex, int intAddRowCount)
		{
			if (fp.InvokeRequired)
			{
				fp.Invoke(new delInvoke_AddRow(Invoke_AddRow), new object[] { fp, sv, intAddIndex, intAddRowCount });
				return;
			}

			if (intAddIndex < 0)
			{
				intAddIndex = sv.Rows.Count;
			}


			sv.Rows.Add(intAddIndex, intAddRowCount);
		}


		/// <summary>
		/// row를 삭제 한다.
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="intDelIndex">삭제 위치 : '-1'이면 마지막부터 위로 삭제 시킨다.</param>
		/// <param name="intDelRowCount"></param>
		public static void Invoke_DeleteRow(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int intDelIndex, int intDelRowCount )
		{
			if (fp.InvokeRequired)
			{
				fp.Invoke(new delInvoke_AddRow(Invoke_DeleteRow), new object[] { fp, sv, intDelIndex, intDelRowCount });
				return;
			}

			if (intDelIndex < 0)
			{
				intDelIndex = sv.Rows.Count - intDelRowCount;
			}


			sv.Rows.Remove(intDelIndex, intDelRowCount);
		}


		delegate void delInvoke_AddRowData(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int intAddIndex,int intMaxRowCount, object[] objData); 

		/// <summary>
		/// fpSheet에 Row를 추가 하여준다.
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="intAddIndex">들어갈 위치 : '-1'이면 마지막에 위치 시킨다.</param>		
		/// <param name="intMaxRowCount">최대 행 유지 갯수 : 0이면 무제한 증가..</param>
		public static void Invoke_AddRowData(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int intAddIndex,
			int intMaxRowCount, object [] objData)
		{
			if (fp.InvokeRequired)
			{
				fp.Invoke(new delInvoke_AddRowData(Invoke_AddRowData), new object[] { fp, sv, intAddIndex, intMaxRowCount, objData });
				return;
			}

			try
			{
				if (intAddIndex < 0)
				{
					intAddIndex = sv.Rows.Count;
				}

				sv.Rows.Add(intAddIndex, 1);

				
				int intCol = 0;
				foreach (object obj in objData)
				{
					//if (sv.ColumnCount <= intCol) break;
					sv.Cells[intAddIndex, intCol].Value = obj;
					intCol++;
				}



				//최대행 유지를 위해 Row를 삭제 한다.
				if (intMaxRowCount > 0 && sv.Rows.Count >= intMaxRowCount)
				{
					int intDelIndex = 0;
					int intDelCount = 1 + intMaxRowCount - sv.Rows.Count;

					//아래 추가 시만 위에 삭제, 그외 경우는 아래 부분 삭제..
					if (intAddIndex < 0)
						intDelIndex = 0;
					else
					{
						intDelIndex = sv.Rows.Count - intDelCount;
					}

					sv.Rows.Remove(intDelIndex, 1);
				}
				

			}
			catch (Exception ex)
			{
				log.WLog_Exception(string.Format("Invoke_AddRowData[fp]{0}[AddIdx]{1}[MaxRow]{2}", fp.Name, intAddIndex, intMaxRowCount), ex);
			}


		}


		delegate void delInvoke_AddData(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, DataTable dt, string[] strColumn,
			int intAddIndex, int intMaxRowCount, bool isClear);

		/// <summary>
		/// 시트에 datatable에 있는 데이터를 컬럼을 이용하여 넣어 준다.
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="dt"></param>
		/// <param name="strColumn"></param>
		/// <param name="intAddIndex">들어갈 위치 : '-1'이면 마지막에 위치 시킨다</param>
		/// <param name="intMaxRowCount">최대 행 유지 갯수 : 0이면 무제한 증가..</param>
		/// <param name="isClear"></param>
		public static void Invoke_AddData(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, DataTable dt, string[] strColumn,
			int intAddIndex, int intMaxRowCount, bool isClear)
		{
			if (fp.InvokeRequired)
			{
				fp.Invoke(new delInvoke_AddData(Invoke_AddData), new object[] { fp, sv, dt, strColumn, intAddIndex, intMaxRowCount, isClear });
				return;
			}

			if (isClear)
			{
				sv.RowCount = 0;
			}

			int intRow;


			

			//sv.RowCount = sv.RowCount + dt.Rows.Count;

			//string strValue;

			foreach (DataRow dr in dt.Rows)
			{

				if (intAddIndex < 0)
				{
					intRow = sv.RowCount;
					sv.RowCount++;
				}
				else
				{
					intRow = 0;
					sv.Rows.Add(0, 1);
				}

				int intCol = 0;

				foreach (string strCol in strColumn)
				{
					if (strCol == string.Empty)
						sv.Cells[intRow, intCol].Value = string.Empty;
					else
					{

						if (sv.Columns.Get(intCol).CellType != null && sv.Columns.Get(intCol).CellType.GetType() == typeof(FarPoint.Win.Spread.CellType.CheckBoxCellType))
						{
							if (dr[strCol].ToString().ToUpper() == "TRUE" || dr[strCol].ToString().ToUpper() == "1")
								sv.Cells[intRow, intCol].Value = true;
							else
								sv.Cells[intRow, intCol].Value = false;
						}
						else
						{
							sv.Cells[intRow, intCol].Value = dr[strCol];
						}
					}

					intCol++;
				}

				intRow++;
			}


			//최대행 유지를 위해 Row를 삭제 한다.
			if(intMaxRowCount > 0 && sv.Rows.Count >= intMaxRowCount)
			{
				int intDelIndex = 0;
				int intDelCount = sv.Rows.Count - intMaxRowCount;

				//아래 추가 시만 위에 삭제, 그외 경우는 아래 부분 삭제..
				if (intAddIndex < 0)
					intDelIndex = 0;
				else
				{
					intDelIndex = intMaxRowCount;
				}

				sv.Rows.Remove(intDelIndex, intDelCount);
			}

		}


		/// <summary>
		/// 시트에 datatable에 있는 데이터를 컬럼을 이용하여 넣어 준다.
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="dt"></param>
		/// <param name="strColumn"></param>
		/// <param name="isClear"></param>
		public static void Invoke_AddData(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, DataTable dt, string[] strColumn, bool isClear)
		{
			Invoke_AddData( fp, sv, dt, strColumn, -1, 0, isClear );
			
		}

		delegate void delInvoke_SearchThenChBackcolor(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int[] intCols, string str, Color colBackColor);
		/// <summary>
		/// 해당 컬럼에 값이 일치 하면 바탕색을 바꿔준다..
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="intCols"></param>
		/// <param name="str"></param>
		/// <param name="colBackColor"></param>
		public static void Invoke_SearchThenChBackcolor(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int [] intCols, string str, Color colBackColor)
		{
			if (fp.InvokeRequired)
			{
				fp.Invoke(new delInvoke_SearchThenChBackcolor(Invoke_SearchThenChBackcolor), new object[] { fp, sv, intCols, str, colBackColor });
				return;
			}


			int intColCnt = sv.ColumnCount - 1;

			for (int y = 0; y < sv.Rows.Count; y++)
			{
				foreach (int intCol in intCols)
				{
					int rst = sv.Cells[y, intCol].Text.IndexOf(str);

					if (rst >= 0)
					{
						sv.Cells[y, 0, y, intColCnt].BackColor = colBackColor;
						break;
					}

				}
			}
		}


		delegate void delInvoke_SearchThenChForecolor(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int[] intCols, string str, Color colForecolor);
		/// <summary>
		/// 해당 컬럼에 값이 일치 하면 글자색을 바꿔준다..
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="intCols"></param>
		/// <param name="str"></param>
		/// <param name="colBackColor"></param>
		public static void Invoke_SearchThenChForecolor(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int[] intCols, string str, Color colForecolor)
		{
			if (fp.InvokeRequired)
			{
				fp.Invoke(new delInvoke_SearchThenChForecolor(Invoke_SearchThenChForecolor), new object[] { fp, sv, intCols, str, colForecolor });
				return;
			}


			int intColCnt = sv.ColumnCount - 1;

			for (int y = 0; y < sv.Rows.Count; y++)
			{
				foreach (int intCol in intCols)
				{
					int rst = sv.Cells[y, intCol].Text.IndexOf(str);

					if (rst >= 0)
					{
						sv.Cells[y, 0, y, intColCnt].ForeColor = colForecolor;
						break;
					}

				}
			}
		}
		


		/// <summary>
		/// 스프레드에서 원하는 값을 가지 row를 찾는다.
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="intSheetIndex"></param>
		/// <param name="strSearchText"></param>
		/// <returns></returns>
		public static int[] SearchData(FarPoint.Win.Spread.FpSpread fp, int intSheetIndex, string strSearchText)
		{
			Dictionary<int, int> q = new Dictionary<int, int>();
			int x = 0;
			int y = 0;

			while (y != -1)
			{
				fp.Search(intSheetIndex, strSearchText, false, false, false, true, y, x, ref y, ref x);

				if (y == -1) break;


				if (y != -1 && !q.ContainsKey(y))
				{
					q.Add(y, y);
				}


				if (x >= fp.Sheets[intSheetIndex].Columns.Count - 1)
				{
					if (y >= fp.Sheets[intSheetIndex].Rows.Count - 1)
						break;
					else
					{
						x = 0;
						y++;
					}
				}
				else
					x++;


			}

			int[] intRows = new int[q.Count];

			y = 0;
			foreach (object intRow in q.Values)
			{
				intRows[y] = (int)intRow;
				y++;
			}


			return intRows;			

		}


		/// <summary>
		/// 스프레드에서 원하는 값을 찾는다.
		/// </summary>
		/// <param name="sv"></param>
		/// <param name="strFindValue"></param>
		/// <param name="intColumns"></param>
		/// <returns></returns>
		public static int[] FindData(FarPoint.Win.Spread.SheetView sv, string strFindValue, int[] intColumns)
		{
			Queue<int> q = new Queue<int>();

			for (int row = 0; row < sv.Rows.Count; row++)
			{
				foreach (int col in intColumns)
				{

					if (sv.Cells[row, col].Text.ToUpper() == strFindValue.ToUpper())
					{
						q.Enqueue(row);
						continue;
					}
				}
			}

			if (q.Count < 1)
				return null;
			else
			{
				int[] j = new int[q.Count];
				int i = 0;
				while (q.Count > 0)
				{
					j[i] = q.Dequeue();
					i++;
				}

				return j;

			}



		}



		/// <summary>
		/// 프린터
		/// </summary>
		/// <param name="fp"></param>
		public static void PrintOption_Show(FarPoint.Win.Spread.FpSpread fp)
		{
			frmPrintOpts frm = new frmPrintOpts(fp);
			frm.ShowDialog();
		}


		public static bool Excel_SaveAs(FarPoint.Win.Spread.FpSpread fp)
		{
			SaveFileDialog sf = new SaveFileDialog();
			sf.Filter = "Excel files (*.xls)|*.xls |All files (*.*) | *.*";
			if (sf.ShowDialog() != DialogResult.OK) return false;

			//if (sf.FileName == null || sf.FileName == string.Empty) return;

			fp.SaveExcel(sf.FileName, FarPoint.Win.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);

			return true;
		}


		public struct StBindingOption
		{
			/// <summary>
			/// 자동 컬럼 바인딩 여부
			/// </summary>
			public bool AutoGenerateColumns;
			/// <summary>
			/// 자동 해더 이름 바인딩 여부
			/// </summary>
			public bool DataAutoHeadings;
			/// <summary>
			/// 자동 셀 타입 변경 여부
			/// </summary>
			public bool DataAutoCellTypes;
			/// <summary>
			/// 자동 컬럼 크기 조절 여부
			/// </summary>		
			public bool DataAutoSizeColumns;

			public StBindingOption(bool all)
			{
				AutoGenerateColumns = all;
				DataAutoHeadings = all;
				DataAutoCellTypes = all;
				DataAutoSizeColumns = all;
			}

			public StBindingOption(bool autoGenerateColumns, bool dataAutoHeadings, bool dataAutoCellTypes, bool dataAutoSizeColumns)
			{
				AutoGenerateColumns = autoGenerateColumns;
				DataAutoHeadings = dataAutoHeadings;
				DataAutoCellTypes = dataAutoCellTypes;
				DataAutoSizeColumns = dataAutoSizeColumns;
			}

		}

		delegate void delInvoke_DataSource(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, DataTable dt, string[] strField, StBindingOption bindingOption);
		/// <summary>
		/// databinding with field
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="dt"></param>
		/// <param name="strField"></param>
		public static void Invoke_DataSource(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, DataTable dt, string[] strField, StBindingOption bindingOption)
		{
			if (fp.InvokeRequired)
			{
				fp.Invoke(new delInvoke_DataSource(Invoke_DataSource), new object[] { fp, sv, dt, strField, bindingOption });
				return;
			}

			try
			{
				sv.Rows.Count = 0;

				sv.AutoGenerateColumns = bindingOption.AutoGenerateColumns;
				sv.DataAutoHeadings = bindingOption.DataAutoHeadings;
				sv.DataAutoCellTypes = bindingOption.DataAutoCellTypes;
				sv.DataAutoSizeColumns = bindingOption.DataAutoSizeColumns;

				if (dt != null) sv.DataSource = dt;

				if (strField == null) return;

				for (int x = 0; x < strField.Length; x++)
				{
					sv.Columns[x].DataField = strField[x];
				}
			}
			catch (Exception ex)
			{
				log.WLog_Exception(string.Format("{0}[fp]{1}", "Invoke_DataSource", fp.Name), ex);
			}
			
		}

		private delegate void delShowCell(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int row, int col);

		/// <summary>
		/// 원하는 셀을 보여 준다.
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="row"></param>
		/// <param name="col"></param>
		public static void Invoke_ShowCell(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int row, int col)
		{
			if(fp.InvokeRequired)
			{
				fp.BeginInvoke(new delShowCell(Invoke_ShowCell), new object[] { fp, sv, row, col });
				return;
			}
			sv.SetActiveCell(row, col, true);
			fp.ShowActiveCell(VerticalPosition.Center, HorizontalPosition.Center);
		}


		/// <summary>
		/// databinding with field
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="dt"></param>
		/// <param name="strField"></param>
		public static void Invoke_DataSource(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, DataTable dt, string[] strField)
		{
			Invoke_DataSource(fp, sv, dt, strField, new StBindingOption(false));
		}

		delegate void delInvoke_DataSource2(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, DataView dt, string[] strField, StBindingOption bindingOption);
		/// <summary>
		/// databinding with field
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="dt"></param>
		/// <param name="strField"></param>
		public static void Invoke_DataSource(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, DataView dt, string[] strField, StBindingOption bindingOption)
		{
			if (fp.InvokeRequired)
			{
				fp.Invoke(new delInvoke_DataSource2(Invoke_DataSource), new object[] { fp, sv, dt, strField, bindingOption });
				return;
			}

			try
			{
				sv.AutoGenerateColumns = bindingOption.AutoGenerateColumns;
				sv.DataAutoHeadings = bindingOption.DataAutoHeadings;
				sv.DataAutoCellTypes = bindingOption.DataAutoCellTypes;
				sv.DataAutoSizeColumns = bindingOption.DataAutoSizeColumns;

				sv.Rows.Count = 0;

				if (dt != null) sv.DataSource = dt;

				if (strField == null) return;

				for (int x = 0; x < strField.Length; x++)
				{
					sv.Columns[x].DataField = strField[x];
				}
			}
			catch (Exception ex)
			{
				log.WLog_Exception(string.Format("{0}[fp]{1}", "Invoke_DataSource", fp.Name), ex);
			}

		}

		/// <summary>
		/// databinding with field
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="dt"></param>
		/// <param name="strField"></param>
		public static void Invoke_DataSource(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, DataView dt, string[] strField)
		{
			Invoke_DataSource(fp, sv, dt, strField, new StBindingOption(false));
		}

		delegate void delInvoke_Row_SetColor(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int intRow, object colFore, object colBack);

		/// <summary>
		/// 해당 Row의 forecolor과 backcolor을 변경한다.
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="intRow"></param>
		/// <param name="colFore">null 이면 변경 않</param>
		/// <param name="colBack">null 이면 변경 않</param>
		public static void Invoke_Row_SetColor(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int intRow, object colFore, object colBack)				
		{
			if (fp.InvokeRequired)
			{
				fp.Invoke(new delInvoke_Row_SetColor(Invoke_Row_SetColor), new object[] { fp, sv, intRow, colFore, colBack });
				return;
			}

			if (intRow >= sv.RowCount) return;

			if (colBack != null) sv.Rows[intRow].BackColor = (Color)colBack;
			if (colFore != null) sv.Rows[intRow].ForeColor = (Color)colFore;

			fp.Refresh();


		}

		delegate void delInvoke_CellRange_InStr_SetRowColor(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, string strInStr,
		int intChkCol, int intStCol, int intEdCol, object colFore, object colBack);
		/// <summary>
		/// 범위에 검색하는 문자가 있으면 범위에 row단위로 글자색/배경색을 변경 하여준다..
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="strInStr"></param>
		/// <param name="intChkCol"></param>
		/// <param name="intStCol"></param>
		/// <param name="intEdCol"></param>
		/// <param name="colFore">null 이면 변경 않함</param>
		/// <param name="colBack">null 이면 변경 않함</param>
		public static void Invoke_CellRange_InStr_SetRowColor(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, string strInStr,
		int intChkCol, int intStCol, int intEdCol, object colFore, object colBack)
		{
			if (fp.InvokeRequired)
			{
				fp.Invoke(new delInvoke_CellRange_InStr_SetRowColor(Invoke_CellRange_InStr_SetRowColor), new object[] { fp, sv, strInStr, intChkCol, intStCol, 
					intEdCol, colFore, colBack });
				return;
			}

			for (int y = 0; y < sv.Rows.Count; y++)
			{
				if (sv.Cells[y, intChkCol].Text.IndexOf(strInStr) >= 0)
				{
					if (colFore != null) sv.Cells[y, intStCol, y, intEdCol].ForeColor = (Color)colFore;
					if (colBack != null)  sv.Cells[y, intStCol, y, intEdCol].BackColor = (Color)colBack;
				}
			}

			fp.Refresh();


		}

		delegate void delInvoke_CellRange_SetRowColor(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int intStRow, int intEdRow, int intStCol, int intEdCol,
				object colFore, object colBack);
		/// <summary>
		/// 범위에 ForeColor와 BackColor를 변경하여 준다..
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="intStRow">시작 row</param>
		/// <param name="intEdRow">종료 row</param>
		/// <param name="intStCol">시작 col</param>
		/// <param name="intEdCol">종료 col</param>
		/// <param name="colFore">object color - 변경 안할시에는 null</param>
		/// <param name="colBack">object color - 변경 안할시에는 null</param>
		public static void Invoke_CellRange_SetRowColor(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int intStRow, int intEdRow, int intStCol, int intEdCol, 
				object colFore, object colBack)
		{
			if (fp.InvokeRequired)
			{
				fp.Invoke(new delInvoke_CellRange_SetRowColor(Invoke_CellRange_SetRowColor), new object[] { fp, sv, intStRow, intEdRow, intStCol, intEdCol, colFore, colBack });
				return;
			}

			if (colFore != null) sv.Cells[intStRow, intStCol, intEdRow, intEdCol].ForeColor = (Color)colFore;
			if (colBack != null) sv.Cells[intStRow, intStCol, intEdRow, intEdCol].BackColor = (Color)colBack;		
			
			

			fp.Refresh();
		}



		delegate void delInvoke_CellSpan(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int intRow, int intCol, int intRowSpanCnt, int intColSpanCnt);
	    /// <summary>
	    /// 셀을 스팬(머지) 한다.
	    /// </summary>
	    /// <param name="fp"></param>
	    /// <param name="sv"></param>
	    /// <param name="intRow"></param>
	    /// <param name="intCol"></param>
	    /// <param name="intRowSpanCnt"></param>
	    /// <param name="intColSpanCnt"></param>		
		public static void Invoke_CellSpan(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int intRow, int intCol, int intRowSpanCnt, int intColSpanCnt)
		{
			if (fp.InvokeRequired)
			{
				fp.Invoke(new delInvoke_CellSpan(Invoke_CellSpan), new object[] { fp, sv, intRow, intCol, intRowSpanCnt, intColSpanCnt });
				return;
			}

			sv.Cells[intRow, intCol].ColumnSpan = intColSpanCnt;
			sv.Cells[intRow, intCol].RowSpan = intRowSpanCnt;		
			
		}



		delegate void delInvoke_RowCount(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int intRowCount);

		/// <summary>
		/// row count를 변경한다.
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="intRowCount"></param>
		public static void Invoke_RowCount(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, int intRowCount)
		{
			if (fp.InvokeRequired)
			{
				fp.Invoke(new delInvoke_RowCount(Invoke_RowCount), new object[] { fp, sv, intRowCount });
				return;
			}

			sv.RowCount = intRowCount;
		}


		/// <summary>
		/// 바인딩된 해당 row의 datarowview를 리턴한다.
		/// </summary>
		/// <param name="sv"></param>
		/// <param name="intRowIndexs">null 이면 선택된 row</param>
		/// <returns></returns>
		public static DataRowView[] DataBind_GetSelected_Rowviews(FarPoint.Win.Spread.SheetView sv, int [] intRowIndexs)
		{
			if (sv.DataSource == null) return null;

			//datasource를 dataview로 변환
			DataView dv = sv.DataSource as DataView;

			if (dv == null)
			{
				DataSet ds = sv.DataSource as DataSet;

				if (ds != null)
					dv = ds.Tables[sv.DataMember].DefaultView;
				else
					dv = ((DataTable)sv.DataSource).DefaultView;
			}

			if (dv == null) return null;

			DataRowView[] drvs;
			int intCnt;

			if (intRowIndexs == null)
			{
				intCnt = sv.ActiveRow.Index2 - sv.ActiveRow.Index + 1;
				drvs = new DataRowView[intCnt];

				for (int i = 0; i < intCnt; i++)
				{
					drvs[i] = dv[sv.ActiveRow.Index + i];
				}

				return drvs;
			}
			else
			{
				intCnt = intRowIndexs.Length;
				drvs = new DataRowView[intCnt];

				for (int i = 0; i < intCnt; i++)
				{
					drvs[i] = dv[intRowIndexs[i]];
				}

				return drvs;

			}

		}

		private delegate void delInvoke_CellFormula_Set(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, FarPoint.Win.Spread.Cell cell, string formulaString,
			FarPoint.Win.Spread.CellType.BaseCellType celltype);
		
		/// <summary>
		/// Cell에 Fomula셑팅을 한다.
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="sv"></param>
		/// <param name="cell"></param>
		/// <param name="formulaString"></param>
		/// <param name="celltype">변경할 셀 타입 FarPoint.Win.Spread.CellType 네임스페이스에서 새로운 클래스생성 하여 넘긴다. 무변경 null</param>
		public static void Invoke_CellFormula_Set(FarPoint.Win.Spread.FpSpread fp, FarPoint.Win.Spread.SheetView sv, FarPoint.Win.Spread.Cell cell, string formulaString,
			FarPoint.Win.Spread.CellType.BaseCellType celltype)
		{

			if (fp.InvokeRequired)
			{
				fp.BeginInvoke(new delInvoke_CellFormula_Set(Invoke_CellFormula_Set), new object[] { fp, sv, cell, formulaString, celltype});
				return;
			}

			string textvalue = string.Empty;
			cell.Formula = formulaString;

			if (celltype != null)
			{
				cell.CellType = celltype;
			}

			/*
			textvalue = cell.Text;
			FarPoint.Win.Spread.CellType.TextCellType t = new FarPoint.Win.Spread.CellType.TextCellType();
			cell.CellType = t;
			cell.Formula = "";
			cell.Value = textvalue;
			 */

		}


		class stColorCellInfo
		{
			public Cell cell;
			public string value;
			public Color changedCellColor;

			public stColorCellInfo(Cell _cell, Color _changedCellColor)
			{
				cell = _cell;
				if (_cell == null)
					value = null;
				else
					value = Fnc.obj2String(_cell.Value);


				changedCellColor = _changedCellColor;
			}

			public void Cell_Set(Cell _cell)
			{
				cell = _cell;
				value = Fnc.obj2String(_cell.Value);

				Console.WriteLine("Set [Cell]{0} [Value]{1}", cell, value);
			}

			public bool IsValueChanged
			{
				get
				{
					Console.WriteLine("Changed [Cell]{0} [OldValue]{1} [NewValue]{2}", cell, value, cell.Value);
					return !value.Equals(Fnc.obj2String(cell.Value));
				}
			}

			
			
		}

		static Dictionary<FpSpread, stColorCellInfo> dicColorCells;

		/// <summary>
		/// 셀 값이 변경 되면 셀의 컬러를 변경 한다.
		/// </summary>
		/// <param name="fp"></param>
		/// <param name="changedCellColor"></param>
		public static void Set_EditCell_ColorChange(FpSpread fp, Color changedCellColor)
		{
			if(dicColorCells == null)  dicColorCells = new Dictionary<FpSpread, stColorCellInfo>();
			fp.EditModeOn += new EventHandler(fp_EditModeOn);
			fp.EditModeOff += new EventHandler(fp_EditModeOff);

			if (dicColorCells.ContainsKey(fp))
				dicColorCells[fp] = new stColorCellInfo(fp.ActiveSheet.ActiveCell, changedCellColor);
			else
				dicColorCells.Add(fp, new stColorCellInfo(fp.ActiveSheet.ActiveCell, changedCellColor));
			
		}

		static void fp_EditModeOff(object sender, EventArgs e)
		{
			FpSpread fp = sender as FpSpread;

			if (dicColorCells[fp].IsValueChanged)
				fp.ActiveSheet.ActiveCell.BackColor = dicColorCells[fp].changedCellColor;
		}

		static void fp_EditModeOn(object sender, EventArgs e)
		{
			try
			{
				FpSpread fp = sender as FpSpread;
				
				dicColorCells[fp].Cell_Set(fp.ActiveSheet.ActiveCell);

			}
			catch
			{
			}
			


		}

		
		

	}
}
