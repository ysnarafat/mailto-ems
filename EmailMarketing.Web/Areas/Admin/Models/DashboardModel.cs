using EmailMarketing.Framework.Services.Campaigns;
using EmailMarketing.Framework.Services.Contacts;
using EmailMarketing.Framework.Services.Groups;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Admin.Models
{
    public class DashboardModel : DashboardBaseModel
    {
        public int TotalUser { get; set; }
        public int TotalContacts { get; set; }
        public int TotalGroups { get; set; }
        public int TotalCampaigns { get; set; }
        public int TotalMailSent { get; set; }

        public DashboardModel(ICampaignService campaignService, IContactService contactService,
            IGroupService groupUserService, ICampaignReportService campaignReportService) : base(campaignService, contactService, groupUserService, campaignReportService)
        {

        }
        public DashboardModel() : base()
        {

        }


        public async Task LoadDashboardData()
        {
            TotalUser = await _applicationuserService.GetAllMembersAsync();
            TotalContacts = await _contactService.GetContactCountAsync();
            TotalMailSent = await _campaignReportService.GetDeleveredMailCountAsync();
            TotalGroups = await _groupService.GetGroupCountAsync();
            TotalCampaigns = await _campaignService.GetCampaignCountAsync();
        }

    }
}
