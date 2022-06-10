using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Entity
{
    public partial class Person
    {
        public Person()
        {
            Users = new HashSet<User>();
        }

        public int PersonId { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string IdentityNumber { get; set; }
        public bool? Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
}
