using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NPOI.SS.UserModel;
using StatementParser.Models;
using StatementParser.Parsers.Pdf;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using PigPdfDocument = UglyToad.PdfPig.PdfDocument;

namespace StatementParser.Parsers.Brokers.Fidelity
{
    internal class FidelityStatementParser : ITransactionParser
    {
        private static readonly Regex employeePurchaseSanitizeRegex = new Regex("[0-9]{2}/[0-9]{2}/[0-9]{4}-[0-9]{2}/[0-9]{2}/[0-9]{4}Employee Purchase", RegexOptions.Compiled);
        private static readonly Regex dividendTransactionRegex = new Regex("([0-9]{2}/[0-9]{2}) (.+?)  ?[0-9]+ Dividend Received  --\\$?([0-9]+\\.[0-9]{2})", RegexOptions.Compiled);
        private static readonly Regex discountedBuyRegex = new Regex("([0-9]{2}/[0-9]{2}/[0-9]{4})\\$([0-9]+\\.[0-9]{5})\\$([0-9]+\\.[0-9]{3})([0-9]+\\.[0-9]{3})", RegexOptions.Compiled);
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

                transactions.AddRange(
                    ParseTransactions(document, year, PdfTableName.ActivityOther, (doc, ts, year) => ParseDepositTransaction(doc, ts, year)));

                transactions.AddRange(
                    ParseTransactions(document, year, PdfTableName.ActivityDividend, (doc, ts, year) => ParseDividendTransaction(doc, ts, year)));

                transactions.AddRange(
                    ParseTransactions(document, year, PdfTableName.SummaryESPP, (doc, ts, year) => ParseDiscountedBuyTransaction(doc, ts, year)));
            }

            return transactions;
        }

        public string SearchForCompanyName(PigPdfDocument document, string amount, string price)
        {
            var transactionStrings = new Pdf.PdfDocument(document, pdfConfiguration)[PdfTableName.ActivityBuy];

            var foundTransactions = transactionStrings.Where(i => i.Value.Contains($" You Bought {amount}${price}") && i.Value.Contains("ESPP"));

            if (foundTransactions.Count() > 1)
            {
                // Probability that somebody will get ESPP for 2 companies of exact discounted price and exact amount on one report is close to 0, lets consider it's impossible.
                // I don't know better way how to find it based on artifacts from page about ESPP.

                throw new Exception("Couldn't properly detect name of company for ESPP.");
            }

            var transaction = foundTransactions.First();

            return transaction.Value.Substring(6, transaction.Value.IndexOf("ESPP", StringComparison.Ordinal) - 7);
        }

        private string SearchForTaxString(PigPdfDocument document, DateTime date)
        {
            var transactionStrings = new Pdf.PdfDocument(document, this.pdfConfiguration)[PdfTableName.ActivityTaxes];

            foreach (var transaction in transactionStrings)
            {
                if (transaction.Value.StartsWith(date.ToString("MM/dd"), StringComparison.Ordinal))
                {
                    return transaction.Value;
                }
            }

            return null;
        }

        private IList<Transaction> ParseTransactions(PigPdfDocument document, int year, PdfTableName tableName, Func<PigPdfDocument, string, int, Transaction> parseFunc)
        {
            var output = new List<Transaction>();

            var transactionStrings = new Pdf.PdfDocument(document, this.pdfConfiguration)[tableName];

            foreach (var transactionString in transactionStrings)
            {
                var result = parseFunc(document, transactionString.Value, year);

                if (result != null)
                {
                    output.Add(result);
                }
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

        private DiscountBuyTransaction ParseDiscountedBuyTransaction(PigPdfDocument document, string transactionString, int year)
        {
            //06/28/2019$120.56000$133.96020.631$276.46

            if (employeePurchaseSanitizeRegex.IsMatch(transactionString))
            {
                transactionString = employeePurchaseSanitizeRegex.Replace(transactionString, "");
            }

            var match = discountedBuyRegex.Match(transactionString);

            var dateString = match.Groups[1].Value;
            var purchasePriceString = match.Groups[2].Value;
            var marketPriceString = match.Groups[3].Value;
            var amountString = match.Groups[4].Value;

            var name = SearchForCompanyName(document, amountString, purchasePriceString);

            var date = DateTime.Parse(dateString);
            var purchasePrice = Convert.ToDecimal(purchasePriceString);
            var marketPrice = Convert.ToDecimal(marketPriceString);
            var amount = Convert.ToDecimal(amountString);

            return new DiscountBuyTransaction(Broker.Fidelity, date, name, Currency.USD, purchasePrice, marketPrice, amount);
        }

        private DividendTransaction ParseDividendTransaction(PigPdfDocument document, string transactionString, int year)
        {
            //09/12 MICROSOFT CORP  594918104 Dividend Received  --$128.82

            var match = dividendTransactionRegex.Match(transactionString);

            var dateString = match.Groups[1].Value + "/" + year;
            var name = match.Groups[2].Value;
            var incomeString = match.Groups[3].Value;

            var date = DateTime.Parse(dateString);
            var income = Convert.ToDecimal(incomeString);
            var taxStringParts = SearchForTaxString(document, date)?.Split(' ');

            decimal tax = 0;
            if (taxStringParts != null)
            {
                tax = Convert.ToDecimal(taxStringParts[taxStringParts.Length - 1].TrimStart('-', '$'));
            }

            return new DividendTransaction(Broker.Fidelity, date, name, income, tax, Currency.USD);
        }

        private DepositTransaction ParseDepositTransaction(PigPdfDocument document, string transactionString, int year)
        {
            if (!transactionString.Contains("SHARES DEPOSITED"))
            {
                return null;
            }

            var dateString = transactionString.Substring(0, 5) + "/" + year;
            var name = transactionString.Substring(6, transactionString.IndexOf("SHARES DEPOSITED", StringComparison.Ordinal) - 7);

            var numbersString = transactionString.Substring(transactionString.IndexOf("Conversion", StringComparison.Ordinal) + 11);
            var amountString = numbersString.Substring(0, numbersString.IndexOf('.', StringComparison.Ordinal) + 4);
            var priceString = numbersString.Substring(amountString.Length);
            priceString = priceString.Substring(0, priceString.IndexOf(".", StringComparison.Ordinal) + 5);

            if (priceString.StartsWith("$", StringComparison.Ordinal))
            {
                priceString = priceString.Substring(1);
            }

            var date = DateTime.Parse(dateString);
            var amount = Convert.ToDecimal(amountString);
            var price = Convert.ToDecimal(priceString);

            return new DepositTransaction(Broker.Fidelity, date, name, amount, price, Currency.USD);
        }
    }
}
