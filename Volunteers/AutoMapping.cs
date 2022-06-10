using AutoMapper;
using DTO;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volunteers
{
    public class AutoMapping :Profile
    {
        public AutoMapping()
        {
            CreateMap<Driver, DriverDTO>().ForMember(d=> d.FullName,d=>d.MapFrom(d=>d.User.  Person.FullName)).ForMember(d => d.Password, d => d.MapFrom(d => d.User.Person.Password))
                .ForMember(d => d.IdentityNumber, d => d.MapFrom(d => d.User.Person.IdentityNumber)).ForMember(d => d.Gender, d => d.MapFrom(d => d.User.Person.Gender))
                .ForMember(d => d.Phone, d => d.MapFrom(d => d.User.Person.Phone)).ForMember(d => d.Email, d => d.MapFrom(d => d.User.Person.Email));
            CreateMap<PassengerRequest, PassengerRequestDTO>();
            CreateMap<Person, UserPerson>().ForMember(u => u.PassengerId, p => p.MapFrom(p => p.Users.SingleOrDefault(u => u.TypeId == 2).UserId))
                //.ForMember(u => u.DriverId, p => p.MapFrom(p => p.Users.SingleOrDefault(u => u.UserId == p.Users.SingleOrDefault(d => d.TypeId == 1).UserId)
                //.Drivers.Select(d => {
                //    if (d != null) 
                //        return d.DriverId;
                //      })))
                .ForMember(u=>u.ManagerId,p=>p.MapFrom(p=>p.Users.SingleOrDefault(m => m.TypeId == 0).UserId));

            CreateMap<DriverRequest, DriverRequestDTO>(); 
                


        }
    }
}
