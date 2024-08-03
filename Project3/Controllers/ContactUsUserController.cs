using Microsoft.AspNetCore.Mvc;

namespace Project3.Controllers
{
    public class ContactUsUserController : Controller
    {
        [Route("contact")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
