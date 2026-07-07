using InvoiceExtractor.Application.Interfaces;
using InvoiceExtractor.Infrastructure.AiEngine;
using InvoiceExtractor.Infrastructure.Persistence;
using InvoiceExtractor.Infrastructure.Persistence.Repositories;
using InvoiceExtractor.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceExtractor.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core / PostgreSQL
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IInvoiceRepository, InvoiceRepository>();

        // File storage
        services.Configure<FileStorageOptions>(configuration.GetSection(FileStorageOptions.SectionName));
        services.AddSingleton<IFileStorage, FileStorage>();

        // AIEngine typed HTTP client
        var aiOptions = new AiEngineOptions();
        configuration.GetSection(AiEngineOptions.SectionName).Bind(aiOptions);
        services.Configure<AiEngineOptions>(configuration.GetSection(AiEngineOptions.SectionName));

        services.AddHttpClient<IAiEngineClient, AiEngineClient>(client =>
        {
            client.BaseAddress = new Uri(aiOptions.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(aiOptions.TimeoutSeconds);
        });

        return services;
    }
}
