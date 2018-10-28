// <auto-generated />

namespace Falconic.Skp.Api.Client
{
    using Falconic.Skp;
    using Falconic.Skp.Api;
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for SkpAPIv10.
    /// </summary>
    public static partial class SkpAPIv10Extensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='simCardNumber'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static ContainerParamsDto GetContainerParams(this ISkpAPIv10 operations, string simCardNumber, string apiVersion = default(string))
            {
                return operations.GetContainerParamsAsync(simCardNumber, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='simCardNumber'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ContainerParamsDto> GetContainerParamsAsync(this ISkpAPIv10 operations, string simCardNumber, string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetContainerParamsWithHttpMessagesAsync(simCardNumber, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static SkpContainerFeaturesDto GetContainerMachineData(this ISkpAPIv10 operations, int containerId, string apiVersion = default(string))
            {
                return operations.GetContainerMachineDataAsync(containerId, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<SkpContainerFeaturesDto> GetContainerMachineDataAsync(this ISkpAPIv10 operations, int containerId, string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetContainerMachineDataWithHttpMessagesAsync(containerId, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static LastCommunicationResult GetContainerLastCommunication(this ISkpAPIv10 operations, int containerId, string apiVersion = default(string))
            {
                return operations.GetContainerLastCommunicationAsync(containerId, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<LastCommunicationResult> GetContainerLastCommunicationAsync(this ISkpAPIv10 operations, int containerId, string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetContainerLastCommunicationWithHttpMessagesAsync(containerId, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='updateLastCommunication'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static void UpdateContainerLastCommunication(this ISkpAPIv10 operations, int containerId, UpdateLastCommunication updateLastCommunication = default(UpdateLastCommunication), string apiVersion = default(string))
            {
                operations.UpdateContainerLastCommunicationAsync(containerId, updateLastCommunication, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='updateLastCommunication'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task UpdateContainerLastCommunicationAsync(this ISkpAPIv10 operations, int containerId, UpdateLastCommunication updateLastCommunication = default(UpdateLastCommunication), string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.UpdateContainerLastCommunicationWithHttpMessagesAsync(containerId, updateLastCommunication, apiVersion, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='updateReadWritePointer'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static void UpdateReadWritePointerMethod(this ISkpAPIv10 operations, int containerId, UpdateReadWritePointer updateReadWritePointer = default(UpdateReadWritePointer), string apiVersion = default(string))
            {
                operations.UpdateReadWritePointerMethodAsync(containerId, updateReadWritePointer, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='updateReadWritePointer'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task UpdateReadWritePointerMethodAsync(this ISkpAPIv10 operations, int containerId, UpdateReadWritePointer updateReadWritePointer = default(UpdateReadWritePointer), string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.UpdateReadWritePointerMethodWithHttpMessagesAsync(containerId, updateReadWritePointer, apiVersion, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='updateGeoPosition'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static void UpdateContainerGeoPosition(this ISkpAPIv10 operations, int containerId, UpdateGeoPosition updateGeoPosition = default(UpdateGeoPosition), string apiVersion = default(string))
            {
                operations.UpdateContainerGeoPositionAsync(containerId, updateGeoPosition, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='updateGeoPosition'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task UpdateContainerGeoPositionAsync(this ISkpAPIv10 operations, int containerId, UpdateGeoPosition updateGeoPosition = default(UpdateGeoPosition), string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.UpdateContainerGeoPositionWithHttpMessagesAsync(containerId, updateGeoPosition, apiVersion, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='storeContainerStatus'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static void StoreContainerStatusMethod(this ISkpAPIv10 operations, int containerId, StoreContainerStatus storeContainerStatus = default(StoreContainerStatus), string apiVersion = default(string))
            {
                operations.StoreContainerStatusMethodAsync(containerId, storeContainerStatus, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='storeContainerStatus'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task StoreContainerStatusMethodAsync(this ISkpAPIv10 operations, int containerId, StoreContainerStatus storeContainerStatus = default(StoreContainerStatus), string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.StoreContainerStatusMethodWithHttpMessagesAsync(containerId, storeContainerStatus, apiVersion, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='storeContainerHardwareInformation'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static void StoreContainerHardwareInformationMethod(this ISkpAPIv10 operations, int containerId, StoreContainerHardwareInformation storeContainerHardwareInformation = default(StoreContainerHardwareInformation), string apiVersion = default(string))
            {
                operations.StoreContainerHardwareInformationMethodAsync(containerId, storeContainerHardwareInformation, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='storeContainerHardwareInformation'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task StoreContainerHardwareInformationMethodAsync(this ISkpAPIv10 operations, int containerId, StoreContainerHardwareInformation storeContainerHardwareInformation = default(StoreContainerHardwareInformation), string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.StoreContainerHardwareInformationMethodWithHttpMessagesAsync(containerId, storeContainerHardwareInformation, apiVersion, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='statusMessage'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static IList<SkpNotificationContactDto> GetNotificationContacts(this ISkpAPIv10 operations, int containerId, int statusMessage, string apiVersion = default(string))
            {
                return operations.GetNotificationContactsAsync(containerId, statusMessage, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='statusMessage'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<SkpNotificationContactDto>> GetNotificationContactsAsync(this ISkpAPIv10 operations, int containerId, int statusMessage, string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetNotificationContactsWithHttpMessagesAsync(containerId, statusMessage, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static void RemoveContainerFromAllLocations(this ISkpAPIv10 operations, int containerId, string apiVersion = default(string))
            {
                operations.RemoveContainerFromAllLocationsAsync(containerId, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='containerId'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task RemoveContainerFromAllLocationsAsync(this ISkpAPIv10 operations, int containerId, string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.RemoveContainerFromAllLocationsWithHttpMessagesAsync(containerId, apiVersion, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='locationId'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static SkpLocationDto GetLocationById(this ISkpAPIv10 operations, int locationId, string apiVersion = default(string))
            {
                return operations.GetLocationByIdAsync(locationId, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='locationId'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<SkpLocationDto> GetLocationByIdAsync(this ISkpAPIv10 operations, int locationId, string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetLocationByIdWithHttpMessagesAsync(locationId, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='locationId'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static IList<string> GetLocationLanguageCodes(this ISkpAPIv10 operations, int locationId, string apiVersion = default(string))
            {
                return operations.GetLocationLanguageCodesAsync(locationId, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='locationId'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<string>> GetLocationLanguageCodesAsync(this ISkpAPIv10 operations, int locationId, string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetLocationLanguageCodesWithHttpMessagesAsync(locationId, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='locationId'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static string GetLocationTimezone(this ISkpAPIv10 operations, int locationId, string apiVersion = default(string))
            {
                return operations.GetLocationTimezoneAsync(locationId, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='locationId'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<string> GetLocationTimezoneAsync(this ISkpAPIv10 operations, int locationId, string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetLocationTimezoneWithHttpMessagesAsync(locationId, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='locationId'>
            /// </param>
            /// <param name='updateContainer'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static void UpdateLocationContainer(this ISkpAPIv10 operations, int locationId, UpdateContainer updateContainer = default(UpdateContainer), string apiVersion = default(string))
            {
                operations.UpdateLocationContainerAsync(locationId, updateContainer, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='locationId'>
            /// </param>
            /// <param name='updateContainer'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task UpdateLocationContainerAsync(this ISkpAPIv10 operations, int locationId, UpdateContainer updateContainer = default(UpdateContainer), string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.UpdateLocationContainerWithHttpMessagesAsync(locationId, updateContainer, apiVersion, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='operatorId'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static OperatorParamsDto GetOperatorParams(this ISkpAPIv10 operations, int operatorId, string apiVersion = default(string))
            {
                return operations.GetOperatorParamsAsync(operatorId, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='operatorId'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<OperatorParamsDto> GetOperatorParamsAsync(this ISkpAPIv10 operations, int operatorId, string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetOperatorParamsWithHttpMessagesAsync(operatorId, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='operatorId'>
            /// </param>
            /// <param name='query'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static IList<SkpLocationDto> GetLocationsForOperator(this ISkpAPIv10 operations, int operatorId, GetSkpLocations query = default(GetSkpLocations), string apiVersion = default(string))
            {
                return operations.GetLocationsForOperatorAsync(operatorId, query, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='operatorId'>
            /// </param>
            /// <param name='query'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<SkpLocationDto>> GetLocationsForOperatorAsync(this ISkpAPIv10 operations, int operatorId, GetSkpLocations query = default(GetSkpLocations), string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetLocationsForOperatorWithHttpMessagesAsync(operatorId, query, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='translationKey'>
            /// </param>
            /// <param name='languageCode'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            public static string GetTranslation(this ISkpAPIv10 operations, string translationKey, string languageCode, string apiVersion = default(string))
            {
                return operations.GetTranslationAsync(translationKey, languageCode, apiVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='translationKey'>
            /// </param>
            /// <param name='languageCode'>
            /// </param>
            /// <param name='apiVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<string> GetTranslationAsync(this ISkpAPIv10 operations, string translationKey, string languageCode, string apiVersion = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetTranslationWithHttpMessagesAsync(translationKey, languageCode, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
