using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StatementParser.Parsers.Pdf;

namespace StatementParser.Parsers.Brokers.Fidelity
{
    internal class FidelityPdfConfiguration : IPdfConfiguration
    {
        private static readonly Regex DefaultSplitRegex = new Regex("([0-9]{2}/[0-9]{2} )", RegexOptions.Compiled);
        private static readonly Regex TimePeriodSplitRegex = new Regex("([0-9]{2}/[0-9]{2}/[0-9]{4}-[0-9]{2}/[0-9]{2}/[0-9]{4})", RegexOptions.Compiled);

        public IDictionary<PdfTableName, Dictionary<Separators, string>> TableRestrictorsMap { get; } = new Dictionary<PdfTableName, Dictionary<Separators, string>>
        {
            [PdfTableName.ActivityOther] = new Dictionary<Separators, string>
            {
                [Separators.Header] = "Other Activity In SettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceTransactionCostAmount",
                [Separators.Footer] = "Total Other Activity In"
            },
            [PdfTableName.ActivityDividend] = new Dictionary<Separators, string>
            {
                [Separators.Header] = "Dividends, Interest & Other Income (Includes dividend reinvestment)SettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceAmount",
                [Separators.Footer] = "Total Dividends, Interest & Other Income"
            },
            [PdfTableName.ActivityTaxes] = new Dictionary<Separators, string>
            {
                [Separators.Header] = "STOCK PLAN ACCOUNTActivityTaxes Withheld DateSecurityDescriptionAmount",
                [Separators.Footer] = "Total Federal Taxes Withheld"
            },
            [PdfTableName.ActivityBuy] = new Dictionary<Separators, string>
            {
                [Separators.Header] = "Securities Bought & SoldSettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceTransactionCostAmounti",
                [Separators.Footer] = "Total Securities Bought"
            },
            [PdfTableName.SummaryESPP] = new Dictionary<Separators, string>
            {
                [Separators.Header] = "Employee Stock Purchase SummaryOffering PeriodDescriptionPurchaseDatePurchase PricePurchase DateFair Market ValueSharesPurchasedGain fromPurchase ",
                [Separators.Footer] = "Total for all Offering Periods"
            }
        };

        public IDictionary<PdfTableName, Regex> TableRowSplitRegexes { get; } = new Dictionary<PdfTableName, Regex>
        {
            [PdfTableName.ActivityBuy] = DefaultSplitRegex,
            [PdfTableName.ActivityDividend] = DefaultSplitRegex,
            [PdfTableName.ActivityOther] = DefaultSplitRegex,
            [PdfTableName.ActivityTaxes] = DefaultSplitRegex,
            [PdfTableName.SummaryESPP] = TimePeriodSplitRegex
        };
    }
}
