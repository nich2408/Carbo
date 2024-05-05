using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;

namespace Carbo.Core.Models.Http
{
    /// <summary>
    /// Class that represents a response from the server.
    /// </summary>
    public class CarboResponse
    {
        private CarboResponse(
            HttpStatusCode? statusCode,
            string reasonPhrase,
            HttpContent content,
            List<CarboKeyValuePair> headers,
            HttpHeaders trailingHeaders,
            Version version,
            TimeSpan elapsedTime,
            bool exceededClientTimeout,
            HttpRequestError? requestError,
            SocketError? socketError)
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Content = content;
            Headers = headers;
            TrailingHeaders = trailingHeaders;
            Version = version;
            ElapsedTime = elapsedTime;
            ExceededClientTimeout = exceededClientTimeout;
            RequestError = requestError;
            SocketError = socketError;
        }

        /// <summary>
        /// Use this method when the request was completed.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="reasonPhrase"></param>
        /// <param name="content"></param>
        /// <param name="headers"></param>
        /// <param name="trailingHeaders"></param>
        /// <param name="version"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        public static CarboResponse Completed(HttpStatusCode statusCode, string reasonPhrase, HttpContent content, List<CarboKeyValuePair> headers, HttpHeaders trailingHeaders, Version version, TimeSpan elapsedTime)
        {
            return new CarboResponse(statusCode: statusCode, reasonPhrase: reasonPhrase, content: content, headers: headers, trailingHeaders: trailingHeaders, version: version, elapsedTime: elapsedTime, exceededClientTimeout: false, socketError: null, requestError: null);
        }

        /// <summary>
        /// Use this method when the request was not completed because of a socket error.
        /// </summary>
        /// <param name="elapsedTime"></param>
        /// <param name="socketError"></param>
        /// <param name="requestError"></param>
        /// <returns></returns>
        public static CarboResponse SocketErr(TimeSpan elapsedTime, SocketError socketError, HttpRequestError requestError)
        {
            return new CarboResponse(statusCode: null, reasonPhrase: null, content: null, headers: null, trailingHeaders: null, version: null, elapsedTime: elapsedTime, exceededClientTimeout: false, socketError: socketError, requestError: requestError);
        }

        /// <summary>
        /// Use this method when the request was not completed due to an error.
        /// </summary>
        /// <param name="elapsedTime"></param>
        /// <param name="requestError"></param>
        /// <returns></returns>
        public static CarboResponse HttpErr(TimeSpan elapsedTime, HttpRequestError requestError)
        {
            return new CarboResponse(statusCode: null, reasonPhrase: null, content: null, headers: null, trailingHeaders: null, version: null, elapsedTime: elapsedTime, exceededClientTimeout: false, socketError: null, requestError: requestError);
        }

        /// <summary>
        /// Use this method when the request was not completed because the elapsed time of the request exceeded the timeout set in the client.
        /// </summary>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        public static CarboResponse ClientTimeout(TimeSpan elapsedTime)
        {
            return new CarboResponse(statusCode: null, reasonPhrase: null, content: null, headers: null, trailingHeaders: null, version: null, elapsedTime: elapsedTime, exceededClientTimeout: true, socketError: null, requestError: null);
        }

        public HttpStatusCode? StatusCode { get; }
        public string ReasonPhrase { get; }
        public HttpContent Content { get; }
        public List<CarboKeyValuePair> Headers { get; }
        public HttpHeaders TrailingHeaders { get; }
        public Version Version { get; }
        public TimeSpan ElapsedTime { get; }

        public bool ExceededClientTimeout { get; }

        /// <summary>
        /// See https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httprequesterror?view=net-8.0
        /// </summary>
        public HttpRequestError? RequestError { get; }
        public SocketError? SocketError { get; }
    }
}
