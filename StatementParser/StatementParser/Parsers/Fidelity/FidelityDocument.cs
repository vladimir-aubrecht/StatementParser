using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace StatementParser.Parsers.Fidelity
{
    internal class FidelityDocument
    {
        private enum Separators { Header, Footer }

        private readonly Dictionary<FidelityTableName, Dictionary<Separators, string>> tableSeparatorsMap = new Dictionary<FidelityTableName, Dictionary<Separators, string>>
        {
            [FidelityTableName.ActivityOther] = new Dictionary<Separators, string>
            {
                [Separators.Header] = "Other Activity In SettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceTransactionCostAmount",
                [Separators.Footer] = "Total Other Activity In"
            },
            [FidelityTableName.ActivityDividend] = new Dictionary<Separators, string>
            {
                [Separators.Header] = "Dividends, Interest & Other Income (Includes dividend reinvestment)SettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceAmount",
                [Separators.Footer] = "Total Dividends, Interest & Other Income"
            },
            [FidelityTableName.ActivityTaxes] = new Dictionary<Separators, string>
            {
                [Separators.Header] = "STOCK PLAN ACCOUNTActivityTaxes Withheld DateSecurityDescriptionAmount",
                [Separators.Footer] = "Total Federal Taxes Withheld"
            },
            [FidelityTableName.ActivityBuy] = new Dictionary<Separators, string>
            {
                [Separators.Header] = "Securities Bought & SoldSettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceTransactionCostAmounti",
                [Separators.Footer] = "Total Securities Bought"
            },
            [FidelityTableName.SummaryESPP] = new Dictionary<Separators, string>
            {
                [Separators.Header] = "Employee Stock Purchase SummaryOffering PeriodDescriptionPurchaseDatePurchase PricePurchase DateFair Market ValueSharesPurchasedGain fromPurchase ",
                [Separators.Footer] = "Total for all Offering Periods"
            }
        };

        private readonly PdfDocument document;

        public FidelityTable this[FidelityTableName tableName]
        {
            get
            {
                return new FidelityTable(ParseTableContent(tableName), tableName);
            }
        }

        public FidelityDocument(PdfDocument document)
        {
            this.document = document ?? throw new ArgumentNullException(nameof(document));
        }

        private string ParseTableContent(FidelityTableName tableName)
        {
            var contents = GetPagesByHeader(this.document, tableName)
                .Select(page => ParseTransactionStringsFromPage(page, tableName));

            return String.Join("", contents); //Lets merge tables splitted cross multiple pages
        }

        private IList<Page> GetPagesByHeader(PdfDocument pdfDocument, FidelityTableName tableName)
        {
            var output = new List<Page>();

            var header = this.tableSeparatorsMap[tableName][Separators.Header];

            foreach (var page in pdfDocument.GetPages())
            {
                if (page.Text.Contains(header))
                {
                    output.Add(page);
                }
            }

            return output;
        }

        private string ParseTransactionStringsFromPage(Page page, FidelityTableName tableName)
        {
            var header = this.tableSeparatorsMap[tableName][Separators.Header];
            var footer = this.tableSeparatorsMap[tableName][Separators.Footer];

            var startIndex = page.Text.IndexOf(header, StringComparison.Ordinal) + header.Length;
            var endIndex = page.Text.IndexOf(footer, StringComparison.Ordinal);

            return page.Text.Substring(startIndex, endIndex - startIndex);
        }
    }
}
