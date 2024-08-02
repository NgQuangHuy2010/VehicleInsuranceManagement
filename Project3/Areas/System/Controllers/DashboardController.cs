using Microsoft.AspNetCore.Mvc;

namespace Project3.Areas.System.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
