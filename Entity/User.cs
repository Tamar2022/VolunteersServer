using System;
using System.Collections.Generic;

#nullable disable

namespace Entity
{
    public partial class User
    {
        public User()
        {
            Drivers = new HashSet<Driver>();
            PassengerRequests = new HashSet<PassengerRequest>();
        }

        public int UserId { get; set; }
        public int PersonId { get; set; }
        public int TypeId { get; set; }

        public virtual Person Person { get; set; }
        public virtual UserType Type { get; set; }
        public virtual ICollection<Driver> Drivers { get; set; }
        public virtual ICollection<PassengerRequest> PassengerRequests { get; set; }
    }
}
