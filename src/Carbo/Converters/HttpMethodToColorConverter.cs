using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Carbo.ViewModels;
using System;
using System.Globalization;

namespace Carbo.Converters
{
    public class HttpMethodToColorConverter : IValueConverter
    {
        public static readonly HttpMethodToColorConverter Instance = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HttpMethodViewModel httpMethod)
            {
                return httpMethod.Method switch
                {
                    "GET" => new SolidColorBrush(Color.FromArgb(255, 50, 121, 219)),
                    "POST" => new SolidColorBrush(Color.FromArgb(255, 204, 201, 144)),
                    "PUT" => new SolidColorBrush(Color.FromArgb(255, 252, 161, 48)),
                    "PATCH" => new SolidColorBrush(Color.FromArgb(255, 252, 161, 48)),
                    "DELETE" => new SolidColorBrush(Color.FromArgb(255, 252, 75, 79)),
                    "TRACE" => new SolidColorBrush(Color.FromArgb(255, 210, 210, 210)),
                    "HEAD" => new SolidColorBrush(Color.FromArgb(255, 176, 78, 237)),
                    "CONNECT" => new SolidColorBrush(Color.FromArgb(255, 194, 113, 70)),
                    "OPTIONS" => new SolidColorBrush(Color.FromArgb(255, 50, 121, 219)),
                    _ => throw new NotImplementedException($"The method {httpMethod.Method} is not implemented for color conversion"),
                };
            }

            // converter used for the wrong type
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
