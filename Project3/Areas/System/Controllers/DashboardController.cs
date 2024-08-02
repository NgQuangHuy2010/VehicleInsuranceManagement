using Microsoft.AspNetCore.Mvc;

namespace Project3.Areas.System.Controllers
{
    [Area("system")]
    [Route("system")]
    public class DashboardController : Controller
    {
        [Route("dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
