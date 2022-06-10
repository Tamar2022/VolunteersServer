using System;
using System.Collections.Generic;

#nullable disable

namespace Entity
{
    public partial class DriverRequest
    {
        public DriverRequest()
        {
            Drives = new HashSet<Drive>();
        }

        public int DriverRequestId { get; set; }
        public DateTime? LeavingHour { get; set; }
        public string SourceStreet { get; set; }
        public string DestinationStreet { get; set; }
        public int? Day { get; set; }
        public int? DriverId { get; set; }
        public int? NumOfSeats { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual Driver Driver { get; set; }
        public virtual ICollection<Drive> Drives { get; set; }
    }
}
