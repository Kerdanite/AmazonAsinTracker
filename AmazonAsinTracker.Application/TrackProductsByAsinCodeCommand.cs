using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AmazonAsinTracker.Domain;
using MediatR;

namespace AmazonAsinTracker.Application
{
    public class TrackProductsByAsinCodeCommand : IRequest
    {
        public IEnumerable<string> ProductAsins { get; set; }
    }

    public class TrackProductsByAsinCodeCommandHandler : IRequestHandler<TrackProductsByAsinCodeCommand>
    {
        private readonly IProductAsinRepository _productAsinRepository;

        public TrackProductsByAsinCodeCommandHandler(IProductAsinRepository productAsinRepository)
        {
            _productAsinRepository = productAsinRepository;
        }

        public async Task<Unit> Handle(TrackProductsByAsinCodeCommand request, CancellationToken cancellationToken)
        {
            await _productAsinRepository.TrackProductsByAsinCodeAsync(request.ProductAsins, cancellationToken);
            return Unit.Value;
        }
    }
}