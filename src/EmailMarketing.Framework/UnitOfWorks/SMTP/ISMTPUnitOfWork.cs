using EmailMarketing.Data;
using EmailMarketing.Framework.Repositories.SMTP;

namespace EmailMarketing.Framework.UnitOfWorks.SMTP
{
    public interface ISMTPUnitOfWork : IUnitOfWork
    {
        public ISMTPRepository SMTPRepository { get; set; }
    }
}
