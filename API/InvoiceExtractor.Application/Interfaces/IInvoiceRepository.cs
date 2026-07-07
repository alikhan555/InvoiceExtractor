using InvoiceExtractor.Domain.Entities;

namespace InvoiceExtractor.Application.Interfaces;

public interface IInvoiceRepository
{
    Task AddAsync(Invoice invoice, CancellationToken ct = default);

    Task<Invoice?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyList<Invoice>> ListAsync(CancellationToken ct = default);
}
