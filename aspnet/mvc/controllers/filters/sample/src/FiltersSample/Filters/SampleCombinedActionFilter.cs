using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Filters;

namespace FiltersSample.Filters
{
    public class SampleCombinedActionFilter : IActionFilter, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, 
            ActionExecutionDelegate next)
        {
            OnActionExecuting(context);
            OnActionExecuted(await next());
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // do something before the action executes
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
        }
    }
}