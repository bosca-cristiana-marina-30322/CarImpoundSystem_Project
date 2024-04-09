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

        public IActionResult Login(string username, string password)
        {
            // Perform authentication here (e.g., check credentials against database)
            // For simplicity, let's assume username is "admin" and password is "password"
            if (username == "admin" && password == "password")
            {
                // Authentication successful, redirect to index page
                // You may also want to implement actual authentication logic here
                return RedirectToAction("Index", "Worker");
            }

            // Authentication failed, display an error message
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

        public IActionResult Impound()
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
