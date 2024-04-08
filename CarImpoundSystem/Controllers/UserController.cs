using Microsoft.AspNetCore.Mvc;

namespace CarImpoundSystem.Controllers
{
    public class UserController : Controller
    {
        public ActionResult SearchByLicensePlate(string licensePlate)
        {



            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SearchByLicense()
        {
            return View();
        }
    }
}
