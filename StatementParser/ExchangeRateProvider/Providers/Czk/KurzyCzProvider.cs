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
            //ForeignExchangeRate: https://financnisprava.gov.cz/assets/cs/prilohy/d-sprava-dani-a-poplatku/Pokyn_GFR-D-65.pdf
            // 2025 year: https://microsofteur-my.sharepoint.com/personal/lsokolovsky_microsoft_com/_layouts/15/onedrive.aspx?id=%2Fpersonal%2Flsokolovsky%5Fmicrosoft%5Fcom%2FDocuments%2FMicrosoft%20Teams%20Chat%20Files%2F2026%2D01%2D14%5FFinancni%2Dzpravodaj%2Dcislo%2D2%2D2026%2Epdf&parent=%2Fpersonal%2Flsokolovsky%5Fmicrosoft%5Fcom%2FDocuments%2FMicrosoft%20Teams%20Chat%20Files&ga=1

            var map = new Dictionary<int, CurrencyDescriptor>
            {
                { 2023, new CurrencyDescriptor("USD", "dolar", 22.14m, "USA") },
                { 2024, new CurrencyDescriptor("USD", "dolar", 23.28m, "USA") },
                { 2025, new CurrencyDescriptor("USD", "dolar", 21.84m, "USA") }
            };

            if (map.TryGetValue(date.Year, out CurrencyDescriptor currency))
                return new CurrencyList(new[] { currency });
            else
                throw new NotImplementedException($"Year {date} is not supported: hardcode it manually at KurzyCzProvider.cz");
            // following code is not working :(


            var url = this.CreateUrlByDate(date);

            var request = new HttpClient(new HttpClientHandler());
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();

            using (var response = await request.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var htmlContent = await response.Content.ReadAsStringAsync();

                    htmlDocument.LoadHtml(htmlContent);
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
