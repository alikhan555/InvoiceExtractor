using InvoiceExtractor.Application.DTOs;
using InvoiceExtractor.Application.Interfaces;
using InvoiceExtractor.Application.Mapping;

namespace InvoiceExtractor.Application.UseCases.ListInvoices;

public class ListInvoicesHandler
{
    private readonly IInvoiceRepository _repository;

    public ListInvoicesHandler(IInvoiceRepository repository) => _repository = repository;

    public async Task<IReadOnlyList<InvoiceListItemDto>> HandleAsync(CancellationToken ct = default)
    {
        var invoices = await _repository.ListAsync(ct);
        return invoices.Select(InvoiceMapper.ToListItem).ToList();
    }
}
