using CarImpoundSystem.Models;
using CarImpoundSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarImpoundSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AuthService authService;

        public AdminController(AuthService authService)
        {
            this.authService = authService;
        }

        public ActionResult UpdateVehicleStatus(Vehicle vehicle, string status)
        {

            return View();
        }


        public ActionResult ProcessVehicle(Vehicle vehicle)
        {

            return View();
        }




        [HttpGet]
        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            { 
            
                return RedirectToAction("Login","Admin");//views unde sa redirect daca nu e ok
            
            }

            if (!authService.HasRole(User.Identity.Name, "Admin"))
            {
                // Unauthorized access
                return RedirectToAction("Unauthorized", "Account");
            }
            return View();
        }
    }
}
