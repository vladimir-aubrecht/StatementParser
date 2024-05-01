using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using StatementParser.Models;

namespace StatementParserCLI
{
	internal class Output
	{
		private Dictionary<string, List<Transaction>> GroupTransactions(IList<Transaction> transactions)
		{
			return transactions.GroupBy(i => i.GetType()).ToDictionary(k => k.Key.Name, i => i.Select(a => a).OrderBy(t => t.Date).ToList());
		}

		public void PrintAsJson(IList<Transaction> transactions)
		{
			var groupedTransactions = GroupTransactions(transactions);

			Console.WriteLine(JsonConvert.SerializeObject(groupedTransactions));
		}

		public void SaveAsExcelSheet(string filePath, IList<Transaction> transactions)
		{
			var groupedTransactions = GroupTransactions(transactions);

            using FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            var wb1 = new XSSFWorkbook();

            foreach (var group in groupedTransactions)
            {
                var sheet = wb1.CreateSheet(@group.Key);

                var headerRow = sheet.CreateRow(0);
                var headerProperties = GetPublicProperties(@group.Value[0]);
                SetRowValues(headerRow, headerProperties.Keys);

                for (int rowIndex = 1; rowIndex <= @group.Value.Count; rowIndex++)
                {
                    var row = sheet.CreateRow(rowIndex);
                    var properties = GetPublicProperties(@group.Value[rowIndex - 1]);

                    SetRowValues(row, properties.Values);
                }
                wb1.Add(sheet);
            }

            wb1.Write(file);
        }

		public void PrintAsPlainText(IList<Transaction> transactions)
		{
			var groupedTransactions = GroupTransactions(transactions);

			foreach (var group in groupedTransactions)
			{
				Console.WriteLine();
				Console.WriteLine(group.Key);
				Console.WriteLine(String.Join("\r\n", group.Value));
			}
		}

		private void SetRowValues(IRow row, ICollection<string> rowValues)
		{
			var columnIndex = 0;
			foreach (var rowValue in rowValues)
			{
				var cell = row.CreateCell(columnIndex);

				if(DateTime.TryParse(rowValue, out var dateValue))
				{
					cell.SetCellValue(dateValue.ToString("s"));
				}
				else
				{
					cell.SetCellValue(rowValue);
				}

				columnIndex++;
			}
		}

		private IDictionary<string, string> GetPublicProperties(Transaction transaction)
		{
			var properties = transaction.GetType().GetProperties();
			return properties.Reverse().ToDictionary(k => k.Name, i => i.GetValue(transaction).ToString());
		}
	}
}
