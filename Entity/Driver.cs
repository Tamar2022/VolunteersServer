using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Entity
{
    public partial class Driver
    {
        public Driver()
        {
            DriverRequests = new HashSet<DriverRequest>();
        }

        public int DriverId { get; set; }
        public int? UserId { get; set; }
        public bool? IsHandicappedCar { get; set; }
        public string DriverLicense { get; set; }
        public bool? IsActive { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        public virtual ICollection<DriverRequest> DriverRequests { get; set; }
    }
}
