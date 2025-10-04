using EmailMarketing.Common.Services;
using System;

namespace EmailMarketing.EmailSendingWorkerService.Services
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
