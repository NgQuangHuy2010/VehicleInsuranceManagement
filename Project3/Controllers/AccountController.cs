using Microsoft.AspNetCore.Mvc;

namespace Project3.Controllers
{
    public class AccountController : Controller
    {
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }
        //[Route("login")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login()
        //{

        //}
    }
}
