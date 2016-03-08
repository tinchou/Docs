using FiltersSample.Filters;
using Microsoft.AspNet.Mvc;

namespace FiltersSample.Controllers
{
    [SampleResourceFilter("Home")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
