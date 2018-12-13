using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAPI.Models;

namespace News.ViewModels
{
    /// <summary>
    /// Search view model to handle search results.
    /// </summary>
    public class SearchViewModel : BaseNewsViewModel
    {
        protected override Task<IEnumerable<ArticleViewModel>> FetchArticlesAsync()
        {
            return Task.FromResult((IEnumerable<ArticleViewModel>)new ArticleViewModel[0]);
        }
    }
}