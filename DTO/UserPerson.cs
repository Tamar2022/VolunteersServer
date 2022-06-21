using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTO
{
    public class UserPerson
    {
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
        public string IdentityNumber { get; set; }
        public bool? Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool? IsHandicappedCar { get; set; }
        public string ?DriverLicense { get; set; }
        public bool? IsActive { get; set; }
        
        public int? ManagerId { get; set; }
        public int? DriverId { get; set; }
        public int? PassengerId { get; set; }

        public string Token { get; set; }

    }
}
