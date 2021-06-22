using Microsoft.VisualStudio.TestTools.UnitTesting;
using AmazonAsinTracker.Application;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AmazonAsinTracker.Domain;

namespace AmazonAsinTracker.Application.Tests
{
    [TestClass()]
    public class ProcessProductReviewCommandHandlerTests
    {
        private static string _filePath;
        private static string _fileContent;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            if (File.Exists("Data/AmazonProductReview.txt"))
            {
                _filePath = "Data/AmazonProductReview.txt";
                _fileContent = File.ReadAllText(_filePath);
            }
        }

        [TestMethod()]
        public async Task HandleTest_GetFirstReviewTitle()
        {
            var amazonReader = new AmazonProductReaderStub(_fileContent);
            var productTrackRepo = new ProductAsinRepositoryStub(new List<string>{"B082XY23D5"});
            var productReviewRepositoryStub = new ProductReviewRepositoryStub();
            var sut = new ProcessProductReviewCommandHandler(productTrackRepo, amazonReader, productReviewRepositoryStub);

            await sut.Handle(new ProcessProductReviewCommand(), CancellationToken.None);

            Assert.AreEqual("Great phone", productReviewRepositoryStub.Reviews.First().Title);
        }
    }

    public class AmazonProductReaderStub : IAmazonProductReader
    {
        private readonly string _amazonContent;

        public AmazonProductReaderStub(string amazonContent)
        {
            _amazonContent = amazonContent;
        }
        public Task<string> TrackAmazonReviewForAsinCodeMoreRecentReviewOnPage(string asinCode, int pageToRead)
        {
            return Task.FromResult(_amazonContent);
        }
    }
    
    public class ProductAsinRepositoryStub : IProductAsinRepository
    {
        private readonly IEnumerable<string> _asinToTrack;

        public ProductAsinRepositoryStub(IEnumerable<string> asinToTrack)
        {
            _asinToTrack = asinToTrack;
        }

        public Task TrackProductsByAsinCodeAsync(IEnumerable<string> requestProductAsins, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetProductAsinToTrack(CancellationToken cancellationToken)
        {
            return Task.FromResult(_asinToTrack);
        }
    }

    public class ProductReviewRepositoryStub : IProductReviewRepository
    {
        public IEnumerable<ProductReview> Reviews { get; private set; }

        public void AppendReview(IEnumerable<ProductReview> reviews)
        {
            Reviews = reviews;
        }
    }
}