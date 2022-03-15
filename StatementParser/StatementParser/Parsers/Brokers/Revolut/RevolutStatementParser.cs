using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASoft.TextDeserializer;
using ASoft.TextDeserializer.Exceptions;
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
                        var transaction = new DividendTransaction(Broker.Revolut, dividend.Date, dividend.Symbol, dividend.Dividend, dividend.WithholdingTax, Currency.USD);
                        transactions.Add(transaction);
                    }

                    foreach (var sale in parsedDocument.Sales)
                    {
                        var transaction = new SaleTransaction(Broker.Revolut, sale.DateSold, sale.Symbol,  Currency.USD, 0, sale.Quantity, sale.CostBasis, sale.GrossProceeds, 0, 0, 0, sale.PnL);
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
    }
}
