using DTO;
using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{

 public class PersonDL : IPersonDL
    {

        VolunteersContext volunteersContext;

        public PersonDL(VolunteersContext volunteersContext)
        {
            this.volunteersContext = volunteersContext;
        }

        public async Task<int> PostPersonDLAsync(Person value)
        {
           
            var person = await volunteersContext.Person.SingleOrDefaultAsync(u => u.Password == value.Password  && u.Email == value.Email);//check if this user name and password is already exist.
            if (person == null)
            {
                var p = await volunteersContext.Person.AddAsync(value);
                await volunteersContext.SaveChangesAsync();
                if (p == null)
                    throw new Exception("cannot add");
                return p.Entity.PersonId;
            }
            else
            {
                return person.PersonId;
            }            
        }
          
        public async Task<Person> PutPersonDLAsync(Person p)
        {
            var person = await volunteersContext.Person.FindAsync(p.PersonId);
            if (person!=null)
            {
                p.Password = person.Password;
                p.Salt = person.Salt;
                volunteersContext.Entry(person).CurrentValues.SetValues(p);
                await volunteersContext.SaveChangesAsync();
                return p;

            }
            else
                return null;

        }

    }
}
