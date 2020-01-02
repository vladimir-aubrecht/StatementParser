using System;
namespace ExchangeRateProvider.Models
{
    public struct CurrencyDescriptor
    {
        public readonly string Code;
        public readonly string Name;
        public readonly decimal Price;
        public readonly string Country;

        public CurrencyDescriptor(string code, string name, decimal price, string country)
        {
            this.Code = code;
            this.Name = name;
            this.Price = price;
            this.Country = country;
        }

        public CurrencyDescriptor(string code, string name, decimal price, decimal amount, string country) : this(code, name, price / amount, country)
        {
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3}", this.Code, this.Name, this.Price, this.Country);
        }

        public static bool operator ==(CurrencyDescriptor cd1, CurrencyDescriptor cd2)
        {
            return cd1.Code == cd2.Code;
        }

        public static bool operator !=(CurrencyDescriptor cd1, CurrencyDescriptor cd2)
        {
            return cd1.Code != cd2.Code;
        }
    }
}
