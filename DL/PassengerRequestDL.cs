using System;
using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DL
{
    public class PassengerRequestDL : IPassengerRequestDL 
    {

        VolunteersContext volunteersContext;

        public PassengerRequestDL(VolunteersContext volunteersContext)
        {
            this.volunteersContext = volunteersContext;
        }
        public async Task<int> GetSumOfSeatsPassengerRequestByDriveDLAsync(DriverRequest dr, DateTime dt)
        {
            int dr_id = dr.DriverRequestId;
            int sumOfSeats = await volunteersContext.PassengerRequest.Join(volunteersContext.Drive, pr => pr.PassengerRequestId,
                drive => drive.PassengerRequestId, (pr, drive) => new { Pr = pr, Dr = drive }).Where(d => d.Dr.DriverRequestId == dr_id && d.Dr.Date == dt).SumAsync(o => o.Pr.NumOfSeats);



            //int x =(int) await volunteersContext.PassengerRequest.SumAsync(o => o.NumOfSeats);
            return sumOfSeats;
        }
        //get by userId
        public async Task<List<PassengerRequest>> GetPassengerRequestDLAsync(int userId)
        {
            var PassengerRequest = await volunteersContext.PassengerRequest.Where(pr => pr.UserId == userId && pr.IsValid == false).ToListAsync();
            if (PassengerRequest == null)
                throw new Exception("not found");
            return PassengerRequest;
        }
        public async Task<List<PassengerRequest>> GetPassengerRequestByDateDLAsync(DateTime date, DateTime lHour, string destination)
        {

            //var PassengerRequest = await volunteersContext.PassengerRequest.Where(pr => 

            //    pr.Date.ToString("dd/MM/yyyy").Equals(date.ToString("dd/MM/yyyy")) && DateTime.Parse(pr.Time.ToString()).ToString("HH:mm")
            //    .Equals(DateTime.Parse(lHour.ToString()).ToString("HH:mm")) && pr.DestinationStreet.Equals(destination)).ToListAsync();//compare date and time and destination


            var PassengerRequest = await volunteersContext.PassengerRequest.Include(pr => pr.User.Person).Where(pr =>

                  pr.Date.Equals(date) && (pr.Time).Equals(lHour) && pr.DestinationStreet.Equals(destination) && pr.IsValid == true).ToListAsync();//compare date and time and destination

            return PassengerRequest;
        }
        

        //post
        public async Task<PassengerRequest> PostPassengerRequestDLAsync(PassengerRequest pr)
        {
            var pr1 = await volunteersContext.PassengerRequest.AddAsync(pr);
            await volunteersContext.SaveChangesAsync();
            if (pr1 == null)
                  throw new Exception("not found");
            return pr;
        }

        // put
        public async Task<PassengerRequest> PutPassengerRequestDLAsync(int id,PassengerRequest pr)
        {
            var existingPassengerRequest = await volunteersContext.PassengerRequest.FindAsync(id);

            if (existingPassengerRequest != null)
            {
                volunteersContext.Entry(existingPassengerRequest).CurrentValues.SetValues(pr);
                await volunteersContext.SaveChangesAsync();
                return pr;
            }
            else
                throw new Exception("not found");

        }

        //delete
        public async Task<bool> DeletePassengerRequestDLAsync(int PassengerRequestId)
        {
            var pr = await volunteersContext.PassengerRequest.FindAsync(PassengerRequestId);
            if (pr == null)
                return false;
            try
            {
                volunteersContext.PassengerRequest.Remove(pr);
                await volunteersContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;

            }



        }
    
}
}
