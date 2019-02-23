// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Falconic.Skp.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class OperatorParamsDto
    {
        /// <summary>
        /// Initializes a new instance of the OperatorParamsDto class.
        /// </summary>
        public OperatorParamsDto()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the OperatorParamsDto class.
        /// </summary>
        public OperatorParamsDto(int? operatorId = default(int?), string operatorName = default(string), System.Guid? currencyId = default(System.Guid?), string languageCode = default(string), string timezoneId = default(string))
        {
            OperatorId = operatorId;
            OperatorName = operatorName;
            CurrencyId = currencyId;
            LanguageCode = languageCode;
            TimezoneId = timezoneId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "operatorId")]
        public int? OperatorId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "operatorName")]
        public string OperatorName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "currencyId")]
        public System.Guid? CurrencyId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "languageCode")]
        public string LanguageCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "timezoneId")]
        public string TimezoneId { get; set; }

    }
}
