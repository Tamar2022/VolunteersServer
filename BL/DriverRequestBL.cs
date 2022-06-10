using DL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class DriverRequestBL : IDriverRequestBL
    {
        IDriverRequestDL driverRequestDL;
        public DriverRequestBL(IDriverRequestDL permanentRideDL)
        {
            this.driverRequestDL = permanentRideDL;

        }
        //get
        public async Task<List<DriverRequest>> GetDriverRequestBL(int DriveId)
        {
            return await driverRequestDL.GetDriverRequestDLAsync(DriveId);
        }
       
        //Put
        public async Task<DriverRequest> PutDriverRequestBL(int id,DriverRequest d)
        {
            return await driverRequestDL.PutDriverRequestDLAsync(id,d);
        }

        //post
        public async Task<DriverRequest> PostDriverRequestBL(DriverRequest d)
        {
            return await driverRequestDL.PostDriverRequestDLAsync(d);
        }

    }
}
