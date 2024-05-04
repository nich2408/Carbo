using Carbo.Core.Models.Http;
using System.Diagnostics;
using System.Net;

namespace Carbo.Core.Client
{
    /// <summary>
    /// Class that sends a CarboRequest and returns a CarboResponse.
    /// </summary>
    public class CarboClient
    {
        /// <summary>
        /// Gets the singleton instance of the CarboClient class.
        /// Currently there'no need for a lazy initialization .
        /// </summary>
        public static CarboClient Instance { get; } = new()
        {
            RequestTimeout = TimeSpan.FromMinutes(1),
            MaxResponseContentBufferSize = int.MaxValue,
            PooledConnectionLifetime = TimeSpan.FromMinutes(20)
        };

        protected CarboClient()
        {
        }

        public TimeSpan RequestTimeout { get; init; }
        public long MaxResponseContentBufferSize { get; init; }
        public TimeSpan PooledConnectionLifetime { get; init; }

        /// <summary>
        /// Sends a CarboRequest.
        /// </summary>
        /// <param name="carboRequest"></param>
        /// <returns>Response as a CarbonResponse</returns>
        public CarboResponse SendRequest(CarboRequest carboRequest)
        {
            ArgumentNullException.ThrowIfNull(carboRequest);
            HttpRequestMessage httpRequestMessage = CarboConverter.ConvertRequest(carboRequest);

            // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0#alternatives-to-ihttpclientfactory
            using HttpClient client = new()
            {
                Timeout = RequestTimeout,
                MaxResponseContentBufferSize = MaxResponseContentBufferSize
            };
            SocketsHttpHandler handler = new()
            {
                PooledConnectionLifetime = PooledConnectionLifetime,
                AutomaticDecompression = DecompressionMethods.All,
            };

            var stopwatch = Stopwatch.StartNew();
            HttpResponseMessage response = client.Send(httpRequestMessage);
            stopwatch.Stop();

            CarboResponse carboResponse = CarboConverter.ConvertResponse(response, stopwatch.Elapsed);
            return carboResponse;
        }
    }
}
