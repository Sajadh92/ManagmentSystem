using Infrastructure.Logger;
using Infrastructure.Service;

namespace Infrastructure.Helper;

public interface IHelper
{
    void Write(Exception? ex, string msg);
    Task WriteAsync(Exception? ex, string msg);
}

public class Helper : IHelper, ISingleton
{
    private readonly IAppLogger _logger;

    public Helper(IAppLogger logger)
    {
        _logger = logger;
    }

    public void Write(Exception? ex, string msg)
    {
        _logger.Write(ex, msg);
    }

    public async Task WriteAsync(Exception? ex, string msg)
    {
        await _logger.WriteAsync(ex, msg);
    }
}
