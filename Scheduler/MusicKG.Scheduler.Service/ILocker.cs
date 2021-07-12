using System.Threading;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Service
{
    public interface ILocker
    {
        Task<bool> TryLockAsync(string name, CancellationToken cancellationToken = default);

        Task ReleaseAsync(string name, CancellationToken cancellationToken = default);
    }
}
