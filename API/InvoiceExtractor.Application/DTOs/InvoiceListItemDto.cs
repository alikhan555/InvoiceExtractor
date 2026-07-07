namespace InvoiceExtractor.Application.DTOs;

/// <summary>Compact invoice summary for the list view.</summary>
public class InvoiceListItemDto
{
    public Guid Id { get; set; }
    public string? VendorName { get; set; }
    public string? InvoiceNumber { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public string? Currency { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
}
