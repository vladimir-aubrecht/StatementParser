using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class PersonalInformation
    {
        /// <summary>
        /// Adresa místa pobytu v den podání DAP - Ulice/část obce
        /// </summary>
        [XmlAttribute("ulice")]
        public string CurrentStreet { get; set; }

        /// <summary>
        /// Adresa místa pobytu v den podání DAP - PSC
        /// </summary>
        [XmlAttribute("psc")]
        public string CurrentZip { get; set; }

        /// <summary>
        /// Kontaktní informace - E-mail
        /// </summary>
        [XmlAttribute("email")]
        public string CurrentEmail { get; set; }

        /// <summary>
        /// Adresa místa pobytu na území České republiky, kde se poplatník obvykle ve zdaňovacím období zdržoval - Ulice/část obce
        /// </summary>
        [XmlAttribute("z_ulice")]
        public string TaxYearStreet { get; set; }

        /// <summary>
        /// Adresa místa pobytu k poslednímu dni kalendářního roku, za který se daň vyměřuje - Obec / Městská část Id
        /// </summary>
        [XmlAttribute("krok_c_obce")]
        public string TaxYearLastDayCityId { get; set; }

        /// <summary>
        /// Adresa místa pobytu na území České republiky, kde se poplatník obvykle ve zdaňovacím období zdržoval - E-mail
        /// </summary>
        [XmlAttribute("z_email")]
        public string TaxYearEmail { get; set; }

        /// <summary>
        /// Adresa místa pobytu v den podání DAP - Stát
        /// </summary>
        [XmlAttribute("k_stat")]
        public string TaxCountry { get; set; }

        /// <summary>
        /// Adresa místa pobytu k poslednímu dni kalendářního roku, za který se daň vyměřuje - Číslo popisné
        /// </summary>
        [XmlAttribute("krok_c_pop")]
        public string TaxYearLastDayStreetDescriptionNumber { get; set; }

        /// <summary>
        /// Identifikace daňového subjektu - Rodné číslo
        /// </summary>
        /// <remarks>
        /// Row 2.
        /// </remarks>
        [XmlAttribute("rod_c")]
        public string BirthNumber { get; set; }

        /// <summary>
        /// Identifikace daňového subjektu - Číslo pasu
        /// </summary>
        [XmlAttribute("c_pasu")]
        public string PassportNumber { get; set; }

        /// <summary>
        /// Identifikace daňového subjektu - Titul
        /// </summary>
        [XmlAttribute("titul")]
        public string Title { get; set; }

        /// <summary>
        /// Identifikace daňového subjektu - Rodné příjmení
        /// </summary>
        [XmlAttribute("rodnepr")]
        public string BirthLastname { get; set; }

        /// <summary>
        /// Adresa místa pobytu k poslednímu dni kalendářního roku, za který se daň vyměřuje - PSČ
        /// </summary>
        [XmlAttribute("krok_psc")]
        public string TaxYearLastDayZip { get; set; }

        /// <summary>
        /// Adresa místa pobytu v den podání DAP - Číslo popisné
        /// </summary>
        [XmlAttribute("c_pop")]
        public string CurrentStreetDescriptionNumber { get; set; }

        /// <summary>
        /// Adresa místa pobytu na území České republiky, kde se poplatník obvykle ve zdaňovacím období zdržoval - PSČ
        /// </summary>
        [XmlAttribute("z_psc")]
        public string TaxYearZip { get; set; }

        /// <summary>
        /// Adresa místa pobytu v den podání DAP - Obec / Městská část
        /// </summary>
        [XmlAttribute("c_obce")]
        public string CurrentCityId { get; set; }

        /// <summary>
        /// Adresa místa pobytu v den podání DAP - Číslo orientační
        /// </summary>
        [XmlAttribute("c_orient")]
        public string CurrentStreetNumber { get; set; }

        /// <summary>
        /// Adresa místa pobytu na území České republiky, kde se poplatník obvykle ve zdaňovacím období zdržoval - Obec / Městská část
        /// </summary>
        [XmlAttribute("z_c_obce")]
        public string TaxYearCityId { get; set; }

        /// <summary>
        /// Kontaktní informace - Telefon/mobilní telefon
        /// </summary>
        [XmlAttribute("c_telef")]
        public string CurrentPhone { get; set; }

        /// <summary>
        /// Adresa místa pobytu na území České republiky, kde se poplatník obvykle ve zdaňovacím období zdržoval - Obec / Městská část
        /// </summary>
        [XmlAttribute("z_naz_obce")]
        public string TaxYearCity { get; set; }

        /// <summary>
        /// Adresa místa pobytu k poslednímu dni kalendářního roku, za který se daň vyměřuje - Obec / Městská část
        /// </summary>
        [XmlAttribute("krok_naz_obce")]
        public string TaxYearLastDayCity { get; set; }

        /// <summary>
        /// PŘIZNÁNÍ k dani z příjmů fyzických osob - Územní pracoviště v, ve, pro 
        /// </summary>
        [XmlAttribute("c_pracufo")]
        public string MinistryOfFinanceId { get; set; }

        /// <summary>
        /// Identifikace daňového subjektu - Příjmení
        /// </summary>
        [XmlAttribute("prijmeni")]
        public string Lastname { get; set; }

        /// <summary>
        /// Adresa místa pobytu na území České republiky, kde se poplatník obvykle ve zdaňovacím období zdržoval - Telefon/mobilní telefon
        /// </summary>
        [XmlAttribute("z_c_telef")]
        public string TaxYearPhone { get; set; }

        /// <summary>
        /// Adresa místa pobytu na území České republiky, kde se poplatník obvykle ve zdaňovacím období zdržoval - Číslo popisné
        /// </summary>
        [XmlAttribute("z_c_pop")]
        public string TaxYearStreetDescriptionNumber { get; set; }

        /// <summary>
        /// Adresa místa pobytu v den podání DAP - Stát
        /// </summary>
        [XmlAttribute("stat")]
        public string CurrentCountry { get; set; }

        /// <summary>
        /// Identifikace daňového subjektu - Daňové identifikační číslo
        /// </summary>
        /// <remarks>
        /// Row 1.
        /// </remarks>
        [XmlAttribute("dic")]
        public string TaxNumber { get; set; }

        /// <summary>
        /// Adresa místa pobytu k poslednímu dni kalendářního roku, za který se daň vyměřuje - Ulice/část obce
        /// </summary>
        [XmlAttribute("krok_ulice")]
        public string TaxYearLastDayStreet { get; set; }

        /// <summary>
        /// Identifikace daňového subjektu - Státní příslušnost
        /// </summary>
        [XmlAttribute("st_prislus")]
        public string Nationality { get; set; }

        /// <summary>
        /// Adresa místa pobytu v den podání DAP - Obec / Městská část
        /// </summary>
        [XmlAttribute("naz_obce")]
        public string City { get; set; }

        /// <summary>
        /// Adresa místa pobytu na území České republiky, kde se poplatník obvykle ve zdaňovacím období zdržoval - Číslo orientační
        /// </summary>
        [XmlAttribute("z_c_orient")]
        public string TaxYearStreetNumber { get; set; }

        /// <summary>
        /// Adresa místa pobytu k poslednímu dni kalendářního roku, za který se daň vyměřuje - Číslo orientační
        /// </summary>
        [XmlAttribute("krok_c_orient")]
        public string TaxYearLastDayStreetNumber { get; set; }

        /// <summary>
        /// Adresa místa pobytu na území České republiky, kde se poplatník obvykle ve zdaňovacím období zdržoval - Fax
        /// </summary>
        [XmlAttribute("z_c_faxu")]
        public string TaxYearFaxNumber { get; set; }

        /// <summary>
        /// Kontaktní informace - Fax
        /// </summary>
        [XmlAttribute("c_faxu")]
        public string CurrentFaxNumber { get; set; }

        /// <summary>
        /// Identifikace daňového subjektu - Jméno
        /// </summary>
        [XmlAttribute("jmeno")]
        public string Firstname { get; set; }
    }
}
