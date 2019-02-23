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

    public partial class SkpContainerFeaturesDto
    {
        /// <summary>
        /// Initializes a new instance of the SkpContainerFeaturesDto class.
        /// </summary>
        public SkpContainerFeaturesDto()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the SkpContainerFeaturesDto class.
        /// </summary>
        public SkpContainerFeaturesDto(IDictionary<string, string> singleValue = default(IDictionary<string, string>), IDictionary<string, IList<string>> multipleValue = default(IDictionary<string, IList<string>>))
        {
            SingleValue = singleValue;
            MultipleValue = multipleValue;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "singleValue")]
        public IDictionary<string, string> SingleValue { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "multipleValue")]
        public IDictionary<string, IList<string>> MultipleValue { get; set; }

    }
}
