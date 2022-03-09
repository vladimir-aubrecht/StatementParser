using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ASoft.TextDeserializer;
using ASoft.TextDeserializer.Exceptions;

using CsvHelper;

using StatementParser.Models;
using StatementParser.Parsers.Brokers.Revolut.CsvModels;
using StatementParser.Parsers.Brokers.Revolut.PdfModels;

namespace StatementParser.Parsers.Brokers.Revolut
{
    internal class RevolutStatementParser2021 : ITransactionParser
    {
        private bool CanParse(string statementFilePath)
        {
            return File.Exists(statementFilePath) && Path.GetExtension(statementFilePath).ToLowerInvariant() == ".csv";
        }

        public IList<Transaction> Parse(string statementFilePath)
        {
            if (!CanParse(statementFilePath))
            {
                return null;
            }

            var transactions = new List<Transaction>();

            var accountTransactions = LoadModel<AccountRowModel>(statementFilePath, "dd/MM/yyyy");

            if (accountTransactions != null)
            { 
                var dividendTransactions = accountTransactions.Where(i => i.Type == "DIVIDEND").ToList();

                foreach (var dividendTransaction in dividendTransactions)
                {
                    if (dividendTransaction.TotalAmount != 0)
                    {
                        var netDividend = 100 * dividendTransaction.TotalAmount / 85 ?? 0;  // HACK: It's expecting Czech tax of 15% being withdraw. Unfortunately Revolut statement doesn't have neither tax neither original value yet :-/
                        var tax = netDividend - dividendTransaction.TotalAmount ?? 0;

                        transactions.Add(new DividendTransaction(Broker.Revolut, dividendTransaction.Date, dividendTransaction.Ticker, Math.Round(netDividend, 2), Math.Round(tax, 2), dividendTransaction.Currency));
                    }
                }
            }

            var activityTransactions = LoadModel<ActivityRowModel>(statementFilePath, "yyyy-MM-dd");

            if (activityTransactions != null)
            {

                foreach (var activityTransaction in activityTransactions)
                {
                    transactions.Add(
                        new SaleTransaction(Broker.Revolut, activityTransaction.DateSold, activityTransaction.Symbol, Currency.USD, 0, activityTransaction.Quantity, activityTransaction.CostBasis, activityTransaction.Amount, 0, 0, 0, activityTransaction.RealisedPnL)
                    );
                }
            }

            return transactions;
        }

        private IList<T> LoadModel<T>(string statementFilePath, string dateFormat)
        {
            try
            {
                var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
                ci.DateTimeFormat = new DateTimeFormatInfo()
                {
                    ShortDatePattern = dateFormat
                };

                using var reader = new StreamReader(statementFilePath);
                using var csv = new CsvReader(reader, ci);

                return csv.GetRecords<T>().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
