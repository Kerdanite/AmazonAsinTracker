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

        [TestMethod()]
        public async Task HandleTest_GetSecondReviewTitle()
        {
            var amazonReader = new AmazonProductReaderStub(_fileContent);
            var productTrackRepo = new ProductAsinRepositoryStub(new List<string>{"B082XY23D5"});
            var productReviewRepositoryStub = new ProductReviewRepositoryStub();
            var sut = new ProcessProductReviewCommandHandler(productTrackRepo, amazonReader, productReviewRepositoryStub);

            await sut.Handle(new ProcessProductReviewCommand(), CancellationToken.None);

            Assert.AreEqual("Great phone, in great condition! Amazon Warehouse items are the best savings!", productReviewRepositoryStub.Reviews.ElementAt(1).Title);
        }

        [TestMethod()]
        public async Task HandleTest_GetFirstReview_Date()
        {
            var amazonReader = new AmazonProductReaderStub(_fileContent);
            var productTrackRepo = new ProductAsinRepositoryStub(new List<string>{"B082XY23D5"});
            var productReviewRepositoryStub = new ProductReviewRepositoryStub();
            var sut = new ProcessProductReviewCommandHandler(productTrackRepo, amazonReader, productReviewRepositoryStub);

            await sut.Handle(new ProcessProductReviewCommand(), CancellationToken.None);

            Assert.AreEqual(new DateTime(2021, 6, 2), productReviewRepositoryStub.Reviews.First().ReviewDate);
        }

        [TestMethod()]
        public async Task HandleTest_GetSecondReview_Date()
        {
            var amazonReader = new AmazonProductReaderStub(_fileContent);
            var productTrackRepo = new ProductAsinRepositoryStub(new List<string>{"B082XY23D5"});
            var productReviewRepositoryStub = new ProductReviewRepositoryStub();
            var sut = new ProcessProductReviewCommandHandler(productTrackRepo, amazonReader, productReviewRepositoryStub);

            await sut.Handle(new ProcessProductReviewCommand(), CancellationToken.None);

            Assert.AreEqual(new DateTime(2021, 5, 25), productReviewRepositoryStub.Reviews.ElementAt(1).ReviewDate);
        }

        [TestMethod()]
        public async Task HandleTest_GetFirstReview_Score()
        {
            var amazonReader = new AmazonProductReaderStub(_fileContent);
            var productTrackRepo = new ProductAsinRepositoryStub(new List<string>{"B082XY23D5"});
            var productReviewRepositoryStub = new ProductReviewRepositoryStub();
            var sut = new ProcessProductReviewCommandHandler(productTrackRepo, amazonReader, productReviewRepositoryStub);

            await sut.Handle(new ProcessProductReviewCommand(), CancellationToken.None);

            Assert.AreEqual(5, productReviewRepositoryStub.Reviews.First().Score);
        }


        [TestMethod()]
        public async Task HandleTest_GetThirdReview_Score()
        {
            var amazonReader = new AmazonProductReaderStub(_fileContent);
            var productTrackRepo = new ProductAsinRepositoryStub(new List<string>{"B082XY23D5"});
            var productReviewRepositoryStub = new ProductReviewRepositoryStub();
            var sut = new ProcessProductReviewCommandHandler(productTrackRepo, amazonReader, productReviewRepositoryStub);

            await sut.Handle(new ProcessProductReviewCommand(), CancellationToken.None);

            Assert.AreEqual(2, productReviewRepositoryStub.Reviews.ElementAt(2).Score);
        }

        [TestMethod()]
        public async Task HandleTest_ReviewIsOnCorrectAsin()
        {
            string asin = "REVIEWTESTASIN";
            var amazonReader = new AmazonProductReaderStub(_fileContent);
            var productTrackRepo = new ProductAsinRepositoryStub(new List<string>{asin});
            var productReviewRepositoryStub = new ProductReviewRepositoryStub();
            var sut = new ProcessProductReviewCommandHandler(productTrackRepo, amazonReader, productReviewRepositoryStub);

            await sut.Handle(new ProcessProductReviewCommand(), CancellationToken.None);

            Assert.AreEqual(asin, productReviewRepositoryStub.Reviews.ElementAt(2).AsinCode);
        }

        [TestMethod()]
        public async Task HandleTest_GetNinthReview_TitleOnOtherLanguage()
        {
            var amazonReader = new AmazonProductReaderStub(_fileContent);
            var productTrackRepo = new ProductAsinRepositoryStub(new List<string>{"B082XY23D5"});
            var productReviewRepositoryStub = new ProductReviewRepositoryStub();
            var sut = new ProcessProductReviewCommandHandler(productTrackRepo, amazonReader, productReviewRepositoryStub);

            await sut.Handle(new ProcessProductReviewCommand(), CancellationToken.None);

            Assert.AreEqual("Muy buen cell", productReviewRepositoryStub.Reviews.ElementAt(8).Title);
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

        public Task AppendReview(IEnumerable<ProductReview> reviews)
        {
            Reviews = reviews;
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ProductReview>> GetLastReviewsForProductByAsinCode(string requestAsinCode)
        {
            throw new NotImplementedException();
        }
    }
}