using System.Windows;
using InvoiceApp.Presentation.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.Presentation.Desktop.Views;

public partial class CompanyPlacesAdminView : Window
{
    public CompanyPlacesAdminView()
    {
        InitializeComponent();
        if (Application.Current is App app)
        {
            var viewModel = app.Services.GetRequiredService<CompanyPlacesAdminViewModel>();
            DataContext = viewModel;
            Loaded += async (_, _) => await viewModel.LoadAsync();
        }
    }
}
