// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Falconic.Skp.Api.Client.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class CreateInsertionTransactionByCustomerNumber
    {
        /// <summary>
        /// Initializes a new instance of the
        /// CreateInsertionTransactionByCustomerNumber class.
        /// </summary>
        public CreateInsertionTransactionByCustomerNumber()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// CreateInsertionTransactionByCustomerNumber class.
        /// </summary>
        public CreateInsertionTransactionByCustomerNumber(string customerNumber, System.DateTime timestamp, int operatorId, int containerId, int locationId, int statusMessageCode, double weightInKilo, int durationInSeconds, string cardUuid = default(string), string alibiStorageNumber = default(string), double? currentBalance = default(double?), double? newCurrentBalance = default(double?), double? chargedAmount = default(double?))
        {
            CustomerNumber = customerNumber;
            CardUuid = cardUuid;
            Timestamp = timestamp;
            OperatorId = operatorId;
            ContainerId = containerId;
            LocationId = locationId;
            StatusMessageCode = statusMessageCode;
            WeightInKilo = weightInKilo;
            DurationInSeconds = durationInSeconds;
            AlibiStorageNumber = alibiStorageNumber;
            CurrentBalance = currentBalance;
            NewCurrentBalance = newCurrentBalance;
            ChargedAmount = chargedAmount;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "customerNumber")]
        public string CustomerNumber { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "cardUuid")]
        public string CardUuid { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        public System.DateTime Timestamp { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "operatorId")]
        public int OperatorId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "containerId")]
        public int ContainerId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "locationId")]
        public int LocationId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "statusMessageCode")]
        public int StatusMessageCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "weightInKilo")]
        public double WeightInKilo { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "durationInSeconds")]
        public int DurationInSeconds { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "alibiStorageNumber")]
        public string AlibiStorageNumber { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "currentBalance")]
        public double? CurrentBalance { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "newCurrentBalance")]
        public double? NewCurrentBalance { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "chargedAmount")]
        public double? ChargedAmount { get; private set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (CustomerNumber == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "CustomerNumber");
            }
            if (CustomerNumber != null)
            {
                if (CustomerNumber.Length > 255)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "CustomerNumber", 255);
                }
                if (CustomerNumber.Length < 0)
                {
                    throw new ValidationException(ValidationRules.MinLength, "CustomerNumber", 0);
                }
            }
            if (CardUuid != null)
            {
                if (CardUuid.Length > 255)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "CardUuid", 255);
                }
                if (CardUuid.Length < 0)
                {
                    throw new ValidationException(ValidationRules.MinLength, "CardUuid", 0);
                }
            }
            if (AlibiStorageNumber != null)
            {
                if (AlibiStorageNumber.Length > 15)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "AlibiStorageNumber", 15);
                }
                if (AlibiStorageNumber.Length < 0)
                {
                    throw new ValidationException(ValidationRules.MinLength, "AlibiStorageNumber", 0);
                }
            }
        }
    }
}
