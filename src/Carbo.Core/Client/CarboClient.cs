using Carbo.Core.Models.Http;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

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
            MaxResponseContentBufferSize = int.MaxValue,
            PooledConnectionLifetime = TimeSpan.FromMinutes(20)
        };

        protected CarboClient()
        {
        }

        public long MaxResponseContentBufferSize { get; init; }
        public TimeSpan PooledConnectionLifetime { get; init; }

        /// <summary>
        /// Sends a CarboRequest.
        /// </summary>
        /// <param name="carboRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Response as a CarbonResponse</returns>
        public async Task<CarboResponse> SendRequestAsync(CarboRequest carboRequest, CancellationToken? cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(carboRequest);
            HttpRequestMessage httpRequestMessage = CarboConverter.ConvertRequest(carboRequest);

            // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0#alternatives-to-ihttpclientfactory
            using HttpClient client = new()
            {
                Timeout = carboRequest.ClientTimeout,
                MaxResponseContentBufferSize = MaxResponseContentBufferSize
            };
            SocketsHttpHandler handler = new()
            {
                PooledConnectionLifetime = PooledConnectionLifetime,
                AutomaticDecompression = DecompressionMethods.All,
            };

            var stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();

                // See https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.sendasync?view=net-8.0#system-net-http-httpclient-sendasync(system-net-http-httprequestmessage)
                HttpResponseMessage response = default;
                if (cancellationToken is not null)
                {
                    response = await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken.Value);
                }
                else
                {
                    response = await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead);
                }
                stopwatch.Stop();

                CarboResponse carboResponse = CarboConverter.ConvertResponse(response, stopwatch.Elapsed);
                return carboResponse;
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException socketException)
            {
                stopwatch.Stop();
                // See https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httprequestexception?view=net-8.0
                // The socket error is the inner exception of the HttpRequestException.
                return CarboResponse.SocketErr(stopwatch.Elapsed, socketException.SocketErrorCode, ex.HttpRequestError);
            }
            catch (HttpRequestException ex)
            {
                // See https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httprequestexception?view=net-8.0
                stopwatch.Stop();
                // This should handle all the other exceptions that are not related to the socket.
                return CarboResponse.HttpErr(stopwatch.Elapsed, ex.HttpRequestError);
            }
            catch (TaskCanceledException ex)
            {
                // The TaskCanceledException can be thrown when the Timeout property of the HttpClient is lower than the time it takes to get a socket response or when the CancellationToken is cancelled.
                // This is different from the SocketException that can be thrown when a timeout occurs at socket level.
                stopwatch.Stop();
                return CarboResponse.ClientTimeout(stopwatch.Elapsed);
            }
        }
    }
}
