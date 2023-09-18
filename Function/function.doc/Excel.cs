
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace function.doc
{
	/// <summary>
	/// 엑셀처리 클래스 - Open XML SDK 사용
	/// </summary>
	public class Excel
    {
		/// <summary>
		/// datatable을 엑셀 파일로 출력 합니다.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="filename">null 파일 다이얼 로그를 연다</param>
		/// <param name="sheetName"></param>
		/// <param name="isColNameWrite"></param>
		public static void Export(DataTable dt, string filename, string sheetName = "Sheet1", bool isColNameWrite = true)
		{
			if(filename == null)
			{
				SaveFileDialog of = new SaveFileDialog();
				of.Filter = "Excel File(xlsx)|*.xlsx|All Files|*.*";

				if (of.ShowDialog() != DialogResult.OK) return;

				filename = of.FileName;
			}			

			// Create a spreadsheet document by supplying the filepath.
			// By default, AutoSave = true, Editable = true, and Type = xlsx.
			SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filename, SpreadsheetDocumentType.Workbook);

			// Add a WorkbookPart to the document.
			WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
			workbookpart.Workbook = new Workbook();

			// Add a WorksheetPart to the WorkbookPart.
			WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
			worksheetPart.Worksheet = new Worksheet(new SheetData());

			// Add Sheets to the Workbook.
			Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

			// Append a new worksheet and associate it with the workbook.
			Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = sheetName };
			sheets.Append(sheet);

			// Get the sheetData cell table.
			SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();


			// Add a row to the cell table.
			Row row;
			UInt32Value rIdx = 0;
			//row = new Row() { RowIndex = 1 };
			//sheetData.Append(row);
			Cell ce;

			int col = 0;

			//컬럼 이름을 쓴다
			if (isColNameWrite)
			{
				rIdx++;
				row = new Row() { RowIndex = rIdx };
				

				sheetData.Append(row);
				
				foreach(DataColumn dc in dt.Columns)
				{
					ce = new Cell();
					ce.DataType = new EnumValue<CellValues>(CellValues.String);
					ce.CellValue = new CellValue(dc.ColumnName);
					row.InsertAt(ce, col);
					col++;
				}
			}

			//data write
			foreach (DataRow dr in dt.Rows)
			{
				rIdx++;
				row = new Row() { RowIndex = rIdx };
				sheetData.Append(row);

				col = 0;

				foreach (DataColumn dc in dt.Columns)
				{
					ce = new Cell();
					ce.DataType = new EnumValue<CellValues>(CellValues.String);
					ce.CellValue = new CellValue(Fnc.obj2String(dr[dc.ColumnName]));
					row.InsertAt(ce, col);
					col++;
				}

			}


			//// In the new row, find the column location to insert a cell in A1.  
			//Cell refCell = null;
			//foreach (Cell cell in row.Elements<Cell>())
			//{
			//	if (string.Compare(cell.CellReference.Value, "A1", true) > 0)
			//	{
			//		refCell = cell;
			//		break;
			//	}
			//}

			//// Add the cell to the cell table at A1.
			//Cell newCell = new Cell() { CellReference = "A1" };
			////row.InsertBefore(newCell, refCell);
			//row.InsertAt(newCell, 0);
			//// Set the cell value to be a numeric value of 100.
			//newCell.CellValue = new CellValue("100");
			//newCell.DataType = new EnumValue<CellValues>(CellValues.Number);
			//refCell = newCell;


			//newCell = new Cell(); // { CellReference = "B1" };
			//					  //row.InsertBefore(newCell, refCell);			
			//row.InsertAt(newCell, 1);

			//// Set the cell value to be a numeric value of 100.
			//newCell.CellValue = new CellValue("300");
			//newCell.DataType = new EnumValue<CellValues>(CellValues.Number);


			workbookpart.Workbook.Save();
			// Close the document.
			spreadsheetDocument.Close();
		}


	}	//end class
}
