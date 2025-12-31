using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using InvoiceApp.Presentation.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.Presentation.Desktop.Views;

public partial class InvoiceBrowserView : UserControl
{
    public InvoiceBrowserView()
    {
        InitializeComponent();
        if (Application.Current is App app)
        {
            var viewModel = app.Services.GetRequiredService<InvoiceBrowserViewModel>();
            DataContext = viewModel;
            Loaded += (_, _) => viewModel.Load();
        }
    }

    private void OnOpenInvoiceClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not InvoiceBrowserViewModel viewModel)
        {
            return;
        }

        if (sender is Button button && button.DataContext is InvoiceBrowserItem invoice)
        {
            viewModel.SelectedInvoice = invoice;
            viewModel.OpenInvoice(invoice);
        }
    }

    private void OnInvoiceDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton != MouseButton.Left)
        {
            return;
        }

        if (DataContext is InvoiceBrowserViewModel viewModel)
        {
            viewModel.OpenInvoice(viewModel.SelectedInvoice);
        }
    }
}
