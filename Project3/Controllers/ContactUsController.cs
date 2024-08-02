using Microsoft.AspNetCore.Mvc;

namespace Project3.Controllers
{
	public class ContactUsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
