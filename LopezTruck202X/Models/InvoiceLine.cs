using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace LopezTruck202X.Models;

public sealed class InvoiceLine : INotifyPropertyChanged
{
    private DateTimeOffset _date = DateTimeOffset.Now;
    private string _company = string.Empty;
    private string _from = string.Empty;
    private string _to = string.Empty;
    private string _dispatch = string.Empty;
    private string _empties = string.Empty;
    private string _fb = string.Empty;
    private decimal _amount;

    public event PropertyChangedEventHandler? PropertyChanged;

    public DateTimeOffset Date
    {
        get => _date;
        set
        {
            if (SetField(ref _date, value))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateDisplay)));
            }
        }
    }

    public string Company
    {
        get => _company;
        set => SetField(ref _company, value);
    }

    public string From
    {
        get => _from;
        set => SetField(ref _from, value);
    }

    public string To
    {
        get => _to;
        set => SetField(ref _to, value);
    }

    public string Dispatch
    {
        get => _dispatch;
        set => SetField(ref _dispatch, value);
    }

    public string Empties
    {
        get => _empties;
        set => SetField(ref _empties, value);
    }

    public string Fb
    {
        get => _fb;
        set => SetField(ref _fb, value);
    }

    public decimal Amount
    {
        get => _amount;
        set
        {
            if (SetField(ref _amount, value))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AmountDisplay)));
            }
        }
    }

    public string DateDisplay => Date.ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);

    public string AmountDisplay => Amount.ToString("C", CultureInfo.CurrentCulture);

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value))
        {
            return;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
