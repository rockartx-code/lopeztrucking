using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using LopezTruck202X.Models;

namespace LopezTruck202X.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private double _invoiceNumber;
    private DateTimeOffset _invoiceDate = DateTimeOffset.Now;
    private string _checkNumber = string.Empty;
    private string _customerName = string.Empty;
    private string _customerAddress = string.Empty;
    private string _customerCity = string.Empty;
    private string _customerState = string.Empty;
    private string _customerPhone = string.Empty;
    private double _subtotal;
    private double _advance;
    private double _total;
    private Customer? _selectedCustomer;
    private bool _isUpdatingCustomerSelection;

    private DateTimeOffset _newLineDate = DateTimeOffset.Now;
    private string _selectedCompanyName = string.Empty;
    private string _newLineFrom = string.Empty;
    private string _newLineTo = string.Empty;
    private string _newLineDispatch = string.Empty;
    private string _newLineEmpties = string.Empty;
    private string _newLineFb = string.Empty;
    private double _newLineAmount;
    private string _newOriginName = string.Empty;
    private string _newDestinationName = string.Empty;
    private string _newCompanyName = string.Empty;
    private double _newPriceAmount;
    private DateTimeOffset? _searchStartDate;
    private DateTimeOffset? _searchEndDate;
    private string _searchCustomerName = string.Empty;
    private InvoiceSummary? _selectedInvoiceSummary;

    public MainViewModel()
    {
        Lines.CollectionChanged += OnLinesChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<InvoiceLine> Lines { get; } = new();
    public ObservableCollection<Customer> Customers { get; } = new();
    public ObservableCollection<Origin> Origins { get; } = new();
    public ObservableCollection<Destination> Destinations { get; } = new();
    public ObservableCollection<Company> Companies { get; } = new();
    public ObservableCollection<Price> Prices { get; } = new();
    public ObservableCollection<InvoiceSummary> SearchResults { get; } = new();

    public double InvoiceNumber
    {
        get => _invoiceNumber;
        set => SetField(ref _invoiceNumber, value);
    }

    public DateTimeOffset InvoiceDate
    {
        get => _invoiceDate;
        set => SetField(ref _invoiceDate, value);
    }

    public string CheckNumber
    {
        get => _checkNumber;
        set => SetField(ref _checkNumber, value);
    }

    public string CustomerName
    {
        get => _customerName;
        set
        {
            if (SetField(ref _customerName, value))
            {
                if (!_isUpdatingCustomerSelection
                    && _selectedCustomer is not null
                    && !string.Equals(_selectedCustomer.Name, value, StringComparison.OrdinalIgnoreCase))
                {
                    SelectedCustomer = null;
                }
            }
        }
    }

    public string CustomerAddress
    {
        get => _customerAddress;
        set => SetField(ref _customerAddress, value);
    }

    public string CustomerCity
    {
        get => _customerCity;
        set => SetField(ref _customerCity, value);
    }

    public string CustomerState
    {
        get => _customerState;
        set => SetField(ref _customerState, value);
    }

    public string CustomerPhone
    {
        get => _customerPhone;
        set => SetField(ref _customerPhone, value);
    }

    public Customer? SelectedCustomer
    {
        get => _selectedCustomer;
        set
        {
            if (SetField(ref _selectedCustomer, value) && value is not null)
            {
                _isUpdatingCustomerSelection = true;
                CustomerName = value.Name;
                CustomerAddress = value.Address;
                CustomerCity = value.City;
                CustomerState = value.State;
                CustomerPhone = value.Phone;
                _isUpdatingCustomerSelection = false;
            }
        }
    }

    public double Subtotal
    {
        get => _subtotal;
        private set
        {
            if (SetField(ref _subtotal, value))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubtotalFormatted)));
            }
        }
    }

    public double Advance
    {
        get => _advance;
        set
        {
            if (SetField(ref _advance, value))
            {
                RecalculateTotals();
            }
        }
    }

    public double Total
    {
        get => _total;
        private set
        {
            if (SetField(ref _total, value))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalFormatted)));
            }
        }
    }

    public DateTimeOffset NewLineDate
    {
        get => _newLineDate;
        set => SetField(ref _newLineDate, value);
    }

    public string SelectedCompanyName
    {
        get => _selectedCompanyName;
        set => SetField(ref _selectedCompanyName, value);
    }

    public string NewLineFrom
    {
        get => _newLineFrom;
        set => SetField(ref _newLineFrom, value);
    }

    public string NewLineTo
    {
        get => _newLineTo;
        set => SetField(ref _newLineTo, value);
    }

    public string NewLineDispatch
    {
        get => _newLineDispatch;
        set => SetField(ref _newLineDispatch, value);
    }

    public string NewLineEmpties
    {
        get => _newLineEmpties;
        set => SetField(ref _newLineEmpties, value);
    }

    public string NewLineFb
    {
        get => _newLineFb;
        set => SetField(ref _newLineFb, value);
    }

    public double NewLineAmount
    {
        get => _newLineAmount;
        set => SetField(ref _newLineAmount, value);
    }

    public string NewOriginName
    {
        get => _newOriginName;
        set => SetField(ref _newOriginName, value);
    }

    public string NewDestinationName
    {
        get => _newDestinationName;
        set => SetField(ref _newDestinationName, value);
    }

    public string NewCompanyName
    {
        get => _newCompanyName;
        set => SetField(ref _newCompanyName, value);
    }

    public double NewPriceAmount
    {
        get => _newPriceAmount;
        set => SetField(ref _newPriceAmount, value);
    }

    public DateTimeOffset? SearchStartDate
    {
        get => _searchStartDate;
        set => SetField(ref _searchStartDate, value);
    }

    public DateTimeOffset? SearchEndDate
    {
        get => _searchEndDate;
        set => SetField(ref _searchEndDate, value);
    }

    public string SearchCustomerName
    {
        get => _searchCustomerName;
        set => SetField(ref _searchCustomerName, value);
    }

    public InvoiceSummary? SelectedInvoiceSummary
    {
        get => _selectedInvoiceSummary;
        set => SetField(ref _selectedInvoiceSummary, value);
    }

    public string SubtotalFormatted => Subtotal.ToString("C", CultureInfo.CurrentCulture);

    public string TotalFormatted => Total.ToString("C", CultureInfo.CurrentCulture);

    public void AddLine()
    {
        var line = new InvoiceLine
        {
            Date = NewLineDate,
            Company = SelectedCompanyName,
            From = NewLineFrom,
            To = NewLineTo,
            Dispatch = NewLineDispatch,
            Empties = NewLineEmpties,
            Fb = NewLineFb,
            Amount = Convert.ToDecimal(NewLineAmount)
        };

        Lines.Add(line);
        ResetNewLine();
    }

    public void RemoveLine(InvoiceLine line)
    {
        Lines.Remove(line);
    }

    public void SetOrigins(IEnumerable<Origin> origins)
    {
        Origins.Clear();
        foreach (var origin in origins)
        {
            Origins.Add(origin);
        }
    }

    public void SetCustomers(IEnumerable<Customer> customers)
    {
        Customers.Clear();
        foreach (var customer in customers)
        {
            Customers.Add(customer);
        }

        if (!string.IsNullOrWhiteSpace(CustomerName))
        {
            SelectedCustomer = Customers.FirstOrDefault(item =>
                string.Equals(item.Name, CustomerName, StringComparison.OrdinalIgnoreCase));
        }
    }

    public void SetDestinations(IEnumerable<Destination> destinations)
    {
        Destinations.Clear();
        foreach (var destination in destinations)
        {
            Destinations.Add(destination);
        }
    }

    public void SetCompanies(IEnumerable<Company> companies)
    {
        Companies.Clear();
        foreach (var company in companies)
        {
            Companies.Add(company);
        }
    }

    public void SetPrices(IEnumerable<Price> prices)
    {
        Prices.Clear();
        foreach (var price in prices)
        {
            Prices.Add(price);
        }
    }

    public void SetSearchResults(IEnumerable<InvoiceSummary> invoices)
    {
        SearchResults.Clear();
        foreach (var invoice in invoices)
        {
            SearchResults.Add(invoice);
        }
    }

    public void ClearCustomerSelection()
    {
        _isUpdatingCustomerSelection = true;
        SelectedCustomer = null;
        _isUpdatingCustomerSelection = false;
        CustomerName = string.Empty;
        CustomerAddress = string.Empty;
        CustomerCity = string.Empty;
        CustomerState = string.Empty;
        CustomerPhone = string.Empty;
    }

    public bool TryGetSelectedRouteIds(out int companyId, out int originId, out int destinationId)
    {
        companyId = 0;
        originId = 0;
        destinationId = 0;

        if (string.IsNullOrWhiteSpace(SelectedCompanyName)
            || string.IsNullOrWhiteSpace(NewLineFrom)
            || string.IsNullOrWhiteSpace(NewLineTo))
        {
            return false;
        }

        var company = Companies.FirstOrDefault(item =>
            string.Equals(item.Name, SelectedCompanyName, StringComparison.OrdinalIgnoreCase));
        var origin = Origins.FirstOrDefault(item =>
            string.Equals(item.Name, NewLineFrom, StringComparison.OrdinalIgnoreCase));
        var destination = Destinations.FirstOrDefault(item =>
            string.Equals(item.Name, NewLineTo, StringComparison.OrdinalIgnoreCase));

        if (company is null || origin is null || destination is null)
        {
            return false;
        }

        companyId = company.Id;
        originId = origin.Id;
        destinationId = destination.Id;
        return true;
    }

    public void ResetNewPrice()
    {
        NewPriceAmount = 0d;
    }

    public Invoice ToInvoice()
    {
        return new Invoice
        {
            Number = Convert.ToInt32(InvoiceNumber),
            Date = InvoiceDate,
            CheckNumber = CheckNumber,
            Customer = new Customer
            {
                Name = CustomerName,
                Address = CustomerAddress,
                City = CustomerCity,
                State = CustomerState,
                Phone = CustomerPhone
            },
            Subtotal = Convert.ToDecimal(Subtotal),
            Advance = Convert.ToDecimal(Advance),
            Total = Convert.ToDecimal(Total),
            Lines = Lines.ToList()
        };
    }

    public void LoadInvoice(Invoice invoice)
    {
        InvoiceNumber = invoice.Number;
        InvoiceDate = invoice.Date;
        CheckNumber = invoice.CheckNumber;
        CustomerName = invoice.Customer.Name;
        CustomerAddress = invoice.Customer.Address;
        CustomerCity = invoice.Customer.City;
        CustomerState = invoice.Customer.State;
        CustomerPhone = invoice.Customer.Phone;
        SelectedCustomer = Customers.FirstOrDefault(customer =>
            string.Equals(customer.Name, invoice.Customer.Name, StringComparison.OrdinalIgnoreCase));

        Lines.Clear();
        foreach (var line in invoice.Lines)
        {
            Lines.Add(new InvoiceLine
            {
                Date = line.Date,
                Company = line.Company,
                From = line.From,
                To = line.To,
                Dispatch = line.Dispatch,
                Empties = line.Empties,
                Fb = line.Fb,
                Amount = line.Amount
            });
        }

        Advance = Convert.ToDouble(invoice.Advance);
        RecalculateTotals();
    }

    private void ResetNewLine()
    {
        NewLineDate = DateTimeOffset.Now;
        SelectedCompanyName = string.Empty;
        NewLineFrom = string.Empty;
        NewLineTo = string.Empty;
        NewLineDispatch = string.Empty;
        NewLineEmpties = string.Empty;
        NewLineFb = string.Empty;
        NewLineAmount = 0d;
    }

    private void OnLinesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems is not null)
        {
            foreach (var item in e.NewItems.OfType<InvoiceLine>())
            {
                item.PropertyChanged += OnLinePropertyChanged;
            }
        }

        if (e.OldItems is not null)
        {
            foreach (var item in e.OldItems.OfType<InvoiceLine>())
            {
                item.PropertyChanged -= OnLinePropertyChanged;
            }
        }

        RecalculateTotals();
    }

    private void OnLinePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(InvoiceLine.Amount))
        {
            RecalculateTotals();
        }
    }

    private void RecalculateTotals()
    {
        var subtotal = Lines.Sum(line => line.Amount);
        Subtotal = (double)subtotal;
        Total = Subtotal - Advance;
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value))
        {
            return false;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}
