using Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public interface IDriverRequestBL
    {
        Task<List<DriverRequest>> GetDriverRequestBL(int DriveId);
        Task<DriverRequest> PostDriverRequestBL(DriverRequest d);
        Task<DriverRequest> PutDriverRequestBL(int id,DriverRequest d);
    }
}