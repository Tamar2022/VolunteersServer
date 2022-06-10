using Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL
{
  public interface IDriverRequestDL
    {
        Task<List<DriverRequest>> GetDriverRequestDLAsync(int DriveId);
        Task<List<DriverRequest>> GetDriverRequestByDayDLAsync(DateTime time, string destination);        Task<DriverRequest> PostDriverRequestDLAsync(DriverRequest d);
        Task<DriverRequest> PutDriverRequestDLAsync(int id,DriverRequest d);
        
    }
}