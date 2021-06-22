using System.Threading.Tasks;

namespace AmazonAsinTracker.Domain
{
    public interface IAmazonProductReader
    {
        Task<string> TrackAmazonReviewForAsinCodeMoreRecentReviewOnPage(string asinCode, int pageToRead);
    }
}