using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.MobCAT.MVVM;
using News.Helpers;
using System.Linq;

namespace News.ViewModels
{
    /// <summary>
    /// Home view model.
    /// </summary>
    public class HomeViewModel : BaseNavigationViewModel
    {
        private CategoryNewsViewModel _selectedCategory;

        public CategoryNewsViewModel SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                RaiseAndUpdate(ref _selectedCategory, value);
            }
        }

        public ObservableCollection<CategoryNewsViewModel> Categories { get; } = new ObservableCollection<CategoryNewsViewModel>
        {
            new CategoryNewsViewModel("Top News \ud83d\udd25"),
            new CategoryNewsViewModel("Business \ud83d\udcbc", NewsAPI.Constants.Categories.Business),
            new CategoryNewsViewModel("Entertainment \ud83d\udc83\ud83d\udd7a", NewsAPI.Constants.Categories.Entertainment),
            new CategoryNewsViewModel("Health \ud83e\uddec", NewsAPI.Constants.Categories.Health),
            new CategoryNewsViewModel("Science \ud83d\udd2c", NewsAPI.Constants.Categories.Science),
            new CategoryNewsViewModel("Sports \ud83e\udd3c", NewsAPI.Constants.Categories.Sports),
            new CategoryNewsViewModel("Technology \ud83d\udc69‍\ud83d\udcbb" ,NewsAPI.Constants.Categories.Technology),
        };

        public HomeViewModel()
        {
        }

        public override Task InitAsync()
        {
            SelectedCategory = Categories.FirstOrDefault();
            // TODO: init a VM only on category activation
            foreach (var item in Categories)
            {
                item.InitAsync().HandleResult();
            }

            return base.InitAsync();
        }
    }
}