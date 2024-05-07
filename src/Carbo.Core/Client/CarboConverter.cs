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
            ArgumentOutOfRangeException.ThrowIfNegative(elapsedTime.Ticks);

            List<CarboKeyValuePair> carboPairs = new();
            foreach (var header in response.Headers)
            {
                var pair = new CarboKeyValuePair { Key = header.Key, Value = string.Join(",", header.Value) };
                carboPairs.Add(pair);
            }

            List<CarboKeyValuePair> trailingHeaders = new();
            foreach (var header in response.TrailingHeaders)
            {
                var pair = new CarboKeyValuePair { Key = header.Key, Value = string.Join(",", header.Value) };
                trailingHeaders.Add(pair);
            }

            CarboResponse carboResponse = CarboResponse.Completed(
                response.StatusCode,
                response.ReasonPhrase,
                response.Content,
                carboPairs,
                trailingHeaders,
                response.Version,
                elapsedTime);

            return carboResponse;
        }
    }
}
