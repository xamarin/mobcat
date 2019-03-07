namespace Microsoft.MobCat.Services
{
    public interface IFileSystemService
    {
        /// <summary>
        /// Gets the document storage service.
        /// </summary>
        /// <value>The document storage service.</value>
        IFileStorageService DocumentStorage { get; }

        /// <summary>
        /// Gets the settings storage service.
        /// </summary>
        /// <value>The settings storage service.</value>
        IFileStorageService SettingsStorage { get; }

        /// <summary>
        /// Gets the temp storage service.
        /// </summary>
        /// <value>The temp storage service.</value>
        IFileStorageService TempStorage { get; }

        /// <summary>
        /// Gets the name for a temp file.
        /// </summary>
        /// <returns>The name for a temp file.</returns>
        string GetTempFileName();
    }
}