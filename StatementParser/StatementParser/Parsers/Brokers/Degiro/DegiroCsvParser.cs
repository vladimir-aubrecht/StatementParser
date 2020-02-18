using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using StatementParser.Models;
using StatementParser.Parsers.Brokers.Degiro.CsvModels;

namespace StatementParser.Parsers.Brokers.Degiro
{
	internal class DegiroParser : ITransactionParser
	{
		private bool CanParse(string statementFilePath)
		{
			if (!File.Exists(statementFilePath) || Path.GetExtension(statementFilePath).ToLowerInvariant() != ".csv")
			{
				return false;
			}

			return true;
		}

		public IList<Transaction> Parse(string statementFilePath)
		{
			if (!CanParse(statementFilePath))
			{
				return null;
			}

			var statementRows = LoadStatementModel(statementFilePath);

			if (statementRows == null)
			{
				return null;
			}

			var output = new List<Transaction>();
			foreach (var row in statementRows)
			{
				if (row.Description.Contains("Dividenda"))
				{
					var tax = SearchForTax(statementRows, row.ISIN, row.Date.Value);
					var transaction = new DividendTransaction(Broker.Degiro, row.Date.Value, row.Name, row.Income.Value, tax, row.Currency.Value);
					output.Add(transaction);
				}
			}

			return output;
		}

		private decimal SearchForTax(IList<StatementRowModel> statementRows, string isin, DateTime date)
		{
			var row = statementRows.Where(i => i.Date.Value == date && i.ISIN == isin && i.Description.Contains("z dividendy")).FirstOrDefault();
			return row.Income.Value;
		}

		private IList<StatementRowModel> LoadStatementModel(string statementFilePath)
		{
			try
			{
				var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
				ci.DateTimeFormat = new DateTimeFormatInfo()
				{
					ShortDatePattern = "dd-MM-yyyy"
				};

				using (var reader = new StreamReader(statementFilePath))
				using (var csv = new CsvReader(reader, ci))
				{
					return csv.GetRecords<StatementRowModel>().Where(i => i.Date != null).ToList();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}
	}
}
