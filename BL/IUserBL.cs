using DTO;
using Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace BL
{
     public interface IUserBL
    {
        Task<UserPerson> GetUserBLAsync(string email, string password);
        //Task<int> GetUserTypeBLAsync(string email, string password);
        Task<List<UserPerson>> GetUserBLAsync(int typeId);
        Task<User> PostUserBLAsync(Person value, int typeId);
        Task<Person> PutUserBLAsync(int id,Person p);

    }
}