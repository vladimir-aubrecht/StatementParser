
using TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance
{
    public class TaxDeclarationBuilder
    {

        public Declarations Build()
        {
            return new Declarations()
            {
                TaxDeclaration = new TaxDeclaration()
                {
                    Taxes = new Taxes()
                    {
                        IsAudit = "N",
                        DidTransactionsWithInternationalPeople = "N",
                        IsPowerOfAttorney = "N",
                        Year = "2019",
                        TaxPeriodStart = "01.01.2019",
                        TaxPeriodEnds = "31.12.2019",
                        TaxToPay = "88918",
                        BasicDiscountForTaxPayer = "24840",
                        TotalTaxAfterDiscounts = "434077",
                        SumOfTaxDiscounts = "24840",
                        SolidarityTax = "43934.87",
                        PayedTax = "345159",
                        TaxAfterDiscounts = "434077",
                        DestinationMinistryOfFinanceId = "451",
                        TotalTax = "458916.54",
                        TaxDeclarationType = "B",
                        TaxLostRounded = "0",
                        TotalTaxRounded = "458917"
                    },
                    PersonalInformation = new PersonalInformation()
                    {
                        BirthLastname = "Aubrecht",
                        BirthNumber = "0000000000",
                        City = "PRAHA 4",
                        CurrentCityId = "500119",
                        TaxYearCityId = "500119",
                        TaxYearLastDayCityId = "500119",
                        CurrentCountry = "ČESKÁ REPUBLIKA",
                        CurrentEmail = "vladimir.aubrecht@me.com",
                        TaxYearEmail = "vladimir.aubrecht@me.com",
                        CurrentFaxNumber = null,
                        TaxYearFaxNumber = null,
                        CurrentPhone = "+420606000000",
                        TaxYearPhone = "+420606000000",
                        CurrentStreet = "Ulice",
                        TaxYearStreet = "Ulice",
                        TaxYearLastDayStreet = "Ulice",
                        Firstname = "Vladimír",
                        Lastname = "Aubrecht",
                        CurrentZip = "14000",
                        TaxYearLastDayZip = "14000",
                        TaxYearZip = "14000",
                        TaxCountry = "CZ",
                        Nationality = "ČR",
                        TaxNumber = null,
                        TaxYearCity = "PRAHA 4",
                        TaxYearLastDayCity = "PRAHA 4",
                        PassportNumber = null,
                        Title = "Ing.",
                        TaxYearLastDayStreetDescriptionNumber = "000",
                        CurrentStreetDescriptionNumber = "000",
                        TaxYearStreetDescriptionNumber = "000",
                        CurrentStreetNumber = "000",
                        TaxYearLastDayStreetNumber = "000",
                        TaxYearStreetNumber = "000",
                        MinistryOfFinanceId = "2004"

                    },
                    Appendix2 = new Appendix2[]
                    {
                        new Appendix2() { Code = "Z", Type = "D", Description = "Příjmy z prodeje indexů", Expenses = "3919", Income = "20500", Profit = "16581" }
                    },
                    AppendixIncomeTable = new AppendixIncomeTable[]
                    {
                        new AppendixIncomeTable() { Income = "1791494", Insurance = "552066", TakenDeposit = "345159" },
                        new AppendixIncomeTable() { Income = "378327" },
                        new AppendixIncomeTable() { Income = "27372" }
                    },
                    Appendix3 = new Appendix3[]
                    {
                        new Appendix3() { Coeficient = 0.01, Country = "PL", Income = 263, MaximumRecognizedTax = 46.33M, RecognizedTax = 46.33M, Tax = 50, UnrecognisedTax = 3.67M }
                    },
                    AppendixSeznam = new AppendixSeznam[]
                    {
                        new AppendixSeznam() { BrokerName = "LYNX B.V.", Country = "PL", IncomeInCZK = 263, TaxInCZK = 50, TaxInOriginalCurrency = 8 },
                        new AppendixSeznam() { BrokerName = "RaiffeisenBank", Country = "US", IncomeInCZK = 4932, TaxInCZK = 740, TaxInOriginalCurrency = 32 }
                    }
                }
            };
        }
    }
}
