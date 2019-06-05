using System;
using Microsoft.MobCAT.Forms.Services.Abstractions;
using Microsoft.MobCAT.MVVM;
using MobCAT.MVVM;
using MobCATShell.Forms.Views;

namespace MobCATShell.Forms.ViewModels
{
    public class DetailsPageViewModel : BaseShellViewModel
    {
        public AsyncCommand DismissCommand => new AsyncCommand(Dismiss);

        private string _title;
        public string Title
        {
            get => _title;
            set => RaiseAndUpdate(ref _title, value);
        }

        private bool _isDismissButtonVisible;
        public bool IsDismissButtonVisible
        {
            get => _isDismissButtonVisible;
            set => RaiseAndUpdate(ref _isDismissButtonVisible, value);
        }
    }
}
