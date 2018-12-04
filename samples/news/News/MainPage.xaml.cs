using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using News.ViewModels;
using News.Helpers;

namespace News
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var newsViewModel = new NewsViewModel();
            newsViewModel.InitAsync().HandleResult();

            this.BindingContext = newsViewModel;
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) 
                return;

            ((ListView)sender).SelectedItem = null;
        }
    }
}
