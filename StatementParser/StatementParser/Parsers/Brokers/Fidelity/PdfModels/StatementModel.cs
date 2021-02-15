using ASoft.TextDeserializer.Attributes;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfModels
{
	[DeserializeByRegex("STOCK PLAN SERVICES REPORT.+, (?<Year>[0-9]{4})Envelope")]
	internal class StatementModel
	{
		public int Year { get; set; }

		[DeserializeCollectionByRegex(
			"([0-9]{2}/[0-9]{2}/[0-9]{4}-[0-9]{2}/[0-9]{2}/[0-9]{4})",
			"Employee Stock Purchase SummaryOffering PeriodDescriptionPurchaseDatePurchase PricePurchase DateFair Market ValueSharesPurchasedGain fromPurchase (.+?)Total for all Offering Periods")]
		public ESPPModel[] ESPP { get; set; }

		[DeserializeCollectionByRegex(
			"([0-9]{2}/[0-9]{2} )",
			"Taxes Withheld DateSecurityDescriptionAmount(.+?)Total Federal Taxes Withheld")]
		public ActivityTaxesModel[] ActivityTaxes { get; set; }

		[DeserializeCollectionByRegex(
			"([0-9]{2}/[0-9]{2} )",
			"Other Activity In (?:\\(continued\\))?SettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPrice(?:TotalCost Basis)?TransactionCostAmount(.+?)(?:Total Other Activity In|$)")]
		public ActivityOtherModel[] ActivityOther { get; set; }

		[DeserializeCollectionByRegex(
			"([0-9]{2}/[0-9]{2} )",
			"Dividends, Interest & Other Income \\(Includes dividend reinvestment\\)SettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceAmount(.+?)Total Dividends, Interest & Other Income")]
		public ActivityDividendModel[] ActivityDividend { get; set; }

		[DeserializeCollectionByRegex(
			"([0-9]{2}/[0-9]{2} )",
			"Securities Bought & SoldSettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceTransactionCostAmounti(.+?)Total Securities Bought")]
		public ActivityBuyModel[] ActivityBuy { get; set; }
	}
}
