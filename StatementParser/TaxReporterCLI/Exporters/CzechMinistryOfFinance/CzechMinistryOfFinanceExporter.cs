using System.Text;
using System.Xml;
using System.Xml.Serialization;

using TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models;
using TaxReporterCLI.Models.Views;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance
{
    internal class CzechMinistryOfFinanceExporter
    {
        private readonly DividendBrokerSummaryView dividendBrokerSummaryView;
        private readonly TransactionView transactionView;

        public CzechMinistryOfFinanceExporter(DividendBrokerSummaryView dividendBrokerSummaryView, TransactionView transactionView)
        {
            this.dividendBrokerSummaryView = dividendBrokerSummaryView;
            this.transactionView = transactionView;
        }

        public TransactionView TransactionView { get; }

        public void Save(string filePath)
        {
            var builder = new TaxDeclarationBuilder();

            var serializer = new XmlSerializer(typeof(Declarations));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (XmlTextWriter tw = new XmlTextWriter(filePath, Encoding.UTF8))
            {
                tw.Formatting = Formatting.Indented;
                tw.Indentation = 4;
                serializer.Serialize(tw, builder.Build(), ns);
            }
        }
    }
}
