using System;
using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DL
{
    public class DriveDL : IDriveDL
    {
        VolunteersContext volunteersContext;

        public DriveDL(VolunteersContext volunteersContext)
        {
            this.volunteersContext = volunteersContext;
        }

        //get by driverId for history list
        public async Task<List<Drive>> GetDriveDLForHistoryAsync(int driverId)
        {

            var historyDrive = await volunteersContext.Drive.Include(d=>d.DriverRequest).Select(d => new
            {
                
                dId = d.DriverRequest.DriverId,
                date = d.Date,
                driveId = d.DriveId,
                driverRequestId = d.DriverRequestId,
                PassengerRequestId = d.PassengerRequestId,
                IsApproved = d.IsApproved
            })
                .Where(dd=>dd.dId == driverId && dd.date<DateTime.Now)
                .ToListAsync();
               
            if (historyDrive.Count==0)
            { 
                throw new Exception("not found");
            }
            List<Drive> drives = new List<Drive>();
            foreach (var item in historyDrive)
            {
                Drive d = new Drive() { Date = item.date, DriveId = item.driveId, DriverRequestId = item.driverRequestId, PassengerRequestId = item.PassengerRequestId,IsApproved=item.IsApproved };
                drives.Add(d);
            }
            return drives;
        }
        //get by driverId for future list

        public async Task<List<Drive>> GetDriveDLForFutureAsync(int driverId)
        {
            var FutureDrive = await volunteersContext.Drive
                .Include(d => d.DriverRequest).Select(d => new
                {

                    dId = d.DriverRequest.DriverId,
                    date = d.Date,
                    driveId = d.DriveId,
                    driverRequestId = d.DriverRequestId,
                    PassengerRequestId = d.PassengerRequestId,
                    IsApproved = d.IsApproved
                })
                .Where(dd => dd.dId == driverId && dd.date >= DateTime.Now)
                .ToListAsync();
                   

            if (FutureDrive.Count == 0)
            {
                throw new Exception("not found");
            }
            List<Drive> drives = new List<Drive>();
            foreach (var item in FutureDrive)
            {
                Drive d = new Drive() { Date = item.date, DriveId = item.driveId, DriverRequestId = item.driverRequestId, PassengerRequestId = item.PassengerRequestId,IsApproved=item.IsApproved };
                drives.Add(d);
            }
            return drives;
        }

        public async Task<Drive> GetDriveByPassengerRequestIdDLAsync(int prId)
        {
            Drive d = await volunteersContext.Drive.Include(d => d.DriverRequest.Driver).Where(d => d.PassengerRequestId == prId).SingleOrDefaultAsync();
            return d;
        }
        public async Task<List<Drive>> GetDriveByDriverRequestIdDLAsync(int drId, DateTime dt)
        {
            List<Drive> drl = await volunteersContext.Drive.Include(d => d.PassengerRequest.User.Person).Where(d => d.DriverRequestId == drId && d.Date.Equals(dt)).ToListAsync();
            return drl;
        }

        public async Task<string> GetStreetByDriverRequestDLAsync(int drId, DateTime dt)//returns street
        {
            int passengerReqId;
            List<Drive> drl = await volunteersContext.Drive.Where(d => d.DriverRequestId == drId && d.Date.Equals(dt)).ToListAsync();
            if (drl.Count != 0)//if he is not the first passenger
            {
                passengerReqId = (int)drl[drl.Count - 1].PassengerRequestId;
                PassengerRequest pr = await volunteersContext.PassengerRequest.Where(p => p.PassengerRequestId == passengerReqId).SingleOrDefaultAsync();
                return pr.SourceStreet;
            }

            else//he is  the first passenger
                return null;
        }

        //post 
        public async Task<Drive> PostDriveDLAsync(Drive d)
        {
            var d1 = await volunteersContext.Drive.AddAsync(d);
            await volunteersContext.SaveChangesAsync();
            if (d1 == null)
                throw new Exception("can`t add");
            return d;
        }

        public async Task DeleteDriveDLAsync(int id)
        {
            var d = await volunteersContext.Drive.FindAsync(id);
            volunteersContext.Drive.Remove(d);
            await volunteersContext.SaveChangesAsync();

        }


        // put
        //public async Task<bool> PutDriveDLAsync(int driverId)//??????????
        //{
        //    var existingDrive = await volunteersContext.Drive.FindAsync();

        //    if (existingPassengerRequest != null)
        //    {
        //        volunteersContext.Entry(existingPassengerRequest).CurrentValues.SetValues(pr);
        //        await volunteersContext.SaveChangesAsync();
        //        return pr;
        //    }
        //    else
        //        throw new Exception("not found");

        //}
    }
}
