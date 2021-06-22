using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmazonAsinTracker.Domain
{
    public interface IProductReviewRepository
    {
        Task AppendReview(IEnumerable<ProductReview> reviews);
        Task<IEnumerable<ProductReview>> GetLastReviewsForProductByAsinCode(string requestAsinCode);
    }
}