using CarImpoundSystem.Data;
using CarImpoundSystem.Models;
using CarImpoundSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarImpoundSystem.Controllers
{
    public class WorkerController : Controller
    {
        
        private readonly AppDBContext _context;
        public WorkerController(AppDBContext context) {
            _context = context;
        }
        public ActionResult RegisterPoundedVehicle(Vehicle vehicle)
        {

            return View();
        }

       

        [HttpGet]
        public IActionResult Login(string username, string password)
        {
            var user = _context.users.FirstOrDefault(u => u.username == username);
            // Perform authentication here (e.g., check credentials against database)
            // For simplicity, let's assume username is "admin" and password is "password"
            if (user != null && user.password == password && user.role == "worker")
            {
                // Authentication successful, redirect to index page
                // You may also want to implement actual authentication logic here
                return RedirectToAction("Impound", "Worker");
            }

            // Authentication failed, display an error message
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

        public IActionResult Impound()
        {
            return View();
        }

       
    }
}
