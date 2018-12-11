using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.MobCAT.MVVM.Abstractions
{
    public interface INavigationService
    {
        /// <summary>
        /// Registers the view models contained within the specified assembly.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> containing the view models to be registered.</param>
        void RegisterViewModels(Assembly assembly);

        /// <summary>
        /// Register the specified viewModelType and viewType.
        /// </summary>
        /// <param name="viewModelType">View model type.</param>
        /// <param name="viewType">View type.</param>
        void Register(Type viewModelType, Type viewType);

        /// <summary>
        /// Pops the current view from the navigation stack.
        /// </summary>
        /// <returns>The Task.</returns>
        Task PopAsync();

        /// <summary>
        /// Pops the current modal view from the navigation stack.
        /// </summary>
        /// <returns>The Task.</returns>
        Task PopModalAsync();

        /// <summary>
        /// Pops all but the root page of the navigation stack.
        /// </summary>
        /// <returns>The Task.</returns>
        /// <param name="animate">If set to <c>true</c> animate.</param>
        Task PopToRootAsync(bool animate);

        /// <summary>
        /// Pushs the specified view model onto the navigation stack.
        /// </summary>
        /// <returns>The Task.</returns>
        /// <param name="viewModel">View model.</param>
        /// <param name="discardCurrent">If set to <c>true</c> discard current page at the top of the navigation stack.</param>
        Task PushAsync(BaseNavigationViewModel viewModel, bool discardCurrent = false);

        /// <summary>
        /// Pushs the specified view model onto the navigation stack.
        /// </summary>
        /// <returns>The Task.</returns>
        /// <param name="initialize">Action returning the view model to be pushed onto the navigation stack.</param>
        /// <typeparam name="T">The view model <see cref="Type"/> deriving from <see cref="BaseNavigationViewModel"/>.</typeparam>
        /// <param name="discardCurrent">If set to <c>true</c> discard current page at the top of the navigation stack.</param>
        Task PushAsync<T>(Action<T> initialize = null, bool discardCurrent = false) where T : BaseNavigationViewModel;

        /// <summary>
        /// Pushes the specified view model onto the navigation stack modally.
        /// </summary>
        /// <returns>The Task.</returns>
        /// <param name="initialize">Action returning the view model to be pushed onto the navigation stack.</param>
        /// <typeparam name="T">The view model <see cref="Type"/> deriving from <see cref="BaseNavigationViewModel"/>.</typeparam>
        Task PushModalAsync<T>(Action<T> initialize = null) where T : BaseNavigationViewModel;

        /// <summary>
        /// Pushes the specified view model onto the navigation stack modally.
        /// </summary>
        /// <returns>The Task.</returns>
        /// <param name="viewModel">The view model <see cref="Type"/> deriving from <see cref="BaseNavigationViewModel"/>.</param>
        Task PushModalAsync(BaseNavigationViewModel viewModel);

        /// <summary>
        /// Switches the detail page based on the <see cref="BaseNavigationViewModel"/> specified.
        /// </summary>
        /// <param name="initialize">Action returning the view model to be switched in.k.</param>
        /// <typeparam name="T">The view model <see cref="Type"/> deriving from <see cref="BaseNavigationViewModel"/>.</typeparam>
        void SwitchDetailPage<T>(Action<T> initialize = null) where T : BaseNavigationViewModel;

        /// <summary>
        /// Switches the detail page based on the <see cref="BaseNavigationViewModel"/> specified.
        /// </summary>
        /// <param name="viewModel">The view model <see cref="Type"/> deriving from <see cref="BaseNavigationViewModel"/>.</param>
        void SwitchDetailPage(BaseNavigationViewModel viewModel);
    }
}