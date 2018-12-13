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
        protected override Task<IEnumerable<Article>> FetchArticlesAsync()
        {
            return Task.FromResult((IEnumerable<Article>)new Article[0]);
        }
    }
}