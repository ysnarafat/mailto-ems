
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Common.Extensions;
using EmailMarketing.Framework.Entities.Campaigns;
using EmailMarketing.Framework.UnitOfWorks.Campaigns;
using EmailMarketing.Framework.UnitOfWorks.Groups;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Services.Campaigns
{
    public class CampaignService : ICampaignService
    {
        private ICampaignUnitOfWork _campaignUnitOfWork;
        private IGroupUnitOfWork _groupUnitOfWork;

        public CampaignService(ICampaignUnitOfWork campaignUnitOfWork, IGroupUnitOfWork groupUnitOfWork)
        {
            _campaignUnitOfWork = campaignUnitOfWork;
            _groupUnitOfWork = groupUnitOfWork;
        }

        public async Task<IList<(int Value, string Text, int Count)>> GetAllGroupsAsync(Guid? userId)
        {
            return (await _groupUnitOfWork.GroupRepository.GetAsync(x => new ValueTuple<int, string, int>(x.Id, x.Name, x.ContactGroups.Count()),
                                                   x => !x.IsDeleted && x.IsActive &&
                                                   (!userId.HasValue || x.UserId == userId.Value), x => x.OrderBy(o => o.Name), null, true));
        }
        public async Task<(IList<Campaign> Items, int Total, int TotalFilter)> GetAllCampaignAsync(
          Guid? userId,
          string searchText,
          string orderBy,
          int pageIndex,
          int pageSize)
        {
            var columnsMap = new Dictionary<string, Expression<Func<Campaign, object>>>()
            {
                ["Name"] = v => v.Name
            };
            var result = (await _campaignUnitOfWork.CampaignRepository.GetAsync(x => x,
                                                  x => !x.IsDeleted && x.IsActive &&
                                                  (!userId.HasValue || x.UserId == userId.Value) && x.Name.Contains(searchText),
                                                  x => x.ApplyOrdering(columnsMap, orderBy),
                                                  x => x.Include(y => y.CampaignReports).Include(y => y.SMTPConfig)
                                                        .Include(y => y.CampaignGroups).ThenInclude(z => z.Group),
                                                  pageIndex, pageSize,
                                                  true));

            if (result.Items == null) throw new NotFoundException(nameof(CampaignReport), userId);

            result.Total = await _campaignUnitOfWork.CampaignRepository.GetCountAsync(x => x.UserId == userId);

            return (result.Items, result.Total, result.TotalFilter);

        }
        public async Task<Campaign> GetCampaignByIdAsync(Guid? userId, int campaignId)
        {
            var result = await _campaignUnitOfWork.CampaignRepository.GetFirstOrDefaultAsync(
                x => x, x => !x.IsDeleted && x.IsActive &&
                (!userId.HasValue || x.UserId == userId.Value) && (x.Id == campaignId),
                  x => x.Include(y => y.SMTPConfig).Include(y => y.EmailTemplate), true);

            if (result == null) throw new NotFoundException(nameof(Campaign), campaignId);

            return result;
        }
        public async Task<(IList<CampaignReport> Items, int Total, int TotalFilter)> GetAllCampaignReportAsync(
          Guid? userId,
          int campaignId,
          string searchText,
          string orderBy,
          int pageIndex,
          int pageSize)
        {
            var result = (await _campaignUnitOfWork.CampaignReportRepository.GetAsync(x => x,
                                                   x => !x.IsDeleted && x.IsActive &&
                                                   (!userId.HasValue || x.Campaign.UserId == userId.Value) && (x.CampaignId == campaignId) && x.Contact.Email.Contains(searchText),
                                                   x => x.OrderBy(o => o.Contact.Email),
                                                   x => x.Include(y => y.Contact).Include(y => y.Campaign).Include(y => y.SMTPConfig), pageIndex, pageSize,
                                                   true));


            if (result.Items == null) throw new NotFoundException(nameof(CampaignReport), userId);

            return (result.Items, result.Total, result.TotalFilter);
        }

        public async Task<IList<EmailTemplate>> GetEmailTemplateByUserIdAsync(Guid? userId)
        {
            return (await _campaignUnitOfWork.EmailTemplateRepository.GetAsync(x => x, x => !x.IsDeleted && x.IsActive &&
                                                                                x.UserId == userId, null, null, true));
        }

        public void Dispose()
        {
            _campaignUnitOfWork?.Dispose();
        }

        public async Task AddCampaign(Campaign campaign)
        {
            await _campaignUnitOfWork.CampaignRepository.AddAsync(campaign);
            await _campaignUnitOfWork.SaveChangesAsync();
        }

        public async Task<IList<Campaign>> GetAllProcessingCampaign()
        {
            return (await _campaignUnitOfWork.CampaignRepository.GetAsync(x => x,
                                                                          x => !x.IsDeleted && x.IsActive && x.IsProcessing == true && x.SendDateTime <= DateTime.Now,
                                                                          null,
                                                                          null,
                                                                          true));
        }

        public async Task<Campaign> GetAllEmailByCampaignId(int campaignId)
        {
            var result = await _campaignUnitOfWork.CampaignRepository.GetFirstOrDefaultAsync(x => x,
                                                                                x => x.Id == campaignId,
                                                                                x => x.Include(s => s.SMTPConfig)
                                                                                        .Include(e => e.EmailTemplate)
                                                                                        .Include(y => y.CampaignGroups)
                                                                                            .ThenInclude(g => g.Group)
                                                                                            .ThenInclude(z => z.ContactGroups)
                                                                                            .ThenInclude(c => c.Contact)
                                                                                            .ThenInclude(cv => cv.ContactValueMaps)
                                                                                            .ThenInclude(fm => fm.FieldMap),
                                                                                true);
            return result;
        }

        public async Task UpdateCampaignAsync(Campaign campaign)
        {
            var existingCampaign = await _campaignUnitOfWork.CampaignRepository.GetByIdAsync(campaign.Id);
            existingCampaign.IsProcessing = false;
            existingCampaign.IsSucceed = true;
            existingCampaign.LastModified = DateTime.Now;
            existingCampaign.LastModifiedBy = campaign.UserId;

            await _campaignUnitOfWork.CampaignRepository.UpdateAsync(existingCampaign);
            await _campaignUnitOfWork.SaveChangesAsync();
        }

        public async Task<int> GetCampaignCountAsync(Guid? userId)
        {
            return await _campaignUnitOfWork.CampaignRepository.GetCountAsync(x => x.UserId == userId);
        }
        public async Task<int> GetCampaignCountAsync()
        {
            return await _campaignUnitOfWork.CampaignRepository.GetCountAsync();
        }
        public async Task<Campaign> ActivateCampaignAsync(int id)
        {
            var existingCampaign = await _campaignUnitOfWork.CampaignRepository.GetByIdAsync(id);
            existingCampaign.IsDraft = !existingCampaign.IsDraft;
            existingCampaign.IsProcessing = !existingCampaign.IsDraft;

            await _campaignUnitOfWork.CampaignRepository.UpdateAsync(existingCampaign);
            await _campaignUnitOfWork.SaveChangesAsync();
            return existingCampaign;
        }
    }
}
