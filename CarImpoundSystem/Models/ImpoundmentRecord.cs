using System.ComponentModel.DataAnnotations.Schema;

namespace CarImpoundSystem.Models
{
        public class ImpoundmentRecord
        {
            public string recordId { get; set; }

            public DateTime date { get; set; }

            public string location { get; set; }

            public string reason { get; set; }

            public double payment { get; set; }

            public string status { get; set; }

            //Relationship
            public string LicensePlate { get; set; }
            [ForeignKey("LicensePlate")]
            public Vehicle vehicle { get; set; }
        }
    }
}

