using System;
using System.IO;
using LopezTruck202X.Data;
using LopezTruck202X.Services;
using LopezTruck202X.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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

        Loaded += OnLoaded;
    }

    public MainViewModel ViewModel { get; }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        await _repository.InitializeAsync();
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
}
