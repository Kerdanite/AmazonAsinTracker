using System.Collections.Generic;

namespace AmazonAsinTracker.Domain
{
    public interface IProductReviewRepository
    {
        void AppendReview(IEnumerable<ProductReview> reviews);
    }
}