using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using StatementParser.Models;

namespace TaxReporterCLI
{
    public class Output
    {
        private Dictionary<string, List<EnrichedTransaction>> GroupTransactions(IList<EnrichedTransaction> transactions)
        {
            return transactions.GroupBy(i => i.Transaction.GetType()).ToDictionary(k => k.Key.Name, i => i.Select(a => a).ToList());
        }

        public void PrintAsJson(IList<EnrichedTransaction> transactions)
        {
            var groupedTransactions = GroupTransactions(transactions);

            Console.WriteLine(JsonConvert.SerializeObject(groupedTransactions));
        }

        public void SaveAsExcelSheet(string filePath, IList<EnrichedTransaction> transactions)
        {
            var groupedTransactions = GroupTransactions(transactions);

            using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                var wb1 = new XSSFWorkbook();

                foreach (var group in groupedTransactions)
                {
                    var sheet = wb1.CreateSheet(group.Key);

                    var headerProperties = GetPublicProperties(group.Value[0].Transaction);
                    headerProperties.Add(nameof(EnrichedTransaction.ExchangeRatePerDay), "");
                    headerProperties.Add(nameof(EnrichedTransaction.ExchangeRatePerYear), "");
                    
                    var headerRow = sheet.CreateRow(0);
                    SetRowValues(headerRow, headerProperties.Keys);

                    for (int rowIndex = 1; rowIndex < group.Value.Count() + 1; rowIndex++)
                    {
                        var row = sheet.CreateRow(rowIndex);
                        var properties = GetPublicProperties(group.Value[rowIndex - 1].Transaction);
                        properties.Add(nameof(EnrichedTransaction.ExchangeRatePerDay), group.Value[rowIndex - 1].ExchangeRatePerDay.ToString());
                        properties.Add(nameof(EnrichedTransaction.ExchangeRatePerYear), group.Value[rowIndex - 1].ExchangeRatePerYear.ToString());

                        SetRowValues(row, properties.Values);
                    }
                    wb1.Add(sheet);
                }

                wb1.Write(file);
            }
        }

        public void PrintAsPlainText(IList<EnrichedTransaction> transactions)
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
        }

        private IDictionary<string, string> GetPublicProperties(Transaction transaction)
        {
            var properties = transaction.GetType().GetProperties();
            return properties.Reverse().ToDictionary(k => k.Name, i => i.GetValue(transaction).ToString());
        }
    }
}
