using CommunityToolkit.Mvvm.ComponentModel;

namespace Carbo.ViewModels
{
    /// <summary>
    /// Class that represents the viewmodel of a HTTP status code.
    /// </summary>
    public partial class HttpStatusCodeViewModel : ObservableObject
    {
        [ObservableProperty]
        private int statusCode;
    }
}
