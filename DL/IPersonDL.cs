using DTO;
using Entity;
using System.Threading.Tasks;

namespace DL
{
   public interface IPersonDL
    {
        Task<int> PostPersonDLAsync(Person value);
        Task<Person> PutPersonDLAsync(Person p);
    }
}