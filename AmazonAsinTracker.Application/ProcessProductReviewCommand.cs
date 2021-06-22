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
        private readonly IAmazonProductReader _amazonProductReader;

        public ProcessProductReviewCommandHandler(IProductAsinRepository productAsinRepository, IAmazonProductReader amazonProductReader)
        {
            _productAsinRepository = productAsinRepository;
            _amazonProductReader = amazonProductReader;
        }

        public async Task<Unit> Handle(ProcessProductReviewCommand request, CancellationToken cancellationToken)
        {
            int pageToRead = 1;
            var productToTrack = await _productAsinRepository.GetProductAsinToTrack(cancellationToken);

            foreach (var asinCode in productToTrack)
            {
                string amazonContent = await _amazonProductReader.TrackAmazonReviewForAsinCodeMoreRecentReviewOnPage(asinCode, pageToRead);
            }

            return Unit.Value;
        }
    }
}