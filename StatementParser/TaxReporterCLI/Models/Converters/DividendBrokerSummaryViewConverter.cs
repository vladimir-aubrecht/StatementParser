
using System;
using System.Collections.Generic;
using System.Linq;

using EnumsNET;

using StatementParser.Models;

using TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models;
using TaxReporterCLI.Models.Views;

namespace TaxReporterCLI.Models.Converters
{
    class DividendBrokerSummaryViewConverter
    {
        public AppendixSeznamRow[] ConvertToAppendixSeznamRows(IList<DividendBrokerSummaryView> dividendBrokerSummaryView)
        {
            return dividendBrokerSummaryView.Select(i => new AppendixSeznamRow()
            {
                BrokerName = ((Broker)i.Broker).AsString(EnumFormat.Description),
                Country = i.Country.TwoLetterISORegionName,
                IncomeInCZK = Math.Round(i.ExchangedPerYearTotalIncome),
                TaxInCZK = Math.Round(i.ExchangedPerYearTotalTax),
                TaxInOriginalCurrency = Math.Round(i.TotalTax)
            }).ToArray();
        }

        public AppendixIncomeTableRow[] ConvertToAppendixIncomeTableRow(IList<ESPPTransactionView> esppTransactionView, IList<DepositTransactionView> depositTransactionView)
        {
            return new AppendixIncomeTableRow[]
            {
                new AppendixIncomeTableRow()
                {
                    Income = Math.Round(depositTransactionView.Select(i => i.ExchangedPerYearTotalPrice).Sum() ?? 0)
                },
                new AppendixIncomeTableRow()
                {
                    Income = Math.Round(esppTransactionView.Select(i => i.ExchangedPerYearTotalProfit).Sum() ?? 0)
                }
            };
        }

        public Appendix3List[] ConvertToAppendix3List(IList<DividendBrokerSummaryView> dividendBrokerSummaryView)
        {
            var countries = dividendBrokerSummaryView.Select(i => i.Country).Distinct();

            var output = new List<Appendix3List>();
            foreach (var country in countries)
            {
                var dividendBrokerSummaryViewPerCountry = dividendBrokerSummaryView.Where(i => i.Country.Equals(country));

                var list = new Appendix3List() { Country = country.TwoLetterISORegionName };

                foreach (var view in dividendBrokerSummaryViewPerCountry)
                {
                    list.Income += view.ExchangedPerYearTotalIncome;
                    list.Tax += view.ExchangedPerYearTotalTax;
                }

                list.Income = Math.Round(list.Income);
                list.Tax = Math.Round(list.Tax);

                output.Add(list);
            }


            return output.ToArray();
        }

        public Appendix2 ConvertToAppendix2(IList<SaleTransactionView> saleTransactions)
        {
            decimal totalIncomes = 0;
            decimal totalExpenses = 0;
            foreach (var transactionView in saleTransactions)
            {
                totalIncomes += transactionView.ExchangedPerYearProfit ?? 0;
                totalExpenses += (transactionView.ExchangedPerYearTaxes ?? 0) + (transactionView.ExchangedPerYearSwap ?? 0) + (transactionView.ExchangedPerYearCommission ?? 0);
            }

            return new Appendix2()
            {
                TotalIncomes = Math.Round(totalIncomes),
                TotalExpenses = Math.Round(totalExpenses),
                TotalProfit = Math.Round(totalIncomes - totalExpenses),
                Row207 = Math.Round(totalIncomes),
                Row208 = Math.Round(totalExpenses),
                Row209 = Math.Round(totalIncomes - totalExpenses)
            };
        }

        public Appendix2OtherIncomeRow[] ConvertToAppendix2OtherIncomeRow(IList<SaleTransactionView> saleTransactions)
        {
            if (saleTransactions.Count == 0)
            {
                return null;
            }

            var appendix2 = ConvertToAppendix2(saleTransactions);

            return new Appendix2OtherIncomeRow[1]
            {
                new Appendix2OtherIncomeRow
                {
                    Code = "Z",
                    Type = "D",
                    Description = "Příjmy z prodeje cenných papírů",
                    Expenses = Math.Round(appendix2.TotalExpenses),
                    Income = Math.Round(appendix2.TotalIncomes),
                    Profit = Math.Round(appendix2.TotalProfit)
                }
            };
        }
    }
}
