using DL;
using DTO;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class DriverBL : IDriverBL
    {
        IUserBL userBL;
        IDriveBL driveBL;
        IDriverDL driverDL;
        IPersonDL personDL;
        IPasswordHashHelper _passwordHashHelper;
        public DriverBL(IDriverDL driverDL, IUserBL userBL, IPersonDL personDL, IDriveBL driveBL, IPasswordHashHelper _passwordHashHelper)
        {
            this.driverDL = driverDL;
            this.userBL = userBL;
            this.personDL = personDL;
            this.driveBL = driveBL;
            this._passwordHashHelper = _passwordHashHelper;
        }
        public async Task<Driver> GetDriverBLAsync(int d)
        {
            return await driverDL.GetDriverDLAsync(d);
        }
        public async Task<List<Driver>> GetAllDriversBLAsync()
        {
           
            return await driverDL.GetAllDriversDLAsync();
        }
        public async Task<Driver> PostDriverBLAsync(DriverDTO dd)
        { 

            Driver newD = new Driver()
            {
                DriverId = dd.DriverId,
                UserId = dd.UserId,
                IsHandicappedCar = dd.IsHandicappedCar,
                DriverLicense = null,
                IsActive = dd.IsActive
            };
            Person newP = new Person()
            {
                Salt =  dd.Salt,
                PersonId = dd.PersonId,
                Password =dd.Password,    
                FullName = dd.FullName,
                IdentityNumber = dd.IdentityNumber,
                Gender = dd.Gender,
                Phone = dd.Phone,
                Email = dd.Email
            };
           
            

            var user = await userBL.PostUserBLAsync(newP, 1);
            newD.UserId = user.UserId;
            var driver = await driverDL.PostDriverDLAsync(newD);

            return driver;
        }
        public async Task<int> PutOnlyDriverBLAsync(int id, string driverLicense)
        {
            
            return await driverDL.PutOnlyDriverDLAsync(id, driverLicense);
        }
        public async Task<Driver> PutDriverDTOBLAsync(int id, DriverDTO dd)
        {
            Driver newD = new Driver() { DriverId = dd.DriverId, UserId = dd.UserId,
                IsHandicappedCar = dd.IsHandicappedCar, IsActive = dd.IsActive };// DriverLicense = dd.DriverLicense,???
            var d= await driverDL.PutDriverDLAsync(id, newD);
            Person newP = new Person() { PersonId = (int)dd.PersonId, Password = dd.Password, FullName = dd.FullName,
                IdentityNumber = dd.IdentityNumber, Gender = dd.Gender, Phone = dd.Phone, Email = dd.Email };
            var p= await personDL.PutPersonDLAsync(newP);
            if (p == null)
               throw new Exception("no person like that");
            //driveBL.send(dd.Email);
            return d;
        }
        public async Task<Driver> PutDriverBLAsync(int id, UserPerson dd)
        {
            Driver newD = new Driver()
            {
                DriverId = (int)dd.DriverId,
               
                IsHandicappedCar = dd.IsHandicappedCar,
                DriverLicense = dd.DriverLicense,
                IsActive = dd.IsActive
            };
         
            var d = await driverDL.PutDriverDLAsync(id, newD);
            Person newP = new Person()
            {
                PersonId = (int)dd.PersonId,
                Password = dd.Password,
                FullName = dd.FullName,
                IdentityNumber = dd.IdentityNumber,
                Gender = dd.Gender,
                Phone = dd.Phone,
                Email = dd.Email
            };
            var p = await personDL.PutPersonDLAsync(newP);
            if (p == null)
                throw new Exception("no person like that");
            return d;
        }
        public async Task DeleteDriverBLAsync(int id)
        {
             await driverDL.DeleteDriverDLAsync(id);
        }
    }
}
