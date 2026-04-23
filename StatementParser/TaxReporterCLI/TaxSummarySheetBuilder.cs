using System;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace TaxReporterCLI
{
	internal class TaxSummarySheetBuilder
	{
		public ISheet Build(XSSFWorkbook workbook, Dictionary<string, (ISheet Sheet, int DataRowCount)> sheetMeta)
		{
			var summarySheet = workbook.CreateSheet("Tax Summary");

			// CZK currency format
			var czkFormat = workbook.CreateDataFormat().GetFormat("#,##0.00 \"Kč\"");
			var czkStyle = workbook.CreateCellStyle();
			czkStyle.DataFormat = czkFormat;

			// Row 0: Header
			var headerRow = summarySheet.CreateRow(0);
			headerRow.CreateCell(0).SetCellValue("Section");
			headerRow.CreateCell(1).SetCellValue("Daily Rate (CZK)");
			headerRow.CreateCell(2).SetCellValue("Yearly Rate (CZK)");

			// Define sections: (label, sheetName, dailyPrefix, dailySuffix, yearlyPrefix, yearlySuffix)
			var sections = new[]
			{
				("Dividends - Income", "DividendTransactionView", "Income", "per Day", "Income", "per Year"),
				("Sales - Profit", "SaleTransactionView", "Profit", "per Day", "Profit", "per Year"),
				("ESPP - Profit", "ESPPTransactionView", "Total Profit", "per Day", "Total Profit", "per Year"),
				("Deposits - Price", "DepositTransactionView", "Total Price", "per Day", "Total Price", "per Year"),
			};

			int firstDataRow = 2; // Excel 1-based: row 2 is the first data row
			int summaryRowIndex = 1;
			foreach (var (label, sheetName, dailyPrefix, dailySuffix, yearlyPrefix, yearlySuffix) in sections)
			{
				var row = summarySheet.CreateRow(summaryRowIndex);
				row.CreateCell(0).SetCellValue(label);

				if (sheetMeta.TryGetValue(sheetName, out var meta) && meta.DataRowCount > 0)
				{
					WriteSumFormulaCell(row, 1, meta.Sheet, meta.DataRowCount, dailyPrefix, dailySuffix, czkStyle);
					WriteSumFormulaCell(row, 2, meta.Sheet, meta.DataRowCount, yearlyPrefix, yearlySuffix, czkStyle);
				}
				else
				{
					CreateZeroCell(row, 1, czkStyle);
					CreateZeroCell(row, 2, czkStyle);
				}

				summaryRowIndex++;
			}

			// Total row (SUM of all section rows above)
			int lastDataRow = summaryRowIndex; // Excel 1-based: current summaryRowIndex equals last data Excel row
			var totalRow = summarySheet.CreateRow(summaryRowIndex);
			totalRow.CreateCell(0).SetCellValue("Total");
			var totalDailyCell = totalRow.CreateCell(1);
			totalDailyCell.SetCellFormula($"SUM(B{firstDataRow}:B{lastDataRow})");
			totalDailyCell.CellStyle = czkStyle;
			var totalYearlyCell = totalRow.CreateCell(2);
			totalYearlyCell.SetCellFormula($"SUM(C{firstDataRow}:C{lastDataRow})");
			totalYearlyCell.CellStyle = czkStyle;
			summaryRowIndex++;

			// Empty row
			summaryRowIndex++;

			// Dividends - Tax withheld (separate from total)
			var taxRow = summarySheet.CreateRow(summaryRowIndex);
			taxRow.CreateCell(0).SetCellValue("Dividends - Tax withheld");

			if (sheetMeta.TryGetValue("DividendTransactionView", out var divMeta) && divMeta.DataRowCount > 0)
			{
				WriteSumFormulaCell(taxRow, 1, divMeta.Sheet, divMeta.DataRowCount, "Tax", "per Day", czkStyle);
				WriteSumFormulaCell(taxRow, 2, divMeta.Sheet, divMeta.DataRowCount, "Tax", "per Year", czkStyle);
			}
			else
			{
				CreateZeroCell(taxRow, 1, czkStyle);
				CreateZeroCell(taxRow, 2, czkStyle);
			}

			// Auto-size columns
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				for (int col = 0; col <= 2; col++)
				{
					summarySheet.AutoSizeColumn(col);
				}
			}

			return summarySheet;
		}

		private static void WriteSumFormulaCell(IRow row, int cellIndex, ISheet sourceSheet, int dataRowCount, string headerPrefix, string headerSuffix, ICellStyle style)
		{
			var colIndex = ExcelHelper.FindColumnIndex(sourceSheet, headerPrefix, headerSuffix);
			var cell = row.CreateCell(cellIndex);
			cell.CellStyle = style;
			if (colIndex >= 0)
			{
				var colLetter = ExcelHelper.GetExcelColumnLetter(colIndex);
				var formula = $"SUM({sourceSheet.SheetName}!{colLetter}2:{colLetter}{dataRowCount + 1})";
				cell.SetCellFormula(formula);
			}
			else
			{
				cell.SetCellValue(0);
			}
		}

		private static void CreateZeroCell(IRow row, int cellIndex, ICellStyle style)
		{
			var cell = row.CreateCell(cellIndex);
			cell.SetCellValue(0);
			cell.CellStyle = style;
		}
	}
}
