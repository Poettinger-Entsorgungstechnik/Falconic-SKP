// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Falconic.Skp.Api.Client.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines values for BalanceMode.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BalanceMode
    {
        [EnumMember(Value = "OnlineBalance")]
        OnlineBalance,
        [EnumMember(Value = "OfflineBalance")]
        OfflineBalance
    }
    internal static class BalanceModeEnumExtension
    {
        internal static string ToSerializedValue(this BalanceMode? value)
        {
            return value == null ? null : ((BalanceMode)value).ToSerializedValue();
        }

        internal static string ToSerializedValue(this BalanceMode value)
        {
            switch( value )
            {
                case BalanceMode.OnlineBalance:
                    return "OnlineBalance";
                case BalanceMode.OfflineBalance:
                    return "OfflineBalance";
            }
            return null;
        }

        internal static BalanceMode? ParseBalanceMode(this string value)
        {
            switch( value )
            {
                case "OnlineBalance":
                    return BalanceMode.OnlineBalance;
                case "OfflineBalance":
                    return BalanceMode.OfflineBalance;
            }
            return null;
        }
    }
}
