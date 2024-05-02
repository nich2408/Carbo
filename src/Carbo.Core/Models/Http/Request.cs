namespace Carbo.Core.Models.Http
{
    /// <summary>
    /// Class that represents a request to be made by the client.
    /// </summary>
    public class Request
    {
        public HttpMethod HttpMethod { get; set; }
        public Uri Url { get; set; }
        public List<RequestKeyValueParameter> QueryParameters { get; set; }
        public List<RequestKeyValueParameter> Headers { get; set; }
        public HttpContent Content { get; set; }
    }
}
