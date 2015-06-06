using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace FileManagement.Json
{
    /// <summary>
    /// Serializes and deserializes objects in JSON format.
    /// </summary>
    public class JsonSerializer : ISerializer
    {
        private JsonSerializerSettings settings;

        /// <summary>
        /// Creates a new <see cref="JsonSerializer"/> instance.
        /// </summary>
        public JsonSerializer()
            : this(null)
        { }

        /// <summary>
        /// Creates a new <see cref="JsonSerializer"/> instance.
        /// </summary>
        /// <param name="settings">The JSON serialization settings to use.</param>
        public JsonSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        #region ISerializer Members

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="stream">The stream to which the object will be serialized.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="obj">The object to serialize.</param>
        public void Serialize<T>(Stream stream, Encoding encoding, T obj)
        {
            using (var writer = new StreamWriter(stream, encoding, 1024, true))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault(settings);
                serializer.Serialize(jsonWriter, obj);
            }
        }

        /// <summary>
        /// Deserializes an object.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize</typeparam>
        /// <param name="stream">The stream from which the object will be deserialized.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <returns>The deserialized object.</returns>
        public T Deserialize<T>(Stream stream, Encoding encoding)
        {
            using (var reader = new StreamReader(stream, encoding, false, 1024, true))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault(settings);
                return serializer.Deserialize<T>(jsonReader);
            }
        }

        #endregion
    }
}
