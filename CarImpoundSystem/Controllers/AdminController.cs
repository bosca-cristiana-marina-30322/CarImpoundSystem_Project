using CarImpoundSystem.Data;
using CarImpoundSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarImpoundSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDBContext _context;

        public AdminController(AppDBContext context)
        {
            _context = context;
        }

        public ActionResult Login(string username, string password)
        {
            var user = _context.users.FirstOrDefault(u => u.username == username);
            if (user != null && user.password == password && user.role == "Admin")
            {
                return RedirectToAction("AdminIndex", "Admin");
            }

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                ViewBag.ShowPopup = true;
            }
            else
            {
                ViewBag.ShowPopup = false;
            }
            return View();
        }

        public ActionResult AdminIndex()
        {
            return View();
        }

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

            var impound = await _context.impoundmentRecords
                                        .Include(ir => ir.User)
                                        .Include(ir => ir.vehicle)
                                        .FirstOrDefaultAsync(ir => ir.recordId == id);

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
            var model = new ImpoundmentRecord
            {
                date = impoundmentRecord.date,
                location = impoundmentRecord.location,
                reason = impoundmentRecord.reason,
                LicensePlate = impoundmentRecord.LicensePlate,
                status = "in",
                payment = 300 // Set initial payment to 300
            };

            await _context.impoundmentRecords.AddAsync(model);
            await _context.SaveChangesAsync();

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

            return RedirectToAction("AddImpound");
        }

        [HttpGet]
        public async Task<ActionResult> EditImpound(string id)
        {
            var record = await _context.impoundmentRecords.FindAsync(id);
            return View(record);
        }

        [HttpPost]
        public async Task<ActionResult> EditImpound(ImpoundmentRecord record)
        {
            var updatedRecord = await _context.impoundmentRecords.FindAsync(record.recordId);

            if (updatedRecord != null)
            {
                updatedRecord.payment = record.payment;
                updatedRecord.status = record.status;

                await _context.SaveChangesAsync();
            }

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

        public async Task<ActionResult> ViewUsers()
        {
            return View(await _context.users.ToListAsync());
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            User model = new User()
            {
                username = user.username,
                password = user.password,
                role = user.role,
            };

            await _context.users.AddAsync(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewUsers");
        }

        [HttpGet]
        public async Task<ActionResult> EditUser(int id)
        {
            var user = await _context.users.FindAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> EditUser(User user)
        {
            var updatedUser = await _context.users.FindAsync(user.UserId);

            if (updatedUser != null)
            {
                updatedUser.username = user.username;
                updatedUser.password = user.password;
                updatedUser.role = user.role;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewUsers");
        }

        public IActionResult DeleteUser(int id)
        {
            var user = _context.users.Find(id);
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
