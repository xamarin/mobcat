using System.IO;

namespace Microsoft.MobCat.Services
{
    public enum OutputStreamMode
    {
        OverwriteOrCreate,
        OpenOrCreate,
        Append
    }

    public interface IFileStorageService
    {
        /// <summary>
        /// Indicates whether the specified file exists.
        /// </summary>
        /// <returns><c>true</c>, if exists was returned, <c>false</c> otherwise.</returns>
        /// <param name="path">Path.</param>
        bool FileExists(string path);

        /// <summary>
        /// Deletes the file at the specified path.
        /// </summary>
        /// <returns><c>true</c>, if file was deleted, <c>false</c> otherwise.</returns>
        /// <param name="path">Path.</param>
        bool DeleteFile(string path);

        /// <summary>
        /// Gets the size for a file at the requested path.
        /// </summary>
        /// <returns>The size of the file.</returns>
        /// <param name="path">Path for the file.</param>
        long FileSize(string path);

        /// <summary>
        /// Creates the output stream for the file at the specified path.
        /// </summary>
        /// <returns>The output stream for the requested file.</returns>
        /// <param name="path">Path to the file.</param>
        /// <param name="mode">Mode for the output stream.</param>
        Stream CreateOutputStream(string path, OutputStreamMode mode = OutputStreamMode.OverwriteOrCreate);

        /// <summary>
        /// Creates the input stream for the requested file.
        /// </summary>
        /// <returns>The input stream for the requested file.</returns>
        /// <param name="path">Path.</param>
        Stream CreateInputStream(string path);

        /// <summary>
        /// Gets the base path for the storage service.
        /// </summary>
        /// <value>The base path.</value>
        string BasePath { get; }
    }
}