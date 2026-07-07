using System.Net.Http.Headers;
using System.Text.Json;
using InvoiceExtractor.Application.DTOs;
using InvoiceExtractor.Application.Exceptions;
using InvoiceExtractor.Application.Interfaces;

namespace InvoiceExtractor.Infrastructure.AiEngine;

/// <summary>Typed HTTP client for the Python AIEngine's POST /extract endpoint.</summary>
public class AiEngineClient : IAiEngineClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly HttpClient _http;

    public AiEngineClient(HttpClient http) => _http = http;

    public async Task<ExtractionResultDto> ExtractAsync(
        byte[] content,
        string fileName,
        CancellationToken ct = default)
    {
        using var form = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(content);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        form.Add(fileContent, "file", fileName);

        HttpResponseMessage response;
        try
        {
            response = await _http.PostAsync("/extract", form, ct);
        }
        catch (HttpRequestException ex)
        {
            throw new AiEngineException($"Could not reach the AIEngine: {ex.Message}");
        }
        catch (TaskCanceledException) when (!ct.IsCancellationRequested)
        {
            // Timeout (not a caller-initiated cancellation).
            throw new AiEngineException("The AIEngine request timed out.");
        }

        using (response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(ct);
                throw new AiEngineException(
                    $"AIEngine returned {(int)response.StatusCode}: {body}");
            }

            await using var stream = await response.Content.ReadAsStreamAsync(ct);
            var result = await JsonSerializer.DeserializeAsync<ExtractionResultDto>(stream, JsonOptions, ct);

            return result ?? throw new AiEngineException("AIEngine returned an empty body.");
        }
    }
}
