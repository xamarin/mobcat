namespace Microsoft.MobCAT.Converters
{
     /// <summary>
    /// Serializer constract to and from type T
    /// </summary>
    public interface ISerializer<F>
    {
        /// <summary>
        /// Serialize the specified value to type F.
        /// </summary>
        /// <returns>The serialized value.</returns>
        /// <param name="value">Value to serialize.</param>
        /// <typeparam name="T">The type of value to serialize.</typeparam>
        F Serialize<T>(T value);

        /// <summary>
        /// Deserialize the specified value.
        /// </summary>
        /// <returns>The deserialized value.</returns>
        /// <param name="value">Value to deserialize.</param>
        /// <typeparam name="T">The type of value to deserialize to.</typeparam>
        T Deserialize<T>(F value);

        string MediaType { get; }
    }
}