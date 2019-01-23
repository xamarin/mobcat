using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.MobCAT.Services;
using News.Helpers;
using News.Models;
using News.Services;
using News.Services.Abstractions;
using Xamarin.Forms;

#if !DEBUG
[assembly: Dependency(typeof(NewsDataService))]
#endif
namespace News.Services
{
    public class NewsDataService : BaseHttpService, INewsDataService
    {
        public NewsDataService() 
            : base(ServiceConfig.NEWSSERVICEURL, null)
        {
            Serializer = new NewtonsoftJsonSerializer();
        }

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

            var actionUrl = "/top-headlines?" +
                $"country={request.Country}&" +
                $"language={request.Language}&" +
                $"category={request.Category}&" +
                $"page={request.Page}&" +
                $"pageSize={request.PageSize}&" +
                $"apiKey={ServiceConfig.NEWSSERVICEAPIKEY}";

            var articles = await GetAsync<ArticlesResult>(actionUrl);
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

            var actionUrl = "/top-headlines?" +
               $"sources={string.Join(",", request.Sources)}&" +
               $"language={request.Language}&" +
               $"page={request.Page}&" +
               $"pageSize={request.PageSize}&" +
               $"apiKey={ServiceConfig.NEWSSERVICEAPIKEY}";

            var articles = await GetAsync<ArticlesResult>(actionUrl);
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
                Country = Countries.US,
                Language = Languages.EN,
                Page = pageNumber,
                PageSize = pageSize,
            };

            var actionUrl = "/top-headlines?" +
               $"q={request.Q}&" +
               $"country={request.Country}&" +
               $"language={request.Language}&" +
               $"page={request.Page}&" +
               $"pageSize={request.PageSize}&" +
               $"apiKey={ServiceConfig.NEWSSERVICEAPIKEY}";

            var articles = await GetAsync<ArticlesResult>(actionUrl);
            if (articles?.Articles != null)
            {
                result.Articles = articles.Articles;
                result.TotalCount = articles.TotalResults;
            }

            return result;
        }
    }
}
