namespace InvoiceExtractor.Infrastructure.AiEngine;

public class AiEngineOptions
{
    public const string SectionName = "AiEngine";

    /// <summary>Base URL of the Python AIEngine, e.g. http://localhost:8000.</summary>
    public string BaseUrl { get; set; } = "http://localhost:8000";

    /// <summary>Request timeout in seconds (LLM calls can be slow).</summary>
    public int TimeoutSeconds { get; set; } = 120;
}
