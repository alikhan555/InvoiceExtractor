using System.Globalization;
using InvoiceExtractor.Application.DTOs;
using InvoiceExtractor.Application.Validation;
using InvoiceExtractor.Domain.Entities;

namespace InvoiceExtractor.Application.Mapping;

/// <summary>Maps between extraction DTOs, domain entities, and response DTOs.</summary>
public static class InvoiceMapper
{
    /// <summary>Build a domain Invoice from an AIEngine extraction result.</summary>
    public static Invoice ToEntity(ExtractionResultDto e, string storedPath, string originalFileName)
    {
        var invoice = new Invoice
        {
            VendorName = e.VendorName,
            InvoiceNumber = e.InvoiceNumber,
            InvoiceDate = ParseDate(e.InvoiceDate),
            DueDate = ParseDate(e.DueDate),
            Currency = e.Currency,
            Subtotal = e.Subtotal ?? 0m,
            Tax = e.Tax ?? 0m,
            Total = e.Total ?? 0m,
            OriginalFilePath = storedPath,
            OriginalFileName = originalFileName,
            CreatedAt = DateTime.UtcNow,
        };

        invoice.LineItems = e.LineItems.Select(li => new LineItem
        {
            InvoiceId = invoice.Id,
            Description = li.Description,
            Quantity = li.Quantity ?? 0m,
            UnitPrice = li.UnitPrice ?? 0m,
            LineAmount = li.LineAmount ?? 0m,
        }).ToList();

        return invoice;
    }

    public static InvoiceResponseDto ToResponse(Invoice inv, ValidationResult validation)
    {
        return new InvoiceResponseDto
        {
            Id = inv.Id,
            VendorName = inv.VendorName,
            InvoiceNumber = inv.InvoiceNumber,
            InvoiceDate = inv.InvoiceDate,
            DueDate = inv.DueDate,
            Currency = inv.Currency,
            Subtotal = inv.Subtotal,
            Tax = inv.Tax,
            Total = inv.Total,
            OriginalFileName = inv.OriginalFileName,
            CreatedAt = inv.CreatedAt,
            IsValid = validation.IsValid,
            ValidationMessages = validation.Messages,
            LineItems = inv.LineItems.Select(li => new LineItemDto
            {
                Id = li.Id,
                Description = li.Description,
                Quantity = li.Quantity,
                UnitPrice = li.UnitPrice,
                LineAmount = li.LineAmount,
            }).ToList(),
        };
    }

    public static InvoiceListItemDto ToListItem(Invoice inv) => new()
    {
        Id = inv.Id,
        VendorName = inv.VendorName,
        InvoiceNumber = inv.InvoiceNumber,
        InvoiceDate = inv.InvoiceDate,
        Currency = inv.Currency,
        Total = inv.Total,
        CreatedAt = inv.CreatedAt,
    };

    private static DateTime? ParseDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        // AIEngine is asked for strict YYYY-MM-DD; fall back to a general parse.
        if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var exact))
            return exact.Date;

        if (DateTime.TryParse(value, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var loose))
            return loose.Date;

        return null;
    }
}
