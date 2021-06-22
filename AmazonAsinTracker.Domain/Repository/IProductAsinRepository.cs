using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmazonAsinTracker.Domain
{
    public interface IProductAsinRepository
    {
        Task TrackProductsByAsinCodeAsync(IEnumerable<string> requestProductAsins, CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetProductAsinToTrack(CancellationToken cancellationToken);
    }
}