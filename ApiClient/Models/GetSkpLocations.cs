// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Falconic.Skp.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class GetSkpLocations
    {
        /// <summary>
        /// Initializes a new instance of the GetSkpLocations class.
        /// </summary>
        public GetSkpLocations()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the GetSkpLocations class.
        /// </summary>
        public GetSkpLocations(double minLatitude, double maxLatitude, double minLongitude, double maxLongitude)
        {
            MinLatitude = minLatitude;
            MaxLatitude = maxLatitude;
            MinLongitude = minLongitude;
            MaxLongitude = maxLongitude;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "minLatitude")]
        public double MinLatitude { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "maxLatitude")]
        public double MaxLatitude { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "minLongitude")]
        public double MinLongitude { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "maxLongitude")]
        public double MaxLongitude { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            //Nothing to validate
        }
    }
}
