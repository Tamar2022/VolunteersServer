using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BL
{
    public interface IImageBL
    {
        
        Task<string> GetImagesForUser(int userId);
    }
}