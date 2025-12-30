using InvoiceApp.Domain.Entities;
using InvoiceApp.Domain.ValueObjects;

namespace InvoiceApp.Application.Models;

public sealed record MixFingerprint(string FingerprintText, Fingerprint FingerprintHash);

public sealed record MixFingerprintId(Guid Id, ItemType ItemType);
