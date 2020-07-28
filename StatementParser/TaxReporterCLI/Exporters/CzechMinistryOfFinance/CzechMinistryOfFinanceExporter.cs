using System.Text;
using System.Xml;
using System.Xml.Serialization;

using TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance
{
    internal class CzechMinistryOfFinanceExporter
    {
        public CzechMinistryOfFinanceExporter()
        {
        }

        public void Save(string filePath, Declaration declaration)
        {
            var serializer = new XmlSerializer(typeof(Declaration));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (XmlTextWriter tw = new XmlTextWriter(filePath, Encoding.UTF8))
            {
                tw.Formatting = Formatting.Indented;
                tw.Indentation = 4;
                serializer.Serialize(tw, declaration, ns);
            }
        }
    }
}
