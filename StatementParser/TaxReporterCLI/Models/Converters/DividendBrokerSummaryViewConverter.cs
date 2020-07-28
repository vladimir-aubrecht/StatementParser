
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
                Country = "US", // TODO: Find better way, currently apparently wrong and relies on review process of tax declaration
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
    }
}
