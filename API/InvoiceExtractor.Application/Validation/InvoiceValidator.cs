using InvoiceExtractor.Application.DTOs;
using InvoiceExtractor.Domain.Entities;

namespace InvoiceExtractor.Application.Validation;

/// <summary>
/// Applies the V1 validation rules (§8). Failures are surfaced as messages —
/// there is no workflow state, invoices are stored regardless.
/// </summary>
public static class InvoiceValidator
{
    // Absolute tolerance for rounding differences in currency amounts.
    private const decimal Tolerance = 0.02m;

    /// <summary>Validate the raw extraction (includes required-field presence checks).</summary>
    public static ValidationResult Validate(ExtractionResultDto e)
    {
        var messages = new List<string>();

        if (string.IsNullOrWhiteSpace(e.InvoiceNumber))
            messages.Add("Invoice number is missing.");
        if (string.IsNullOrWhiteSpace(e.VendorName))
            messages.Add("Vendor name is missing.");
        if (e.Total is null)
            messages.Add("Total is missing.");

        AddTotalsChecks(
            messages,
            e.Subtotal,
            e.Tax,
            e.Total,
            e.LineItems.Select(li => li.LineAmount ?? 0m));

        return ValidationResult.From(messages);
    }

    /// <summary>Re-validate a stored invoice (numeric consistency only).</summary>
    public static ValidationResult Validate(Invoice inv)
    {
        var messages = new List<string>();

        if (string.IsNullOrWhiteSpace(inv.InvoiceNumber))
            messages.Add("Invoice number is missing.");
        if (string.IsNullOrWhiteSpace(inv.VendorName))
            messages.Add("Vendor name is missing.");

        AddTotalsChecks(
            messages,
            inv.Subtotal,
            inv.Tax,
            inv.Total,
            inv.LineItems.Select(li => li.LineAmount));

        return ValidationResult.From(messages);
    }

    private static void AddTotalsChecks(
        List<string> messages,
        decimal? subtotal,
        decimal? tax,
        decimal? total,
        IEnumerable<decimal> lineAmounts)
    {
        // subtotal + tax ≈ total
        if (subtotal is not null && tax is not null && total is not null)
        {
            var expected = subtotal.Value + tax.Value;
            if (Math.Abs(expected - total.Value) > Tolerance)
                messages.Add(
                    $"Totals do not add up: subtotal ({subtotal:0.00}) + tax ({tax:0.00}) " +
                    $"= {expected:0.00}, but total is {total:0.00}.");
        }

        // sum(line amounts) ≈ subtotal
        var lineSum = lineAmounts.Sum();
        if (subtotal is not null && lineSum > 0m && Math.Abs(lineSum - subtotal.Value) > Tolerance)
        {
            messages.Add(
                $"Line items do not sum to the subtotal: line total is {lineSum:0.00}, " +
                $"subtotal is {subtotal:0.00}.");
        }
    }
}
