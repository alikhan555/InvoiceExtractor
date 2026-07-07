namespace InvoiceExtractor.Domain.Entities;

/// <summary>
/// An extracted invoice. Header fields are nullable because the source PDF may
/// not contain them; monetary totals default to zero when absent.
/// </summary>
public class Invoice
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? VendorName { get; set; }
    public string? InvoiceNumber { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Currency { get; set; }

    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }

    /// <summary>Path to the original stored PDF.</summary>
    public string OriginalFilePath { get; set; } = string.Empty;

    /// <summary>Original client-supplied file name (for display).</summary>
    public string OriginalFileName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<LineItem> LineItems { get; set; } = new();
}
