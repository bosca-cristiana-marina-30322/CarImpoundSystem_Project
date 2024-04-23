using CarImpoundSystem.Data;
using CarImpoundSystem.Models;
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
        //public async Task<ActionResult> ViewImpound()
        //{
        //    return View(await _context.impoundmentRecords.ToListAsync());
        //}

        [HttpGet]
        public async Task<IActionResult> SearchImpound(string? LicensePlate)
        {


            var impound =  _context.impoundmentRecords.Where( u  => u.LicensePlate == LicensePlate);



            return View(impound);
        }

        public async Task<IActionResult> ImpoundDetails(string? id)
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

        public  IActionResult Index()
        {
            return View();
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
