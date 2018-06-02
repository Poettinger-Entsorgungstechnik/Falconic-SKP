// <auto-generated />

namespace Falconic.Skp.Api.Client.Models
{
    using Falconic.Skp;
    using Falconic.Skp.Api;
    using Falconic.Skp.Api.Client;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class SkpLocationDto
    {
        /// <summary>
        /// Initializes a new instance of the SkpLocationDto class.
        /// </summary>
        public SkpLocationDto()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the SkpLocationDto class.
        /// </summary>
        public SkpLocationDto(int? locationId = default(int?), string name = default(string), int? locationTypeId = default(int?), double? latitude = default(double?), double? longitude = default(double?), bool? locationMonitoringActive = default(bool?), System.DateTime? nightlockStart = default(System.DateTime?), System.DateTime? nightlockStop = default(System.DateTime?), int? percentFullMessage = default(int?), int? percentPreFullMessage = default(int?), int? numberOfPresses = default(int?), bool? pressPosition = default(bool?))
        {
            LocationId = locationId;
            Name = name;
            LocationTypeId = locationTypeId;
            Latitude = latitude;
            Longitude = longitude;
            LocationMonitoringActive = locationMonitoringActive;
            NightlockStart = nightlockStart;
            NightlockStop = nightlockStop;
            PercentFullMessage = percentFullMessage;
            PercentPreFullMessage = percentPreFullMessage;
            NumberOfPresses = numberOfPresses;
            PressPosition = pressPosition;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "locationId")]
        public int? LocationId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "locationTypeId")]
        public int? LocationTypeId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "latitude")]
        public double? Latitude { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "longitude")]
        public double? Longitude { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "locationMonitoringActive")]
        public bool? LocationMonitoringActive { get; set; }

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
        [JsonProperty(PropertyName = "percentFullMessage")]
        public int? PercentFullMessage { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "percentPreFullMessage")]
        public int? PercentPreFullMessage { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "numberOfPresses")]
        public int? NumberOfPresses { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "pressPosition")]
        public bool? PressPosition { get; set; }

    }
}
