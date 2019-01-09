using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using News.Services.Abstractions;
using News.Services.FakeNews;
using NewsAPI.Constants;
using NewsAPI.Models;
using Xamarin.Forms;
using News.Helpers;
using Newtonsoft.Json;

#if DEBUG
[assembly: Dependency(typeof(FakeNewsDataService))]
#endif
namespace News.Services.FakeNews
{
    public class FakeNewsDataService : INewsDataService
    {
        public FakeNewsDataService()
        {
        }

        public async Task<IEnumerable<Article>> FetchArticlesByCategory(Categories? category = null)
        {
            // Simulate network request
            await Task.Delay(500);
            var result = new List<Article>();
            var resourceName = $"{GetType().Namespace}.category.{category?.ToString().ToLower() ?? "all"}.json";
            var prestoredResponseContent = await resourceName.ReadResourceContent();
            if (!string.IsNullOrWhiteSpace(prestoredResponseContent))
            {
                var articles = JsonConvert.DeserializeObject<ArticlesResult>(prestoredResponseContent);
                result = articles?.Articles ?? new List<Article>();
            }

            return result;
        }

        public async Task<IEnumerable<Article>> FetchArticlesBySource(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentNullException(nameof(source));

            // Simulate network request
            await Task.Delay(500);
            var result = new List<Article>();
            var resourceName = $"{GetType().Namespace}.source.{source}.json";
            var prestoredResponseContent = await resourceName.ReadResourceContent();
            if (!string.IsNullOrWhiteSpace(prestoredResponseContent))
            {
                var articles = JsonConvert.DeserializeObject<ArticlesResult>(prestoredResponseContent);
                result = articles?.Articles ?? new List<Article>();
            }

            return result;
        }

        public Task<IEnumerable<Article>> FetchArticlesBySearchQuery(string query)
        {
            // TODO: implement
            return new NewsDataService().FetchArticlesBySearchQuery(query);
        }
    }
}
