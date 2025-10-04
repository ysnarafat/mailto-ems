using EmailMarketing.Common.Services;
using System;

namespace EmailMarketing.CampaingReportExcelExportService.Services
{
    public class WorkerDateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
