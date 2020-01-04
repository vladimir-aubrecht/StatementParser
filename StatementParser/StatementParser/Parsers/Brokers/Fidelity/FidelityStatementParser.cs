using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StatementParser.Models;
using StatementParser.Parsers.Brokers.Fidelity.PdfModels;
using StatementParser.Parsers.Pdf;
using PigPdfDocument = UglyToad.PdfPig.PdfDocument;

namespace StatementParser.Parsers.Brokers.Fidelity
{
    internal class FidelityStatementParser : ITransactionParser
    {
        public bool CanParse(string statementFilePath)
        {
            if (!File.Exists(statementFilePath) || Path.GetExtension(statementFilePath).ToLowerInvariant() != ".pdf")
            {
                return false;
            }

            return true;
        }

        public IList<Transaction> Parse(string statementFilePath)
        {
            var transactions = new List<Transaction>();

            using (var document = PigPdfDocument.Open(statementFilePath))
            {
                var year = ParseYear(document);

                transactions.AddRange(ParseTransactions<ActivityOtherModel>(document, i => CreateOtherTransaction(i.Value, year)));
                transactions.AddRange(ParseTransactions<ActivityDividendModel>(document, i => CreateDividendTransaction(document, i.Value, year)));
                transactions.AddRange(ParseTransactions<ESPPModel>(document, i => CreateESPPTransaction(document, i.Value)));
            }

            return transactions;
        }

        public string SearchForCompanyName(PigPdfDocument document, decimal amount, decimal price)
        {
            var transactionStrings = new Pdf.PdfDocument(document).ParseTable<ActivityBuyModel>();

            var foundTransactions = transactionStrings.Where(i => i.Value.Price == price && i.Value.Amount == amount);

            if (foundTransactions.Count() > 1)
            {
                // Probability that somebody will get ESPP for 2 companies of exact discounted price and exact amount on one report is close to 0, lets consider it's impossible.
                // I don't know better way how to find it based on artifacts from page about ESPP.

                throw new InvalidOperationException("Couldn't properly detect name of company for ESPP.");
            }

            return foundTransactions.First().Value.Name;
        }

        private decimal SearchForTaxString(PigPdfDocument document, DateTime date)
        {
            var row = new Pdf.PdfDocument(document).ParseTable<ActivityTaxesModel>().Where(i => i.Value.Date == date.ToString("MM/dd")).FirstOrDefault();
            return row?.Value.Tax ?? 0;
        }

        private IEnumerable<Transaction> ParseTransactions<TModel>(PigPdfDocument document, Func<PdfTableRow<TModel>, Transaction> createTransactionFunc) where TModel : new()
        {
            return new Pdf.PdfDocument(document).ParseTable<TModel>().Select(i => createTransactionFunc(i));
        }

        private int ParseYear(PigPdfDocument document)
        {
            return new Pdf.PdfDocument(document).ParseTable<StatementModel>().First().Value.Year;
        }

        private ESPPTransaction CreateESPPTransaction(PigPdfDocument document, ESPPModel esppRow)
        {
            var name = SearchForCompanyName(document, esppRow.Amount, esppRow.PurchasePrice);

            return new ESPPTransaction(Broker.Fidelity, esppRow.Date, name, Currency.USD, esppRow.PurchasePrice, esppRow.MarketPrice, esppRow.Amount);
        }

        private DividendTransaction CreateDividendTransaction(PigPdfDocument document, ActivityDividendModel activityDividendRow, int year)
        {
            var dateString = activityDividendRow.Date + "/" + year;
            var date = DateTime.Parse(dateString);
            var tax = SearchForTaxString(document, date);

            return new DividendTransaction(Broker.Fidelity, date, activityDividendRow.Name, activityDividendRow.Income, tax, Currency.USD);
        }

        private DepositTransaction CreateOtherTransaction(ActivityOtherModel activityOtherRow, int year)
        {
            var date = DateTime.Parse(activityOtherRow.Date + "/" + year);
            return new DepositTransaction(Broker.Fidelity, date, activityOtherRow.Name, activityOtherRow.Amount, activityOtherRow.Price, Currency.USD);
        }
    }
}
