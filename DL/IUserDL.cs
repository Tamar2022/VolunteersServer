using DTO;
using Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL
{
  public  interface IUserDL
    {
        //Task<UserPerson> GetUserDLAsync(string email, string password);
        Task<List<UserPerson>> GetUsersByEmailDLAsync(string email);
        Task<List<User>> GetUserDLByPersonIdAsync(int personId);
        
          Task<List<Person>> GetUserDLAsync(int typeId);
        Task<User> PostUserDLAsync(User u);
        Task<Person> PutUserDLAsync(int id,Person u);
    }
}