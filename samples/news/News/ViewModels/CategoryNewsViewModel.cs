using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;

namespace News.ViewModels
{
    /// <summary>
    /// Category news view model.
    /// </summary>
    public class CategoryNewsViewModel : BaseNewsViewModel
    {
        public string Title { get; }

        public Categories? Category { get; }

        public CategoryNewsViewModel(string title, Categories? category = null)
        {
            Title = title;
            Category = category;
        }

        protected async override Task<IEnumerable<ArticleViewModel>> FetchArticlesAsync()
        {
            // init with your API key
            var request = new TopHeadlinesRequest
            {
                //Q = "Apple",
                Country = Countries.US,
                //SortBy = SortBys.Popularity,
                Language = Languages.EN,
                Category = Category,
                //From = DateTime.UtcNow.AddDays(-1),
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