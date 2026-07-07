using InvoiceExtractor.Application.DTOs;

namespace InvoiceExtractor.Application.Interfaces;

/// <summary>Client for the internal Python AIEngine (/extract).</summary>
public interface IAiEngineClient
{
    Task<ExtractionResultDto> ExtractAsync(
        byte[] content,
        string fileName,
        CancellationToken ct = default);
}
