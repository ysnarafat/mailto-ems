using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Core
{
    public interface IFileStorage
    {
        Task<string> StoreFileAsync(string uploadsFolderPath, IFormFile file);
    }
}