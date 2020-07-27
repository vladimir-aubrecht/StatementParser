using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class Appendix1
    {
        /// <summary>
        /// Výpočet dílčího základu daně z příjmů ze samostatné činnosti (§ 7 zákona) - Uplatňuji výdaje procentem z příjmů
        /// </summary>
        [XmlAttribute("vyd7proc")]
        public string ApplyForExpensesGivenByFixedPercentage { get; set; }
    }
}
