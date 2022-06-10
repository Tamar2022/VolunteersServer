using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace DTO
{
  public class DriverDTO
    {
        public int DriverId { get; set; }
        public int? UserId { get; set; }
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string IdentityNumber { get; set; }
        public bool ?Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool? IsHandicappedCar { get; set; }
        public string DriverLicense { get; set; }
        public bool ?IsActive { get; set; }
    }
}
