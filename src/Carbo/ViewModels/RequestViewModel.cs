using Carbo.Core.Models.Http;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Carbo.ViewModels
{
    /// <summary>
    /// Class that represents the viewmodel of a request.
    /// </summary>
    public partial class RequestViewModel : ObservableObject
    {
        /// <summary>
        /// The HTTP method to use in the request.
        /// </summary>
        [ObservableProperty]
        private HttpMethodViewModel httpMethod;

        /// <summary>
        /// The URL to send the request.
        /// </summary>
        [ObservableProperty]
        private string url;

        /// <summary>
        /// The content to send in the request.
        /// Use this for non-stream content.
        /// </summary>
        [ObservableProperty]
        private string stringContent;

        /// <summary>
        /// The client timeout for the request in milliseconds.
        /// </summary>
        [ObservableProperty]
        private double clientTimeoutMs;

        /// <summary>
        /// The query parameters to send in the request.
        /// </summary>
        public ObservableCollection<KeyValuePairViewModel> QueryParameters { get; private set; } = new();

        /// <summary>
        /// The headers to send in the request.
        /// </summary>
        public ObservableCollection<KeyValuePairViewModel> Headers { get; private set; } = new();

        /// <summary>
        /// Assigns data from a CarboRequest to the viewmodel.
        /// </summary>
        /// <param name="carboRequest"></param>
        /// <returns></returns>
        public async Task FromCarboRequest(CarboRequest carboRequest)
        {
            HttpMethod = new HttpMethodViewModel { Method = carboRequest.HttpMethod.Method, };
            Url = carboRequest.Url.ToString();

            QueryParameters.Clear();
            foreach (var queryParameter in carboRequest.QueryParameters)
            {
                QueryParameters.Add(new KeyValuePairViewModel { Key = queryParameter.Key, Value = queryParameter.Value, });
            }

            Headers.Clear();
            foreach (var header in carboRequest.Headers)
            {
                Headers.Add(new KeyValuePairViewModel { Key = header.Key, Value = header.Value, });
            }

            StringContent = await carboRequest.Content.ReadAsStringAsync();
            ClientTimeoutMs = (double)carboRequest.ClientTimeout.TotalMilliseconds;
        }

        /// <summary>
        /// Returns a carbo request from the viewmodel.
        /// </summary>
        /// <returns></returns>
        private CarboRequest ToCarboRequest()
        {
            return new()
            {
                HttpMethod = new HttpMethod(HttpMethod.Method),
                Url = new Uri(Url),
                QueryParameters = QueryParameters.Select(x => new CarboKeyValuePair { Key = x.Key, Value = x.Value, }).ToList(),
                Headers = Headers.Select(x => new CarboKeyValuePair { Key = x.Key, Value = x.Value, }).ToList(),
                Content = new StringContent(StringContent),
                ClientTimeout = TimeSpan.FromMilliseconds(ClientTimeoutMs)
            };
        }
    }
}
