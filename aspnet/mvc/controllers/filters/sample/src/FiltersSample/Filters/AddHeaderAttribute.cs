using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FiltersSample.Filters
{
    /// <summary>
    /// https://github.com/aspnet/Mvc/blob/master/test/WebSites/FiltersWebSite/Filters/AddHeaderAttribute.cs
    /// </summary>
    public class AddHeaderAttribute : ResultFilterAttribute
    {
        private ILogger _logger;
        public AddHeaderAttribute(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AddHeaderAttribute>();
        }

        public AddHeaderAttribute()
        {
            
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