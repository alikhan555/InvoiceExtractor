namespace InvoiceExtractor.Application.Exceptions;

/// <summary>Raised when the AIEngine call fails or returns an unusable response.</summary>
public class AiEngineException : Exception
{
    public AiEngineException(string message) : base(message) { }
}
