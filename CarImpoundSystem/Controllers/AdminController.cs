using CarImpoundSystem.Data;
using CarImpoundSystem.Models;
using CarImpoundSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
                return RedirectToAction(nameof(Index));
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

        public async Task<ActionResult> Users()
        {
            return View(await _context.users.ToListAsync());
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
                return RedirectToAction("ViewImpounds", "Admin");
            }

            // Authentication failed, display an error message
            ViewBag.ErrorMessage = "Invalid username or password.";

            return View();
        }
    }
}
