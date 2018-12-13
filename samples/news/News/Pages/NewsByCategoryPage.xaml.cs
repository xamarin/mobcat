using System;
using Microsoft.MobCAT.Forms.Pages;
using News.ViewModels;
using NewsAPI.Models;
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

            if (selectedArticle != null)
            {
                // TODO: move to view model as a command
                // TODO: parse and validate url
                var validUri = new Uri(selectedArticle.UrlToArticle);
                Device.OpenUri(validUri);
            }
        }
    }
}
