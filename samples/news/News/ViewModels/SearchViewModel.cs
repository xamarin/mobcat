using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;

namespace News.ViewModels
{
    /// <summary>
    /// Search view model to handle search results.
    /// </summary>
    public class SearchViewModel : BaseNewsViewModel
    {
        private string _searchTerm = "trump";

        public string SearchTerm
        {
            get { return _searchTerm; }
            set { RaiseAndUpdate(ref _searchTerm, value); }
        }

        public SearchViewModel()
        {
        }

        protected async override Task<IEnumerable<ArticleViewModel>> FetchArticlesAsync()
        {
            System.Diagnostics.Debug.WriteLine($"{GetType().Name} FetchArticlesAsync for [{SearchTerm}] Search Term");

            if (string.IsNullOrWhiteSpace(SearchTerm))
                return new List<ArticleViewModel>();

            var request = new TopHeadlinesRequest
            {
                Q = SearchTerm,
                Language = Languages.EN,
                Country = Countries.US,
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