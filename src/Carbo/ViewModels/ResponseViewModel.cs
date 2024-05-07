using Carbo.Core.Models.Http;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Carbo.ViewModels
{
    /// <summary>
    /// Class that represents the viewmodel of a response.
    /// </summary>
    public partial class ResponseViewModel : ObservableObject
    {
        [ObservableProperty]
        private HttpStatusCodeViewModel statusCode;

        [ObservableProperty]
        private string reasonPhrase;

        [ObservableProperty]
        private string stringContent;

        
        [ObservableProperty]
        private string version;

        [ObservableProperty]
        private double elapsedTimeMs;

        [ObservableProperty]
        private bool exceededClientTimeout;

        [ObservableProperty]
        private RequestErrorViewModel requestError;

        public ObservableCollection<KeyValuePairViewModel> Headers { get; private set; } = new();
        public ObservableCollection<KeyValuePairViewModel> TrailingHeaders { get; private set; } = new();

        /// <summary>
        /// Assigns data from a CarboResponse to the viewmodel.
        /// </summary>
        /// <param name="carboResponse"></param>
        /// <returns></returns>
        public async Task FromCarboResponse(CarboResponse carboResponse)
        {
            StatusCode = new HttpStatusCodeViewModel { StatusCode = (int)carboResponse.StatusCode, };
            ReasonPhrase = carboResponse.ReasonPhrase;
            StringContent = await carboResponse.Content.ReadAsStringAsync();
            Version = carboResponse.Version.ToString();
            ElapsedTimeMs = carboResponse.ElapsedTime.TotalMilliseconds;
            ExceededClientTimeout = carboResponse.ExceededClientTimeout;

            Headers.Clear();
            foreach (CarboKeyValuePair header in carboResponse.Headers)
            {
                Headers.Add(new KeyValuePairViewModel { Key = header.Key, Value = header.Value });
            }

            TrailingHeaders.Clear();
            foreach (CarboKeyValuePair header in carboResponse.TrailingHeaders)
            {
                TrailingHeaders.Add(new KeyValuePairViewModel { Key = header.Key, Value = header.Value });
            }

            RequestErrorViewModel requestErrorViewModel = new();
            requestErrorViewModel.ErrorType = carboResponse.RequestError is not null ? RequestErrorType.RequestError : RequestErrorType.SocketError;
            requestErrorViewModel.ErrorCode = requestErrorViewModel.ErrorType switch
            {
                RequestErrorType.RequestError => (int)carboResponse.RequestError,
                RequestErrorType.SocketError => (int)carboResponse.SocketError,
                _ => -1, // Unknown error type.
            };
            requestErrorViewModel.ErrorMessage = requestErrorViewModel.ErrorType switch
            {
                RequestErrorType.RequestError => carboResponse.RequestError.ToString(),
                RequestErrorType.SocketError => carboResponse.SocketError.ToString(),
                _ => "Unknown error message.", // Unknown error type.
            };
            RequestError = requestErrorViewModel;
        }

        /// <summary>
        /// Returns a carbo response from the viewmodel.
        /// </summary>
        /// <returns></returns>
        public CarboResponse ToCarboResponse()
        {
            CarboResponse carboResponse = new()
            {
                StatusCode = (HttpStatusCode)StatusCode.StatusCode,
                ReasonPhrase = ReasonPhrase,
                Content = new StringContent(StringContent),
                Version = new Version(Version),
                ElapsedTime = TimeSpan.FromMilliseconds(ElapsedTimeMs),
                ExceededClientTimeout = ExceededClientTimeout,
                RequestError = RequestError.ErrorType == RequestErrorType.RequestError ? (HttpRequestError)RequestError.ErrorCode : null,
                SocketError = RequestError.ErrorType == RequestErrorType.SocketError ? (SocketError)RequestError.ErrorCode : null,
            };

            foreach (KeyValuePairViewModel header in Headers)
            {
                carboResponse.Headers.Add(new CarboKeyValuePair { Key = header.Key, Value = header.Value });
            }

            foreach (KeyValuePairViewModel header in TrailingHeaders)
            {
                carboResponse.TrailingHeaders.Add(new CarboKeyValuePair { Key = header.Key, Value = header.Value });
            }

            return carboResponse;
        }
    }
}
