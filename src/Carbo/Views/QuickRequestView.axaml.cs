using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Carbo.Views;

public partial class QuickRequestView : UserControl
{
    public QuickRequestView()
    {
        InitializeComponent();
        DataContext = new ViewModels.QuickRequestViewModel();
    }
}