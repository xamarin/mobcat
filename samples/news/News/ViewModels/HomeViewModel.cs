using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.MobCAT.MVVM;
using News.Helpers;

namespace News.ViewModels
{
    /// <summary>
    /// Home view model.
    /// </summary>
    public class HomeViewModel : BaseNavigationViewModel
    {
        public ObservableCollection<CategoryNewsViewModel> Categories { get; } = new ObservableCollection<CategoryNewsViewModel>
        {
            new CategoryNewsViewModel("Top News"),
            new CategoryNewsViewModel("Business", NewsAPI.Constants.Categories.Business),
            new CategoryNewsViewModel("Entertainment",NewsAPI.Constants.Categories.Entertainment),
            new CategoryNewsViewModel("Health",NewsAPI.Constants.Categories.Health),
            new CategoryNewsViewModel("Science",NewsAPI.Constants.Categories.Science),
            new CategoryNewsViewModel("Sports",NewsAPI.Constants.Categories.Sports),
            new CategoryNewsViewModel("Technology",NewsAPI.Constants.Categories.Technology),
        };

        public HomeViewModel()
        {
        }

        public override Task InitAsync()
        {
            // TODO: init a VM only on category activation
            foreach (var item in Categories)
            {
                item.InitAsync().HandleResult();
            }

            return base.InitAsync();
        }
    }
}