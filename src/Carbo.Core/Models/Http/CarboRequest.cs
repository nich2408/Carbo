using System.Net.Http.Headers;

namespace Carbo.Core.Models.Http
{
    /// <summary>
    /// Class that represents a request to be made by the carbo client.
    /// </summary>
    public class CarboRequest
    {
        public HttpMethod HttpMethod { get; set; }
        public Uri Url { get; set; }
        public List<RequestKeyValueParameter> QueryParameters { get; set; }
        public HttpHeaders Headers { get; set; }
        public HttpContent Content { get; set; }
    }
}
