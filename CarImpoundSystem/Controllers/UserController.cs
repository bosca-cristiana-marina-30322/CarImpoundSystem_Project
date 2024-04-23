using CarImpoundSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

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
        public async Task<ActionResult> ViewImpound()
        {
            return View(await _context.impoundmentRecords.ToListAsync());
        }

        public async Task<IActionResult> Index(String LicensePlate)
        {

            var impound = await _context.impoundmentRecords.ToListAsync();

            // Search functionality
            if (!string.IsNullOrEmpty(LicensePlate))
            {
                impound = (List<Models.ImpoundmentRecord>)impound.Where(e => e.LicensePlate.Contains(LicensePlate));
            }

            
            return View(ViewImpound);

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
