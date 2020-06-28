
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
                    PersonalInformation = new PersonalInformation(),
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
