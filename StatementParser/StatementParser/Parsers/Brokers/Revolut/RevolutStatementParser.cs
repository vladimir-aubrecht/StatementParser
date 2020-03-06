using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASoft.TextDeserializer;
using ASoft.TextDeserializer.Exceptions;
using StatementParser.Models;
using StatementParser.Parsers.Brokers.Revolut.PdfModels;

namespace StatementParser.Parsers.Brokers.Revolut
{
	internal class RevolutStatementParser : ITransactionParser
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

			var transactions = new List<Transaction>();

			using (var textSource = new TextSource(statementFilePath))
			{
				try
				{
					var parsedDocument = new TextDocumentParser<StatementModel>().Parse(textSource);

                    foreach (var transaction in parsedDocument.ActivityDividend)
                    {
                        if (transaction.ActivityType == "DIV")
                        {
                            transactions.Add(CreateDividendTransaction(transaction, parsedDocument.ActivityDividend));
                        }
                    }

				}
				catch (TextException)
				{
					return null;
				}
			}

			return transactions;
		}

        private DividendTransaction CreateDividendTransaction(ActivityDividendModel activityDividendRow, ActivityDividendModel[] activities)
		{
            decimal tax = SearchForTax(activityDividendRow, activities);
            var currency = (Currency)Enum.Parse(typeof(Currency), activityDividendRow.Currency);
			return new DividendTransaction(Broker.Revolut, activityDividendRow.SettleDate, activityDividendRow.Description, activityDividendRow.Amount, tax, currency);
		}

        private decimal SearchForTax(ActivityDividendModel dividendTransactionRow, ActivityDividendModel[] activities)
        {
            var transactionRow = activities.FirstOrDefault(i => 
                i.ActivityType == "DIVNRA" &&
                i.TradeDate == dividendTransactionRow.TradeDate &&
                i.SettleDate == dividendTransactionRow.SettleDate &&
                i.Description.Substring(0, i.Description.IndexOf(" - DIV")) == dividendTransactionRow.Description.Substring(0, dividendTransactionRow.Description.IndexOf(" - DIV"))
            );

            return transactionRow?.Amount ?? 0;
        }
	}
}
