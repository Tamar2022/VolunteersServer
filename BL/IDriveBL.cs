using Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public interface IDriveBL
    {
        Task<List<Drive>> GetFutureDrivesBLAsync(int userId);
        Task<List<Drive>> GetDriveBLForHistoryAsync(int driverId);
        Task<List<Drive>> GetDriveBLForFutureAsync(int driverId);
        Task<Drive> PostDriveBLAsync(Drive d);
        //Task<Drive> PostDriveBLForRecorderedRequestAsync(/*הקלטה,*/int driverId);
        //Task PutAsync();
        Task DeleteDriveBLAsync(int id);
        //void send();

    }
}