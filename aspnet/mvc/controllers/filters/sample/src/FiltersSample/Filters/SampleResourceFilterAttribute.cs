using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FiltersSample.Filters
{
    public class SampleResourceFilterAttribute : Attribute,
            IResourceFilter, IAsyncResourceFilter
    {
        public string Name { get; }

        public SampleResourceFilterAttribute(string name)
        {
            Name = name;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var loggerFactory = (LoggerFactory)context.HttpContext.RequestServices.GetService(typeof(ILoggerFactory));
            var logger = loggerFactory.CreateLogger(nameof(SampleResourceFilterAttribute));
            logger.LogInformation($"OnResourceExecutionAsync for {Name}");

            OnResourceExecuting(context);
            OnResourceExecuted(await next());
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var loggerFactory = (LoggerFactory)context.HttpContext.RequestServices.GetService(typeof (ILoggerFactory));
            var logger = loggerFactory.CreateLogger(nameof(SampleResourceFilterAttribute));
            logger.LogInformation($"OnResourceExecuting for {Name}");
            context.HttpContext.Response.Headers.Append("filters", Name + " - OnResourceExecuting");
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            var loggerFactory = (LoggerFactory)context.HttpContext.RequestServices.GetService(typeof (ILoggerFactory));
            var logger = loggerFactory.CreateLogger(nameof(SampleResourceFilterAttribute));
            logger.LogInformation($"OnResourceExecuted for {Name}");
            context.HttpContext.Response.Headers.Append("filters", Name + " - OnResourceExecuted");
        }
    }
}
