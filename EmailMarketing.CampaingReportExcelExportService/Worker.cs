using EmailMarketing.CampaingReportExcelExportService.Core;
using EmailMarketing.CampaingReportExcelExportService.Templates;
using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Enums;
using EmailMarketing.Framework.Services.Campaigns;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmailMarketing.CampaingReportExcelExportService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IExportMailerService _exportMailerService;
        private readonly ICampaignReportExportService _campaignReportExportService;
        private WorkerSettings _workerSettings;

        public Worker(ILogger<Worker> logger, ICampaignReportExportService campaignReportExportService,
            IExportMailerService exportMailerService, IOptions<WorkerSettings> workerSettings)
        {
            _logger = logger;
            _exportMailerService = exportMailerService;
            _campaignReportExportService = campaignReportExportService;
            _workerSettings = workerSettings.Value;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker started at: {DateTime.Now}");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker Service running at: {time}", DateTimeOffset.Now);

                try
                {
                    var result = await _campaignReportExportService.GetDownloadQueue();

                    foreach (var item in result)
                    {
                        try
                        {

                            var importResult = await _campaignReportExportService.GetDownloadQueueByIdAsync(item.Id);

                            if (item.DownloadQueueFor == DownloadQueueFor.CampaignAllReportExport)
                            {
                                await _campaignReportExportService.ExcelExportForAllCampaignAsync(item);
                            }
                            else if (item.DownloadQueueFor == DownloadQueueFor.CampaignDetailsReportExport)
                            {
                                await _campaignReportExportService.ExcelExportForCampaignWiseAsync(item);
                            }
                            importResult.IsProcessing = false;
                            importResult.IsSucceed = true;
                            importResult.LastModified = DateTime.Now;
                            importResult.LastModifiedBy = item.UserId;
                            await _campaignReportExportService.UpdateDownloadQueueAync(importResult);

                            _logger.LogInformation($"Successfully Exported. FileUrl: {item.FileUrl}{item.FileName}");


                            //Sending Email
                            if (item.IsSendEmailNotify)
                            {
                                var url = item.FileUrl;

                                var emailSubject = "Campaign Export Confirmation";
                                var campReportExportTemplate = new CampReportExportTemplate("Sir", _workerSettings.CompanyFullName,
                                                                    _workerSettings.CompanyShortName, _workerSettings.CompanyWebsiteUrl);

                                var emailBody = campReportExportTemplate.TransformText();

                                await _exportMailerService.SendEmailAsync(item.SendEmailAddress, emailSubject, emailBody, url);
                            }
                        }
                        catch (Exception ex)
                        {
                            //_logger.LogError($"Campaign export failed : {item.FileUrl} - {item.FileName}");
                            _logger.LogError(ex, $"Campaign Export Failed for Url = {item.FileUrl}{item.FileName} and Error Message: " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error message : {ex.Message}");
                }

                await Task.Delay(30000, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker stopped at: {DateTime.Now}");
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"Worker disposed at: {DateTime.Now}");
            base.Dispose();
        }
    }
}
