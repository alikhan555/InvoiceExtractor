# Build + publish with the .NET 10 SDK, run on the .NET 10 ASP.NET runtime.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore first for better layer caching.
COPY InvoiceExtractor.sln ./
COPY InvoiceExtractor.Domain/*.csproj InvoiceExtractor.Domain/
COPY InvoiceExtractor.Application/*.csproj InvoiceExtractor.Application/
COPY InvoiceExtractor.Infrastructure/*.csproj InvoiceExtractor.Infrastructure/
COPY InvoiceExtractor.Api/*.csproj InvoiceExtractor.Api/
RUN dotnet restore InvoiceExtractor.sln

COPY . .
RUN dotnet publish InvoiceExtractor.Api/InvoiceExtractor.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "InvoiceExtractor.Api.dll"]
