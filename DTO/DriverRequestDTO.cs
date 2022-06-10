using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DriverRequestDTO
    {
            public int DriverRequestId { get; set; }
            public DateTime? LeavingHour { get; set; }
            public string SourceStreet { get; set; }
            public string DestinationStreet { get; set; }
            public int? Day { get; set; }
            public int? DriverId { get; set; }
            public int? NumOfSeats { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

        public DateTime date { get; set; }
    }
    }



