using Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL
{
    public interface IDriveDL
    {
        Task<List<Drive>> GetDriveDLForHistoryAsync(int driverId);
        Task<List<Drive>> GetDriveDLForFutureAsync(int driverId);
        Task<Drive> GetDriveByPassengerRequestIdDLAsync(int prId);
        Task<string> GetStreetByDriverRequestDLAsync(int drId, DateTime dt);
        Task<List<Drive>> GetDriveByDriverRequestIdDLAsync(int drId, DateTime dt);
        Task<Drive> PostDriveDLAsync(Drive d);
        Task DeleteDriveDLAsync(int id);
    }
}