using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using News.Services.Abstractions;
using News.Services;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Xamarin.Forms;

#if !DEBUG
[assembly: Dependency(typeof(NewsDataService))]
#endif
namespace News.Services
{
    public class NewsDataService : INewsDataService
    {
        public async Task<IEnumerable<Article>> FetchArticlesByCategory(Categories? category = null)
        {
            var request = new TopHeadlinesRequest
            {
                Country = Countries.US,
                Language = Languages.EN,
                Category = category,
            };

            // TODO: replace with custom implementation of the data provider
            var newsApiClient = new NewsApiClient(ServiceConfig.NEWSSERVICEAPIKEY);
            var articles = await newsApiClient.GetTopHeadlinesAsync(request);
            var result = articles?.Articles ?? new List<Article>();
            return result;
        }

        public async Task<IEnumerable<Article>> FetchArticlesBySource(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentNullException(nameof(source));

            var request = new TopHeadlinesRequest
            {
                Sources = new List<string> { source },
                Language = Languages.EN,
            };

            // TODO: replace with custom implementation of the data provider
            var newsApiClient = new NewsApiClient(ServiceConfig.NEWSSERVICEAPIKEY);
            var articles = await newsApiClient.GetTopHeadlinesAsync(request);
            var result = articles?.Articles ?? new List<Article>();
            return result;
        }

        public async Task<IEnumerable<Article>> FetchArticlesBySearchQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Article>();

            var request = new TopHeadlinesRequest
            {
                Q = query,
                Language = Languages.EN,
                Country = Countries.US,
            };

            // TODO: replace with custom implementation of the data provider
            var newsApiClient = new NewsApiClient(ServiceConfig.NEWSSERVICEAPIKEY);
            var articles = await newsApiClient.GetTopHeadlinesAsync(request);
            var result = articles?.Articles ?? new List<Article>();
            return result;
        }
    }
}
