using Entity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
  public  class DriverRequestDL : IDriverRequestDL
    {
        VolunteersContext volunteersContext;

        public DriverRequestDL(VolunteersContext volunteersContext)
        {
            this.volunteersContext = volunteersContext;
        }
        //GetPermanentRideDLAsync
        public async Task<List<DriverRequest>> GetDriverRequestDLAsync(int DriverId)
        {
            var drive = await volunteersContext.DriverRequest.Where(u => u.DriverId == DriverId && u.EndDate>=DateTime.Now).ToListAsync();
            return drive;
        }
        public async Task<List<DriverRequest>> GetDriverRequestByDayDLAsync(DateTime time, string destination)
        {

            var drive = await volunteersContext.DriverRequest.Include(dr => dr.Driver.User.Person).Where(dr =>

                dr.LeavingHour.Equals(time) && dr.DestinationStreet.Equals(destination)).ToListAsync();//compare  time and destination

            return drive;
        }



        //PutPermanentRideDLAsync
        public async Task<DriverRequest> PutDriverRequestDLAsync(int id,DriverRequest d)
        {

            var x = await volunteersContext.DriverRequest.FindAsync(id);
            if (x == null)
                return null;
            volunteersContext.Entry(x).CurrentValues.SetValues(d);
            await volunteersContext.SaveChangesAsync();
            return d;


        }


        //PostPermanentRideDLAsync
        public async Task<DriverRequest> PostDriverRequestDLAsync(DriverRequest d)
        {

            var x = await volunteersContext.DriverRequest.AddAsync(d);
            await volunteersContext.SaveChangesAsync();
            if (x != null)
                return d;
            return null;

        }
    }
}

