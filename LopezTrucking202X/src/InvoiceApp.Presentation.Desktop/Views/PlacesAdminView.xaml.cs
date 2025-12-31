using System.Windows;
using InvoiceApp.Presentation.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.Presentation.Desktop.Views;

public partial class PlacesAdminView : Window
{
    public PlacesAdminView()
    {
        InitializeComponent();
        if (Application.Current is App app)
        {
            var viewModel = app.Services.GetRequiredService<PlacesAdminViewModel>();
            DataContext = viewModel;
            Loaded += async (_, _) => await viewModel.LoadAsync();
        }
    }
}
