using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NPOI.SS.UserModel;
using StatementParser.Models;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace StatementParser.Parsers.Fidelity
{
    public class FidelityStatementParser : ITransactionParser
    {
        private static readonly Regex employeePurchaseSanitizeRegex = new Regex("[0-9]{2}/[0-9]{2}/[0-9]{4}-[0-9]{2}/[0-9]{2}/[0-9]{4}Employee Purchase", RegexOptions.Compiled);
        private static readonly Regex dividendTransactionRegex = new Regex("([0-9]{2}/[0-9]{2}) (.+?)  ?[0-9]+ Dividend Received  --\\$?([0-9]+\\.[0-9]{2})", RegexOptions.Compiled);
        private static readonly Regex discountedBuyRegex = new Regex("([0-9]{2}/[0-9]{2}/[0-9]{4})\\$([0-9]+\\.[0-9]{5})\\$([0-9]+\\.[0-9]{3})([0-9]+\\.[0-9]{3})", RegexOptions.Compiled);

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

            using (PdfDocument document = PdfDocument.Open(statementFilePath))
            {
                var year = ParseYear(document);

                transactions.AddRange(
                    ParseTransactions(document, year, TableName.ActivityOther, (doc, ts, year) => ParseDepositTransaction(doc, ts, year)));

                transactions.AddRange(
                    ParseTransactions(document, year, TableName.ActivityDividend, (doc, ts, year) => ParseDividendTransaction(doc, ts, year)));

                transactions.AddRange(
                    ParseTransactions(document, year, TableName.SummaryESPP, (doc, ts, year) => ParseDiscountedBuyTransaction(doc, ts, year)));
            }

            return transactions;
        }

        public string SearchForCompanyName(PdfDocument document, string amount, string price)
        {
            var transactionStrings = new FidelityDocument(document)[TableName.ActivityBuy];

            var foundTransactions = transactionStrings.Where(i => i.Contains($" You Bought {amount}${price}") && i.Contains("ESPP"));

            if (foundTransactions.Count() > 1)
            {
                // Probability that somebody will get ESPP for 2 companies of exact discounted price and exact amount on one report is close to 0, lets consider it's impossible.
                // I don't know better way how to find it based on artifacts from page about ESPP.

                throw new Exception("Couldn't properly detect name of company for ESPP.");
            }

            var transaction = foundTransactions.First();

            return transaction.Substring(6, transaction.IndexOf("ESPP", StringComparison.Ordinal) - 7);
        }

        private string SearchForTaxString(PdfDocument document, DateTime date)
        {
            var transactionStrings = new FidelityDocument(document)[TableName.ActivityTaxes];

            foreach (var transaction in transactionStrings)
            {
                if (transaction.StartsWith(date.ToString("MM/dd"), StringComparison.Ordinal))
                {
                    return transaction;
                }
            }

            return null;
        }

        private IList<Transaction> ParseTransactions(PdfDocument document, int year, TableName tableName, Func<PdfDocument, string, int, Transaction> parseFunc)
        {
            var output = new List<Transaction>();

            var transactionStrings = new FidelityDocument(document)[tableName];

            foreach (var transactionString in transactionStrings)
            {
                var result = parseFunc(document, transactionString, year);

                if (result != null)
                {
                    output.Add(result);
                }
            }

            return output;
        }

        private int ParseYear(PdfDocument document)
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

        private DiscountBuyTransaction ParseDiscountedBuyTransaction(PdfDocument document, string transactionString, int year)
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

        private DividendTransaction ParseDividendTransaction(PdfDocument document, string transactionString, int year)
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

        private DepositTransaction ParseDepositTransaction(PdfDocument document, string transactionString, int year)
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
