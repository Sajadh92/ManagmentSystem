using Infrastructure.AppException;
using Infrastructure.Logger;
using Infrastructure.Validation;
using Newtonsoft.Json;
using System.Net;

namespace Infrastructure.Middleware;

public class ErrorHandler
{
    private readonly RequestDelegate _next;
    private readonly IAppLogger _logger;

    public ErrorHandler(RequestDelegate next, IAppLogger logger)
    {
        _next = next; _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";

            object objectResult;

            switch (exception)
            {
                case LogicException:
                    response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    objectResult = new { Message = exception.Message };
                    break;

                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    objectResult = new { Message = $"{exception.Message} {ErrorCode.KeyNotFound}" };
                    break;

                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    objectResult = new { Message = exception.Message };
                    break;

                case DuplicateException:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    objectResult = new
                    {
                        //Errors = new List<Error>
                        //{
                        //    new Error(string.Concat(exception.Message[0].ToString().ToLower(),
                        //        exception.Message.AsSpan(1)), new() { ErrorCode.Duplicate })
                        //}
                    };
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    objectResult = new
                    {
                        exception.Message,
                        InnerMessage = exception.InnerException?.Message,
                        exception.Data,
                        exception.Source,
                        exception.HelpLink,
                        exception.HResult
                    };
                    break;
            }

            await _logger.WriteAsync(exception, context, objectResult);
            await response.WriteAsync(JsonConvert.SerializeObject(objectResult));
        }
    }
}