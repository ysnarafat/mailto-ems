using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Repositories.SMTP;

namespace EmailMarketing.Framework.UnitOfWorks.SMTP
{
    public class SMTPUnitOfWork : EmailMarketing.Data.UnitOfWork, ISMTPUnitOfWork
    {
        public ISMTPRepository SMTPRepository { get; set; }
        public SMTPUnitOfWork(FrameworkContext dbContext, ISMTPRepository smtpRepository) : base(dbContext)
        {
            SMTPRepository = smtpRepository;
        }
    }
}
