
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using StatementParser.Models;

namespace StatementParser.Parsers.Brokers.FxChoice
{
	internal class FxChoiceStatementParser : ITransactionParser
	{
		private bool CanParse(string statementFilePath)
		{
			if (!File.Exists(statementFilePath) || Path.GetExtension(statementFilePath).ToLowerInvariant() != ".htm")
			{
				return false;
			}

			return true;
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

			var levarageCell = pageDocument.DocumentNode.SelectSingleNode("/html/body/div/table/tr[1]/td[4]").InnerText;
			var levarage = Convert.ToInt32(levarageCell.Substring(levarageCell.LastIndexOf(':') + 1));

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
					var amount = Convert.ToDecimal(row.SelectSingleNode("./td[4]").InnerText);
					var name = row.SelectSingleNode("./td[5]").InnerText;
					var purchasePrice = Convert.ToDecimal(row.SelectSingleNode("./td[6]").InnerText);
					var date = DateTime.Parse(row.SelectSingleNode("./td[09]").InnerText);
					var salePrice = Convert.ToDecimal(row.SelectSingleNode("./td[10]").InnerText);
					var commision = Convert.ToDecimal(row.SelectSingleNode("./td[11]").InnerText);
					var taxes = Convert.ToDecimal(row.SelectSingleNode("./td[12]").InnerText);
					var swap = Convert.ToDecimal(row.SelectSingleNode("./td[13]").InnerText);
					var profit = Convert.ToDecimal(row.SelectSingleNode("./td[14]").InnerText);

					var transaction = new SaleTransaction(Broker.FxChoice, date, name, currency, levarage, amount, purchasePrice, salePrice, commision, taxes, swap, profit);
					transactions.Add(transaction);
				}
			}

			return transactions;
		}
	}
}
