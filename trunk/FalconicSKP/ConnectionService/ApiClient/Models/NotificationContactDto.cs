// <auto-generated />

namespace Falconic.Skp.Api.Client.Models
{
    using Falconic.Skp;
    using Falconic.Skp.Api;
    using Falconic.Skp.Api.Client;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class NotificationContactDto
    {
        /// <summary>
        /// Initializes a new instance of the NotificationContactDto class.
        /// </summary>
        public NotificationContactDto()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the NotificationContactDto class.
        /// </summary>
        /// <param name="contactMethod">Possible values include: 'Email',
        /// 'Sms'</param>
        public NotificationContactDto(string contact = default(string), SkpContactMethod? contactMethod = default(SkpContactMethod?), int? statusMessageId = default(int?), int? statusGroupId = default(int?), int? statusTypeId = default(int?))
        {
            Contact = contact;
            ContactMethod = contactMethod;
            StatusMessageId = statusMessageId;
            StatusGroupId = statusGroupId;
            StatusTypeId = statusTypeId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "contact")]
        public string Contact { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Email', 'Sms'
        /// </summary>
        [JsonProperty(PropertyName = "contactMethod")]
        public SkpContactMethod? ContactMethod { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "statusMessageId")]
        public int? StatusMessageId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "statusGroupId")]
        public int? StatusGroupId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "statusTypeId")]
        public int? StatusTypeId { get; set; }

    }
}
