using FiltersSample.Helper;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FiltersSample.Filters
{
    public class SampleGlobalActionFilter : IActionFilter
    {
        private ILogger _logger;
        public SampleGlobalActionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SampleGlobalActionFilter>();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogWarning(nameof(OnActionExecuting) + " for " + context.ActionDescriptor.DisplayName);
            if (context.ActionDescriptor.DisplayName == "FiltersSample.Controllers.HomeController.Hello")
            {
                _logger.LogInformation("Manipulating action arguments...");
                if (!context.ActionArguments.ContainsKey("name"))
                {
                    context.ActionArguments["name"] = "Steve";
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogWarning(nameof(OnActionExecuted) + " for " + context.ActionDescriptor.DisplayName);
            if (context.ActionDescriptor.DisplayName == "FiltersSample.Controllers.HomeController.Hello")
            {
                _logger.LogInformation("Manipulating action result...");
                context.Result = Helpers.GetContentResult(context.Result, "FIRST: ");
            }
        }

    }
}