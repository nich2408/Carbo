using CommunityToolkit.Mvvm.ComponentModel;

namespace Carbo.ViewModels
{
    /// <summary>
    /// Class that represents the viewmodel of a http method.
    /// </summary>
    public partial class HttpMethodViewModel : ObservableObject
    {
        [ObservableProperty]
        private string method;
    }
}
