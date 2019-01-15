using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using News.Services.Abstractions;
using News.Services;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Xamarin.Forms;
using News.Models;
using News.Helpers;

#if !DEBUG
[assembly: Dependency(typeof(NewsDataService))]
#endif
namespace News.Services
{
    public class NewsDataService : INewsDataService
    {
        public async Task<FetchArticlesResponse> FetchArticlesByCategory(Categories? category = null, int pageNumber = 1, int pageSize = Constants.DefaultArticlesPageSize)
        {
            var request = new TopHeadlinesRequest
            {
                Country = Countries.US,
                Language = Languages.EN,
                Category = category,
                Page = pageNumber,
                PageSize = pageSize,
            };

            // TODO: replace with custom implementation of the data provider
            var newsApiClient = new NewsApiClient(ServiceConfig.NEWSSERVICEAPIKEY);
            var articles = await newsApiClient.GetTopHeadlinesAsync(request);
            var result = new FetchArticlesResponse(pageNumber, pageSize);
            if (articles?.Articles != null)
            {
                result.Articles = articles.Articles;
                result.TotalCount = articles.TotalResults;
            }

            return result;
        }

        public async Task<FetchArticlesResponse> FetchArticlesBySource(string source, int pageNumber = 1, int pageSize = Constants.DefaultArticlesPageSize)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentNullException(nameof(source));

            var request = new TopHeadlinesRequest
            {
                Sources = new List<string> { source },
                Language = Languages.EN,
                Page = pageNumber,
                PageSize = pageSize,
            };

            // TODO: replace with custom implementation of the data provider
            var newsApiClient = new NewsApiClient(ServiceConfig.NEWSSERVICEAPIKEY);
            var articles = await newsApiClient.GetTopHeadlinesAsync(request);
            var result = new FetchArticlesResponse(pageNumber, pageSize);
            if (articles?.Articles != null)
            {
                result.Articles = articles.Articles;
                result.TotalCount = articles.TotalResults;
            }

            return result;
        }

        public async Task<FetchArticlesResponse> FetchArticlesBySearchQuery(string query, int pageNumber = 1, int pageSize = Constants.DefaultArticlesPageSize)
        {
            var result = new FetchArticlesResponse(pageNumber, pageSize);
            if (string.IsNullOrWhiteSpace(query))
                return result;

            var request = new TopHeadlinesRequest
            {
                Q = query,
                Language = Languages.EN,
                Country = Countries.US,
                Page = pageNumber,
                PageSize = pageSize,
            };

            // TODO: replace with custom implementation of the data provider
            var newsApiClient = new NewsApiClient(ServiceConfig.NEWSSERVICEAPIKEY);
            var articles = await newsApiClient.GetTopHeadlinesAsync(request);
            if (articles?.Articles != null)
            {
                result.Articles = articles.Articles;
                result.TotalCount = articles.TotalResults;
            }

            return result;
        }
    }
}
