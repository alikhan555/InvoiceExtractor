# API (.NET 10, Clean Architecture)

ASP.NET Core Web API that orchestrates upload → extract → validate → store, and
serves invoices to the UI. EF Core + PostgreSQL for persistence.

## Projects

| Project | Responsibility |
|---|---|
| `InvoiceExtractor.Domain` | Entities (`Invoice`, `LineItem`). No dependencies. |
| `InvoiceExtractor.Application` | Interfaces, DTOs, use cases, validation, mapping. |
| `InvoiceExtractor.Infrastructure` | EF Core `DbContext`, repository, AIEngine HTTP client, file storage. |
| `InvoiceExtractor.Api` | Controllers, DI wiring, `Program.cs`. |

Dependency direction: `Api → Application → Domain`, with `Infrastructure`
implementing `Application` interfaces.

## Prerequisites

- A running PostgreSQL (see the root `docker-compose.yml` for a one-liner) with a
  `invoiceextractor` database, or update `ConnectionStrings:DefaultConnection`.
- The AIEngine running at `AiEngine:BaseUrl` (default `http://localhost:8000`).

## Run

```bash
cd API
dotnet run --project InvoiceExtractor.Api        # http://localhost:5080 (Swagger at /swagger)
```

Migrations are applied automatically on startup (with a short retry loop while
Postgres comes up).

## Endpoints

```
POST /api/invoices/upload   multipart form field "file" (a text-based PDF)
GET  /api/invoices/{id}     one invoice + line items
GET  /api/invoices          list invoices
```

## Migrations

Uses the local `dotnet-ef` tool (pinned in `.config/dotnet-tools.json`):

```bash
cd API
dotnet tool restore
dotnet dotnet-ef migrations add <Name> \
  --project InvoiceExtractor.Infrastructure \
  --startup-project InvoiceExtractor.Api
```

## Note on the target framework

Projects target **net10.0** and run on the .NET 10 runtime natively. EF Core,
Npgsql, and the ASP.NET packages are all on the 10.x line. The Docker image uses
the `mcr.microsoft.com/dotnet/{sdk,aspnet}:10.0` images.
