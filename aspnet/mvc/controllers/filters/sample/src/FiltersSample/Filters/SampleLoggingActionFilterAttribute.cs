using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FiltersSample.Filters
{
    public class SampleLoggingActionFilterAttribute : TypeFilterAttribute
    {
        public SampleLoggingActionFilterAttribute():base(typeof(SampleLoggingActionFilterImpl))
        {
        }

        private class SampleLoggingActionFilterImpl : IActionFilter, IAsyncActionFilter
        {
            private readonly ILogger _logger;
            public SampleLoggingActionFilterImpl(ILoggerFactory loggerFactory)
            {
                _logger = loggerFactory.CreateLogger<SampleLoggingActionFilterAttribute>();
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                OnActionExecuting(context);
                OnActionExecuted(await next());
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                _logger.LogInformation("Filter Executing");
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                _logger.LogInformation("Filter Executed");
            }
        }
    }
}