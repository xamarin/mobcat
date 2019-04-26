namespace Microsoft.MobCAT.Services
{
    public interface IFileSystemService
    {
        /// <summary>
        /// Gets the document storage folder path.
        /// </summary>
        /// <value>The document storage path.</value>
        string DocumentStorage { get; }

        /// <summary>
        /// Gets the settings storage.
        /// </summary>
        /// <value>The settings storage path.</value>
        string SettingsStorage { get; }

        /// <summary>
        /// Gets the temp storage path.
        /// </summary>
        /// <value>The temp storage path.</value>
        string TempStorage { get; }
    }
}