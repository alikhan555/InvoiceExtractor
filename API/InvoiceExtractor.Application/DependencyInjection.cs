using InvoiceExtractor.Application.UseCases.GetInvoice;
using InvoiceExtractor.Application.UseCases.ListInvoices;
using InvoiceExtractor.Application.UseCases.UploadInvoice;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceExtractor.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<UploadInvoiceHandler>();
        services.AddScoped<GetInvoiceHandler>();
        services.AddScoped<ListInvoicesHandler>();
        return services;
    }
}
