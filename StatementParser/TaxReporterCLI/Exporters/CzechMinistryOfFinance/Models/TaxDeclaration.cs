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

        [XmlElement("VetaS")]
        public Section3 Section3 { get; set; }

        [XmlElement("VetaB")]
        public AppendixSummary AppendixSummary { get; set; }

        [XmlElement("VetaT")]
        public Appendix1 Appendix1 { get; set; }

        [XmlElement("VetaV")]
        public Appendix2 Appendix2 { get; set; }

        [XmlElement("VetaJ")]
        public Appendix2OtherIncomeRow[] Appendix2OtherIncomeRow { get; set; }

        [XmlElement("VetaW")]
        public Appendix3 Appendix3 { get; set; }

        [XmlElement("VetaR")]
        public AppendixOther AppendixOther { get; set; }

        [XmlElement("VetaL")]
        public Appendix3List[] Appendix3Lists { get; set; }

        [XmlElement("Vetab")]
        public AppendixIncomeTableRow[] AppendixIncomeTable { get; set; }

        [XmlElement("Vetad")]
        public AppendixSeznamRow[] AppendixSeznam { get; set; }

        [XmlAttribute("verzePis")]
        public string TaxDeclarationVersion { get; set; } = "06.02";
    }
}
