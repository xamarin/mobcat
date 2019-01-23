using Microsoft.MobCAT.Forms.Pages;
using News.Helpers;
using News.ViewModels;

namespace News.Pages
{
    public partial class HomePage : BaseTabbedPage<HomeViewModel>
    {
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            // TODO: implement with the based tabbed page
            if (CurrentPage is FavoritesPage favoritesPage)
            {
                favoritesPage.ViewModel.InitAsync().HandleResult();
            }
        }
    }
}
