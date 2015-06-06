using System.IO;
using System.Text;

namespace FileManagement
{
    /// <summary>
    /// Represents an object that can serialize of deserialize objects to a stream.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="stream">The stream to which the object will be serialized.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="obj">The object to serialize.</param>
        void Serialize<T>(Stream stream, Encoding encoding, T obj);

        /// <summary>
        /// Deserializes an object.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize</typeparam>
        /// <param name="stream">The stream from which the object will be deserialized.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <returns>The deserialized object.</returns>
        T Deserialize<T>(Stream stream, Encoding encoding);
    }
}
