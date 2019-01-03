using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.MobCAT.MVVM;
using News.Helpers;

namespace News.ViewModels
{
    /// <summary>
    /// Base news view model which manages list of articles with lazy loading ability regardless of the data source.
    /// </summary>
    public abstract class BaseNewsViewModel : BaseNavigationViewModel
    {
        private bool _isRefreshing;
        private bool _initialized;

        public ObservableCollection<ArticleViewModel> Articles { get; } = new ObservableCollection<ArticleViewModel>();

        public bool IsEmpty => _initialized && Articles.IsNullOrEmpty();

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

        public BaseNewsViewModel()
        {
            RefreshCommand = new AsyncCommand(OnRefreshCommandExecutedAsync, () => !IsRefreshing);
        }

        public async override Task InitAsync()
        {
            await base.InitAsync();
            await InitNewsAsync();

            _initialized = true;
            Raise(nameof(IsEmpty));
        }

        public async virtual Task InitNewsAsync(bool forceRefresh = false)
        {
            if (!Articles.IsNullOrEmpty() && !forceRefresh)
                return;

            var articles = await FetchArticlesAsync();
            if (articles != null)
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    Articles.Clear();
                    foreach (var article in articles)
                    {
                        Articles.Add(article);
                    }
                    Raise(nameof(IsEmpty));
                });
            }
        }

        protected abstract Task<IEnumerable<ArticleViewModel>> FetchArticlesAsync();

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
    }
}