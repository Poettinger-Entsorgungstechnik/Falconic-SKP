// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Falconic.Skp.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class StoreContainerHardwareInformation
    {
        /// <summary>
        /// Initializes a new instance of the StoreContainerHardwareInformation
        /// class.
        /// </summary>
        public StoreContainerHardwareInformation()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the StoreContainerHardwareInformation
        /// class.
        /// </summary>
        public StoreContainerHardwareInformation(int operatingMinutes, int numberOfStartings, int gsmSignalStrength, System.DateTime timestamp, string dataConnection = default(string), string firmwareVersion = default(string))
        {
            OperatingMinutes = operatingMinutes;
            NumberOfStartings = numberOfStartings;
            GsmSignalStrength = gsmSignalStrength;
            DataConnection = dataConnection;
            FirmwareVersion = firmwareVersion;
            Timestamp = timestamp;
            CustomInit();
        }
        /// <summary>
        /// Static constructor for StoreContainerHardwareInformation class.
        /// </summary>
        static StoreContainerHardwareInformation()
        {
            FirmwareType = "Presscontrol";
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "operatingMinutes")]
        public int OperatingMinutes { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "numberOfStartings")]
        public int NumberOfStartings { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "gsmSignalStrength")]
        public int GsmSignalStrength { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "dataConnection")]
        public string DataConnection { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "firmwareVersion")]
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        public System.DateTime Timestamp { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "firmwareType")]
        public static string FirmwareType { get; private set; }

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
