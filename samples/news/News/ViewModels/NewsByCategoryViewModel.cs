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
        private int _selectedCategoryPosition;
        private bool _isSelectNextCategoryTipEnabled;
        private bool _isSelectNextCategoryTipNotRequired;

        public CategoryNewsViewModel SelectedCategory
        {
            get { return _selectedCategory; }
            set { RaiseAndUpdate(ref _selectedCategory, value); }
        }

        public int SelectedCategoryPosition
        {
            get { return _selectedCategoryPosition; }
            set
            {
                if (RaiseAndUpdate(ref _selectedCategoryPosition, value))
                {
                    OnSelectedCategoryPositionChanged();
                }
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

            ShowSelectNextCategoryTipIfRequired().HandleResult();

            return base.InitAsync();
        }

        private void OnSelectedCategoryPositionChanged()
        {
            System.Diagnostics.Debug.WriteLine($"SelectedCategoryPosition changed to {SelectedCategoryPosition}");
            IsSelectNextCategoryTipEnabled = false;

            // TODO: save to preferences
            _isSelectNextCategoryTipNotRequired = true;
        }

        private async Task ShowSelectNextCategoryTipIfRequired()
        {
            if (_isSelectNextCategoryTipNotRequired)
                return;

            IsSelectNextCategoryTipEnabled = false;
            await Task.Delay(5000);

            // Verify again if a user took an action and changed a category
            if (_isSelectNextCategoryTipNotRequired)
                return;

            IsSelectNextCategoryTipEnabled = true;
        }

        public void OnSelectNextCategoryCommandExecuted()
        {
            var nextPosition = SelectedCategoryPosition + 1;
            if (nextPosition >= Categories.Count || nextPosition < 0)
                nextPosition = 0;

            SelectedCategoryPosition = nextPosition;
        }
    }
}