using Autofac;
using Autofac.Extensions.DependencyInjection;
using EmailMarketing.Common.Services;
using EmailMarketing.ExcelWorkerService.Core;
using EmailMarketing.ExcelWorkerService.Services;
using EmailMarketing.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace EmailMarketing.ExcelWorkerService
{
    public class Program
    {
        private static string _connectionString;
        private static string _migrationAssemblyName;
        private IConfiguration _configuration;

        public static void Main(string[] args)
        {
            var _configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", false, true)
                                .AddEnvironmentVariables()
                                .Build();

            var workerSettings = _configuration.GetSection("WorkerSettings").Get<WorkerSettings>();

            _connectionString = _configuration.GetConnectionString("DefaultConnection");

            _migrationAssemblyName = typeof(Worker).Assembly.FullName;

            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .WriteTo.File(workerSettings.ContactImportLogUrl, rollingInterval: RollingInterval.Day)
                        .CreateLogger();

            try
            {
                Log.Information("Application Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseWindowsService()
                // .UseSystemd()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog()
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new FrameworkModule(_connectionString,
                        _migrationAssemblyName));
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.Configure<WorkerSmtpSettings>(hostContext.Configuration.GetSection("SmtpSetting"));
                    services.Configure<WorkerSettings>(hostContext.Configuration.GetSection("WorkerSettings"));
                    services.AddSingleton<IMailerService, WorkerMailerService>();
                    services.AddSingleton<ICurrentUserService, WorkerCurrentUserService>();
                    services.AddSingleton<IDateTime, WorkerDateTimeService>();
                    //services.AddHttpContextAccessor();
                });
    }
}
