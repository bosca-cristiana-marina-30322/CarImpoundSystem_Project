using CarImpoundSystem.Data;
using CarImpoundSystem.Models;
using CarImpoundSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing.Printing;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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


        public async Task<IActionResult> DetailsImpound(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var impound = await _context.impoundmentRecords.FindAsync(id);
            if (impound == null)
            {
                return NotFound();
            }
            return View(impound);
        }

        public async Task<IActionResult> EditImpound(String? recordid)
        {
            if (recordid == null)
            {
                return NotFound();
            }

            var impoundment = await _context.impoundmentRecords.FindAsync(recordid);
            if (impoundment == null)
            {
                return NotFound();
            }
            return View(impoundment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditImpound(String recordId, [Bind("Id,FirstName,LastName")] ImpoundmentRecord impound)
        {
            if (recordId != impound.recordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recordId);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (ImpoundmentRecordExists(impound.recordId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AdminIndex));
            }
            return View(impound);
        }
        private bool ImpoundmentRecordExists(string impound)
        {
            return _context.impoundmentRecords.Any(e => e.recordId == impound);
        }



        public async Task<ActionResult> ViewImpounds()
        {
            return View(await _context.impoundmentRecords.ToListAsync());
        }

        public ActionResult AdminIndex()
        {
            return View();
        }

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

        //GET: Employee/Edit
        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {

            //Fetch the employee details
            var user = await _context.users.FindAsync(id);
            return View(user);
        }

        //POST: Employee/Edit
        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {

            if (!ModelState.IsValid)
            {
                return View(user); // Return to the form with validation errors
            }
            //Update the database with modified details
            _context.users.Update(user);

            // Redirect to List all department page
            return RedirectToAction("ViewUsers", "Admin");
        }
        private bool UserExists(int user)
        {
            return _context.users.Any(e => e.UserId == user);
        }



        



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
    }
}
