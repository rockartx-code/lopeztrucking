using System.Windows;
using InvoiceApp.Presentation.Desktop.ViewModels;

namespace InvoiceApp.Presentation.Desktop;

public partial class MainWindow : Window
{
    private readonly AddDetailGroupViewModel _viewModel;

    public MainWindow(AddDetailGroupViewModel viewModel)
    {
        _viewModel = viewModel;
        InitializeComponent();
        DataContext = _viewModel;
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        await _viewModel.LoadAsync();
    }
}
