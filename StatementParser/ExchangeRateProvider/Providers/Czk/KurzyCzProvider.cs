using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.Providers.Czk
{
    public class KurzyCzProvider : IExchangeProvider
    {
        private string apiUrl = "https://www.kurzy.cz/kurzy-men/jednotny-kurz/";

        public KurzyCzProvider()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public async Task<CurrencyList> FetchCurrencyListByDateAsync(DateTime date)
        {
            var url = this.CreateUrlByDate(date);

            var request = WebRequest.CreateHttp(url);
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();

            // TODO: Why async version doesn't work on Mac?
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseStream = response.GetResponseStream();
                var encoding = Encoding.GetEncoding(response.CharacterSet);
                using (var sr = new StreamReader(responseStream, encoding))
                {
                    var htmlContent = sr.ReadToEnd();
                    htmlDocument.LoadHtml(htmlContent);
                }
            }

            var table = htmlDocument.DocumentNode.SelectNodes("//*[@id=\"leftcolumn\"]/div[2]/div[2]/table/tr");

            var output = new List<CurrencyDescriptor>();
            for (int i = 1; i < table.Count; i++)
            {
                var cellNode = table[i].SelectNodes("td");

                var country = this.SanitizeValue(cellNode[0].SelectNodes("a/span")[0].InnerHtml);
                var name = this.SanitizeValue(cellNode[0].SelectNodes("a/span")[1].InnerHtml);
                var code = this.SanitizeValue(cellNode[2].InnerHtml);
                var amount = Convert.ToDecimal(this.SanitizeValue(cellNode[3].InnerHtml));

                if (amount == 0)
                {
                    // Kurzy.cz has bug on website for Indonesia in 2013. Reported to them, hopefully they fix it soon.
                    continue;
                }

                var price = Decimal.Parse(this.SanitizeValue(cellNode[4].InnerHtml), System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"));

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
            return apiUrl + date.Year;
        }
    }
}
