using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AmazonAsinTracker.Domain;
using MediatR;

namespace AmazonAsinTracker.Application
{
    public class GetLastProductReviewByAsinCodeQuery : IRequest<IEnumerable<ProductReview>>
    {
        public string AsinCode { get; }

        public GetLastProductReviewByAsinCodeQuery(string asinCode)
        {
            AsinCode = asinCode;
        }
    }

    public class GetLastProductReviewByAsinCodeQueryHandler : IRequestHandler<GetLastProductReviewByAsinCodeQuery, IEnumerable<ProductReview>>
    {
        private readonly IProductReviewRepository _productReviewRepository;

        public GetLastProductReviewByAsinCodeQueryHandler(IProductReviewRepository productReviewRepository)
        {
            _productReviewRepository = productReviewRepository;
        }

        public async Task<IEnumerable<ProductReview>> Handle(GetLastProductReviewByAsinCodeQuery request, CancellationToken cancellationToken)
        {
            return await _productReviewRepository.GetLastReviewsForProductByAsinCode(request.AsinCode);
        }
    }
}