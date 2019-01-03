using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.MobCAT.MVVM;
using News.Helpers;

namespace News.ViewModels
{
    /// <summary>
    /// News by source view model.
    /// </summary>
    public class NewsBySourceViewModel : BaseNavigationViewModel
    {
        private int _selectedSourcePosition;
        private bool _isRefreshing;

        public SourceNewsViewModel SelectedSource => Sources.Count > _selectedSourcePosition ? Sources[_selectedSourcePosition] : null;

        public int SelectedSourcePosition
        {
            get { return _selectedSourcePosition; }
            set
            {
                if (RaiseAndUpdate(ref _selectedSourcePosition, value))
                {
                    Raise(nameof(SelectedSource));
                    Raise(nameof(IsRefreshing));
                    RefreshCommand.ChangeCanExecute();
                    OnSelectedCategoryPositionChanged();
                }
            }
        }

        // TODO: Load full list via API and add lazy initialization
        public ObservableCollection<SourceNewsViewModel> Sources { get; } = new ObservableCollection<SourceNewsViewModel>()
        {
            new SourceNewsViewModel("The New York Times", "the-new-york-times"),
            new SourceNewsViewModel("CNN", "cnn"),
            new SourceNewsViewModel("ABC News", "abc-news"),
            new SourceNewsViewModel("The Washington Post", "the-washington-post"),
            new SourceNewsViewModel("Fox News", "fox-news"),
            new SourceNewsViewModel("CBS News", "cbs-news"),
            new SourceNewsViewModel("NBC News", "nbc-news"),
            new SourceNewsViewModel("Reuters", "reuters"),
            new SourceNewsViewModel("USA Today", "usa-today"),
            new SourceNewsViewModel("Bloomberg", "bloomberg"),
            new SourceNewsViewModel("Wired", "wired"),
            new SourceNewsViewModel("CNBC", "cnbc"),
        };

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

        public NewsBySourceViewModel()
        {
            RefreshCommand = new AsyncCommand(OnRefreshCommandExecutedAsync, () => !IsRefreshing);
        }

        public async override Task InitAsync()
        {
            await SelectedSource.InitNewsAsync(false);
            await base.InitAsync();
        }

        private void OnSelectedCategoryPositionChanged()
        {
            System.Diagnostics.Debug.WriteLine($"SelectedCategoryPosition changed to {SelectedSourcePosition}");
            SelectedSource.InitNewsAsync(false).HandleResult();
        }

        private async Task OnRefreshCommandExecutedAsync()
        {
            try
            {
                IsRefreshing = true;
                await SelectedSource.InitNewsAsync(true);
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}