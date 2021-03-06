// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Falconic.Skp.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class StatusMessageDto
    {
        /// <summary>
        /// Initializes a new instance of the StatusMessageDto class.
        /// </summary>
        public StatusMessageDto()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the StatusMessageDto class.
        /// </summary>
        public StatusMessageDto(int code, System.DateTime timestamp, bool shouldNotifyUser, bool includedInTransactions, double? fillState = default(double?))
        {
            Code = code;
            FillState = fillState;
            Timestamp = timestamp;
            ShouldNotifyUser = shouldNotifyUser;
            IncludedInTransactions = includedInTransactions;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "fillState")]
        public double? FillState { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        public System.DateTime Timestamp { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "shouldNotifyUser")]
        public bool ShouldNotifyUser { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "includedInTransactions")]
        public bool IncludedInTransactions { get; set; }

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
