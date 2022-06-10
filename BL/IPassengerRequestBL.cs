using Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public interface IPassengerRequestBL
    {
      Task<List<PassengerRequest>> GetPassengerRequestBLAsync(global::System.Int32 userId);
       Task<PassengerRequest> PostPassengerRequestBLAsync(PassengerRequest pr);
        Task<PassengerRequest> PutPassengerRequestBLAsync(int id,PassengerRequest pr); 
        
         Task<bool> DeletePassengerRequestBLAsync(int PassengerRequestId);
    }
}