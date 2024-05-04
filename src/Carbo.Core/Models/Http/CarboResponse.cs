using System.Net;
using System.Net.Http.Headers;

namespace Carbo.Core.Models.Http
{
    /// <summary>
    /// Class that represents a response from the server.
    /// </summary>
    public class CarboResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public HttpContent Content { get; set; }
        public HttpHeaders Headers { get; set; }
        public HttpHeaders TrailingHeaders { get; set; }
        public Version Version { get; set; }
        public TimeSpan ElapsedTime { get; set; }
    }
}
