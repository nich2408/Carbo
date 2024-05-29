using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carbo.ViewModels
{
    /// <summary>
    /// Class that represents the viewmodel of a http method selector.
    /// </summary>
    public partial class HttpMethodSelectorViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<HttpMethodViewModel> httpMethods;

        [ObservableProperty]
        private HttpMethodViewModel selectedHttpMethod;

        public static HttpMethodSelectorViewModel Default()
        {
            return new HttpMethodSelectorViewModel
            {
                HttpMethods =
                [
                    HttpMethodViewModel.Get(),
                    HttpMethodViewModel.Post(),
                    HttpMethodViewModel.Put(),
                    HttpMethodViewModel.Patch(),
                    HttpMethodViewModel.Delete(),
                    HttpMethodViewModel.Trace(),
                    HttpMethodViewModel.Head(),
                    HttpMethodViewModel.Connect(),
                    HttpMethodViewModel.Options()
                ],
                SelectedHttpMethod = HttpMethodViewModel.Get(),
            };
        }
    }
}
