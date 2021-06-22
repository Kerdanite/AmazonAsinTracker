using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AmazonAsinTracker.Domain;

namespace AmazonAsinTracker.Infrastructure
{
    public class AmazonProductReader : IAmazonProductReader
    {
        private readonly HttpClient _amazonClient;
        public AmazonProductReader()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };   
            _amazonClient = new HttpClient(handler);
            _amazonClient.BaseAddress = new Uri("https://www.amazon.com/hz/reviews-render/ajax/reviews/get/");
        }
        public async Task<string> TrackAmazonReviewForAsinCodeMoreRecentReviewOnPage(string asinCode, int pageToRead)
        {

            var parameters = BuildQueryParameters(asinCode, pageToRead);
            string requestedUri = "ref=cm_cr_arp_d_viewopt_srt";
            using (var response = await _amazonClient.PostAsync(requestedUri, new FormUrlEncodedContent(parameters)))
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }

        private static Dictionary<string, string> BuildQueryParameters(string asinCode, int pageToRead)
        {
            var parameters = new Dictionary<string, string>();
            parameters["asin"] = asinCode;
            parameters["sortBy"] = "recent";
            parameters["pageSize"] = "10";
            //parameters["reftag"] = "cm_cr_arp_d_viewopt_srt";
            parameters["pageNumber"] = pageToRead.ToString();
            return parameters;
        }
    }
}