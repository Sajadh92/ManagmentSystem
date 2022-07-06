namespace Infrastructure.AppException;

public class DuplicateException : Exception
{
    public DuplicateException(string message)
        : base(message)
    {
    }
}

public class LogicException : Exception
{
    public LogicException(string message)
        : base(message)
    {
    }
}
