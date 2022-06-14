using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class DriveDTO
    {
        public int DriveId { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? Date { get; set; }
        public int? PassengerRequestId { get; set; }
        public DateTime Time { get; set; }
        public string SourceStreet { get; set; }
        public string DestinationStreet { get; set; }
        public bool IsValid { get; set; }
        public int NumOfSeats { get; set; }
        public bool IsHandicapped { get; set; }

    }
}
