using System;
using FiltersSample.Filters;
using Microsoft.AspNet.Mvc;

namespace FiltersSample.Controllers
{
    [TypeFilter(typeof(LoggingExceptionFilterAttribute))]
    public class FailingController : Controller
    {
        public IActionResult Index()
        {
            throw new Exception("Boom!");
        }    
    }
}