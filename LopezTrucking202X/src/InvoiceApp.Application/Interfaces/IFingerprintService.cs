using InvoiceApp.Application.Models;
using InvoiceApp.Domain.ValueObjects;

namespace InvoiceApp.Application.Interfaces;

public interface IFingerprintService
{
    Fingerprint Create(string value);
    Fingerprint Create(Stream stream);
    MixFingerprint CreateMixFingerprint(IEnumerable<MixFingerprintId> mixIds, bool useLegacyFormat = false);
}
