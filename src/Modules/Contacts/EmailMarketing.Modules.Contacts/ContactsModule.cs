using EmailMarketing.Modules.Contacts.Repositories;
using EmailMarketing.Modules.Contacts.Services;
using EmailMarketing.Modules.Contacts.UnitOfWorks;
using EmailMarketing.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EmailMarketing.Modules.Contacts;

public class ContactsModule : IModule
{
    public string Name => "Contacts";

    public void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IContactGroupRepository, ContactGroupRepository>();
        services.AddScoped<IContactUploadRepository, ContactUploadRepository>();
        services.AddScoped<IContactUploadFieldMapRepository, ContactUploadFieldMapRepository>();
        services.AddScoped<IContactUploadGroupRepository, ContactUploadGroupRepository>();
        services.AddScoped<IContactValueMapRepository, ContactValueMapRepository>();
        services.AddScoped<IFieldMapRepository, FieldMapRepository>();
        services.AddScoped<IDownloadQueueRepository, DownloadQueueRepository>();
        services.AddScoped<IDownloadQueueSubEntityRepository, DownloadQueueSubEntityRepository>();
        services.AddScoped<IGroupReadRepository, GroupReadRepository>();

        services.AddScoped<IContactUnitOfWork, ContactUnitOfWork>();
        services.AddScoped<IContactUploadUnitOfWork, ContactUploadUnitOfWork>();
        services.AddScoped<IFieldMapUnitOfWork, FieldMapUnitOfWork>();
        services.AddScoped<IContactExportUnitOfWork, ContactExportUnitOfWork>();

        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IContactUploadService, ContactUploadService>();
        services.AddScoped<IFieldMapService, FieldMapService>();
        services.AddScoped<IContactExportService, ContactExportService>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ContactsModule).Assembly));
    }
}
