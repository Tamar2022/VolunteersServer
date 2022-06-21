using DL;
using DTO;
using Entity;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BL
{
    public class UserBL : IUserBL
    {   IPersonDL personDL;
        IUserDL userDL;
        IDriverDL driverDL;
        IPasswordHashHelper _passwordHashHelper;
        IMapper _mapper;
        IConfiguration _configuration;
        public UserBL(IConfiguration configuration,IUserDL userDL, IPersonDL personDL, IDriverDL driverDL, IPasswordHashHelper passwordHashHelper, IMapper mapper)
        {
            this.userDL = userDL;
            this.personDL = personDL; 
            this.driverDL = driverDL;
            _passwordHashHelper = passwordHashHelper;
            _mapper = mapper;
            _configuration = configuration;

        }
        public async Task<UserPerson> GetUserBLAsync(string email, string password)
        {
           List<UserPerson> users = await userDL.GetUsersByEmailDLAsync(email);
            if (users == null) return null;
            foreach (var u in users)
            {
                string Hashedpassword = _passwordHashHelper.HashPassword(password, u.Salt, 1000, 8);
                if (Hashedpassword.Equals(u.Password.TrimEnd()))
                {
                    string token;
                    if (u.DriverId != null)
                    {
                        var d = await driverDL.GetDriverDLAsync((int)u.DriverId);
                       
                        if (d.IsActive == true)//the manager let him to join
                        {
                            u.DriverLicense = d.DriverLicense;
                            u.IsHandicappedCar = d.IsHandicappedCar;
                            u.IsActive = d.IsActive;
                            token = createToken(d.DriverId);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        if(u.ManagerId!=null)
                        {
                            token = createToken((int)u.ManagerId);

                        }
                        else//passenger
                        {
                            token = createToken((int)u.PassengerId);
                        }

                        
                    }
                    u.Token = token;
                    return WithoutPassword(u);
                }
            }
            return null;
            //var u= await userDL.GetUserDLAsync(email, password);
            //if (u.DriverId != null)
            //{
            //    var d = await driverDL.GetDriverDLAsync((int)u.DriverId);
            //    u.DriverLicense = d.DriverLicense;
            //    u.IsHandicappedCar = d.IsHandicappedCar;
            //    u.IsActive = d.IsActive;
            //}
            //return u;
        }
       
        //public async Task<int> GetUserTypeBLAsync(string email, string password)
        //{


        //        bool isDriver = false;

        //    var person = await userDL.GetUserDLAsync(email, password);
        //        if (person == null)
        //            throw new Exception("can`t get information");

        //        var users = await userDL.GetUserDLByPersonIdAsync(person.PersonId);
        //        if (users == null)
        //            throw new Exception("can`t get information");
        //        if (users.Count() == 1)
        //            return (int)users[0].TypeId;
        //        if (users.Count() == 2)
        //        {
        //            foreach (var item in users)
        //            {
        //                if (item.TypeId == 0)
        //                {
        //                    return 0;
        //                }
        //                else if (item.TypeId == 1)
        //                    isDriver = true;
        //            }
        //        }
        //        if (isDriver)
        //            return 1;

        //        return 2;
        //    } 

        public async Task<List<UserPerson>> GetUserBLAsync(int typeId)
        {
            List <Person> people= await userDL.GetUserDLAsync(typeId);
            List<UserPerson> UserPersonPeople=new List<UserPerson>();
            foreach (var p in people)
            {
                //UserPerson tmp =new UserPerson()
                //{ PersonId=p.PersonId,Email=p.Email, FullName=p.FullName, Gender=p.Gender, Password=p.Password ,Salt=p.Salt, Phone=p.Phone};
                var up = _mapper.Map<Person, UserPerson>(p);
                
                UserPersonPeople.Add(up);//do it in controller!!
            }
            return WithoutPasswords(UserPersonPeople);
        }
        public async Task<UserPerson> PostUserBLAsync(Person p, int typeId)
        {
            p.Salt = _passwordHashHelper.GenerateSalt(8);
            p.Password = _passwordHashHelper.HashPassword(p.Password, p.Salt, 1000, 8);
           
            int newPersonId = await personDL.PostPersonDLAsync(p);
            if (newPersonId == -1)
                return null;
            User u = new User() { PersonId = newPersonId, TypeId = typeId };
             u=await userDL.PostUserDLAsync(u);
            if (u == null) return null;
            var userPerson = _mapper.Map<Person, UserPerson>(p);
            userPerson.PassengerId = u.UserId;
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("key").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,userPerson.PassengerId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userPerson.Token = tokenHandler.WriteToken(token);
            
            return WithoutPassword(userPerson);
            
        }


        public async Task<Person> PutUserBLAsync(int id,Person p)
        {
            return await personDL.PutPersonDLAsync(p);
            //return await userDL.PutUserDLAsync(id,p);
        }
        public static List<UserPerson> WithoutPasswords(List<UserPerson> users)
        {
            return users.Select(x => WithoutPassword(x)).ToList();
        }
        public  string createToken(int id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("key").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string token1 = tokenHandler.WriteToken(token);
            return token1;
        }

        public static UserPerson WithoutPassword(UserPerson user)
        {
            user.Password = null;
            return user;
        }
    }

}

