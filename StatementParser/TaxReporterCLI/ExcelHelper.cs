using NPOI.SS.UserModel;

namespace TaxReporterCLI
{
	internal static class ExcelHelper
	{
		public static string GetExcelColumnLetter(int columnIndex)
		{
			var result = "";
			while (columnIndex >= 0)
			{
				result = (char)('A' + columnIndex % 26) + result;
				columnIndex = columnIndex / 26 - 1;
			}
			return result;
		}

		public static int FindColumnIndex(ISheet sheet, string startsWith, string endsWith)
		{
			var headerRow = sheet.GetRow(0);
			if (headerRow == null) return -1;

			for (int i = 0; i < headerRow.LastCellNum; i++)
			{
				var cell = headerRow.GetCell(i);
				if (cell == null || cell.CellType != CellType.String) continue;

				var value = cell.StringCellValue;
				if (value != null && value.StartsWith(startsWith) && value.EndsWith(endsWith))
				{
					return i;
				}
			}

			return -1;
		}
	}
}
