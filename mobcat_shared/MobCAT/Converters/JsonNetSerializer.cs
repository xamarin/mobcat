using System;
using Newtonsoft.Json;

namespace Microsoft.MobCAT.Converters
{
    public class JsonNetSerializer : ISerializer<string>
    {
        private readonly JsonSerializerSettings _settings;

        public JsonNetSerializer(JsonSerializerSettings settings = null)
        {
            _settings = settings ?? new JsonSerializerSettings();
        }

        public string MediaType => throw new NotImplementedException();

        /// <inheritdoc />
        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, _settings);
        }

        /// <inheritdoc />
        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }
    }
}
