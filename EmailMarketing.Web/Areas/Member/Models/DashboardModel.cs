using EmailMarketing.Framework.Services.Campaigns;
using EmailMarketing.Framework.Services.Contacts;
using EmailMarketing.Framework.Services.Groups;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models
{
    public class DashboardModel : DashboardBaseModel
    {
        public int TotalContacts { get; set; }
        public int TotalGroups { get; set; }
        public int TotalCampaigns { get; set; }
        public DashboardModel(ICampaignService campaignService, IContactService contactService,
            IGroupService groupUserService) : base(campaignService, contactService, groupUserService)
        {

        }
        public DashboardModel() : base()
        {

        }
        public async Task LoadDashboardData()
        {
            var userId = _currentUserService.UserId;
            TotalContacts = await _contactService.GetContactCountAsync(userId);
            TotalGroups = await _groupService.GetGroupCountAsync(userId);
            TotalCampaigns = await _campaignService.GetCampaignCountAsync(userId);
        }

    }
}
