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
        /// <summary>
        /// Indicates if the response viewmodel is busy.
        /// </summary>
        [ObservableProperty]
        private bool isBusy;

        /// <summary>
        /// The status code of the response.
        /// </summary>
        [ObservableProperty]
        private HttpStatusCodeViewModel statusCode;

        /// <summary>
        /// The reason phrase of the response.
        /// </summary>
        [ObservableProperty]
        private string reasonPhrase;

        /// <summary>
        /// The content of the response.
        /// Use this for non-stream content.
        /// </summary>
        [ObservableProperty]
        private string stringContent;

        /// <summary>
        /// The version of the response (for example 3.0.0).
        /// </summary>
        [ObservableProperty]
        private string version;

        /// <summary>
        /// The elapsed time of the response in milliseconds.
        /// </summary>
        [ObservableProperty]
        private double elapsedTimeMs;

        /// <summary>
        /// Indicates if the client timeout was exceeded.
        /// </summary>
        [ObservableProperty]
        private bool exceededClientTimeout;

        /// <summary>
        /// The error of the response (if any).
        /// </summary>
        [ObservableProperty]
        private RequestErrorViewModel requestError;

        /// <summary>
        /// The unknown error message of the response (if any).
        /// </summary>
        [ObservableProperty]
        private string unknownErrorMessage;

        /// <summary>
        /// The headers of the response.
        /// </summary>
        public ObservableCollection<KeyValuePairViewModel> Headers { get; private set; }

        /// <summary>
        /// The trailing headers of the response.
        /// </summary>
        public ObservableCollection<KeyValuePairViewModel> TrailingHeaders { get; private set; }

        /// <summary>
        /// Creates a new instance of the viewmodel with default.
        /// </summary>
        /// <returns></returns>
        public static ResponseViewModel Default()
        {
            return new ResponseViewModel()
            {
                StatusCode = null,
                ReasonPhrase = null,
                StringContent = null,
                Version = null,
                ElapsedTimeMs = 0,
                ExceededClientTimeout = false,
                RequestError = null,
                Headers = [],
                TrailingHeaders = [],
                UnknownErrorMessage = null,
            };
        }

        /// <summary>
        /// Assigns data from a CarboResponse to the viewmodel.
        /// </summary>
        /// <param name="carboResponse"></param>
        /// <returns></returns>
        public async Task LoadFromCarboResponseAsync(CarboResponse carboResponse)
        {
            // Assign the data from the CarboResponse to the viewmodel.
            StatusCode = new HttpStatusCodeViewModel { StatusCode = (int)carboResponse.StatusCode, };
            ReasonPhrase = carboResponse.ReasonPhrase;
            StringContent = await carboResponse.Content.ReadAsStringAsync();
            Version = carboResponse.Version.ToString();
            ElapsedTimeMs = carboResponse.ElapsedTime.TotalMilliseconds;
            ExceededClientTimeout = carboResponse.ExceededClientTimeout;
            UnknownErrorMessage = carboResponse.Exception?.Message;

            // Assign the headers and trailing headers to the viewmodel.
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

            // Assign the request error to the viewmodel.
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
            // Assign the data from the viewmodel to the CarboResponse.
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

            // Assign the headers and trailing headers to the CarboResponse.
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
