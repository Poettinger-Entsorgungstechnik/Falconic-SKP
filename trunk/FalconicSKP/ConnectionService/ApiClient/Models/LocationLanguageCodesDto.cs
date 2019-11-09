// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Falconic.Skp.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class LocationLanguageCodesDto
    {
        /// <summary>
        /// Initializes a new instance of the LocationLanguageCodesDto class.
        /// </summary>
        public LocationLanguageCodesDto()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the LocationLanguageCodesDto class.
        /// </summary>
        public LocationLanguageCodesDto(int calculatedLanguageCode, IList<string> languageCodes = default(IList<string>))
        {
            CalculatedLanguageCode = calculatedLanguageCode;
            LanguageCodes = languageCodes;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "calculatedLanguageCode")]
        public int CalculatedLanguageCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "languageCodes")]
        public IList<string> LanguageCodes { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
        }
    }
}
