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
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace LopezTruck202X;

public sealed partial class MainWindow : Window
{
    private readonly SqliteInvoiceRepository _repository;
    private readonly AccessImportService _accessImportService;
    private readonly InvoicePdfService _pdfService = new();
    private XamlRoot? DialogXamlRoot => (Content as FrameworkElement)?.XamlRoot;

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new MainViewModel();

        var databasePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "lopeztrucking.db");
        var database = new SqliteDatabase(databasePath);
        _repository = new SqliteInvoiceRepository(database);
        _accessImportService = new AccessImportService(database);

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
        if (await _repository.IsCustomerInactiveAsync(ViewModel.CustomerName))
        {
            var dialog = new ContentDialog
            {
                XamlRoot = DialogXamlRoot,
                Title = "Cliente eliminado",
                Content = "Este cliente fue eliminado y no se puede usar de nuevo.",
                CloseButtonText = "Cerrar"
            };
            await dialog.ShowAsync();
            return;
        }

        var invoice = ViewModel.ToInvoice();
        try
        {
            await _repository.SaveInvoiceAsync(invoice);
        }
        catch (InvalidOperationException)
        {
            var dialog = new ContentDialog
            {
                XamlRoot = DialogXamlRoot,
                Title = "Cliente eliminado",
                Content = "Este cliente fue eliminado y no se puede usar de nuevo.",
                CloseButtonText = "Cerrar"
            };
            await dialog.ShowAsync();
            return;
        }
        await LoadCustomersAsync();
    }

    private async void OnGeneratePdf(object sender, RoutedEventArgs e)
    {
        var invoice = ViewModel.ToInvoice();
        InvoiceSummaryDialog.XamlRoot = DialogXamlRoot;
        InvoiceSummaryDialog.DataContext = invoice;
        var result = await InvoiceSummaryDialog.ShowAsync();
        if (result != ContentDialogResult.Primary)
        {
            return;
        }

        var outputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        await _pdfService.GenerateAsync(invoice, outputDirectory);
    }

    private async void OnOpenCatalogs(object sender, RoutedEventArgs e)
    {
        CatalogDialog.XamlRoot = DialogXamlRoot;
        await CatalogDialog.ShowAsync();
    }

    private async void OnOpenSearch(object sender, RoutedEventArgs e)
    {
        SearchDialog.XamlRoot = DialogXamlRoot;
        await SearchDialog.ShowAsync();
    }

    private async void OnImportAccess(object sender, RoutedEventArgs e)
    {
        var picker = new FileOpenPicker
        {
            SuggestedStartLocation = PickerLocationId.DocumentsLibrary
        };
        picker.FileTypeFilter.Add(".accdb");
        picker.FileTypeFilter.Add(".mdb");
        InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(this));

        var file = await picker.PickSingleFileAsync();
        if (file is null)
        {
            return;
        }

        var confirmDialog = new ContentDialog
        {
            XamlRoot = DialogXamlRoot,
            Title = "Importar Access",
            Content = "Esta acción reemplazará los datos actuales en SQLite. ¿Deseas continuar?",
            PrimaryButtonText = "Importar",
            CloseButtonText = "Cancelar"
        };

        if (await confirmDialog.ShowAsync() != ContentDialogResult.Primary)
        {
            return;
        }

        try
        {
            await _accessImportService.ImportAsync(file.Path);
            await LoadCatalogsAsync();
            ViewModel.InvoiceNumber = await _repository.GetNextInvoiceNumberAsync();

            var successDialog = new ContentDialog
            {
                XamlRoot = DialogXamlRoot,
                Title = "Importación completa",
                Content = "La información de Access fue importada correctamente.",
                CloseButtonText = "Cerrar"
            };
            await successDialog.ShowAsync();
        }
        catch (Exception ex)
        {
            var errorDialog = new ContentDialog
            {
                XamlRoot = DialogXamlRoot,
                Title = "Error al importar",
                Content = $"No se pudo importar el archivo de Access. Detalle: {ex.Message}",
                CloseButtonText = "Cerrar"
            };
            await errorDialog.ShowAsync();
        }
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

    private async void OnAddCompany(object sender, RoutedEventArgs e)
    {
        var name = ViewModel.NewCompanyName?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        if (ViewModel.Companies.Any(company => string.Equals(company.Name, name, StringComparison.OrdinalIgnoreCase)))
        {
            ViewModel.NewCompanyName = string.Empty;
            return;
        }

        await _repository.AddCompanyAsync(name);
        ViewModel.NewCompanyName = string.Empty;
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

    private async void OnUpdateCompany(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Company company })
        {
            var name = company.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            company.Name = name;
            await _repository.UpdateCompanyAsync(company);
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

    private async void OnDeleteCompany(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Company company })
        {
            await _repository.DeleteCompanyAsync(company.Id);
            await LoadCatalogsAsync();
        }
    }

    private async void OnDeleteCustomer(object sender, RoutedEventArgs e)
    {
        if (sender is not Button { Tag: Customer customer })
        {
            return;
        }

        var deleted = await _repository.DeleteCustomerAsync(customer.Id);
        if (!deleted)
        {
            var dialog = new ContentDialog
            {
                XamlRoot = DialogXamlRoot,
                Title = "No se puede eliminar",
                Content = "El cliente ya estaba eliminado o no existe.",
                CloseButtonText = "Cerrar"
            };
            await dialog.ShowAsync();
            return;
        }

        ViewModel.ClearCustomerSelection();
        await LoadCustomersAsync();
    }

    private async Task LoadCatalogsAsync()
    {
        await LoadCustomersAsync();
        var origins = await _repository.GetOriginsAsync();
        var destinations = await _repository.GetDestinationsAsync();
        var companies = await _repository.GetCompaniesAsync();
        var prices = await _repository.GetPricesAsync();
        ViewModel.SetOrigins(origins);
        ViewModel.SetDestinations(destinations);
        ViewModel.SetCompanies(companies);
        ViewModel.SetPrices(prices);
    }

    private async Task LoadCustomersAsync()
    {
        var customers = await _repository.GetCustomersAsync();
        ViewModel.SetCustomers(customers);
    }

    private async void OnLinePriceSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        await UpdateLinePriceAsync();
    }

    private async void OnSearchInvoices(object sender, RoutedEventArgs e)
    {
        var invoices = await _repository.GetInvoicesAsync(
            ViewModel.SearchStartDate,
            ViewModel.SearchEndDate,
            ViewModel.SearchCustomerName);
        ViewModel.SetSearchResults(invoices);
    }

    private async void OnLoadSelectedInvoice(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedInvoiceSummary is null)
        {
            return;
        }

        var invoice = await _repository.GetInvoiceAsync(ViewModel.SelectedInvoiceSummary.Id);
        if (invoice is null)
        {
            return;
        }

        ViewModel.LoadInvoice(invoice);
        ViewModel.SelectedInvoiceSummary = null;
        SearchDialog.Hide();
    }

    private async Task UpdateLinePriceAsync()
    {
        if (!ViewModel.TryGetSelectedRouteIds(out var companyId, out var originId, out var destinationId))
        {
            return;
        }

        var amount = await _repository.GetPriceAmountAsync(companyId, originId, destinationId);
        ViewModel.NewLineAmount = amount ?? 0d;
    }

    private async void OnAddPrice(object sender, RoutedEventArgs e)
    {
        if (ViewModel.NewPriceCompanyId is null
            || ViewModel.NewPriceOriginId is null
            || ViewModel.NewPriceDestinationId is null)
        {
            return;
        }

        var price = new Price
        {
            CompanyId = ViewModel.NewPriceCompanyId.Value,
            OriginId = ViewModel.NewPriceOriginId.Value,
            DestinationId = ViewModel.NewPriceDestinationId.Value,
            Amount = ViewModel.NewPriceAmount
        };

        await _repository.UpsertPriceAsync(price);
        ViewModel.ResetNewPrice();
        await LoadCatalogsAsync();
    }

    private async void OnUpdatePrice(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Price price })
        {
            await _repository.UpsertPriceAsync(price);
            await LoadCatalogsAsync();
        }
    }

    private async void OnDeletePrice(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Price price })
        {
            await _repository.DeletePriceAsync(price.CompanyId, price.OriginId, price.DestinationId);
            await LoadCatalogsAsync();
        }
    }
}
