using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace InvoiceApp.Presentation.Desktop.ViewModels;

public sealed class InvoiceBrowserViewModel : INotifyPropertyChanged
{
    private const string AllSubhaulersLabel = "All";
    private readonly InvoiceEditorViewModel _invoiceEditorViewModel;
    private DateTime? _startDate;
    private DateTime? _endDate;
    private string _selectedSubhauler = AllSubhaulersLabel;
    private InvoiceBrowserItem? _selectedInvoice;

    public InvoiceBrowserViewModel(InvoiceEditorViewModel invoiceEditorViewModel)
    {
        _invoiceEditorViewModel = invoiceEditorViewModel;
        SubhaulerFilters = new ObservableCollection<string>();
        Invoices = new ObservableCollection<InvoiceBrowserItem>();

        InvoicesView = CollectionViewSource.GetDefaultView(Invoices);
        InvoicesView.Filter = FilterInvoices;
        InvoicesView.SortDescriptions.Add(new SortDescription(nameof(InvoiceBrowserItem.InvoiceNumber), ListSortDirection.Descending));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<string> SubhaulerFilters { get; }

    public ObservableCollection<InvoiceBrowserItem> Invoices { get; }

    public ICollectionView InvoicesView { get; }

    public DateTime? StartDate
    {
        get => _startDate;
        set
        {
            if (SetField(ref _startDate, value))
            {
                InvoicesView.Refresh();
            }
        }
    }

    public DateTime? EndDate
    {
        get => _endDate;
        set
        {
            if (SetField(ref _endDate, value))
            {
                InvoicesView.Refresh();
            }
        }
    }

    public string SelectedSubhauler
    {
        get => _selectedSubhauler;
        set
        {
            if (SetField(ref _selectedSubhauler, value))
            {
                InvoicesView.Refresh();
            }
        }
    }

    public InvoiceBrowserItem? SelectedInvoice
    {
        get => _selectedInvoice;
        set => SetField(ref _selectedInvoice, value);
    }

    public void Load()
    {
        var invoices = BuildSampleInvoices();
        Invoices.Clear();
        foreach (var invoice in invoices)
        {
            Invoices.Add(invoice);
        }

        SubhaulerFilters.Clear();
        SubhaulerFilters.Add(AllSubhaulersLabel);
        foreach (var subhauler in invoices.Select(item => item.SubhaulerName).Distinct())
        {
            SubhaulerFilters.Add(subhauler);
        }

        SelectedSubhauler = AllSubhaulersLabel;
    }

    public void OpenInvoice(InvoiceBrowserItem? invoice)
    {
        var target = invoice ?? SelectedInvoice;
        if (target is null)
        {
            return;
        }

        _invoiceEditorViewModel.LoadFromBrowser(target);
    }

    private bool FilterInvoices(object item)
    {
        if (item is not InvoiceBrowserItem invoice)
        {
            return false;
        }

        if (StartDate.HasValue && invoice.InvoiceDate.Date < StartDate.Value.Date)
        {
            return false;
        }

        if (EndDate.HasValue && invoice.InvoiceDate.Date > EndDate.Value.Date)
        {
            return false;
        }

        if (!string.Equals(SelectedSubhauler, AllSubhaulersLabel, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(invoice.SubhaulerName, SelectedSubhauler, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }

    private IReadOnlyList<InvoiceBrowserItem> BuildSampleInvoices()
    {
        var subhaulers = _invoiceEditorViewModel.Subhaulers;
        var fallbackSubhauler = subhaulers.FirstOrDefault()?.Name ?? "Lopez Trucking";
        var secondarySubhauler = subhaulers.Skip(1).FirstOrDefault()?.Name ?? fallbackSubhauler;

        return new List<InvoiceBrowserItem>
        {
            new("1025", DateTime.Today.AddDays(-1), fallbackSubhauler),
            new("1024", DateTime.Today.AddDays(-3), secondarySubhauler),
            new("1023", DateTime.Today.AddDays(-8), fallbackSubhauler),
            new("1022", DateTime.Today.AddDays(-12), secondarySubhauler),
            new("1021", DateTime.Today.AddDays(-20), fallbackSubhauler)
        };
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}
