using System;
using System.IO;
using Newtonsoft.Json;

namespace Microsoft.MobCAT.Converters
{
    public class JsonNetStreamSerializer : IStreamSerializer
    {
        Newtonsoft.Json.JsonSerializer _serializer;

        public JsonNetStreamSerializer()
        {
            _serializer = new Newtonsoft.Json.JsonSerializer();
        }

        public string MediaType => throw new NotImplementedException();

        /// <inheritdoc />
        public T Read<T>(Stream value)
        {
            if (value.Length == 0) return default(T);

            using (var streamReader = new StreamReader(value))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return _serializer.Deserialize<T>(jsonReader);
            }
        }

        /// <inheritdoc />
        public void Write<T>(Stream stream, T value)
        {
            using (var streamWriter = new StreamWriter(stream))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                _serializer.Serialize(jsonWriter, value);
            }
        }
    }
}
