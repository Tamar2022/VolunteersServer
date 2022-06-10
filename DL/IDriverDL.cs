using DTO;
using Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL
{
    public interface IDriverDL
    {
        Task<Driver> GetDriverDLAsync(int d);
        Task<List<Driver>> GetAllDriversDLAsync();
        Task<string> GetUserImages(int driverId);
        Task<Driver> PostDriverDLAsync(Driver d);
        Task<int> PutOnlyDriverDLAsync(int id, string driverLicense);
        Task<Driver> PutDriverDLAsync(int id,Driver d);
        Task DeleteDriverDLAsync(int id);

    }
}