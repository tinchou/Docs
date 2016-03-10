using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FiltersSample.Filters
{
    /// <summary>
    /// https://github.com/aspnet/Mvc/blob/master/test/WebSites/FiltersWebSite/Filters/AddHeaderAttribute.cs
    /// </summary>
    public class LoggingAddHeaderFilter : IResultFilter
    {
        private ILogger _logger;
        public LoggingAddHeaderFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LoggingAddHeaderFilter>();
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            _logger.LogWarning(nameof(OnResultExecuting));
            context.HttpContext.Response.Headers.Add(
                "OnResultExecuting", new string[] { "ResultExecutingSuccessfully" });
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            _logger.LogWarning(nameof(OnResultExecuted));
            context.HttpContext.Response.Headers.Add(
                "OnResultExecuted", new string[] { "ResultExecutedSuccessfully" });

        }
    }
}