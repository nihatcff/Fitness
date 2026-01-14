using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.Controllers
{
    public class HomeController : Controller    
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
