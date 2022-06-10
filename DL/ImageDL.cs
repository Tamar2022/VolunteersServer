using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
   public class ImageDL
    {
        VolunteersContext volunteersContext;

        public ImageDL(VolunteersContext volunteersContext)
        {
            this.volunteersContext = volunteersContext;
        }
        //public async Task<bool> ValidateDuplicateFileNameDLAsync(int userId, string fileName)
        //{


        //    return await volunteersContext.Driver.AnyAsync(x => x.UserId == userId && x.DriverLicense == fileName);


        //}

       
    }
}
