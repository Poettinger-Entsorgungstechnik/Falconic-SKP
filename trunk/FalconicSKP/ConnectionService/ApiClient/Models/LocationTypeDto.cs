// <auto-generated />

namespace Falconic.Skp.Api.Client.Models
{
    using Falconic.Skp;
    using Falconic.Skp.Api;
    using Falconic.Skp.Api.Client;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class LocationTypeDto
    {
        /// <summary>
        /// Initializes a new instance of the LocationTypeDto class.
        /// </summary>
        public LocationTypeDto()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the LocationTypeDto class.
        /// </summary>
        public LocationTypeDto(System.Guid? id = default(System.Guid?), int? logicalId = default(int?), string name = default(string))
        {
            Id = id;
            LogicalId = logicalId;
            Name = name;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public System.Guid? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "logicalId")]
        public int? LogicalId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

    }
}
