using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarImpoundSystem.Models
{
        public class ImpoundmentRecord
        {
        [Key]
            public string recordId { get; set; }

            public DateTime date { get; set; }

            public string location { get; set; }

            public string reason { get; set; }

            public double payment { get; set; }

            public string status { get; set; }

        //Relationship
        // Foreign key for User
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        // Foreign key for Vehicle
        public string LicensePlate { get; set; }
            [ForeignKey("LicensePlate")]
            public Vehicle vehicle { get; set; }
        }
}


