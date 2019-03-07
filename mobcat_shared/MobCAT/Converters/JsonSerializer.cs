using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace Microsoft.MobCAT.Converters
{
    /// <summary>
    /// System.Runtime.Serialization.Json string serializer.
    /// </summary>
    public class JsonSerializer : ISerializer<string>
    {
        public string MediaType => "application/json";

        /// <inheritdoc />
        public T Deserialize<T>(string value)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(memoryStream);
            }
        }

        /// <inheritdoc />
        public string Serialize<T>(T value)
        {
            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(memoryStream, value);
                var json = memoryStream.ToArray();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
        }
    }
}