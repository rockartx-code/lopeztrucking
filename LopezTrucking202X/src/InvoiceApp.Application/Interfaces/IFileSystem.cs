namespace InvoiceApp.Application.Interfaces;

public interface IFileSystem
{
    bool FileExists(string path);
    Stream OpenRead(string path);
    Stream OpenWrite(string path);
    Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default);
    Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default);
}
