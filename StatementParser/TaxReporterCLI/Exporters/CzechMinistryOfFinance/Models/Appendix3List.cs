using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    public class Appendix3List
    {
        /// <summary>
        /// Příjmy ze zdrojů v zahraničí - metoda zápočtu daně zaplacené v zahraničí - Koeficient zápočtu (ř. 321 - ř. 322) děleno ř. 42 výsledek vynásobte stem
        /// </summary>
        /// <remarks>Row 324.</remarks>
        [XmlAttribute("proczahr")]
        public decimal Coeficient { get; set; }

        /// <summary>
        /// Příjmy ze zdrojů v zahraničí - metoda zápočtu daně zaplacené v zahraničí - Daň uznaná k zápočtu (ř. 323 maximálně však do výše ř. 325)
        /// </summary>
        /// <remarks>Row 326.</remarks>
        [XmlAttribute("da_uznzap")]
        public decimal MaximumRecognizedTax { get; set; }

        /// <summary>
        /// Příjmy ze zdrojů v zahraničí - metoda zápočtu daně zaplacené v zahraničí - Daň zaplacená v zahraničípokyny k vyplnění
        /// </summary>
        /// <remarks>Row 323.</remarks>
        [XmlAttribute("da_zahr")]
        public decimal Tax { get; set; }

        /// <summary>
        /// Příjmy ze zdrojů v zahraničí - metoda zápočtu daně zaplacené v zahraničí - Příjmy ze zdrojů v zahraničí, u nichž se použije metoda zápočtu
        /// </summary>
        /// <remarks>Row 321.</remarks>
        [XmlAttribute("kc_prijzap")]
        public decimal Income { get; set; }

        /// <summary>
        /// Příjmy ze zdrojů v zahraničí - metoda zápočtu daně zaplacené v zahraničí - Rozdíl řádků
        /// </summary>
        /// <remarks>Row 327.</remarks>
        [XmlAttribute("roz_od12")]
        public decimal UnrecognisedTax { get; set; }

        /// <summary>
        /// Příjmy ze zdrojů v zahraničí - metoda zápočtu daně zaplacené v zahraničí - Daň uznaná k zápočtu
        /// </summary>
        /// <remarks>Row 326.</remarks>
        [XmlAttribute("kc_k_zapzahr")]
        public decimal RecognizedTax { get; set; }

        /// <summary>
        /// Příjmy ze zdrojů v zahraničí - metoda zápočtu daně zaplacené v zahraničí - Kód státu
        /// </summary>
        [XmlAttribute("kod_statu")]
        public string Country { get; set; }

        /// <summary>
        /// Příjmy ze zdrojů v zahraničí - metoda zápočtu daně zaplacené v zahraničí - Výdaje
        /// </summary>
        /// <remarks>Row 322.</remarks>
        [XmlAttribute("kc_vydzap")]
        public decimal Expenses { get; set; }
    }
}
