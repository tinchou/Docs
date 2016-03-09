using System.Diagnostics;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FiltersSample.Filters
{
    public class DurationActionFilter : IActionFilter
    {
        private ILogger _logger;
        private Stopwatch _stopwatch;
        public DurationActionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DurationActionFilter>();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var actionDuration = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Stop();
            _logger.LogInformation($"{context.ActionDescriptor.DisplayName}: {actionDuration}ms");

            // add time to viewbag if viewresult
            ViewResult result = context.Result as ViewResult;
            if (result != null)
            {
                result.ViewData["actionDuration"] = actionDuration;
            }
        }
    }
}