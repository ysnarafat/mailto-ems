using EmailMarketing.Shared.Infrastructure.Data.Entities.Groups;

namespace EmailMarketing.Shared.Infrastructure.Data.Entities.Campaigns;

public class CampaignGroup
{
    public int CampaignId { get; set; }
    public Campaign Campaign { get; set; } = null!;
    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
}
