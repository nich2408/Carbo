namespace Carbo.Core.Models.Http
{
    /// <summary>
    /// Class that represents a request to be made by the carbo client.
    /// </summary>
    public class CarboRequest
    {
        public HttpMethod HttpMethod { get; set; }
        public CarboUrl Url { get; set; }
        public List<CarboKeyValuePair> Headers { get; set; }
        public HttpContent Content { get; set; }
        public TimeSpan ClientTimeout { get; set; }
    }
}
