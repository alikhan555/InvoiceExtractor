namespace InvoiceExtractor.Application.DTOs;

/// <summary>Full invoice payload returned to the UI, including validation outcome.</summary>
public class InvoiceResponseDto
{
    public Guid Id { get; set; }

    public string? VendorName { get; set; }
    public string? InvoiceNumber { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Currency { get; set; }

    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }

    public string OriginalFileName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public List<LineItemDto> LineItems { get; set; } = new();

    /// <summary>True when all validation checks passed.</summary>
    public bool IsValid { get; set; }

    /// <summary>Human-readable validation warnings (empty when valid).</summary>
    public List<string> ValidationMessages { get; set; } = new();
}

public class LineItemDto
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineAmount { get; set; }
}
