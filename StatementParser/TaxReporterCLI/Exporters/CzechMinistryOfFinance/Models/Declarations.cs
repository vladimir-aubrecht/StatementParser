using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    [XmlRoot(ElementName = "Pisemnost")]
    public class Declaration
    {
        [XmlElement("DPFDP5")]
        public TaxDeclaration TaxDeclaration { get; set; }

        [XmlAttribute("nazevSW")]
        public string SoftwareName { get; set; } = "EPO MF ČR";

        [XmlAttribute("verzeSW")]
        public string SoftwareVersion { get; set; } = "41.10.6";
    }
}
