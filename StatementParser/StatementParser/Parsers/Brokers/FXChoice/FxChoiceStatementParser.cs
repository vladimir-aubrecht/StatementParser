
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

using StatementParser.Models;

namespace StatementParser.Parsers.Brokers.FxChoice
{
    internal class FxChoiceStatementParser : ITransactionParser
    {
        private readonly ILogger<FxChoiceStatementParser> logger;

        public FxChoiceStatementParser(ILogger<FxChoiceStatementParser> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private bool CanParse(string statementFilePath)
        {
            return File.Exists(statementFilePath) && Path.GetExtension(statementFilePath).ToLowerInvariant() == ".htm";
        }

        public IList<Transaction> Parse(string statementFilePath)
        {
            if (!CanParse(statementFilePath))
            {
                return null;
            }

            var pageDocument = new HtmlDocument();
            pageDocument.Load(statementFilePath);

            var currencyCell = pageDocument.DocumentNode.SelectSingleNode("/html/body/div/table/tr[1]/td[3]").InnerText;
            var currency = (Currency)Enum.Parse(typeof(Currency), currencyCell.Substring(currencyCell.LastIndexOf(':') + 2));

            var leverageCell = pageDocument.DocumentNode.SelectSingleNode("/html/body/div/table/tr[1]/td[4]").InnerText;
            var leverage = Convert.ToInt32(leverageCell.Substring(leverageCell.LastIndexOf(':') + 1));

            var rows = pageDocument.DocumentNode.SelectNodes("/html/body/div/table/tr").Skip(3);

            var transactions = new List<Transaction>();
            foreach (var row in rows)
            {
                var firstCellAttributes = row.SelectSingleNode("./td[1]").Attributes;

                if (firstCellAttributes["colspan"]?.Value == "10")
                {
                    break;
                }

                var type = row.SelectSingleNode("./td[3]")?.InnerText;
                if (type == "buy" || type == "sell")
                {
                    var amountString = row.SelectSingleNode("./td[4]").InnerText;
                    var amount = Convert.ToDecimal(amountString, CultureInfo.InvariantCulture);
                    var name = row.SelectSingleNode("./td[5]").InnerText;
                    var purchasePrice = Convert.ToDecimal(row.SelectSingleNode("./td[6]").InnerText, CultureInfo.InvariantCulture);
                    var date = DateTime.Parse(row.SelectSingleNode("./td[09]").InnerText);
                    var salePrice = Convert.ToDecimal(row.SelectSingleNode("./td[10]").InnerText, CultureInfo.InvariantCulture);
                    var commission = Math.Abs(Convert.ToDecimal(row.SelectSingleNode("./td[11]").InnerText, CultureInfo.InvariantCulture));
                    var taxes = Math.Abs(Convert.ToDecimal(row.SelectSingleNode("./td[12]").InnerText, CultureInfo.InvariantCulture));
                    var swap = -Convert.ToDecimal(row.SelectSingleNode("./td[13]").InnerText, CultureInfo.InvariantCulture);
                    var profit = Convert.ToDecimal(row.SelectSingleNode("./td[14]").InnerText, CultureInfo.InvariantCulture);

                    var transaction = new SaleTransaction(Broker.FxChoice, date, name, currency, leverage, amount, purchasePrice, salePrice, commission, taxes, swap, profit);
                    transactions.Add(transaction);
                }
            }

            return transactions;
        }
    }
}
