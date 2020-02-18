using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;

namespace StatementParser.Parsers.Brokers.Lynx.CsvModels
{
	internal class StatementModel
	{
		[Name("Statement")]
		public List<StatementRowModel> Statement { get; set; }

		[Name("Dividends")]
		public List<DividendsRowModel> Dividends { get; set; }

		[Name("Withholding Tax")]
		public List<WithholdingTaxRowModel> WithholdingTaxes { get; set; }
	}
}