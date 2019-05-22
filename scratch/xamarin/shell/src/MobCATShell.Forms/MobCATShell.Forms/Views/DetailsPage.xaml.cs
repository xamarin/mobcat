using System;
using System.Collections.Generic;
using Microsoft.MobCAT.Forms.Pages;
using MobCATShell.Forms.ViewModels;
using Xamarin.Forms;

namespace MobCATShell.Forms.Views
{
    [QueryProperty(nameof(RoutedTitle), nameof(RoutedTitle))]
    [QueryProperty(nameof(RoutedDismissButtonVisibility), nameof(RoutedDismissButtonVisibility))]
    public partial class DetailsPage : BaseContentPage<DetailsPageVM>
    {
        public DetailsPage()
        {
            InitializeComponent();
        }

        public string RoutedTitle
        {
            set
            {
                if (ViewModel != null)
                {
                    ViewModel.Title = Uri.UnescapeDataString(value);
                }
            }
        }

        public string RoutedDismissButtonVisibility
        {
            set
            {
                if (ViewModel != null)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return;
                    }
                    bool.TryParse(Uri.UnescapeDataString(value), out bool isDismissButtonVisible);
                    ViewModel.IsDismissButtonVisible = isDismissButtonVisible;
                }
            }
        }
    }
}