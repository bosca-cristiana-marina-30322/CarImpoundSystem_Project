using CarImpoundSystem.Data;
using CarImpoundSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NServiceKit.Text;

namespace CarImpoundSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDBContext _context;

        /// <summary>
        /// DATABASE CONTEXT
        /// </summary>
        /// <param name="context"></param>

        public AdminController(AppDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// LOGIN
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult Login(string username, string password)
        {
            var user = _context.users.FirstOrDefault(u => u.username == username);
            // Perform authentication here (e.g., check credentials against database)
            // For simplicity, let's assume username is "admin" and password is "password"
            if (user != null && user.password == password && user.role == "admin")
            {
                // Authentication successful, redirect to index page
                // You may also want to implement actual authentication logic here
                return RedirectToAction("AdminIndex", "Admin");
            }

            // Authentication failed, display an error message
            ViewBag.ErrorMessage = "Invalid username or password.";

            return View();
        }

        /// <summary>
        /// ADMIN INDEX
        /// </summary>
        /// <returns></returns>

        public ActionResult AdminIndex()
        {
            return View();
        }

        /// <summary>
        /// IMPUNDMENT RECORDS MANAGEMENT
        /// </summary>
        /// <returns></returns>


        [HttpGet]
        public async Task<IActionResult> ViewImpounds()
        {
            var impounds = await _context.impoundmentRecords.ToListAsync();
            return View(impounds);
        }


        public async Task<IActionResult> DetailsImpound(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var impound = await _context.impoundmentRecords.FindAsync(id);
            var vehicle = await _context.vehicles.FindAsync(impound.LicensePlate);
            var user = await _context.users.FindAsync(impound.UserId);
            if (impound == null)
            {
                return NotFound();
            }
            return View(impound);
        }

        [HttpGet]
        public IActionResult AddImpound()
        {
            var vehicles = _context.vehicles.ToList();
            var vehicleList = new SelectList(vehicles, "LicensePlate", "LicensePlate");
            ViewBag.LicensePlate = vehicleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddImpound(ImpoundmentRecord impoundmentRecord)
        {
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
            return RedirectToAction("ViewImpounds");

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
            return RedirectToAction("AddImpound");
        }


        [HttpGet]
        public async Task<ActionResult> EditImpound(string id)
        {
            System.Diagnostics.Debug.WriteLine("EditImpound get method started");
            System.Diagnostics.Debug.WriteLine($"EditImpound action method started for Id: {id}");

            var record = await _context.impoundmentRecords.FindAsync(id);
            System.Diagnostics.Debug.WriteLine($"EditImpound action method found record: {record.recordId}");

            System.Diagnostics.Debug.WriteLine($"Returning record: {record.recordId}");

            return View(record);
        }

        [HttpPost]
        public async Task<ActionResult> EditImpound(ImpoundmentRecord record)
        {
            System.Diagnostics.Debug.WriteLine("EditImpound action method started");
            System.Diagnostics.Debug.WriteLine($"EditImpound action method started for RecordId: {record.recordId}");

            // Retrieve the existing record from the database based on its ID
            var updatedRecord = await _context.impoundmentRecords.FindAsync(record.recordId);

            if (updatedRecord != null)
            {
                System.Diagnostics.Debug.WriteLine($"Record found: {updatedRecord.recordId}, {updatedRecord.payment}, {updatedRecord.status}");

                // Update the properties of the existing record
                updatedRecord.payment = record.payment;
                updatedRecord.status = record.status;

                // Save changes to the database
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine("Record updated.");
                ViewBag.Message = "Record updated.";
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Record not found.");
                ViewBag.Message = "Record not found.";
            }

            System.Diagnostics.Debug.WriteLine("EditImpound action method completed");

            return RedirectToAction("ViewImpounds");
        }


        public IActionResult DeleteImpound(string id)
        {
            var imp = _context.impoundmentRecords.Find(id);
            if (imp == null)
            {
                return RedirectToAction("ViewImpounds");
            }

            _context.impoundmentRecords.Remove(imp);
            _context.SaveChanges(true);

            return RedirectToAction("ViewImpounds");
        }


        private bool ImpoundmentRecordExists(string impound)
        {
            return _context.impoundmentRecords.Any(e => e.recordId == impound);
        }


        /// <summary>
        /// USER MANAGEMENT
        /// </summary>
        /// <returns></returns>

        public async Task<ActionResult> ViewUsers()
        {
            return View(await _context.users.ToListAsync());
        }

        //GET: Employee/Add
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        //POST: Employee/Add
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            User model = new User()
            {
                username = user.username,
                password = user.password,
                role = user.role,
            };
            // Save the employee to the database
            await _context.users.AddAsync(model);
            await _context.SaveChangesAsync();

            // Redirect to List all department page
            return RedirectToAction("ViewUsers");
        }


        // GET: Home
        [HttpGet]
        public async Task<ActionResult> EditUser(int id)
        {
            System.Diagnostics.Debug.WriteLine("EditUser get method started");
            System.Diagnostics.Debug.WriteLine($"EditUser action method started for Id: {id}");

            var user = await _context.users.FindAsync(id);
            System.Diagnostics.Debug.WriteLine($"EditUser action method found user: {user.UserId}");

            System.Diagnostics.Debug.WriteLine($"REturning user: {user.UserId}");

            return View(user);
        }


        [HttpPost]
        public async Task<ActionResult> EditUser(User user)
        {
            
            System.Diagnostics.Debug.WriteLine("EditUser action method started");
            System.Diagnostics.Debug.WriteLine($"EditUser action method started for UserId: {user.UserId}");


            // Retrieve the existing user from the database based on its ID
            var updatedUser = await _context.users.FindAsync(user.UserId);

            if (updatedUser != null)
            {
                System.Diagnostics.Debug.WriteLine($"User found: {updatedUser.UserId}, {updatedUser.username}, {updatedUser.password}, {updatedUser.role}");

                // Update the properties of the existing user
                updatedUser.username = user.username;
                updatedUser.password = user.password;
                updatedUser.role = user.role;

                // Save changes to the database
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine("User record updated.");
                ViewBag.Message = "User record updated.";
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("User not found.");
                ViewBag.Message = "User not found.";
            }

            System.Diagnostics.Debug.WriteLine("EditUser action method completed");

            return RedirectToAction("ViewUsers");
        }



        public IActionResult DeleteUser(int id)
        {
            var user =  _context.users.Find(id);
            if (user == null)
            {
                return RedirectToAction("ViewUsers");
            }

            _context.users.Remove(user);
            _context.SaveChangesAsync(true);

            return RedirectToAction("ViewUsers");
        }

        [HttpGet]
        public IActionResult DetailsUser(int id)
        {
            var user = _context.users.Find(id);
            if (user == null)
            {
                return RedirectToAction("ViewUsers");
            }

            return View(user);
        }



    }
}
