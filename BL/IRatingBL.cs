using Entity;
using System.Threading.Tasks;

namespace BL
{
    public interface IRatingBL
    {
        Task<Rating> PostRatingBLAsync(Rating rating);
    }
}