using Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL
{
    public interface IPassengerRequestDL
    {
       Task<List<PassengerRequest>> GetPassengerRequestDLAsync(int userId);
        Task<int> GetSumOfSeatsPassengerRequestByDriveDLAsync(DriverRequest dr, DateTime dt);
        Task<List<PassengerRequest>> GetPassengerRequestByDateDLAsync(DateTime date, DateTime lHour, string destination);       Task<PassengerRequest> PostPassengerRequestDLAsync(PassengerRequest pr);
       Task<PassengerRequest> PutPassengerRequestDLAsync(int id,PassengerRequest pr);
       Task<bool>DeletePassengerRequestDLAsync(int PassengerRequestId);
    }
}