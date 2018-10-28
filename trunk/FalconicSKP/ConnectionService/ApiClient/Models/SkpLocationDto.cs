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
        public SkpLocationDto(int? locationId = default(int?), string name = default(string), string fractionName = default(string), double? latitude = default(double?), double? longitude = default(double?), bool? locationMonitoringActive = default(bool?), bool? nightLockActive = default(bool?), System.DateTime? nightLockStart = default(System.DateTime?), System.DateTime? nightLockStop = default(System.DateTime?), int? percentFullMessage = default(int?), int? percentPreFullMessage = default(int?), int? numberOfPresses = default(int?), bool? pressPosition = default(bool?), int? machineUtilization = default(int?))
        {
            LocationId = locationId;
            Name = name;
            FractionName = fractionName;
            Latitude = latitude;
            Longitude = longitude;
            LocationMonitoringActive = locationMonitoringActive;
            NightLockActive = nightLockActive;
            NightLockStart = nightLockStart;
            NightLockStop = nightLockStop;
            PercentFullMessage = percentFullMessage;
            PercentPreFullMessage = percentPreFullMessage;
            NumberOfPresses = numberOfPresses;
            PressPosition = pressPosition;
            MachineUtilization = machineUtilization;
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
        [JsonProperty(PropertyName = "fractionName")]
        public string FractionName { get; set; }

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
        [JsonProperty(PropertyName = "nightLockActive")]
        public bool? NightLockActive { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "nightLockStart")]
        public System.DateTime? NightLockStart { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "nightLockStop")]
        public System.DateTime? NightLockStop { get; set; }

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

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "machineUtilization")]
        public int? MachineUtilization { get; set; }

    }
}
