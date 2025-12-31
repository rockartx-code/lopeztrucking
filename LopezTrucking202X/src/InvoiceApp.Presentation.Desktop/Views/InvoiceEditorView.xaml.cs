using System.Windows;
using System.Windows.Controls;
using InvoiceApp.Presentation.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.Presentation.Desktop.Views;

public partial class InvoiceEditorView : UserControl
{
    public InvoiceEditorView()
    {
        InitializeComponent();
        if (Application.Current is App app)
        {
            var viewModel = app.Services.GetRequiredService<InvoiceEditorViewModel>();
            DataContext = viewModel;
            Loaded += async (_, _) => await viewModel.LoadAsync();
        }
    }

    private void OnManagePlacesClick(object sender, RoutedEventArgs e)
    {
        if (Application.Current is not App app)
        {
            return;
        }

        var window = app.Services.GetRequiredService<PlacesAdminView>();
        ShowAdminWindow(window);
    }

    private void OnManageCompanyPlacesClick(object sender, RoutedEventArgs e)
    {
        if (Application.Current is not App app)
        {
            return;
        }

        var window = app.Services.GetRequiredService<CompanyPlacesAdminView>();
        ShowAdminWindow(window);
    }

    private void ShowAdminWindow(Window window)
    {
        if (DataContext is not InvoiceEditorViewModel viewModel)
        {
            window.Show();
            return;
        }

        EventHandler? onSaved = async (_, _) => await viewModel.RefreshPlacesAsync().ConfigureAwait(true);

        if (window.DataContext is PlacesAdminViewModel placesAdmin)
        {
            placesAdmin.DataSaved += onSaved;
            window.Closed += (_, _) => placesAdmin.DataSaved -= onSaved;
        }
        else if (window.DataContext is CompanyPlacesAdminViewModel companyPlacesAdmin)
        {
            companyPlacesAdmin.DataSaved += onSaved;
            window.Closed += (_, _) => companyPlacesAdmin.DataSaved -= onSaved;
        }

        window.Owner = Window.GetWindow(this);
        window.Closed += async (_, _) => await viewModel.RefreshPlacesAsync().ConfigureAwait(true);
        window.Show();
    }
}
