using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAPI.Models;

namespace News.ViewModels
{
    /// <summary>
    /// Favorite news view model.
    /// </summary>
    public class FavoritesViewModel : BaseNewsViewModel
    {
        protected override Task<IEnumerable<ArticleViewModel>> FetchArticlesAsync()
        {
            return Task.FromResult((IEnumerable<ArticleViewModel>)new ArticleViewModel[0]);
        }
    }
}