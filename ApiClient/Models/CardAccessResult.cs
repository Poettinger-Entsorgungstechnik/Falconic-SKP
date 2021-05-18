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
    /// Defines values for CardAccessResult.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CardAccessResult
    {
        [EnumMember(Value = "Ok")]
        Ok,
        [EnumMember(Value = "CardValidButCustomerHasNoAccess")]
        CardValidButCustomerHasNoAccess,
        [EnumMember(Value = "CustomerHasNotEnoughBalance")]
        CustomerHasNotEnoughBalance,
        [EnumMember(Value = "CardNotFound")]
        CardNotFound,
        [EnumMember(Value = "CardIsLocked")]
        CardIsLocked
    }
    internal static class CardAccessResultEnumExtension
    {
        internal static string ToSerializedValue(this CardAccessResult? value)
        {
            return value == null ? null : ((CardAccessResult)value).ToSerializedValue();
        }

        internal static string ToSerializedValue(this CardAccessResult value)
        {
            switch( value )
            {
                case CardAccessResult.Ok:
                    return "Ok";
                case CardAccessResult.CardValidButCustomerHasNoAccess:
                    return "CardValidButCustomerHasNoAccess";
                case CardAccessResult.CustomerHasNotEnoughBalance:
                    return "CustomerHasNotEnoughBalance";
                case CardAccessResult.CardNotFound:
                    return "CardNotFound";
                case CardAccessResult.CardIsLocked:
                    return "CardIsLocked";
            }
            return null;
        }

        internal static CardAccessResult? ParseCardAccessResult(this string value)
        {
            switch( value )
            {
                case "Ok":
                    return CardAccessResult.Ok;
                case "CardValidButCustomerHasNoAccess":
                    return CardAccessResult.CardValidButCustomerHasNoAccess;
                case "CustomerHasNotEnoughBalance":
                    return CardAccessResult.CustomerHasNotEnoughBalance;
                case "CardNotFound":
                    return CardAccessResult.CardNotFound;
                case "CardIsLocked":
                    return CardAccessResult.CardIsLocked;
            }
            return null;
        }
    }
}