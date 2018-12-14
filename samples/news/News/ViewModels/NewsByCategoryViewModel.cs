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
        private int _selectedCategoryPosition;
        private bool _isSelectNextCategoryTipEnabled;
        private bool _isSelectNextCategoryTipNotRequired;
        private bool _isRefreshing;

        public CategoryNewsViewModel SelectedCategory => Categories.Count > _selectedCategoryPosition ? Categories[_selectedCategoryPosition] : null;

        public int SelectedCategoryPosition
        {
            get { return _selectedCategoryPosition; }
            set
            {
                if (RaiseAndUpdate(ref _selectedCategoryPosition, value))
                {
                    Raise(nameof(SelectedCategory));
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

        public Command SelectNextCategoryCommand { get; }
        public AsyncCommand RefreshCommand { get; }

        public NewsByCategoryViewModel()
        {
            SelectNextCategoryCommand = new Command(OnSelectNextCategoryCommandExecuted);
            RefreshCommand = new AsyncCommand(OnRefreshCommandExecutedAsync, ()=> !IsRefreshing);
        }

        public override Task InitAsync()
        {
            // TODO: init a VM only on category activation
            foreach (var item in Categories)
            {
                item.InitAsync().HandleResult();
            }

            ShowSelectNextCategoryTipIfRequiredAsync().HandleResult();

            return base.InitAsync();
        }

        private void OnSelectedCategoryPositionChanged()
        {
            System.Diagnostics.Debug.WriteLine($"SelectedCategoryPosition changed to {SelectedCategoryPosition}");
            IsSelectNextCategoryTipEnabled = false;

            // TODO: save to preferences
            _isSelectNextCategoryTipNotRequired = true;
        }

        private async Task ShowSelectNextCategoryTipIfRequiredAsync()
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

        private void OnSelectNextCategoryCommandExecuted()
        {
            var nextPosition = SelectedCategoryPosition + 1;
            if (nextPosition >= Categories.Count || nextPosition < 0)
                nextPosition = 0;

            SelectedCategoryPosition = nextPosition;
        }

        private async Task OnRefreshCommandExecutedAsync()
        {
            try
            {
                IsRefreshing = true;
                await SelectedCategory.InitAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}