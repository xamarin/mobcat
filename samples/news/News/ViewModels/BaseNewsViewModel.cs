using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.MobCAT.MVVM;
using NewsAPI.Models;

namespace News.ViewModels
{
    /// <summary>
    /// Base news view model which manages list of articles with lazy loading ability regardless of the data source.
    /// </summary>
    public abstract class BaseNewsViewModel : BaseNavigationViewModel
    {
        public ObservableCollection<Article> Articles { get; } = new ObservableCollection<Article>();

        public BaseNewsViewModel()
        {
        }

        public async override Task InitAsync()
        {
            await base.InitAsync();
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
                });
            }
        }

        protected abstract Task<IEnumerable<Article>> FetchArticlesAsync();
    }
}