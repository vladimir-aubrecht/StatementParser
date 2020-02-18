using System.Collections.Generic;
using System.Threading.Tasks;
using StatementParser.Models;
using StatementParser.Parsers;
using StatementParser.Parsers.Brokers.Degiro;
using StatementParser.Parsers.Brokers.Fidelity;
using StatementParser.Parsers.Brokers.FxChoice;
using StatementParser.Parsers.Brokers.Lynx;
using StatementParser.Parsers.Brokers.MorganStanley;

namespace StatementParser
{
	public class TransactionParser
	{
		private IList<ITransactionParser> parsers = new List<ITransactionParser>();

		public TransactionParser()
		{
			parsers.Add(new MorganStanleyStatementXlsParser());
			parsers.Add(new MorganStanleyStatementPdfParser());
			parsers.Add(new FidelityStatementParser());
			parsers.Add(new FxChoiceStatementParser());
			parsers.Add(new LynxCsvParser());
			parsers.Add(new DegiroParser());
		}

		public Task<IList<Transaction>> ParseAsync(string statementFilePath)
		{
			return Task.Run(() => ParseFiles(statementFilePath));
		}

		private IList<Transaction> ParseFiles(string statementFilePath)
		{
			foreach (var parser in parsers)
			{
				var result = parser.Parse(statementFilePath);

				if (result != null)
				{
					return result;
				}
			}

			return null;
		}
	}
}
