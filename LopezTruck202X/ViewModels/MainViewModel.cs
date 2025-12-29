using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
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

    private DateTimeOffset _newLineDate = DateTimeOffset.Now;
    private string _newLineCompany = string.Empty;
    private string _newLineFrom = string.Empty;
    private string _newLineTo = string.Empty;
    private string _newLineDispatch = string.Empty;
    private string _newLineEmpties = string.Empty;
    private string _newLineFb = string.Empty;
    private double _newLineAmount;

    public MainViewModel()
    {
        Lines.CollectionChanged += OnLinesChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<InvoiceLine> Lines { get; } = new();

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
        set => SetField(ref _customerName, value);
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

    public string NewLineCompany
    {
        get => _newLineCompany;
        set => SetField(ref _newLineCompany, value);
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

    public string SubtotalFormatted => Subtotal.ToString("C", CultureInfo.CurrentCulture);

    public string TotalFormatted => Total.ToString("C", CultureInfo.CurrentCulture);

    public void AddLine()
    {
        var line = new InvoiceLine
        {
            Date = NewLineDate,
            Company = NewLineCompany,
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

    private void ResetNewLine()
    {
        NewLineDate = DateTimeOffset.Now;
        NewLineCompany = string.Empty;
        NewLineFrom = string.Empty;
        NewLineTo = string.Empty;
        NewLineDispatch = string.Empty;
        NewLineEmpties = string.Empty;
        NewLineFb = string.Empty;
        NewLineAmount = 0m;
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
