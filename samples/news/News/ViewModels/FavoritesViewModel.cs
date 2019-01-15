using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.MobCAT.MVVM;
using News.Helpers;
using News.Services.Abstractions;
using NewsAPI.Models;
using Xamarin.Forms;
using News.Models;

namespace News.ViewModels
{
    /// <summary>
    /// Favorite news view model.
    /// </summary>
    public class FavoritesViewModel : BaseNewsViewModel
    {
        public FavoritesViewModel()
        {

        }

        protected override Task<FetchArticlesResult> FetchArticlesAsync(int pageNumber = 1, int pageSize = Constants.DefaultArticlesPageSize)
        {
            var result = new FetchArticlesResult(pageNumber, pageSize);
            var favoritesService = DependencyService.Resolve<IFavoritesService>();
            var favorites = favoritesService.Get()
                .Select(f => new ArticleViewModel(f))
                .ToList();

            if (favorites != null)
            {
                result.Articles = favorites;
                result.TotalCount = favorites.Count;
            };

            return Task.FromResult(result);
        }
    }
}