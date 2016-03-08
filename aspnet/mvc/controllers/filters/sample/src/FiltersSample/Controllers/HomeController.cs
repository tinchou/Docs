using FiltersSample.Filters;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FiltersSample.Controllers
{
    //[SampleResourceFilter("Home")]
    public class HomeController : Controller
    {
        private ILogger _logger;
        public HomeController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HomeController>();
        }
        //[AddHeader]
        [ServiceFilter(typeof(AddHeaderAttribute))]
        public IActionResult Index()
        {
            _logger.LogInformation(nameof(Index));
            return View();
        }

        public IActionResult Hello(string name)
        {
            _logger.LogInformation(nameof(Hello));
            return Content($"Hello {name}");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation(nameof(OnActionExecuting));
            base.OnActionExecuting(context);
        }
    }
}
