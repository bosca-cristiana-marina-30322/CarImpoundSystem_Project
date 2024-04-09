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
        public ActionResult EditImpound()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Login(string username, string password)
        {
            // Perform authentication here (e.g., check credentials against database)
            // For simplicity, let's assume username is "admin" and password is "password"
            if (username == "admin" && password == "password")
            {
                // Authentication successful, redirect to index page
                // You may also want to implement actual authentication logic here
                return RedirectToAction("Index", "Admin");
            }

            // Authentication failed, display an error message
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }


        [HttpGet]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            { 
            
                return RedirectToAction("Login","Worker");//views unde sa redirect daca nu e ok
            
            }

           // if (!authService.HasRole(User.Identity.Name, "Admin"))
            {
                // Unauthorized access
            //    return RedirectToAction("Unauthorized", "Account");
            }
            return View();
        }
    }
}
