using InvoiceApp.Application.Interfaces;
using InvoiceApp.Infrastructure.Persistence;
using InvoiceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("InvoiceApp")
            ?? "Data Source=invoiceapp.db";

        services.AddDbContext<InvoiceAppDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IDetailGroupRepository, DetailGroupRepository>();
        services.AddScoped<IPriceAgreementRepository, PriceAgreementRepository>();
        services.AddScoped<IPlaceRepository, PlaceRepository>();
        services.AddScoped<ISubhaulerRepository, SubhaulerRepository>();
        services.AddScoped<ISettingRepository, SettingRepository>();

        return services;
    }
}
