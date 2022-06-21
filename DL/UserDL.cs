using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DL
{
    public class UserDL : IUserDL
    {

        VolunteersContext volunteersContext;

        public UserDL(VolunteersContext volunteersContext)
        {
            this.volunteersContext = volunteersContext;
        }
        public async Task<List<Person>> GetUserDLAsync(int typeId)
        {
            var user = await volunteersContext.User.Where(u => u.TypeId == typeId).ToListAsync();
            if (user.Count() == 0)
            {
                throw new Exception("can`t get information");
            }
            List<Person> person = new List<Person>();
            foreach (var item in user)
            {
                var p = await volunteersContext.Person.SingleOrDefaultAsync(p1 => p1.PersonId == item.PersonId);
                if (p != null)
                    person.Add(p);
            }
            if (person == null)
                throw new Exception("can`t get information!");
            return person;
        }


        //public async Task<UserPerson> GetUserDLAsync(string email , string password)
        //{

        //    var person = await volunteersContext.Person.Where(u => u.Password == password && u.Email == email).Select(u => new UserPerson
        //    {
        //        PersonId = u.PersonId,
        //        FullName = u.FullName,
        //        Email = u.Email,
        //        Gender = u.Gender,
        //        IdentityNumber = u.IdentityNumber,
        //        Password = u.Password,
        //        Phone = u.Phone,
        //        ManagerId = u.Users.SingleOrDefault(m => m.TypeId == 0).UserId,
        //        DriverId =( volunteersContext.Driver.SingleOrDefault(y => y.UserId == u.Users.SingleOrDefault(d => d.TypeId == 1).UserId)).DriverId,//add async
        //        PassengerId = u.Users.SingleOrDefault(d => d.TypeId == 2).UserId,//??? ask///

        //    }).SingleOrDefaultAsync<UserPerson>();
            
        //    if (person == null)
        //        throw new Exception("can`t get information");

        //    return person;
        //}
      
              public async Task <List<UserPerson>> GetUsersByEmailDLAsync(string email)
        {

            var person = await volunteersContext.Person.Where(u =>u.Email == email).Select(u => new UserPerson
            {
                PersonId = u.PersonId,
                FullName = u.FullName,
                Email = u.Email,
                Gender = u.Gender,
                IdentityNumber = u.IdentityNumber,
                Password = u.Password,
                Phone = u.Phone,
                Salt=u.Salt,
                ManagerId = u.Users.SingleOrDefault(m => m.TypeId == 0).UserId,
                DriverId = (volunteersContext.Driver.SingleOrDefault(y => y.UserId == u.Users.SingleOrDefault(d => d.TypeId == 1).UserId)).DriverId,//add async
                PassengerId = u.Users.SingleOrDefault(d => d.TypeId == 2).UserId

            }).ToListAsync<UserPerson>();

            if (person == null)
                throw new Exception("can`t get information");

            return person;
        }
        public async Task<List<User>> GetUserDLByPersonIdAsync(int personId)
        {

            var users = await volunteersContext.User.Where(u => u.PersonId == personId).ToListAsync();
            return users;
        }
        public async Task<User> PostUserDLAsync(User u)
        {
            var u1 = await volunteersContext.User.AddAsync(u);
            await volunteersContext.SaveChangesAsync();
            if (u1 == null)

                return null;


            return u;


        }


        public async Task<Person> PutUserDLAsync(int id, Person p)
        {

            var p1 = await volunteersContext.Person.FindAsync(id);
            if (p1 == null)
                throw new Exception("not exist!");
            p.Password = p1.Password;
            p.Salt = p1.Salt;
            volunteersContext.Entry(p1).CurrentValues.SetValues(p);
            await volunteersContext.SaveChangesAsync();
            return p;
        }

    }
}
