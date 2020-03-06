using System;
using ASoft.TextDeserializer.Attributes;

namespace StatementParser.Parsers.Brokers.Revolut.PdfModels
{
    [DeserializeByRegex("(?<TradeDate>[0-9]{2}/[0-9]{2}/[0-9]{4})(?<SettleDate>[0-9]{2}/[0-9]{2}/[0-9]{4})(?<Currency>[A-Z]{3})(?<ActivityType>DIVNRA|DIV)(?<Description>.+?)00.00(?<AmountRaw>\\(?\\d+\\.\\d{2}\\)?)")]
	internal class ActivityDividendModel
	{
		public DateTime TradeDate { get; set; }

        public DateTime SettleDate { get; set; }

        public string Currency { get; set; }

        public string ActivityType { get; set; }

        public string Description { get; set; }

        public decimal Amount => decimal.Parse(AmountRaw.Replace('(', '-').TrimEnd(')'));
        public string AmountRaw { get; set; }
	}
}
