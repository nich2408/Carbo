using CommunityToolkit.Mvvm.ComponentModel;

namespace Carbo.ViewModels
{
    public partial class UrlTextBoxViewModel : ObservableObject
    {
        [ObservableProperty]
        private string typedUrl;

        public static UrlTextBoxViewModel Default()
        {
            return new UrlTextBoxViewModel
            {
                TypedUrl = null,
            };
        }
    }
}
