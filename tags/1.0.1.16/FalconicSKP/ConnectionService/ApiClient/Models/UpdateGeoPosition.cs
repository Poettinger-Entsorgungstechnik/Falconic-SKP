// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Falconic.Skp.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class UpdateGeoPosition
    {
        /// <summary>
        /// Initializes a new instance of the UpdateGeoPosition class.
        /// </summary>
        public UpdateGeoPosition()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the UpdateGeoPosition class.
        /// </summary>
        public UpdateGeoPosition(double? latitude = default(double?), double? longitude = default(double?))
        {
            Latitude = latitude;
            Longitude = longitude;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "latitude")]
        public double? Latitude { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "longitude")]
        public double? Longitude { get; set; }

    }
}
