using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LopezTruck202X.Data;
using LopezTruck202X.Models;
using LopezTruck202X.Services;
using LopezTruck202X.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace LopezTruck202X;

public sealed partial class MainWindow : Window
{
    private readonly SqliteInvoiceRepository _repository;
    private readonly InvoicePdfService _pdfService = new();

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new MainViewModel();

        var databasePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "lopeztrucking.db");
        var database = new SqliteDatabase(databasePath);
        _repository = new SqliteInvoiceRepository(database);

        if (Content is FrameworkElement root)
        {
            root.Loaded += OnLoaded;
        }
    }

    public MainViewModel ViewModel { get; }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        await _repository.InitializeAsync();
        await LoadCatalogsAsync();
        ViewModel.InvoiceNumber = await _repository.GetNextInvoiceNumberAsync();
    }

    private void OnAddLine(object sender, RoutedEventArgs e)
    {
        ViewModel.AddLine();
    }

    private void OnRemoveLine(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Models.InvoiceLine line })
        {
            ViewModel.RemoveLine(line);
        }
    }

    private async void OnSave(object sender, RoutedEventArgs e)
    {
        var invoice = ViewModel.ToInvoice();
        await _repository.SaveInvoiceAsync(invoice);
    }

    private async void OnGeneratePdf(object sender, RoutedEventArgs e)
    {
        var invoice = ViewModel.ToInvoice();
        var outputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        await _pdfService.GenerateAsync(invoice, outputDirectory);
    }

    private async void OnOpenCatalogs(object sender, RoutedEventArgs e)
    {
        CatalogDialog.XamlRoot = Root.XamlRoot;
        await CatalogDialog.ShowAsync();
    }

    private async void OnAddOrigin(object sender, RoutedEventArgs e)
    {
        var name = ViewModel.NewOriginName?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        if (ViewModel.Origins.Any(origin => string.Equals(origin.Name, name, StringComparison.OrdinalIgnoreCase)))
        {
            ViewModel.NewOriginName = string.Empty;
            return;
        }

        await _repository.AddOriginAsync(name);
        ViewModel.NewOriginName = string.Empty;
        await LoadCatalogsAsync();
    }

    private async void OnAddDestination(object sender, RoutedEventArgs e)
    {
        var name = ViewModel.NewDestinationName?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        if (ViewModel.Destinations.Any(destination => string.Equals(destination.Name, name, StringComparison.OrdinalIgnoreCase)))
        {
            ViewModel.NewDestinationName = string.Empty;
            return;
        }

        await _repository.AddDestinationAsync(name);
        ViewModel.NewDestinationName = string.Empty;
        await LoadCatalogsAsync();
    }

    private async void OnUpdateOrigin(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Origin origin })
        {
            var name = origin.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            origin.Name = name;
            await _repository.UpdateOriginAsync(origin);
            await LoadCatalogsAsync();
        }
    }

    private async void OnUpdateDestination(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Destination destination })
        {
            var name = destination.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            destination.Name = name;
            await _repository.UpdateDestinationAsync(destination);
            await LoadCatalogsAsync();
        }
    }

    private async Task LoadCatalogsAsync()
    {
        var origins = await _repository.GetOriginsAsync();
        var destinations = await _repository.GetDestinationsAsync();
        ViewModel.SetOrigins(origins);
        ViewModel.SetDestinations(destinations);
    }
}
