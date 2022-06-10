using DL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class RatingBL : IRatingBL
    {
        IRatingDL raRatingDL;
        public RatingBL(IRatingDL raRatingDL)
        {
            this.raRatingDL = raRatingDL;
        }
        public async Task<Rating> PostRatingBLAsync(Rating rating)
        {
            return await raRatingDL.PostRatingDLAsync(rating);

        }
    }
}
