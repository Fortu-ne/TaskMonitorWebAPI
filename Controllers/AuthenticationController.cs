using Microsoft.AspNetCore.Mvc;

namespace TaskMonitorWebAPI.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
