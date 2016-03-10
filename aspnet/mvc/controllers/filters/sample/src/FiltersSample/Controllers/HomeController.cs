using System;
using System.Threading;
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

        [ServiceFilter(typeof(LoggingAddHeaderFilter))]
        public IActionResult Index()
        {
            _logger.LogInformation(nameof(Index));
            return View();
        }

        [AddHeader("Author","Steve Smith @ardalis")]
        public IActionResult Hello(string name)
        {
            _logger.LogInformation(nameof(Hello));
            return Content($"Hello {name}");
        }

        [TypeFilter(typeof(AddHeaderAttribute), 
            Arguments =new object[] { "Author", "Steve Smith (@ardalis)"})]
        public IActionResult Hi(string name)
        {
            _logger.LogInformation(nameof(Hi));
            return Content($"Hi {name}");
        }

        [TypeFilter(typeof(DurationActionFilter))]
        public IActionResult RandomTime()
        {
            _logger.LogInformation(nameof(RandomTime));
            Thread.Sleep(new Random().Next(2000));
            return View();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation(nameof(OnActionExecuting) + " from controller.");
            base.OnActionExecuting(context);
        }
    }
}
