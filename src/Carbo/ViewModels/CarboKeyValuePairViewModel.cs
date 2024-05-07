using Carbo.Core.Models.Http;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Carbo.ViewModels
{
    /// <summary>
    /// Class that represents the viewmodel of a key-value pair.
    /// </summary>
    public partial class CarboKeyValuePairViewModel : ObservableObject
    {
        [ObservableProperty]
        private string key;

        [ObservableProperty]
        private string value;

        [ObservableProperty]
        private bool enabled;
    }
}
