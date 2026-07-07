namespace InvoiceExtractor.Domain.Entities;

/// <summary>A single billed line on an invoice.</summary>
public class LineItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid InvoiceId { get; set; }

    public string? Description { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineAmount { get; set; }

    public Invoice? Invoice { get; set; }
}
