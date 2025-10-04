using EmailMarketing.Common.Services;
using System;

namespace EmailMarketing.CampaingReportExcelExportService.Services
{
    public class WorkerCurrentUserService : ICurrentUserService
    {
        public WorkerCurrentUserService()
        {
            UserId = Guid.Empty;
            IsAuthenticated = false;
        }

        public Guid UserId { get; }
        public bool IsAuthenticated { get; }
    }
}
