using CarImpoundSystem.Data;
using CarImpoundSystem.Models;
using CarImpoundSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarImpoundSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDBContext _context;

        public AdminController(AppDBContext context)
        {
            _context = context;
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
            var user = _context.users.FirstOrDefault(u => u.username == username);
            // Perform authentication here (e.g., check credentials against database)
            // For simplicity, let's assume username is "admin" and password is "password"
            if (username == "admin" && password == "password" && user.role == "admin")
            {
                // Authentication successful, redirect to index page
                // You may also want to implement actual authentication logic here
                return RedirectToAction("Index", "Admin");
            }

            // Authentication failed, display an error message
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }
    }
}
