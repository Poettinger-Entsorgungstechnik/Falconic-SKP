using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Falconic.Skp.Api.Client
{
    public class ApiKeyDelegatingHandler : DelegatingHandler
    {
        private readonly string appId;
        private readonly string apiKey;

        public ApiKeyDelegatingHandler(string appId, string apiKey)
        {
            this.appId = appId ?? throw new ArgumentNullException(nameof(appId));
            this.apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            string requestContentBase64String = string.Empty;

            string requestUri = WebUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());

            string requestHttpMethod = request.Method.Method;

            // Calculate UNIX time
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

            // create random nonce for each request
            string nonce = Guid.NewGuid().ToString("N");

            // Checking if the request contains body, usually will be null wiht HTTP GET and DELETE
            if (request.Content != null)
            {
                byte[] content = await request.Content.ReadAsByteArrayAsync();
                MD5 md5 = MD5.Create();

                // Hashing the request body, any change in request body will result in different hash, we'll incure message integrity
                byte[] requestContentHash = md5.ComputeHash(content);
                requestContentBase64String = Convert.ToBase64String(requestContentHash);
            }

            // Creating the raw signature string
            string signatureRawData = string.Format("{0}{1}{2}{3}{4}{5}", this.appId, requestHttpMethod, requestUri, requestTimeStamp, nonce, requestContentBase64String);

            var apiKeyBytes = Convert.FromBase64String(this.apiKey);
            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

            using (HMACSHA256 hmac = new HMACSHA256(apiKeyBytes))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);

                // Setting the values in the Authorization header using custom scheme (Hmac)
                request.Headers.Authorization = new AuthenticationHeaderValue("Hmac", string.Format("{0}:{1}:{2}:{3}", this.appId, requestSignatureBase64String, nonce, requestTimeStamp));
            }

            response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
