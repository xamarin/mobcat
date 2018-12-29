using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using News.Services.Abstractions;
using NewsAPI.Models;
using Xamarin.Forms;

namespace News.ViewModels
{
    /// <summary>
    /// Favorite news view model.
    /// </summary>
    public class FavoritesViewModel : BaseNewsViewModel
    {
        protected async override Task<IEnumerable<ArticleViewModel>> FetchArticlesAsync()
        {
            var favoritesService = DependencyService.Resolve<IFavoritesService>();
            var favorites = favoritesService.Get()
                .Select(f => new ArticleViewModel(f))
                .ToList();

            return favorites;
        }
    }
}