namespace InvoiceApp.Presentation.Desktop.ViewModels;

public sealed class InvoiceBrowserItem
{
    public InvoiceBrowserItem(string invoiceNumber, DateTime invoiceDate, string subhaulerName)
    {
        InvoiceNumber = invoiceNumber;
        InvoiceDate = invoiceDate;
        SubhaulerName = subhaulerName;
    }

    public string InvoiceNumber { get; }

    public DateTime InvoiceDate { get; }

    public string SubhaulerName { get; }
}
