using System;
using System.Threading.Tasks;

namespace Microsoft.MobCAT.MVVM.Abstractions
{
    public interface IRouteNavigationService : INavigationService
    {
        Task GoToRouteAsync(string route);

        Task GoToRouteAsync(Uri route);
    }
}
