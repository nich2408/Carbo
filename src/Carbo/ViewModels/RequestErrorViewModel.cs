using CommunityToolkit.Mvvm.ComponentModel;

namespace Carbo.ViewModels
{
    /// <summary>
    /// Class that represents the viewmodel of a request error.
    /// </summary>
    public partial class RequestErrorViewModel : ObservableObject
    {
        [ObservableProperty]
        private int errorCode;

        [ObservableProperty]
        private string errorMessage;

        public RequestErrorType ErrorType { get; set; }
    }
}
