namespace InvoiceExtractor.Application.DTOs;

/// <summary>
/// Raw result returned by the AIEngine (/extract). Everything is nullable so we
/// can tell "not found" apart from "zero" during validation. Dates arrive as
/// strings in YYYY-MM-DD form.
/// </summary>
public class ExtractionResultDto
{
    public string? VendorName { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? InvoiceDate { get; set; }
    public string? DueDate { get; set; }
    public string? Currency { get; set; }
    public decimal? Subtotal { get; set; }
    public decimal? Tax { get; set; }
    public decimal? Total { get; set; }
    public List<ExtractionLineItemDto> LineItems { get; set; } = new();
}

public class ExtractionLineItemDto
{
    public string? Description { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? LineAmount { get; set; }
}
