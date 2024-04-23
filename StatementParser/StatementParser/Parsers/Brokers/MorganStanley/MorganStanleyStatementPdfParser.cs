using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ASoft.TextDeserializer;
using ASoft.TextDeserializer.Exceptions;
using StatementParser.Models;
using StatementParser.Parsers.Brokers.MorganStanley.PdfModels;

namespace StatementParser.Parsers.Brokers.MorganStanley
{
	internal class MorganStanleyStatementPdfParser : ITransactionParser
	{
		private bool CanParse(string statementFilePath)
		{
			return File.Exists(statementFilePath) && Path.GetExtension(statementFilePath).ToLowerInvariant() == ".pdf";
		}

		public IList<Transaction> Parse(string statementFilePath)
		{
			if (!CanParse(statementFilePath))
			{
				return null;
			}

			using var textSource = new TextSource(statementFilePath, true);
			try
			{
				if (Regex.IsMatch(textSource.Title ?? "", @"^Morgan Stanley Smith Barney Document SP10 History Statements "))
				{
					return ParseLegacyStatement(textSource);
				}
				else if (Regex.IsMatch(textSource.Title ?? "", @"^Morgan Stanley Smith Barney Document EPS217CCC linux-TTF New$"))
				{
					return Parse2022Statement(textSource);
				}
				else
				{
					return null;
				}
			}
			catch (TextException)
			{
				return null;
			}
		}

		private decimal SearchForTax(StatementModel statementModel, TransactionModel transactionModel)
		{
			return statementModel.Transactions
				.Where(i => i.Type == "Withholding Tax" && i.Date == transactionModel.Date)
				.Select(i => i.NetAmount).FirstOrDefault();
		}

		private IList<Transaction> ParseLegacyStatement(TextSource textSource)
		{
			var statementModel = new TextDocumentParser<StatementModel>().Parse(textSource);

			var output = new List<Transaction>();

			output.AddRange(statementModel.Transactions
				.Where(i => i.Type == "Share Deposit")
				.Select(i => new DepositTransaction(Broker.MorganStanley, i.Date, statementModel.Name, i.Quantity, i.Price, Currency.USD)));

			output.AddRange(statementModel.Transactions
				.Where(i => i.Type == "Dividend Credit")
				.Select(i => new DividendTransaction(Broker.MorganStanley, i.Date, statementModel.Name, i.GrossAmount, SearchForTax(statementModel, i), Currency.USD)));

			return output;
		}

		private decimal SearchForTax(StatementModel2022 statementModel, TransactionModel2022 transactionModel)
		{
			return statementModel.Transactions
				.Where(i => i.Type == "Withholding Tax" && i.Date == transactionModel.Date)
				.Select(i => i.TotalNetAmount).FirstOrDefault();
		}

		private IList<Transaction> Parse2022Statement(TextSource textSource)
		{
			var statementModel = new TextDocumentParser<StatementModel2022>().Parse(textSource);

			var output = new List<Transaction>();

			output.AddRange(statementModel.Transactions
				.Where(i => i.Type == "Share Deposit")
				.Select(i => new DepositTransaction(Broker.MorganStanley, i.Date, statementModel.Name, i.Quantity, i.Price, Currency.USD)));

			output.AddRange(statementModel.Transactions
				.Where(i => i.Type == "Dividend Credit")
				.Select(i => new DividendTransaction(Broker.MorganStanley, i.Date, statementModel.Name, i.GrossAmount, SearchForTax(statementModel, i), Currency.USD)));

			return output;
		}
	}
}
