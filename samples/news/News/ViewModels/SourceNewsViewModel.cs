using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;

namespace News.ViewModels
{
    public class SourceNewsViewModel : BaseNewsViewModel
    {
        public string Title { get; }

        public string Source { get; }

        public SourceNewsViewModel(string title, string source)
        {
            Title = title;
            Source = source;
        }

        protected async override Task<IEnumerable<ArticleViewModel>> FetchArticlesAsync()
        {
            System.Diagnostics.Debug.WriteLine($"{GetType().Name} FetchArticlesAsync for {Title} Source");

            var request = new TopHeadlinesRequest
            {
                Sources = new List<string> { Source },
                Language = Languages.EN,
            };

            // TODO: replace with custom implementation of the data provider
            var newsApiClient = new NewsApiClient(ServiceConfig.NEWSSERVICEAPIKEY);
            var articles = await newsApiClient.GetTopHeadlinesAsync(request);
            var result = (IEnumerable<ArticleViewModel>)articles?.Articles?
                .Select(a => new ArticleViewModel(a))
                .ToList();

            return result;
        }
    }
}