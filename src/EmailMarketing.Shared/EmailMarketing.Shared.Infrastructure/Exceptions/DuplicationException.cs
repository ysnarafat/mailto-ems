namespace EmailMarketing.Shared.Infrastructure.Exceptions;

public class DuplicationException : Exception
{
    public DuplicationException(string name)
        : base($"{name} already exists.")
    {
    }
}
