// <auto-generated />

namespace Falconic.Skp.Api.Client.Models
{
    using Falconic.Skp;
    using Falconic.Skp.Api;
    using Falconic.Skp.Api.Client;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class LastCommunicationResult
    {
        /// <summary>
        /// Initializes a new instance of the LastCommunicationResult class.
        /// </summary>
        public LastCommunicationResult()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the LastCommunicationResult class.
        /// </summary>
        public LastCommunicationResult(System.DateTime? lastCommunication = default(System.DateTime?))
        {
            LastCommunication = lastCommunication;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "lastCommunication")]
        public System.DateTime? LastCommunication { get; set; }

    }
}
