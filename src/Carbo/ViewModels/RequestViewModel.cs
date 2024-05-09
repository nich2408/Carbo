using Carbo.Core.Models.Http;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
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
        /// Indicates if the request viewmodel is busy.
        /// </summary>
        [ObservableProperty]
        private bool isBusy;

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
        /// The route parameters to send in the request.
        /// </summary>
        public ObservableCollection<KeyValuePairViewModel> RouteParameters { get; private set; } = new();

        /// <summary>
        /// The headers to send in the request.
        /// </summary>
        public ObservableCollection<KeyValuePairViewModel> Headers { get; private set; } = new();

        /// <summary>
        /// Returns a default request viewmodel.
        /// </summary>
        /// <returns></returns>
        public static RequestViewModel Default()
        {
            return new()
            {
                HttpMethod = HttpMethodViewModel.Get(),
                Url = "https://catfact.ninja/fact",
                StringContent = null,
                ClientTimeoutMs = TimeSpan.FromMinutes(1).TotalMilliseconds,
                QueryParameters = [],
                RouteParameters = [],
                Headers = [],
            };
        }

        /// <summary>
        /// Assigns data from a CarboRequest to the viewmodel.
        /// </summary>
        /// <param name="carboRequest"></param>
        /// <returns></returns>
        public async Task LoadFromCarboRequest(CarboRequest carboRequest)
        {
            // Assign data from CarboRequest to the viewmodel.
            HttpMethod = new HttpMethodViewModel { Method = carboRequest.HttpMethod.Method, };
            Url = carboRequest.Url.TemplatedUrl;
            StringContent = await carboRequest.Content.ReadAsStringAsync();
            ClientTimeoutMs = (double)carboRequest.ClientTimeout.TotalMilliseconds;

            // Assign data from CarboRequest to the viewmodel.
            QueryParameters.Clear();
            foreach (var queryParameter in carboRequest.Url.QueryParameters)
            {
                QueryParameters.Add(new KeyValuePairViewModel { Key = queryParameter.Key, Value = queryParameter.Value, });
            }
            RouteParameters.Clear();
            foreach (var routeParameter in carboRequest.Url.RouteParameters)
            {
                RouteParameters.Add(new KeyValuePairViewModel { Key = routeParameter.Key, Value = routeParameter.Value, });
            }
            Headers.Clear();
            foreach (var header in carboRequest.Headers)
            {
                Headers.Add(new KeyValuePairViewModel { Key = header.Key, Value = header.Value, });
            }
        }

        /// <summary>
        /// Returns a carbo request from the viewmodel.
        /// </summary>
        /// <returns></returns>
        public CarboRequest ToCarboRequest()
        {
            List<CarboKeyValuePair> queryParameters = QueryParameters.Select(x => new CarboKeyValuePair { Key = x.Key, Value = x.Value, }).ToList();
            List<CarboKeyValuePair> routeParameters = RouteParameters.Select(x => new CarboKeyValuePair { Key = x.Key, Value = x.Value, }).ToList();
            return new()
            {
                HttpMethod = new HttpMethod(HttpMethod.Method),
                Url = CarboUrl.Create(Url, routeParameters, queryParameters),
                Headers = Headers.Select(x => new CarboKeyValuePair { Key = x.Key, Value = x.Value, }).ToList(),
                Content = new StringContent(StringContent),
                ClientTimeout = TimeSpan.FromMilliseconds(ClientTimeoutMs)
            };
        }
    }
}
