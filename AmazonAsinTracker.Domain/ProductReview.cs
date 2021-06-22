using System;

namespace AmazonAsinTracker.Domain
{
    public record ProductReview
    {
        public ProductReview(string title, DateTime reviewDate, int score, string asinCode)
        {
            Title = title;
            ReviewDate = reviewDate;
            Score = score;
            AsinCode = asinCode;
        }

        public string Title { get; }

        public int Score { get; }

        public DateTime ReviewDate { get; }
        public string AsinCode { get; }
    }
}