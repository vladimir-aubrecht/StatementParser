using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using StatementParser.Models;

namespace StatementParser.Parsers.Brokers.MorganStanley
{
	internal class MorganStanleyStatementXlsParser : ITransactionParser
	{
		private const int HeaderRowOffset = 8;
		private const int FooterRowOffset = 6;
		private const string SheetName = "4_Stock Award Plan_Completed";
		private const string Signature = "Morgan Stanley Smith Barney LLC. Member SIPC.";

		private bool CanParse(string statementFilePath)
		{
			if (!File.Exists(statementFilePath) || Path.GetExtension(statementFilePath).ToLowerInvariant() != ".xls")
			{
				return false;
			}

			var sheet = GetSheet(statementFilePath);

			return sheet.GetRow(sheet.LastRowNum).Cells[0].StringCellValue == Signature;
		}

		public IList<Transaction> Parse(string statementFilePath)
		{
			if (!CanParse(statementFilePath))
			{
				return null;
			}

			var sheet = GetSheet(statementFilePath);

			var name = sheet.GetRow(3).Cells[1].StringCellValue;

			return GetTransactionRows(sheet).Select(i => ParseTransaction(sheet, ParseTransactionType(i), i, name)).Where(i => i != null).ToList();
		}

		private Transaction ParseTransaction(ISheet sheet, string type, IRow row, string name)
		{
			switch (type)
			{
				case "Share Deposit":
					return ParseDepositTransaction(row, name);

				case "Dividend Credit":
					var taxRow = SearchTaxRow(sheet, row);
					return ParseDividendTransaction(row, taxRow, name);

				case "IRS Withholding":
				case "Dividend Reinvested":
				case "Sale":
				case "Wire Transfer":
					// no usecase for now, lets ignore them.
					return null;
			}

			return null;
		}

		private DividendTransaction ParseDividendTransaction(IRow dividendRow, IRow taxRow, string name)
		{
			var date = ParseTransactionDate(dividendRow);
			var grossProceed = Convert.ToDecimal(dividendRow.Cells[5].NumericCellValue);
			var tax = Convert.ToDecimal(taxRow?.Cells[5].NumericCellValue);
			var income = grossProceed + tax;

			return new DividendTransaction(Broker.MorganStanley, date, name, income, tax, Currency.USD);
		}

		private DepositTransaction ParseDepositTransaction(IRow row, string name)
		{
			var date = ParseTransactionDate(row);
			var amount = Convert.ToDecimal(row.Cells[3].NumericCellValue);
			var price = Convert.ToDecimal(row.Cells[4].NumericCellValue);

			return new DepositTransaction(Broker.MorganStanley, date, name, amount, price, Currency.USD);
		}

		private IRow SearchTaxRow(ISheet sheet, IRow creditRow)
		{
			var creditRowDate = ParseTransactionDate(creditRow);

			return GetTransactionRows(sheet).FirstOrDefault(i => ParseTransactionType(i) == "IRS Withholding" && ParseTransactionDate(i) == creditRowDate);
		}

		private ISheet GetSheet(string statementFilePath)
		{
			HSSFWorkbook workbook;
			using (FileStream stream = new FileStream(statementFilePath, FileMode.Open, FileAccess.Read))
			{
				workbook = new HSSFWorkbook(stream);
			}

			return workbook.GetSheet(SheetName);
		}

		private IList<IRow> GetTransactionRows(ISheet sheet)
		{
			var output = new List<IRow>();

			for (int i = HeaderRowOffset; i <= sheet.LastRowNum - FooterRowOffset; i++)
			{
				var row = sheet.GetRow(i);
				output.Add(row);
			}

			return output;
		}

		private DateTime ParseTransactionDate(IRow row)
		{
			return DateTime.Parse(row.Cells[0].StringCellValue);
		}

		private string ParseTransactionType(IRow row)
		{
			return row.Cells[1].StringCellValue;
		}
	}
}
