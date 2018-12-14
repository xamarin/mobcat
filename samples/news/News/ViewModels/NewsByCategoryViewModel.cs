using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.MobCAT.MVVM;
using News.Helpers;
using NewsAPI.Models;

namespace News.ViewModels
{
    /// <summary>
    /// News by category view model.
    /// </summary>
    public class NewsByCategoryViewModel : BaseNavigationViewModel
    {
        private CategoryNewsViewModel _selectedCategory;
        private bool _isSelectNextCategoryTipEnabled;

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

        public bool IsSelectNextCategoryTipEnabled
        {
            get { return _isSelectNextCategoryTipEnabled; }
            set { RaiseAndUpdate(ref _isSelectNextCategoryTipEnabled, value); }
        }

        public Command SelectNextCategoryCommand { get; }

        public NewsByCategoryViewModel()
        {
            SelectNextCategoryCommand = new Command(OnSelectNextCategoryCommandExecuted);
        }

        public override Task InitAsync()
        {
            SelectedCategory = Categories.FirstOrDefault();
            // TODO: init a VM only on category activation
            foreach (var item in Categories)
            {
                item.InitAsync().HandleResult();
            }

            OnEnableChangeCategoryTipWithDelay().HandleResult();

            return base.InitAsync();
        }

        private async Task OnEnableChangeCategoryTipWithDelay()
        {
            // TODO: ensure we have only one instance of the tip awaiter
            IsSelectNextCategoryTipEnabled = false;
            await Task.Delay(5000);
            IsSelectNextCategoryTipEnabled = true;
            await Task.Delay(3000);
            IsSelectNextCategoryTipEnabled = false;
        }

        public void OnSelectNextCategoryCommandExecuted()
        {

        }
    }
}