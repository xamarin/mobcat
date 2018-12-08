using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using News.ViewModels;
using News.Helpers;
using Microsoft.MobCAT.Forms.Pages;

namespace News.Pages
{
    public partial class LoadingPage : BaseContentPage<LoadingViewModel>
    {
        public LoadingPage()
        {
            InitializeComponent();
            //var newsViewModel = new LoadingViewModel();
            //newsViewModel.InitAsync().HandleResult();
            //var initViewModel = ViewModel;
            //this.BindingContext = newsViewModel;
        }


        //private void OnItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    if (e == null) 
        //        return;

        //    ((ListView)sender).SelectedItem = null;
        //}
    }
}
