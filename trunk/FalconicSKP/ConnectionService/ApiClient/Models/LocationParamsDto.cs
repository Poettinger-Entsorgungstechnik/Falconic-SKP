// <auto-generated />

namespace Falconic.Skp.Api.Client.Models
{
    using Falconic.Skp;
    using Falconic.Skp.Api;
    using Falconic.Skp.Api.Client;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class LocationParamsDto
    {
        /// <summary>
        /// Initializes a new instance of the LocationParamsDto class.
        /// </summary>
        public LocationParamsDto()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the LocationParamsDto class.
        /// </summary>
        public LocationParamsDto(string locationName = default(string), System.DateTime? nightlockStart = default(System.DateTime?), System.DateTime? nightlockStop = default(System.DateTime?), double? accessLimit = default(double?), double? pricePerOneHundretKilo = default(double?), bool? emptyingIsMaintained = default(bool?))
        {
            LocationName = locationName;
            NightlockStart = nightlockStart;
            NightlockStop = nightlockStop;
            AccessLimit = accessLimit;
            PricePerOneHundretKilo = pricePerOneHundretKilo;
            EmptyingIsMaintained = emptyingIsMaintained;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "locationName")]
        public string LocationName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "nightlockStart")]
        public System.DateTime? NightlockStart { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "nightlockStop")]
        public System.DateTime? NightlockStop { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "accessLimit")]
        public double? AccessLimit { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "pricePerOneHundretKilo")]
        public double? PricePerOneHundretKilo { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "emptyingIsMaintained")]
        public bool? EmptyingIsMaintained { get; set; }

    }
}