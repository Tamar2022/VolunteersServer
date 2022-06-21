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
        Task<UserPerson> PostUserBLAsync(Person p, int typeId);
        Task<Person> PutUserBLAsync(int id,Person p);

    }
}