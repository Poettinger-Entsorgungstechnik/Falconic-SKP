// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Falconic.Skp.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class ContainerHardwareInformationDto
    {
        /// <summary>
        /// Initializes a new instance of the ContainerHardwareInformationDto
        /// class.
        /// </summary>
        public ContainerHardwareInformationDto()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ContainerHardwareInformationDto
        /// class.
        /// </summary>
        /// <param name="firmwareType">Possible values include:
        /// 'Presscontrol'</param>
        public ContainerHardwareInformationDto(int? operatingMinutes = default(int?), int? numberOfStartings = default(int?), int? gsmSignalStrength = default(int?), string dataConnection = default(string), FirmwareType? firmwareType = default(FirmwareType?), string firmwareVersion = default(string), string targetFirmwareVersion = default(string), System.DateTime? targetFirmwareUpdateDate = default(System.DateTime?))
        {
            OperatingMinutes = operatingMinutes;
            NumberOfStartings = numberOfStartings;
            GsmSignalStrength = gsmSignalStrength;
            DataConnection = dataConnection;
            FirmwareType = firmwareType;
            FirmwareVersion = firmwareVersion;
            TargetFirmwareVersion = targetFirmwareVersion;
            TargetFirmwareUpdateDate = targetFirmwareUpdateDate;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "operatingMinutes")]
        public int? OperatingMinutes { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "numberOfStartings")]
        public int? NumberOfStartings { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "gsmSignalStrength")]
        public int? GsmSignalStrength { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "dataConnection")]
        public string DataConnection { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Presscontrol'
        /// </summary>
        [JsonProperty(PropertyName = "firmwareType")]
        public FirmwareType? FirmwareType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "firmwareVersion")]
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "targetFirmwareVersion")]
        public string TargetFirmwareVersion { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "targetFirmwareUpdateDate")]
        public System.DateTime? TargetFirmwareUpdateDate { get; set; }

    }
}
