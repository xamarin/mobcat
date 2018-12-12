using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAPI.Models;

namespace News.ViewModels
{
    /// <summary>
    /// Favorite news view model.
    /// </summary>
    public class FavoriteNewsViewModel : BaseNewsViewModel
    {
        protected override Task<IEnumerable<Article>> FetchArticlesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}