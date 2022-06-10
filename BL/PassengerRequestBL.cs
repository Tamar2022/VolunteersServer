using DL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class PassengerRequestBL : IPassengerRequestBL 
    {
        IPassengerRequestDL PassengerRequestDL;
        public PassengerRequestBL(IPassengerRequestDL PassengerRequestDL)
        {
            this.PassengerRequestDL = PassengerRequestDL;
        }
        public async Task<int> GetSumOfSeatsPassengerRequestByDriveBLAsync(DriverRequest dr, DateTime dt)
        {
            return await PassengerRequestDL.GetSumOfSeatsPassengerRequestByDriveDLAsync(dr, dt);
        }
        //get by userId
        public async Task<List<PassengerRequest>> GetPassengerRequestBLAsync(int userId)
        {
            return await PassengerRequestDL.GetPassengerRequestDLAsync(userId);
        }
        //post
        public async Task< PassengerRequest> PostPassengerRequestBLAsync(PassengerRequest pr)
        {
            return await PassengerRequestDL.PostPassengerRequestDLAsync(pr);
            
        }
        //put
        public async Task<PassengerRequest> PutPassengerRequestBLAsync(int id,PassengerRequest pr)
        {
            return await PassengerRequestDL.PutPassengerRequestDLAsync(id,pr);
             
        }
        //delete
        public async Task<bool> DeletePassengerRequestBLAsync(int PassengerRequestId)
        {
            return await PassengerRequestDL.DeletePassengerRequestDLAsync(PassengerRequestId);
            
        }
    }
}
