using Microsoft.MobCAT.Forms.Pages;
using News.ViewModels;
using Xamarin.Forms;

namespace News.Pages
{
    public partial class NewsByCategoryPage : BaseContentPage<NewsByCategoryViewModel>
    {
        public NewsByCategoryPage()
        {
            InitializeComponent();
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var selectedArticle = (ArticleViewModel)e.SelectedItem;
            ((ListView)sender).SelectedItem = null;
        }
    }
}
