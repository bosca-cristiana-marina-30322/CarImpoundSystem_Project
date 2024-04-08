using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarImpoundSystem.Models
{
    public class Payment
    {
        [Key]
        public string paymentId { get; set; }

        public DateTime date { get; set; }

        public double amount { get; set; }

        //Relationship
        public string LicensePlate { get; set; }
        [ForeignKey("LicensePlate")]
        public Vehicle vehicle { get; set; }
    }
}
