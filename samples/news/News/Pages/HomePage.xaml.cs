using Microsoft.MobCAT.Forms.Pages;
using Microsoft.MobCAT.Forms.Services.Abstractions;
using Microsoft.MobCAT.MVVM;
using News.Helpers;
using News.ViewModels;
using Xamarin.Forms;

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
