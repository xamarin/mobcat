using Microsoft.MobCAT.MVVM;

namespace Microsoft.MobCAT.Forms.Services.Abstractions
{
    public interface IViewFor
    {
        object ViewModel { get; set; }
    }

    public interface IViewFor<T> : IViewFor where T : BaseNavigationViewModel
    {
        new T ViewModel { get; set; }
    }
}