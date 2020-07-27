using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class AppendixSummary
    {
        /// <summary>
        /// PŘÍLOHY DAP - Počet příloh celkem
        /// </summary>
        [XmlAttribute("priloh_celk")]
        public string TotalAmountOfAppendixes { get; set; }

        /// <summary>
        /// PŘÍLOHY DAP - Příloha č. 2 - "Výpočet dílčích základů daně z příjmů z nájmu (2013: pronájmu) (§ 9 zákona) a z ostatních příjmů (§ 10 zákona)"
        /// </summary>
        [XmlAttribute("priloha2")]
        public string TotalAmountOfAppendixes2 { get; set; }

        /// <summary>
        /// PŘÍLOHY DAP - Seznam pro poplatníky uplatňující nárok na vyloučení dvojího zdanění podle § 38f odst. 10 zákona
        /// </summary>
        [XmlAttribute("seznam")]
        public string TotalAmountOfAppendixesSeznam { get; set; }

        /// <summary>
        /// PŘÍLOHY DAP - Příloha č. 3 - Výpočet daně z příjmů ze zahraničí (§ 38f zákona) včetně Samostatných listů 1. oddílu
        /// </summary>
        [XmlAttribute("pril3_samlist")]
        public string TotalAmountOfAppendixes3 { get; set; }

        /// <summary>
        /// PŘÍLOHY DAP - Potvrzení o vyplacených příjmech a sražené dani (2014-2016: podle § 36 odst. 2 písm. p) (2014: nebo t)) zákona)
        /// </summary>
        [XmlAttribute("potv_36")]
        public string TotalAmountOfAppendixesEmployeeIncomeConfirmation { get; set; }


    }
}
