using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASoft.TextDeserializer;
using ASoft.TextDeserializer.Exceptions;
using NPOI.HSSF.Record;
using StatementParser.Models;
using StatementParser.Parsers.Brokers.Revolut.PdfModels;

namespace StatementParser.Parsers.Brokers.Revolut
{
    internal class RevolutStatementParser : ITransactionParser
    {
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

                    foreach (var dividend in parsedDocument.Dividends)
                    {
                        var rate = FindRateAmount(dividend.Dividend);
                        var reconstructedDividends = ReconstructUsdAmount(dividend.Dividend, rate);
                        var reconstructedTax = ReconstructUsdAmount(dividend.WithholdingTax, rate);

                        var transaction = new DividendTransaction(Broker.Revolut, dividend.Date, dividend.Symbol, reconstructedDividends, reconstructedTax, Currency.USD);
                        transactions.Add(transaction);
                    }

                    foreach (var sale in parsedDocument.Sales)
                    {
                        var buyRate = FindRateAmount(sale.CostBasis);
                        var saleRate = FindRateAmount(sale.GrossProceeds);

                        var reconstructedCostBasis = ReconstructUsdAmount(sale.CostBasis, buyRate);
                        var reconstructedGrossProceeds = ReconstructUsdAmount(sale.GrossProceeds, saleRate);

                        var pnl = reconstructedGrossProceeds - reconstructedCostBasis;

                        var transaction = new SaleTransaction(Broker.Revolut, sale.DateSold, sale.Symbol,  Currency.USD, 0, sale.Quantity, reconstructedCostBasis, reconstructedGrossProceeds, 0, 0, 0, pnl);
                        transactions.Add(transaction);
                    }

                }
                catch (TextException)
                {
                    return null;
                }
            }

            return transactions;
        }

        private decimal FindRateAmount(string dividend)
        {
            try
            {
                var rateIndex = dividend.IndexOf("Rate: ");
                var rate = Convert.ToDecimal(dividend.Substring(rateIndex + 6));

                return rate;
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(dividend);
            }
        }

        private decimal ReconstructUsdAmount(string dividend, decimal rate)
        {
            var czkIndex = dividend.IndexOf("CZK");

            if (czkIndex == -1)
            {
                czkIndex = dividend.Length - 3;
            }

            var numbersString = dividend.Replace("US$", "").Substring(0, czkIndex - 1);

            var readString = "";
            for (var i = 0; i < numbersString.Length; i++)
            {
                readString += numbersString[i];

                try
                {
                    var usdSuggestion = Convert.ToDecimal(readString);
                    var leftover = numbersString.Substring(readString.Length);

                    var czkSuggestion = Convert.ToDecimal(leftover);

                    var calculatedCZK = usdSuggestion * rate;

                    if (Math.Abs(czkSuggestion - calculatedCZK) < 1)
                    {
                        return usdSuggestion;
                    }
                }
                catch
                {
                    continue;
                }
            }

            return -1;
        }

    }
}
