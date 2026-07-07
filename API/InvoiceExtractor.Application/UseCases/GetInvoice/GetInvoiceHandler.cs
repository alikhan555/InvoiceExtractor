using InvoiceExtractor.Application.DTOs;
using InvoiceExtractor.Application.Interfaces;
using InvoiceExtractor.Application.Mapping;
using InvoiceExtractor.Application.Validation;

namespace InvoiceExtractor.Application.UseCases.GetInvoice;

public class GetInvoiceHandler
{
    private readonly IInvoiceRepository _repository;

    public GetInvoiceHandler(IInvoiceRepository repository) => _repository = repository;

    public async Task<InvoiceResponseDto?> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var invoice = await _repository.GetByIdAsync(id, ct);
        if (invoice is null)
            return null;

        var validation = InvoiceValidator.Validate(invoice);
        return InvoiceMapper.ToResponse(invoice, validation);
    }
}
