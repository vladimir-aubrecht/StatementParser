using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StatementParser.Models;
using StatementParser.Parsers.Brokers.Fidelity.PdfRows;
using StatementParser.Parsers.Pdf;
using PigPdfDocument = UglyToad.PdfPig.PdfDocument;

namespace StatementParser.Parsers.Brokers.Fidelity
{
    internal class FidelityStatementParser : ITransactionParser
    {
        private readonly IPdfConfiguration pdfConfiguration;

        public FidelityStatementParser(IPdfConfiguration pdfConfiguration)
        {
            this.pdfConfiguration = pdfConfiguration ?? throw new ArgumentNullException(nameof(pdfConfiguration));
        }

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

                transactions.AddRange(ParseOtherRow(document, year));
                transactions.AddRange(ParseDividendRow(document, year));
                transactions.AddRange(ParseDiscountedBuyRow(document));
            }

            return transactions;
        }

        public string SearchForCompanyName(PigPdfDocument document, decimal amount, decimal price)
        {
            var transactionStrings = new Pdf.PdfDocument(document, pdfConfiguration).ParseTable<ActivityBuyRow>(PdfTableName.ActivityBuy);

            var foundTransactions = transactionStrings.Where(i => i.Value.Price == price && i.Value.Amount == amount);

            if (foundTransactions.Count() > 1)
            {
                // Probability that somebody will get ESPP for 2 companies of exact discounted price and exact amount on one report is close to 0, lets consider it's impossible.
                // I don't know better way how to find it based on artifacts from page about ESPP.

                throw new Exception("Couldn't properly detect name of company for ESPP.");
            }

            var transaction = foundTransactions.First();

            return transaction.Value.Name;
        }

        private decimal SearchForTaxString(PigPdfDocument document, DateTime date)
        {
            var transactionStrings = new Pdf.PdfDocument(document, this.pdfConfiguration).ParseTable<ActivityTaxesRow>(PdfTableName.ActivityTaxes);

            foreach (var transaction in transactionStrings)
            {
                if (transaction.Value.Date == date.ToString("MM/dd"))
                {
                    return transaction.Value.Tax;
                }
            }

            return 0;
        }

        private IList<Transaction> ParseDiscountedBuyRow(PigPdfDocument document)
        {
            var output = new List<Transaction>();
            var transactionStrings = new Pdf.PdfDocument(document, this.pdfConfiguration).ParseTable<DiscountedBuyRow>(PdfTableName.SummaryESPP);

            foreach (var transactionString in transactionStrings)
            {
                output.Add(CreateDiscountBuyTransaction(document, transactionString.Value));
            }

            return output;
        }

        private IList<Transaction> ParseDividendRow(PigPdfDocument document, int year)
        {
            var output = new List<Transaction>();
            var transactionStrings = new Pdf.PdfDocument(document, this.pdfConfiguration).ParseTable<ActivityDividendRow>(PdfTableName.ActivityDividend);

            foreach (var transactionString in transactionStrings)
            {
                output.Add(CreateDividendTransaction(document, transactionString.Value, year));
            }

            return output;
        }

        private IList<Transaction> ParseOtherRow(PigPdfDocument document, int year)
        {
            var output = new List<Transaction>();
            var transactionStrings = new Pdf.PdfDocument(document, this.pdfConfiguration).ParseTable<ActivityOtherRow>(PdfTableName.ActivityOther);

            foreach (var transactionString in transactionStrings)
            {
                output.Add(CreateOtherTransaction(transactionString.Value, year));
            }

            return output;
        }

        private int ParseYear(PigPdfDocument document)
        {
            var page = document.GetPage(1);
            var stockPlanServicesReport = "STOCK PLAN SERVICES REPORT";
            var envelope = "Envelope";

            var startIndex = page.Text.IndexOf(stockPlanServicesReport, StringComparison.Ordinal) + stockPlanServicesReport.Length;
            var endIndex = page.Text.IndexOf(envelope, StringComparison.Ordinal);

            var reportPeriod = page.Text.Substring(startIndex, endIndex - startIndex);

            var parts = reportPeriod.Split(' ');

            return Convert.ToInt32(parts[parts.Length - 1]);
        }

        private DiscountBuyTransaction CreateDiscountBuyTransaction(PigPdfDocument document, DiscountedBuyRow discountedBuyRow)
        {
            var name = SearchForCompanyName(document, discountedBuyRow.Amount, discountedBuyRow.PurchasePrice);

            return new DiscountBuyTransaction(Broker.Fidelity, discountedBuyRow.Date, name, Currency.USD, discountedBuyRow.PurchasePrice, discountedBuyRow.MarketPrice, discountedBuyRow.Amount);
        }

        private DividendTransaction CreateDividendTransaction(PigPdfDocument document, ActivityDividendRow activityDividendRow, int year)
        {
            var dateString = activityDividendRow.Date + "/" + year;
            var date = DateTime.Parse(dateString);
            var tax = SearchForTaxString(document, date);

            return new DividendTransaction(Broker.Fidelity, date, activityDividendRow.Name, activityDividendRow.Income, tax, Currency.USD);
        }

        private DepositTransaction CreateOtherTransaction(ActivityOtherRow activityOtherRow, int year)
        {
            var date = DateTime.Parse(activityOtherRow.Date + "/" + year);
            return new DepositTransaction(Broker.Fidelity, date, activityOtherRow.Name, activityOtherRow.Amount, activityOtherRow.Price, Currency.USD);
        }
    }
}
