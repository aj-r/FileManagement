using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace FileManagement
{
    /// <summary>
    /// Serializes and deserializes objects as binary data.
    /// </summary>
    public class BinaryFileSerializer : ISerializer
    {
        private readonly IFormatter serializer;

        /// <summary>
        /// Creates a new <see cref="BinaryFileSerializer"/> instance.
        /// </summary>
        public BinaryFileSerializer()
            : this(new BinaryFormatter())
        { }

        /// <summary>
        /// Creates a new <see cref="BinaryFileSerializer"/> instance.
        /// </summary>
        /// <param name="formatter">The IFormatter implementation to use.</param>
        public BinaryFileSerializer(IFormatter formatter)
        {
            if (formatter == null)
                throw new ArgumentNullException("formatter");
            serializer = formatter;
        }

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="stream">The stream to which the object will be serialized.</param>
        /// <param name="encoding">Not used for binary serialization.</param>
        /// <param name="obj">The object to serialize.</param>
        public void Serialize<T>(Stream stream, Encoding encoding, T obj)
        {
            serializer.Serialize(stream, obj);
        }

        /// <summary>
        /// Deserializes an object.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize</typeparam>
        /// <param name="stream">The stream from which the object will be deserialized.</param>
        /// <param name="encoding">Not used for binary serialization.</param>
        /// <returns>The deserialized object.</returns>
        public T Deserialize<T>(Stream stream, Encoding encoding)
        {
            // For now just deserialize the file.
            // In the future we may need to check the version first to prevent serialization errors.
            return (T)serializer.Deserialize(stream);
        }
    }
}
