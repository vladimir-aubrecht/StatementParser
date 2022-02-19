using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.Providers.Czk
{
	public class KurzyCzProvider : IExchangeProvider
	{
		private const string ApiUrl = "https://www.kurzy.cz/kurzy-men/jednotny-kurz/";

		public KurzyCzProvider()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}

		public async Task<ICurrencyList> FetchCurrencyListByDateAsync(DateTime date)
		{
			var url = this.CreateUrlByDate(date);

			// var request = WebRequest.CreateHttp(url);
			var request = new HttpClient(new HttpClientHandler());
			var htmlDocument = new HtmlAgilityPack.HtmlDocument();

			using (var response = await request.GetAsync(url))
			{
				if (response.IsSuccessStatusCode)
				{
					var responseStream = await response.Content.ReadAsStringAsync();

					htmlDocument.LoadHtml(responseStream);
				}
			}

			var table = htmlDocument.DocumentNode.SelectNodes("//*[@id=\"leftcolumn\"]/div[2]/div[1]/table/tr");

			var output = new List<CurrencyDescriptor>();
			for (int i = 1; i < table.Count; i++)
			{
				var cellNode = table[i].SelectNodes("td");

				var country = this.SanitizeValue(cellNode[0].SelectNodes("a/span")[0].InnerHtml);
				var name = this.SanitizeValue(cellNode[0].SelectNodes("a/span")[1].InnerHtml);
				var code = this.SanitizeValue(cellNode[2].InnerHtml);
				var amount = Convert.ToDecimal(this.SanitizeValue(cellNode[3].InnerHtml));
				var price = Decimal.Parse(this.SanitizeValue(cellNode[4].InnerHtml), NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"));

				output.Add(new CurrencyDescriptor(code, name, price, amount, country));
			}

			return new CurrencyList(output);
		}

		private string SanitizeValue(string value)
		{
			return Regex.Replace(HttpUtility.HtmlDecode(value).Trim(), "<.*?>", string.Empty);
		}

		private string CreateUrlByDate(DateTime date)
		{
			return ApiUrl + date.Year;
		}
	}
}
