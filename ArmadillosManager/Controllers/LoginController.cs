using Microsoft.AspNetCore.Mvc;

namespace ArmadillosManager.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
