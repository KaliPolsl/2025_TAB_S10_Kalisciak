using Microsoft.AspNetCore.Mvc;

namespace AquaparkApp.Server.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
