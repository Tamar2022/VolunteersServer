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
   public class DriverDL : IDriverDL
    {
        VolunteersContext volunteersContext;

        public DriverDL(VolunteersContext volunteersContext)
        {
            this.volunteersContext = volunteersContext;
        }
        public async Task<Driver> GetDriverDLAsync(int id)
        {
            var driver = await volunteersContext.Driver.Include(d => d.User.Person).SingleOrDefaultAsync(d=>d.DriverId==id);

            if(driver==null)
                throw new Exception("driver is not exist");
            return driver;

        }


        public async Task<string> GetUserImages(int driverId)
        {
            var img= await volunteersContext.Driver.Where(i => i.DriverId == driverId).Select(d => d.DriverLicense).SingleOrDefaultAsync();
            return img;
        }
        public async Task<List<Driver>> GetAllDriversDLAsync()
        {
            var drivers = await volunteersContext.Driver.Include(d => d.User.Person).ToListAsync();

            if (drivers == null)
                throw new Exception("driver is not exist");
            return drivers;
            //var driver = await volunteersContext.Driver.Include(d=>d.User.Person).Select(u => new DriverDTO
            //{   DriverId=u.DriverId,
            //    PersonId = u.User.PersonId,
            //    FullName = u.User.Person.FullName,
            //    Email = u.User.Person.Email,
            //    Gender = u.User.Person.Gender,
            //    IdentityNumber = u.User.Person.IdentityNumber,
            //    Password = u.User.Person.Password,
            //    Phone = u.User.Person.Phone,
            //   IsActive=u.IsActive,
            //   IsHandicappedCar=u.IsHandicappedCar,
            //   DriverLicense=u.DriverLicense,
            //   UserId=u.UserId
            //}).ToListAsync();

            // if (driver == null)
            //   throw new Exception("there is no driver");
            //return driver;

        }


        public async Task<Driver> PostDriverDLAsync(Driver d)
        {
            var driver = await volunteersContext.Driver.AddAsync(d);
            await volunteersContext.SaveChangesAsync();
            if (driver == null)
                throw new Exception("can`t add");
            
            return d;
        }
        public async Task<int> PutOnlyDriverDLAsync(int id, string driverLicense)
        {
            var driver = await volunteersContext.Driver.FindAsync(id);
            if (driver != null)
            {
                Driver d = new Driver()
                {
                    DriverId = id,
                    DriverLicense = driverLicense,
                    DriverRequests = driver.DriverRequests,
                    IsActive = driver.IsActive,
                    IsHandicappedCar = driver.IsHandicappedCar,
                    User = driver.User,
                    UserId = driver.UserId
                };
                
            volunteersContext.Entry(driver).CurrentValues.SetValues(d);
            await volunteersContext.SaveChangesAsync();
            return id;
            }
            return -1;
        }
        public async Task<Driver> PutDriverDLAsync(int id,Driver d)
        {
            var driver = await volunteersContext.Driver.FindAsync(id);
           
            if (driver == null)
                throw new Exception("driver is not exist");
            d.UserId = driver.UserId;
            volunteersContext.Entry(driver).CurrentValues.SetValues(d);
            await volunteersContext.SaveChangesAsync();
            return d;
        }
        public async Task DeleteDriverDLAsync(int id)
        {
           var d= await volunteersContext.Driver.FindAsync(id);
            if (d == null)
                throw new Exception("driver is not exist");
            volunteersContext.Driver.Remove(d);
            await volunteersContext.SaveChangesAsync();
            
        }
    }

 
    
}
