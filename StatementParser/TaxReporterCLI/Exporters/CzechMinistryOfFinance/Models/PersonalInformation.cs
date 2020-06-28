using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class PersonalInformation
    {
        [XmlAttribute("ulice")]
        public string CurrentStreet { get; set; }

        [XmlAttribute("psc")]
        public string CurrentZip { get; set; }

        [XmlAttribute("email")]
        public string CurrentEmail { get; set; }

        [XmlAttribute("z_ulice")]
        public string TaxYearStreet { get; set; }

        [XmlAttribute("krok_c_obce")]
        public string TaxYearLastDayCityId { get; set; }

        [XmlAttribute("z_email")]
        public string TaxYearEmail { get; set; }

        [XmlAttribute("k_stat")]
        public string TaxCountry { get; set; }

        [XmlAttribute("krok_c_pop")]
        public string TaxYearLastDayStreetDescriptionNumber { get; set; }

        /// <remarks>
        /// Row 2.
        /// </remarks>
        [XmlAttribute("rod_c")]
        public string BirthNumber { get; set; }

        [XmlAttribute("c_pasu")]
        public string PassportNumber { get; set; }

        [XmlAttribute("titul")]
        public string Title { get; set; }

        [XmlAttribute("rodnepr")]
        public string BirthLastname { get; set; }

        [XmlAttribute("krok_psc")]
        public string TaxYearLastDayZip { get; set; }

        [XmlAttribute("c_pop")]
        public string CurrentStreetDescriptionNumber { get; set; }

        [XmlAttribute("z_psc")]
        public string TaxYearZip { get; set; }

        [XmlAttribute("c_obce")]
        public string CurrentCityId { get; set; }

        [XmlAttribute("c_orient")]
        public string CurrentStreetNumber { get; set; }

        [XmlAttribute("z_c_obce")]
        public string TaxYearCityId { get; set; }

        [XmlAttribute("c_telef")]
        public string CurrentPhone { get; set; }

        [XmlAttribute("z_naz_obce")]
        public string TaxYearCity { get; set; }

        [XmlAttribute("krok_naz_obce")]
        public string TaxYearLastDayCity { get; set; }

        [XmlAttribute("c_pracufo")]
        public string MinistryOfFinanceId { get; set; }

        [XmlAttribute("prijmeni")]
        public string Lastname { get; set; }

        [XmlAttribute("z_c_faxu")]
        public string TaxYearFaxNumber { get; set; }

        [XmlAttribute("z_c_telef")]
        public string TaxYearPhone { get; set; }

        [XmlAttribute("c_faxu")]
        public string CurrentFaxNumber { get; set; }

        [XmlAttribute("z_c_pop")]
        public string TaxYearStreetDescriptionNumber { get; set; }

        [XmlAttribute("stat")]
        public string CurrentCountry { get; set; }

        /// <remarks>
        /// Row 1.
        /// </remarks>
        [XmlAttribute("dic")]
        public string TaxNumber { get; set; }

        [XmlAttribute("krok_ulice")]
        public string TaxYearLastDayStreet { get; set; }

        [XmlAttribute("st_prislus")]
        public string Nationality { get; set; }

        [XmlAttribute("naz_obce")]
        public string City { get; set; }

        [XmlAttribute("z_c_orient")]
        public string TaxYearStreetNumber { get; set; }

        [XmlAttribute("krok_c_orient")]
        public string TaxYearLastDayStreetNumber { get; set; }

        [XmlAttribute("jmeno")]
        public string Firstname { get; set; }
    }
}
