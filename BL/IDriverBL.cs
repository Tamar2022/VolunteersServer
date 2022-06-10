using DTO;
using Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public interface IDriverBL
    {
        Task<Driver> GetDriverBLAsync(int d);
        Task<List<Driver>> GetAllDriversBLAsync();
        Task<int> PutOnlyDriverBLAsync(int id,string driverLicense);
        Task<Driver> PutDriverDTOBLAsync(int id, DriverDTO d);
        Task<Driver> PutDriverBLAsync(int id, UserPerson d);

        Task<Driver> PostDriverBLAsync(DriverDTO pd);
        Task DeleteDriverBLAsync(int id);
    }
}