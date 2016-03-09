using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FiltersSample.Filters
{
    /// <summary>
    /// https://github.com/aspnet/Mvc/blob/master/test/WebSites/FiltersWebSite/Filters/AddHeaderAttribute.cs
    /// </summary>
    public class LoggingAddHeaderAttribute : ResultFilterAttribute
    {
        private ILogger _logger;
        public LoggingAddHeaderAttribute(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LoggingAddHeaderAttribute>();
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            _logger.LogWarning(nameof(OnResultExecuting));
            context.HttpContext.Response.Headers.Add(
                "OnResultExecuting", new string[] { "ResultExecutingSuccessfully" });
            base.OnResultExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            _logger.LogWarning(nameof(OnResultExecuted));
            context.HttpContext.Response.Headers.Add(
                "OnResultExecuted", new string[] { "ResultExecutedSuccessfully" });

            base.OnResultExecuted(context);
        }
    }
}