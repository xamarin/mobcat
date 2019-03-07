using System.IO;

namespace Microsoft.MobCAT.Converters
{
    /// <summary>
    /// Serializer constract to and from streams
    /// </summary>
    public interface IStreamSerializer
    {
        /// <summary>
        /// Write the specified value to the stream.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">Value to write.</param>
        /// <typeparam name="T">The type of value to write to stream.</typeparam>
        void Write<T>(Stream stream, T value);

        /// <summary>
        /// Read the specified value from stream.
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="value">Stream to read.</param>
        /// <typeparam name="T">The type to read the stream to.</typeparam>
        T Read<T>(Stream value);

        string MediaType { get; }
    }
}