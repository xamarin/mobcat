using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.MobCAT.MVVM;
using News.Helpers;
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


        public FavoritesViewModel()
        {
           
        }

     

        protected override Task<IEnumerable<ArticleViewModel>> FetchArticlesAsync()
        {
            var favoritesService = DependencyService.Resolve<IFavoritesService>();
            var favorites = favoritesService.Get()
                .Select(f => new ArticleViewModel(f))
                .ToList();

            return Task.FromResult<IEnumerable<ArticleViewModel>>(favorites);
        }
    }
}