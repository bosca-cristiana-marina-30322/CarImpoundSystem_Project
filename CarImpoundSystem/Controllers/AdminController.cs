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
        public IActionResult ChangeImpound()
        {
            // Add any necessary logic here
            return View();
        }
        // GET: Vehicle/EditImpound/5

        // GET: Impound/EditImpound/5
        [HttpGet]
        public async Task<IActionResult> EditImpound(string id)
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

        // POST: Impound/EditImpound/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditImpound(string id, [Bind("recordId, status, payment")] ImpoundmentRecord impound)
        {
            if (id != impound.recordId || id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing impound record from the database
                    var existingRecord = await _context.impoundmentRecords.FindAsync(id);

                    if (existingRecord == null)
                    {
                        return NotFound();
                    }

                    // Update the fields
                    existingRecord.status = impound.status;
                    existingRecord.payment = impound.payment;

                    // Save changes to the database
                    _context.Update(existingRecord);
                    await _context.SaveChangesAsync();

                    // Redirect to the ViewImpounds action
                    return RedirectToAction("ViewImpounds");
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
            }
            // If ModelState is not valid, return the view with validation errors
            return View(impound);
        }



        private bool ImpoundmentRecordExists(string impound)
        {
            return _context.impoundmentRecords.Any(e => e.recordId == impound);
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
        [HttpGet]
        public async Task<IActionResult> ViewImpounds()
        {
            var impounds = await _context.impoundmentRecords.ToListAsync();
            return View(impounds);
        }
        // GET: Employees/Edit/5
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.users.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, [Bind("Id,FirstName,LastName")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    int userid= user.UserId;
                    if (!UserExists(userid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ViewUsers));
            }
            return View(ViewUsers);
        }

        private bool UserExists(int user)
        {
            return _context.users.Any(e => e.UserId == user);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewUsers));
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
