namespace InvoiceExtractor.Application.Validation;

public record ValidationResult(bool IsValid, List<string> Messages)
{
    public static ValidationResult From(List<string> messages) =>
        new(messages.Count == 0, messages);
}
