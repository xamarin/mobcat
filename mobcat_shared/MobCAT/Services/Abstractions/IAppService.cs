namespace Microsoft.MobCAT.Services
{
    /// <summary>
    /// Application state enum, mapped to UIApplicationState
    /// </summary>
    public enum ApplicationState : long
    {
        Active,
        Inactive,
        Background
    }

    public interface IAppService
    {
        /// <summary>
        /// Gets the user-visible display name of the application visible on the home screen.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets the short name of the application. (Bundle name)  
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets the build version of the application.  (Bundle version e.g. 3.1.2a1 )
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the release version of the application.  (BundleShortVersionString e.g. 3.1.2 )
        /// </summary>
        string ShortVersion { get; }

        /// <summary>
        /// Gets current application state.  Can only be called on main thread
        /// </summary>
        /// <value>The state of the application.</value>
        ApplicationState CurrentState { get; }
    }
}