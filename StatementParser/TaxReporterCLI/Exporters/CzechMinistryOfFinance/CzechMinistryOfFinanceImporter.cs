using System.IO;
using System.Xml.Serialization;

using TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance
{
    internal class CzechMinistryOfFinanceImporter
    {
        public CzechMinistryOfFinanceImporter()
        {
        }

        public Declaration Load(string filePath)
        {
            var serializer = new XmlSerializer(typeof(Declaration));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (var reader = new StreamReader(filePath))
            {
                return (Declaration)serializer.Deserialize(reader);
            }
        }
    }
}
