using CommunityToolkit.Mvvm.ComponentModel;

namespace Carbo.ViewModels
{
    public partial class UrlTextBlockViewModel : ObservableObject
    {
        [ObservableProperty]
        private string typedUrl;
    }
}
