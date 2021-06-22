using System;

namespace AmazonAsinTracker.Domain
{
    public record ProductReview
    {
        public ProductReview(string title)
        {
            Title = title;
        }

        public string Title { get; }

        public string Content { get; }

        public DateTime ReviewDate { get; }
    }
}