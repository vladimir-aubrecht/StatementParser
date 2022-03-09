using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using StatementParser.Models;
using StatementParser.Parsers.Brokers.Lynx.CsvModels;
using StatementParser.Parsers.Brokers.Lynx.Extensions;

namespace StatementParser.Parsers.Brokers.Lynx
{
    internal class LynxCsvParser : ITransactionParser
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

            var statement = LoadStatementModel(statementFilePath);

            if (statement == null || statement.Statement[0].FieldValue != "Activity Statement")
            {
                return null;
            }

            var output = new List<Transaction>();
            foreach (var dividend in statement.Dividends)
            {
                var currency = Enum.Parse<Currency>(dividend.Currency);
                var tax = SearchForTax(dividend, statement.WithholdingTaxes);

                var transaction = new DividendTransaction(Broker.Lynx, dividend.Date.Value, dividend.Description, dividend.Amount, tax, currency);
                output.Add(transaction);
            }

            return output;
        }

        private decimal SearchForTax(DividendsRowModel dividend, List<WithholdingTaxRowModel> withholdingTaxes)
        {
            var tax = withholdingTaxes.FirstOrDefault(i => dividend.Description.Contains(Regex.Replace(i.Description, " - [^ ]+ Tax", "", RegexOptions.Compiled)) && i.Date == dividend.Date);

            return tax?.Amount ?? 0;
        }

        private StatementModel LoadStatementModel(string statementFilePath)
        {
            try
            {
                var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
                ci.DateTimeFormat = new DateTimeFormatInfo()
                {
                    ShortDatePattern = "yyyy-MM-dd"
                };

                var csvConfig = new CsvConfiguration(ci)
                {
                    BadDataFound = _ => { /* Some legal notes may be invalid csv, lets ignore them */ }
                };
                using var reader = new StreamReader(statementFilePath);
                using var csv = new CsvReader(reader, ci);
                csv.Configuration.BadDataFound = (readingContext) => { /* Some legal notes may be invalid csv, lets ignore them */ };

                var statement = csv.ReadObject<StatementModel>(0, () => csv.GetField(1) == "Header");

                if (statement == null)
                {
                    return null;
                }

                statement.Dividends = statement.Dividends.Where(i => i.Date != null).ToList();
                statement.WithholdingTaxes = statement.WithholdingTaxes.ToList();

                return statement;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
