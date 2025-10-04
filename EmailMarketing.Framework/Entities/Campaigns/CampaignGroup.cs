using EmailMarketing.Framework.Entities.Groups;

namespace EmailMarketing.Framework.Entities.Campaigns
{
    public class CampaignGroup
    {
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
