using InvoiceExtractor.Application.DTOs;
using InvoiceExtractor.Application.Interfaces;
using InvoiceExtractor.Application.Mapping;
using InvoiceExtractor.Application.Validation;

namespace InvoiceExtractor.Application.UseCases.UploadInvoice;

/// <summary>
/// Orchestrates the upload: store the PDF, call the AIEngine, validate the
/// result, persist the invoice, and return it with any validation warnings.
/// </summary>
public class UploadInvoiceHandler
{
    private readonly IFileStorage _storage;
    private readonly IAiEngineClient _aiEngine;
    private readonly IInvoiceRepository _repository;

    public UploadInvoiceHandler(
        IFileStorage storage,
        IAiEngineClient aiEngine,
        IInvoiceRepository repository)
    {
        _storage = storage;
        _aiEngine = aiEngine;
        _repository = repository;
    }

    public async Task<InvoiceResponseDto> HandleAsync(UploadInvoiceCommand command, CancellationToken ct = default)
    {
        // 1. Persist the original PDF, reference by path.
        var storedPath = await _storage.SaveAsync(command.Content, command.FileName, ct);

        // 2. Call the stateless AIEngine to extract structured data.
        var extraction = await _aiEngine.ExtractAsync(command.Content, command.FileName, ct);

        // 3. Validate (surfaced as warnings; we store regardless).
        var validation = InvoiceValidator.Validate(extraction);

        // 4. Map + persist.
        var invoice = InvoiceMapper.ToEntity(extraction, storedPath, command.FileName);
        await _repository.AddAsync(invoice, ct);

        // 5. Return the stored invoice with the extraction-time validation outcome.
        return InvoiceMapper.ToResponse(invoice, validation);
    }
}
