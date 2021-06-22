using System;
using System.Collections.Generic;
using System.Linq;

namespace AmazonAsinTracker.Domain
{
    public class AmazonReviewParser
    {
        private readonly string _amazonContent;
        private readonly string _asinCode;
        private IList<ProductReview> _reviews;

        public AmazonReviewParser(string amazonContent, string asinCode)
        {
            _amazonContent = amazonContent;
            _asinCode = asinCode;
            _reviews = new List<ProductReview>();
            ParseAmazonResponse();
        }

        private void ParseAmazonResponse()
        {
            var lines = _amazonContent.Split(new[] { "\r\n", "\r", "\n" },      StringSplitOptions.None).Where(w => w.Contains("data-hook=\\\"review\\\""));
            foreach (var line in lines)
            {
                ParseLineContent(line);
            }
        }

        private void ParseLineContent(string line)
        {
            string title = FindReviewTitle(line);
            DateTime reviewDate = FindReviewDate(line);
            int score = FindScore(line);

            _reviews.Add(new ProductReview(title, reviewDate, score, _asinCode));
        }

        private int FindScore(string line)
        {
            var scoreIndex = line.IndexOf("review-rating");
            var startScoreSearch = "<span class=\\\"a-icon-alt\\\">";
            var startIndexDate = line.IndexOf(startScoreSearch, scoreIndex);
            var endIndexDate = line.IndexOf("</span>", startIndexDate);
            var spanLength = 3;

            var scoreString = line.Substring(startIndexDate +startScoreSearch.Length, 1).Trim();

            return int.Parse(scoreString);
        }

        private DateTime FindReviewDate(string line)
        {
            var dateIndex = line.IndexOf("review-date");
            var startIndexDate = line.IndexOf("on ", dateIndex);
            var endIndexDate = line.IndexOf("</span>", startIndexDate);
            var spanLength = 3;

            var dateString = line.Substring(startIndexDate +spanLength, endIndexDate - startIndexDate - spanLength).Trim();

            return DateTime.Parse(dateString);
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