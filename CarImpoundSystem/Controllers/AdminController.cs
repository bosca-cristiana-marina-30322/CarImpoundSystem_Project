using CarImpoundSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarImpoundSystem.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult UpdateVehicleStatus(Vehicle vehicle, string status)
        {

            return View();
        }


        public ActionResult ProcessVehicle(Vehicle vehicle)
        {

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
