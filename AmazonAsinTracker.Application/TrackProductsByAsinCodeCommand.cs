using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AmazonAsinTracker.Application
{
    public class TrackProductsByAsinCodeCommand : IRequest
    {
        public IEnumerable<string> ProductAsins { get; set; }
    }

    public class TrackProductsByAsinCodeCommandHandler : IRequestHandler<TrackProductsByAsinCodeCommand>
    {
        public async Task<Unit> Handle(TrackProductsByAsinCodeCommand request, CancellationToken cancellationToken)
        {

            return Unit.Value;
        }
    }
}