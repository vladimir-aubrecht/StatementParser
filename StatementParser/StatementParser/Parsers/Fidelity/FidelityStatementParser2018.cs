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
    public class FidelityStatementParser2018 : ITransactionParser
    {
        private const string DepositTransactionTableHeader = "Other Activity In SettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceTransactionCostAmount";
        private const string DepositTransactionTableFooter = "Total Other Activity In--";

        private const string DividendTransactionTableHeader = "Dividends, Interest & Other Income (Includes dividend reinvestment)SettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceAmount";
        private const string DividendTransactionTableFooter = "Total Dividends, Interest & Other Income";

        private const string TaxTransactionTableHeader = "STOCK PLAN ACCOUNTActivityTaxes Withheld DateSecurityDescriptionAmount";
        private const string TaxTransactionTableFooter = "Total Federal Taxes Withheld";

        private static readonly Regex rowSplitRegex = new Regex("([0-9]{2}/[0-9]{2} )", RegexOptions.Compiled);
        private static readonly Regex dividendTransactionRegex = new Regex("([0-9]{2}/[0-9]{2}) (.+?)  ?[0-9]+ Dividend Received  --\\$?([0-9]+\\.[0-9]{2})", RegexOptions.Compiled);

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
                    ParseTransactions(document, year, DepositTransactionTableHeader, DepositTransactionTableFooter, (doc, ts, year) => ParseDepositTransaction(doc, ts, year)));

                transactions.AddRange(
                    ParseTransactions(document, year, DividendTransactionTableHeader, DividendTransactionTableFooter, (doc, ts, year) => ParseDividendTransaction(doc, ts, year)));
            }

            return transactions;
        }

        private string SearchForTaxString(PdfDocument document, DateTime date)
        {
            var transactionStrings = ParseTransactionStringsFromDocument(document, TaxTransactionTableHeader, TaxTransactionTableFooter);

            foreach (var transaction in transactionStrings)
            {
                if (transaction.StartsWith(date.ToString("MM/dd"), StringComparison.Ordinal))
                {
                    return transaction;
                }
            }

            return null;
        }

        private IList<Transaction> ParseTransactions(PdfDocument document, int year, string header, string footer, Func<PdfDocument, string, int, Transaction> parseFunc)
        {
            var output = new List<Transaction>();

            var transactionStrings = ParseTransactionStringsFromDocument(document, header, footer);

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

        private IList<string> ParseTransactionStringsFromDocument(PdfDocument document, string header, string footer)
        {
            var output = new List<string>();

            foreach (Page page in GetPagesByHeader(document, header))
            {
                var transactionStrings = ParseTransactionStringsFromPage(page, header, footer);
                output.AddRange(transactionStrings);
            }

            return output;
        }

        private IList<Page> GetPagesByHeader(PdfDocument pdfDocument, string header)
        {
            var output = new List<Page>();

            foreach (var page in pdfDocument.GetPages())
            {
                if (page.Text.Contains(header))
                {
                    output.Add(page);
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

        private static IList<string> ParseTransactionStringsFromPage(Page page, string header, string footer)
        {
            var startIndex = page.Text.IndexOf(header, StringComparison.Ordinal) + header.Length;
            var endIndex = page.Text.IndexOf(footer, StringComparison.Ordinal);

            var transactionsTableString = page.Text.Substring(startIndex, endIndex - startIndex);

            var parts = rowSplitRegex.Split(transactionsTableString);

            var output = new List<string>();

            for (int i = 1; i < parts.Length; i += 2)
            {
                output.Add(parts[i] + parts[i + 1]);
            }

            return output;
        }
    }
}
