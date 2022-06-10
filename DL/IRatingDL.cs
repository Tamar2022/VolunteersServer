using Entity;
using System.Threading.Tasks;

namespace DL
{
    public interface IRatingDL
    {
        Task<Rating> PostRatingDLAsync(Rating rating);
    }
}