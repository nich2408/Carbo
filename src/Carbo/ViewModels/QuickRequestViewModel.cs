using Carbo.Core.Client;
using Carbo.Core.Models.Http;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using System.Threading.Tasks;

namespace Carbo.ViewModels
{
    /// <summary>
    /// Class that represents the viewmodel of a quick request.
    /// </summary>
    public partial class QuickRequestViewModel : ObservableObject
    {
        /// <summary>
        /// The cancellation token source for sending the request.
        /// </summary>
        private CancellationTokenSource ctsSendRequest;

        /// <summary>
        /// If true the request can be sent.
        /// </summary>
        private bool CanSendRequest => !RequestViewModel.IsBusy && !ResponseViewModel.IsBusy;

        /// <summary>
        /// If true the request can be cancelled.
        /// </summary>
        private bool CanCancelRequest => RequestViewModel.IsBusy && ResponseViewModel.IsBusy;

        /// <summary>
        /// The viewmodel of the request.
        /// </summary>
        [ObservableProperty]
        private RequestViewModel requestViewModel;

        /// <summary>
        /// The viewmodel of the response.
        /// </summary>
        [ObservableProperty]
        private ResponseViewModel responseViewModel;

        /// <summary>
        /// The viewmodel of the URL text box.
        /// </summary>
        [ObservableProperty]
        private UrlTextBoxViewModel urlTextBoxViewModel;

        /// <summary>
        /// The viewmodel of the HTTP method selector.
        /// </summary>
        [ObservableProperty]
        private HttpMethodSelectorViewModel httpMethodSelectorViewModel;

        public QuickRequestViewModel()
        {
            RequestViewModel = RequestViewModel.Default();
            ResponseViewModel = ResponseViewModel.Default();
            UrlTextBoxViewModel = UrlTextBoxViewModel.Default();
            HttpMethodSelectorViewModel = HttpMethodSelectorViewModel.Default();
        }

        /// <summary>
        /// Command for sending the request.
        /// </summary>
        /// <returns></returns>
        [RelayCommand(CanExecute = nameof(CanSendRequest))]
        private async Task SendRequestCommand()
        {
            RequestViewModel.IsBusy = true;
            ResponseViewModel.IsBusy = true;
            ctsSendRequest = new CancellationTokenSource();

            CarboRequest request = RequestViewModel.ToCarboRequest();
            CarboResponse carboResponse = await CarboClient.Instance.SendRequestAsync(request, ctsSendRequest.Token);
            await ResponseViewModel.LoadFromCarboResponseAsync(carboResponse);

            RequestViewModel.IsBusy = false;
            ResponseViewModel.IsBusy = false;
        }

        /// <summary>
        /// Command for cancelling the request.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanCancelRequest))]
        private void CancelRequest()
        {
            ctsSendRequest?.Cancel();
            RequestViewModel.IsBusy = false;
            ResponseViewModel.IsBusy = false;
        }
    }
}
