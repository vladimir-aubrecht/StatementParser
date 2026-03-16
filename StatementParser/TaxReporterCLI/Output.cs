using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using StatementParser.Attributes;
using TaxReporterCLI.Models.Views;

namespace TaxReporterCLI
{
	internal class Output
	{
		private Dictionary<string, List<IView>> GroupViews(IList<IView> views)
		{
			return views.GroupBy(i => i.GetType()).ToDictionary(k => k.Key.Name, i => i.Select(a => a).OrderBy(x => x).ToList());
		}

		public void PrintAsJson(IList<IView> views)
		{
			var groupedTransactions = GroupViews(views);

			Console.WriteLine(JsonConvert.SerializeObject(groupedTransactions));
		}

		public void SaveAsExcelSheet(string filePath, IList<IView> views)
		{
			var groupedViews = GroupViews(views);

            using FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            var wb1 = new XSSFWorkbook();

            var sheetMeta = new Dictionary<string, (ISheet Sheet, int DataRowCount)>();

            foreach (var group in groupedViews)
            {
                var sheet = CreateSheet(wb1, @group.Key, @group.Value);
                wb1.Add(sheet);
                sheetMeta[@group.Key] = (sheet, @group.Value.Count);
            }

            wb1.Add(CreateTaxSummarySheet(wb1, sheetMeta));

            wb1.Write(file);
        }

		public void PrintAsPlainText(IList<IView> views)
		{
			var groupedViews = GroupViews(views);

			foreach (var group in groupedViews)
			{
				Console.WriteLine();
				Console.WriteLine(group.Key);
				Console.WriteLine(String.Join("\r\n", group.Value));
			}
		}

		private ISheet CreateSheet(XSSFWorkbook workbook, string sheetName, IList<IView> objects)
		{
			var sheet = workbook.CreateSheet(sheetName);

			var headerProperties = CollectPublicProperties(objects[0]);
			var haeders = headerProperties.Select( i => DescriptionAttribute.ConstructDescription(i.Key, objects[0])).ToList();
			CreateRow(sheet, 0, haeders);

			for (int rowIndex = 1; rowIndex <= objects.Count; rowIndex++)
			{
				var properties = CollectPublicProperties(objects[rowIndex - 1]);
				CreateRow(sheet, rowIndex, properties.Values.ToList());
			}

			return sheet;
		}

		private IRow CreateRow(ISheet sheet, int rowIndex, IList<string> rowValues)
		{
			var row = sheet.CreateRow(rowIndex);

			var columnIndex = 0;
			foreach (var rowValue in rowValues)
			{
				var cell = row.CreateCell(columnIndex);

				// In case it's a number lets store it as a number
				if (Decimal.TryParse(rowValue, out var value))
				{
					cell.SetCellValue((double)value);
				}
				//For dates using the sortable format so excel can treat it better or at least sort as a string
				else if(DateTime.TryParse(rowValue, out var dateValue))
				{
					cell.SetCellValue(dateValue.ToString("s"));
				}
				else
				{
					cell.SetCellValue(rowValue);
				}

				columnIndex++;
			}

			return row;
		}

		private ISheet CreateTaxSummarySheet(XSSFWorkbook workbook, Dictionary<string, (ISheet Sheet, int DataRowCount)> sheetMeta)
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
					var dailyCell = row.CreateCell(1);
					dailyCell.SetCellValue(0);
					dailyCell.CellStyle = czkStyle;
					var yearlyCell = row.CreateCell(2);
					yearlyCell.SetCellValue(0);
					yearlyCell.CellStyle = czkStyle;
				}

				summaryRowIndex++;
			}

			// Total row (SUM of the 4 section rows above)
			var totalRow = summarySheet.CreateRow(summaryRowIndex);
			totalRow.CreateCell(0).SetCellValue("Total");
			var totalDailyCell = totalRow.CreateCell(1);
			totalDailyCell.SetCellFormula("SUM(B2:B5)");
			totalDailyCell.CellStyle = czkStyle;
			var totalYearlyCell = totalRow.CreateCell(2);
			totalYearlyCell.SetCellFormula("SUM(C2:C5)");
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
				var dailyCell = taxRow.CreateCell(1);
				dailyCell.SetCellValue(0);
				dailyCell.CellStyle = czkStyle;
				var yearlyCell = taxRow.CreateCell(2);
				yearlyCell.SetCellValue(0);
				yearlyCell.CellStyle = czkStyle;
			}

			// Auto-size columns
			for (int col = 0; col <= 2; col++)
			{
				summarySheet.AutoSizeColumn(col);
			}

			return summarySheet;
		}

		private void WriteSumFormulaCell(IRow row, int cellIndex, ISheet sourceSheet, int dataRowCount, string headerPrefix, string headerSuffix, ICellStyle style)
		{
			var colIndex = FindColumnIndex(sourceSheet, headerPrefix, headerSuffix);
			var cell = row.CreateCell(cellIndex);
			cell.CellStyle = style;
			if (colIndex >= 0)
			{
				var colLetter = GetExcelColumnLetter(colIndex);
				var formula = $"SUM({sourceSheet.SheetName}!{colLetter}2:{colLetter}{dataRowCount + 1})";
				cell.SetCellFormula(formula);
			}
			else
			{
				cell.SetCellValue(0);
			}
		}

		private static int FindColumnIndex(ISheet sheet, string startsWith, string endsWith)
		{
			var headerRow = sheet.GetRow(0);
			if (headerRow == null) return -1;

			for (int i = 0; i < headerRow.LastCellNum; i++)
			{
				var cell = headerRow.GetCell(i);
				if (cell == null) continue;

				var value = cell.StringCellValue;
				if (value != null && value.StartsWith(startsWith) && value.EndsWith(endsWith))
				{
					return i;
				}
			}

			return -1;
		}

		private static string GetExcelColumnLetter(int columnIndex)
		{
			var result = "";
			while (columnIndex >= 0)
			{
				result = (char)('A' + columnIndex % 26) + result;
				columnIndex = columnIndex / 26 - 1;
			}
			return result;
		}

		private IDictionary<PropertyInfo, string> CollectPublicProperties(Object instance)
		{
			var properties = instance.GetType().GetProperties();

			var dictionary = properties.Reverse().ToDictionary(
				k => k,
				i => i.GetValue(instance));

			var output = new Dictionary<PropertyInfo, string>();
			foreach (var pair in dictionary)
			{
				if (pair.Value is null)
				{
					output.Add(pair.Key, null);
				}
				else if (pair.Value is IFormattable || pair.Value is string)
				{
					output.Add(pair.Key, pair.Value.ToString());
				}
				else
				{
					foreach (var prop in CollectPublicProperties(pair.Value))
					{
						output.Add(prop.Key, prop.Value);
					}
				}
			}

			return output;
		}
	}
}