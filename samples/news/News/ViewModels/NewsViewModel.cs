using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.MobCAT.MVVM;
using NewsAPI.Models;
using NewsAPI;
using NewsAPI.Constants;
using News;

namespace News.ViewModels
{
    public class NewsViewModel : BaseViewModel
    {
        public ObservableCollection<Article> Articles { get; } = new ObservableCollection<Article>();

        public NewsViewModel()
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