using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.MobCAT.MVVM;
using NewsAPI.Models;
using NewsAPI;
using NewsAPI.Constants;

namespace News.ViewModels
{
    /// <summary>
    /// Base news view model which manages list of articles with lazy loading ability regardless of the data source.
    /// </summary>
    public class BaseNewsViewModel : BaseNavigationViewModel
    {
        public ObservableCollection<Article> Articles { get; } = new ObservableCollection<Article>();

        public BaseNewsViewModel()
        {
        }

        public async override Task InitAsync()
        {
            // init with your API key
            var newsApiClient = new NewsApiClient(ServiceConfig.NEWSSERVICEAPIKEY);
            var articles = await newsApiClient.GetTopHeadlinesAsync(new TopHeadlinesRequest
            {
                //Q = "Apple",
                Country = Countries.US,
                //SortBy = SortBys.Popularity,
                Language = Languages.EN,
                //From = DateTime.UtcNow.AddDays(-1),
            });

            if (articles?.Articles?.Count > 0)
            {

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    Articles.Clear();
                    articles.Articles.ForEach(Articles.Add);
                });
            }

            await base.InitAsync();
        }
    }
}