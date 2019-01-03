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
        private bool _isRefreshing;

        // TODO: move to the category view model (base)
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                if (RaiseAndUpdate(ref _isRefreshing, value))
                {
                    RefreshCommand.ChangeCanExecute();
                }
            }
        }

        public AsyncCommand RefreshCommand { get; }

        public FavoritesViewModel()
        {
            RefreshCommand = new AsyncCommand(OnRefreshCommandExecutedAsync, () => !IsRefreshing);
        }

        private async Task OnRefreshCommandExecutedAsync()
        {
            try
            {
                IsRefreshing = true;
                await InitNewsAsync(true);
            }
            finally
            {
                IsRefreshing = false;
            }
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