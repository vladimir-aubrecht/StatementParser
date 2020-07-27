using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class TaxDeclaration
    {
        [XmlElement("VetaD")]
        public Taxes Taxes { get; set; }

        [XmlElement("VetaP")]
        public PersonalInformation PersonalInformation { get; set; }

        [XmlElement("VetaO")]
        public Section2 Section2 { get; set; }

        [XmlElement("VetaJ")]
        public Appendix2[] Appendix2 { get; set; }

        [XmlElement("VetaL")]
        public Appendix3[] Appendix3 { get; set; }

        [XmlElement("Vetab")]
        public AppendixIncomeTable[] AppendixIncomeTable { get; set; }

        [XmlElement("Vetad")]
        public AppendixSeznam[] AppendixSeznam { get; set; }

        [XmlAttribute("verzePis")]
        public string TaxDeclarationVersion { get; set; } = "06.02";
    }
}
