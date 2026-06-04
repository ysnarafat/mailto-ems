using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Entities;
using EmailMarketing.Framework.Entities.Campaigns;
using EmailMarketing.Framework.Enums;
using EmailMarketing.Framework.Services.Campaigns;
using EmailMarketing.Web.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.Campaigns
{
    public class CampaignsModel : CampaignBaseModel
    {
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public string Description { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string EmailSubject { get; set; }
        public EmailTemplate EmailTemplate { get; set; }
        public int Id { get; set; }
        public bool IsExportAll { get; set; }
        public string SendEmailAddress { get; set; }
        public bool IsSendEmailNotifyForAll { get; set; }
        public bool IsSendEmailNotifyForCampaignwise { get; set; }
        public IList<object> CampaignSelectList { get; set; }

        public AppSettings _appSettings;
        public CampaignsModel(ICampaignService campaignService, ICampaignReportExportService campaignREService,
            ICurrentUserService currentUserService) : base(campaignService, campaignREService, currentUserService)
        {

        }
        public CampaignsModel() : base()
        {

        }

        public async Task GetCampaignData(int campaignId)
        {

            var result = await _campaignService.GetCampaignByIdAsync(_currentUserService.UserId, campaignId);
            this.CampaignName = result.Name;
            this.Description = result.Description;
            this.SenderEmail = result.SMTPConfig.SenderEmail;
            this.SenderName = result.SMTPConfig.SenderName;
            this.EmailSubject = result.EmailSubject;
            this.EmailTemplate = result.EmailTemplate;
            //return result;
        }

        public async Task<object> GetAllCampaignsAsync(DataTablesAjaxRequestModel tableModel)
        {

            var result = await _campaignService.GetAllCampaignAsync(
                _currentUserService.UserId,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "Name" }),
                tableModel.PageIndex, tableModel.PageSize);

            return new
            {

                recordsTotal = result.Total,
                recordsFiltered = result.TotalFilter,
                data = (from item in result.Items
                        select new string[]
                        {
                            item.Name,
                            string.Join(", ", item.CampaignGroups.Select(x => x.Group.Name)),
                            item.IsDraft ? "Yes" : "No",
                            item.IsProcessing ? "Processing" : "Finished",
                            item.IsSucceed ? "Yes" : "No",
                            item.CampaignReports.Count().ToString(),
                            item.SendDateTime.ToString(),
                            item.Id.ToString()

                        }).ToArray()
            };
        }

        internal async Task SetCapaignId(int id)
        {
            this.CampaignId = id;

        }

        public async Task<object> GetAllExportedCampaignReportAsync(DataTablesAjaxRequestModel tableModel)
        {

            var result = await _campaignReportExportService.GetAllCampaignReportsFromDownloadQueueAsync(
                _currentUserService.UserId,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "Created" }),
                tableModel.PageIndex, tableModel.PageSize);

            return new
            {
                recordsTotal = result.Total,
                recordsFiltered = result.TotalFilter,
                data = (from item in result.Items
                        select new string[]
                        {
                            item.FileName,
                            item.Created.ToString(),
                            item.IsProcessing?"Processing":"Finished",
                            item.IsSucceed?"Yes":"No",
                            item.IsSendEmailNotify?"Yes":"No",
                            item.Id.ToString()

                        }).ToArray()
            };
        }

        public async Task<object> GetCampaignReportByCampaignIdAsync(DataTablesAjaxRequestModel tableModel, int CampaignId)
        {

            var result = await _campaignService.GetAllCampaignReportAsync(
                _currentUserService.UserId,
                CampaignId,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "CampaignName", "Email" }),
                tableModel.PageIndex, tableModel.PageSize);


            return new
            {
                recordsTotal = result.Total,
                recordsFiltered = result.TotalFilter,
                data = (from item in result.Items
                        select new string[]
                        {
                            item.Contact.Email.ToString(),
                            item.IsDelivered ? "Yes" : "No",
                            item.IsSeen ? "Yes" : "No",
                            item.SeenDateTime == null ? "" : item.SeenDateTime.ToString(),
                            item.SendDateTime.ToString()

                        }).ToArray()
            };
        }
        public async Task LoadAllCampaignSelectListAsync()
        {
            this.CampaignSelectList = await _campaignReportExportService.GetCampaignsForSelectAsync(_currentUserService.UserId);
        }
        public async Task ExportAllCampaign()
        {
            if (IsSendEmailNotifyForAll == true && string.IsNullOrWhiteSpace(SendEmailAddress))
            {
                throw new Exception("Please Provide Email");
            }
            if (Directory.Exists(_appSettings.CampaignExportFileUrl) == false)
            {
                DirectoryInfo directory = Directory.CreateDirectory(_appSettings.CampaignExportFileUrl);
            }

            try
            {
                var userId = _currentUserService.UserId;
                var distinctiveFileName = Guid.NewGuid().ToString();
                var downloadQueue = new DownloadQueue();
                downloadQueue.FileName = "AllCampaignReport_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                downloadQueue.FileUrl = Path.Combine(_appSettings.CampaignExportFileUrl, distinctiveFileName) + Path.GetExtension(downloadQueue.FileName);
                downloadQueue.IsProcessing = true;
                downloadQueue.IsSucceed = false;
                downloadQueue.UserId = userId;
                downloadQueue.Created = DateTime.Now;
                downloadQueue.CreatedBy = userId;
                downloadQueue.DownloadQueueFor = DownloadQueueFor.CampaignAllReportExport;
                downloadQueue.IsSendEmailNotify = IsSendEmailNotifyForAll;
                downloadQueue.SendEmailAddress = SendEmailAddress;
                await _campaignReportExportService.SaveDownloadQueueAsync(downloadQueue);
            }
            catch (Exception)
            {
                throw new Exception("Failed to export. Please try again");
            }

        }

        public async Task<Campaign> ActivateCampaign(int id)
        {
            return await _campaignService.ActivateCampaignAsync(id);
        }

        public async Task ExportCampaignWise()
        {
            if (IsSendEmailNotifyForCampaignwise == true && string.IsNullOrWhiteSpace(SendEmailAddress))
            {
                throw new Exception("Please Provide Email");
            }
            if (Directory.Exists(_appSettings.CampaignExportFileUrl) == false)
            {
                DirectoryInfo directory = Directory.CreateDirectory(_appSettings.CampaignExportFileUrl);
            }

            try
            {
                var userId = _currentUserService.UserId;
                var distinctiveFileName = Guid.NewGuid().ToString();
                var downloadQueue = new DownloadQueue();
                downloadQueue.FileName = "CampaignwiseReport_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                downloadQueue.FileUrl = Path.Combine(_appSettings.CampaignExportFileUrl, distinctiveFileName) + Path.GetExtension(downloadQueue.FileName);
                downloadQueue.IsProcessing = true;
                downloadQueue.IsSucceed = false;
                downloadQueue.UserId = userId;
                downloadQueue.Created = DateTime.Now;
                downloadQueue.CreatedBy = userId;
                downloadQueue.DownloadQueueFor = DownloadQueueFor.CampaignDetailsReportExport;
                downloadQueue.IsSendEmailNotify = IsSendEmailNotifyForCampaignwise;
                downloadQueue.SendEmailAddress = SendEmailAddress;
                await _campaignReportExportService.SaveDownloadQueueAsync(downloadQueue);

                var dowloadQueueSubEntity = new DownloadQueueSubEntity();
                dowloadQueueSubEntity.DownloadQueueId = downloadQueue.Id;
                dowloadQueueSubEntity.DownloadQueueSubEntityId = this.Id;

                await _campaignReportExportService.AddDownloadQueueSubEntities(dowloadQueueSubEntity);
            }
            catch (Exception)
            {
                throw new Exception("Failed to Export. Please try again.");
            }

        }
    }
}

