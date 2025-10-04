using ClosedXML.Excel;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Common.Extensions;
using EmailMarketing.Framework.Entities;
using EmailMarketing.Framework.Entities.Campaigns;
using EmailMarketing.Framework.Enums;
using EmailMarketing.Framework.UnitOfWorks.Campaigns;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Services.Campaigns
{
    public class CampaignReportExportService : ICampaignReportExportService
    {
        private ICampaignReportExportUnitOfWork _campaignReportExportUnitOfWork;
        private ICampaignReportUnitOfWork _campaignReportUnitOfWork;
        private ICampaignUnitOfWork _campaignUnitOfWork;
        public CampaignReportExportService(
            ICampaignReportExportUnitOfWork campaignReportExportUnitOfWork,
            ICampaignReportUnitOfWork campaignReportUnitOfWork,
            ICampaignUnitOfWork campaignUnitOfWork)
        {
            _campaignReportExportUnitOfWork = campaignReportExportUnitOfWork;
            _campaignReportUnitOfWork = campaignReportUnitOfWork;
            _campaignUnitOfWork = campaignUnitOfWork;
        }

        public async Task<DownloadQueueSubEntity> GetAllDownloadQueueSubEntityById(int id)
        {
            var result = await _campaignReportExportUnitOfWork.DownloadQueueSubEntityRepository.GetFirstOrDefaultAsync<DownloadQueueSubEntity>(
                x => x,
                x => x.DownloadQueueId == id,
                null,
                true);
            return result;
        }
        public async Task<(IList<DownloadQueue> Items, int Total, int TotalFilter)> GetAllCampaignReportsFromDownloadQueueAsync(
          Guid? userId,
          string searchText,
          string orderBy,
          int pageIndex,
          int pageSize)
        {
            var columnsMap = new Dictionary<string, Expression<Func<DownloadQueue, object>>>()
            {
                ["Created"] = v => v.Created
            };
            var result = (await _campaignReportExportUnitOfWork.DownloadQueueRepository.GetAsync(x => x,
                                                  x => !x.IsDeleted && x.IsActive &&
                                                  (!userId.HasValue || x.UserId == userId.Value) &&
                                                  x.FileName.Contains(searchText) &&
                                                  (x.DownloadQueueFor == DownloadQueueFor.CampaignAllReportExport || x.DownloadQueueFor == DownloadQueueFor.CampaignDetailsReportExport),
                                                  x => x.ApplyOrdering(columnsMap, orderBy),
                                                  x => x.Include(y => y.DownloadQueueSubEntities),
                                                  pageIndex, pageSize,
                                                  true));

            if (result.Items == null) throw new NotFoundException(nameof(DownloadQueue), userId);

            result.Total = await _campaignReportExportUnitOfWork.DownloadQueueRepository.GetCountAsync(x => x.UserId == userId && (x.DownloadQueueFor == DownloadQueueFor.CampaignAllReportExport || x.DownloadQueueFor == DownloadQueueFor.CampaignDetailsReportExport));

            return (result.Items, result.Total, result.TotalFilter);
        }
        public async Task<IList<DownloadQueue>> GetDownloadQueue()
        {
            var result = await _campaignReportExportUnitOfWork.DownloadQueueRepository.GetAsync(
                x => x,
                x => (x.IsProcessing == true || x.IsSucceed == false) && (x.DownloadQueueFor == DownloadQueueFor.CampaignAllReportExport || x.DownloadQueueFor == DownloadQueueFor.CampaignDetailsReportExport),
                null,
                x => x.Include(y => y.DownloadQueueSubEntities),
                true);
            return result;
        }

        public async Task<DownloadQueue> GetDownloadQueueByIdAsync(int downloadQueueId)
        {
            var contactUpload = await _campaignReportExportUnitOfWork.DownloadQueueRepository.GetFirstOrDefaultAsync(
                x => x,
                x => x.Id == downloadQueueId,
                null,
                true);
            return contactUpload;
        }
        public async Task UpdateDownloadQueueAync(DownloadQueue downloadQueue)
        {
            await _campaignReportExportUnitOfWork.DownloadQueueRepository.UpdateAsync(downloadQueue);
            await _campaignReportExportUnitOfWork.SaveChangesAsync();
        }
        public void Dispose()
        {
            _campaignReportUnitOfWork?.Dispose();
        }
        public async Task<IList<CampaignReport>> GetAllCampaignReportAsync(Guid? userId)
        {
            var result = (await _campaignReportUnitOfWork.CampaingReportRepository.GetAsync(x => x,
                                                   x => !x.IsDeleted && x.IsActive &&
                                                   (!userId.HasValue || x.Campaign.UserId == userId.Value),
                                                   x => x.OrderBy(o => o.Contact.Email),
                                                   x => x.Include(y => y.Contact).Include(y => y.Campaign),
                                                   true));
            if (result == null) throw new NotFoundException(nameof(CampaignReport), userId);
            return result;
        }
        public async Task<IList<object>> GetCampaignsForSelectAsync(Guid? userId)
        {
            return await _campaignUnitOfWork.CampaignRepository.GetAsync<object>(
                                                    x => new { Value = x.Id, Text = x.Name },
                                                    x => !x.IsDeleted && x.IsActive &&
                                                   (!userId.HasValue || x.UserId == userId.Value),
                                                    null, null, true);
        }
        public async Task<IList<CampaignReport>> GetCampaignWiseReportAsync(Guid? userId, int campaignId)
        {
            var result = (await _campaignReportUnitOfWork.CampaingReportRepository.GetAsync(x => x,
                                                   x => !x.IsDeleted && x.IsActive &&
                                                   (!userId.HasValue || x.Campaign.UserId == userId.Value) && (x.CampaignId == campaignId),
                                                   x => x.OrderBy(o => o.Contact.Email),
                                                   x => x.Include(y => y.Contact).Include(y => y.Campaign),
                                                   true));
            if (result == null) throw new NotFoundException(nameof(CampaignReport), userId);
            return result;
        }
        public async Task ExcelExportForAllCampaignAsync(DownloadQueue downloadQueue)
        {
            using (var workbook = new XLWorkbook())
            {
                var campaignReport = await GetAllCampaignReportAsync(downloadQueue.UserId);

                var worksheet = workbook.Worksheets.Add("All Campaign Report");
                var currentRow = 1;
                int i = 3;

                worksheet.Cell(currentRow, 1).Value = "Email";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;

                worksheet.Cell(currentRow, 2).Value = "Delivered";
                worksheet.Cell(currentRow, 2).Style.Font.Bold = true;

                worksheet.Cell(currentRow, 3).Value = "Seen";
                worksheet.Cell(currentRow, 3).Style.Font.Bold = true;

                worksheet.Cell(currentRow, 4).Value = "Send Date";
                worksheet.Cell(currentRow, 4).Style.Font.Bold = true;

                worksheet.Cell(currentRow, 5).Value = "Seen Date";
                worksheet.Cell(currentRow, 5).Style.Font.Bold = true;

                foreach (var report in campaignReport)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = report.Contact.Email;
                    worksheet.Cell(currentRow, 2).Value = report.IsDelivered == true ? "Yes" : "No";
                    worksheet.Cell(currentRow, 3).Value = report.IsSeen == true ? "Yes" : "No";
                    worksheet.Cell(currentRow, 4).Value = "" + report.SendDateTime.ToString();
                    worksheet.Cell(currentRow, 5).Value = report.SeenDateTime == null ? "" : report.SeenDateTime.ToString();
                }
                worksheet.Columns("1", "5").AdjustToContents();

                var memory = new MemoryStream();
                using (var stream = new FileStream(downloadQueue.FileUrl, FileMode.Create))
                {
                    workbook.SaveAs(stream);
                }
            }
        }
        public async Task ExcelExportForCampaignWiseAsync(DownloadQueue downloadQueue)
        {
            for (int cnt = 0; cnt < downloadQueue.DownloadQueueSubEntities.Count(); cnt++)
            {
                using (var workbook = new XLWorkbook())
                {
                    var campaignReport = await GetCampaignWiseReportAsync(downloadQueue.UserId, downloadQueue.DownloadQueueSubEntities[cnt].DownloadQueueSubEntityId);

                    var worksheet = workbook.Worksheets.Add("CampaignWiseReport");
                    var currentRow = 1;
                    int i = 3;

                    worksheet.Cell(currentRow, 1).Value = "Email";
                    worksheet.Cell(currentRow, 1).Style.Font.Bold = true;

                    worksheet.Cell(currentRow, 2).Value = "Delivered";
                    worksheet.Cell(currentRow, 2).Style.Font.Bold = true;

                    worksheet.Cell(currentRow, 3).Value = "Seen";
                    worksheet.Cell(currentRow, 3).Style.Font.Bold = true;

                    worksheet.Cell(currentRow, 4).Value = "Send Date";
                    worksheet.Cell(currentRow, 4).Style.Font.Bold = true;

                    worksheet.Cell(currentRow, 5).Value = "Seen Date";
                    worksheet.Cell(currentRow, 5).Style.Font.Bold = true;

                    foreach (var report in campaignReport)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = report.Contact.Email;
                        worksheet.Cell(currentRow, 2).Value = report.IsDelivered == true ? "Yes" : "No";
                        worksheet.Cell(currentRow, 3).Value = report.IsSeen == true ? "Yes" : "No";
                        worksheet.Cell(currentRow, 4).Value = "" + report.SendDateTime.ToString();
                        worksheet.Cell(currentRow, 5).Value = report.SeenDateTime == null ? "" : report.SeenDateTime.ToString();
                    }
                    worksheet.Columns("1", "5").AdjustToContents();

                    var memory = new MemoryStream();
                    using (var stream = new FileStream(downloadQueue.FileUrl, FileMode.Create))
                    {
                        workbook.SaveAs(stream);
                    }
                }
            }
        }
        public async Task SaveDownloadQueueAsync(DownloadQueue downloadQueue)
        {
            await _campaignReportExportUnitOfWork.DownloadQueueRepository.AddAsync(downloadQueue);
            await _campaignReportExportUnitOfWork.SaveChangesAsync();
        }
        public async Task AddDownloadQueueSubEntities(DownloadQueueSubEntity downloadQueueSubEntity)
        {
            await _campaignReportExportUnitOfWork.DownloadQueueSubEntityRepository.AddAsync(downloadQueueSubEntity);
            await _campaignReportExportUnitOfWork.SaveChangesAsync();
        }
    }
}
