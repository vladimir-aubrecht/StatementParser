﻿using System;
using ASoft.TextDeserializer.Attributes;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfModels
{
	[DeserializeByRegex("(?<Date>[0-9]{2}/[0-9]{2}/[0-9]{4})\\$?(?<PurchasePrice>[0-9]+\\.[0-9]{5})\\$?(?<MarketPrice>[0-9]+\\.[0-9]{3})(?<Amount>[0-9]+\\.[0-9]{3})")]
	internal class ESPPModel
	{
		public DateTime Date { get; set; }
		public decimal PurchasePrice { get; set; }
		public decimal MarketPrice { get; set; }
		public decimal Amount { get; set; }
	}
}
