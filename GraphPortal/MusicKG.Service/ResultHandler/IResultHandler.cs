using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MusicKG.Service.ResultHandler
{
    public interface IResultHandler
    {
        Task HandleResultAsync(IConfiguration configuraiton, byte[] resultContent);
    }
}
