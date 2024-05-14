using CarImpoundSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarImpoundSystem.Data
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) :base(options) {
        
        }
        public DbSet<Vehicle> vehicles { get; set; }
        public DbSet<ImpoundmentRecord> impoundmentRecords { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<User> users { get; set; }


    }
}
