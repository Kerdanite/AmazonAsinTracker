using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AmazonAsinTracker.Domain;
using AmazonAsinTracker.Infrastructure.FileStorage;

namespace AmazonAsinTracker.Infrastructure
{
    public class ProductReviewFileRepository : IProductReviewRepository
    {
        private readonly IFileStorageProvider _fileStorageProvider;
        private readonly string _producReviewsFile;
        private string fileSeparator = ";ùù;";

        public ProductReviewFileRepository(IFileStorageProvider fileStorageProvider)
        {
            _fileStorageProvider = fileStorageProvider;
            _producReviewsFile = _fileStorageProvider.GetFolderLocation() + "/productReviews.txt";
        }

        public async Task AppendReview(IEnumerable<ProductReview> reviews)
        {
            if (File.Exists(_producReviewsFile))
            {
                File.Delete(_producReviewsFile);
            }

            using (StreamWriter sw = File.CreateText(_producReviewsFile))
            {
                foreach (var productReview in reviews)
                {
                    await sw.WriteLineAsync($"{productReview.Title}{fileSeparator}{productReview.ReviewDate}{fileSeparator}{productReview.Score}{fileSeparator}{productReview.AsinCode}");
                }
            }
        }

        public async Task<IEnumerable<ProductReview>> GetLastReviewsForProductByAsinCode(string requestAsinCode)
        {
            if (!File.Exists(_producReviewsFile))
            {
                return new List<ProductReview>();
            }

            using (StreamReader sr = File.OpenText(_producReviewsFile))
            {
                string content = (await sr.ReadToEndAsync()).Trim();
                var lines = content.Split(Environment.NewLine);
                var reviews = new List<ProductReview>();
                foreach (var line in lines)
                {
                    var lineInfo = line.Split(fileSeparator);
                    reviews.Add(new ProductReview(lineInfo[0], DateTime.Parse(lineInfo[1]), int.Parse(lineInfo[2]), lineInfo[3]));
                }

                return reviews;
            }
        }
    }
}