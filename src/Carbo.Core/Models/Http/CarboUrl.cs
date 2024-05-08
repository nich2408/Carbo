namespace Carbo.Core.Models.Http
{
    /// <summary>
    /// Class that represents a URL with route and query parameters.
    /// </summary>
    public class CarboUrl
    {
        protected CarboUrl(string templatedUrl, List<CarboKeyValuePair> routeParameters, List<CarboKeyValuePair> queryParameters)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(templatedUrl);
            ArgumentNullException.ThrowIfNull(routeParameters);
            ArgumentNullException.ThrowIfNull(queryParameters);
            TemplatedUrl = templatedUrl;
            RouteParameters = routeParameters;
            QueryParameters = queryParameters;
        }

        /// <summary>
        /// The templated URL.
        /// Example: https://api.example.com/{version}/resource?orderby={orderby}
        /// </summary>
        public string TemplatedUrl { get; }
        public List<CarboKeyValuePair> RouteParameters { get; }
        public List<CarboKeyValuePair> QueryParameters { get; }

        /// <summary>
        /// Creates a new instance of CarboUrl.
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="routeParameters"></param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        public static CarboUrl Create(string baseUrl, List<CarboKeyValuePair> routeParameters, List<CarboKeyValuePair> queryParameters)
        {
            string templatedUrl = baseUrl;
            foreach (var routeParameter in routeParameters)
            {
                string key = Uri.EscapeDataString(routeParameter.Key);
                string token = $"{{{key}}}";
                if (!baseUrl.Contains(token))
                {
                    templatedUrl = templatedUrl.Replace(routeParameter.Key, token);
                }
            }
            if (queryParameters.Count > 0)
            {
                var firstQueryParameter = queryParameters.First();
                string key = Uri.EscapeDataString(firstQueryParameter.Key);
                templatedUrl += $"?{key}={{{key}}}";
                foreach (var queryParameter in queryParameters.Skip(1))
                {
                    key = Uri.EscapeDataString(queryParameter.Key);
                    templatedUrl += $"?{key}={{{key}}}";
                }
            }
            return new CarboUrl(templatedUrl, routeParameters, queryParameters);
        }

        /// <summary>
        /// Return the URL as a Uri object.
        /// </summary>
        /// <returns></returns>
        public Uri ToUri()
        {
            string url = TemplatedUrl;
            foreach (var routeParameter in RouteParameters)
            {
                string value = Uri.EscapeDataString(routeParameter.Value);
                url = url.Replace($"{{{routeParameter.Key}}}", value);
            }
            foreach (var queryParameter in QueryParameters)
            {
                string value = Uri.EscapeDataString(queryParameter.Value);
                url = url.Replace($"{{{queryParameter.Key}}}", value);
            }
            return new Uri(url);
        }
    }
}
