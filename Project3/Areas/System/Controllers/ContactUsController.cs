using Microsoft.AspNetCore.Mvc;

namespace Project3.Areas.System.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
