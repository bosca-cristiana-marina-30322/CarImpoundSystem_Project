using System.ComponentModel.DataAnnotations;

namespace CarImpoundSystem.Models
{
    public class Vehicle
    {
        [Key]
        public string LicensePlate { get; set; }
        public int VIN { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public string Status { get; set; }

        // Navigation property for ImpoundmentRecords
        public virtual ICollection<ImpoundmentRecord> ImpoundmentRecords { get; set; }
    }
}
