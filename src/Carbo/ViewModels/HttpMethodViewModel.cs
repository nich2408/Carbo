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

        public static HttpMethodViewModel Get() => new() { Method = "GET" };
        public static HttpMethodViewModel Post() => new() { Method = "POST" };
        public static HttpMethodViewModel Put() => new() { Method = "PUT" };
        public static HttpMethodViewModel Patch() => new() { Method = "PATCH" };
        public static HttpMethodViewModel Delete() => new() { Method = "DELETE" };
        public static HttpMethodViewModel Trace() => new() { Method = "TRACE" };
        public static HttpMethodViewModel Head() => new() { Method = "HEAD" };
        public static HttpMethodViewModel Connect() => new() { Method = "CONNECT" };
        public static HttpMethodViewModel Options() => new() { Method = "OPTIONS" };
    }
}
