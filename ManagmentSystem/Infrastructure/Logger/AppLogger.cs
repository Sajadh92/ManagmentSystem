using Infrastructure.Service;
using Newtonsoft.Json;

namespace Infrastructure.Logger;

public interface IAppLogger
{
    void Write(Exception? ex, string msg);
    Task WriteAsync(Exception? ex, string msg);
    Task WriteAsync(Exception? ex, HttpContext context, object result);
}

public class AppLogger : IAppLogger, ISingleton
{
    private const string path1 = @"Files\Logger.txt";
    private const string path2 = @"Files\ExceptionLog.txt";

    public void Write(Exception? exception, string message)
    {
        try
        {
            string logFormat = "^[{0}]\t[{1}]\t[{2}]\t[{3}]$\n";

            string logRecord = string.Format(logFormat, DateTime.UtcNow.AddHours(3),
                 message, exception?.Message, exception?.InnerException?.Message);

            File.AppendAllText(path1, logRecord);
        }
        catch (Exception)
        {
            //throw; // TEST
        }
    }

    public async Task WriteAsync(Exception? exception, string message)
    {
        try
        {
            string logFormat = "^[{0}]\t[{1}]\t[{2}]\t[{3}]$\n";

            string logRecord = string.Format(logFormat, DateTime.UtcNow.AddHours(3), message,
                 exception?.Message, exception?.InnerException?.Message);

            await File.AppendAllTextAsync(path1, logRecord);
        }
        catch (Exception)
        {
            //throw; // TEST
        }
    }

    public async Task WriteAsync(Exception? exception, HttpContext context, object result)
    {
        try
        {
            //////////
            /// 0: DateTime
            /// 1: Method 
            /// 2: Host/IP 
            /// 3: Path 
            /// 4: StatusCode 
            /// 5: Exception.Message
            /// 6: Exception.InnerException.Message
            /// 7: ObjcetResultJson
            //////////

            string logFormat = "^[{0}]\t[{1}]\t[{2}]\t[{3}]\t[{4}]\t[{5}]\t[{6}]\t[{7}]$\n";

            string logRecord = string.Format(logFormat, 
                DateTime.UtcNow.AddHours(3),
                context.Request.Method, 
                context.Request.Host.Value, 
                context.Request.Path.Value, 
                context.Response.StatusCode,
                exception?.Message, 
                exception?.InnerException?.Message,
                JsonConvert.SerializeObject(result));

            await File.AppendAllTextAsync(path2, logRecord);
        }
        catch (Exception)
        {
            //throw; // TEST
        }
    }
}
