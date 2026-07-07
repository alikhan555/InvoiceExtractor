namespace InvoiceExtractor.Application.Interfaces;

/// <summary>Persists the original uploaded PDF and returns a reference path.</summary>
public interface IFileStorage
{
    Task<string> SaveAsync(
        byte[] content,
        string fileName,
        CancellationToken ct = default);
}
