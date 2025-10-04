using EmailMarketing.Common.Services;
using System;

namespace EmailMarketing.Web.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
