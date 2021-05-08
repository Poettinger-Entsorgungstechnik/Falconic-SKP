// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Falconic.Skp.Api.Client
{
    using Microsoft.Rest;
    using Models;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// </summary>
    public partial interface ISkpAPIv10 : System.IDisposable
    {
        /// <summary>
        /// The base URI of the service.
        /// </summary>
        System.Uri BaseUri { get; set; }

        /// <summary>
        /// Gets or sets json serialization settings.
        /// </summary>
        JsonSerializerSettings SerializationSettings { get; }

        /// <summary>
        /// Gets or sets json deserialization settings.
        /// </summary>
        JsonSerializerSettings DeserializationSettings { get; }


        /// <param name='simCardNumber'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<ContainerParamsDto>> GetContainerParamsWithHttpMessagesAsync(string simCardNumber, string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='containerId'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<SkpContainerFeaturesDto>> GetContainerMachineDataWithHttpMessagesAsync(int containerId, string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='containerId'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<LastCommunicationResult>> GetContainerLastCommunicationWithHttpMessagesAsync(int containerId, string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='containerId'>
        /// </param>
        /// <param name='updateLastCommunication'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> UpdateContainerLastCommunicationWithHttpMessagesAsync(int containerId, UpdateLastCommunication updateLastCommunication = default(UpdateLastCommunication), string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='containerId'>
        /// </param>
        /// <param name='updateReadWritePointer'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> UpdateReadWritePointerMethodWithHttpMessagesAsync(int containerId, UpdateReadWritePointer updateReadWritePointer = default(UpdateReadWritePointer), string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='containerId'>
        /// </param>
        /// <param name='updateGeoPosition'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> UpdateContainerGeoPositionWithHttpMessagesAsync(int containerId, UpdateGeoPosition updateGeoPosition = default(UpdateGeoPosition), string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='containerId'>
        /// </param>
        /// <param name='storeContainerStatus'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> StoreContainerStatusMethodWithHttpMessagesAsync(int containerId, StoreContainerStatus storeContainerStatus = default(StoreContainerStatus), string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='containerId'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<ContainerHardwareInformationDto>> GetContainerHardwareInformationWithHttpMessagesAsync(int containerId, string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='containerId'>
        /// </param>
        /// <param name='storeContainerHardwareInformation'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> StoreContainerHardwareInformationMethodWithHttpMessagesAsync(int containerId, StoreContainerHardwareInformation storeContainerHardwareInformation = default(StoreContainerHardwareInformation), string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='containerId'>
        /// </param>
        /// <param name='statusMessage'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<SkpNotificationContactDto>>> GetNotificationContactsWithHttpMessagesAsync(int containerId, int statusMessage, string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='containerId'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> RemoveContainerFromAllLocationsWithHttpMessagesAsync(int containerId, string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='locationId'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<SkpLocationDto>> GetLocationByIdWithHttpMessagesAsync(int locationId, string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='locationId'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<LocationLanguageCodesDto>> GetLocationLanguageCodesWithHttpMessagesAsync(int locationId, string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='locationId'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<string>> GetLocationTimezoneWithHttpMessagesAsync(int locationId, string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='locationId'>
        /// </param>
        /// <param name='updateContainer'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> UpdateLocationContainerWithHttpMessagesAsync(int locationId, UpdateContainer updateContainer = default(UpdateContainer), string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='operatorId'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<OperatorParamsDto>> GetOperatorParamsWithHttpMessagesAsync(int operatorId, string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='operatorId'>
        /// </param>
        /// <param name='query'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<SkpLocationDto>>> GetLocationsForOperatorWithHttpMessagesAsync(int operatorId, GetSkpLocations query = default(GetSkpLocations), string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='query'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<CardAccessResult?>> HasCardAccessByCustomerNumberWithHttpMessagesAsync(HasCardAccessToLocationFractionByCustomerNumber query = default(HasCardAccessToLocationFractionByCustomerNumber), string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='query'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<CardAccessResult?>> HasCardAccessByCardUuidWithHttpMessagesAsync(HasCardAccessToLocationFractionByCardUuid query = default(HasCardAccessToLocationFractionByCardUuid), string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='command'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<CreateInsertionResult>> CreateInsertionByCustomerNumberWithHttpMessagesAsync(CreateInsertionTransactionByCustomerNumber command = default(CreateInsertionTransactionByCustomerNumber), string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='command'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<CreateInsertionResult>> CreateInsertionByCardUuidWithHttpMessagesAsync(CreateInsertionTransactionByCardUuid command = default(CreateInsertionTransactionByCardUuid), string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <param name='translationKey'>
        /// </param>
        /// <param name='languageCode'>
        /// </param>
        /// <param name='apiVersion'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<string>> GetTranslationWithHttpMessagesAsync(string translationKey, string languageCode, string apiVersion = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

    }
}
