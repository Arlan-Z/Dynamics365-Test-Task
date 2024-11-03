using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace AwaraIT.Training.Application.Core
{
    public class JsonSerializer
    {
        public static string Serialize<T>(T entity, DataContractJsonSerializerSettings settings = null)
        {
            using (var stream = new MemoryStream())
            using (var streamReader = new StreamReader(stream))
            {
                var serializerSettings = settings
                                         ?? new DataContractJsonSerializerSettings
                                         {
                                             UseSimpleDictionaryFormat = true,
                                             KnownTypes = new[] { typeof(string[]), typeof(List<object>) }
                                         };

                var serializer = new DataContractJsonSerializer(typeof(T), serializerSettings);
                serializer.WriteObject(stream, entity);
                stream.Position = 0;
                return streamReader.ReadToEnd();
            }
        }

        public static T Deserialize<T>(string json, DataContractJsonSerializerSettings settings = null)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)json;
            }

            using (var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializerSettings = settings ?? new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true };

                var serializer = new DataContractJsonSerializer(typeof(T), serializerSettings);
                return (T)serializer.ReadObject(memoryStream);
            }
        }
    }
}
