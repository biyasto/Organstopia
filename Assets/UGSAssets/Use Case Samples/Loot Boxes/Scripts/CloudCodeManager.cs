using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using UnityEngine;

namespace Unity.Services.Samples.LootBoxes
{
    public class CloudCodeManager : MonoBehaviour
    {
        // Cloud Code SDK status codes from Client
        const int k_CloudCodeUnprocessableEntityExceptionStatusCode = 9009;
        const int k_CloudCodeRateLimitExceptionStatusCode = 50;
        const int k_CloudCodeMissingScriptExceptionStatusCode = 9002;

        // HTTP REST API status codes
        const int k_HttpBadRequestStatusCode = 400;
        const int k_HttpTooManyRequestsStatusCode = 429;

        // Custom status codes
        const int k_UnexpectedFormatCustomStatusCode = int.MinValue;

        public static CloudCodeManager instance { get; private set; }


        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }

        void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        public async Task CallGrantRandomCurrencyEndpoint()
        {
            try
            {
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    Debug.LogError("Cloud Code can't be called to grant random currency because you're not logged in.");
                    throw new CloudCodeResultUnavailableException(null,
                        "Not logged in to authentication in CallGrantRandomCurrencyEndpoint.");
                }

                Debug.Log("Calling Cloud Code 'LootBoxes_GrantRandomCurrency' to grant random reward now.");

                var grantResult = await CloudCodeService.Instance.CallEndpointAsync<GrantRandomCurrencyResult>(
                    "LootBoxes_GrantRandomCurrency",
                    new Dictionary<string, object>());

                // Check that scene has not been unloaded while processing async wait to prevent throw.
                if (this == null) return;

                Debug.Log("CloudCode script rewarded currency id: " +
                    $"{grantResult.currencyId} amount: {grantResult.amount}");
            }
            catch (CloudCodeException e)
            {
                HandleCloudCodeException(e);
                throw new CloudCodeResultUnavailableException(e,
                    "Handled exception in CallGrantRandomCurrencyEndpoint.");
            }
        }

        void HandleCloudCodeException(CloudCodeException e)
        {
            if (e is CloudCodeRateLimitedException cloudCodeRateLimitedException)
            {
                Debug.Log("Cloud Code rate limit has been exceeded. " +
                            $"Wait {cloudCodeRateLimitedException.RetryAfter} seconds and try again.");
                return;
            }

            switch (e.ErrorCode)
            {
                case k_CloudCodeUnprocessableEntityExceptionStatusCode:
                    var cloudCodeCustomError = ConvertToActionableError(e);
                    HandleCloudCodeScriptError(cloudCodeCustomError);
                    break;

                case k_CloudCodeRateLimitExceptionStatusCode:
                    Debug.Log("Rate Limit Exceeded. Try Again.");
                    break;

                case k_CloudCodeMissingScriptExceptionStatusCode:
                    Debug.Log("Couldn't find requested Cloud Code Script");
                    break;

                default:
                    Debug.Log(e);
                    break;
            }
        }

        static CloudCodeCustomError ConvertToActionableError(CloudCodeException e)
        {
            try
            {
                // extract the JSON part of the exception message
                var trimmedMessage = e.Message;
                trimmedMessage = trimmedMessage.Substring(trimmedMessage.IndexOf('{'));
                trimmedMessage = trimmedMessage.Substring(0, trimmedMessage.LastIndexOf('}') + 1);

                // Convert the message string ultimately into the Cloud Code Custom Error object which has a
                // standard structure for all errors.
                return JsonUtility.FromJson<CloudCodeCustomError>(trimmedMessage);
            }
            catch (Exception exception)
            {
                return new CloudCodeCustomError("Failed to Parse Error", k_UnexpectedFormatCustomStatusCode,
                    "Cloud Code Unprocessable Entity exception is in an unexpected format and " +
                    $"couldn't be parsed: {exception.Message}", e);
            }
        }

        // This method does whatever handling is appropriate given the specific error. So for example for an invalid
        // play in the Cloud Ai Mini Game, it shows a popup in the scene to explain the error.
        void HandleCloudCodeScriptError(CloudCodeCustomError cloudCodeCustomError)
        {
            switch (cloudCodeCustomError.status)
            {
                case k_HttpBadRequestStatusCode:
                    Debug.Log("A bad server request occurred during Cloud Code script execution: " + 
                                $"{cloudCodeCustomError.name}: {cloudCodeCustomError.message} : " +
                                $"{cloudCodeCustomError.details[0]}");
                    break;

                case k_HttpTooManyRequestsStatusCode:
                    Debug.Log($"Rate Limit has been exceeded. Wait {cloudCodeCustomError.retryAfter} " +
                                $"seconds and try again.");
                    break;

                case k_UnexpectedFormatCustomStatusCode:
                    Debug.Log($"Cloud Code returned an Unprocessable Entity exception, " +
                                $"but it could not be parsed: { cloudCodeCustomError.message }. " +
                                $"Original error: { cloudCodeCustomError.InnerException?.Message }");
                    break;

                default:
                    Debug.Log($"Cloud code returned error: {cloudCodeCustomError.status}: " +
                                $"{cloudCodeCustomError.name}: {cloudCodeCustomError.message}");
                    break;
            }
        }

        // Struct used to receive result from Cloud Code.
        public struct GrantRandomCurrencyResult
        {
            public string currencyId;
            public int amount;
        }

        class CloudCodeCustomError : Exception
        {
            public int status;
            public string name;
            public string message;
            public string retryAfter;
            public string[] details;

            public CloudCodeCustomError(string name, int status, string message = null, 
                Exception innerException = null) : base(message, innerException)
            {
                this.name = name;
                this.status = status;
                this.message = message;
                retryAfter = null;
                details = new string[] { };
            }
        }
    }
}
