using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using StatementParser.Models;
using TaxReporterCLI.Models;

namespace TaxReporterCLI
{
	internal class Output
	{
		private Dictionary<string, List<object>> GroupTransactions(IList<object> transactions)
		{
			return transactions.GroupBy(i => i.GetType()).ToDictionary(k => k.Key.Name, i => i.Select(a => a).ToList());
		}

		public void PrintAsJson(IList<object> transactions)
		{
			var groupedTransactions = GroupTransactions(transactions);

			Console.WriteLine(JsonConvert.SerializeObject(groupedTransactions));
		}

		public void SaveAsExcelSheet(string filePath, IList<object> transactions)
		{
			var groupedTransactions = GroupTransactions(transactions);

			using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
			{
				var wb1 = new XSSFWorkbook();

				foreach (var group in groupedTransactions)
				{
					wb1.Add(CreateSheet(wb1, group.Key, group.Value));
				}

				wb1.Write(file);
			}
		}

		public void PrintAsPlainText(IList<object> transactions)
		{
			var groupedTransactions = GroupTransactions(transactions);

			foreach (var group in groupedTransactions)
			{
				Console.WriteLine();
				Console.WriteLine(group.Key);
				Console.WriteLine(String.Join("\r\n", group.Value));
			}
		}

		private ISheet CreateSheet(XSSFWorkbook workbook, string sheetName, IList<object> objects)
		{
			var sheet = workbook.CreateSheet(sheetName);

			var headerProperties = CollectPublicProperties(objects[0]);
			CreateRow(sheet, 0, headerProperties.Keys);

			for (int rowIndex = 1; rowIndex < objects.Count() + 1; rowIndex++)
			{
				var properties = CollectPublicProperties(objects[rowIndex - 1]);
				CreateRow(sheet, rowIndex, properties.Values);
			}

			return sheet;
		}

		private IRow CreateRow(ISheet sheet, int rowIndex, ICollection<string> rowValues)
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
				else
				{
					cell.SetCellValue(rowValue);
				}

				columnIndex++;
			}

			return row;
		}

		private IDictionary<string, string> CollectPublicProperties(Object instance)
		{
			var properties = instance.GetType().GetProperties();
			var dictionary = properties.Reverse().ToDictionary(k => k.Name, i => i.GetValue(instance));

			var output = new Dictionary<string, string>();
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
