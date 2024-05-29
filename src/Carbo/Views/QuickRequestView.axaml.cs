using Avalonia.Controls;

namespace Carbo.Views;

public partial class QuickRequestView : UserControl
{
    public QuickRequestView()
    {
        InitializeComponent();
        DataContext = new ViewModels.QuickRequestViewModel();
    }
}