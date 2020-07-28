using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using ASoft.TextDeserializer;
using ASoft.TextDeserializer.Exceptions;

using Microsoft.Extensions.Logging;

using StatementParser.Models;
using StatementParser.Parsers.Brokers.Fidelity.PdfModels;

namespace StatementParser.Parsers.Brokers.Fidelity
{
    internal class FidelityStatementParser : ITransactionParser
    {
        private readonly ILogger<FidelityStatementParser> logger;

        public FidelityStatementParser(ILogger<FidelityStatementParser> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

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

                    transactions.AddRange(parsedDocument.ActivityOther.Select(i => CreateOtherTransaction(i, parsedDocument.Year)));
                    transactions.AddRange(parsedDocument.ActivityDividend.Select(i => CreateDividendTransaction(parsedDocument.ActivityTaxes, i, parsedDocument.Year)));
                    transactions.AddRange(parsedDocument.ESPP.Select(i => CreateESPPTransaction(parsedDocument.ActivityBuy, i)));
                }
                catch (TextException)
                {
                    return null;
                }
            }

            return transactions;
        }

        public string SearchForCompanyName(ActivityBuyModel[] activityBuyModels, ESPPModel esppRow)
        {
            static string removeLastCharFunc(decimal number)
            {
                return number.ToString().Remove(number.ToString().Length - 1);
            }

            // Fidelity has a bug in generation and amount can be rounded up in some tables on third number after digit.
            var foundTransactions = activityBuyModels.Where(i => i.Price == esppRow.PurchasePrice && removeLastCharFunc(i.Amount) == removeLastCharFunc(esppRow.Amount));

            if (foundTransactions.Count() > 1)
            {
                // Probability that somebody will get ESPP for 2 companies of exact discounted price and exact amount on one report is close to 0, lets consider it's impossible.
                // I don't know better way how to find it based on artifacts from page about ESPP.

                throw new InvalidOperationException("Couldn't properly detect name of company for ESPP.");
            }

            return foundTransactions.First().Name;
        }

        private decimal SearchForTaxString(ActivityTaxesModel[] activityTaxesModels, DateTime date)
        {
            return activityTaxesModels.FirstOrDefault(i => i.Date == date.ToString("MM/dd", CultureInfo.InvariantCulture))?.Tax ?? 0;
        }

        private ESPPTransaction CreateESPPTransaction(ActivityBuyModel[] activityBuyModels, ESPPModel esppRow)
        {
            var name = SearchForCompanyName(activityBuyModels, esppRow);

            return new ESPPTransaction(Broker.Fidelity, esppRow.Date, name, Currency.USD, esppRow.PurchasePrice, esppRow.MarketPrice, esppRow.Amount);
        }

        private DividendTransaction CreateDividendTransaction(ActivityTaxesModel[] activityTaxesModels, ActivityDividendModel activityDividendRow, int year)
        {
            var date = CreateDateTime(activityDividendRow.Date, year);
            var tax = SearchForTaxString(activityTaxesModels, date);

            return new DividendTransaction(Broker.Fidelity, date, activityDividendRow.Name, activityDividendRow.Income, tax, Currency.USD);
        }

        private DepositTransaction CreateOtherTransaction(ActivityOtherModel activityOtherRow, int year)
        {
            var date = CreateDateTime(activityOtherRow.Date, year);
            return new DepositTransaction(Broker.Fidelity, date, activityOtherRow.Name, activityOtherRow.Amount, activityOtherRow.Price, Currency.USD);
        }

        private DateTime CreateDateTime(string dayAndYear, int year)
        {
            var dateString = dayAndYear + "/" + year;
            return DateTime.ParseExact(dateString, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
