using InvoiceExtractor.Application.DTOs;
using InvoiceExtractor.Application.Exceptions;
using InvoiceExtractor.Application.UseCases.GetInvoice;
using InvoiceExtractor.Application.UseCases.ListInvoices;
using InvoiceExtractor.Application.UseCases.UploadInvoice;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceExtractor.Api.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly UploadInvoiceHandler _upload;
    private readonly GetInvoiceHandler _get;
    private readonly ListInvoicesHandler _list;
    private readonly long _maxBytes;

    public InvoicesController(
        UploadInvoiceHandler upload,
        GetInvoiceHandler get,
        ListInvoicesHandler list,
        IConfiguration configuration)
    {
        _upload = upload;
        _get = get;
        _list = list;
        _maxBytes = configuration.GetValue<long?>("Upload:MaxBytes") ?? 10 * 1024 * 1024;
    }

    /// <summary>Upload one text-based PDF → extract → validate → store → return result.</summary>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(InvoiceResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Upload(IFormFile? file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "A PDF file is required." });

        var isPdf = string.Equals(file.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase)
                    || Path.GetExtension(file.FileName).Equals(".pdf", StringComparison.OrdinalIgnoreCase);
        if (!isPdf)
            return BadRequest(new { error = "Only PDF files are supported." });

        if (file.Length > _maxBytes)
            return StatusCode(StatusCodes.Status413PayloadTooLarge,
                new { error = $"File exceeds the {_maxBytes / (1024 * 1024)} MB limit." });

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms, ct);
        var bytes = ms.ToArray();

        try
        {
            var result = await _upload.HandleAsync(
                new UploadInvoiceCommand(bytes, file.FileName, file.ContentType), ct);
            return Ok(result);
        }
        catch (AiEngineException ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new { error = ex.Message });
        }
    }

    /// <summary>Get one invoice with its line items.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(InvoiceResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var invoice = await _get.HandleAsync(id, ct);
        return invoice is null ? NotFound() : Ok(invoice);
    }

    /// <summary>List all invoices (no filters in V1).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<InvoiceListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var invoices = await _list.HandleAsync(ct);
        return Ok(invoices);
    }
}
