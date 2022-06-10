using DTO;
using Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public interface IMatchingFunctionBL
    {
        List<DateTime> GetWeekdayInRange(DateTime from, DateTime to, DayOfWeek day);
        Task<bool> MatchingFunctionForDriverReq(DriverRequest dr);
        Task<bool> MatchingFunctionForOneDriverReq(DriverRequest dr, DateTime dt);
        Task<bool> MatchingFunctionForPassengerRequest(PassengerRequest pr);
        Task MatchingFunctionForCancelDriverRequest(DriverRequestDTO dr);
        Task MatchingFunctionForCancelPassengerRequest(PassengerRequest pr);
        void sendEmail(string to, string mailBody);
    }
}