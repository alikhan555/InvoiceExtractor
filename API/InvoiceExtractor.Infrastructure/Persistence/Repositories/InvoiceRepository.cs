using InvoiceExtractor.Application.Interfaces;
using InvoiceExtractor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoiceExtractor.Infrastructure.Persistence.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _db;

    public InvoiceRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Invoice invoice, CancellationToken ct = default)
    {
        _db.Invoices.Add(invoice);
        await _db.SaveChangesAsync(ct);
    }

    public Task<Invoice?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _db.Invoices
            .Include(i => i.LineItems)
            .FirstOrDefaultAsync(i => i.Id == id, ct);

    public async Task<IReadOnlyList<Invoice>> ListAsync(CancellationToken ct = default) =>
        await _db.Invoices
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(ct);
}
