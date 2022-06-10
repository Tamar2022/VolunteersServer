using DL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ImageBL : IImageBL
    {
        IDriverDL driverDL;
        public ImageBL(IDriverDL driverDL)
        {        
            this.driverDL = driverDL;
        }

        public async Task<string> GetImagesForUser(int userId)
        {
            return await driverDL.GetUserImages(userId);
        }
    }
}

