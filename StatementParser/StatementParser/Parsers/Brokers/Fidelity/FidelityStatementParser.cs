using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StatementParser.Models;
using StatementParser.Parsers.Brokers.Fidelity.PdfModels;
using StatementParser.Parsers.Pdf;
using StatementParser.Parsers.Pdf.Exceptions;
using PigPdfDocument = UglyToad.PdfPig.PdfDocument;

namespace StatementParser.Parsers.Brokers.Fidelity
{
    internal class FidelityStatementParser : ITransactionParser
    {
        private bool CanParse(string statementFilePath)
        {
            if (!File.Exists(statementFilePath) || Path.GetExtension(statementFilePath).ToLowerInvariant() != ".pdf")
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

            var transactions = new List<Transaction>();

            using (var document = PigPdfDocument.Open(statementFilePath))
            {
                try
                {
                    var parsedDocument = new Pdf.PdfDocumentParser<StatementModel>().Parse(new PdfSource(document));

                    transactions.AddRange(parsedDocument.ActivityOther.Select(i => CreateOtherTransaction(i, parsedDocument.Year)));
                    transactions.AddRange(parsedDocument.ActivityDividend.Select(i => CreateDividendTransaction(parsedDocument.ActivityTaxes, i, parsedDocument.Year)));
                    transactions.AddRange(parsedDocument.ESPP.Select(i => CreateESPPTransaction(parsedDocument.ActivityBuy, i)));
                }
                catch (PdfException)
                {
                    return null;
                }
            }

            return transactions;
        }

        public string SearchForCompanyName(ActivityBuyModel[] activityBuyModels, ESPPModel esppRow)
        {
            string removeLastCharFunc(decimal number)
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
            return activityTaxesModels.Where(i => i.Date == date.ToString("MM/dd")).FirstOrDefault()?.Tax ?? 0;
        }

        private ESPPTransaction CreateESPPTransaction(ActivityBuyModel[] activityBuyModels, ESPPModel esppRow)
        {
            var name = SearchForCompanyName(activityBuyModels, esppRow);

            return new ESPPTransaction(Broker.Fidelity, esppRow.Date, name, Currency.USD, esppRow.PurchasePrice, esppRow.MarketPrice, esppRow.Amount);
        }

        private DividendTransaction CreateDividendTransaction(ActivityTaxesModel[] activityTaxesModels, ActivityDividendModel activityDividendRow, int year)
        {
            var dateString = activityDividendRow.Date + "/" + year;
            var date = DateTime.Parse(dateString);
            var tax = SearchForTaxString(activityTaxesModels, date);

            return new DividendTransaction(Broker.Fidelity, date, activityDividendRow.Name, activityDividendRow.Income, tax, Currency.USD);
        }

        private DepositTransaction CreateOtherTransaction(ActivityOtherModel activityOtherRow, int year)
        {
            var date = DateTime.Parse(activityOtherRow.Date + "/" + year);
            return new DepositTransaction(Broker.Fidelity, date, activityOtherRow.Name, activityOtherRow.Amount, activityOtherRow.Price, Currency.USD);
        }
    }
}
