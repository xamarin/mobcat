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
            if (string.IsNullOrWhiteSpace(SearchTerm))
                return new List<ArticleViewModel>();

            System.Diagnostics.Debug.WriteLine($"{GetType().Name} FetchArticlesAsync for [{SearchTerm}] Search Term");
            var articles = await NewsDataService.FetchArticlesBySearchQuery(SearchTerm);
            var result = articles.Select(a => new ArticleViewModel(a)).ToList();
            return result;
        }
    }
}