using CommunityToolkit.Mvvm.ComponentModel;

namespace Carbo.ViewModels
{
    /// <summary>
    /// Class that represents the viewmodel of a quick request.
    /// </summary>
    public partial class QuickRequestViewModel : ObservableObject
    {
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

        public QuickRequestViewModel()
        {
            requestViewModel = RequestViewModel.Default();
            responseViewModel = null;
        }
    }
}
