using CarImpoundSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarImpoundSystem.Controllers
{
    public class UserController : Controller
    {

        private readonly AppDBContext _context;
        public UserController(AppDBContext context)
        {
            _context = context;
        }

        //FIX THIS
        public async Task<ActionResult> ViewImpounds(String? LicencePlate)
        {
            if (LicencePlate == null)
            {
                return NotFound();
            }

            var impound = await _context.impoundmentRecords.FindAsync(LicencePlate);
            if (impound == null)
            {
                return NotFound();
            }
            return View(impound);
        }
        public IActionResult Index()
        {
            var users = _context.users.ToList();
            return View(users);
        }
        public IActionResult SearchByLicense()
        {
            return View();
        }


        public ActionResult Users()
        {
            var Users = _context.users.ToList(); // Fetch users from the database
            return View(Users); // Pass users to the view
        }
    }
}
