using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml;
using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.Providers.Czk
{
	public class CzechNationalBankProvider : IExchangeProvider
	{
		private string apiUrl = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml?date=";

		public Task<ICurrencyList> FetchCurrencyListByDateAsync(DateTime date)
		{
			var url = this.CreateUrlByDate(date);
			var xmlDocument = new XmlDocument();
			xmlDocument.Load(url);

			var rows = xmlDocument.GetElementsByTagName("radek");

			var list = new List<CurrencyDescriptor>();
			foreach (XmlNode row in rows)
			{
				var code = row.Attributes["kod"].Value;
				var name = row.Attributes["mena"].Value;
				var amount = Convert.ToUInt32(row.Attributes["mnozstvi"].Value);
				var price = Decimal.Parse(row.Attributes["kurz"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("cs-cz"));
				var country = row.Attributes["zeme"].Value;

				list.Add(new CurrencyDescriptor(code, name, price, amount, country));
			}

			return Task.FromResult<ICurrencyList>(new CurrencyList(list));
		}

		private string CreateUrlByDate(DateTime date)
		{
			var czechDate = date.Day + "." + date.Month + "." + date.Year;

			return apiUrl + czechDate;
		}
	}
}
