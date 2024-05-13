using CarImpoundSystem.Data;
using CarImpoundSystem.Models;
using CarImpoundSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
        public ActionResult RegisterVehicle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterVehicle(Vehicle vehicle)
        {
            Vehicle model = new Vehicle()
            {

                  LicensePlate = vehicle.LicensePlate,
                  VIN = vehicle.VIN,
                  Make = vehicle.Make,
                  Model = vehicle.Model,
                  Color = vehicle.Color,
                  Status = vehicle.Status,
            };

            
            await _context.vehicles.AddAsync(model);
            await _context.SaveChangesAsync();

            // Redirect to the form to add impoundment record after registering the vehicle
            return RedirectToAction("Impound");
        }

        public async Task<ActionResult> ViewCars()
        {
            return View(await _context.impoundmentRecords.ToListAsync());
        }
        [HttpGet]
        public IActionResult Impound()
        {
            var vehicles = _context.vehicles.ToList();
            var vehicleList = new SelectList(vehicles, "LicensePlate", "LicensePlate");
            ViewBag.LicensePlate = vehicleList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Impound(ImpoundmentRecord impoundmentRecord)
        {
            // Check if the selected vehicle exists

                // Create an instance of ImpoundmentRecord and populate its properties
                var model = new ImpoundmentRecord
                {
                    date = impoundmentRecord.date,
                    location = impoundmentRecord.location,
                    reason = impoundmentRecord.reason,
                    LicensePlate = impoundmentRecord.LicensePlate,
                    status = "in",
                };

                // Save the impoundmentRecord to the database
                await _context.impoundmentRecords.AddAsync(model);
                await _context.SaveChangesAsync();

                // Redirect to the view with the list of impoundment records
                return RedirectToAction("ViewCars");
           
        }

        public ActionResult Index()
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
                return RedirectToAction("Index", "Worker");
            }

            // Authentication failed, display an error message
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }
        public IActionResult Error()
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(errorViewModel);
        }



    }
}
