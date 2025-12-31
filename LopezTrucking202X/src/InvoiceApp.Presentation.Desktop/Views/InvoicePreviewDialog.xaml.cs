using System.Windows;

namespace InvoiceApp.Presentation.Desktop.Views;

public partial class InvoicePreviewDialog : Window
{
    public InvoicePreviewDialog()
    {
        InitializeComponent();
    }

    private void OnAcceptClicked(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

    private void OnCancelClicked(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
