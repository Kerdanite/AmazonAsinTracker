using System.Collections.Generic;
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
        private readonly IProductReviewRepository _productReviewRepository;

        public ProcessProductReviewCommandHandler(IProductAsinRepository productAsinRepository,
            IAmazonProductReader amazonProductReader, IProductReviewRepository productReviewRepository)
        {
            _productAsinRepository = productAsinRepository;
            _amazonProductReader = amazonProductReader;
            _productReviewRepository = productReviewRepository;
        }

        public async Task<Unit> Handle(ProcessProductReviewCommand request, CancellationToken cancellationToken)
        {
            int pageToRead = 1;
            var productToTrack = await _productAsinRepository.GetProductAsinToTrack(cancellationToken);
            var reviews = new List<ProductReview>();
            foreach (var asinCode in productToTrack)
            {
                string amazonContent = await _amazonProductReader.TrackAmazonReviewForAsinCodeMoreRecentReviewOnPage(asinCode, pageToRead);
                var amazonReviewParser = new AmazonReviewParser(amazonContent, asinCode);
                reviews.AddRange(amazonReviewParser.GetReviews());
            }

            await _productReviewRepository.AppendReview(reviews);

            return Unit.Value;
        }
    }
}