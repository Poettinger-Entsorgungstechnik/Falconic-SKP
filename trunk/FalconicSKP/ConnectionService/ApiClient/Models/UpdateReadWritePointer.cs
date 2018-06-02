// <auto-generated />

namespace Falconic.Skp.Api.Client.Models
{
    using Falconic.Skp;
    using Falconic.Skp.Api;
    using Falconic.Skp.Api.Client;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class UpdateReadWritePointer
    {
        /// <summary>
        /// Initializes a new instance of the UpdateReadWritePointer class.
        /// </summary>
        public UpdateReadWritePointer()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the UpdateReadWritePointer class.
        /// </summary>
        public UpdateReadWritePointer(int readPointer, int writePointer)
        {
            ReadPointer = readPointer;
            WritePointer = writePointer;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "readPointer")]
        public int ReadPointer { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "writePointer")]
        public int WritePointer { get; set; }

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
