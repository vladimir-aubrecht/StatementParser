namespace ExchangeRateProvider.Models
{
    public interface ICurrencyList
    {
        bool IsEmpty { get; }

        CurrencyDescriptor this[string currencyCode] { get; }
    }
}