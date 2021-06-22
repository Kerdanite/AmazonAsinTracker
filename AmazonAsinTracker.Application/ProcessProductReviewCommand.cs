using System.Threading;
using System.Threading.Tasks;
using AmazonAsinTracker.Domain;
using MediatR;

namespace AmazonAsinTracker.Application
{
    public class ProcessProductReviewCommand : IRequest
    {
        
    }

    public class ProcessProductReviewCommandHandler : IRequestHandler<ProcessProductReviewCommand>
    {
        private readonly IProductAsinRepository _productAsinRepository;

        public ProcessProductReviewCommandHandler(IProductAsinRepository productAsinRepository)
        {
            _productAsinRepository = productAsinRepository;
        }

        public async Task<Unit> Handle(ProcessProductReviewCommand request, CancellationToken cancellationToken)
        {
            var productToTrack = await _productAsinRepository.GetProductAsinToTrack(cancellationToken);

            return Unit.Value;
        }
    }
}