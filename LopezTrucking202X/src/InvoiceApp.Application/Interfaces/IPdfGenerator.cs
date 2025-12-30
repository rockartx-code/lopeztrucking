using InvoiceApp.Application.DTOs;

namespace InvoiceApp.Application.Interfaces;

public interface IPdfGenerator
{
    Task<byte[]> GenerateInvoiceAsync(InvoiceDto invoice, CancellationToken cancellationToken = default);
}
