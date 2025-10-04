using Autofac;
using EmailMarketing.Framework.Services.Campaigns;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models
{
    public class EmailTrackerModel
    {
        protected readonly ICampaignReportService _campaignReportService;

        public EmailTrackerModel(ICampaignReportService campaignReportService)
        {
            _campaignReportService = campaignReportService;
        }

        public EmailTrackerModel()
        {
            _campaignReportService = Startup.AutofacContainer.Resolve<ICampaignReportService>();
        }

        public async Task EmailOpenTracking(int campaignId, int contactId, string email)
        {
            await _campaignReportService.EmailOpenTracking(campaignId, contactId, email);
        }

        public void Dispose()
        {
            _campaignReportService?.Dispose();
        }
    }
}
