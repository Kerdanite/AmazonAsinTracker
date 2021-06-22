using System;
using System.Collections.Generic;
using System.Linq;

namespace AmazonAsinTracker.Domain
{
    public class AmazonReviewParser
    {
        private readonly string _amazonContent;
        private IList<ProductReview> _reviews;

        public AmazonReviewParser(string amazonContent)
        {
            _amazonContent = amazonContent;
            _reviews = new List<ProductReview>();
            ParseAmazonResponse();
        }

        private void ParseAmazonResponse()
        {
            var lines = _amazonContent.Split(Environment.NewLine).Where(w => w.Contains("data-hook=\\\"review\\\""));
            foreach (var line in lines)
            {
                ParseLineContent(line);
            }
        }

        private void ParseLineContent(string line)
        {
            string title = FindReviewTitle(line);

            _reviews.Add(new ProductReview(title));
        }

        private string FindReviewTitle(string line)
        {
            var titleIndex = line.IndexOf("review-title-content");
            var startReviewIndex = line.IndexOf("<span>", titleIndex);
            var endTitleIndex = line.IndexOf("</span>", startReviewIndex);
            var spanLength = 6;

            var title = line.Substring(startReviewIndex +spanLength, endTitleIndex - startReviewIndex - spanLength).Trim();
            return title;
        }

        public IEnumerable<ProductReview> GetReviews()
        {
            return _reviews;
        }
    }
}