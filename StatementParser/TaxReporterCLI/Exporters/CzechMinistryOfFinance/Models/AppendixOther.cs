using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class AppendixOther
    {
        /// <summary>
        /// If you know what this is, please create PR or just write me an email (vladimir.aubrecht@me.com)!
        /// </summary>
        [XmlAttribute("kod_sekce")]
        public string SectionCode { get; set; } = "O";

        /// <summary>
        /// Jiné přílohy - Textová příloha
        /// </summary>
        [XmlAttribute("t_prilohy")]
        public string AppendixText { get; set; }

        /// <summary>
        /// If you know what this is, please create PR or just write me an email (vladimir.aubrecht@me.com)!
        /// </summary>
        [XmlAttribute("poradi")]
        public string Order { get; set; } = "1";
    }
}
