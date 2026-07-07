namespace InvoiceExtractor.Application.UseCases.UploadInvoice;

/// <summary>Input for the upload use case: the raw PDF bytes plus metadata.</summary>
public record UploadInvoiceCommand(byte[] Content, string FileName, string? ContentType);
