using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace StatementParser.Parsers.Fidelity
{
    internal class FidelityTable
    {
        public enum TableName { ActivityOther, ActivityDividend, ActivityTaxes, ActivityBuy, SummaryESPP }
        private enum Separators { Header, Footer }

        private readonly Dictionary<TableName, Dictionary<Separators, string>> tableSeparatorsMap = new Dictionary<TableName, Dictionary<Separators, string>>
        {
            [TableName.ActivityOther] = new Dictionary<Separators, string>
            {
                [Separators.Header] = "Other Activity In SettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceTransactionCostAmount",
                [Separators.Footer] = "Total Other Activity In"
            },
            [TableName.ActivityDividend] = new Dictionary<Separators, string>
            {
                [Separators.Header] = "Dividends, Interest & Other Income (Includes dividend reinvestment)SettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceAmount",
                [Separators.Footer] = "Total Dividends, Interest & Other Income"
            },
            [TableName.ActivityTaxes] = new Dictionary<Separators, string>
            {
                [Separators.Header] = "STOCK PLAN ACCOUNTActivityTaxes Withheld DateSecurityDescriptionAmount",
                [Separators.Footer] = "Total Federal Taxes Withheld"
            },
            [TableName.ActivityBuy] = new Dictionary<Separators, string>
            {
                [Separators.Header] = "Securities Bought & SoldSettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceTransactionCostAmounti",
                [Separators.Footer] = "Total Securities Bought"
            },
            [TableName.SummaryESPP] = new Dictionary<Separators, string>
            {
                [Separators.Header] = "Employee Stock Purchase SummaryOffering PeriodDescriptionPurchaseDatePurchase PricePurchase DateFair Market ValueSharesPurchasedGain fromPurchase ",
                [Separators.Footer] = "Total for all Offering Periods"
            }
        };

        private readonly PdfDocument document;

        public FidelityTable(PdfDocument document)
        {
            this.document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public string ParseTableContent(TableName tableName)
        {
            var tableSeparators = this.tableSeparatorsMap[tableName];
            var contents = GetPagesByHeader(this.document, tableSeparators[Separators.Header])
                .Select(page => ParseTransactionStringsFromPage(page, tableSeparators[Separators.Header], tableSeparators[Separators.Footer]));

            return String.Join("", contents); //Lets merge tables splitted cross multiple pages
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

        private static string ParseTransactionStringsFromPage(Page page, string header, string footer)
        {
            var startIndex = page.Text.IndexOf(header, StringComparison.Ordinal) + header.Length;
            var endIndex = page.Text.IndexOf(footer, StringComparison.Ordinal);

            return page.Text.Substring(startIndex, endIndex - startIndex);
        }
    }
}
