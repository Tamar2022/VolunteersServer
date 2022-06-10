using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Entity
{
    public partial class PassengerRequest
    {
        public PassengerRequest()
        {
            Drives = new HashSet<Drive>();
        }

        public int PassengerRequestId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string SourceStreet { get; set; }
        public string DestinationStreet { get; set; }
        public bool IsValid { get; set; }
        public int NumOfSeats { get; set; }
        public bool IsHandicapped { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual ICollection<Drive> Drives { get; set; }
    }
}
