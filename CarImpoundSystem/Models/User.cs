using System.ComponentModel.DataAnnotations;

namespace CarImpoundSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }

        // Navigation property for ImpoundmentRecords
        public virtual ICollection<ImpoundmentRecord> ImpoundmentRecords { get; set; }
    }
}
