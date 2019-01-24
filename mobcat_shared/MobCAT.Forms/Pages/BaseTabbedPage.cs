using System;
using System.Threading.Tasks;
using Microsoft.MobCAT.Forms.Services.Abstractions;
using Microsoft.MobCAT.MVVM;
using Xamarin.Forms;

namespace Microsoft.MobCAT.Forms.Pages
{
    public class BaseTabbedPage<T> : TabbedPage, IViewFor<T> where T : BaseNavigationViewModel, new()
    {
        private T _viewModel;

        public T ViewModel
        {
            get
            {
                return _viewModel;
            }
            set
            {
                _viewModel = value;

                BindingContext = _viewModel;

                Task.Run(async () =>
                {
                    try
                    {
                        await Init();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex.Message);
                    }
                });
            }
        }

        protected BaseTabbedPage()
        {
            if (DesignMode.IsDesignModeEnabled) //create a viewmodel for design-time data
            {
                ViewModel = Activator.CreateInstance(typeof(T)) as T;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            if (CurrentPage is IViewFor viewForPage 
                && viewForPage.ViewModel is BaseNavigationViewModel viewModelForPage)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        await viewModelForPage.InitAsync();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex.Message);
                    }
                });
            }
        }

        object IViewFor.ViewModel
        {
            get
            {
                return _viewModel;
            }

            set
            {
                ViewModel = (T)value;
            }
        }

        private async Task Init()
        {
            try
            {
                await ViewModel.InitAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}