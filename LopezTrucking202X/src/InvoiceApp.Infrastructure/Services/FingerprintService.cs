using System.Security.Cryptography;
using System.Text;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Application.Models;
using InvoiceApp.Domain.Entities;
using InvoiceApp.Domain.ValueObjects;

namespace InvoiceApp.Infrastructure.Services;

public sealed class FingerprintService : IFingerprintService
{
    public Fingerprint Create(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var bytes = Encoding.UTF8.GetBytes(value);
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(bytes);
        return new Fingerprint(ConvertToHex(hashBytes));
    }

    public Fingerprint Create(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(stream);
        return new Fingerprint(ConvertToHex(hashBytes));
    }

    public MixFingerprint CreateMixFingerprint(IEnumerable<MixFingerprintId> mixIds, bool useLegacyFormat = false)
    {
        ArgumentNullException.ThrowIfNull(mixIds);

        var ordered = mixIds
            .OrderBy(id => id.ItemType switch
            {
                ItemType.Company => 0,
                ItemType.From => 1,
                ItemType.To => 2,
                _ => 3
            })
            .ThenBy(id => id.Id)
            .ToArray();

        var fingerprintText = useLegacyFormat
            ? string.Join("|", ordered.Select(id => id.Id))
            : string.Join("|", ordered.Select(id => $"{id.ItemType}:{id.Id}"));

        var fingerprintHash = Create(fingerprintText);
        return new MixFingerprint(fingerprintText, fingerprintHash);
    }

    private static string ConvertToHex(byte[] bytes)
    {
        var builder = new StringBuilder(bytes.Length * 2);
        foreach (var value in bytes)
        {
            builder.Append(value.ToString("x2"));
        }

        return builder.ToString();
    }
}
