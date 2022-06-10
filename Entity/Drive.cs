using System;
using System.Collections.Generic;

#nullable disable

namespace Entity
{
    public partial class Drive
    {
        

        public int DriveId { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? Date { get; set; }
        public int? PassengerRequestId { get; set; }
        public int? DriverRequestId { get; set; }

        public virtual DriverRequest DriverRequest { get; set; }
        public virtual PassengerRequest PassengerRequest { get; set; }
    }
}
