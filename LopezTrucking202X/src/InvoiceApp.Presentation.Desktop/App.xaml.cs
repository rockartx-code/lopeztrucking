using System;
using System.Windows;
using InvoiceApp.Application.UseCases.Commands;
using InvoiceApp.Application.UseCases.Queries;
using InvoiceApp.Infrastructure;
using InvoiceApp.Presentation.Desktop.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InvoiceApp.Presentation.Desktop;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(builder =>
            {
                builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(context.Configuration, services);
            })
            .Build();
    }

    public IServiceProvider Services => _host.Services;

    protected override void OnStartup(StartupEventArgs e)
    {
        _host.Start();
        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        base.OnExit(e);
    }

    private static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        services.AddSingleton(configuration);
        services.AddInfrastructure(configuration);
        services.AddTransient<AddDetailGroupHandler>();
        services.AddTransient<FindPriceAgreementByMixHandler>();
        services.AddSingleton<AddDetailGroupViewModel>();
        services.AddSingleton<InvoiceEditorViewModel>();
        services.AddSingleton<MainWindow>();
    }
}
