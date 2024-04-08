using CarImpoundSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarImpoundSystem.Controllers
{
    public class WorkerController : Controller
    {
        public ActionResult RegisterPoundedVehicle(Vehicle vehicle)
        {

            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
