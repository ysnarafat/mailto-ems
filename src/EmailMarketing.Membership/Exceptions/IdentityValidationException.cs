using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace EmailMarketing.Membership.Exceptions
{
    public class IdentityValidationException : Exception
    {
        public IDictionary<string, string> Failures { get; }

        public IdentityValidationException()
            : base("One or more validation failures have occurred.")
        {
            Failures = new Dictionary<string, string>();
        }

        public IdentityValidationException(IEnumerable<IdentityError> failures)
            : this()
        {
            foreach (var failure in failures)
            {
                var propertyName = failure.Code;
                var propertyFailure = failure.Description;

                Failures.Add(propertyName, propertyFailure);
            }
        }
    }
}
