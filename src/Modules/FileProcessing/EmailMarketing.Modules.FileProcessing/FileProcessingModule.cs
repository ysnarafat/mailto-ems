using EmailMarketing.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EmailMarketing.Modules.FileProcessing;

public class FileProcessingModule : IModule
{
    public string Name => "FileProcessing";

    public void RegisterServices(IServiceCollection services)
    {
        // Register file processing services
        // services.AddScoped<IExcelImportService, ExcelImportService>();
        // services.AddScoped<IExcelExportService, ExcelExportService>();
        // services.AddScoped<IFileProcessingService, FileProcessingService>();
        
        // Register background services
        // services.AddHostedService<ExcelWorkerService>();
        // services.AddHostedService<ExcelExportWorkerService>();
        // services.AddHostedService<CampaignReportExcelExportService>();
        
        // Register MediatR handlers for this module
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(FileProcessingModule).Assembly));
    }
}