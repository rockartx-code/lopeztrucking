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
}
