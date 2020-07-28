using System.ComponentModel;

namespace StatementParser.Models
{
    public enum Broker
    {
        [Description("Morgan Stanley Smith Barney LLC")]
        MorganStanley,

        [Description("Fidelity Stock Plan Services")]
        Fidelity,

        [Description("FxChoice Limited")]
        FxChoice,

        [Description("Degiro")]
        Degiro,

        [Description("LYNX B.V.")]
        Lynx,

        [Description("Revolut Ltd")]
        Revolut
    }
}
