using CarImpoundSystem.Models;
using CarImpoundSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarImpoundSystem.Controllers
{
    public class WorkerController : Controller
    {
        private readonly AuthService authService;
        public WorkerController(AuthService authService)
        {
            this.authService = authService;
        }
        public ActionResult RegisterPoundedVehicle(Vehicle vehicle)
        {

            return View();
        }

        public IActionResult Index()
        {
            // Check if user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to worker login page
                return RedirectToAction("Login", "Worker");
               
            }
           
            // Check if user has the correct role
            if (!authService.HasRole(User.Identity.Name, "Worker"))
            {
                // Unauthorized access
                return RedirectToAction("Unauthorized", "Account");
            }

            return View();
        }
    }
}
