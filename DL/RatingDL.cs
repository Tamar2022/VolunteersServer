using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class RatingDL : IRatingDL
    {
        VolunteersContext volunteersContext;
        public RatingDL(VolunteersContext volunteersContext)
        {
            this.volunteersContext = volunteersContext;
        }
        public async Task<Rating> PostRatingDLAsync(Rating rating)
        {
            var rrating = await volunteersContext.Rating.AddAsync(rating);
            await volunteersContext.SaveChangesAsync();
            if (rrating == null)
                throw new Exception("can`t add");
            return rating;//מה העניין להחזיר אותו???

        }
    }
}
