using EmailMarketing.Common.Services;
using System;

namespace EmailMarketing.ExcelWorkerService.Services
{
    public class WorkerDateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
