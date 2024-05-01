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

            foreach (var group in groupedViews)
            {
                wb1.Add(CreateSheet(wb1, @group.Key, @group.Value));
            }

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