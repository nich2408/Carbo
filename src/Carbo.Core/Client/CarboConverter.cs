using Carbo.Core.Models.Http;

namespace Carbo.Core.Client
{
    /// <summary>
    /// Class that converts System.Net.Http objects to Carbo.Core.Models.Http models.
    /// </summary>
    internal class CarboConverter
    {
        /// <summary>
        /// Converts a Request object to a HttpRequestMessage object.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static HttpRequestMessage ConvertRequest(CarboRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            HttpRequestMessage httpRequestMessage = new(request.HttpMethod, request.Url);
            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                {
                    httpRequestMessage.Headers.Add(header.Key, header.Value);
                }
            }
            if (request.Content != null)
            {
                httpRequestMessage.Content = request.Content;
            }
            return httpRequestMessage;
        }

        /// <summary>
        /// Converts a HttpResponseMessage object to a Response object.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        public static CarboResponse ConvertResponse(HttpResponseMessage response, TimeSpan elapsedTime)
        {
            ArgumentNullException.ThrowIfNull(response);

            CarboResponse carboResponse = new()
            {
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                Content = response.Content,
                Headers = response.Headers,
                TrailingHeaders = response.TrailingHeaders,
                Version = response.Version,
                ElapsedTime = elapsedTime,
            };
            return carboResponse;
        }
    }
}
