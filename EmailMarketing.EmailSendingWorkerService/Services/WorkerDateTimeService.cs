using EmailMarketing.Common.Services;
using System;

namespace EmailMarketing.EmailSendingWorkerService.Services
{
    public class WorkerDateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
