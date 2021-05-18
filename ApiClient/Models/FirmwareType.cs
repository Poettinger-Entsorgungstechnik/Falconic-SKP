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
    /// Defines values for FirmwareType.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FirmwareType
    {
        [EnumMember(Value = "Presscontrol")]
        Presscontrol
    }
    internal static class FirmwareTypeEnumExtension
    {
        internal static string ToSerializedValue(this FirmwareType? value)
        {
            return value == null ? null : ((FirmwareType)value).ToSerializedValue();
        }

        internal static string ToSerializedValue(this FirmwareType value)
        {
            switch( value )
            {
                case FirmwareType.Presscontrol:
                    return "Presscontrol";
            }
            return null;
        }

        internal static FirmwareType? ParseFirmwareType(this string value)
        {
            switch( value )
            {
                case "Presscontrol":
                    return FirmwareType.Presscontrol;
            }
            return null;
        }
    }
}