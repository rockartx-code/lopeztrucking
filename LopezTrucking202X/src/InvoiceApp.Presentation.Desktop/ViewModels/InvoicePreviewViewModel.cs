using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InvoiceApp.Presentation.Desktop.ViewModels;

public sealed class InvoicePreviewViewModel
{
    public InvoicePreviewViewModel(
        string subhaulerName,
        string? subhaulerContact,
        string? subhaulerPhone,
        string invoiceNo,
        DateTime invoiceDate,
        string checkNo,
        decimal subtotal,
        decimal advance,
        decimal total,
        IEnumerable<InvoicePreviewDetailGroup> detailGroups)
    {
        SubhaulerName = subhaulerName;
        SubhaulerContact = subhaulerContact;
        SubhaulerPhone = subhaulerPhone;
        InvoiceNo = invoiceNo;
        InvoiceDate = invoiceDate;
        CheckNo = checkNo;
        Subtotal = subtotal;
        Advance = advance;
        Total = total;
        DetailGroups = new ObservableCollection<InvoicePreviewDetailGroup>(detailGroups);
    }

    public string SubhaulerName { get; }

    public string? SubhaulerContact { get; }

    public string? SubhaulerPhone { get; }

    public string InvoiceNo { get; }

    public DateTime InvoiceDate { get; }

    public string CheckNo { get; }

    public decimal Subtotal { get; }

    public decimal Advance { get; }

    public decimal Total { get; }

    public ObservableCollection<InvoicePreviewDetailGroup> DetailGroups { get; }
}

public sealed record InvoicePreviewDetailGroup(
    string Dispatch,
    string Fb,
    DateTime DetailDate,
    int Empties,
    decimal AmountBase,
    decimal Total,
    string Companies,
    string FromPlaces,
    string ToPlaces);
